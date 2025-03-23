using Godot;
using System;

public partial class Player : CharacterBody3D {
	public const float Speed = 250.0f;
	public const float JumpVelocity = 1.5f;

	public const float VerticalSensitivity = 0.001f;
	public const float HorizontalSensitivity = 0.001f;

	[Export]
	public Camera3D Camera;

	private bool inPrecisionMode = false;

    public override void _Ready() {
		Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta) {
		HandleMovement(delta);
	}

    public override void _Input(InputEvent @event) {
		if (!inPrecisionMode && @event is InputEventMouseMotion m) {
			RotateY(-1 * m.ScreenRelative.X * HorizontalSensitivity);
			Camera.RotateX(-1 * m.ScreenRelative.Y * VerticalSensitivity);
			Camera.Rotation = new Vector3(Mathf.Clamp(Camera.Rotation.X, -0.5f * Mathf.Pi, 0.5f * Mathf.Pi), 0, 0);
		} else if (@event.IsActionPressed("precision_mode") && !inPrecisionMode) {
			EnterPrecisionMode();
		} else if (@event.IsActionReleased("precision_mode") && inPrecisionMode) {
			ExitPrecisionMode();
		}
    }

    private void HandleMovement(double delta) {
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor()) {
			velocity += GetGravity() * (float)delta;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("player_left", "player_right", "player_forward", "player_backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero) {
			velocity.X = direction.X * Speed * (float) delta;
			velocity.Z = direction.Z * Speed * (float) delta;
		} else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed * (float) delta);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed * (float) delta);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void EnterPrecisionMode() {
		inPrecisionMode = true;
		Input.MouseMode = Input.MouseModeEnum.Confined;
	}

	private void ExitPrecisionMode() {
		inPrecisionMode = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
}
