namespace BattleCity;

using Avalonia.Input;
using System.Collections.Generic;

internal static class Keyboard {
    public static readonly HashSet<Key> Keys = [];

    public static bool IsKeyDown(Key key) {
        return Keys.Contains(key);
    }
}