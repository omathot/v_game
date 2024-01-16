using Godot;
using System;

public partial class WaveSpawner : Node2D
{

	private Camera2D camera;
	int		number_enemy;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		camera = GetNode<Camera2D>("../CameraPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// if (number_enemy < 5)
			
	}
}
