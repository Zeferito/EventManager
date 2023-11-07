using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;

namespace EventManager.Desktop.Scenes.DeleteEvento.Components.Scripts;

public partial class RemoveEventButton : Button
{
    private VBoxContainer _parentListContainer;

    private AgregableEventoItemComponent _parentContainer;

    public override void _Ready()
    {
        _parentListContainer = GetParent<HBoxContainer>().GetParent<VBoxContainer>().GetParent<VBoxContainer>();

        _parentContainer = GetParent<HBoxContainer>().GetParent<AgregableEventoItemComponent>();

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

            int id = _parentContainer.Event.Id;

            Error error = httpRequest.Request($"{apiConnection.Url}/events/{id}", headers, HttpClient.Method.Delete);

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
            case 204:
                _parentListContainer.RemoveChild(_parentContainer);
                _parentContainer.QueueFree();
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