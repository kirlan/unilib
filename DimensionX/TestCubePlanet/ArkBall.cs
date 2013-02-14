using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestCubePlanet
{
    //from http://rainwarrior.ca/dragon/arcball.html
    public class ArkBall
    {
        /// <summary>
        /// Actual rotation matrix
        /// </summary>
        public Matrix ab_quat = new Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        Matrix ab_last = new Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        Matrix ab_next = new Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

        // the distance from the origin to the eye
        float ab_zoom = 1.0f;
        float ab_zoom2 = 1.0f;
        // the radius of the arcball
        float ab_sphere = 1.0f;
        float ab_sphere2 = 1.0f;
        // the distance from the origin of the plane that intersects
        // the edge of the visible sphere (tangent to a ray from the eye)
        float ab_edge = 1.0f;
        // whether we are using a sphere or plane
        bool ab_planar = false;
        float ab_planedist = 0.5f;

        Vector3 ab_start = new Vector3(0, 0, 1);
        Vector3 ab_curr = new Vector3(0, 0, 1);
        public Vector3 ab_eye = new Vector3(0, 0, 1);
        public Vector3 ab_eyedir = new Vector3(0, 0, 1);
        public Vector3 ab_up = new Vector3(0, 1, 0);
        Vector3 ab_out = new Vector3(1, 0, 0);

        //Matrix ab_glp = new Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        //Matrix ab_glm = new Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        //Vector4 ab_glv = new Vector4(0,0,640,480);

        /// <summary>
        /// Call arcball_setzoom after setting up the projection matrix.
        /// You should call arcball_setzoom after use of gluLookAt. 
        /// gluLookAt(eye.x,eye.y,eye.z, ?,?,?, up.x,up.y,up.z); 
        /// The arcball derives its transformation information from the 
        /// openGL projection and viewport matrices. (modelview is ignored)
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="eye"></param>
        /// <param name="up"></param>
        public void arcball_setzoom(float radius, Vector3 eye, Vector3 up)
        {
            ab_eye = eye; // store eye vector
            ab_zoom2 = Vector3.Dot(ab_eye, ab_eye);
            ab_zoom = (float)Math.Sqrt(ab_zoom2); // store eye distance
            ab_sphere = radius; // sphere radius
            ab_sphere2 = ab_sphere * ab_sphere;
            ab_eyedir = ab_eye * (1.0f / ab_zoom); // distance to eye
            ab_edge = ab_sphere2 / ab_zoom; // plane of visible edge

            if (ab_sphere <= 0.0) // trackball mode
            {
                ab_planar = true;
                ab_up = up;
                ab_out = Vector3.Cross(ab_eyedir, ab_up);
                ab_planedist = (0.0f - ab_sphere) * ab_zoom;
            }
            else
                ab_planar = false;

            //glGetDoublev(GL_PROJECTION_MATRIX, ab_glp);
            //glGetIntegerv(GL_VIEWPORT,ab_glv);
        }

        // find the intersection with the plane through the visible edge
        Vector3 edge_coords(Vector3 m)
        {
            // find the intersection of the edge plane and the ray
            float t = (ab_edge - ab_zoom) / Vector3.Dot(ab_eyedir, m);
            Vector3 a = ab_eye + (m * t);
            // find the direction of the eye-axis from that point
            // along the edge plane
            Vector3 c = (ab_eyedir * ab_edge) - a;

            // find the intersection of the sphere with the ray going from
            // the plane outside the sphere toward the eye-axis.
            float ac = Vector3.Dot(a, c);
            float c2 = Vector3.Dot(c, c);
            float q = (0.0f - ac - (float)Math.Sqrt(ac * ac - c2 * (Vector3.Dot(a, a) - ab_sphere2))) / c2;

            return Vector3.Normalize(a + (c * q));
        }

        // find the intersection with the sphere
        Vector3 sphere_coords(Vector3 pTarget)
        {
            //pTarget.Normalize();
            Vector3 m = pTarget - ab_eye;

            // mouse position represents ray: eye + t*m
            // intersecting with a sphere centered at the origin
            float a = Vector3.Dot(m, m);
            float b = Vector3.Dot(ab_eye, m);
            float root = (b * b) - a * (ab_zoom2 - ab_sphere2);
            if (root <= 0) return edge_coords(m);
            float t = (0.0f - b - (float)Math.Sqrt(root)) / a;
            return Vector3.Normalize(ab_eye + (m * t));
        }

        // get intersection with plane for "trackball" style rotation
        Vector3 planar_coords(Vector3 pTarget)
        {
            //pTarget.Normalize();
            Vector3 m = pTarget - ab_eye;
            // intersect the point with the trackball plane
            float t = (ab_planedist - ab_zoom) / Vector3.Dot(ab_eyedir, m);
            Vector3 d = ab_eye + m * t;

            return new Vector3(Vector3.Dot(d, ab_up), Vector3.Dot(d, ab_out), 0.0f);
        }

        /// <summary>
        /// Call arcball_reset if you wish to reset the arcball rotation.
        /// </summary>
        public void arcball_reset()
        {
            // reset the arcball
            ab_quat = Matrix.Identity;
            ab_last = Matrix.Identity;
            //quatidentity(ref ab_quat);
            //quatidentity(ref ab_last);
        }

        /// <summary>
        ///  Call arcball_start with a mouse position, and the arcball will
        ///  be ready to manipulate. (Call on mouse button down.)
        /// </summary>
        /// <param name="pTarget"></param>
        public void arcball_start(Vector3 pTarget)
        {
            // begin arcball rotation

            // saves a copy of the current rotation for comparison
            ab_last = ab_quat;
            //quatcopy(ref ab_last, ab_quat);
            if (ab_planar)
                ab_start = planar_coords(pTarget);
            else
                //ab_start = sphere_coords(pTarget);
                ab_start = Vector3.Normalize(pTarget);// sphere_coords(pTarget);
        }

        /// <summary>
        ///  Call arcball_move with a mouse position, and the arcball will
        ///  find the rotation necessary to move the start mouse position to
        ///  the current mouse position on the sphere. (Call on mouse move.)
        /// </summary>
        /// <param name="pTarget"></param>
        public void arcball_move(Vector3 pTarget)
        {
            // update current arcball rotation
            if (ab_planar)
            {
                ab_curr = planar_coords(pTarget);
                if (ab_curr.Equals(ab_start))
                    return;

                // d is motion since the last position
                Vector3 d = ab_curr - ab_start;

                float angle = d.Length() * 0.5f;
                float cosa = (float)Math.Cos(angle);
                float sina = (float)Math.Sin(angle);
                // p is perpendicular to d
                Vector3 p = Vector3.Normalize((ab_out * d.X) - (ab_up * d.Y)) * sina;

                ab_next = Matrix.CreateFromQuaternion(new Quaternion(p, cosa));
                //quaternion(ref ab_next, p.X, p.Y, p.Z, cosa);
                ab_quat = Matrix.Multiply(ab_last, ab_next);
                //quatnext(ref ab_quat, ab_last, ab_next);
                // planar style only ever relates to the last point
                ab_last = ab_quat;
                //quatcopy(ref ab_last, ab_quat);
                ab_start = ab_curr;

            }
            else
            {
                ab_curr = Vector3.Normalize(pTarget);// sphere_coords(pTarget);
                //ab_curr = sphere_coords(pTarget);
                if (ab_curr.Equals(ab_start))
                { // avoid potential rare divide by tiny
                    ab_quat = ab_last;
                    //quatcopy(ref ab_quat, ab_last);
                    return;
                }

                // use a dot product to get the angle between them
                // use a cross product to get the vector to rotate around
                float cos2a = Vector3.Dot(ab_start, ab_curr);
                //float sina = (float)Math.Sqrt((1.0 - cos2a*cos2a));
                float sina = (float)Math.Sqrt((1.0 - cos2a) * 0.5);
                float cosa = (float)Math.Sqrt((1.0 + cos2a) * 0.5);
                Vector3 cross = Vector3.Normalize(Vector3.Cross(ab_start, ab_curr)) * sina;
                //Vector3 cross = Vector3.Cross(ab_start, ab_curr);// *sina;
                ab_next = Matrix.CreateFromQuaternion(new Quaternion(cross, -cosa));
                //quaternion(ref ab_next, cross.X, cross.Y, cross.Z, cosa);

                // update the rotation matrix
                ab_quat = Matrix.Multiply(ab_last, ab_next);
                //quatnext(ref ab_quat, ab_last, ab_next);
            }
        }
    }

    public class ArcBallCamera
    {
        private ArkBall m_pBall = new ArkBall();

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

            m_pBall.arcball_setzoom(1500, Position, Vector3.Up);
        }

        public void UpdateAspectRatio()
        {
            generatePerspectiveProjectionMatrix(Microsoft.Xna.Framework.MathHelper.ToRadians(45));
        }

        public Vector3 startVector;
        private Quaternion quatRotation = Quaternion.Identity;
        private Quaternion m_StartRot;

        /// <summary>
        /// Begin dragging
        /// </summary>
        /// <param name="startPoint">The X/Y position in your window at the beginning of dragging</param>
        /// <param name="rotation"></param>
        public void StartDrag(Vector3 startPoint)
        {
            //m_pBall.arcball_start(startPoint);
            startVector = Vector3.Normalize(startPoint);
            m_StartRot = quatRotation;
        }

        public Vector3 axis;

        public void Drag(Vector3 currentPoint)
        {
            //m_pBall.arcball_move(currentPoint);
            Vector3 currentVector = Vector3.Normalize(currentPoint);

            if (startVector.Equals(currentVector))
                return;

            float angle = Vector3.Dot(startVector, currentVector);

            //float sina = (float)Math.Sqrt((1.0 - cos2a*cos2a));
            float sina = (float)Math.Sqrt((1.0 - angle) * 0.5);
            float cosa = (float)Math.Sqrt((1.0 + angle) * 0.5);

            axis = Vector3.Normalize(Vector3.Cross(startVector, currentVector)) * sina;

            Quaternion delta = new Quaternion(axis.X, axis.Y, axis.Z, -cosa);

            //quatRotation = Quaternion.Multiply(quatRotation, delta);
            quatRotation = Quaternion.Multiply(m_StartRot, delta);

            //float cosAlpha = Vector3.Dot(startVector, currentVector);
            //Vector3 orthoVec = Vector3.Cross(startVector, currentVector);

            //quatRotation = m_StartRot * new Quaternion(-orthoVec, cosAlpha);
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

            Direction = Vector3.Transform(Vector3.Forward, cameraRotation);//m_pBall.ab_quat);
            Position = Vector3.Zero - Direction * m_fDistance;

            Top = Vector3.Transform(Vector3.Up, cameraRotation);//m_pBall.ab_quat);

            View = Matrix.CreateLookAt(Position, Vector3.Zero, Top);
        }
    }
}
