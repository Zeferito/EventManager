using EventManager.Database.Models.Entities;
using Godot;
using System;

namespace EventManager.Desktop.Scenes.ConfirmacionEvento.Components.Scripts
{
    public partial class EmpleadosItemComponent : HBoxContainer
    {
        private Label _labelEmpleado;

        private Empleado _empleado;

        public Empleado Empleado
        {
            get => _empleado;
            set => SetEmpleado(value);
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _labelEmpleado = GetNode<Label>("LabelEmpleado");
        }

        private void SetEmpleado(Empleado value)
        {
            _empleado = value;
            _labelEmpleado.Text = value.Nombre;
        }
    }
}
