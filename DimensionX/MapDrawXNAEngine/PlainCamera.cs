using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Socium;

namespace MapDrawXNAEngine
{
    public class PlainCamera : Camera
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        public PlainCamera(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.Position = new Vector3(0, 75000 / 1000, 0);
            this.Yaw = MathHelper.ToRadians(0);
            this.Pitch = MathHelper.ToRadians(270);
            this.Roll = MathHelper.ToRadians(0);

            m_fDistance = Vector3.Distance(Position, Target);
        }

        public override void Orbit(float YawChange, float PitchChange, float RollChange)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;

            this.Pitch = Math.Max(MathHelper.ToRadians(270), Math.Min(MathHelper.ToRadians(359), this.Pitch));

            this.Roll += RollChange;
        }

        public override void Pan(float fLeft, float fUp)
        {
        }

        public override void ZoomIn(float fDistance)
        {
            fDistance *= m_fDistance / 10000;

            m_fDistance -= fDistance;

            if (m_fDistance < 1)
                m_fDistance = 1;

            if (m_fDistance > 100)
                m_fDistance = 100;
        }

        public override void Update()
        {
            UpdateAspectRatio();

            Matrix cameraRotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);

            Target = Vector3.Transform(Target, Matrix.CreateScale(1, 0, 1));
            Direction = Vector3.Transform(Vector3.Forward, cameraRotation);

            Vector3 cameraPos = Target - Direction * m_fDistance;
            Position = cameraPos;

            Top = Vector3.Transform(Vector3.Up, cameraRotation);
            View = Matrix.CreateLookAt(Position, Target, Top);
        }
    }
}
