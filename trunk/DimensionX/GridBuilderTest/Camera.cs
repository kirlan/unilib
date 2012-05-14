using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAEngine
{
    public abstract class Camera
    {
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        protected GraphicsDevice GraphicsDevice { get; set; }
        public Camera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            generatePerspectiveProjectionMatrix(MathHelper.PiOver4);
        }
        private void generatePerspectiveProjectionMatrix(float FieldOfView)
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            //float aspectRatio = (float)pp.BackBufferWidth /
            //(float)pp.BackBufferHeight;
            this.Projection = Matrix.CreatePerspectiveFieldOfView(
            FieldOfView, GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000000.0f);
        }
        public void UpdateAspectRatio()
        {
            generatePerspectiveProjectionMatrix(MathHelper.PiOver4);
        }
        public virtual void Update()
        {
        }
    }
}
