using Godot;
using System;

public partial class Caboom_attack : Node2D
{
	public Timer timer = new Timer();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GetChild<Sprite2D>(0).Visible = false;
		AddChild(timer);
		timer.WaitTime = 2f;
		timer.Start();
	}

	Godot.Collections.Array<Node2D>	Bodies;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Bodies = GetChild<Area2D>(1).GetOverlappingBodies();
		foreach (var item in Bodies)
		{
			if (item.Name == "Ennemy")
				item.QueueFree();
		}
		if (timer.IsStopped())
		QueueFree();
	}
}
