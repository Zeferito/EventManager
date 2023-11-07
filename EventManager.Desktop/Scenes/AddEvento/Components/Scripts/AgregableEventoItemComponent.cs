using EventManager.Desktop.Api.Entities;
using Godot;

public partial class AgregableEventoItemComponent : HBoxContainer
{
	private VBoxContainer _parentContainer;
	private Label _labelNombreEvento;
	private Label _labelDescripcionEvento;
	private Label _labelFechaInicio;
	private Label _labelFechaTermino;
	private Label _labelUsuarioId;

	private Event _event;

	public Event Event
	{
		get => _event;
		set => SetEvent(value);
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

	private void SetEvent(Event value)
	{
		_event = value;
		_labelNombreEvento.Text = value.Name;
		_labelDescripcionEvento.Text = value.Description;
		_labelFechaInicio.Text = value.StartDate;
		_labelFechaTermino.Text = value.EndDate;
		_labelUsuarioId.Text = value.User.Id.ToString();
	}
}
