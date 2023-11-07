using System;
using System.Collections.Generic;
using System.Text.Json;
using EventManager.Desktop.Api.Dto;
using Godot;
using static EventManager.Desktop.Api.Entities.Event;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts;

public partial class ButtonConfirm : Button
{
    [Export]
    private LineEdit _lineEditTitleEvent;

    [Export]
    private TextEdit _textEditDescriptionEvent;

    [Export]
    private LineEdit _lineEditStartDateEvent;

    [Export]
    private LineEdit _lineEditEndDateEvent;

    [Export]
    private VBoxContainer _clientesListaContainer;

    [Export]
    private VBoxContainer _salasListaContainer;

    [Export]
    private VBoxContainer _empleadosListaContainer;

    [Export]
    private VBoxContainer _agregablesListaContainer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Global global = GetNode<Global>("/root/Global");

        Pressed += () =>
        {
            List<int> clientIds = new();
            List<int> employeeIds = new();
            List<int> roomIds = new();
            List<EventDto.MaterialReserved> materialsReserved = new();

            foreach (ClienteItemContainer node in _clientesListaContainer.GetChildren())
            {
                clientIds.Add(node.Client.Id);
            }

            foreach (EmpleadoItemComponent node in _empleadosListaContainer.GetChildren())
            {
                employeeIds.Add(node.Employee.Id);
            }

            foreach (ItemSalaComponent node in _salasListaContainer.GetChildren())
            {
                roomIds.Add(node.Room.Id);
            }

            foreach (var node in _agregablesListaContainer.GetChildren())
            {
                var materialReservedItem = (AgregableMaterialItemComponent)node;
                EventDto.MaterialReserved materialReserved = new EventDto.MaterialReserved
                {
                    MaterialId = materialReservedItem.EventToMaterial.Material.Id,
                    Amount = materialReservedItem.EventToMaterial.AmountReserved
                };
                materialsReserved.Add(materialReserved);
            }

            EventDto evento = new EventDto
            {
                Status = "active",
                Name = _lineEditTitleEvent.Text,
                Description = _textEditDescriptionEvent.Text,
                StartDate = _lineEditStartDateEvent.Text,
                EndDate = _lineEditEndDateEvent.Text,
                UserId = 1,
                ClientIds = clientIds,
                EmployeeIds = employeeIds,
                RoomIds = roomIds,
                MaterialsReserved = materialsReserved
            };

            global.EventToSend = evento;

            GetTree()
                .ChangeSceneToFile(
                    "res://Scenes/ConfirmacionEvento/confirmacion_evento_scene.tscn"
                );
        };
    }
}