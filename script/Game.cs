using Godot;
using System;

public partial class Game : Node {
	Star system_star;
	Star[] stars;

	[Export]
	public ShaderMaterial skybox_material;

	public Game() {
		system_star = new Star { name = "Googer", position = new Vector3(0, 0, 0), diameter = 1400000 };
		stars = [
			new Star { name = "Goober", position = new Vector3(4.0f, 2.0f, 0.0f), diameter = 1200000 },
			new Star { name = "Stroober", position = new Vector3(4.0f, 2.0f, 10.0f), diameter = 1200000 },
		];
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		// Setup skybox stars.
		Vector3[] star_dirs = new Vector3[stars.Length];
		for (int i = 0; i < stars.Length; i++) {
			star_dirs[i] = stars[i].position.Normalized();
		}
		skybox_material.SetShaderParameter("stars", star_dirs);
		skybox_material.SetShaderParameter("star_count", star_dirs.Length);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}
}
