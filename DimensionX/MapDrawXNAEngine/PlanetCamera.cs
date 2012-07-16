using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapDrawXNAEngine
{
    class PlanetCamera : Camera
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        private float m_fR;

        public PlanetCamera(float fR, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            m_fR = fR;

            Position = new Vector3(0, m_fR, 0);
            Target = new Vector3(0, m_fR*2, 0);

            Yaw = MathHelper.ToRadians(0);
            Pitch = MathHelper.ToRadians(-91);
            Roll = MathHelper.ToRadians(0);
            
            m_fDistance = Vector3.Distance(Position, Target);
        }

        public override void Orbit(float YawChange, float PitchChange, float RollChange)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;

            //this.Pitch = Math.Max(MathHelper.ToRadians(271), Math.Min(MathHelper.ToRadians(359), this.Pitch)); 
            //this.Pitch = Math.Max(MathHelper.ToRadians(91), Math.Min(MathHelper.ToRadians(269), this.Pitch));

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

            if (m_fDistance > 1000)
                m_fDistance = 1000;
        }

        public override void Update()
        {
            UpdateAspectRatio();

            Matrix cameraRotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);

            //float fPhi = (float)Math.Atan2(Target.X, Target.Y);
            Vector3 pTargetOriginal = Vector3.Normalize(Target);
            Vector3 pAxis = Vector3.Cross(Vector3.Backward, pTargetOriginal);
            pAxis.Normalize();
            float fRotAngle = (float)Math.Acos(Vector3.Dot(Vector3.Backward, pTargetOriginal));

            float fZ = Target.Z;

            Target = Vector3.Normalize(Target);
            Target *= m_fR;

            Direction = Vector3.Transform(Vector3.Forward, cameraRotation);
            Direction = Vector3.Transform(Direction, Matrix.CreateFromAxisAngle(pAxis, fRotAngle));

            Position = Target - Direction * m_fDistance;

            Vector3 cameraOriginalUpVector = Vector3.Cross(Target, Direction);
            cameraOriginalUpVector = Vector3.Cross(-cameraOriginalUpVector, Direction);
            cameraOriginalUpVector = Vector3.Normalize(cameraOriginalUpVector);
            Top = cameraOriginalUpVector;

            View = Matrix.CreateLookAt(Position, Target, Top);
        }
    }
}
