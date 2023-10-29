using EventManager.Database.BusinessLogic.Services;
using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;
using Godot;
using System;
using System.Collections.Generic;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts
{
    public partial class OptionButtonSeleccionarMaterial : OptionButton
    {
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            DatabaseConnection databaseConnection = GetNode<DatabaseConnection>(
                "/root/DatabaseConnection"
            );

            List<Agregable> materiales = new List<Agregable>();

            using (
                DatabaseContext context = new DatabaseContext(databaseConnection.ConnectionString)
            )
            {
                AgregableRepository agregableRepository = new AgregableRepository(context);
                AgregableService agregableService = new AgregableService(agregableRepository);
                materiales = agregableService.GetAll();
            }

            for (int i = 0; i < materiales.Count; i++)
            {
                GD.Print(materiales[i].Nombre);
                AddItem(materiales[i].Nombre, i);
                SetItemMetadata(i, materiales[i].Id);
            }
        }
    }
}
