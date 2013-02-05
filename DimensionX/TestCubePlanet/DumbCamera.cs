using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestCubePlanet
{
    public class DumbCamera
    {
        public Vector3 Position { get; set; }
      
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public float m_fDistance;
        
        protected GraphicsDevice GraphicsDevice { get; set; }
        private void generatePerspectiveProjectionMatrix(float FieldOfView)
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            this.Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, GraphicsDevice.Viewport.AspectRatio, 0.1f, 100000f);
        }
        
        public Vector3 Direction { get; private set; }

        public DumbCamera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            generatePerspectiveProjectionMatrix(Microsoft.Xna.Framework.MathHelper.PiOver4);

            this.Position = new Vector3(0, 5000, 0);

            m_fDistance = Vector3.Distance(Position, Vector3.Zero);
        }

        public void UpdateAspectRatio()
        {
            generatePerspectiveProjectionMatrix(Microsoft.Xna.Framework.MathHelper.ToRadians(45));
        }

        public void Update(bool bMode)
        {
            UpdateAspectRatio();

            Matrix cameraRotation = Matrix.CreateFromYawPitchRoll(0, 0, 0);

            Direction = Vector3.Transform(bMode? Vector3.Down : Vector3.Forward, cameraRotation);
            Position = Vector3.Zero - Direction * m_fDistance;

            Vector3 cameraRotatedUpVector = Vector3.Transform(bMode? Vector3.Forward : Vector3.Up, cameraRotation);

            View = Matrix.CreateLookAt(Position, Vector3.Zero, cameraRotatedUpVector);
        }
    }
}
