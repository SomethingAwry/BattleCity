namespace BattleCity;

using Avalonia.Controls;
using Avalonia.Input;

/// Interaction logic for MainWindow.xaml
public partial class MainWindow : Window {

    public MainWindow() {
        InitializeComponent();
    }

    protected override void OnKeyDown(KeyEventArgs e) {
        Keyboard.Keys.Add(e.Key);
        base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e) {
        Keyboard.Keys.Remove(e.Key);
        base.OnKeyUp(e);
    }
}