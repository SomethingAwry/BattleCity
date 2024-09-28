namespace BattleCity

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Data.Core
open Avalonia.Data.Core.Plugins
open Avalonia.Markup.Xaml
open BattleCity.Model

type App() =
    inherit Application()

    override this.Initialize() =
            AvaloniaXamlLoader.Load(this)

    override this.OnFrameworkInitializationCompleted() =

        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0)

        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktop ->
             desktop.MainWindow <- MainWindow(DataContext = MainWindowViewModel())
        | _ -> ()

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime) {
            var mainWindow = new MainWindow();

            var field = new GameField();
            var game = new Game(field);
            game.Start();
            mainWindow.DataContext = field;

            lifetime.MainWindow = mainWindow;

        base.OnFrameworkInitializationCompleted()
