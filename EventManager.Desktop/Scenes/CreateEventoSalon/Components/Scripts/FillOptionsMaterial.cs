using EventManager.Database.BusinessLogic.Services;
using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;
using Godot;
using System;
using System.Collections.Generic;

public partial class FillOptionsMaterial : Node
{
	[Export]
	private OptionButton _optionButtonSeleccionarMaterial;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DatabaseConnection databaseConnection = GetNode<DatabaseConnection>("/root/DatabaseConnection");
		List<Agregable> materiales = new List<Agregable>();

		using (DatabaseContext context = new DatabaseContext(databaseConnection.ConnectionString))
		{
			AgregableRepository agregableRepository = new AgregableRepository(context);
			AgregableService agregableService = new AgregableService(agregableRepository);
			materiales = agregableService.GetAll();
		}
		for (int i = 0; i < materiales.Count; i++)
		{
			GD.Print(materiales[i].Nombre);
			_optionButtonSeleccionarMaterial.AddItem(materiales[i].Nombre, i);
			_optionButtonSeleccionarMaterial.SetItemMetadata(i, materiales[i].Id);
		}
		
	}
	public override void _Process(double delta) { }
}
