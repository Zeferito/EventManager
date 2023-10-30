using EventManager.Database.BusinessLogic.Services;
using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;
using Godot;
using System;

public partial class ButtonAgregarSala : Button
{
    [Export]
    private VBoxContainer _listaSalasContainer;

    [Export]
    private OptionButton _optionButtonSala;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        DatabaseConnection databaseConnection = GetNode<DatabaseConnection>(
            "/root/DatabaseConnection"
        );

        Pressed += () =>
        {
            int id = (int)_optionButtonSala.GetSelectedMetadata();

            foreach (ItemSalaComponent node in _listaSalasContainer.GetChildren())
            {
                if (node.Sala.Id == id)
                {
                    return;
                }
            }

            PackedScene _salaItemComponentScene = ResourceLoader.Load<PackedScene>(
                "res://Scenes/CreateEventoSalon/Components/item_sala.tscn"
            );

            ItemSalaComponent itemSalaComponent = (ItemSalaComponent)_salaItemComponentScene.Instantiate();

            _listaSalasContainer.AddChild(itemSalaComponent);

            Sala sala;

            using (DatabaseContext context = new DatabaseContext(databaseConnection.ConnectionString))
            {
                SalaRepository salaRepository = new SalaRepository(context);
                SalaService salaService = new SalaService(salaRepository);
                sala = salaService.GetById(id);
            }

            if (sala == null)
            {
                return;
            }

            itemSalaComponent.Sala = sala;
        };
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
