using EventManager.Desktop.Api.Entities;
using Godot;

namespace EventManager.Desktop.Scenes.EditEvent.Components.Scripts;

public partial class ItemSala : HBoxContainer
{
	private VBoxContainer _parentContainer;

	private Label _labelSala;

	private Button _textureButtonEliminar;

	private Room _room;

	public Room Room
	{
		get => _room;
		set => SetRoom(value);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_parentContainer = GetParent<VBoxContainer>();
		_labelSala = GetNode<Label>("LabelSala");
		_textureButtonEliminar = GetNode<Button>("ButtonRemoveSala");

		_textureButtonEliminar.Pressed += () =>
		{
			_parentContainer.RemoveChild(this);
			QueueFree();
		};
	}

	private void SetRoom(Room value)
	{
		_room = value;
		_labelSala.Text = _room.Name;
	}
}
