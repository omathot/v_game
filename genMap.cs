using Godot;
using System;

public partial class genMap : TileMap
{
	FastNoiseLite humid_Noise = new FastNoiseLite();
	FastNoiseLite temp_Noise = new FastNoiseLite();
	FastNoiseLite altitude_Noise = new FastNoiseLite();

	private Camera2D camera;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		humid_Noise.Seed = (int)GD.Randi();
		temp_Noise.Seed = (int)GD.Randi();
		altitude_Noise.Seed = (int)GD.Randi();
		altitude_Noise.Frequency = 0.005f;
		temp_Noise.Frequency = 0.05f;
		humid_Noise.Frequency = 0.05f;
		camera = GetNode<Camera2D>("../CameraPlayer");
	}

	Godot.Collections.Array<Vector2I> location_Sand = new Godot.Collections.Array<Vector2I>();
	Godot.Collections.Array<Vector2I> location_water = new Godot.Collections.Array<Vector2I>();
	Godot.Collections.Array<Vector2I> location_grass = new Godot.Collections.Array<Vector2I>();

	/*
	Terain Types:
	Sand
	Dirt
	Grass
	Lake
	Ocean
	*/

	private string	get_type_terain(int x, int y)
	{
		string name;

		if (altitude_Noise.GetNoise2D(x, y) < -0.4 && humid_Noise.GetNoise2D(x, y) > 0.3)
		{
			name = "Ocean";
		}
		else if (altitude_Noise.GetNoise2D(x, y) < -0.3 && humid_Noise.GetNoise2D(x, y) > 0.3)
		{
			name = "Lake";
		}
		else if (temp_Noise.GetNoise2D(x, y) > 0.3 && humid_Noise.GetNoise2D(x, y) < -0.3)
		{
			name = "Sand";
		}
		else if (temp_Noise.GetNoise2D(x, y) > -0.3 && humid_Noise.GetNoise2D(x, y) > 0.3)
		{
			name = "Grass";
		}
		else
			name = "Dirt";
		return name;
	}
	
	private bool	have_type_around_cell(string type, int x, int y)
	{
		if (type == get_type_terain(x - 1, y)
		||	type == get_type_terain(x + 1, y)
		||	type == get_type_terain(x, y - 1)
		||	type == get_type_terain(x, y + 1)
		||	type == get_type_terain(x + 1, y - 1)
		||	type == get_type_terain(x + 1, y + 1)
		||	type == get_type_terain(x - 1, y - 1)
		||	type == get_type_terain(x - 1, y + 1))
			return (true);
		return (false);
	}

	private Vector2I get_vector_of_set_depending_on_type(string type, int x, int y, int vecX, int vecY)
	{
		int	change_x = 0;
		int	change_y = 0;
		if (type == get_type_terain(x - 1, y))
			change_x--;
		if (type == get_type_terain(x + 1, y))
			change_x++;
		if (type == get_type_terain(x, y - 1))
			change_y--;
		if (type == get_type_terain(x, y + 1))
			change_y++;
		if (change_x != 0 || change_y != 0)
			return new Vector2I(vecX + change_x, vecY + change_y);
		if (type == "Grass")
		{
			if (type == get_type_terain(x + 1, y - 1))
				return new Vector2I(0, 4);
			if (type == get_type_terain(x + 1, y + 1))
				return new Vector2I(0, 3);
			if (type == get_type_terain(x - 1, y - 1))
				return new Vector2I(1, 4);
			// if (type == get_type_terain(x - 1, y + 1))
				return new Vector2I(1, 3);
		}
		return new Vector2I(5, 12);
	}

	private Vector2I Get_right_cell_type(int x, int y)
	{
		Vector2I	type;

		if (get_type_terain(x, y) == "Grass")
			type = new Vector2I(4, 0);
		else if (get_type_terain(x, y) == "Dirt")
		{
			if (have_type_around_cell("Grass", x, y))
			{
				type = get_vector_of_set_depending_on_type("Grass", x, y, 1, 1);
			}
			else
			type = new Vector2I(1, 1);
		}
		else if (get_type_terain(x, y) == "Sand")
			type = new Vector2I(6, 1);
		else if (get_type_terain(x, y) == "Ocean")
			type = new Vector2I(6, 6);
		else
			type = new Vector2I(1, 6);
		return (type);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		for (int i = ((int)(camera.Position.X) - 550) / 32; i < ((int)(camera.Position.X) + 550) / 32; i++)
		{
			for (int j = ((int)(camera.Position.Y) - 360) / 32; j  < ((int)(camera.Position.Y) + 360) / 32; j++)
			{
				Vector2I curent_tile = new Vector2I(i, j);
				SetCell(0, curent_tile, 1, Get_right_cell_type(i, j));
			}
		}
	}
}
