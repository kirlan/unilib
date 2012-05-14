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
        public Vector3 Direction { get; private set; }

        private Vector3 translationPan;

        public PlainCamera(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.Position = new Vector3(0, 100000, 0);
            this.Yaw = MathHelper.ToRadians(0);
            this.Pitch = MathHelper.ToRadians(270);
            this.Roll = MathHelper.ToRadians(0);

            translationPan = Vector3.Zero;

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
            Vector3 pMove = new Vector3(fLeft, 0, fUp);
            this.translationPan += pMove;
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
            translationPan = Vector3.Transform(translationPan, Matrix.CreateRotationY(Yaw));
            Target += translationPan;

            translationPan = Vector3.Zero;

            Target = Vector3.Transform(Target, Matrix.CreateScale(1, 0, 1));

            Vector3 pOldDirection = Direction;

            Vector3 cameraOriginalTarget = Vector3.Forward;
            Direction = Vector3.Transform(cameraOriginalTarget, cameraRotation);

            Vector3 cameraOrbit = Vector3.Zero;

            cameraOrbit = Target - Direction * m_fDistance;

            Position = cameraOrbit;

            Vector3 cameraFinalTarget = Target;

            Vector3 cameraOriginalUpVector = Vector3.Up;
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            View = Matrix.CreateLookAt(Position, cameraFinalTarget, cameraRotatedUpVector);
        }
    }
}
