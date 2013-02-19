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
        public Vector3 FocusPoint { get; set; }
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
        /// радиус планеты
        /// </summary>
        private float m_fR = 150;

        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        /// <summary>
        /// ArcBall Constructor
        /// </summary>
        /// <param name="width">The width of your display window</param>
        /// <param name="height">The height of your display window</param>
        public ArcBallCamera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            generatePerspectiveProjectionMatrix(Microsoft.Xna.Framework.MathHelper.PiOver4);

            //this.Position = new Vector3(0, 0, 400);

            Yaw = MathHelper.ToRadians(0);
            Pitch = MathHelper.ToRadians(359);
            Roll = MathHelper.ToRadians(0);
            
            m_fDistance = 250;
        }

        public void UpdateAspectRatio()
        {
            generatePerspectiveProjectionMatrix(Microsoft.Xna.Framework.MathHelper.ToRadians(45));
        }

        public void Initialize(float fR)
        {
            m_fR = fR;
        }

        /// <summary>
        /// Сохранённая точка фокуса камеры на момент начала её перетаскивания
        /// ВАЖНО: координаты в системе ArkBall-полусферы!
        /// </summary>
        private Vector3 m_pStartFocusPoint;
        /// <summary>
        /// Текущий кватернион поворота точки фокуса камеры
        /// В мировых координатах.
        /// </summary>
        private Quaternion m_pFocusPointRotation = Quaternion.Identity;
        /// <summary>
        /// Сохранённый кватернион поворота точки фокуса камеры на момент начала перетаскивания
        /// В мировых координатах.
        /// </summary>
        private Quaternion m_pStartFocusPointRotation;

        /// <summary>
        /// ось поворота при перетаскивании точки фокуса камеры.
        /// ВАЖНО: координаты в системе ArkBall-полусферы!
        /// </summary>
        private Vector3 m_pFocusPointRotationAxis;

        /// <summary>
        /// переводит мировые координаты точки на поверхности планеты (с рельефом) в координаты
        /// точки на поверхности ArkBall полусферы, всегда ориентированной по камере, основанием от наблюдателя
        /// </summary>
        /// <param name="pPoint">точка</param>
        /// <param name="m_fR">радиус планеты</param>
        /// <returns></returns>
        private Vector3 MapToSphere(Vector3 pPoint)
        {
            //получим экранные координаты центра планеты
            Vector3 pCenter = GraphicsDevice.Viewport.Project(Vector3.Zero, Projection, View, Matrix.Identity);
            pCenter.Z = 0;
            float fCenterX = pCenter.X - (float)GraphicsDevice.Viewport.Width / 2;
            float fCenterY = pCenter.Y - (float)GraphicsDevice.Viewport.Height / 2;

            //float fQuasyR = m_fR * m_pCamera.Position.Length() / (float)Math.Sqrt(m_pCamera.Position.Length() * m_pCamera.Position.Length() - m_fR*m_fR);

            //получим экранный размер радиуса планеты
            Vector3 pRadius = Vector3.Normalize(Vector3.Cross(Position, Top)) * m_fR;// fQuasyR;
            pRadius = GraphicsDevice.Viewport.Project(pRadius, Projection, View, Matrix.Identity);
            pRadius.Z = 0;
            float fRadius = (pRadius - pCenter).Length();

            Vector3 pMousePoint = GraphicsDevice.Viewport.Project(Vector3.Normalize(pPoint) * m_fR, Projection, View, Matrix.Identity);
            pMousePoint.Z = 0;

            //точка, куда указывает мышь - в экранных координатах, относительно центра экрана
            //float fMouseX = x - (float)ClientRectangle.Width / 2;
            //float fMouseY = y - (float)ClientRectangle.Height / 2;
            float fMouseX = pMousePoint.X - (float)GraphicsDevice.Viewport.Width / 2;
            float fMouseY = -(pMousePoint.Y - (float)GraphicsDevice.Viewport.Height / 2);

            //проекция точки, куда указывает мышь, в мировых координатах - на плоскость перпендикулярную вектору взгляда и проходящую через центр планеты
            Vector2 pRelativeMousePos = new Vector2((fMouseX - fCenterX) * m_fR / fRadius,
                                                    (fMouseY - fCenterY) * m_fR / fRadius);

            float fCenterLength = (float)Math.Sqrt(fCenterX * fCenterX + fCenterY * fCenterY);
            float fCameraScreenDistance = (float)Math.Sqrt(Position.Length() * Position.Length() - fCenterLength * fCenterLength);

            //теперь вычислим точку пересечения проецирующего луча с планетарной сферой - в мировых координатах, но в системе координат, образованной вектором взгляда и перпендикулярной ему плоскоостью, проходящей через центр планеты.
            //эта сфера - и есть наш ArkBall.
            //поскольку мы уже знаем координаты проекции, то работать будем в плоскости, образованной камерой, центром планеты и найденной проекцией точки.
            //всё, что нам на самом деле нужно - это найти пересечение проецирующего луча на плоскости с окружностью радиуса планеты
            //использованные формулы взяты на http://e-maxx.ru/algo/circle_line_intersection

            //коэффициенты уравнения прямой на плоскости, проходящей через точки (-fCenterLength, fCameraScreenDistance) и (pRelativeMousePos.Length(), 0)
            //уравнение прямой по двум точкам: (y1-y2)*x + (x2-x1)*y + (x1*y2 - x2*y1) = 0  <http://www.math.by/geometry/eqline.html>
            float A = fCameraScreenDistance;
            float B = pRelativeMousePos.Length() + fCenterLength;
            float C = -pRelativeMousePos.Length() * fCameraScreenDistance;

            //некоторые промежуточные вычисления, для оптимизации
            float A2B2 = A * A + B * B;
            float R2 = m_fR * m_fR;
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
        /// Захватить точку для перемещения точки фокуса камеры на поверхности планеты
        /// </summary>
        /// <param name="startPoint">точка на поверхности планеты (под курсором)</param>
        /// <param name="m_fR">радиус планеты</param>
        public void StartDrag(Vector3 startPoint)
        {
            m_pStartFocusPoint = Vector3.Normalize(MapToSphere(startPoint));
            m_pStartFocusPointRotation = m_pFocusPointRotation;
        }

        /// <summary>
        /// Перетащить захваченную точку фокуса камеры на поверхности планеты в новые координаты
        /// </summary>
        /// <param name="currentPoint">новая точка на поверхности планеты (под курсором)</param>
        /// <param name="fR">радиус планеты</param>
        public void Drag(Vector3 currentPoint)
        {
            Vector3 pCurrentFocusPoint = Vector3.Normalize(MapToSphere(currentPoint));

            if (m_pStartFocusPoint.Equals(pCurrentFocusPoint))
                return;

            float fCos2A = Vector3.Dot(m_pStartFocusPoint, pCurrentFocusPoint);

            float fSinA = (float)Math.Sqrt((1.0 - fCos2A) * 0.5);
            float fCosA = (float)Math.Sqrt((1.0 + fCos2A) * 0.5);

            m_pFocusPointRotationAxis = Vector3.Normalize(Vector3.Cross(m_pStartFocusPoint, pCurrentFocusPoint)) * fSinA;

            Quaternion pFocusPointRotationDelta = new Quaternion(m_pFocusPointRotationAxis.X, m_pFocusPointRotationAxis.Y, m_pFocusPointRotationAxis.Z, -fCosA);

            m_pFocusPointRotation = Quaternion.Multiply(m_pStartFocusPointRotation, pFocusPointRotationDelta);
        }

        public void Orbit(float YawChange, float PitchChange, float RollChange)
        {
            //Yaw += RollChange;
            Pitch -= RollChange;

            //Pitch = Math.Max(MathHelper.ToRadians(1), Math.Min(MathHelper.ToRadians(90), this.Pitch));
            //this.Pitch = Math.Max(MathHelper.ToRadians(91), Math.Min(MathHelper.ToRadians(269), this.Pitch));

            //Roll += RollChange;
        }

        public void ZoomIn(float fDistance)
        {
            fDistance *= m_fDistance / 10000;

            m_fDistance -= fDistance;

            if (m_fDistance < 1)
                m_fDistance = 1;

            if (m_fDistance > 1000)
                m_fDistance = 1000;
        }        

        /// <summary>
        /// Обновить положение камеры
        /// </summary>
        public void Update()
        {
            UpdateAspectRatio();

            Matrix pFocusPointRotation = Matrix.CreateFromQuaternion(m_pFocusPointRotation);

            Vector3 pFocusPointDirection = Vector3.Transform(Vector3.Backward, pFocusPointRotation);
            FocusPoint = pFocusPointDirection * m_fR;

            Matrix cameraRotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);
            Vector3 camDir = Vector3.Transform(Vector3.Forward, cameraRotation);

            Direction = Vector3.Transform(camDir, pFocusPointRotation);
            Position = FocusPoint - Direction * m_fDistance;

            Vector3 cameraLeft = Vector3.Cross(FocusPoint, Direction);
            Top = Vector3.Normalize(Vector3.Cross(cameraLeft, Direction));

            View = Matrix.CreateLookAt(Position, FocusPoint, Top);
        }
    }
}
