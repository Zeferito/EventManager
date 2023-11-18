using EventManager.Desktop.Api.Entities;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventManager.Desktop.Scenes.AdministrarCliente.Components.Scripts;

public partial class ListaClientesContainer : VBoxContainer
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Refresh();
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
                GD.Print(responseArray);

                Clear();

                for (int i = 0; i < responseArray.Count; i++)
                {
                    Dictionary dictionaryItem = responseArray[i].AsGodotDictionary();

                    string dictionaryJson = Json.Stringify(dictionaryItem);

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                    };

                    Client client = JsonSerializer.Deserialize<Client>(dictionaryJson, options);

                    PackedScene _borrarClienteComponent = ResourceLoader.Load<PackedScene>(
                        "res://Scenes/AdministrarCliente/Components/borrar_cliente_component.tscn"
                    );
                    BorrarClienteComponent borrarClienteComponent = (BorrarClienteComponent)
                        _borrarClienteComponent.Instantiate();

                    AddChild(borrarClienteComponent);
                    borrarClienteComponent.Client = client;
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

    public void Clear()
    {
        foreach (Node node in GetChildren())
        {
            RemoveChild(node);
            node.QueueFree();
        }
    }

    public void Refresh()
    {
        Clear();

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
        string[] headers = { $"Content-Type: {contentType}", $"Authorization: Bearer {authToken}" };

        Error error = httpRequest.Request(
            $"{apiConnection.Url}/clients",
            headers,
            HttpClient.Method.Get
        );

        if (error != Error.Ok)
        {
            GD.PushError("An error occurred in the HTTP request.");
        }
    }
}
