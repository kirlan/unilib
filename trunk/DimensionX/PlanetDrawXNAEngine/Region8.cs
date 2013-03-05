using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TestCubePlanet
{
    /// <summary>
    /// Defines an irregular solid region with 8 boundary points.
    /// From http://stackoverflow.com/questions/8933958/creating-a-boundingfrustum-from-8-corners
    /// </summary>
    public struct Region8
    {
        //the corners of the region
        private Vector3 tfl, tfr, tbl, tbr, bfl, bfr, bbl, bbr, center, normal;
        #region public corner accessors
        public Vector3 TopFrontLeft { get { return tfl; } }
        public Vector3 TopFrontRight { get { return tfr; } }
        public Vector3 TopBackLeft { get { return tbl; } }
        public Vector3 TopBackRight { get { return tbr; } }
        public Vector3 BottomFrontLeft { get { return bfl; } }
        public Vector3 BottomFrontRight { get { return bfr; } }
        public Vector3 BottomBackLeft { get { return bbl; } }
        public Vector3 BottomBackRight { get { return bbr; } }
        public Vector3 Center { get { return center; } }
        public Vector3 Normal { get { return normal; } }
        #endregion
        //vertices to represent the corners for drawing
        private VertexPositionColor[] vertices;
        //planes which represent the faces of the region
        private Plane front, back, top, bottom, left, right;
        private Plane[] planes;
        public Plane[] Planes { get { return planes; } }
        public Plane Top { get { return top; } }
        //stores dot products used in collsions.
        public float[] dotProducts;
        public BoundingSphere m_pSphere;

        /// <summary>
        /// Creates a new Region defined by 8 arbitrary points. The faces of this region must be flat planes for it to work. Recommended for frustums.
        /// NOTE: to assign corners like "top front left" simply choose an arbitrary vantage point from which to view your region.
        /// However, it IS necessary that, from said vantage point, the corner names correspont to their positions!
        /// </summary>
        /// <param name="topFrontLeft">The corner on the top face of the region, toward the front and left</param>
        /// <param name="topFrontRight">The corner on the top face of the region, toward the front and right</param>
        /// <param name="topBackLeft">The corner on the top face of the region, toward the back and left</param>
        /// <param name="topBackRight">The corner on the top face of the region, toward the back and right</param>
        /// <param name="bottomFrontLeft">The corner on the bottom face of the region, toward the front and left</param>
        /// <param name="bottomFrontRight">The corner on the bottom face of the region, toward the front and right</param>
        /// <param name="bottomBackLeft">The corner on the bottom face of the region, toward the back and left</param>
        /// <param name="bottomBackRight">The corner on the bottom face of the region, toward the back and right</param>
        public Region8(Vector3 topFrontLeft, Vector3 topFrontRight, Vector3 topBackLeft, Vector3 topBackRight,
            Vector3 bottomFrontLeft, Vector3 bottomFrontRight, Vector3 bottomBackLeft, Vector3 bottomBackRight)
        {
            //assign corner values
            tfl = topFrontLeft;
            tfr = topFrontRight;
            tbl = topBackLeft;
            tbr = topBackRight;
            bfl = bottomFrontLeft;
            bfr = bottomFrontRight;
            bbl = bottomBackLeft;
            bbr = bottomBackRight;
            //create planes
            front = new Plane(tfl, tfr, bfl);
            back = new Plane(tbr, tbl, bbl);
            top = new Plane(tfr, tfl, tbl);
            bottom = new Plane(bfl, bfr, bbl);
            left = new Plane(tbl, tfl, bbl);
            right = new Plane(tfr, tbr, bbr);
            planes = new Plane[6] { front, back, top, bottom, left, right };
            vertices = new VertexPositionColor[8];

            //the average of the corners represents the center of the region
            center = tfl + tfr + tbl + tbr + bfl + bfr + bbl + bbr;
            center /= 8;

            normal = Vector3.Normalize(center);

            //Fix normals to point outward
            for (int p = 0; p < planes.Length; p++)
            {
                //choose the most convenient point on the plane
                Vector3 pointOnPlane = planes[p].Normal * -planes[p].D;
                //create a vector from the plane to the center
                Vector3 planeToAvg = center - pointOnPlane;
                //if the normal of the plane and the vector to the center point in the same direction, invert the normal
                if (Vector3.Dot(planeToAvg, planes[p].Normal) > 0)
                {
                    //normal inversion:
                    //reverse normal
                    planes[p].Normal = -planes[p].Normal;
                    //invert D
                    planes[p].D = -planes[p].D;
                    //normalize plane
                    planes[p].Normalize();
                }
            }

            dotProducts = new float[6];

            Vector3[] aVer = { bbl, bbr, bfl, bfr, tbl, tbr, tfl, tfr };
            m_pSphere = BoundingSphere.CreateFromPoints(aVer);

            vertices[0] = MakeVert(tfl);
            vertices[1] = MakeVert(tfr);
            vertices[2] = MakeVert(tbl);
            vertices[3] = MakeVert(tbr);
            vertices[4] = MakeVert(bfl);
            vertices[5] = MakeVert(bfr);
            vertices[6] = MakeVert(bbl);
            vertices[7] = MakeVert(bbr);
        }

        //private helper to make VertexPositionColors
        private VertexPositionColor MakeVert(Vector3 pos)
        {
            return new VertexPositionColor(pos, Color.Red);
        }

        /// <summary>
        /// Checks to see if the region intersects a ray. Returns the intersection depth of the ray if it does.
        /// </summary>
        /// <param name="ray">ray to intersect with this region</param>
        /// <returns>intersection depth, if any, of the ray</returns>
        public float? Intersects(Ray ray)
        {
            float? depth = null;
            int i = 0;
            for (int p = 0; p < planes.Length; p++)
            {
                Plane plane = planes[p];
                //calculate at what distance the ray intersects this plane.
                //a ray intersects a plane only once, so we only need to calc once.
                float? d = ray.Intersects(plane);
                //if the ray intersects (is not parallel), and either our best depth does not exist OR this intersection depth is less than the best so far
                if (d != null && (depth == null || d < depth))
                {
                    //calculate the point at which it intersects
                    Vector3 intersectionPoint = ray.Position + (ray.Direction * (float)d);
                    //set a flag for whether we are inside the region
                    bool isInside = true;
                    //for each other plane
                    for (int op = 0; op < planes.Length; op++)
                    {
                        //if this plane is not the same as the plane we intersected with
                        if (op != p)
                        {
                            Plane otherPlane = planes[op];
                            //calculate a point on the plane
                            Vector3 pointOnPlane = otherPlane.Normal * -otherPlane.D;
                            //generate a vector from the intersection point to the point on the plane
                            Vector3 vectorToPlane = pointOnPlane - intersectionPoint;
                            //get the dot product of the vectors
                            float intersectionDotPlane = Vector3.Dot(otherPlane.Normal, vectorToPlane);
                            //if the vectors are pointing in opposite directions, we are not inside the other planes
                            if (intersectionDotPlane <= 0)
                            {
                                //this intersection is disqualified
                                isInside = false;
                                i = 0;
                                break;
                            }
                            else
                                dotProducts[i++] = intersectionDotPlane;
                        }
                        else
                            dotProducts[i++] = 0;
                    }
                    //if the check says we are inside all other planes
                    if (isInside)
                    {
                        //set the best depth
                        depth = d;
                        if (i != 6)
                            throw new Exception(); //sanity check
                        i = 0;
                    }

                }
            }
            //return the best depth
            return depth;
        }

        /// <summary>
        /// Returns VertexPositionColors for the corners of the region.  Useful for debug drawing.
        /// </summary>
        /// <returns></returns>
        public VertexPositionColor[] GetVertices()
        {
            return vertices;
        }

        /// <summary>
        /// Returns an array of indices which correspond to GetVertices().  Useful for debug wireframe drawing (by lines, not triangles!).
        /// </summary>
        /// <returns></returns>
        public int[] GetIndices()
        {
            int[] indices = {
                                0,2,
                                0,1,
                                2,3,
                                1,3,
                                4,6,
                                6,7,
                                5,7,
                                4,5,
                                0,4,
                                2,6,
                                3,7,
                                1,5
                            };
            //int[] indices = {
            //                    0, 2, 1, //top
            //                    2, 3, 1,
            //                    5, 4, 0, //front
            //                    5, 0, 1,
            //                    6, 3, 2, //back
            //                    3, 6, 7,
            //                    4, 5, 6, //bottom
            //                    6, 5, 7,
            //                    2, 0, 6, //left
            //                    6, 0, 4,
            //                    1, 3, 5, //right
            //                    5, 3, 7
            //                };
            return indices;
        }

        public bool Intersects(BoundingFrustum pFrustum)
        {
            return pFrustum.Contains(bbl) != ContainmentType.Disjoint ||
                pFrustum.Contains(bbr) != ContainmentType.Disjoint ||
                pFrustum.Contains(bfl) != ContainmentType.Disjoint ||
                pFrustum.Contains(bfr) != ContainmentType.Disjoint ||
                pFrustum.Contains(tbl) != ContainmentType.Disjoint ||
                pFrustum.Contains(tbr) != ContainmentType.Disjoint ||
                pFrustum.Contains(tfl) != ContainmentType.Disjoint ||
                pFrustum.Contains(tfr) != ContainmentType.Disjoint ||
                pFrustum.Contains(center) != ContainmentType.Disjoint;


            //return Intersects(pFrustum, ref bbl, ref bbr, ref bfr) ||
            //        Intersects(pFrustum, ref bbr, ref bfr, ref bfl);// ||
                    //Intersects(pFrustum, ref tbl, ref tbr, ref tfr) ||
                    //Intersects(pFrustum, ref tbr, ref tfr, ref tfl);
        }

        /// <summary>
        /// Do a full perspective transform of the given vector by the given matrix,
        /// dividing out the w coordinate to return a Vector3 result.
        /// </summary>
        /// <param name="position">Vector3 of a point in space</param>
        /// <param name="matrix">4x4 matrix</param>
        /// <param name="result">Transformed vector after perspective divide</param>
        public static void PerspectiveTransform(ref Vector3 position, ref Matrix matrix, out Vector3 result)
        {
            float w = position.X * matrix.M14 + position.Y * matrix.M24 + position.Z * matrix.M34 + matrix.M44;
            float winv = 1.0f / w;

            float x = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
            float y = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
            float z = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;

            result = new Vector3();
            result.X = x * winv;
            result.Y = y * winv;
            result.Z = z * winv;
        }

        /// <summary>
        /// Returns true if the given frustum intersects the triangle (v0,v1,v2).
        /// </summary>
        private static bool Intersects(BoundingFrustum frustum, ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
        {
            // A BoundingFrustum is defined by a matrix that projects the frustum shape
            // into the box from (-1,-1,0) to (1,1,1). We will project the triangle
            // through this matrix, and then do a simpler box-triangle test.
            Matrix m = frustum.Matrix;
            Vector3 localTriV0;
            Vector3 localTriV1;
            Vector3 localTriV2;
            PerspectiveTransform(ref v0, ref m, out localTriV0);
            PerspectiveTransform(ref v1, ref m, out localTriV1);
            PerspectiveTransform(ref v2, ref m, out localTriV2);

            BoundingBox box;
            box.Min = new Vector3(-1, -1, 0);
            box.Max = new Vector3(1, 1, 1);

            return Intersects(ref box, ref localTriV0, ref localTriV1, ref localTriV2);
        }

        /// <summary>
        /// Returns true if the given box intersects the triangle (v0,v1,v2).
        /// </summary>
        private static bool Intersects(ref BoundingBox box, ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
        {
            Vector3 boxCenter = (box.Max + box.Min) * 0.5f;
            Vector3 boxHalfExtent = (box.Max - box.Min) * 0.5f;

            // Transform the triangle into the local space with the box center at the origin
            Vector3 localTriV0;
            Vector3 localTriV1;
            Vector3 localTriV2;
            Vector3.Subtract(ref v0, ref boxCenter, out localTriV0);
            Vector3.Subtract(ref v1, ref boxCenter, out localTriV1);
            Vector3.Subtract(ref v2, ref boxCenter, out localTriV2);

            return OriginBoxContains(ref boxHalfExtent, ref localTriV0, ref localTriV1, ref localTriV2) != ContainmentType.Disjoint;
        }

        /// <summary>
        /// Check if an origin-centered, axis-aligned box with the given half extents contains,
        /// intersects, or is disjoint from the given triangle. This is used for the box and
        /// frustum vs. triangle tests.
        /// </summary>
        private static ContainmentType OriginBoxContains(ref Vector3 halfExtent, ref Vector3 triV0, ref Vector3 triV1, ref Vector3 triV2)
        {
            BoundingBox triBounds = new BoundingBox(); // 'new' to work around NetCF bug
            triBounds.Min.X = Math.Min(triV0.X, Math.Min(triV1.X, triV2.X));
            triBounds.Min.Y = Math.Min(triV0.Y, Math.Min(triV1.Y, triV2.Y));
            triBounds.Min.Z = Math.Min(triV0.Z, Math.Min(triV1.Z, triV2.Z));

            triBounds.Max.X = Math.Max(triV0.X, Math.Max(triV1.X, triV2.X));
            triBounds.Max.Y = Math.Max(triV0.Y, Math.Max(triV1.Y, triV2.Y));
            triBounds.Max.Z = Math.Max(triV0.Z, Math.Max(triV1.Z, triV2.Z));

            Vector3 triBoundhalfExtent;
            triBoundhalfExtent.X = (triBounds.Max.X - triBounds.Min.X) * 0.5f;
            triBoundhalfExtent.Y = (triBounds.Max.Y - triBounds.Min.Y) * 0.5f;
            triBoundhalfExtent.Z = (triBounds.Max.Z - triBounds.Min.Z) * 0.5f;

            Vector3 triBoundCenter;
            triBoundCenter.X = (triBounds.Max.X + triBounds.Min.X) * 0.5f;
            triBoundCenter.Y = (triBounds.Max.Y + triBounds.Min.Y) * 0.5f;
            triBoundCenter.Z = (triBounds.Max.Z + triBounds.Min.Z) * 0.5f;

            if (triBoundhalfExtent.X + halfExtent.X <= Math.Abs(triBoundCenter.X) ||
                triBoundhalfExtent.Y + halfExtent.Y <= Math.Abs(triBoundCenter.Y) ||
                triBoundhalfExtent.Z + halfExtent.Z <= Math.Abs(triBoundCenter.Z))
            {
                return ContainmentType.Disjoint;
            }

            if (triBoundhalfExtent.X + Math.Abs(triBoundCenter.X) <= halfExtent.X &&
                triBoundhalfExtent.Y + Math.Abs(triBoundCenter.Y) <= halfExtent.Y &&
                triBoundhalfExtent.Z + Math.Abs(triBoundCenter.Z) <= halfExtent.Z)
            {
                return ContainmentType.Contains;
            }

            Vector3 edge1, edge2, edge3;
            Vector3.Subtract(ref triV1, ref triV0, out edge1);
            Vector3.Subtract(ref triV2, ref triV0, out edge2);

            Vector3 normal;
            Vector3.Cross(ref edge1, ref edge2, out normal);
            float triangleDist = Vector3.Dot(triV0, normal);
            if (Math.Abs(normal.X * halfExtent.X) + Math.Abs(normal.Y * halfExtent.Y) + Math.Abs(normal.Z * halfExtent.Z) <= Math.Abs(triangleDist))
            {
                return ContainmentType.Disjoint;
            }

            // Worst case: we need to check all 9 possible separating planes
            // defined by Cross(box edge,triangle edge)
            // Check for separation in plane containing an axis of box A and and axis of box B
            //
            // We need to compute all 9 cross products to find them, but a lot of terms drop out
            // since we're working in A's local space. Also, since each such plane is parallel
            // to the defining axis in each box, we know those dot products will be 0 and can
            // omit them.
            Vector3.Subtract(ref triV1, ref triV2, out edge3);
            float dv0, dv1, dv2, dhalf;

            // a.X ^ b.X = (1,0,0) ^ edge1
            // axis = Vector3(0, -edge1.Z, edge1.Y);
            dv0 = triV0.Z * edge1.Y - triV0.Y * edge1.Z;
            dv1 = triV1.Z * edge1.Y - triV1.Y * edge1.Z;
            dv2 = triV2.Z * edge1.Y - triV2.Y * edge1.Z;
            dhalf = Math.Abs(halfExtent.Y * edge1.Z) + Math.Abs(halfExtent.Z * edge1.Y);
            if (Math.Min(dv0, Math.Min(dv1, dv2)) >= dhalf || Math.Max(dv0, Math.Max(dv1, dv2)) <= -dhalf)
                return ContainmentType.Disjoint;

            // a.X ^ b.Y = (1,0,0) ^ edge2
            // axis = Vector3(0, -edge2.Z, edge2.Y);
            dv0 = triV0.Z * edge2.Y - triV0.Y * edge2.Z;
            dv1 = triV1.Z * edge2.Y - triV1.Y * edge2.Z;
            dv2 = triV2.Z * edge2.Y - triV2.Y * edge2.Z;
            dhalf = Math.Abs(halfExtent.Y * edge2.Z) + Math.Abs(halfExtent.Z * edge2.Y);
            if (Math.Min(dv0, Math.Min(dv1, dv2)) >= dhalf || Math.Max(dv0, Math.Max(dv1, dv2)) <= -dhalf)
                return ContainmentType.Disjoint;

            // a.X ^ b.Y = (1,0,0) ^ edge3
            // axis = Vector3(0, -edge3.Z, edge3.Y);
            dv0 = triV0.Z * edge3.Y - triV0.Y * edge3.Z;
            dv1 = triV1.Z * edge3.Y - triV1.Y * edge3.Z;
            dv2 = triV2.Z * edge3.Y - triV2.Y * edge3.Z;
            dhalf = Math.Abs(halfExtent.Y * edge3.Z) + Math.Abs(halfExtent.Z * edge3.Y);
            if (Math.Min(dv0, Math.Min(dv1, dv2)) >= dhalf || Math.Max(dv0, Math.Max(dv1, dv2)) <= -dhalf)
                return ContainmentType.Disjoint;

            // a.Y ^ b.X = (0,1,0) ^ edge1
            // axis = Vector3(edge1.Z, 0, -edge1.X);
            dv0 = triV0.X * edge1.Z - triV0.Z * edge1.X;
            dv1 = triV1.X * edge1.Z - triV1.Z * edge1.X;
            dv2 = triV2.X * edge1.Z - triV2.Z * edge1.X;
            dhalf = Math.Abs(halfExtent.X * edge1.Z) + Math.Abs(halfExtent.Z * edge1.X);
            if (Math.Min(dv0, Math.Min(dv1, dv2)) >= dhalf || Math.Max(dv0, Math.Max(dv1, dv2)) <= -dhalf)
                return ContainmentType.Disjoint;

            // a.Y ^ b.X = (0,1,0) ^ edge2
            // axis = Vector3(edge2.Z, 0, -edge2.X);
            dv0 = triV0.X * edge2.Z - triV0.Z * edge2.X;
            dv1 = triV1.X * edge2.Z - triV1.Z * edge2.X;
            dv2 = triV2.X * edge2.Z - triV2.Z * edge2.X;
            dhalf = Math.Abs(halfExtent.X * edge2.Z) + Math.Abs(halfExtent.Z * edge2.X);
            if (Math.Min(dv0, Math.Min(dv1, dv2)) >= dhalf || Math.Max(dv0, Math.Max(dv1, dv2)) <= -dhalf)
                return ContainmentType.Disjoint;

            // a.Y ^ b.X = (0,1,0) ^ bX
            // axis = Vector3(edge3.Z, 0, -edge3.X);
            dv0 = triV0.X * edge3.Z - triV0.Z * edge3.X;
            dv1 = triV1.X * edge3.Z - triV1.Z * edge3.X;
            dv2 = triV2.X * edge3.Z - triV2.Z * edge3.X;
            dhalf = Math.Abs(halfExtent.X * edge3.Z) + Math.Abs(halfExtent.Z * edge3.X);
            if (Math.Min(dv0, Math.Min(dv1, dv2)) >= dhalf || Math.Max(dv0, Math.Max(dv1, dv2)) <= -dhalf)
                return ContainmentType.Disjoint;

            // a.Y ^ b.X = (0,0,1) ^ edge1
            // axis = Vector3(-edge1.Y, edge1.X, 0);
            dv0 = triV0.Y * edge1.X - triV0.X * edge1.Y;
            dv1 = triV1.Y * edge1.X - triV1.X * edge1.Y;
            dv2 = triV2.Y * edge1.X - triV2.X * edge1.Y;
            dhalf = Math.Abs(halfExtent.Y * edge1.X) + Math.Abs(halfExtent.X * edge1.Y);
            if (Math.Min(dv0, Math.Min(dv1, dv2)) >= dhalf || Math.Max(dv0, Math.Max(dv1, dv2)) <= -dhalf)
                return ContainmentType.Disjoint;

            // a.Y ^ b.X = (0,0,1) ^ edge2
            // axis = Vector3(-edge2.Y, edge2.X, 0);
            dv0 = triV0.Y * edge2.X - triV0.X * edge2.Y;
            dv1 = triV1.Y * edge2.X - triV1.X * edge2.Y;
            dv2 = triV2.Y * edge2.X - triV2.X * edge2.Y;
            dhalf = Math.Abs(halfExtent.Y * edge2.X) + Math.Abs(halfExtent.X * edge2.Y);
            if (Math.Min(dv0, Math.Min(dv1, dv2)) >= dhalf || Math.Max(dv0, Math.Max(dv1, dv2)) <= -dhalf)
                return ContainmentType.Disjoint;

            // a.Y ^ b.X = (0,0,1) ^ edge3
            // axis = Vector3(-edge3.Y, edge3.X, 0);
            dv0 = triV0.Y * edge3.X - triV0.X * edge3.Y;
            dv1 = triV1.Y * edge3.X - triV1.X * edge3.Y;
            dv2 = triV2.Y * edge3.X - triV2.X * edge3.Y;
            dhalf = Math.Abs(halfExtent.Y * edge3.X) + Math.Abs(halfExtent.X * edge3.Y);
            if (Math.Min(dv0, Math.Min(dv1, dv2)) >= dhalf || Math.Max(dv0, Math.Max(dv1, dv2)) <= -dhalf)
                return ContainmentType.Disjoint;

            return ContainmentType.Intersects;
        }
    }
}
