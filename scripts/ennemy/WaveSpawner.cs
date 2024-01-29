using Godot;
using System;
using System.Collections.Generic;

public partial class WaveSpawner : Node2D
{
	[Export]
	public float	SpawnNbr = 10;
	[Export]
	private float	spawnInterval = 5.0f;
	[Export]
	private int		EnemiesToSpawn = 1;
	private	Timer	myTimer;
    private List<PackedScene> possibleSpawns = new List<PackedScene>();
	private Camera2D camera;

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
		// CharacterBody2D new_ennemy;
		possibleSpawns.Add((PackedScene)ResourceLoader.Load("res://scenes/ennemy/ennemy.tscn"));
		possibleSpawns.Add((PackedScene)ResourceLoader.Load("res://scenes/ennemy/bigZombie.tscn"));
		possibleSpawns.Add((PackedScene)ResourceLoader.Load("res://scenes/ennemy/bigArmor.tscn"));
		camera = GetNode<Camera2D>("/root/Node2D/CameraPlayer");
		for (int i = 0; i < SpawnNbr; i++)
		{
			int 		randomIndex = new Random().Next(possibleSpawns.Count);
            PackedScene	NewEnemy = possibleSpawns[randomIndex];
			CharacterBody2D		enemy = (CharacterBody2D)NewEnemy.Instantiate();
            AddChild(enemy);
			enemy.Position = get_random_enemy_position();
		}
		myTimer = GetNode<Timer>("/root/Node2D/WaveSpawner/Timer");
		myTimer.Connect("timeout", new Callable(this, nameof(OnMyTimerTimeout)));
		myTimer.WaitTime = spawnInterval;
		myTimer.Start();
	}

	private void OnMyTimerTimeout()
	{
		if (possibleSpawns.Count > 0)
		{
			for (int i = 0; i < EnemiesToSpawn; i++)
			{
				int randomIndex = new Random().Next(possibleSpawns.Count);
				PackedScene NewEnemy = possibleSpawns[randomIndex];
				CharacterBody2D	enemy = (CharacterBody2D)NewEnemy.Instantiate();
				AddChild(enemy);
				enemy.Position = get_random_enemy_position();
			}
			GD.Print("spawned");
			EnemiesToSpawn++;
			if (spawnInterval == 1.0f)
				spawnInterval = 5;
			spawnInterval = Mathf.Max(spawnInterval * 0.9f, 1.0f);
			myTimer.WaitTime = spawnInterval;
		}
		else
			GD.Print("nothing to spawn");
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
