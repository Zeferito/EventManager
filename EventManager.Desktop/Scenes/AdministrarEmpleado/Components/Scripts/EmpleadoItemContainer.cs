using EventManager.Desktop.Api.Entities;
using Godot;
using System;


public partial class EmpleadoItemContainer : HBoxContainer
{
	private Label _labelNombre;

	private Employee _employee;

	public Employee Employee
	{
		get => _employee;
		set => SetEmploye(value);
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_labelNombre = GetNode<Label>("LabelNombre");
	}

	private void SetEmploye(Employee value)
	{
		_employee = value;
		_labelNombre.Text = _employee.Name;
	}
}
