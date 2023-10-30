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
			int id = (int)_optionButtonSeleccionarMaterial.GetSelectedMetadata();

			int cantidad;

			if (!int.TryParse(_lineEditCantidadMaterial.Text, out cantidad))
			{
				return;
			}

			foreach (AgregableMaterialItemComponent node in _listaMaterialesContainer.GetChildren())
			{
				if (node.Agregable.Id == id && node.Cantidad == cantidad)
				{
					return;
				}

				if (node.Agregable.Id == id)
				{
					_listaMaterialesContainer.RemoveChild(node);
					node.QueueFree();
				}
			}

			PackedScene agregableItemComponent
				= ResourceLoader.Load<PackedScene>("res://Scenes/CreateEventoSalon/Components/agregable_material_item_component.tscn");

			AgregableMaterialItemComponent agregableMaterialItemComponent
				= (AgregableMaterialItemComponent)agregableItemComponent.Instantiate();

			_listaMaterialesContainer.AddChild(agregableMaterialItemComponent);

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

			agregableMaterialItemComponent.Cantidad = cantidad;
			agregableMaterialItemComponent.Agregable = material;
		};
	}
}
