using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using EventManager.Desktop.Api.Dto;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;

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
        ApiConnection apiConnection = GetNode<ApiConnection>("/root/ApiConnection");

        Pressed += () =>
        {
            DateTime parsedStartDate;
            if (!DateTime.TryParse(_lineEditStartDateEvent.Text, out parsedStartDate))
            {
                return;
            }

            DateTime parsedEndDate;
            if (!DateTime.TryParse(_lineEditEndDateEvent.Text, out parsedEndDate))
            {
                return;
            }

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

            EventDto eventDto = new EventDto
            {
                Status = "active",
                Name = _lineEditTitleEvent.Text,
                Description = _textEditDescriptionEvent.Text,
                StartDate = parsedStartDate.ToString("o", CultureInfo.InvariantCulture),
                EndDate = parsedEndDate.ToString("o", CultureInfo.InvariantCulture),
                UserId = 1,
                ClientIds = clientIds,
                EmployeeIds = employeeIds,
                RoomIds = roomIds,
                MaterialsReserved = materialsReserved
            };

            HttpRequest httpRequest = new HttpRequest();
            httpRequest.UseThreads = true;
            AddChild(httpRequest);
            httpRequest.RequestCompleted += HttpRequestCompleted;
            httpRequest.RequestCompleted += (result, code, strings, body) =>
            {
                RemoveChild(httpRequest);
                httpRequest.QueueFree();
            };

            string contentType = "application/json";
            string authToken = apiConnection.AuthDetails.AuthToken;
            string[] headers =
            {
                $"Content-Type: {contentType}",
                $"Authorization: Bearer {authToken}"
            };

            string body = JsonSerializer.Serialize(eventDto);

            Error error = httpRequest.Request($"{apiConnection.Url}/events", headers, HttpClient.Method.Post, body);

            if (error != Error.Ok)
            {
                GD.PushError("An error occurred in the HTTP request.");
            }
        };
    }

    private void HttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        Json json = new Json();
        json.Parse(body.GetStringFromUtf8());

        Godot.Collections.Array responseArray = json.Data.AsGodotArray();
        Dictionary responseDictionary = json.Data.AsGodotDictionary();

        switch (responseCode)
        {
            case 201:
                GD.Print(responseDictionary);
                GetTree().ChangeSceneToFile("res://Scenes/Inicio/scena_pantalla_inicio.tscn");
                break;
            default:
                GD.PrintErr(responseDictionary);
                break;
        }
    }
}