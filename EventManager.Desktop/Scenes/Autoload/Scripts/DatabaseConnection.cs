using Godot;
using System;

public partial class DatabaseConnection : Node
{
	public string ConnectionString { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!Load())
		{
			Save();
		}
	}

	public void Save()
	{
		ConfigFile config = new ConfigFile();

		config.SetValue(
			"connection",
			"url",
			"Server=localhost;Port=3306;Database=libraryTest;Uid=root;Pwd=password;"
		);

		Error err = config.Save("user://database.cfg");

		GD.Print("Database Config Save Result: " + err);

		ConnectionString = "Server=localhost;Port=3306;Database=libraryTest;Uid=root;Pwd=password;";
	}

	private bool Load()
	{
		ConfigFile config = new ConfigFile();

		Error err = config.Load("user://database.cfg");

		GD.Print("Database Config Load Result: " + err);

		if (err != Error.Ok)
		{
			return false;
		}

		if (!config.HasSectionKey("connection", "url"))
		{
			return false;
		}

		ConnectionString = (string)config.GetValue("connection", "url");

		return true;
	}
}
