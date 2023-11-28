using System.Text.Json;
using System.Text.Json.Serialization;
using EventManager.Desktop.Api.Dto;
using EventManager.Desktop.Api.Entities;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts;

public partial class ButtonRegistrarCliente : Button
{
    [Export]
    private LineEdit _lineEditNombre;

    [Export]
    private LineEdit _lineEditTelefono;
    [Export]
    private OptionButtonBuscarCliente _optionButtonBuscarCliente;
    [Export]
    private VBoxContainer _listaClientesContainer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Pressed += () =>
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

            ClientDto clientDto = new ClientDto
            {
                Name = _lineEditNombre.Text,
                Phone = _lineEditTelefono.Text
            };

            string body = JsonSerializer.Serialize(clientDto);

            Error error = httpRequest.Request($"{apiConnection.Url}/clients", headers, HttpClient.Method.Post, body);

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

        Array responseArray = json.Data.AsGodotArray();
        Dictionary responseDictionary = json.Data.AsGodotDictionary();

        switch (responseCode)
        {
            case 201:
                GD.Print(responseDictionary);

                string dictionaryJson = Json.Stringify(responseDictionary);

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters =
                        {
                            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                        },
                };

                Client client = JsonSerializer.Deserialize<Client>(dictionaryJson, options);

                PackedScene _clienteItemContainerScene = ResourceLoader.Load<PackedScene>(
                        "res://Scenes/CreateEventoSalon/Components/cliente_item_container.tscn");

                ClienteItemContainer clienteItemContainer = (ClienteItemContainer)_clienteItemContainerScene.Instantiate();

                _listaClientesContainer.AddChild(clienteItemContainer);

                clienteItemContainer.Client = client;

                _optionButtonBuscarCliente.Refresh();
                break;
            default:
                GD.PrintErr(responseDictionary);
                break;
        }
    }
    
}