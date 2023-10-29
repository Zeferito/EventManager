using EventManager.Database.BusinessLogic.Services;
using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;
using Godot;
using System;
using System.Collections.Generic;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts
{
	public partial class OptionButtonSeleccionarSalon : OptionButton
	{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			DatabaseConnection databaseConnection = GetNode<DatabaseConnection>(
				"/root/DatabaseConnection"
			);

			List<Sala> salas = new List<Sala>();

			using (
				DatabaseContext context = new DatabaseContext(databaseConnection.ConnectionString)
			)
			{
				SalaRepository salaRepository = new SalaRepository(context);
				SalaService salaService = new SalaService(salaRepository);
				salas = salaService.GetAll();
			}

			for (int i = 0; i < salas.Count; i++)
			{
				GD.Print(salas[i].Nombre);
				AddItem(salas[i].Nombre, i);
				SetItemMetadata(i, salas[i].Id);
			}
		}
	}
}
