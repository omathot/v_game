using Godot;
using System;

// public float	timer = GetNode<Timer>("/root/Node2D/Player/Timer");

public partial class Player : CharacterBody2D
{
	[Export]
	public float	Speed = 300;
	[Export]
	public float	Health = 100;
	public float	Level = 1;
	public float	Exp = 0;
	// public float	ExpToNextLevel;
	private Timer	myTimer;
	private ProgressBar	healthBar;
	private ProgressBar	ExpBar;
	PackedScene scene = GD.Load<PackedScene>("res://scenes/attacks/projectile.tscn");
	PackedScene orbitals = GD.Load<PackedScene>("res://scenes/attacks/orbit_attack.tscn");
	public const float JumpVelocity = -400.0f;

	public override void _Ready()
	{
		myTimer = GetNode<Timer>("/root/Node2D/Player/Timer");
		myTimer.Connect("timeout", new Callable(this, nameof(OnMyTimerTimeout)));
		
		healthBar = GetNode<ProgressBar>("/root/Node2D/UI/playerElement/healthBar");
		healthBar.MaxValue = Health;
		healthBar.Value = Health;
		
		ExpBar = GetNode<ProgressBar>("/root/Node2D/UI/playerElement/ExpBar");
		ExpBar.MaxValue = 100;
		ExpBar.Value = 0;
		
		myTimer.Start();
	}
	private void OnMyTimerTimeout()
	{
		CharacterBody2D NewAttack;
		NewAttack = (CharacterBody2D)scene.Instantiate();
		GetTree().Root.AddChild(NewAttack);
		NewAttack.GlobalPosition = GlobalPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
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
		Velocity = velocity;
		MoveAndSlide();
	}

	public override void	_Input(InputEvent @event)
	{
		if (@event.IsActionPressed("jump"))
			Speed += 400;
		if (@event.IsActionReleased("jump"))
			Speed -= 400;
	}

	public void	takeDamage(float damage)
	{
		Health -= damage;
		healthBar.Value = Health;
		if (Health <= 0)
			GD.Print("You dead");
	}

	public void	gainExp(float exp)
	{
		GD.Print("gaining EXP");
		Exp += exp;
		ExpBar.Value += exp;
		// ExpToNextLevel -= exp;
		if (ExpBar.Value == 100)
		{
			ExpBar.Value = 0;
			Level++;
			Label	label;
			label = GetNode<Label>("/root/Node2D/UI/playerElement/Level");
			label.Text = $"Lvl. {Level}";
		}
		if (Level == 2)
		{
			// PackedScene orbitals = GD.Load<PackedScene>("res://scenes/attacks/orbit_attack.tscn");
			CallDeferred("addOrbitAttack");
			// CharacterBody2D orbits = (CharacterBody2D)orbitals.Instantiate();
			// GetTree().Root.AddChild(orbits);
		}
	}

	public void	addOrbitAttack()
	{
		CharacterBody2D orbits = (CharacterBody2D)orbitals.Instantiate();
		GetTree().Root.AddChild(orbits);
	}
}
