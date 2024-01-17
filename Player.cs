using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public	Node2D	caboom;

	// public override void _Process(double delta)
	// {
		
		
	// }
	PackedScene scene = GD.Load<PackedScene>("res://caboom_attack.tscn");

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("jump"))
		{
			Node instant = scene.Instantiate();
			AddChild(instant);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		// GD.Print("Hello, Godot!");
		// AddChild(GetNode("res://caboom_attack.tscn"));
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}
		
		// if (direction != Vector2.Zero)
		// {
			
		// }
		// else
		// {
		// }
		Velocity = velocity;
		MoveAndSlide();
	}
}
