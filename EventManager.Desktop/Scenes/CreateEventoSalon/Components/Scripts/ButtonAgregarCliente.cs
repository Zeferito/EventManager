using System.Text.Json;
using System.Text.Json.Serialization;
using EventManager.Desktop.Api.Entities;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts;

public partial class ButtonAgregarCliente : Button
{
    [Export]
    private VBoxContainer _listaClientesContainer;

    [Export]
    private OptionButton _optionButtonBuscarCliente;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ApiConnection apiConnection = GetNode<ApiConnection>("/root/ApiConnection");

        Pressed += () =>
        {
            int id = (int)_optionButtonBuscarCliente.GetSelectedMetadata();

            foreach (var node1 in _listaClientesContainer.GetChildren())
            {
                var node = (ClienteItemContainer)node1;
                if (node.Client.Id == id)
                {
                    return;
                }
            }

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

            Error error = httpRequest.Request($"{apiConnection.Url}/clients/{id}", headers, HttpClient.Method.Get);

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

                Client client = JsonSerializer.Deserialize<Client>(dictionaryJson, options);

                PackedScene clienteItemContainerScene
                    = ResourceLoader.Load<PackedScene>(
                        "res://Scenes/CreateEventoSalon/Components/cliente_item_container.tscn");

                ClienteItemContainer clienteItemContainer =
                    (ClienteItemContainer)clienteItemContainerScene.Instantiate();

                _listaClientesContainer.AddChild(clienteItemContainer);

                clienteItemContainer.Client = client;
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
}