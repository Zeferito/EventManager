using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.BusinessLogic.Services;
using Godot;
using System;
using EventManager.Database.Models.Entities;

public partial class ButtonRegistrarCliente : Button
{
	[Export]
	private LineEdit _lineEditNombre;

	[Export]
	private LineEdit _lineEditTelefono;

	[Export]
	private VBoxContainer _listaClientesContainer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DatabaseConnection databaseConnection = GetNode<DatabaseConnection>("/root/DatabaseConnection");

		Pressed += () =>
		{
			Cliente cliente = new Cliente
			{
				Nombre = _lineEditNombre.Text,
				Telefono = _lineEditTelefono.Text
			};

			using (DatabaseContext context = new DatabaseContext(databaseConnection.ConnectionString))
			{
				ClienteRepository clienteRepository = new ClienteRepository(context);
				ClienteService clienteService = new ClienteService(clienteRepository);

				clienteService.Create(cliente);
			}

			PackedScene _clienteItemContainerScene
				= ResourceLoader.Load<PackedScene>("res://Scenes/CreateEventoSalon/Components/cliente_item_container.tscn");

			ClienteItemContainer clienteItemContainer = (ClienteItemContainer)_clienteItemContainerScene.Instantiate();

			clienteItemContainer.Cliente = cliente;

			_listaClientesContainer.AddChild(clienteItemContainer);
		};
	}
}
