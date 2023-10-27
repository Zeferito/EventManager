using EventManager.Database.Models.Entities;
using Godot;
using System;

public partial class AgregableMaterialItemComponent : HBoxContainer
{
	private VBoxContainer _parentContainer;
	private Label _labelMaterial;
	private Label _labelCantidad;
	private TextureButton _textureButtonEliminar;
	private Agregable _agregable;
	public Agregable Agregable
	{
		get => _agregable;
		set => SetAgregable(value);
	}
	private int _cantidad;
	public int Cantidad
	{
		get => _cantidad;
		set => SetCantidad(value);
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
	private void SetAgregable(Agregable value)
	{
		_agregable = value;
		_labelMaterial.Text = _agregable.Nombre;
	}

	private void SetCantidad(int value)
	{
		_cantidad = value;
		_labelCantidad.Text = value.ToString();
	}
}
