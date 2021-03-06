﻿namespace Hink

open Fable.Import.Browser
open Fable.Import.PIXI
open Hink.Inputs
open System

module Core =

  [<AbstractClass>]
  type Widget () =
    let mutable x = 0.
    let mutable y = 0.
    let mutable width = 100.
    let mutable height = 32.

    member val UI = Container() with get, set

    abstract CreateUI: unit -> unit

    default self.CreateUI () = ()

    // General helpers
    member self.Position pos =
      self.UI.position <- pos

    member self.X
      with get () = x
      and set (value) =
        x <- value
        self.CreateUI()

    member self.Y
      with get () = y
      and set (value) =
        y <- value
        self.CreateUI()

    member self.Width
      with get () = width
      and set (value) =
        width <- value
        self.CreateUI()

    member self.Height
      with get () = height
      and set (value) =
        height <- value
        self.CreateUI()

  and Application () =
    member val Renderer = Unchecked.defaultof<WebGLRenderer> with get, set
    member val Canvas = Unchecked.defaultof<HTMLCanvasElement> with get, set
    member val StartDate : DateTime = DateTime.Now with get, set
    member val LastTickDate = 0. with get, set
    member val DeltaTime = 0. with get, set
    member val Widgets : Widget list = [] with get, set
    member val RootContainer = Container() with get

    member self.Init () =
      let options =
        [ BackgroundColor (float 0xFFFFFF)
          Resolution 1.
          Antialias true
        ]
      // Init the renderer
      self.Renderer <- WebGLRenderer(window.innerWidth, window.innerHeight, options)
      // Init the canvas
      self.Canvas <- self.Renderer.view
      self.Canvas.setAttribute("tabindex", "1")
      self.Canvas.id <- "editor"

      self.Canvas.addEventListener_click(fun ev ->
        self.Canvas.focus()
        null
      )

      document.body
        .appendChild(self.Canvas) |> ignore

      Mouse.init self.Canvas
      Keyboard.init self.Canvas true

      self.Canvas.focus()

    member self.Start() =
      self.StartDate <- DateTime.Now
      self.RequestUpdate()

    member self.RequestUpdate() =
      window.requestAnimationFrame(fun dt -> self.Update(dt)) |> ignore

    member self.Update(dt: float) =
      self.Renderer.render(self.RootContainer)
      self.RequestUpdate()

    member self.AddWidget(widget: Widget) =
      self.RootContainer.addChild(widget.UI)
      |> ignore

module Theme =

  type TextTheme =
    {
      FontFamilly: string
      FontSize: float
    }

  type ButtonTheme =
    {
      Height: float
      Width: float
      BackgroundColor: int
      HoverColor: int
      ClickColor: int
    }

  type StateSwitchTheme =
    {
      BackgroundColor: int
      TextStyle: TextStyle list
      CircleColor: int
    }

  type SwitchTheme =
    {
      Height: float
      Width: float
      ActiveTheme: StateSwitchTheme
      InactiveTheme: StateSwitchTheme
      TextStyle: TextStyle list
      CircleRadius: float
      CirclePadding: float
    }

  module Default =
    open Fable.Core

    let TURQUOISE = 0x1ABC9C
    let GREEN_SEA = 0x16A085
    let EMERALD = 0x2ECC71
    let NEPHRITIS = 0x27AE60
    let PETER_RIVER = 0x3498DB
    let BELIZE_HOLE = 0x2980B9
    let AMETHYST = 0x9B59B6
    let WISTERIA = 0x8E44AD
    let WET_ASPHALT = 0x34495E
    let MIDNIGHT_BLUE = 0x2C3E50
    let SUN_FLOWER = 0xF1C40F
    let ORANGE = 0xF39C12
    let CARROT = 0xE67E22
    let PUMPKIN = 0xD35400
    let ALIZARIN = 0xE74C3C
    let POMEGRANATE = 0xC0392B
    let CLOUDS = 0xECF0F1
    let SILVER = 0xBDC3C7
    let CONCRETE = 0x95A5A6
    let ASBESTOS = 0x7F8C8D

    let TextStyle =
      [
        FontFamilly "Arial"
        FontSize 14.
      ]

    let Switch =
      {
        Height = 28.
        Width = 70.
        ActiveTheme =
          {
            BackgroundColor = WET_ASPHALT
            CircleColor = TURQUOISE
            TextStyle =
              [
                Fill (Case2 (float TURQUOISE))
                FontFamilly "Arial"
                FontSize 14.
              ]
          }
        InactiveTheme =
          {
            BackgroundColor = SILVER
            CircleColor = ASBESTOS
            TextStyle =
              [
                Fill (Case2 (float CLOUDS))
                FontFamilly "Arial"
                FontSize 14.
              ]
          }
        TextStyle = TextStyle
        CircleRadius = 10.
        CirclePadding = 5.
      }
