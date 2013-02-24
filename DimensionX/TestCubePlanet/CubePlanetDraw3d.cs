﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using UniLibXNA;
using Random;

namespace TestCubePlanet
{
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, which allows it to
    /// render using a GraphicsDevice. This control shows how to draw animating
    /// 3D graphics inside a WinForms application. It hooks the Application.Idle
    /// event, using this to invalidate the control, which will cause the animation
    /// to constantly redraw.
    /// </summary>
    public class CubePlanetDraw3d : GraphicsDeviceControl
    {
        BasicEffect effect;

        class Square
        {
            public VertexPositionColor[] userPrimitives;
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

            public Square(Chunk pChunk, bool bColored, int iFaceSize)
            {
                userPrimitives = new VertexPositionColor[pChunk.m_aLocations.Length +
                    pChunk.m_aVertexes.Length];

                Dictionary<Vertex, int> vertexIndex = new Dictionary<Vertex, int>();
                Dictionary<Location, int> locationIndex = new Dictionary<Location, int>();

                int index = 0;

                m_iTrianglesCountLR = 0;

                //var chunk = pCube.m_cFaces[Cube.Face3D.Forward].m_cChunk[2,2];
                Microsoft.Xna.Framework.Color color = Microsoft.Xna.Framework.Color.White;

                for (int i=0; i<pChunk.m_aVertexes.Length; i++)
                {
                    var vertex = pChunk.m_aVertexes[i];
                    userPrimitives[index] = new VertexPositionColor();
                    userPrimitives[index].Position = new Vector3(vertex.m_fX, vertex.m_fY, vertex.m_fZ);
                    userPrimitives[index].Color = Microsoft.Xna.Framework.Color.Lerp(color, Microsoft.Xna.Framework.Color.Black, 0.2f);
                    if (bColored)
                        userPrimitives[index].Color = Microsoft.Xna.Framework.Color.Lerp(vertex.m_eColor, Microsoft.Xna.Framework.Color.Black, 0.2f);

                    vertexIndex[vertex] = index;

                    index++;
                }

                BuildBoundingBox(pChunk);

                for (int i=0; i<pChunk.m_aLocations.Length; i++)
                {
                    var loc = pChunk.m_aLocations[i];
                    //if (loc.Ghost)
                    //    continue;

                    userPrimitives[index] = new VertexPositionColor();
                    userPrimitives[index].Position = new Vector3(loc.m_fX, loc.m_fY, loc.m_fZ);
                    userPrimitives[index].Color = color;
                    if (bColored)
                        userPrimitives[index].Color = loc.m_eColor;

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

                        userPrimitives[vertexIndex[edge.Value.m_pInnerPoint]].Color = userPrimitives[locationIndex[loc]].Color;
                    }
                }
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
        }

        class Face
        {
            public Square[] m_aSquares;

            public Face(CubeFace pFace, bool bColored, GraphicsDevice graphicsDevice)
            {
                m_aSquares = new Square[pFace.Resolution * pFace.Resolution];

                int index = 0;
                foreach (var chunk in pFace.m_cChunk)
                {
                    m_aSquares[index++] = new Square(chunk, bColored, pFace.Resolution);
                }
            }
        }

        private Face[] m_aFaces = new Face[6];

        private Cube m_pCube = null;

        public void Assign(Cube pCube, bool bColored)
        {
            if (m_pCube == null && m_pCamera != null)
                m_pCamera.Initialize(pCube.R);

            m_pCube = null;

            int index = 0;
            foreach (var pFace in pCube.m_cFaces)
            {
                m_aFaces[index++] = new Face(pFace.Value, bColored, GraphicsDevice);
            }

            m_pCube = pCube;
        }

        private Microsoft.Xna.Framework.Color eSkyColor = Microsoft.Xna.Framework.Color.Lavender;

        //DumbCamera m_pCamera = null;
        public ArcBallCamera m_pCamera = null;

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            // Create our effect.
            effect = new BasicEffect(GraphicsDevice);
            effect.VertexColorEnabled = true;

            // create the effect and vertex declaration for drawing the
            // picked triangle.
            lineEffect = new BasicEffect(GraphicsDevice);
            lineEffect.VertexColorEnabled = true;

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };

            m_pCamera = new ArcBallCamera(GraphicsDevice);
            if (m_pCube != null)
                m_pCamera.Initialize(m_pCube.R);

            timer = Stopwatch.StartNew();
            lastTime = timer.Elapsed.TotalMilliseconds;
        }

        public float m_fScaling = 0;
        Stopwatch timer;
        double lastTime = 0;
        private int m_iFrame = 0;
        private double m_fDrawingTime = 0;
        private double m_fFrameTime = 0;
        private uint m_iTrianglesCount = 0;
        public int FPS { get { return m_iFrame; } }
        public double DrawingTime { get { return m_fDrawingTime; } }
        public double FrameTime { get { return m_fFrameTime; } }
        public uint TrianglesCount { get { return m_iTrianglesCount; } }


        public void ResetFPS()
        {
            m_iFrame = 0;
        }

        public Vector3? m_pTarget = null;

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            GraphicsDevice.Clear(eSkyColor);

            if (timer == null)
                return;

            m_iFrame++; 
            
            m_fFrameTime = timer.Elapsed.TotalMilliseconds - lastTime;
            lastTime = timer.Elapsed.TotalMilliseconds; 
            
            if (m_pCube == null)
                return;

            if (m_fScaling != 0)
            {
                m_pCamera.ZoomIn(m_fScaling * (float)m_fFrameTime);
                m_fScaling = 0;
            }

            if (m_pTarget.HasValue)
                m_pCamera.MoveTarget(m_pTarget.Value, 0.005f * (float)m_fFrameTime);

            m_pCamera.Update();

            Vector3 pPole = Vector3.Normalize(Vector3.Backward + Vector3.Up + Vector3.Right);

            m_pPoints = new VertexPositionColor[4];
            m_pPoints[0] = new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.Black);
            m_pPoints[1] = new VertexPositionColor(m_pCamera.FocusPoint, Microsoft.Xna.Framework.Color.Black);
            m_pPoints[2] = new VertexPositionColor(-pPole * 200, Microsoft.Xna.Framework.Color.DarkRed);
            m_pPoints[3] = new VertexPositionColor(pPole * 200, Microsoft.Xna.Framework.Color.Violet);

            effect.World = Matrix.Identity;
            effect.View = m_pCamera.View;
            effect.Projection = m_pCamera.Projection;

            // Set renderstates.
            RasterizerState rs = new RasterizerState();
            //rs.CullMode = CullMode.None;
            rs.CullMode = CullMode.CullClockwiseFace;
            //rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;

            effect.CurrentTechnique.Passes[0].Apply();
            //var pFace = m_aFaces[2];

            BoundingFrustum pFrustrum = new BoundingFrustum(Matrix.Multiply(m_pCamera.View, m_pCamera.Projection));

            double startTime = timer.Elapsed.TotalMilliseconds;
            
            int iCount = 0;
            int iGotit = 0;
            m_iTrianglesCount = 0;
            foreach (var pFace in m_aFaces)
                foreach (var pSquare in pFace.m_aSquares)
                {
                    if (Vector3.Dot(Vector3.Normalize(pSquare.m_pBounds8.Center), m_pCamera.Direction) > 0.1)
                        continue;

                    Vector3 pViewVector = pSquare.m_pBounds8.Center - m_pCamera.Position;

                    float fCos = Vector3.Dot(Vector3.Normalize(pViewVector), m_pCamera.Direction);
                    if (fCos < 0.6) //cos(45) = 0,70710678118654752440084436210485...
                    {
                        if (pFrustrum.Contains(pSquare.m_pBounds8.BottomBackLeft) == ContainmentType.Disjoint &&
                            pFrustrum.Contains(pSquare.m_pBounds8.BottomBackRight) == ContainmentType.Disjoint &&
                            pFrustrum.Contains(pSquare.m_pBounds8.BottomFrontLeft) == ContainmentType.Disjoint &&
                            pFrustrum.Contains(pSquare.m_pBounds8.BottomFrontRight) == ContainmentType.Disjoint &&
                            pFrustrum.Contains(pSquare.m_pBounds8.TopBackLeft) == ContainmentType.Disjoint &&
                            pFrustrum.Contains(pSquare.m_pBounds8.TopBackRight) == ContainmentType.Disjoint &&
                            pFrustrum.Contains(pSquare.m_pBounds8.TopFrontLeft) == ContainmentType.Disjoint &&
                            pFrustrum.Contains(pSquare.m_pBounds8.TopFrontRight) == ContainmentType.Disjoint)
                        {
                            Vector3[] aVer = {pSquare.m_pBounds8.BottomBackLeft, 
                                                 pSquare.m_pBounds8.BottomBackRight, 
                                                 pSquare.m_pBounds8.BottomFrontLeft, 
                                                 pSquare.m_pBounds8.BottomFrontRight, 
                                                 pSquare.m_pBounds8.TopBackLeft, 
                                                 pSquare.m_pBounds8.TopBackRight, 
                                                 pSquare.m_pBounds8.TopFrontLeft, 
                                                 pSquare.m_pBounds8.TopFrontRight};
                            BoundingSphere pSphere = BoundingSphere.CreateFromPoints(aVer);
                            if (pFrustrum.Contains(pSphere) == ContainmentType.Disjoint)
                            {
                                iGotit++;
                                continue;
                            }
                        }
                    }

                    if (pViewVector.Length() < 50)
                    {
                        GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList,
                                                    pSquare.userPrimitives, 0, pSquare.userPrimitives.Length - 1, pSquare.userPrimitivesIndicesHR, 0, pSquare.m_iTrianglesCountHR);
                        m_iTrianglesCount += (uint)pSquare.m_iTrianglesCountHR;
                    }
                    else
                    {
                        GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList,
                                                    pSquare.userPrimitives, 0, pSquare.userPrimitives.Length - 1, pSquare.userPrimitivesIndicesLR, 0, pSquare.m_iTrianglesCountLR);
                        m_iTrianglesCount += (uint)pSquare.m_iTrianglesCountLR;
                    }
                    iCount++;
                }

            m_fDrawingTime = timer.Elapsed.TotalMilliseconds - startTime;

            //rs = new RasterizerState();
            //rs.CullMode = CullMode.None;
            ////rs.CullMode = CullMode.CullClockwiseFace;
            //rs.FillMode = FillMode.WireFrame;
            //GraphicsDevice.RasterizerState = rs;

            //lineEffect.World = Matrix.Identity;
            //lineEffect.View = m_pCamera.View;
            //lineEffect.Projection = m_pCamera.Projection;
            //lineEffect.CurrentTechnique.Passes[0].Apply();

            //if (m_pSelectedSquare != null)
            //{
            //    var bbvertices = m_pSelectedSquare.m_pBounds8.GetVertices();
            //    var bbindices = m_pSelectedSquare.m_pBounds8.GetIndices();
            //    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList,
            //                                bbvertices, 0, bbvertices.Length, bbindices, 0, bbindices.Length / 2);
            //}
        
            // Draw the outline of the triangle under the cursor.
            DrawPickedTriangle();
        }

        // CalculateCursorRay Calculates a world space ray starting at the camera's
        // "eye" and pointing in the direction of the cursor. Viewport.Unproject is used
        // to accomplish this. see the accompanying documentation for more explanation
        // of the math behind this function.
        public Ray CalculateCursorRay(int x, int y, Matrix projectionMatrix, Matrix viewMatrix)
        {
            // create 2 positions in screenspace using the cursor position. 0 is as
            // close as possible to the camera, 1 is as far away as possible.
            Vector3 nearSource = new Vector3(x, y, 0f);
            Vector3 farSource = new Vector3(x, y, 1f);

            // use Viewport.Unproject to tell what those two screen space positions
            // would be in world space. we'll need the projection matrix and view
            // matrix, which we have saved as member variables. We also need a world
            // matrix, which can just be identity.
            Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(nearSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 farPoint = GraphicsDevice.Viewport.Unproject(farSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // and then create a new ray using nearPoint as the source.
            return new Ray(nearPoint, direction);
        }

        // Vertex array that stores exactly which triangle was picked.
        VertexPositionColor[] m_aPickedTriangle =
        {
            new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.Magenta),
            new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.Magenta),
            new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.Magenta),
        };

        // Effect and vertex declaration for drawing the picked triangle.
        BasicEffect lineEffect;
        
        /// <summary>
        /// Helper for drawing the outline of the triangle currently under the cursor.
        /// </summary>
        void DrawPickedTriangle()
        {
            if (m_bPicked)
            {
                // Set line drawing renderstates. We disable backface culling
                // and turn off the depth buffer because we want to be able to
                // see the picked triangle outline regardless of which way it is
                // facing, and even if there is other geometry in front of it.
                GraphicsDevice.RasterizerState = RasterizerState.CullNone; 
                GraphicsDevice.DepthStencilState = DepthStencilState.None;

                // Activate the line drawing BasicEffect.
                lineEffect.Projection = m_pCamera.Projection;
                lineEffect.View = m_pCamera.View;

                lineEffect.CurrentTechnique.Passes[0].Apply();

                // Draw the triangle.
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                                          m_aPickedTriangle, 0, 1);

                // Reset renderstates to their default values.
                GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList,
                    m_pPoints,
                    0,  // index of the first vertex to draw
                    m_pPoints.Length / 2   // number of primitives
                );
            }
        }

        bool m_bPicked = false;

        VertexPositionColor[] m_pPoints;

        Square m_pSelectedSquare = null;
        public Vector3? m_pCurrentPicking = null;

        Ray m_pCursorRay;

        /// <summary>
        /// Runs a per-triangle picking algorithm over all the models in the scene,
        /// storing which triangle is currently under the cursor.
        /// </summary>
        public void UpdatePicking(int x, int y)
        {
            if (m_pCube == null)
                return;

            m_bPicked = false;

            //m_pCurrentPicking = GetFocusedPoint(m_iMouseX, m_iMouseY);

            //if (!m_pCurrentPicking.HasValue)
            //    return;

            // Look up a collision ray based on the current cursor position. See the
            // Picking Sample documentation for a detailed explanation of this.
            m_pCursorRay = CalculateCursorRay(x, y, m_pCamera.Projection, m_pCamera.View);

            // calculate the ray-plane intersection point
            //Vector3 n = new Vector3(0f, 1f, 0f);
            //Plane p = new Plane(n, 0f);

            // calculate distance of intersection point from r.origin
            //float denominator = Vector3.Dot(p.Normal, m_pCursorRay.Direction);
            //float numerator = Vector3.Dot(p.Normal, m_pCursorRay.Position) + p.D;
            //float t = -(numerator / denominator);

            //m_pPoints = new VertexPositionColor[4];
            //m_pPoints[0] = new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.DarkGoldenrod);
            //m_pPoints[1] = new VertexPositionColor(m_pCursorRay.Position + m_pCursorRay.Direction * t, Microsoft.Xna.Framework.Color.DarkGoldenrod);
            //m_pPoints[2] = new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.DarkGoldenrod);
            //m_pPoints[3] = new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.DarkGoldenrod);

            //m_iCursorX = (int)m_pPoints[1].Position.X;
            //m_iCursorY = (int)m_pPoints[1].Position.Z;

            // Keep track of the closest object we have seen so far, so we can
            // choose the closest one if there are several models under the cursor.
            float closestIntersection = float.MaxValue;

            foreach (var pFace in m_aFaces)
                foreach (var pSquare in pFace.m_aSquares)
                {
                    if (pSquare.m_pBounds8.Intersects(m_pCursorRay).HasValue)
                    {
                        Vector3 vertex1, vertex2, vertex3;

                        // Perform the ray to model intersection test.
                        float? intersection = pSquare.RayIntersectsLandscape(m_pCursorRay, Matrix.Identity,//CreateScale(0.5f),
                                                                    out vertex1, out vertex2,
                                                                    out vertex3);
                        // Do we have a per-triangle intersection with this model?
                        if (intersection != null)
                        {
                            // If so, is it closer than any other model we might have
                            // previously intersected?
                            if (intersection < closestIntersection)
                            {
                                // Store information about this model.
                                closestIntersection = intersection.Value;

                                // Store vertex positions so we can display the picked triangle.
                                m_aPickedTriangle[0].Position = vertex1;
                                m_aPickedTriangle[1].Position = vertex2;
                                m_aPickedTriangle[2].Position = vertex3;

                                m_bPicked = true;

                                m_pSelectedSquare = pSquare;

                                m_pCurrentPicking = m_pCursorRay.Position + Vector3.Normalize(m_pCursorRay.Direction) * intersection;
                            }
                        }
                    }
                }

            if(!m_bPicked)
                m_pCurrentPicking = null;
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
