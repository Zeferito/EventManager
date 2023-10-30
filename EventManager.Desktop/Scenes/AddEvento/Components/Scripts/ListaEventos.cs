using EventManager.Database.BusinessLogic.Services;
using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;
using Godot;
using System;
using System.Collections.Generic;

public partial class ListaEventos : VBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Clear();

		DatabaseConnection databaseConnection = GetNode<DatabaseConnection>(
				"/root/DatabaseConnection"
			);
		List<Evento> eventos = new List<Evento>();

		using (
			DatabaseContext context = new DatabaseContext(databaseConnection.ConnectionString)
		)
		{
			EventoRepository eventoRepository = new EventoRepository(context);
			EventoService eventoService = new EventoService(eventoRepository);
			eventos = eventoService.GetEventosWithRelatedData();
		}
		foreach (Evento evento in eventos)
		{
			PackedScene _eventoItemComponent = ResourceLoader.Load<PackedScene>(
				"res://Scenes/AddEvento/Components/agregable_evento_item_component.tscn"
			);
			AgregableEventoItemComponent eventoItemComponent = (AgregableEventoItemComponent)
				_eventoItemComponent.Instantiate();

			AddChild(eventoItemComponent);
			eventoItemComponent.Evento = evento;
		}
	}

	public void Clear()
	{
		foreach (Node node in GetChildren())
		{
			RemoveChild(node);
			node.QueueFree();
		}
	}
}
