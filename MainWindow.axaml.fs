namespace BattleCity

open Avalonia
open Avalonia.Controls
open Avalonia.Input
open Avalonia.Markup.Xaml

/// Interaction logic for MainWindow.xaml
type MainWindow () as this = 
    inherit Window ()

    do this.InitializeComponent()

    // As F# does not have partial classes, InitializeComponent is
    // not defined elsewhere to do the below; we do it here ourselves.
    member private me.InitializeComponent() =
#if DEBUG
        me.AttachDevTools()
#endif
        AvaloniaXamlLoader.Load(me)

    override _.OnKeyDown (e: KeyEventArgs) =
        Keyboard.Keys.Add(e.Key) |> ignore
        base.OnKeyDown(e)

    override _.OnKeyUp (e: KeyEventArgs) =
        Keyboard.Keys.Remove(e.Key) |> ignore
        base.OnKeyUp(e)
