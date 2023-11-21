using EventManager.Desktop.Api.Entities;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public partial class OptionButtonBuscarEmpleado : OptionButton
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

				for (int i = 0; i < responseArray.Count; i++)
				{
					Dictionary dictionaryItem = responseArray[i].AsGodotDictionary();

					string dictionaryJson = Json.Stringify(dictionaryItem);

					JsonSerializerOptions options = new JsonSerializerOptions
					{
						Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
					};

					Employee employee = JsonSerializer.Deserialize<Employee>(dictionaryJson, options);

					AddItem(employee.Name, i);
					SetItemMetadata(i, employee.Id);
				}

				break;
			default:
				GD.PrintErr(responseDictionary);
				break;
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
			$"{apiConnection.Url}/employees",
			headers,
			HttpClient.Method.Get
		);

		if (error != Error.Ok)
		{
			GD.PushError("An error occurred in the HTTP request.");
		}
	}
}
