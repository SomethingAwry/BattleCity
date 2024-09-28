namespace BattleCity.Infrastructure

open System.ComponentModel
open System.Runtime.CompilerServices

[<AbstractClass>]
type PropertyChangedBase() =

    // Define the propertyChanged event.
    let propertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()

    // Expose the PropertyChanged event as a first class .NET event.
    [<CLIEvent>]
    member _.PropertyChanged = propertyChanged.Publish

    // Define the add and remove methods to implement this interface.
    interface INotifyPropertyChanged with
        member _.add_PropertyChanged(handler) = propertyChanged.Publish.AddHandler(handler)
        member _.remove_PropertyChanged(handler) = propertyChanged.Publish.RemoveHandler(handler)

    // This is the event-handler method.
    abstract member OnPropertyChanged: string -> unit
    default me.OnPropertyChanged(propertyName) =
        // only trigger if the name provided is a real property
        if me.GetType().GetProperty(propertyName) <> null then
            propertyChanged.Trigger(me, PropertyChangedEventArgs(propertyName))
