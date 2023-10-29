using EventManager.Database.BusinessLogic.Services;
using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;
using Godot;
using System;
using System.Collections.Generic;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts
{
    public partial class OptionButtonEmpleados : OptionButton
    {
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            DatabaseConnection databaseConnection = GetNode<DatabaseConnection>(
                "/root/DatabaseConnection"
            );

            List<Empleado> empleados = new List<Empleado>();

            using (
                DatabaseContext context = new DatabaseContext(databaseConnection.ConnectionString)
            )
            {
                EmpleadoRepository empleadoRepository = new EmpleadoRepository(context);
                EmpleadoService empleadoService = new EmpleadoService(empleadoRepository);
                empleados = empleadoService.GetAll();
            }

            for (int i = 0; i < empleados.Count; i++)
            {
                GD.Print(empleados[i].Nombre);
                AddItem(empleados[i].Nombre, i);
                SetItemMetadata(i, empleados[i].Id);
            }
        }
    }
}
