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
        Matrix ab_quat = new Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
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
        Vector3 ab_eye = new Vector3(0, 0, 1);
        Vector3 ab_eyedir = new Vector3(0, 0, 1);
        Vector3 ab_up = new Vector3(0, 1, 0);
        Vector3 ab_out = new Vector3(1, 0, 0);

        Matrix ab_glp = new Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        Matrix ab_glm = new Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
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

            glGetDoublev(GL_PROJECTION_MATRIX, ab_glp);
            //glGetIntegerv(GL_VIEWPORT,ab_glv);
        }

        /// <summary>
        /// Call arcball_rotate after resetting the modelview matrix in your 
        /// drawing code. It will call glRotate with its current rotation.
        /// </summary>
        public void arcball_rotate()
        {
            // affect the arcball's orientation on openGL
            glMultMatrixf(ab_quat);
        }

        // convert the quaternion into a rotation matrix
        static void quaternion(ref Matrix q, float x, float y, float z, float w)
        {
            float x2 = x * x;
            float y2 = y * y;
            float z2 = z * z;
            float xy = x * y;
            float xz = x * z;
            float yz = y * z;
            float wx = w * x;
            float wy = w * y;
            float wz = w * z;

            q.M11 = 1 - 2 * y2 - 2 * z2;
            q.M12 = 2 * xy + 2 * wz;
            q.M13 = 2 * xz - 2 * wy;

            q.M21 = 2 * xy - 2 * wz;
            q.M22 = 1 - 2 * x2 - 2 * z2;
            q.M23 = 2 * yz + 2 * wx;

            q.M31 = 2 * xz + 2 * wy;
            q.M32 = 2 * yz - 2 * wx;
            q.M33 = 1 - 2 * x2 - 2 * y2;
        }

        // reset the rotation matrix
        static void quatidentity(ref Matrix q)
        {
            //q.M11=1;  q.M12=0;  q.M13=0;  q.M14=0;
            //q.M21=0;  q.M22=1;  q.M23=0;  q.M24=0;
            //q.M31=0;  q.M32=0;  q.M33=1;  q.M34=0;
            //q.M41=0;  q.M42=0;  q.M43=0;  q.M44=1; 
            q = Matrix.Identity;
        }

        // copy a rotation matrix
        static void quatcopy(ref Matrix dst, Matrix src)
        {
            //dst.M11=src.M11; dst.M12=src.M12; dst.M13=src.M13;
            //dst.M21=src.M21; dst.M22=src.M22; dst.M23=src.M23;
            //dst.M31=src.M31; dst.M32=src.M32; dst.M33=src.M33; 
            dst = src;
        }

        // multiply two rotation matrices
        static void quatnext(ref Matrix dest, Matrix left, Matrix right)
        {
            //dest[0] = left[0]*right[0] + left[1]*right[4] + left[2] *right[8];
            //dest[1] = left[0]*right[1] + left[1]*right[5] + left[2] *right[9];
            //dest[2] = left[0]*right[2] + left[1]*right[6] + left[2] *right[10];
            //dest[4] = left[4]*right[0] + left[5]*right[4] + left[6] *right[8];
            //dest[5] = left[4]*right[1] + left[5]*right[5] + left[6] *right[9];
            //dest[6] = left[4]*right[2] + left[5]*right[6] + left[6] *right[10];
            //dest[8] = left[8]*right[0] + left[9]*right[4] + left[10]*right[8];
            //dest[9] = left[8]*right[1] + left[9]*right[5] + left[10]*right[9];
            //dest[10]= left[8]*right[2] + left[9]*right[6] + left[10]*right[10];

            dest = Matrix.Multiply(left, right);
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
            quatidentity(ref ab_quat);
            quatidentity(ref ab_last);
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
            quatcopy(ref ab_last, ab_quat);
            if (ab_planar)
                ab_start = planar_coords(pTarget);
            else
                ab_start = sphere_coords(pTarget);
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

                quaternion(ref ab_next, p.X, p.Y, p.Z, cosa);
                quatnext(ref ab_quat, ab_last, ab_next);
                // planar style only ever relates to the last point
                quatcopy(ref ab_last, ab_quat);
                ab_start = ab_curr;

            }
            else
            {
                ab_curr = sphere_coords(pTarget);
                if (ab_curr.Equals(ab_start))
                { // avoid potential rare divide by tiny
                    quatcopy(ref ab_quat, ab_last);
                    return;
                }

                // use a dot product to get the angle between them
                // use a cross product to get the vector to rotate around
                float cos2a = Vector3.Dot(ab_start, ab_curr);
                float sina = (float)Math.Sqrt((1.0 - cos2a) * 0.5);
                float cosa = (float)Math.Sqrt((1.0 + cos2a) * 0.5);
                Vector3 cross = Vector3.Normalize(Vector3.Cross(ab_start, ab_curr)) * sina;
                quaternion(ref ab_next, cross.X, cross.Y, cross.Z, cosa);

                // update the rotation matrix
                quatnext(ref ab_quat, ab_last, ab_next);
            }
        }
    }

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
