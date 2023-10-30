using EventManager.Database.BusinessLogic.Services;
using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;
using Godot;
using System;

public partial class ButtonAgregarCliente : Button
{
	[Export]
	private VBoxContainer _listaClientesContainer;

	[Export]
	private OptionButton _optionButtonBuscarCliente;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DatabaseConnection databaseConnection = GetNode<DatabaseConnection>("/root/DatabaseConnection");

		Pressed += () =>
		{
			int id = (int)_optionButtonBuscarCliente.GetSelectedMetadata();

			foreach (ClienteItemContainer node in _listaClientesContainer.GetChildren())
			{
				if (node.Cliente.Id == id)
				{
					return;
				}
			}

			PackedScene _clienteItemContainerScene
				= ResourceLoader.Load<PackedScene>("res://Scenes/CreateEventoSalon/Components/cliente_item_container.tscn");

			ClienteItemContainer clienteItemContainer = (ClienteItemContainer)_clienteItemContainerScene.Instantiate();

			_listaClientesContainer.AddChild(clienteItemContainer);

			Cliente cliente;

			using (DatabaseContext context = new DatabaseContext(databaseConnection.ConnectionString))
			{
				ClienteRepository clienteRepository = new ClienteRepository(context);
				ClienteService clienteService = new ClienteService(clienteRepository);
				cliente = clienteService.GetById(id);
			}

			if (cliente == null)
			{
				return;
			}

			clienteItemContainer.Cliente = cliente;
		};
	}
}
