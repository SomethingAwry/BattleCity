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
    member private this.InitializeComponent() =
#if DEBUG
        this.AttachDevTools()
#endif
        AvaloniaXamlLoader.Load(this)

    protected override void OnKeyDown(KeyEventArgs e) {
        Keyboard.Keys.Add(e.Key);
        base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e) {
        Keyboard.Keys.Remove(e.Key);
        base.OnKeyUp(e);
    }
}