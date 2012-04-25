using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapDrawXNAEngine
{
    public class FreeCamera : Camera
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Target { get; private set; }
        public Vector3 Direction { get; private set; }
        private Vector3 translation;
        public FreeCamera(Vector3 Position, float Yaw, float Pitch, float Roll, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.Position = Position;
            this.Yaw = Yaw;
            this.Pitch = Pitch;
            this.Pitch = Roll;
            translation = Vector3.Zero;
        }

        public void Rotate(float YawChange, float PitchChange, float RollChange)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;
            this.Roll += RollChange;
        }

        public void Move(Vector3 Translation)
        {
            this.translation += Translation;
        }

        public void Move(float fLeft, float fUp)
        {
            Vector3 pMove = new Vector3(fLeft, -fUp, 0);
            this.translation += pMove;
        }

        public override void Update()
        {
            UpdateAspectRatio();
            // Calculate the rotation matrix
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);
            // Offset the position and reset the translation
            translation = Vector3.Transform(translation, rotation);
            Position += translation;
            translation = Vector3.Zero;
            // Calculate the new target
            Direction = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + Direction;
            // Calculate the up vector
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);
            // Calculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
