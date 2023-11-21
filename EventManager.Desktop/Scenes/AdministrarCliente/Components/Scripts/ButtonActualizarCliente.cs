using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using EventManager.Desktop.Api.Dto;
using EventManager.Desktop.Api.Entities;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace EventManager.Desktop.Scenes.AdministrarCliente.Components.Scripts;

public partial class ButtonActualizarCliente : Button
{
    [Export]
    private LineEdit _lineEditNombre;

    [Export]
    private LineEdit _lineEditTelefono;

    [Export]
    private OptionButton _optionButtonBuscarCliente;

    [Export]
    private AdministrarCliente _administrarCliente;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ApiConnection apiConnection = GetNode<ApiConnection>("/root/ApiConnection");

        Pressed += () =>
        {
            int id = (int)_optionButtonBuscarCliente.GetSelectedMetadata();

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

            Error error = httpRequest.Request(
                $"{apiConnection.Url}/clients/{id}",
                headers,
                HttpClient.Method.Get
            );
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

                ApiConnection apiConnection = GetNode<ApiConnection>("/root/ApiConnection");

                int id = responseDictionary["id"].ToString().ToInt();

                int version = responseDictionary["version"].ToString().ToInt();

                HttpRequest httpRequest = new HttpRequest();
                httpRequest.UseThreads = true;
                AddChild(httpRequest);
                httpRequest.RequestCompleted += HttpRequestCompletedData;
                httpRequest.RequestCompleted += (result, code, strings, body) =>
                {
                    RemoveChild(httpRequest);
                    httpRequest.QueueFree();
                };

                string contentType = "application/json";
                string authToken = apiConnection.AuthDetails.AuthToken;
                string[] header =
                {
                    $"Content-Type: {contentType}",
                    $"Authorization: Bearer {authToken}"
                };


                string phone = null;
                if(!_lineEditTelefono.Text.Equals("")){
                    phone = _lineEditTelefono.Text;
                }

                ClientDto clientDto = new ClientDto
                {
                    Name = _lineEditNombre.Text,
                    Phone = phone,
                    Version = version
                };

                string bodyData = JsonSerializer.Serialize(clientDto);

                Error error = httpRequest.Request(
                    $"{apiConnection.Url}/clients/{id}",
                    header,
                    HttpClient.Method.Put,
                    bodyData
                );
                if (error != Error.Ok)
                {
                    GD.PushError("An error occurred in the HTTP request.");
                }
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

    private void HttpRequestCompletedData (long result, long responseCode, string[] headers, byte[] body){
        Json json = new Json();
        json.Parse(body.GetStringFromUtf8());

        Array responseArray = json.Data.AsGodotArray();
        Dictionary responseDictionary = json.Data.AsGodotDictionary();

        switch (responseCode)
        {
            case 200:
                GD.Print(responseDictionary);
                _administrarCliente.RefreshContainers();
                break;
            default:
                GD.PrintErr(responseDictionary);
                break;
        }
    }
}
