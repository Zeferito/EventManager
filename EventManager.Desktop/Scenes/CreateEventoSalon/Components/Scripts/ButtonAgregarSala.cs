using System.Text.Json;
using System.Text.Json.Serialization;
using EventManager.Desktop.Api.Entities;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts;

public partial class ButtonAgregarSala : Button
{
    [Export]
    private VBoxContainer _listaSalasContainer;

    [Export]
    private OptionButton _optionButtonSala;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ApiConnection apiConnection = GetNode<ApiConnection>("/root/ApiConnection");

        Pressed += () =>
        {
            int id = (int)_optionButtonSala.GetSelectedMetadata();

            foreach (ItemSalaComponent node in _listaSalasContainer.GetChildren())
            {
                if (node.Room.Id == id)
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

            Error error = httpRequest.Request($"{apiConnection.Url}/rooms/{id}", headers, HttpClient.Method.Get);

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

                Room room = JsonSerializer.Deserialize<Room>(dictionaryJson, options);

                PackedScene salaItemComponentScene = ResourceLoader.Load<PackedScene>(
                    "res://Scenes/CreateEventoSalon/Components/item_sala.tscn"
                );

                ItemSalaComponent itemSalaComponent = (ItemSalaComponent)salaItemComponentScene.Instantiate();

                _listaSalasContainer.AddChild(itemSalaComponent);

                itemSalaComponent.Room = room;
                break;
            default:
                GD.PrintErr(responseDictionary);
                break;
        }
    }
}