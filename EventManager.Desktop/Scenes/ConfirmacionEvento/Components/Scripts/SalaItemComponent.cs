using EventManager.Desktop.Api.Entities;
using Godot;
using System;

namespace EventManager.Desktop.Scenes.ConfirmacionEvento.Components.Scripts
{
    public partial class SalaItemComponent : HBoxContainer
    {
        private Label _labelSala;

        private Room _room;

        public Room Room
        {
            get => _room;
            set => SetSala(value);
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _labelSala = GetNode<Label>("LabelSala");
        }

        private void SetSala(Room value)
        {
            _room = value;
            _labelSala.Text = value.Name;
        }
    }
}
