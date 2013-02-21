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

        public Vector3 m_pTarget = Vector3.Backward * 150;
        private Quaternion m_pTargetRotation1 = Quaternion.Identity;
        private Quaternion m_pTargetRotation2 = Quaternion.Identity;

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

        public List<KeyValuePair<Vector3, Vector3>> m_cTargets = new List<KeyValuePair<Vector3,Vector3>>();

        private Quaternion GetRotation(Vector3 src, Vector3 dest)
        {
            src.Normalize();
            dest.Normalize();

            float fCosA = Vector3.Dot(src, dest);

            if (fCosA >= 1f)
            {
                return Quaternion.Identity;
            }
            else
            {
                if (fCosA < (1e-6f - 1.0f))
                {
                    Vector3 axis = Vector3.Cross(Vector3.UnitX, src);

                    if (axis.LengthSquared() == 0)
                    {
                        axis = Vector3.Cross(Vector3.UnitY, src);
                    }

                    axis.Normalize();
                    return Quaternion.CreateFromAxisAngle(axis, MathHelper.Pi);
                }
                else
                {
                    float s = (float)Math.Sqrt((1 + fCosA) * 2);
                    float invS = 1 / s;

                    float fCosHalfA = (float)Math.Sqrt((1 + fCosA) / 2);
                    float fSinHalfA = (float)Math.Sqrt((1 - fCosA) / 2);

                    Vector3 c = Vector3.Cross(src, dest);
                    Quaternion q = new Quaternion(invS * c, 0.5f * s);
                    //Quaternion q = new Quaternion(fSinHalfA * c, fCosHalfA);
                    q.Normalize();

                    return q;
                }
            }
        }
        
        public void MoveTarget(Vector3 pNewPosition, float fDistance)
        {
            //Quaternion pTargetRotationDelta1 = GetRotation(Vector3.Backward, pNewPosition, MathHelper.ToRadians(180));

            m_pTargetRotation1 = GetRotation(Vector3.Backward, m_pTarget);
            Vector3 pDelta = pNewPosition - m_pTarget;
            if (pDelta.Length() > 10)
                pDelta = Vector3.Normalize(pDelta)*10;
            m_pTargetRotation2 = GetRotation(m_pTarget, m_pTarget + pDelta);

            //m_pTargetRotation = Quaternion.Lerp(m_pTargetRotation, pTargetRotationDelta1, 0.1f);

            Vector3 pTarget1 = Vector3.Transform(Vector3.Backward, Matrix.CreateFromQuaternion(m_pTargetRotation1)) * m_fR;
            Vector3 pTarget2 = Vector3.Transform(pTarget1, Matrix.CreateFromQuaternion(m_pTargetRotation2));

            //Vector3 pNewTarget = Vector3.Transform(Vector3.Backward, Matrix.CreateFromQuaternion(m_pTargetRotation1)) * m_fR;

            Vector3 pNewTarget = pTarget2;

            Vector3 pTargetDir1 = Vector3.Transform(Vector3.Backward*m_fR + Vector3.Up, Matrix.CreateFromQuaternion(m_pTargetRotation1));
            Vector3 pTargetDir2 = Vector3.Transform(pTargetDir1, Matrix.CreateFromQuaternion(m_pTargetRotation2));

            m_cTargets.Add(new KeyValuePair<Vector3, Vector3>(pTarget2, pTargetDir2 - pTarget2));
            m_pTarget = pNewTarget;

            return;
        }

        /// <summary>
        /// Обновить положение камеры
        /// </summary>
        public void Update()
        {
            UpdateAspectRatio();

            //Matrix pFocusPointRotation = Matrix.CreateFromQuaternion(m_pCursorPointRotation);
            Matrix pFocusPointRotation1 = Matrix.CreateFromQuaternion(m_pTargetRotation1);
            Matrix pFocusPointRotation2 = Matrix.CreateFromQuaternion(m_pTargetRotation2);

            Vector3 pFocusPointDirection = Vector3.Transform(Vector3.Backward, pFocusPointRotation1);
            pFocusPointDirection = Vector3.Transform(pFocusPointDirection, pFocusPointRotation2);
            FocusPoint = pFocusPointDirection * m_fR;

            Matrix cameraRotationYaw = Matrix.CreateFromAxisAngle(Vector3.Backward, Yaw);
            Matrix cameraRotationPitch = Matrix.CreateFromAxisAngle(Vector3.Left, Pitch);
            Vector3 camDir = Vector3.Transform(Vector3.Up, cameraRotationPitch);
            camDir = Vector3.Transform(camDir, cameraRotationYaw);

            Direction = Vector3.Transform(camDir, pFocusPointRotation1);
            Direction = Vector3.Transform(Direction, pFocusPointRotation2);
            Position = FocusPoint - Direction * m_fDistance;

            Vector3 cameraLeft = Vector3.Cross(FocusPoint, Direction);
            Top = Vector3.Normalize(Vector3.Cross(-cameraLeft, Direction));

            View = Matrix.CreateLookAt(Position, FocusPoint, Top);

            m_pAxis = Vector3.Normalize(m_pCursorPointRotationAxis)*m_fR;
            m_pStart = m_pStartCursorPoint * m_fR;
        }

        public float m_fCameraLength { get; set; }
    }
}
