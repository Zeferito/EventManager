using EventManager.Database.Models.Entities;
using Godot;
using System;
using System.Text.Json;

public partial class ProcessUserInput : Node
{
    [Export]
    private LineEdit _lineEditTitleEvent;

    [Export]
    private TextEdit _textEditDescriptionEvent;

    [Export]
    private LineEdit _lineEditStartDateEvent;

    [Export]
    private LineEdit _lineEditEndDateEvent;

    [Export]
    private Button _buttonConfirm;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _buttonConfirm.Pressed += () =>
        {
            Evento evento = new Evento();

            if (_lineEditTitleEvent.Text == "")
            {
                GD.Print("No se puede crear un evento sin nombre");
                return;
            }

            if (_textEditDescriptionEvent.Text == "")
            {
                GD.Print("No se puede crear un evento sin descripción");
                return;
            }

            if (_lineEditStartDateEvent.Text == "")
            {
                GD.Print("No se puede crear un evento sin fecha de inicio");
                return;
            }

            if (_lineEditEndDateEvent.Text == "")
            {
                GD.Print("No se puede crear un evento infinito wtf");
                return;
            }

            evento.Nombre = _lineEditTitleEvent.Text;
            evento.Descripcion = _textEditDescriptionEvent.Text;

            DateTime startDateValue;
            if (!DateTime.TryParse(_lineEditStartDateEvent.Text, out startDateValue))
            {
                GD.Print("No se puede crear un evento con una fecha de inicio inválida");
                return;
            }

            evento.FechaInicio = startDateValue;

            DateTime endDateValue;
            if (!DateTime.TryParse(_lineEditEndDateEvent.Text, out endDateValue))
            {
                GD.Print("No se puede crear un evento con una fecha de inicio inválida");
                return;
            }

            evento.FechaTermino = endDateValue;

            string jsonString = JsonSerializer.Serialize(evento);

            GD.Print(jsonString);

            // GetTree()
            //     .ChangeSceneToFile(
            //         "res://Scenes/ConfirmacionEvento/confirmacion_evento_scene.tscn"
            //     );
        };
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
