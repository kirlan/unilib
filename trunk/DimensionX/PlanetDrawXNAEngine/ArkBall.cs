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

        private float m_fStoredAspectRatio = 0;
        private float m_fStoredFieldOfView = 0;

        private bool generatePerspectiveProjectionMatrix(float FieldOfView)
        {
            //PresentationParameters pp = GraphicsDevice.PresentationParameters;
            if (m_fStoredAspectRatio != GraphicsDevice.Viewport.AspectRatio || m_fStoredFieldOfView != FieldOfView)
            {
                Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, GraphicsDevice.Viewport.AspectRatio, 0.1f, 800f);
                m_fStoredAspectRatio = GraphicsDevice.Viewport.AspectRatio;
                m_fStoredFieldOfView = FieldOfView;
                return true;
            }
            return false;
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

        public bool UpdateAspectRatio()
        {
            return generatePerspectiveProjectionMatrix(Microsoft.Xna.Framework.MathHelper.ToRadians(45));
        }

        public void Initialize(float fR)
        {
            m_fR = fR;

            Yaw = MathHelper.ToRadians(0);
            Pitch = MathHelper.ToRadians(20);
            Roll = MathHelper.ToRadians(0);

            m_fDistance = m_fFutureDistance;// 250;
            
            Pitch = 0.1f*(float)Math.Sqrt(m_fDistance);

            FocusPoint = Vector3.Backward * m_fR;
        }

        public Vector3 m_pTarget = Vector3.Backward * 150;
        public Vector3 m_pTargetDir = Vector3.Up;

        private bool m_bNeedUpdate = true;

        private float m_fFutureYaw = 0;

        public void Orbit(float YawChange, float PitchChange, float RollChange)
        {
            if (YawChange == 0 && PitchChange == 0 && RollChange == 0)
                return;

            m_fFutureYaw += YawChange;
            //Pitch -= PitchChange;

            //Pitch = Math.Max(MathHelper.ToRadians(1), Math.Min(MathHelper.ToRadians(89), this.Pitch));

            //Roll += RollChange;

            m_bNeedUpdate = true;
        }

        private float m_fFutureDistance = 8;

        public void ZoomIn(float fDistance)
        {
            if (fDistance == 0)
                return;

            fDistance *= 0.01f;// (float)Math.Sqrt(m_fDistance) / 600;

            m_fFutureDistance -= fDistance;

            if (m_fFutureDistance < 2.2)
                m_fFutureDistance = 2.2f;

            if (m_fFutureDistance > 246)
                m_fFutureDistance = 246;

            m_bNeedUpdate = true;
        }

        public void MoveTarget(Vector3 pNewPosition, float fDistance)
        {
            if (fDistance == 0)
                return;

            Vector3 pAxis = Vector3.Cross(m_pTarget, pNewPosition);
            pAxis.Normalize();

            float fCosA = Vector3.Dot(Vector3.Normalize(m_pTarget), Vector3.Normalize(pNewPosition));
            if (fCosA >= 1f - 1e-6f)
                return;

            float fRotAngle = MathHelper.Pi;

            if (fCosA < (1e-6f - 1.0f))
            {
                pAxis = Vector3.Cross(Vector3.UnitX, m_pTarget);

                if (pAxis.LengthSquared() == 0)
                {
                    pAxis = Vector3.Cross(Vector3.UnitY, m_pTarget);
                }

                pAxis.Normalize();
            }
            else
            {

                fRotAngle = (float)Math.Acos(fCosA);
            }

            if (float.IsNaN(fRotAngle))
                throw new Exception();

            if (fRotAngle > MathHelper.ToRadians(fDistance))
                fRotAngle = MathHelper.ToRadians(fDistance);
            else
                fRotAngle /= 2;

            if (fRotAngle == 0)
                return;

            Vector3 pDelta1 = Vector3.Transform(m_pTarget, Matrix.CreateFromAxisAngle(pAxis, fRotAngle));

            Vector3 pDelta = pDelta1 - m_pTarget;
            
            Vector3 pNewTarget = Vector3.Normalize(m_pTarget + pDelta) * m_fR;

            m_pTargetDir = Vector3.Normalize(Vector3.Normalize(m_pTarget + pDelta + m_pTargetDir) * m_fR - pNewTarget);

            //m_cTargets.Add(new KeyValuePair<Vector3, Vector3>(pTarget1, m_pTargetDir));
            m_pTarget = pNewTarget;

            m_bNeedUpdate = true;
        }

        public Vector3 Left = Vector3.Left;

        /// <summary>
        /// Обновить положение камеры
        /// </summary>
        public bool Update()
        {
            if (!UpdateAspectRatio() && !m_bNeedUpdate)
                return false;

            m_fDistance = (m_fDistance * 2 + m_fFutureDistance) / 3;
            if (Math.Abs(m_fDistance - m_fFutureDistance) < 0.5)
                m_fDistance = m_fFutureDistance;

            Yaw = (Yaw * 2 + m_fFutureYaw) / 3;
            if (Math.Abs(Yaw - m_fFutureYaw) < 0.01)
                Yaw = m_fFutureYaw;

            Pitch = 0.1f * (float)Math.Sqrt(m_fDistance);

            //Matrix pFocusPointRotation = Matrix.CreateFromQuaternion(m_pCursorPointRotation);
            //Matrix pFocusPointRotation1 = Matrix.CreateFromQuaternion(m_pTargetRotation1);
            //Matrix pFocusPointRotation2 = Matrix.Identity;//CreateFromQuaternion(m_pTargetRotation2);

            //Vector3 pFocusPointDirection = Vector3.Transform(Vector3.Backward, pFocusPointRotation1);
            //pFocusPointDirection = Vector3.Transform(pFocusPointDirection, pFocusPointRotation2);
            //FocusPoint = pFocusPointDirection * m_fR;

            FocusPoint = m_pTarget;// + Vector3.Normalize(m_pTarget) * 0.2f;

            Matrix cameraRotationYaw = Matrix.CreateFromAxisAngle(Vector3.Normalize(FocusPoint), Yaw);
            Matrix cameraRotationPitch = Matrix.CreateFromAxisAngle(Vector3.Cross(Vector3.Normalize(FocusPoint), m_pTargetDir), Pitch);

            Vector3 camDir = Vector3.Transform(m_pTargetDir, cameraRotationPitch);
            Direction = Vector3.Transform(camDir, cameraRotationYaw);

            //Direction = Vector3.Transform(camDir, pFocusPointRotation1);
            //Direction = Vector3.Transform(Direction, pFocusPointRotation2);
            Position = FocusPoint +  - Direction * m_fDistance;

            Left = Vector3.Cross(FocusPoint, Direction);
            Top = Vector3.Normalize(Vector3.Cross(-Left, Direction));

            View = Matrix.CreateLookAt(Position, FocusPoint, Top);

            m_bNeedUpdate = (m_fDistance != m_fFutureDistance) || (Yaw != m_fFutureYaw);

            return true;
        }

        public float m_fCameraLength { get; set; }
    }
}
