using Godot;
using System;

public partial class Door : Node3D {
	[Export]
	public AnimationPlayer animation_player;

	[Export]
	public Area3D area3D;

	[Export]
	public bool manual = false;
	private bool manual_side_front = false;

    public override void _Ready() {
		if (area3D != null) {
			area3D.BodyEntered += BodyEntered;
			area3D.BodyExited += BodyExited;
		}
    }

    public void Open() {
		PlayUniform("open");
	}

	public void Close() {
		PlayUniform("close");
	}

	public void OpenSided(bool front) {
		if (manual && manual_side_front != front) return;
		Open();
	}

	public void CloseSided(bool front) {
		if (manual && manual_side_front != front) return;
		Close();
	}

	public void SetManual(bool value, bool front) {
		// Only the side that engaged manual mode can alter manual mode.
		if (manual && manual_side_front != front) return;

		manual = value;
		manual_side_front = front;
	}

	void PlayUniform(StringName name) {
		if (animation_player.AssignedAnimation == "") {
			animation_player.Play(name);
			return;
		}

		double previous_position = animation_player.CurrentAnimationPosition;
		animation_player.Play(name);
		animation_player.Seek(animation_player.CurrentAnimationLength - previous_position);
	}

	void BodyEntered(Node3D body) {
		if (!manual && body.IsInGroup("player")) {
			Open();
		}
	}

	void BodyExited(Node3D body) {
		if (!manual && body.IsInGroup("player")) {
			Close();
		}
	}
}
