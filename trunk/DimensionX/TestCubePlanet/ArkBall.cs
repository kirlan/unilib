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
        }

        public void UpdateAspectRatio()
        {
            generatePerspectiveProjectionMatrix(Microsoft.Xna.Framework.MathHelper.ToRadians(45));
        }

        public void Initialize(float fR)
        {
            m_fR = fR;

            Yaw = MathHelper.ToRadians(90);
            Pitch = MathHelper.ToRadians(0);
            Roll = MathHelper.ToRadians(0);

            m_fDistance = 250;

            FocusPoint = Vector3.Backward * m_fR;
        }

        /// <summary>
        /// Сохранённая точка фокуса камеры на момент начала её перетаскивания
        /// ВАЖНО: координаты в системе ArkBall-полусферы!
        /// </summary>
        public Vector3 m_pStartCursorPoint = Vector3.Up*150;
        /// <summary>
        /// Текущий кватернион поворота точки фокуса камеры
        /// В мировых координатах.
        /// </summary>
        private Quaternion m_pCursorPointRotation = Quaternion.Identity;
        /// <summary>
        /// Сохранённый кватернион поворота точки фокуса камеры на момент начала перетаскивания
        /// В мировых координатах.
        /// </summary>
        private Quaternion m_pStartCursorPointRotation;


        public float m_fRadius;
        public float m_fMouseX;
        public float m_fMouseY;
        public Vector3 m_pArcBallMouseRelative;
        public Vector3 m_pArcBallCenter;
        private Matrix m_pCameraMatrix;

        /// <summary>
        /// ось поворота при перетаскивании точки фокуса камеры.
        /// ВАЖНО: координаты в системе ArkBall-полусферы!
        /// </summary>
        public Vector3 m_pCursorPointRotationAxis = Vector3.Forward*150;

        /// <summary>
        /// переводит мировые координаты точки на поверхности планеты (с рельефом) в координаты
        /// точки на поверхности ArkBall полусферы, всегда ориентированной по камере, основанием от наблюдателя
        /// </summary>
        /// <param name="pPoint">точка</param>
        /// <param name="m_fR">радиус планеты</param>
        /// <returns></returns>
        public Vector3 MapToSphere(Vector3 pPoint)
        {
            //экранные координаты точки на планетарной сфере под курсором
            Vector3 pMousePoint = GraphicsDevice.Viewport.Project(Vector3.Normalize(pPoint) * m_fR, Projection, View, Matrix.Identity);
            //экранные координаты центра планеты
            Vector3 pCenter = GraphicsDevice.Viewport.Project(Vector3.Zero, Projection, View, Matrix.Identity);

            //матрица вида с в камеро-центричной системе координат
            Matrix pArcBallView = Matrix.CreateLookAt(Vector3.Backward * m_fDistance, Vector3.Zero, Vector3.Up);

            //мировые координаты курсора в камеро-центричной системе координат
            Vector3 pArcBallMouse = GraphicsDevice.Viewport.Unproject(pMousePoint, Projection, pArcBallView, Matrix.Identity);
            m_fMouseX = pArcBallMouse.X;
            m_fMouseY = pArcBallMouse.Y;
            //мировые координаты центра плаенты в камеро-центричной системе координат
            m_pArcBallCenter = GraphicsDevice.Viewport.Unproject(pCenter, Projection, pArcBallView, Matrix.Identity);

            m_pCameraMatrix = Matrix.Identity;
            Vector3 pLeft = Vector3.Cross(Direction, Top);
            m_pCameraMatrix.M11 = pLeft.X;
            m_pCameraMatrix.M21 = pLeft.Y;
            m_pCameraMatrix.M31 = pLeft.Z;

            m_pCameraMatrix.M12 = Top.X;
            m_pCameraMatrix.M22 = Top.Y;
            m_pCameraMatrix.M32 = Top.Z;

            m_pCameraMatrix.M13 = -Direction.X;
            m_pCameraMatrix.M23 = -Direction.Y;
            m_pCameraMatrix.M33 = -Direction.Z;

            Vector3 pArcBallMouseBis = Vector3.Transform(Vector3.Normalize(pPoint) * m_fR, m_pCameraMatrix);

            //координаты курсора относительно центра планеты в камеро-центричной системе координат
            //т.е. фактически в системе ArkBall-полусферы
            m_pArcBallMouseRelative = pArcBallMouse - m_pArcBallCenter;

            Vector3 pArcBallMouseRelative = m_pArcBallMouseRelative + m_pArcBallCenter;

            Vector3 pArcBallMouse2 = GraphicsDevice.Viewport.Project(pArcBallMouseRelative, Projection, pArcBallView, Matrix.Identity);

            Vector3 pMousePoint2 = Vector3.Normalize(GraphicsDevice.Viewport.Unproject(pArcBallMouse2, Projection, View, Matrix.Identity)) * m_fR;

            Matrix pCameraMatrixReverce = Matrix.Invert(m_pCameraMatrix);

            Vector3 pMousePoint2bis = Vector3.Transform(pArcBallMouseBis, pCameraMatrixReverce);

            Vector3 pMousePoint3bis = Vector3.Transform(m_pStartCursorPoint, pCameraMatrixReverce);

            m_pArcBallMouseRelative = pArcBallMouseBis;

            return m_pArcBallMouseRelative;
        }

        public Vector3 MapFromSphere(Vector3 pPoint)
        {
            //матрица вида с в камеро-центричной системе координат
            Matrix pArcBallView = Matrix.CreateLookAt(Vector3.Backward * m_fDistance, Vector3.Zero, Vector3.Up);

            Vector3 pArcBallMouseRelative = pPoint + m_pArcBallCenter;

            Vector3 pArcBallMouse = GraphicsDevice.Viewport.Project(pArcBallMouseRelative, Projection, pArcBallView, Matrix.Identity);

            Vector3 pMousePoint = Vector3.Normalize(GraphicsDevice.Viewport.Unproject(pArcBallMouse, Projection, View, Matrix.Identity)) * m_fR;

            Matrix pCameraMatrixReverce = Matrix.Invert(m_pCameraMatrix);
            Vector3 pMousePoint2bis = Vector3.Transform(pPoint, pCameraMatrixReverce);

            return pMousePoint2bis;
            //return pMousePoint;
        }

        /// <summary>
        /// Захватить точку для перемещения точки фокуса камеры на поверхности планеты
        /// </summary>
        /// <param name="startPoint">точка на поверхности планеты (под курсором)</param>
        /// <param name="m_fR">радиус планеты</param>
        public void StartDrag(Vector3 startPoint)
        {
            m_pStartCursorPoint = Vector3.Normalize(startPoint);// MapToSphere(startPoint);
            m_pStartCursorPointRotation = m_pCursorPointRotation;
        }

        public Vector3 m_pAxis = Vector3.Up;
        public Vector3 m_pStart = Vector3.Forward;

        /// <summary>
        /// Перетащить захваченную точку фокуса камеры на поверхности планеты в новые координаты
        /// </summary>
        /// <param name="currentPoint">новая точка на поверхности планеты (под курсором)</param>
        /// <param name="fR">радиус планеты</param>
        public void Drag(Vector3 currentPoint)
        {
            Vector3 pStartCursorPointScreen = GraphicsDevice.Viewport.Project(Vector3.Normalize(m_pStartCursorPoint) * m_fR, Projection, View, Matrix.Identity);
            Vector3 pCursorPointScreen = GraphicsDevice.Viewport.Project(Vector3.Normalize(currentPoint) * m_fR, Projection, View, Matrix.Identity);

            Vector3 pStartCursorPoint = m_pStartCursorPoint;// MapFromSphere(m_pStartCursorPoint);
            //Vector3 pCurrentCursorPoint = Vector3.Normalize(MapToSphere(currentPoint));
            Vector3 pCurrentCursorPoint = Vector3.Normalize(currentPoint);

            if (pStartCursorPoint.Equals(pCurrentCursorPoint))
                return;

            float fCos2A = Math.Min(1, Vector3.Dot(Vector3.Normalize(pStartCursorPoint), Vector3.Normalize(pCurrentCursorPoint)));

            float fSinA = (float)Math.Sqrt((1.0 - fCos2A) * 0.5);
            float fCosA = (float)Math.Sqrt((1.0 + fCos2A) * 0.5);

            m_pCursorPointRotationAxis = Vector3.Normalize(Vector3.Cross(pStartCursorPoint, pCurrentCursorPoint)) * fSinA;

            if (float.IsNaN(m_pCursorPointRotationAxis.X) ||
                float.IsNaN(m_pCursorPointRotationAxis.Y) ||
                float.IsNaN(m_pCursorPointRotationAxis.Z))
                throw new Exception();

            Quaternion pFocusPointRotationDelta = new Quaternion(m_pCursorPointRotationAxis.X, m_pCursorPointRotationAxis.Y, m_pCursorPointRotationAxis.Z, -fCosA);

            m_pCursorPointRotation = Quaternion.Multiply(m_pStartCursorPointRotation, pFocusPointRotationDelta);

            //StartDrag(currentPoint);
            //m_pStartCursorPointRotation = m_pCursorPointRotation;
        
            Matrix pFocusPointRotation = Matrix.CreateFromQuaternion(m_pCursorPointRotation);

            Vector3 pFocusPointDirection = Vector3.Transform(Vector3.Backward, pFocusPointRotation);
            Vector3 pFocusPoint = pFocusPointDirection * m_fR;

            //Matrix cameraRotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);
            Matrix cameraRotationYaw = Matrix.CreateFromAxisAngle(Vector3.Backward, Yaw);
            Matrix cameraRotationPitch = Matrix.CreateFromAxisAngle(Vector3.Left, Pitch);
            Vector3 camDir = Vector3.Transform(Vector3.Up, cameraRotationPitch);
            camDir = Vector3.Transform(camDir, cameraRotationYaw);

            Vector3 pDirection = Vector3.Transform(camDir, pFocusPointRotation);
            Vector3 pPosition = pFocusPoint - pDirection * m_fDistance;

            Vector3 cameraLeft = Vector3.Cross(pFocusPoint, pDirection);
            Vector3 pTop = Vector3.Normalize(Vector3.Cross(-cameraLeft, pDirection));

            Matrix pView = Matrix.CreateLookAt(pPosition, pFocusPoint, pTop);

            Vector3 pEndCursorPointScreen = GraphicsDevice.Viewport.Project(Vector3.Normalize(m_pStartCursorPoint) * m_fR, Projection, pView, Matrix.Identity);
            Vector3 pNewStartCursorPoint = Vector3.Normalize(GraphicsDevice.Viewport.Unproject(pStartCursorPointScreen, Projection, pView, Matrix.Identity));
            Vector3 pEndCursorPointScreen2 = GraphicsDevice.Viewport.Project(Vector3.Normalize(pNewStartCursorPoint) * m_fR, Projection, pView, Matrix.Identity);

            m_pStartCursorPoint = pNewStartCursorPoint;
        }

        public void Orbit(float YawChange, float PitchChange, float RollChange)
        {
            Yaw += YawChange;
            Pitch -= PitchChange;

            Pitch = Math.Max(MathHelper.ToRadians(1), Math.Min(MathHelper.ToRadians(89), this.Pitch));
            //this.Pitch = Math.Max(MathHelper.ToRadians(91), Math.Min(MathHelper.ToRadians(269), this.Pitch));

            Roll += RollChange;
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

            Matrix pFocusPointRotation = Matrix.CreateFromQuaternion(m_pCursorPointRotation);

            Vector3 pFocusPointDirection = Vector3.Transform(Vector3.Backward, pFocusPointRotation);
            FocusPoint = pFocusPointDirection * m_fR;

            //Matrix cameraRotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);
            Matrix cameraRotationYaw = Matrix.CreateFromAxisAngle(Vector3.Backward, Yaw);
            Matrix cameraRotationPitch = Matrix.CreateFromAxisAngle(Vector3.Left, Pitch);
            Vector3 camDir = Vector3.Transform(Vector3.Up, cameraRotationPitch);
            camDir = Vector3.Transform(camDir, cameraRotationYaw);

            Direction = Vector3.Transform(camDir, pFocusPointRotation);
            Position = FocusPoint - Direction * m_fDistance;

            Vector3 cameraLeft = Vector3.Cross(FocusPoint, Direction);
            Top = Vector3.Normalize(Vector3.Cross(-cameraLeft, Direction));

            View = Matrix.CreateLookAt(Position, FocusPoint, Top);

            m_pAxis = Vector3.Normalize(m_pCursorPointRotationAxis)*m_fR;
            m_pStart = m_pStartCursorPoint * m_fR;// MapFromSphere(m_pStartCursorPoint);
        }

        public float m_fCameraLength { get; set; }
    }
}
