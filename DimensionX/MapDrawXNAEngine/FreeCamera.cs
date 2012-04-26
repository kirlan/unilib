using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapDrawXNAEngine
{
    public class FreeCamera : Camera
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Target { get; private set; }
        public Vector3 Direction { get; private set; }

        private Vector3 translation;
        private Vector3 translationJump;
        private Vector3 translationPan;

        private Vector3 pPOI = Vector3.Zero;

        private float m_fDistance;
        
        public FreeCamera(Vector3 Position, float Yaw, float Pitch, float Roll, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.Position = Position;
            this.Yaw = Yaw;
            this.Pitch = Pitch;
            this.Roll = Roll;
            translation = Vector3.Zero;
            translationJump = Vector3.Zero;
            translationPan = Vector3.Zero;

            m_fDistance = Vector3.Distance(Position, pPOI);
        }

        public void Orbit(float YawChange, float PitchChange, float RollChange)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;

            this.Pitch = Math.Max(MathHelper.ToRadians(270), Math.Min(MathHelper.ToRadians(359), this.Pitch));

            this.Roll += RollChange;
        }

        public void Jump(Vector3 Translation)
        {
            this.translationJump += Translation;
        }

        public void Move(Vector3 Translation)
        {
            this.translation += Translation;
        }

        public void Move(float fLeft, float fUp)
        {
            Vector3 pMove = new Vector3(fLeft, -fUp, 0);
            this.translationPan += pMove;
        }

        public void MoveForward(float fDistance)
        {
            //Vector3 VTrans = Vector3.Forward;//new Vector3(0, 0, -1);
            //// Move 3 units per millisecond, independent of frame rate
            //VTrans *= fDistance;
            //Move(VTrans);

            m_fDistance -= fDistance;

            if (m_fDistance < 0)
                m_fDistance = 0;
        }

        public override void Update()
        {
            UpdateAspectRatio();

//            Matrix cameraRotation = Matrix.CreateRotationY(Yaw) * Matrix.CreateRotationX(Pitch);
            Matrix cameraRotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);

            // Offset the position and reset the translation
            translation = Vector3.Transform(translation, cameraRotation);
            //Position += translation;
            pPOI += translation;

            translation = Vector3.Zero;

            // Offset the position and reset the translation
            translationPan = Vector3.Transform(translationPan, cameraRotation);
            translationPan = Vector3.Transform(translationPan, Matrix.CreateScale(1, 0, 1));
            //Position += translationPan;
            pPOI += translationPan;

            translationPan = Vector3.Zero;

            //Position += translationJump;
            pPOI += translationJump;

            translationJump = Vector3.Zero;

            pPOI = Vector3.Transform(pPOI, Matrix.CreateScale(1, 0, 1));

            Vector3 pOldDirection = Direction;

//            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
//            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
            Vector3 cameraOriginalTarget = Vector3.Forward;//new Vector3(0, 1, 0);
            Direction = Vector3.Transform(cameraOriginalTarget, cameraRotation);

            Vector3 cameraOrbit = Vector3.Zero;

            //float distance = Vector3.Distance(Position, pPOI);
            cameraOrbit = pPOI - Direction * m_fDistance;

            Position = cameraOrbit;

            Vector3 cameraFinalTarget = pPOI;

            Vector3 cameraOriginalUpVector = Vector3.Up;//new Vector3(0, 0, 1);
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            View = Matrix.CreateLookAt(Position, cameraFinalTarget, cameraRotatedUpVector);
            
            /*
            // Calculate the rotation matrix
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);
            // Offset the position and reset the translation
            translation = Vector3.Transform(translation, rotation);
            Position += translation;
            translation = Vector3.Zero;
            // Calculate the new target
            Direction = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + Direction;
            // Calculate the up vector
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);
            // Calculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
             */
        }
    }
}
