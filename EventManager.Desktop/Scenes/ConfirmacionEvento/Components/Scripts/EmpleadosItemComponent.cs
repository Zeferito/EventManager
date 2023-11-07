using EventManager.Desktop.Api.Entities;
using Godot;
using System;

namespace EventManager.Desktop.Scenes.ConfirmacionEvento.Components.Scripts;

public partial class EmpleadosItemComponent : HBoxContainer
{
    private Label _labelEmpleado;

    private Employee _employee;

    public Employee Employee
    {
        get => _employee;
        set => SetEmpleado(value);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _labelEmpleado = GetNode<Label>("LabelEmpleado");
    }

    private void SetEmpleado(Employee value)
    {
        _employee = value;
        _labelEmpleado.Text = value.Name;
    }
}
