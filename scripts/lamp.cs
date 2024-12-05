using Godot;

namespace hakim.scripts;

public partial class lamp : Sprite2D
{
	private PointLight2D _light;
	private bool _isOn = true;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_light = GetParent().GetNode<PointLight2D>("PointLight2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// if left mouse button is pressed
		if (Input.IsActionJustPressed("left_click"))
		{
			if (_isOn)
			{
				_isOn = false;
				Modulate = new Color(1, 1, 1, 0.1f);
				_light.SetEnergy(0.3f);
			}
			else
			{
				_isOn = true;
				Modulate = new Color(1.5f, 1.5f, 1.5f, 1);
				_light.SetEnergy(2);
			}
		}
	}
}