using EventManager.Database.BusinessLogic.Services;
using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;
using Godot;
using System;

public partial class ButtonAgregarMaterial : TextureButton
{
	[Export]
	private VBoxContainer _listaMaterialesContainer;

	[Export]
	private OptionButton _optionButtonSeleccionarMaterial;

	[Export]
	private LineEdit _lineEditCantidadMaterial;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DatabaseConnection databaseConnection = GetNode<DatabaseConnection>("/root/DatabaseConnection");

		Pressed += () =>
		{
			PackedScene _agregableItemComponent
				= ResourceLoader.Load<PackedScene>("res://Scenes/CreateEventoSalon/Components/agregable_material_item_component.tscn");

			AgregableMaterialItemComponent agregableMaterialItemComponent
				= (AgregableMaterialItemComponent)_agregableItemComponent.Instantiate();

			_listaMaterialesContainer.AddChild(agregableMaterialItemComponent);

			int id = (int)_optionButtonSeleccionarMaterial.GetSelectedMetadata();

			Agregable material;

			using (DatabaseContext context = new DatabaseContext(databaseConnection.ConnectionString))
			{
				AgregableRepository agregableRepository = new AgregableRepository(context);
				AgregableService agregableService = new AgregableService(agregableRepository);
				material = agregableService.GetById(id);
			}

			if (material == null)
			{
				return;
			}

			int cantidad;

			if (!int.TryParse(_lineEditCantidadMaterial.Text, out cantidad))
			{
				return;
			}

			agregableMaterialItemComponent.Cantidad = cantidad;
			agregableMaterialItemComponent.Agregable = material;
		};
	}
}
