using EventManager.Database.Models.Entities;
using Godot;

public partial class AgregableEventoItemComponent : HBoxContainer
{
	private VBoxContainer _parentContainer;
	private Label _labelNombreEvento;
	private Label _labelDescripcionEvento;
	private Label _labelFechaInicio;
	private Label _labelFechaTermino;
	private Label _labelUsuarioId;

	private Evento _evento;

	public Evento Evento
	{
		get => _evento;
		set => SetEvento(value);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_parentContainer = GetParent<VBoxContainer>();
		_labelNombreEvento = GetNode<Label>("LabelNombreEvento");
		_labelDescripcionEvento = GetNode<Label>("LabelDescripcionEvento");
		_labelFechaInicio = GetNode<Label>("LabelFechaInicio");
		_labelFechaTermino = GetNode<Label>("LabelFechaTermino");
		_labelUsuarioId = GetNode<Label>("LabelUsuario");
	}

	private void SetEvento(Evento value)
	{
		_evento = value;
		_labelNombreEvento.Text = value.Nombre;
		_labelDescripcionEvento.Text = value.Descripcion;
		_labelFechaInicio.Text = value.FechaInicio.ToString();
		_labelFechaTermino.Text = value.FechaTermino.ToString();
		_labelUsuarioId.Text = value.Usuario.Id.ToString();
	}
}
