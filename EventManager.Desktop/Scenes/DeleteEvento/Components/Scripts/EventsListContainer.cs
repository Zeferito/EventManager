using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using EventManager.Desktop.Api.Entities;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace EventManager.Desktop.Scenes.DeleteEvento.Components.Scripts;

public partial class EventsListContainer : VBoxContainer
{
    public override void _Ready()
    {
        foreach (Node node in GetChildren())
        {
            RemoveChild(node);
            node.QueueFree();
        }

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

        Error error = httpRequest.Request($"{apiConnection.Url}/events", headers, HttpClient.Method.Get);

        if (error != Error.Ok)
        {
            GD.PushError("An error occurred in the HTTP request.");
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
                GD.Print(responseArray);

                foreach (Node node in GetChildren())
                {
                    RemoveChild(node);
                    node.QueueFree();
                }

                foreach (var item in responseArray)
                {

                    Dictionary itemDictionary = item.AsGodotDictionary();
                    string dictionaryJson = Json.Stringify(itemDictionary);

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        Converters =
                        {
                            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                        },
                    };

                    Event evt = JsonSerializer.Deserialize<Event>(dictionaryJson, options);

                    PackedScene agregableEventoItemComponent = ResourceLoader.Load<PackedScene>(
                        "res://Scenes/DeleteEvento/Components/agregable_evento_item_component.tscn"
                    );

                    AgregableEventoItemComponent itemEventoComponent = (AgregableEventoItemComponent)
                        agregableEventoItemComponent.Instantiate();

                    AddChild(itemEventoComponent);

                    itemEventoComponent.Event = evt;
                }

                break;
            case 401:
                GD.Print(responseDictionary);
                break;
            default:
                GD.Print(responseDictionary);
                break;
        }
    }
}