using Godot;
using System;

public partial class Ennemy : CharacterBody2D
{
	[Export]
	public float	Speed = 150;
	[Export]
	public float	Health = 10;
	[Export]
	public float	Damage = 10;
	private bool	canDealDamage = true;
	private CharacterBody2D player;
	PackedScene scene = GD.Load<PackedScene>("res://scenes/collectibles/coin.tscn");

	public override void _Ready()
	{
		// Assuming the player node is named "Player" and is at the root
		player = GetNode<CharacterBody2D>("/root/Node2D/Player");
	}
	public override void _PhysicsProcess(double delta)
	{
		if (player != null)
		{
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			Velocity = direction * Speed;
		}
		MoveAndSlide();
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision2D collision = GetSlideCollision(i);
			if (collision != null)
			{
				var collider = collision.GetCollider() as Node;
				if (collider != null && collider.IsInGroup("friend"))
				{
					var enemy = collider as Player;
					if (enemy != null && enemy.HasMethod("takeDamage") && canDealDamage)
					{
						enemy.takeDamage(Damage);
						canDealDamage = false;
						damageCooldown();
					}
				}
			}
		}
	}

	private async void damageCooldown()
	{
		await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
		canDealDamage = true;
	}

	public void	takeDamage(float damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			
			CallDeferred("spawnCollectible");
			// Node2D instance = (Node2D)scene.Instantiate();
			// GetTree().Root.AddChild(instance);
			// instance.Position = new Vector2(Position.X, Position.Y);
			CallDeferred("queue_free");
		}
	}

	public void	spawnCollectible()
	{
		Node2D instance = (Node2D)scene.Instantiate();
		GetTree().Root.AddChild(instance);
		instance.Position = new Vector2(Position.X, Position.Y);
	}
}
