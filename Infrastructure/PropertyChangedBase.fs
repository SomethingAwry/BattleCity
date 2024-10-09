namespace BattleCity.Infrastructure

open System.ComponentModel
open Microsoft.FSharp.Quotations.Patterns

[<AbstractClass>]
type PropertyChangedBase() =

    // Define the propertyChanged event.
    let propertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()

    // Allow for quotations to change properties as well (part 1 of 2)
    let getPropertyName = function 
        | PropertyGet(_,pi,_) -> pi.Name
        | _ -> invalidOp "Expecting property getter expression"

    // Expose the PropertyChanged event as a first class .NET event, implementing the interface. 
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member _.PropertyChanged = propertyChanged.Publish

    // For use on verified property names
    member private me.triggerByPropertyName propertyName =
        propertyChanged.Trigger(me, PropertyChangedEventArgs(propertyName))

    // This is the event-handler method.
    abstract member OnPropertyChanged: string -> unit
    default me.OnPropertyChanged(propertyName) =
        // only trigger if the name provided is a real property
        if me.GetType().GetProperty(propertyName) <> null then
            me.triggerByPropertyName propertyName

    // Allow for quotations to change properties as well (part 2 of 2)
    member me.OnPropertyChanged quotation = 
        quotation |> getPropertyName |> me.triggerByPropertyName
