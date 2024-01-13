using Godot;
using System;

public partial class Ennemy : CharacterBody2D
{
	public const float Speed = 300.0f;

	private CharacterBody2D player;
	public override void _Ready()
	{
		// Assuming the player node is named "Player" and is at the root
		player = GetNode<CharacterBody2D>("../Player");
	}
	public override void _PhysicsProcess(double delta)
	{
		if (player != null)
		{
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			Velocity = direction * Speed;
			MoveAndSlide();
		}
	}
	// public override void _PhysicsProcess(double delta)
	// {
	// 	Vector2 velocity = Velocity;

	// 	// Get the input direction and handle the movement/deceleration.
	// 	// As good practice, you should replace UI actions with custom gameplay actions.
	// 	Vector2 direction = get_player_pos()
	// 	Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
	// 	if (direction != Vector2.Zero)
	// 	{
	// 		velocity.X = direction.X * Speed;
	// 	}
	// 	else
	// 	{
	// 		velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
	// 	}

	// 	Velocity = velocity;
	// 	MoveAndSlide();
	// }
}
