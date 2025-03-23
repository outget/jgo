using Godot;
using System;

public partial class Button : Node3D {
	[Export]
	public bool toggle = false;

	[Export]
	public AnimationPlayer animationPlayer;

	[Signal]
	public delegate void ButtonUpEventHandler();

	[Signal]
	public delegate void ButtonDownEventHandler();

	private bool pressed = false;

	// Check global input in case toggle is set to false and user moves mouse off button before releasing.
    public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton e) {
			if (e.ButtonIndex == MouseButton.Left && !e.Pressed && pressed && !toggle) {
				Release();
			}
		}
    }

    public void mouse_event(Camera3D camera, InputEvent @event, Vector3 event_position, Vector3 normal, int shape_index) {
		if (@event is InputEventMouseButton e) {
			if (e.ButtonIndex == MouseButton.Left && e.Pressed) {
				if (toggle) {
					if (pressed) Release(); else Press();
				} else {
					if (!pressed) Press();
				}
			} else if (e.ButtonIndex == MouseButton.Left && !e.Pressed && pressed && !toggle) {
				Release();
			}
		}
	}

	private void Press() {
		pressed = true;
		EmitSignal(SignalName.ButtonDown);
		animationPlayer.Play("press");
	}

	private void Release() {
		pressed = false;
		EmitSignal(SignalName.ButtonUp);
		animationPlayer.Play("release");
	}
}
