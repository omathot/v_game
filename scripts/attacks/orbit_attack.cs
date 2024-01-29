using Godot;
using System;

public partial class orbit_attack : CharacterBody2D
{
	[Export]
	public float		orbitSpeed = 1.0f;
	[Export]
	private float		orbitRadius = 100.0f;
	[Export]
	private float		Damage = 10.0f;
	private double		Angle;
	public const float JumpVelocity = -400.0f;
	public CharacterBody2D			Player;

	public override void _Ready()
	{
		Area2D	area = GetNode<Area2D>("Area2D");
		area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		Player = GetNode<CharacterBody2D>("/root/Node2D/Player");
	}

	public override void _PhysicsProcess(double delta)
	{
		// Update the angle based on the orbit speed
		Angle += orbitSpeed * (float)delta;
		Angle = Angle % (2 * Mathf.Pi); // Keep the angle in the range [0, 2*Pi]

		// Calculate the new position
		Vector2 newPosition = new Vector2(
			Player.GlobalPosition.X + orbitRadius * Mathf.Cos((float)Angle),
			Player.GlobalPosition.Y + orbitRadius * Mathf.Sin((float)Angle)
		);

		// Set the projectile's position
		GlobalPosition = newPosition;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("enemy"))
		{
			if (body.HasMethod("takeDamage"))
			{
				GD.Print("orb dealing damage");
				Ennemy enemy = body as Ennemy;
				enemy.takeDamage(Damage);
			}
		}
	}
}
