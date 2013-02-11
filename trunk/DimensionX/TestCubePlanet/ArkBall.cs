using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestCubePlanet
{
    public class ArcBallCamera
    {
        public Vector3 Position { get; set; }

        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        protected GraphicsDevice GraphicsDevice { get; set; }
        private void generatePerspectiveProjectionMatrix(float FieldOfView)
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            this.Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, GraphicsDevice.Viewport.AspectRatio, 0.1f, 100000f);
        }

        public Vector3 Direction { get; private set; }
        public float m_fDistance;

        private Vector3 startVector = Vector3.Zero;
        private Quaternion quatRotation = Quaternion.Identity;

        /// <summary>
        /// ArcBall Constructor
        /// </summary>
        /// <param name="width">The width of your display window</param>
        /// <param name="height">The height of your display window</param>
        public ArcBallCamera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            generatePerspectiveProjectionMatrix(Microsoft.Xna.Framework.MathHelper.PiOver4);

            this.Position = new Vector3(0, 4000, 0);

            m_fDistance = Vector3.Distance(Position, Vector3.Zero);
        }

        public void UpdateAspectRatio()
        {
            generatePerspectiveProjectionMatrix(Microsoft.Xna.Framework.MathHelper.ToRadians(45));
        }
        
        /// <summary>
        /// Begin dragging
        /// </summary>
        /// <param name="startPoint">The X/Y position in your window at the beginning of dragging</param>
        /// <param name="rotation"></param>
        public void StartDrag(Vector3 startPoint)
        {
            this.startVector = Vector3.Normalize(startPoint);
        }

        public void Drag(Vector3 currentPoint)
        {
            Vector3 currentVector = Vector3.Normalize(currentPoint);

            if (startVector == currentVector)
                return;

            Vector3 axis = Vector3.Cross(startVector, currentVector);

            float angle = Vector3.Dot(startVector, currentVector);

            Quaternion delta = new Quaternion(axis.X, axis.Y, axis.Z, -angle);

            quatRotation = Quaternion.Multiply(quatRotation, delta);
        }

        /// <summary>
        /// Get an updated rotation based on the current mouse position
        /// </summary>
        /// <param name="currentVector">The curren X/Y position of the mouse</param>
        /// <param name="result">The resulting quaternion to use to rotate your object</param>
        public void Update()
        {
            UpdateAspectRatio();

            Matrix cameraRotation = Matrix.CreateFromQuaternion(quatRotation);

            Direction = Vector3.Transform(Vector3.Forward, cameraRotation);
            Position = Vector3.Zero - Direction * m_fDistance;

            Vector3 cameraRotatedUpVector = Vector3.Transform(Vector3.Up, cameraRotation);

            View = Matrix.CreateLookAt(Position, Vector3.Zero, cameraRotatedUpVector);
        }
    }
}
