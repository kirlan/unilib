using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SphereCameraTest
{
    public class FreeCamera
    {
        protected GraphicsDevice GraphicsDevice { get; set; }
        
        /// <summary>
        /// Поворот вокруг оси Y
        /// </summary>
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        public Vector3 Target { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; protected set; }
        public Vector3 Top { get; set; }

        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        
        private float m_fR;
        public float m_fDistance;

        public FreeCamera(float fR, GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            UpdateAspectRatio(); 
            
            m_fR = fR;

            Position = new Vector3(0, m_fR, 0);
            Target = new Vector3(0, m_fR * 2, 0);

            Yaw = MathHelper.ToRadians(0);
            Pitch = MathHelper.ToRadians(359);
            Roll = MathHelper.ToRadians(0);

            m_fDistance = Vector3.Distance(Position, Target);
        }

        private void generatePerspectiveProjectionMatrix(float FieldOfView)
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            //float aspectRatio = (float)pp.BackBufferWidth /
            //(float)pp.BackBufferHeight;
            this.Projection = Matrix.CreatePerspectiveFieldOfView(
            FieldOfView, GraphicsDevice.Viewport.AspectRatio, 0.1f, float.MaxValue);
        }
        public void UpdateAspectRatio()
        {
            generatePerspectiveProjectionMatrix(MathHelper.ToRadians(45));
        }

        public void Orbit(float YawChange, float PitchChange, float RollChange)
        {
            this.Yaw += MathHelper.ToRadians(YawChange);
            this.Pitch -= MathHelper.ToRadians(PitchChange);

            this.Pitch = Math.Max(MathHelper.ToRadians(270), Math.Min(MathHelper.ToRadians(359), this.Pitch));
            //this.Pitch = Math.Max(MathHelper.ToRadians(91), Math.Min(MathHelper.ToRadians(269), this.Pitch));

            //this.Roll += MathHelper.ToRadians(RollChange);
        }

        internal void ZoomIn(float fDistance)
        {
            fDistance *= m_fDistance / 10000;

            m_fDistance -= fDistance;

            if (m_fDistance < 1)
                m_fDistance = 1;

            if (m_fDistance > 1000)
                m_fDistance = 1000;
        }

        internal void Update()
        {
            UpdateAspectRatio();

            Matrix cameraRotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);

            //вычисляем ось и угол поворота, которые могли бы привести Vector3.Backward в Vector3.Normalize(Target)
            Vector3 pTargetOriginal = Vector3.Normalize(Target);
            Vector3 pAxis = Vector3.Cross(Vector3.Up, pTargetOriginal);
            pAxis.Normalize();
            float fRotAngle = (float)Math.Acos(Vector3.Dot(Vector3.Up, pTargetOriginal));

            float fZ = Target.Z;

            Target = Vector3.Normalize(Target);
            Target *= m_fR;

            //вычислим направление камеры, как если бы мишень никуда не смещалась, т.е. была бы Vector3.Backward
            Direction = Vector3.Transform(Vector3.Down, cameraRotation);
            //а теперь повернём вектор направления так, чтобы совместить Vector3.Backward с Vector3.Normalize(Target)
            if(!float.IsNaN(pAxis.X) && !float.IsNaN(pAxis.Y) && !float.IsNaN(pAxis.Z))
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
