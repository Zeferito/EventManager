using EventManager.Desktop.Api.Entities;
using Godot;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts;

public partial class AgregableMaterialItemComponent : HBoxContainer
{
	private VBoxContainer _parentContainer;

	private Label _labelMaterial;

	private Label _labelCantidad;

	private TextureButton _textureButtonEliminar;

	private EventToMaterial _eventToMaterial;

	public EventToMaterial EventToMaterial
	{
		get => _eventToMaterial;
		set => SetEventToMaterial(value);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_parentContainer = GetParent<VBoxContainer>();
		_labelMaterial = GetNode<Label>("LabelMaterial");
		_labelCantidad = GetNode<Label>("LabelCantidad");
		_textureButtonEliminar = GetNode<TextureButton>("VBoxContainer/TextureButtonEliminar");

		_textureButtonEliminar.Pressed += () =>
		{
			_parentContainer.RemoveChild(this);
			QueueFree();
		};
	}

	private void SetEventToMaterial(EventToMaterial value)
	{
		_eventToMaterial = value;
		_labelMaterial.Text = _eventToMaterial.Material.Name;
		_labelCantidad.Text = _eventToMaterial.AmountReserved.ToString();
	}
}
