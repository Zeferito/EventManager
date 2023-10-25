using Godot;
using System;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts
{
    public partial class HandleBottomButtons : Node
    {
        [Export]
        private Button _buttonCancel;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _buttonCancel.Pressed += () =>
            {
                GetTree().ChangeSceneToFile("res://Scenes/Inicio/scena_pantalla_inicio.tscn");
            };
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta) { }
    }
}
