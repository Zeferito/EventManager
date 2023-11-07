using EventManager.Desktop.Api.Entities;
using Godot;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts;

public partial class EmpleadoItemComponent : HBoxContainer
{
	private VBoxContainer _parentContainer;

	private Label _labelNombreEmpleado;

	private TextureButton _textureButtonEliminar;

	private Employee _employee;

	public Employee Employee
	{
		get => _employee;
		set => SetEmployee(value);
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

	private void SetEmployee(Employee value)
	{
		_employee = value;
		_labelNombreEmpleado.Text = _employee.Name;
	}
}