using Godot;
using System;

public partial class WaveSpawner : Node2D
{

	private Camera2D camera;
	// int		number_enemy;
	PackedScene scene = GD.Load<PackedScene>("res://ennemy.tscn");

	// GetViewport().GetVisibleRect().Size
	private Vector2 get_random_enemy_position()
	{
		float x, y;
		float	half_screen_size_x = GetViewport().GetVisibleRect().Size.X/2; 
		float	half_screen_size_y = GetViewport().GetVisibleRect().Size.Y/2; 

		x = GD.RandRange(1500, -1500);
		if (x < half_screen_size_x  && x > -half_screen_size_x)
		{
			y = GD.RandRange(1500, -1500);
			while (y < half_screen_size_y && y > -half_screen_size_y)
				y = GD.RandRange(1500, -1500);
		}
		else
			y = GD.RandRange(1500, -1500);
		Vector2 return_vec = new Vector2(x, y);
		return (return_vec);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CharacterBody2D new_ennemy;
		camera = GetNode<Camera2D>("/root/Node2D/CameraPlayer");
		// number_enemy = 10;
		for (int i = 0; i < 10; i++)
		{
			new_ennemy = (CharacterBody2D)scene.Instantiate();
			AddChild(new_ennemy);
			new_ennemy.Position = get_random_enemy_position();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
