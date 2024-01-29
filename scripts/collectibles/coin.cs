using Godot;
using System;

public partial class coin : StaticBody2D
{
	public override void _Ready()
	{
		var area = GetNode<Area2D>("Area2D");
		area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	public override void _Process(double delta)
	{
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("friend"))
		{
			var pickup = body as Player;
			if (pickup.HasMethod("gainExp"))
			{
				pickup.gainExp(10);
				CallDeferred("queue_free");
			}
		}
	}
}
