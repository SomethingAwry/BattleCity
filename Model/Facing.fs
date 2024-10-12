namespace BattleCity.Model

[<Measure>]
type radians

type Facing = float<radians>

module Facing =
    [<Literal>]
    let North = 0.0<radians>
    [<Literal>]
    let East = 1.5708<radians>
    [<Literal>]
    let South = 3.1416<radians>
    [<Literal>]
    let West = 4.7124<radians>

    let Directions = [| North; East; South; West |]