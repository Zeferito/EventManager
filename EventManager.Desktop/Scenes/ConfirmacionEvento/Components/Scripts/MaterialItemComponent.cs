using EventManager.Desktop.Api.Entities;
using Godot;
using System;

namespace EventManager.Desktop.Scenes.ConfirmacionEvento.Components.Scripts
{
    public partial class MaterialItemComponent : HBoxContainer
    {
        private Label _labelMaterial;

        private Label _labelCantidad;

        private EventToMaterial _eventToMaterial;

        public EventToMaterial EventToMaterial
        {
            get => _eventToMaterial;
            set => SetEventoAgregable(value);
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _labelMaterial = GetNode<Label>("LabelMaterial");
            _labelCantidad = GetNode<Label>("LabelCantidad");
        }

        private void SetEventoAgregable(EventToMaterial value)
        {
            _eventToMaterial = value;
            _labelMaterial.Text = value.Material.Name;
            _labelCantidad.Text = value.AmountReserved.ToString();
        }
    }
}
