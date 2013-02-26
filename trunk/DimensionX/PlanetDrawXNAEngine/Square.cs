using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UniLibXNA;

namespace TestCubePlanet
{
    class Square
    {
        public VertexMultitextured[] userPrimitives;
        public int[] userPrimitivesIndicesLR;
        public int[] userPrimitivesIndicesHR;
        public int m_iTrianglesCountLR = 0;
        public int m_iTrianglesCountHR = 0;

        public Region8 m_pBounds8;

        private void BuildBoundingBox(Chunk pChunk)
        {
            Vector3 pBoundTopLeft = new Vector3(pChunk.m_pBoundTopLeft.m_fX, pChunk.m_pBoundTopLeft.m_fY, pChunk.m_pBoundTopLeft.m_fZ);
            Vector3 pBoundTopRight = new Vector3(pChunk.m_pBoundTopRight.m_fX, pChunk.m_pBoundTopRight.m_fY, pChunk.m_pBoundTopRight.m_fZ);
            Vector3 pBoundBottomLeft = new Vector3(pChunk.m_pBoundBottomLeft.m_fX, pChunk.m_pBoundBottomLeft.m_fY, pChunk.m_pBoundBottomLeft.m_fZ);
            Vector3 pBoundBottomRight = new Vector3(pChunk.m_pBoundBottomRight.m_fX, pChunk.m_pBoundBottomRight.m_fY, pChunk.m_pBoundBottomRight.m_fZ);

            Vector3 pCentral = (pBoundTopLeft + pBoundTopRight + pBoundBottomLeft + pBoundBottomRight) / 4;

            Plane pInnerPlane = new Plane(-Vector3.Normalize(pCentral), (float)pCentral.Length());
            Plane pOuterPlane = new Plane(-Vector3.Normalize(pCentral), (float)pBoundTopLeft.Length());

            Ray pBoundTopLeftRay = new Ray(Vector3.Zero, Vector3.Normalize(pBoundTopLeft));
            Ray pBoundTopRightRay = new Ray(Vector3.Zero, Vector3.Normalize(pBoundTopRight));
            Ray pBoundBottomLeftRay = new Ray(Vector3.Zero, Vector3.Normalize(pBoundBottomLeft));
            Ray pBoundBottomRightRay = new Ray(Vector3.Zero, Vector3.Normalize(pBoundBottomRight));

            float? fDist = pBoundTopLeftRay.Intersects(pInnerPlane);
            Vector3 pBoundTopLeft1 = Vector3.Normalize(pBoundTopLeft) * (float)fDist;
            fDist = pBoundTopRightRay.Intersects(pInnerPlane);
            Vector3 pBoundTopRight1 = Vector3.Normalize(pBoundTopRight) * (float)fDist;
            fDist = pBoundBottomLeftRay.Intersects(pInnerPlane);
            Vector3 pBoundBottomLeft1 = Vector3.Normalize(pBoundBottomLeft) * (float)fDist;
            fDist = pBoundBottomRightRay.Intersects(pInnerPlane);
            Vector3 pBoundBottomRight1 = Vector3.Normalize(pBoundBottomRight) * (float)fDist;

            fDist = pBoundTopLeftRay.Intersects(pOuterPlane);
            Vector3 pBoundTopLeft2 = Vector3.Normalize(pBoundTopLeft) * (float)fDist;
            fDist = pBoundTopRightRay.Intersects(pOuterPlane);
            Vector3 pBoundTopRight2 = Vector3.Normalize(pBoundTopRight) * (float)fDist;
            fDist = pBoundBottomLeftRay.Intersects(pOuterPlane);
            Vector3 pBoundBottomLeft2 = Vector3.Normalize(pBoundBottomLeft) * (float)fDist;
            fDist = pBoundBottomRightRay.Intersects(pOuterPlane);
            Vector3 pBoundBottomRight2 = Vector3.Normalize(pBoundBottomRight) * (float)fDist;

            m_pBounds8 = new Region8(pBoundTopLeft1, pBoundTopRight1, pBoundTopLeft2, pBoundTopRight2,
                pBoundBottomLeft1, pBoundBottomRight1, pBoundBottomLeft2, pBoundBottomRight2);
        }

        public VertexBuffer myVertexBuffer;
        public IndexBuffer myIndexBufferLR;
        public IndexBuffer myIndexBufferHR;

        private void CopyToBuffers(GraphicsDevice pDevice)
        {
            myVertexBuffer = new VertexBuffer(pDevice, VertexMultitextured.VertexDeclaration, userPrimitives.Length, BufferUsage.WriteOnly);
            myVertexBuffer.SetData(userPrimitives);

            myIndexBufferLR = new IndexBuffer(pDevice, typeof(int), userPrimitivesIndicesLR.Length, BufferUsage.WriteOnly);
            myIndexBufferLR.SetData(userPrimitivesIndicesLR);

            myIndexBufferHR = new IndexBuffer(pDevice, typeof(int), userPrimitivesIndicesHR.Length, BufferUsage.WriteOnly);
            myIndexBufferHR.SetData(userPrimitivesIndicesHR);
        }

        private Chunk m_pChunk;

        public Square(GraphicsDevice pDevice, Chunk pChunk, int iFaceSize)
        {
            m_pChunk = pChunk;

            BuildBoundingBox(pChunk);

            userPrimitives = new VertexMultitextured[pChunk.m_aLocations.Length +
                pChunk.m_aVertexes.Length];

            Dictionary<Vertex, int> vertexIndex = new Dictionary<Vertex, int>();
            Dictionary<Location, int> locationIndex = new Dictionary<Location, int>();

            int index = 0;

            m_iTrianglesCountLR = 0;

            //var chunk = pCube.m_cFaces[Cube.Face3D.Forward].m_cChunk[2,2];
            Microsoft.Xna.Framework.Color color = Microsoft.Xna.Framework.Color.White;

            for (int i = 0; i < pChunk.m_aVertexes.Length; i++)
            {
                var vertex = pChunk.m_aVertexes[i];
                userPrimitives[index] = new VertexMultitextured();
                userPrimitives[index].Position = new Vector3(vertex.m_fX, vertex.m_fY, vertex.m_fZ);
                userPrimitives[index].Normal = Vector3.Normalize(userPrimitives[index].Position);
                userPrimitives[index].Tangent = Vector3.Zero;
                userPrimitives[index].TextureCoordinate = new Vector4(0, 0, 0, 0); // new Vector4(GetTexture(vertex), 0, 0); 
                userPrimitives[index].TexWeights = new Vector4(0, 0, 0, 1);
                userPrimitives[index].TexWeights2 = new Vector4(0, 0, 0, 0);

                vertexIndex[vertex] = index;

                index++;
            }

            for (int i = 0; i < pChunk.m_aLocations.Length; i++)
            {
                var loc = pChunk.m_aLocations[i];
                //if (loc.Ghost)
                //    continue;

                userPrimitives[index] = new VertexMultitextured();
                userPrimitives[index].Position = new Vector3(loc.m_fX, loc.m_fY, loc.m_fZ);
                userPrimitives[index].Normal = Vector3.Normalize(userPrimitives[index].Position);
                userPrimitives[index].Tangent = Vector3.Zero;
                userPrimitives[index].TextureCoordinate = new Vector4(0, 0, 0, 0); //new Vector4(GetTexture(loc), 0, 0); 
                userPrimitives[index].TexWeights = new Vector4(0, 0, 0, 1);
                userPrimitives[index].TexWeights2 = new Vector4(0, 0, 0, 0);

                m_iTrianglesCountLR += loc.m_cEdges.Count;
                m_iTrianglesCountHR += loc.m_cEdges.Count * 4;

                locationIndex[loc] = index;

                index++;
            }


            userPrimitivesIndicesLR = new int[m_iTrianglesCountLR * 3];
            userPrimitivesIndicesHR = new int[m_iTrianglesCountHR * 3];

            index = 0;
            int indexHR = 0;

            //pChunk.DebugVertexes();

            //var chunk = pCube.m_cFaces[Cube.Face3D.Forward].m_cChunk[2, 2];
            for (int i = 0; i < pChunk.m_aLocations.Length; i++)
            {
                var loc = pChunk.m_aLocations[i];
                //if (chunk != pFace.m_cChunk[CubeFace.Size / 2, CubeFace.Size / 2])
                if (loc.Ghost)
                    continue;

                foreach (var edge in loc.m_cEdges)
                {
                    userPrimitivesIndicesLR[index++] = locationIndex[loc];
                    userPrimitivesIndicesLR[index++] = vertexIndex[edge.Value.m_pFrom];
                    userPrimitivesIndicesLR[index++] = vertexIndex[edge.Value.m_pTo];

                    userPrimitivesIndicesHR[indexHR++] = vertexIndex[edge.Value.m_pInnerPoint];
                    userPrimitivesIndicesHR[indexHR++] = vertexIndex[edge.Value.m_pFrom];
                    userPrimitivesIndicesHR[indexHR++] = vertexIndex[edge.Value.m_pMidPoint];

                    userPrimitivesIndicesHR[indexHR++] = vertexIndex[edge.Value.m_pInnerPoint];
                    userPrimitivesIndicesHR[indexHR++] = vertexIndex[edge.Value.m_pMidPoint];
                    userPrimitivesIndicesHR[indexHR++] = vertexIndex[edge.Value.m_pTo];

                    userPrimitivesIndicesHR[indexHR++] = vertexIndex[edge.Value.m_pInnerPoint];
                    userPrimitivesIndicesHR[indexHR++] = vertexIndex[edge.Value.m_pTo];
                    userPrimitivesIndicesHR[indexHR++] = vertexIndex[edge.Value.m_pNext.m_pInnerPoint];

                    userPrimitivesIndicesHR[indexHR++] = vertexIndex[edge.Value.m_pInnerPoint];
                    userPrimitivesIndicesHR[indexHR++] = vertexIndex[edge.Value.m_pNext.m_pInnerPoint];
                    userPrimitivesIndicesHR[indexHR++] = locationIndex[loc];

                    userPrimitives[vertexIndex[edge.Value.m_pInnerPoint]].TexWeights = userPrimitives[locationIndex[loc]].TexWeights;
                }
            }

            CopyToBuffers(pDevice);
        }

        private Color GetTemperature(Vertex pVertex)
        {
            float fNorthX = 150 / (float)Math.Sqrt(3);
            float fNorthY = 150 / (float)Math.Sqrt(3);
            float fNorthZ = 150 / (float)Math.Sqrt(3);

            float fDistNorth = (float)Math.Sqrt((pVertex.m_fX - fNorthX) * (pVertex.m_fX - fNorthX) + (pVertex.m_fY - fNorthY) * (pVertex.m_fY - fNorthY) + (pVertex.m_fZ - fNorthZ) * (pVertex.m_fZ - fNorthZ));
            float fDistSouth = (float)Math.Sqrt((pVertex.m_fX + fNorthX) * (pVertex.m_fX + fNorthX) + (pVertex.m_fY + fNorthY) * (pVertex.m_fY + fNorthY) + (pVertex.m_fZ + fNorthZ) * (pVertex.m_fZ + fNorthZ));

            if (fDistNorth < fDistSouth)
                return Color.Lerp(Color.Cyan, Color.Yellow, fDistNorth / 213);
            else
                return Color.Lerp(Color.Cyan, Color.Yellow, fDistSouth / 213);
        }
        
        /// <summary>
        /// Checks whether a ray intersects a model. This method needs to access
        /// the model vertex data, so the model must have been built using the
        /// custom TrianglePickingProcessor provided as part of this sample.
        /// Returns the distance along the ray to the point of intersection, or null
        /// if there is no intersection.
        /// </summary>
        public float? RayIntersectsLandscape(Ray ray, Matrix modelTransform,
                                         out Vector3 vertex1, out Vector3 vertex2,
                                         out Vector3 vertex3)
        {
            vertex1 = vertex2 = vertex3 = Vector3.Zero;

            // The input ray is in world space, but our model data is stored in object
            // space. We would normally have to transform all the model data by the
            // modelTransform matrix, moving it into world space before we test it
            // against the ray. That transform can be slow if there are a lot of
            // triangles in the model, however, so instead we do the opposite.
            // Transforming our ray by the inverse modelTransform moves it into object
            // space, where we can test it directly against our model data. Since there
            // is only one ray but typically many triangles, doing things this way
            // around can be much faster.

            Matrix inverseTransform = Matrix.Invert(modelTransform);

            ray.Position = Vector3.Transform(ray.Position, inverseTransform);
            ray.Direction = Vector3.TransformNormal(ray.Direction, inverseTransform);

            // Keep track of the closest triangle we found so far,
            // so we can always return the closest one.
            float? closestIntersection = null;

            for (int i = 0; i < userPrimitivesIndicesLR.Length; i += 3)
            {
                // Perform a ray to triangle intersection test.
                float? intersection;

                RayIntersectsTriangle(ref ray,
                                        ref userPrimitives[userPrimitivesIndicesLR[i]].Position,
                                        ref userPrimitives[userPrimitivesIndicesLR[i + 1]].Position,
                                        ref userPrimitives[userPrimitivesIndicesLR[i + 2]].Position,
                                        out intersection);

                // Does the ray intersect this triangle?
                if (intersection != null)
                {
                    // If so, is it closer than any other previous triangle?
                    if ((closestIntersection == null) ||
                        (intersection < closestIntersection))
                    {
                        // Store the distance to this triangle.
                        closestIntersection = intersection;

                        // Transform the three vertex positions into world space,
                        // and store them into the output vertex parameters.
                        Vector3.Transform(ref userPrimitives[userPrimitivesIndicesLR[i]].Position,
                                            ref modelTransform, out vertex1);

                        Vector3.Transform(ref userPrimitives[userPrimitivesIndicesLR[i + 1]].Position,
                                            ref modelTransform, out vertex2);

                        Vector3.Transform(ref userPrimitives[userPrimitivesIndicesLR[i + 2]].Position,
                                            ref modelTransform, out vertex3);
                    }
                }
            }

            return closestIntersection;
        }
        /// <summary>
        /// Checks whether a ray intersects a triangle. This uses the algorithm
        /// developed by Tomas Moller and Ben Trumbore, which was published in the
        /// Journal of Graphics Tools, volume 2, "Fast, Minimum Storage Ray-Triangle
        /// Intersection".
        /// 
        /// This method is implemented using the pass-by-reference versions of the
        /// XNA math functions. Using these overloads is generally not recommended,
        /// because they make the code less readable than the normal pass-by-value
        /// versions. This method can be called very frequently in a tight inner loop,
        /// however, so in this particular case the performance benefits from passing
        /// everything by reference outweigh the loss of readability.
        /// </summary>
        static void RayIntersectsTriangle(ref Ray ray,
                                          ref Vector3 vertex1,
                                          ref Vector3 vertex2,
                                          ref Vector3 vertex3, out float? result)
        {
            // Compute vectors along two edges of the triangle.
            Vector3 edge1, edge2;

            Vector3.Subtract(ref vertex2, ref vertex1, out edge1);
            Vector3.Subtract(ref vertex3, ref vertex1, out edge2);

            // Compute the determinant.
            Vector3 directionCrossEdge2;
            //векторное произведение - перпендикуляр к перемножаемым векторам
            Vector3.Cross(ref ray.Direction, ref edge2, out directionCrossEdge2);

            float determinant;
            //скалярное произведение - произведение длин векторов на косинус угла между ними. если угол 90, то 0
            Vector3.Dot(ref edge1, ref directionCrossEdge2, out determinant);

            // If the ray is parallel to the triangle plane, there is no collision.
            if (determinant > -float.Epsilon && determinant < float.Epsilon)
            {
                result = null;
                return;
            }

            float inverseDeterminant = 1.0f / determinant;

            // Calculate the U parameter of the intersection point.
            Vector3 distanceVector;
            Vector3.Subtract(ref ray.Position, ref vertex1, out distanceVector);

            float triangleU;
            //скалярное произведение - произведение длин векторов на косинус угла между ними. если угол 90, то 0
            Vector3.Dot(ref distanceVector, ref directionCrossEdge2, out triangleU);
            triangleU *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (triangleU < 0 || triangleU > 1)
            {
                result = null;
                return;
            }

            // Calculate the V parameter of the intersection point.
            Vector3 distanceCrossEdge1;
            Vector3.Cross(ref distanceVector, ref edge1, out distanceCrossEdge1);

            float triangleV;
            Vector3.Dot(ref ray.Direction, ref distanceCrossEdge1, out triangleV);
            triangleV *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (triangleV < 0 || triangleU + triangleV > 1)
            {
                result = null;
                return;
            }

            // Compute the distance along the ray to the triangle.
            float rayDistance;
            Vector3.Dot(ref edge2, ref distanceCrossEdge1, out rayDistance);
            rayDistance *= inverseDeterminant;

            // Is the triangle behind the ray origin?
            if (rayDistance < 0)
            {
                result = null;
                return;
            }

            result = rayDistance;
        }
    }
}
