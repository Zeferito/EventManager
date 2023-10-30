using EventManager.Database.BusinessLogic.Services;
using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;
using Godot;
using System;

namespace EventManager.Desktop.Scenes.ConfirmacionEvento.Components.Scripts
{
    public partial class ButtonRegister : Button
    {
        [Export]
        private ConfirmacionEvento _confirmacionEvento;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Pressed += () =>
            {
                RegisterStoredGlobalEvento();
                GetTree().ChangeSceneToFile("res://Scenes/Inicio/scena_pantalla_inicio.tscn");
            };
        }

        private void RegisterStoredGlobalEvento()
        {
            DatabaseConnection databaseConnection = GetNode<DatabaseConnection>(
                "/root/DatabaseConnection"
            );

            Global global = GetNode<Global>("/root/Global");

            using DatabaseContext context = new DatabaseContext(
                databaseConnection.ConnectionString
            );

            EventoRepository eventoRepository = new EventoRepository(context);
            EventoService eventoService = new EventoService(eventoRepository);

            GD.Print("El evento se ha registrado: \n" + global.EventoToSend.ToString());
            eventoService.Create(global.EventoToSend);
        }
    }
}
