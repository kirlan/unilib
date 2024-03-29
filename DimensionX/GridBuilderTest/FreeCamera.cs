﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GridBuilderTest
{
    public class FreeCamera : Camera
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
        public Vector3 Direction { get; private set; }
        public Vector3 Top { get; private set; }

        private Vector3 targetTranslation;

        private float m_fR;

        public FreeCamera(float fR, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            m_fR = fR;

            Position = new Vector3(0, 0, 0);
            Target = new Vector3(0, m_fR, 0);

            Yaw = MathHelper.ToRadians(90);
            Pitch = MathHelper.ToRadians(45);
            Roll = MathHelper.ToRadians(0);
            
            targetTranslation = Vector3.Zero;

            m_fDistance = Vector3.Distance(Position, Target);
        }

        public override void Orbit(float YawChange, float PitchChange, float RollChange)
        {
            this.Yaw += MathHelper.ToRadians(YawChange);
            this.Pitch += MathHelper.ToRadians(PitchChange);

            //this.Pitch = Math.Max(MathHelper.ToRadians(90), Math.Min(MathHelper.ToRadians(179), this.Pitch));

            this.Roll += MathHelper.ToRadians(RollChange);
        }

        public override void Pan(float fLeft, float fUp)
        {
            Vector3 pMove = new Vector3(fLeft, 0, fUp);
            this.targetTranslation += pMove;
        }

        public override void ZoomIn(float fDistance)
        {
            fDistance *= m_fDistance / 10000;

            m_fDistance -= fDistance;

            if (m_fDistance < 1)
                m_fDistance = 1;

            if (m_fDistance > 10*m_fR-1)
                m_fDistance = 10*m_fR-1;
        }

        public override void Update()
        {
            UpdateAspectRatio();

            Matrix cameraRotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);

            // Offset the position and reset the translation
            targetTranslation = Vector3.Transform(targetTranslation, Matrix.CreateRotationY(Yaw));
            Target += targetTranslation;

            targetTranslation = Vector3.Zero;

            float fPhi = (float)Math.Atan2(Target.X, Target.Y);

            float fZ = Target.Z;

            Target = Vector3.Transform(Target, Matrix.CreateScale(1, 1, 0));
            Target = Vector3.Normalize(Target);
            Target *= m_fR;
            Target = new Vector3(Target.X, Target.Y, fZ);

            Direction = Vector3.Transform(Vector3.Forward, cameraRotation);
            Direction = Vector3.Transform(Direction, Matrix.CreateRotationZ(-fPhi));

            Position = Target - Direction * m_fDistance;

            Vector3 cameraOriginalUpVector = Vector3.Cross(Vector3.Transform(Position, Matrix.CreateScale(1, 1, 0)), Direction);
            cameraOriginalUpVector = Vector3.Cross(cameraOriginalUpVector, Direction);
            cameraOriginalUpVector = Vector3.Normalize(cameraOriginalUpVector);
            Top = cameraOriginalUpVector;

            View = Matrix.CreateLookAt(Position, Target, Top);
        }
    }
}
