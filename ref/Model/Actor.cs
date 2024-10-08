﻿namespace BattleCity.Model;

using Avalonia;
using BattleCity.Infrastructure;

public abstract class GameObject : PropertyChangedBase {
    private Point _location;

    protected GameObject(Point location) {
        Location = location;
    }

    public Point Location {
        get => _location;
        protected set {
            if (value.Equals(_location)) return;
            _location = value;
            OnPropertyChanged();
        }
    }

    public virtual int Layer => 0;
}