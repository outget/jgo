using Godot;

public partial class Lever : Node3D {
	[Export]
	public Node3D lever;

	[Signal]
	public delegate void LeverUpEventHandler();

	[Signal]
	public delegate void LeverDownEventHandler();

	// Max rotation in both directions, any greater value will result in clipping.
	private float max_rot = Mathf.DegToRad(59);
	private float sensitivity = 0.005f;

	private bool held = false;

	private float position = 0.0f;

	public float GetLeverPosition() {
		return position;
	}

    public override void _Input(InputEvent @event) {
		if (!held) return;

		if (@event is InputEventMouseMotion e) {
			float newpos = Mathf.Clamp(position - sensitivity * e.ScreenRelative.Y, -1.0f, 1.0f);
			if (position < 0.9f && newpos >= 0.9f) {
				EmitSignal(SignalName.LeverUp);
				GD.Print("UP");
			} else if (position > -0.9f && newpos <= -0.9f) {
				EmitSignal(SignalName.LeverDown);
				GD.Print("DOWN");
			}
			position = newpos;

			UpdateRotation();
		} else if (@event is InputEventMouseButton em) {
			if (em.ButtonIndex == MouseButton.Left && !em.Pressed && held) {
				held = false;
			}
		}
    }

    public void mouse_event(Camera3D camera, InputEvent @event, Vector3 event_position, Vector3 normal, int shape_index) {
		if (@event is InputEventMouseButton e) {
			if (e.ButtonIndex == MouseButton.Left && e.Pressed && !held) {
				held = true;
			} else if (e.ButtonIndex == MouseButton.Left && !e.Pressed && held) {
				held = false;
			}
		}
	}

	private void UpdateRotation() {
		lever.Rotation = new Vector3(-max_rot * position, 0, 0);
	}
}
