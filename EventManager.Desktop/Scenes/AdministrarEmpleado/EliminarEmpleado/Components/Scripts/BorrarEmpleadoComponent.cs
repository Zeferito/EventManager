using EventManager.Desktop.Api.Entities;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;
using System;

namespace EventManager.Desktop.Scenes.AdministrarEmpleado.EliminarEmpleado.Components.Scripts;

public partial class BorrarEmpleadoComponent : HBoxContainer
{
    private VBoxContainer _parentContainer;

    private Label _labelNombre;

    private TextureButton _textureButtonEliminar;

    private Employee _employee;

    public Employee Client
    {
        get => _employee;
        set => SetEmployee(value);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _parentContainer = GetParent<VBoxContainer>();
        _labelNombre = GetNode<Label>("LabelNombre");
        _textureButtonEliminar = GetNode<TextureButton>("VBoxContainer/TextureButtonEliminar");

        _textureButtonEliminar.Pressed += () =>
        {
            ApiConnection apiConnection = GetNode<ApiConnection>("/root/ApiConnection");
            int id = Client.Id;

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

            Error error = httpRequest.Request(
                $"{apiConnection.Url}/employees/{id}",
                headers,
                HttpClient.Method.Delete
            );
            if (error != Error.Ok)
            {
                GD.PushError("An error occurred in the HTTP request.");
            }
        };
    }

    private void SetEmployee(Employee value)
    {
        _employee = value;
        _labelNombre.Text = _employee.Name;
    }

    private void HttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        Json json = new Json();
        json.Parse(body.GetStringFromUtf8());

        Godot.Collections.Array responseArray = json.Data.AsGodotArray();
        Dictionary responseDictionary = json.Data.AsGodotDictionary();

        switch (responseCode)
        {
            case 200:
                GD.Print(responseDictionary);
                break;
            case 204:
                GD.Print(responseDictionary);
                _parentContainer.RemoveChild(this);
                QueueFree();
                break;
            default:
                GD.PrintErr(responseDictionary);
                break;
        }
    }
}
