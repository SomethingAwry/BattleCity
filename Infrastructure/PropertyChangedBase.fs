namespace BattleCity.Infrastructure

open System.ComponentModel
open Microsoft.FSharp.Quotations.Patterns

/// A base class for objects of which the properties must be observable.
[<AbstractClass>]
type PropertyChangedBase () =
    member val PropertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()
    
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member my.PropertyChanged = my.PropertyChanged.Publish
      
    abstract member OnPropertyChanged: e: PropertyChangedEventArgs -> unit
    default me.OnPropertyChanged e =
        me.PropertyChanged.Trigger(me, e)
    
    member me.OnPropertyChanged quotation =
        match quotation with
        | PropertyGet(_,pi,_) -> me.OnPropertyChanged(PropertyChangedEventArgs(pi.Name))
        | _ -> invalidArg (nameof quotation) "Expecting a Property expression"       
    