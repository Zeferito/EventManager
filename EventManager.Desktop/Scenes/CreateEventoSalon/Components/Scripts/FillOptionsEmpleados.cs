using EventManager.Database.BusinessLogic.Services;
using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;
using Godot;
using System;
using System.Collections.Generic;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts
{
    public partial class FillOptionsEmpleados : Node
    {
        [Export]
        private OptionButton _optionButtonEmpleados;

        [Export]
        private string _connectionString;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            List<Empleado> empleados = new List<Empleado>();

            using (DatabaseContext context = new DatabaseContext(_connectionString))
            {
                EmpleadoRepository empleadoRepository = new EmpleadoRepository(context);
                EmpleadoService empleadoService = new EmpleadoService(empleadoRepository);
                empleados = empleadoService.GetAll();
            }

            foreach (Empleado empleado in empleados)
            {
                GD.Print(empleado.Nombre);
                _optionButtonEmpleados.AddItem(empleado.Nombre);
            }
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta) { }
    }
}
