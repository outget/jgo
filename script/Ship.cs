using Godot;

public partial class Ship : RigidBody3D {

    [Export]
    public Label velocityLabel;
    [Export]
    public Label angularLabel;
    [Export]
    public Label angularDampLabel;
    [Export]
    public float thrust = 10;

    public bool angDampStatus = true;

    public override void _IntegrateForces(PhysicsDirectBodyState3D state) {

        Vector3 forward = -GetGlobalTransformInterpolated().Basis.Z;        
        Vector3 right = GetGlobalTransformInterpolated().Basis.X;
        Vector3 up = GetGlobalTransformInterpolated().Basis.Y;


        // up down left right

        state.AddConstantCentralForce(Input.GetActionStrength("player_forward") * forward * state.Step * thrust);
        state.AddConstantCentralForce(Input.GetActionStrength("player_backward") * forward * state.Step * -thrust);

        state.AddConstantCentralForce(Input.GetActionStrength("player_up") * up * state.Step * thrust * 0.5f);
        state.AddConstantCentralForce(Input.GetActionStrength("player_down") * up * state.Step * -thrust * 0.5f);

        state.AddConstantCentralForce(Input.GetActionStrength("player_right") * right * state.Step * thrust * 0.5f);
        state.AddConstantCentralForce(Input.GetActionStrength("player_left") * right * state.Step * -thrust * 0.5f);

        if (Input.IsActionPressed("spacebrake")) 
        {
            state.AddConstantCentralForce(LinearVelocity / -10);
            if (LinearVelocity.Length() < 0.5f)
            {
                LinearVelocity = new Vector3(0,0,0);
                state.SetConstantForce(new Vector3(0,0,0));
            }
        }


        // mouse lookaround

        /* 
        state.AddConstantTorque(Input.GetLastMouseScreenVelocity().Y * right * state.Step * -0.00001f);
        state.AddConstantTorque(Input.GetLastMouseScreenVelocity().X * up * state.Step * -0.00001f);

        GD.Print(Input.GetLastMouseVelocity());
        if (Input.GetLastMouseVelocity().Length() == 0) {
            state.SetConstantTorque(AngularVelocity / -100);
            if (AngularVelocity.Length() < 0.1) {
                AngularVelocity = new Vector3(0,0,0);
                state.SetConstantTorque(new Vector3(0,0,0));
            }
        }
        */


        state.AddConstantTorque(Input.GetActionStrength("player_roll_left") * forward * state.Step * -10);
        state.AddConstantTorque(Input.GetActionStrength("player_roll_right") * forward * state.Step * 10);


        if (Input.IsActionJustPressed("dampening")) 
        {
            angDampStatus = !angDampStatus;
        }

        if (angDampStatus == true)
        {
            AngularDamp = 100f;
            if (AngularVelocity.Length() > 2f) 
            {
                state.AddConstantTorque(new Vector3(0,0,-AngularVelocity.Z));
            }

            if (AngularVelocity.Length() < 0.1f && !Input.IsAnythingPressed()) 
            {
                state.SetConstantTorque(new Vector3(0,0,0));
            }

        } else 
            AngularDamp = 0f;

        /*
        if (AngularVelocity.Length() > 2) 
        {
            state.AddConstantTorque(AngularVelocity / -100);
            if (AngularVelocity.Length() < 1) 
            {
                AngularVelocity = new Vector3(0,0,0);
                state.SetConstantTorque(new Vector3(0,0,0));
            }
        }
        */

        velocityLabel.Text = "Velocity: " + LinearVelocity.ToString();
        angularLabel.Text = "Angular Velocity: " + AngularVelocity.ToString();
        angularDampLabel.Text = "Angular Dampening: " + angDampStatus.ToString();

    }

}
