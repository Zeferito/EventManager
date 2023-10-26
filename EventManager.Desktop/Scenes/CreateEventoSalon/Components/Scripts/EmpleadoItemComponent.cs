using Godot;
using System;
using EventManager.Database.Models.Entities;

public partial class EmpleadoItemComponent : HBoxContainer
{
	private VBoxContainer _parentContainer;
	private Label _labelNombreEmpleado;
	private TextureButton _textureButtonEliminar; 
	private Empleado _empleado;
	public Empleado Empleado
	{
		get => _empleado;
		set => SetEmpleado(value);
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_parentContainer = GetParent<VBoxContainer>();
		_labelNombreEmpleado = GetNode<Label>("LabelNombreEmpleado");
		_textureButtonEliminar = GetNode<TextureButton>("VBoxContainer/TextureButtonEliminar");

		_textureButtonEliminar.Pressed += () =>
		{
			_parentContainer.RemoveChild(this);
			QueueFree();
		};
	}
	private void SetEmpleado(Empleado value)
	{
		_empleado = value;
		_labelNombreEmpleado.Text = _empleado.Nombre;
	}
	
}
