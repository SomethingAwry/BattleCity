namespace BattleCity

open Avalonia.Input
open System.Collections.Generic

module internal Keyboard =
    let Keys = HashSet<Key>()

    let IsKeyDown (key: Key) =
        Keys.Contains key
