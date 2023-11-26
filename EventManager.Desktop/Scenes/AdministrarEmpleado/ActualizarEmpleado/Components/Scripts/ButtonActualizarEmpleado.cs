using EventManager.Desktop.Api.Dto;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;
using System;
using System.Text.Json;
using Array = Godot.Collections.Array;

public partial class ButtonActualizarEmpleado : Button
{
    [Export]
    private LineEdit _lineEditNombre;

    [Export]
    private OptionButton _optionButtonBuscarEmpleado;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ApiConnection apiConnection = GetNode<ApiConnection>("/root/ApiConnection");

        Pressed += () =>
        {
            int id = (int)_optionButtonBuscarEmpleado.GetSelectedMetadata();

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

            EmployeeDto employeeDto = new EmployeeDto { Name = _lineEditNombre.Text, };

            string body = JsonSerializer.Serialize(employeeDto);

            Error error = httpRequest.Request(
                $"{apiConnection.Url}/employees/{id}",
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

                EmployeeDto employeeDto = new EmployeeDto
                {
                    Name = _lineEditNombre.Text,
                    Version = version
                };

                string bodyData = JsonSerializer.Serialize(employeeDto);

                Error error = httpRequest.Request(
                    $"{apiConnection.Url}/employees/{id}",
                    header,
                    HttpClient.Method.Put,
                    bodyData
                );
                if (error != Error.Ok)
                {
                    GD.PushError("An error occurred in the HTTP request.");
                }
                break;
            default:
                GD.PrintErr(responseDictionary);
                break;
        }
    }

    private void HttpRequestCompletedData(
        long result,
        long responseCode,
        string[] headers,
        byte[] body
    )
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
            default:
                GD.PrintErr(responseDictionary);
                break;
        }
    }
}
