using Godot;
using System;

public partial class CameraPlayer : Camera2D
{

	CharacterBody2D player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<CharacterBody2D>("../Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = player.Position;
	}
}
