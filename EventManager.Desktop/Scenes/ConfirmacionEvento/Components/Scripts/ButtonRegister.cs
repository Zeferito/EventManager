using EventManager.Desktop.Api.Dto;
using EventManager.Desktop.Api.Entities;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace EventManager.Desktop.Scenes.ConfirmacionEvento.Components.Scripts;

public partial class ButtonRegister : Button
{
    public override void _Ready()
    {
        Global global = GetNode<Global>("/root/Global");


        Pressed += () =>
        {
            Global global = GetNode<Global>("/root/Global");
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

            EventDto eventDto = global.EventToSend;
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
            case 204:
                GD.Print(responseDictionary);
                GetTree().ChangeSceneToFile("res://Scenes/Inicio/scena_pantalla_inicio.tscn");
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