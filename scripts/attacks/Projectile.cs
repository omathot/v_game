using Godot;
using System;
using System.Collections.Generic;

public partial class Projectile : CharacterBody2D
{
	[Export]
	public float	Speed = 350;
	[Export]
	public float	Damage = 10;
	private Vector2 velocity;
	float randomAngle;
	private Node2D target;
	private List<Node2D> bodiesInArea = new List<Node2D>();
	private RandomNumberGenerator rng = new RandomNumberGenerator();


	public override void _Ready()
	{
		var area = GetNode<Area2D>("Area2D");
		area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		area.Connect("body_exited", new Callable (this, nameof(OnBodyExited)));
		randomAngle = rng.RandfRange(0, 2 * Mathf.Pi);
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("enemy"))
		{
			bodiesInArea.Add(body);
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body.IsInGroup("enemy"))
		{
			bodiesInArea.Remove(body);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction;
		var nearestBody = FindNearestBody(GlobalPosition);
		if (nearestBody != null)
		{
			direction = (target.GlobalPosition - GlobalPosition).Normalized();
		}
		else
		{	
			direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
		}
		Velocity = direction * Speed;
		MoveAndSlide();
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision2D collision = GetSlideCollision(i);
			if (collision != null)
			{
				var collider = collision.GetCollider() as Node;
				if (collider != null && collider.IsInGroup("enemy"))
				{
					var enemy = collider as Ennemy;
					if (enemy != null && enemy.HasMethod("takeDamage"))
					{
						enemy.takeDamage(Damage);
						QueueFree();
					}
				}
			}
		}
	}

	private Node2D FindNearestBody(Vector2 toPosition)
	{
		Node2D nearestBody = null;
		float nearestDistance = float.MaxValue;

		foreach (var body in bodiesInArea)
		{
			float distance = toPosition.DistanceTo(body.GlobalPosition);
			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				nearestBody = body;
			}
		}
		target = nearestBody;
		return (nearestBody);
	}
}


