using Godot;

namespace hakim.scripts;

public partial class Lamp : Sprite2D
{
	private PointLight2D _light;
	private bool _isOn = true;
	private bool _isTransitioning;
	private const float TransitionSpeed = 4.0f;
	private float _currentEnergy;
	private float _targetEnergy;
	private float _currentAlpha;
	private float _targetAlpha;

	public override void _Ready()
	{
		_light = GetNode<PointLight2D>("PointLight2D");
		_currentEnergy = 2.0f;
		_targetEnergy = 2.0f;
		_currentAlpha = 1.0f;
		_targetAlpha = 1.0f;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
			{
				var clickPosition = mouseButton.Position;
				if (GetRect().HasPoint(ToLocal(clickPosition)))
				{
					ToggleLight();
				}
			}
		}
	}

	private void ToggleLight()
	{
		_isTransitioning = true;
		if (_isOn)
		{
			_isOn = false;
			_targetEnergy = 0.3f;
			_targetAlpha = 0.1f;
		}
		else
		{
			_isOn = true;
			_targetEnergy = 2.0f;
			_targetAlpha = 1.0f;
		}
	}

	public override void _Process(double delta)
	{
		if (_isTransitioning)
		{
			_currentEnergy = Mathf.MoveToward(_currentEnergy, _targetEnergy, TransitionSpeed * (float)delta);
			_currentAlpha = Mathf.MoveToward(_currentAlpha, _targetAlpha, TransitionSpeed * (float)delta);

			Modulate = new Color(1.5f, 1.5f, 1.5f, _currentAlpha);
			_light.SetEnergy(_currentEnergy);

			if (Mathf.IsEqualApprox(_currentEnergy, _targetEnergy) && Mathf.IsEqualApprox(_currentAlpha, _targetAlpha))
			{
				_isTransitioning = false;
			}
		}
	}
}