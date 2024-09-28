namespace BattleCity

open System
open Avalonia

module Program =

    // Avalonia configuration, don't remove; also used by visual designer.
    [<CompiledName "BuildAvaloniaApp">] 
    let buildAvaloniaApp () = 
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace(areas = [||])

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [<EntryPoint; STAThread>]
    let main args =
        buildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args)
