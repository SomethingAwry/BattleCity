using BattleCity.Infrastructure;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BattleCity.Model;

public abstract class GameObject : ObservableObject
{
    private Point _location;

    protected GameObject(Point location)
    {
        Location = location;
    }

    public Point Location
    {
        get => _location;
        protected set => SetProperty(ref _location, value);
    }

    public virtual int Layer => 0;
}