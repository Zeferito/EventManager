using EventManager.Desktop.Api.Dto;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;
using System;
using System.Text.Json;
using Array = Godot.Collections.Array;

public partial class ButtonRegistrarEmpleado : Button
{
	[Export]
    private LineEdit _lineEditNombre;

    [Export]
    private AdministrarEmpleado _administrarEmpleado;
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

            EmployeeDto employeeDto = new EmployeeDto
            {
                Name = _lineEditNombre.Text,
            };

            string body = JsonSerializer.Serialize(employeeDto);

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
                GD.Print(responseArray);
                _administrarEmpleado.RefreshContainers();
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
