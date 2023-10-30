using EventManager.Database.Models.Entities;
using Godot;
using System;

namespace EventManager.Desktop.Scenes.ConfirmacionEvento.Components.Scripts
{
    public partial class SalaItemComponent : HBoxContainer
    {
        private Label _labelSala;

        private Sala _sala;

        public Sala Sala
        {
            get => _sala;
            set => SetSala(value);
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _labelSala = GetNode<Label>("LabelSala");
        }

        private void SetSala(Sala value)
        {
            _sala = value;
            _labelSala.Text = value.Nombre;
        }
    }
}
