using Godot;

namespace hakim.scripts;

public partial class Lamp : Sprite2D
{
    private const float TransitionSpeed = 4.0f;
    private const float LightOnEnergy = 2.0f;
    private const float LightOffEnergy = 0.3f;
    private const float LightOnAlpha = 1.0f;
    private const float LightOffAlpha = 0.1f;
    private const float LightOnScale = 1.5f;
    private const float LightOffScale = 1.0f;
    private const float ModulateIntensity = 1.5f;

    private PointLight2D _light;
    private bool _isOn = true;
    private bool _isTransitioning;
    private float _currentEnergy;
    private float _targetEnergy;
    private float _currentAlpha;
    private float _targetAlpha;
    private float _currentScale;
    private float _targetScale;

    public override void _Ready()
    {
        _light = GetNode<PointLight2D>("PointLight2D");
        _currentEnergy = LightOnEnergy;
        _targetEnergy = LightOnEnergy;
        _currentAlpha = LightOnAlpha;
        _targetAlpha = LightOnAlpha;
        _currentScale = LightOnScale;
        _targetScale = LightOnScale;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventMouseButton mouseButton || 
            mouseButton.ButtonIndex != MouseButton.Left || 
            !mouseButton.Pressed) return;

        if (GetRect().HasPoint(ToLocal(mouseButton.Position)))
        {
            ToggleLight();
        }
    }

    private void ToggleLight()
    {
        _isTransitioning = true;
        _isOn = !_isOn;
        
        (_targetEnergy, _targetAlpha, _targetScale) = _isOn 
            ? (LightOnEnergy, LightOnAlpha, LightOnScale)
            : (LightOffEnergy, LightOffAlpha, LightOffScale);
    }

    public override void _Process(double delta)
    {
        if (!_isTransitioning) return;

        var deltaSpeed = TransitionSpeed * (float)delta;
        _currentEnergy = Mathf.MoveToward(_currentEnergy, _targetEnergy, deltaSpeed);
        _currentAlpha = Mathf.MoveToward(_currentAlpha, _targetAlpha, deltaSpeed);
        _currentScale = Mathf.MoveToward(_currentScale, _targetScale, deltaSpeed);

        Modulate = new Color(ModulateIntensity, ModulateIntensity, ModulateIntensity, _currentAlpha);
        _light.SetEnergy(_currentEnergy);
        _light.TextureScale = _currentScale;

        if (IsTransitionComplete())
        {
            _isTransitioning = false;
        }
    }

    private bool IsTransitionComplete() => 
        Mathf.IsEqualApprox(_currentEnergy, _targetEnergy) &&
        Mathf.IsEqualApprox(_currentAlpha, _targetAlpha) &&
        Mathf.IsEqualApprox(_currentScale, _targetScale);
}