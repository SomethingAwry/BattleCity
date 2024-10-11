namespace BattleCity.Infrastructure

open System.Collections.Generic
open System.ComponentModel
open System.Runtime.CompilerServices
open Microsoft.FSharp.Quotations.Patterns

/// A base class for objects of which the properties must be observable.
[<AbstractClass>]
type PropertyChangedBase (enableINotifyPropertyChangingSupport: bool) =
    new() = PropertyChangedBase(false)

    member val PropertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()
    member val PropertyChanging = Event<PropertyChangingEventHandler, PropertyChangingEventArgs>()
    
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member my.PropertyChanged = my.PropertyChanged.Publish
  
    interface INotifyPropertyChanging with
        [<CLIEvent>]
        member my.PropertyChanging = my.PropertyChanging.Publish
    
    abstract member OnPropertyChanged: e: PropertyChangedEventArgs -> unit
    default me.OnPropertyChanged e =
        me.PropertyChanged.Trigger(me, e)
    
    abstract member OnPropertyChanging: e: PropertyChangingEventArgs -> unit
    default me.OnPropertyChanging e =
        if enableINotifyPropertyChangingSupport then
            me.PropertyChanging.Trigger(me, e)
  
    member me.OnPropertyChanged quotation =
        match quotation with
        | PropertyGet(_,pi,_) -> me.OnPropertyChanged(PropertyChangedEventArgs(pi.Name))
        | _ -> invalidArg (nameof quotation) "Expecting a Property expression"       
    
    member me.OnPropertyChanging quotation =
        if enableINotifyPropertyChangingSupport then
            match quotation with
            | PropertyGet(_,pi,_) -> me.OnPropertyChanging(PropertyChangingEventArgs(pi.Name))
            | _ -> invalidArg (nameof quotation) "Expecting a Property expression"       
        
    member me.SetProperty<'T> (field: byref<'T>, newValue: 'T, [<CallerMemberName>] ?propertyName: string) : bool =
        match EqualityComparer<'T>.Default.Equals(field, newValue), propertyName with
        | true, _ -> false
        | false, None -> invalidArg (nameof propertyName) "No Property specified"
#if DEBUG
        | false, Some badName when me.GetType().GetProperty(badName) = null -> invalidArg (nameof propertyName) $"Property {badName} doesn't exist"
#endif
        | false, Some name ->
            me.OnPropertyChanging(PropertyChangingEventArgs(name))
            field <- newValue
            me.OnPropertyChanged(PropertyChangedEventArgs(name))
            true
