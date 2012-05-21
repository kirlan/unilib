using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapDrawXNAEngine
{
    public abstract class Camera
    {
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }
        public Vector3 Direction { get; protected set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public float m_fDistance;
        
        protected GraphicsDevice GraphicsDevice { get; set; }
        public Camera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            UpdateAspectRatio();
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
            generatePerspectiveProjectionMatrix(MathHelper.ToRadians(75));
        }
        public virtual void Update()
        {
        }
        public virtual void Pan(float fLeft, float fUp)
        {
        }
        public virtual void ZoomIn(float fDistance)
        { 
        }
        public virtual void Orbit(float YawChange, float PitchChange, float RollChange)
        { 
        }

    }
}
