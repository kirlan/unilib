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
        public Vector3 Top;
        public float m_fDistance;

        /// <summary>
        /// ArcBall Constructor
        /// </summary>
        /// <param name="width">The width of your display window</param>
        /// <param name="height">The height of your display window</param>
        public ArcBallCamera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            generatePerspectiveProjectionMatrix(Microsoft.Xna.Framework.MathHelper.PiOver4);

            this.Position = new Vector3(0, 0, 400);

            m_fDistance = Vector3.Distance(Position, Vector3.Zero);
        }

        public void UpdateAspectRatio()
        {
            generatePerspectiveProjectionMatrix(Microsoft.Xna.Framework.MathHelper.ToRadians(45));
        }

        public Vector3 startVector;
        private Quaternion quatRotation = Quaternion.Identity;
        private Quaternion m_StartRot;

        private Vector3 MapToSphere(Vector3 pPoint, float fR)
        {
            //получим экранные координаты центра планеты
            Vector3 pCenter = GraphicsDevice.Viewport.Project(Vector3.Zero, Projection, View, Matrix.Identity);
            pCenter.Z = 0;
            float fCenterX = pCenter.X - (float)GraphicsDevice.Viewport.Width / 2;
            float fCenterY = pCenter.Y - (float)GraphicsDevice.Viewport.Height / 2;

            //float fQuasyR = m_fR * m_pCamera.Position.Length() / (float)Math.Sqrt(m_pCamera.Position.Length() * m_pCamera.Position.Length() - m_fR*m_fR);

            //получим экранный размер радиуса планеты
            Vector3 pRadius = Vector3.Normalize(Vector3.Cross(Position, Top)) * fR;// fQuasyR;
            pRadius = GraphicsDevice.Viewport.Project(pRadius, Projection, View, Matrix.Identity);
            pRadius.Z = 0;
            float fRadius = (pRadius - pCenter).Length();

            Vector3 pMousePoint = GraphicsDevice.Viewport.Project(Vector3.Normalize(pPoint) * fR, Projection, View, Matrix.Identity);
            pMousePoint.Z = 0;

            //точка, куда указывает мышь - в экранных координатах, относительно центра экрана
            //float fMouseX = x - (float)ClientRectangle.Width / 2;
            //float fMouseY = y - (float)ClientRectangle.Height / 2;
            float fMouseX = pMousePoint.X - (float)GraphicsDevice.Viewport.Width / 2;
            float fMouseY = -(pMousePoint.Y - (float)GraphicsDevice.Viewport.Height / 2);

            //проекция точки, куда указывает мышь, в мировых координатах - на плоскость перпендикулярную вектору взгляда и проходящую через центр планеты
            Vector2 pRelativeMousePos = new Vector2((fMouseX - fCenterX) * fR / fRadius,
                                                    (fMouseY - fCenterY) * fR / fRadius);

            //теперь вычислим точку пересечения проецирующего луча с планетарной сферой - в мировых координатах, но в системе координат, образованной вектором взгляда и перпендикулярной ему плоскоостью, проходящей через центр планеты.
            //эта сфера - и есть наш ArkBall.
            //поскольку мы уже знаем координаты проекции, то работать будем в плоскости, образованной камерой, центром планеты и найденной проекцией точки.
            //всё, что нам на самом деле нужно - это найти пересечение проецирующего луча на плоскости с окружностью радиуса планеты

            //коэффициенты уравнения прямой на плоскости, проходящей через точки (0, m_pCamera.Position.Length()) и (pRelativeMousePos.Length(), 0)
            float A = Position.Length();
            float B = pRelativeMousePos.Length();
            float C = -A * B;

            //некоторые промежуточные вычисления, для оптимизации
            float A2B2 = A * A + B * B;
            float R2 = fR * fR;
            float C2 = C * C;

            float bx, by = 0;
            //условие того, что прямая вообще пересекается с окружностью
            if (C2 + float.Epsilon < R2 * A2B2)
            {
                //пересечение нашей прямой и перпендикуляра к ней, проведённого из центра окружности
                float x0 = -A * C / A2B2;
                float y0 = -B * C / A2B2;

                if (Math.Abs(C2 - R2 * A2B2) < float.Epsilon)
                {
                    //если прямая не пересекает окружность, а только касается её, то найденная точка пересечения праямой и перпендикуляра и есть то, что мы ищем
                    pRelativeMousePos = Vector2.Normalize(pRelativeMousePos) * x0;
                }
                else
                {
                    //иначе решаем квадратное уравнение...
                    float d = R2 - C2 / A2B2;
                    float mult = (float)Math.Sqrt(d / A2B2);
                    bx = x0 - B * mult;
                    by = y0 + A * mult;
                    pRelativeMousePos = Vector2.Normalize(pRelativeMousePos) * bx;
                }
            }

            return new Vector3(pRelativeMousePos.X, pRelativeMousePos.Y, by);
        }

        /// <summary>
        /// Begin dragging
        /// </summary>
        /// <param name="startPoint">The X/Y position in your window at the beginning of dragging</param>
        /// <param name="rotation"></param>
        public void StartDrag(Vector3 startPoint, float fR)
        {
            startVector = Vector3.Normalize(MapToSphere(startPoint, fR));
            m_StartRot = quatRotation;
        }

        public Vector3 axis;

        public void Drag(Vector3 currentPoint, float fR)
        {
            Vector3 currentVector = Vector3.Normalize(MapToSphere(currentPoint, fR));

            if (startVector.Equals(currentVector))
                return;

            float angle = Vector3.Dot(startVector, currentVector);

            float sina = (float)Math.Sqrt((1.0 - angle) * 0.5);
            float cosa = (float)Math.Sqrt((1.0 + angle) * 0.5);

            axis = Vector3.Normalize(Vector3.Cross(startVector, currentVector)) * sina;

            Quaternion delta = new Quaternion(axis.X, axis.Y, axis.Z, -cosa);

            quatRotation = Quaternion.Multiply(m_StartRot, delta);
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

            Top = Vector3.Transform(Vector3.Up, cameraRotation);

            View = Matrix.CreateLookAt(Position, Vector3.Zero, Top);
        }
    }
}
