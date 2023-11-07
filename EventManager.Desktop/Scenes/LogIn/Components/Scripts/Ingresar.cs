using EventManager.Desktop.Api.Dto;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;
using System;
using System.Text.Json;

public partial class Ingresar : Button
{
	[Export]
	private LineEdit _lineEditUsuario;
	[Export]
	private LineEdit _lineEditPassword;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ApiConnection apiConnection = GetNode<ApiConnection>("/root/ApiConnection");

		Pressed += () =>
		{
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

			LoginRequestDto loginRequestDto = new LoginRequestDto
			{
				Identifier = _lineEditUsuario.Text,
				Password = _lineEditPassword.Text
			};
			string body = JsonSerializer.Serialize(loginRequestDto);
			Error error = httpRequest.Request($"{apiConnection.Url}/auth/login", headers, HttpClient.Method.Post, body);

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
			case 200:
				string token = responseDictionary["accessToken"].ToString();
				ApiConnection apiConnection = GetNode<ApiConnection>("/root/ApiConnection");
				apiConnection.AuthDetails.AuthToken = token;
				GetTree()
				.ChangeSceneToFile("res://Scenes/Inicio/scena_pantalla_inicio.tscn");
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
