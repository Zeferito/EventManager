using EventManager.Database.BusinessLogic.Services;
using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;
using Godot;
using System;

public partial class AgregarButton : Button
{
	[Export]
	private VBoxContainer _listaEmpleadosContainer;
	[Export]
	private OptionButton _optionButtonEmpleados;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DatabaseConnection databaseConnection = GetNode<DatabaseConnection>("/root/DatabaseConnection");
		
		Pressed += () =>
		{
			PackedScene _empleadoItemComponentScene
				= ResourceLoader.Load<PackedScene>("res://Scenes/CreateEventoSalon/Components/empleado_item_component.tscn");

			EmpleadoItemComponent empleadoItemComponent = (EmpleadoItemComponent)_empleadoItemComponentScene.Instantiate();

			_listaEmpleadosContainer.AddChild(empleadoItemComponent);

			int id = (int)_optionButtonEmpleados.GetSelectedMetadata();

			Empleado empleado;

			using (DatabaseContext context = new DatabaseContext(databaseConnection.ConnectionString))
			{
				EmpleadoRepository empleadoRepository = new EmpleadoRepository(context);
				EmpleadoService empleadoService = new EmpleadoService(empleadoRepository);
				empleado = empleadoService.GetById(id);
			}

			if (empleado == null)
			{
				return;
			}

			empleadoItemComponent.Empleado = empleado;
		};
	}
}
