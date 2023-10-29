using EventManager.Database.Models.Entities;
using Godot;
using System;

public partial class MaterialItemComponent : HBoxContainer
{
	private Label _labelMaterial;

	private Label _labelCantidad;

	private EventoAgregable _eventoAgregable;

	public EventoAgregable EventoAgregable
	{
		get => _eventoAgregable;
		set => SetEventoAgregable(value);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_labelMaterial = GetNode<Label>("LabelMaterial");
		_labelCantidad = GetNode<Label>("LabelCantidad");
	}

	private void SetEventoAgregable(EventoAgregable value)
	{
		_eventoAgregable = value;
		_labelMaterial.Text = value.Agregable.Nombre;
		_labelCantidad.Text = value.CantidadReservada.ToString();
	}
}
