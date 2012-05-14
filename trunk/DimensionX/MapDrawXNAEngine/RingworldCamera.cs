using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapDrawXNAEngine
{
    public class RingworldCamera : Camera
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
        public Vector3 Direction { get; private set; }

        private Vector3 targetTranslation;

        private float m_fR;

        public RingworldCamera(float fR, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            m_fR = fR;

            Position = new Vector3(0, 0, 0);
            Target = new Vector3(0, m_fR, 0);

            Yaw = MathHelper.ToRadians(0);
            Pitch = MathHelper.ToRadians(45);
            Roll = MathHelper.ToRadians(0);
            
            targetTranslation = Vector3.Zero;

            m_fDistance = Vector3.Distance(Position, Target);
        }

        public override void Orbit(float YawChange, float PitchChange, float RollChange)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;

            this.Pitch = Math.Max(MathHelper.ToRadians(90), Math.Min(MathHelper.ToRadians(179), this.Pitch));

            this.Roll += RollChange;
        }

        public override void Pan(float fLeft, float fUp)
        {
            Vector3 pMove = new Vector3(fLeft, 0, fUp);
            this.targetTranslation += pMove;
        }

        public override void ZoomIn(float fDistance)
        {
            fDistance *= m_fDistance / 100000;

            m_fDistance -= fDistance;

            if (m_fDistance < 1)
                m_fDistance = 1;

            if (m_fDistance > 100000)
                m_fDistance = 100000;
        }

        public override void Update()
        {
            UpdateAspectRatio();

            Matrix cameraRotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);

            // Offset the position and reset the translation
            targetTranslation = Vector3.Transform(targetTranslation, Matrix.CreateRotationY(Yaw));
            Target += targetTranslation;

            targetTranslation = Vector3.Zero;

            Target = Vector3.Normalize(Target);
            Target *= m_fR;

            Vector3 pOldDirection = Direction;

            Vector3 cameraOriginalTarget = Vector3.Forward;
            Direction = Vector3.Transform(cameraOriginalTarget, cameraRotation);

            Vector3 cameraOrbit = Vector3.Zero;

            cameraOrbit = Target - Direction * m_fDistance;

            Position = cameraOrbit;

            Vector3 cameraFinalTarget = Target;

            Vector3 cameraOriginalUpVector = Vector3.Normalize(Target) ;//Vector3.Up;//new Vector3(0, 0, 1);
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            View = Matrix.CreateLookAt(Position, cameraFinalTarget, cameraRotatedUpVector);
        }
    }
}
