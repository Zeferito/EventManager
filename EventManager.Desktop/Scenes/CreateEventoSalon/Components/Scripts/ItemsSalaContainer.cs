using Godot;
using System;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts
{
    public partial class ItemsSalaContainer : VBoxContainer
    {
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            foreach (Node node in GetChildren())
            {
                RemoveChild(node);
                node.QueueFree();
            }
        }
    }
}
