using System.Text.Json;
using System.Text.Json.Serialization;
using EventManager.Desktop.Api.Entities;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;
using System;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts;

public partial class AgregarButton : Button
{
	[Export]
	private VBoxContainer _listaEmpleadosContainer;

	[Export]
	private OptionButton _optionButtonEmpleados;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ApiConnection apiConnection = GetNode<ApiConnection>("/root/ApiConnection");

		Pressed += () =>
		{
			int id = (int)_optionButtonEmpleados.GetSelectedMetadata();

			foreach (var node1 in _listaEmpleadosContainer.GetChildren())
			{
				var node = (EmpleadoItemComponent)node1;
				if (node.Employee.Id == id)
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

			Error error = httpRequest.Request($"{apiConnection.Url}/employees/{id}", headers, HttpClient.Method.Get);

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
				GD.Print(responseDictionary);

				string dictionaryJson = Json.Stringify(responseDictionary);

				JsonSerializerOptions options = new JsonSerializerOptions
				{
					Converters =
					{
						new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
					},
				};

				Employee employee = JsonSerializer.Deserialize<Employee>(dictionaryJson, options);

				PackedScene empleadoItemComponentScene
					= ResourceLoader.Load<PackedScene>(
						"res://Scenes/CreateEventoSalon/Components/empleado_item_component.tscn");

				EmpleadoItemComponent
					empleadoItemComponent =
						(EmpleadoItemComponent)
						empleadoItemComponentScene.Instantiate();

				_listaEmpleadosContainer.AddChild(empleadoItemComponent);

				empleadoItemComponent.Employee = employee;
				break;
			default:
				GD.PrintErr(responseDictionary);
				break;
		}
	}
}
