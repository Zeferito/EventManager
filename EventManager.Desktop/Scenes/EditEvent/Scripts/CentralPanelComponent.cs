using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using EventManager.Desktop.Api.Dto;
using EventManager.Desktop.Api.Entities;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using EventManager.Desktop.Scenes.EditEvent.Components.Scripts;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace EventManager.Desktop.Scenes.EditEvent.Scripts;

public partial class CentralPanelComponent : Control
{
    [Export]
    private LineEdit _lineEditNuevoNombreEvento;

    [Export]
    private LineEdit _lineEditNuevaDescripcionEvento;

    [Export]
    private LineEdit _lineEditFechaInicio;

    [Export]
    private LineEdit _lineEditFechaTermino;

    [Export]
    private VBoxContainer _listaClientesContainer;

    [Export]
    private VBoxContainer _listaMaterialesContainer;

    [Export]
    private VBoxContainer _listaEmpleadosContainer;

    [Export]
    private VBoxContainer _listaSalaContainer;

    [Export]
    private Button _actualizarButton;

    private Event _event;

    public Event Event
    {
        get => _event;
        set => SetEvent(value);
    }

    public override void _Ready()
    {
        TestLoadExampleEvent();

        _actualizarButton.Pressed += () =>
        {
            ApiConnection apiConnection = GetNode<ApiConnection>("/root/ApiConnection");

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

            EventDto eventDto = BuildEventDto();
            string body = JsonSerializer.Serialize(eventDto);

            int id = Event.Id;

            Error error = httpRequest.Request($"{apiConnection.Url}/events/{id}", headers, HttpClient.Method.Put,
                body);

            if (error != Error.Ok)
            {
                GD.PushError("An error occurred in the HTTP request.");
            }
        };
    }

    private void TestLoadExampleEvent()
    {
        ApiConnection apiConnection = GetNode<ApiConnection>("/root/ApiConnection");

        HttpRequest httpRequest = new HttpRequest();
        httpRequest.UseThreads = true;
        AddChild(httpRequest);
        httpRequest.RequestCompleted += HttpTestLoadExampleEventRequestCompleted;
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

        int id = 6;

        Error error = httpRequest.Request($"{apiConnection.Url}/events/{id}", headers, HttpClient.Method.Get);

        if (error != Error.Ok)
        {
            GD.PushError("An error occurred in the HTTP request.");
        }
    }

    private void HttpTestLoadExampleEventRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        Json json = new Json();
        json.Parse(body.GetStringFromUtf8());

        Array responseArray = json.Data.AsGodotArray();
        Dictionary responseDictionary = json.Data.AsGodotDictionary();

        switch (responseCode)
        {
            case 200:
                GD.Print(responseDictionary);

                string dictionaryJson = Json.Stringify(responseDictionary);

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    },
                };

                Event evt = JsonSerializer.Deserialize<Event>(dictionaryJson, options);
                Event = evt;

                break;
            case 401:
                GD.Print(responseDictionary);
                break;
            case 404:
                GD.Print(responseDictionary);
                break;
            default:
                GD.Print(responseDictionary);
                break;
        }
    }

    private EventDto BuildEventDto()
    {
        int version = Event.Version;
        string name = _lineEditNuevoNombreEvento.Text;
        string description = _lineEditNuevaDescripcionEvento.Text;
        string startDate = _lineEditFechaInicio.Text;
        string endDate = _lineEditFechaTermino.Text;
        int userId = Event.User.Id;

        List<int> clientIds = new();
        foreach (var node in _listaClientesContainer.GetChildren())
        {
            var clienteItem = (ClienteItemContainer)node;
            clientIds.Add(clienteItem.Client.Id);
        }

        List<int> employeeIds = new();
        foreach (var node in _listaEmpleadosContainer.GetChildren())
        {
            var employeeItem = (EmpleadoItemComponent)node;
            employeeIds.Add(employeeItem.Employee.Id);
        }

        List<int> roomIds = new();
        foreach (var node in _listaSalaContainer.GetChildren())
        {
            var roomItem = (ItemSala)node;
            roomIds.Add(roomItem.Room.Id);
        }

        List<EventDto.MaterialReserved> materialsReserved = new();
        foreach (var node in _listaMaterialesContainer.GetChildren())
        {
            var materialReservedItem = (AgregableMaterialItemComponent)node;
            EventDto.MaterialReserved materialReserved = new EventDto.MaterialReserved
            {
                MaterialId = materialReservedItem.EventToMaterial.Material.Id,
                Amount = materialReservedItem.EventToMaterial.AmountReserved
            };
            materialsReserved.Add(materialReserved);
        }

        return new EventDto
        {
            Version = version,
            // TODO: Parse enum name to string
            Status = "active",
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            UserId = userId,
            ClientIds = clientIds,
            EmployeeIds = employeeIds,
            RoomIds = roomIds,
            MaterialsReserved = materialsReserved
        };
    }

    private void SetEvent(Event value)
    {
        _event = value;

        _lineEditNuevoNombreEvento.Text = _event.Name;
        _lineEditNuevaDescripcionEvento.Text = _event.Description;
        _lineEditFechaInicio.Text = _event.StartDate;
        _lineEditFechaTermino.Text = _event.EndDate;

        foreach (Room room in Event.Rooms)
        {
            PackedScene salaItemComponentScene = ResourceLoader.Load<PackedScene>(
                "res://Scenes/EditEvent/Components/item_sala.tscn"
            );

            ItemSala itemSalaComponent = (ItemSala)
                salaItemComponentScene.Instantiate();

            _listaSalaContainer.AddChild(itemSalaComponent);

            itemSalaComponent.Room = room;
        }

        foreach (Employee employee in Event.Employees)
        {
            PackedScene empleadoItemComponentScene = ResourceLoader.Load<PackedScene>(
                "res://Scenes/EditEvent/Components/empleado_item_component.tscn"
            );

            EmpleadoItemComponent itemEmpleadosComponent = (EmpleadoItemComponent)
                empleadoItemComponentScene.Instantiate();

            _listaEmpleadosContainer.AddChild(itemEmpleadosComponent);

            itemEmpleadosComponent.Employee = employee;
        }

        foreach (EventToMaterial eventToMaterial in Event.EventToMaterial)
        {
            PackedScene materialItemComponentScene = ResourceLoader.Load<PackedScene>(
                "res://Scenes/EditEvent/Components/agregable_material_item_component.tscn"
            );

            AgregableMaterialItemComponent itemMaterialComponent = (AgregableMaterialItemComponent)
                materialItemComponentScene.Instantiate();

            _listaMaterialesContainer.AddChild(itemMaterialComponent);

            GD.Print(JsonSerializer.Serialize(itemMaterialComponent.EventToMaterial));

            itemMaterialComponent.EventToMaterial = eventToMaterial;
        }

        foreach (Client client in Event.Clients)
        {
            PackedScene clienteItemComponentScene = ResourceLoader.Load<PackedScene>(
                "res://Scenes/EditEvent/Components/cliente_item_container.tscn"
            );

            ClienteItemContainer itemClienteComponent = (ClienteItemContainer)
                clienteItemComponentScene.Instantiate();

            _listaClientesContainer.AddChild(itemClienteComponent);

            itemClienteComponent.Client = client;
        }
    }

    private void HttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        Json json = new Json();
        json.Parse(body.GetStringFromUtf8());

        Array responseArray = json.Data.AsGodotArray();
        Dictionary responseDictionary = json.Data.AsGodotDictionary();

        switch (responseCode)
        {
            case 200:
                GD.Print(responseDictionary);
                break;
            case 401:
                GD.Print(responseDictionary);
                break;
            case 404:
                GD.Print(responseDictionary);
                break;
            case 409:
                GD.Print(responseDictionary);
                break;
            default:
                GD.Print(responseDictionary);
                break;
        }
    }
}