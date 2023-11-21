using System.Text.Json;
using EventManager.Desktop.Api.Dto;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;
public partial class Registrar : Button
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
			string[] headers =
			{
				$"Content-Type: {contentType}"
			};

			RegisterUserDto registerUserDto = new RegisterUserDto
			{
				Username = _lineEditUsuario.ToString(),
				Password = _lineEditPassword.ToString()
			};
			string body = JsonSerializer.Serialize(registerUserDto);

			Error error = httpRequest.Request($"{apiConnection.Url}/auth/register", headers, HttpClient.Method.Post, body);

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
				break;
			default:
				GD.PrintErr(responseDictionary);
				break;
		}
	}
}
