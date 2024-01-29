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
		timer.WaitTime = 0.5f;
		timer.OneShot = true;
		Callable callable = new Callable(this, MethodName._on_timer_timeout);
		timer.Connect("timeout", callable);
		timer.Start();
	}
	public void _on_timer_timeout()
	{
		QueueFree();
	}

	Godot.Collections.Array<Node2D>	Bodies;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Bodies = GetChild<Area2D>(1).GetOverlappingBodies();
		foreach (var item in Bodies)
		{
			GD.Print("see this");
			GD.Print(item.Name);
			if (item.Name == "Ennemy")
				item.QueueFree();
		}
		GD.PrintRich(timer.WaitTime);
		// if ( += )
		// {
		// 	GD.Print("died");
		// 	QueueFree();
		// }
	}
}
