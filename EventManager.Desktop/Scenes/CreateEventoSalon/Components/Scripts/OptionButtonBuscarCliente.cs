using EventManager.Database.BusinessLogic.Services;
using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;
using Godot;
using System;
using System.Collections.Generic;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts
{
	public partial class OptionButtonBuscarCliente : OptionButton
	{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			DatabaseConnection databaseConnection = GetNode<DatabaseConnection>(
				"/root/DatabaseConnection"
			);

			List<Cliente> clientes = new List<Cliente>();

			using (
				DatabaseContext context = new DatabaseContext(databaseConnection.ConnectionString)
			)
			{
				ClienteRepository clienteRepository = new ClienteRepository(context);
				ClienteService clienteService = new ClienteService(clienteRepository);
				clientes = clienteService.GetAll();
			}

			for (int i = 0; i < clientes.Count; i++)
			{
				GD.Print(clientes[i].Nombre);
				AddItem(clientes[i].Nombre, i);
				SetItemMetadata(i, clientes[i].Id);
			}
		}
	}
}
