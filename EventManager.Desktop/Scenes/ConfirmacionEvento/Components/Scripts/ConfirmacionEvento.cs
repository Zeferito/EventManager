using EventManager.Database.Models.Entities;
using Godot;
using System;

public partial class ConfirmacionEvento : Control
{
	[Export]
	private EventoDescripcionComponent _eventoDescripcionComponent;

	[Export]
	private Label _labelFechaInicio;

	[Export]
	private Label _labelFechaTermino;

	[Export]
	private VBoxContainer _listaClientesContainer;

	[Export]
	private VBoxContainer _listaEmpleadosContainer;

	[Export]
	private VBoxContainer _listaSalasContainer;

	[Export]
	private VBoxContainer _listaAgregablesContainer;

	private Evento _evento;

	public Evento Evento
	{
		get => _evento;
		set => SetEvento(value);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		foreach (Node node in _listaClientesContainer.GetChildren())
		{
			_listaClientesContainer.RemoveChild(node);
			node.QueueFree();
		}

		foreach (Node node in _listaEmpleadosContainer.GetChildren())
		{
			_listaEmpleadosContainer.RemoveChild(node);
			node.QueueFree();
		}

		foreach (Node node in _listaSalasContainer.GetChildren())
		{
			_listaSalasContainer.RemoveChild(node);
			node.QueueFree();
		}

		foreach (Node node in _listaAgregablesContainer.GetChildren())
		{
			_listaAgregablesContainer.RemoveChild(node);
			node.QueueFree();
		}

		Global global = GetNode<Global>("/root/Global");
		Evento = global.EventoToSend;
		global.EventoToSend = null;
	}

	private void SetEvento(Evento value)
	{
		_evento = value;
		_eventoDescripcionComponent.Evento = value;
		_labelFechaInicio.Text = value.FechaInicio.ToString();
		_labelFechaTermino.Text = value.FechaTermino.ToString();

		foreach (Sala sala in value.Salas)
		{
			PackedScene _salaItemComponentScene = ResourceLoader.Load<PackedScene>(
				"res://Scenes/ConfirmacionEvento/Components/sala_item_component.tscn"
			);

			SalaItemComponent itemSalaComponent = (SalaItemComponent)
				_salaItemComponentScene.Instantiate();

			_listaSalasContainer.AddChild(itemSalaComponent);

			itemSalaComponent.Sala = sala;
		}

		foreach (Empleado empleado in value.Empleados)
		{
			PackedScene _empleadoItemComponentScene = ResourceLoader.Load<PackedScene>(
				"res://Scenes/ConfirmacionEvento/Components/empleados_item_component.tscn"
			);

			EmpleadosItemComponent itemEmpleadosComponent = (EmpleadosItemComponent)
				_empleadoItemComponentScene.Instantiate();

			_listaEmpleadosContainer.AddChild(itemEmpleadosComponent);

			itemEmpleadosComponent.Empleado = empleado;
		}

		foreach (EventoAgregable eventoAgregable in value.EventoAgregables)
		{
			PackedScene _materialItemComponentScene = ResourceLoader.Load<PackedScene>(
				"res://Scenes/ConfirmacionEvento/Components/material_item_component.tscn"
			);

			MaterialItemComponent itemMaterialComponent = (MaterialItemComponent)
				_materialItemComponentScene.Instantiate();

			_listaAgregablesContainer.AddChild(itemMaterialComponent);

			itemMaterialComponent.EventoAgregable = eventoAgregable;
		}

		foreach (Cliente cliente in value.Clientes)
		{
			PackedScene _clienteItemComponentScene = ResourceLoader.Load<PackedScene>(
				"res://Scenes/ConfirmacionEvento/Components/cliente_item_component.tscn"
			);

			ClienteItemComponent itemClienteComponent = (ClienteItemComponent)
				_clienteItemComponentScene.Instantiate();

			_listaClientesContainer.AddChild(itemClienteComponent);

			itemClienteComponent.Cliente = cliente;
		}
	}
}
