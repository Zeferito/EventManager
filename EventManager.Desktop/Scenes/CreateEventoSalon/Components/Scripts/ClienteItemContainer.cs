using EventManager.Desktop.Api.Entities;
using Godot;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts;

public partial class ClienteItemContainer : HBoxContainer
{
	private VBoxContainer _parentContainer;

	private Label _labelNombre;

	private Label _labelTelefono;

	private TextureButton _textureButtonEliminar;

	private Client _client;

	public Client Client
	{
		get => _client;
		set => SetClient(value);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_parentContainer = GetParent<VBoxContainer>();
		_labelNombre = GetNode<Label>("LabelNombre");
		_labelTelefono = GetNode<Label>("LabelTelefono");
		_textureButtonEliminar = GetNode<TextureButton>("VBoxContainer/TextureButtonEliminar");

		_textureButtonEliminar.Pressed += () =>
		{
			_parentContainer.RemoveChild(this);
			QueueFree();
		};
	}

	private void SetClient(Client value)
	{
		_client = value;
		_labelNombre.Text = _client.Name;
		_labelTelefono.Text = _client.Phone;
	}
}