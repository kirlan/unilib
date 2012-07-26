using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UniLibXNA;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace SphereCameraTest
{
    public partial class _3dview : GraphicsDeviceControl
    {
        BasicEffect effect;
        BasicEffect lineEffect;

        Stopwatch timer;
        double lastTime = 0;
        
        VertexPositionColorNormal[] userPrimitives;
        int[] userPrimitivesIndices;
        int m_iTrianglesCount = 0;

        private Microsoft.Xna.Framework.Color eSkyColor = Microsoft.Xna.Framework.Color.DarkGray;

        public FreeCamera m_pCamera = null;
        Matrix SteadyView;

        private void FillPrimitives()
        {
            // Create the verticies for our triangle
            userPrimitives = new VertexPositionColorNormal[14];

            m_iTrianglesCount = 24;

            userPrimitivesIndices = new int[m_iTrianglesCount * 3];

            int iCounter = 0;

            userPrimitives[0] = new VertexPositionColorNormal();
            userPrimitives[0].Position = new Vector3(0, 1, 0);
            userPrimitives[1] = new VertexPositionColorNormal();
            userPrimitives[1].Position = new Vector3(1, 1, 1);
            userPrimitives[2] = new VertexPositionColorNormal();
            userPrimitives[2].Position = new Vector3(1, 1, -1);
            userPrimitives[3] = new VertexPositionColorNormal();
            userPrimitives[3].Position = new Vector3(-1, 1, -1);
            userPrimitives[4] = new VertexPositionColorNormal();
            userPrimitives[4].Position = new Vector3(-1, 1, 1);

            int iCenter = 0;
            userPrimitivesIndices[iCounter++] = iCenter;
            userPrimitivesIndices[iCounter++] = iCenter + 1;
            userPrimitivesIndices[iCounter++] = iCenter + 2;
            userPrimitivesIndices[iCounter++] = iCenter;
            userPrimitivesIndices[iCounter++] = iCenter + 2;
            userPrimitivesIndices[iCounter++] = iCenter + 3;
            userPrimitivesIndices[iCounter++] = iCenter;
            userPrimitivesIndices[iCounter++] = iCenter + 3;
            userPrimitivesIndices[iCounter++] = iCenter + 4;
            userPrimitivesIndices[iCounter++] = iCenter;
            userPrimitivesIndices[iCounter++] = iCenter + 4;
            userPrimitivesIndices[iCounter++] = iCenter + 1;

            userPrimitives[5] = new VertexPositionColorNormal();
            userPrimitives[5].Position = new Vector3(0, -1, 0);
            userPrimitives[6] = new VertexPositionColorNormal();
            userPrimitives[6].Position = new Vector3(1, -1, 1);
            userPrimitives[7] = new VertexPositionColorNormal();
            userPrimitives[7].Position = new Vector3(-1, -1, 1);
            userPrimitives[8] = new VertexPositionColorNormal();
            userPrimitives[8].Position = new Vector3(-1, -1, -1);
            userPrimitives[9] = new VertexPositionColorNormal();
            userPrimitives[9].Position = new Vector3(1, -1, -1);

            iCenter = 5;
            userPrimitivesIndices[iCounter++] = iCenter;
            userPrimitivesIndices[iCounter++] = iCenter + 1;
            userPrimitivesIndices[iCounter++] = iCenter + 2;
            userPrimitivesIndices[iCounter++] = iCenter;
            userPrimitivesIndices[iCounter++] = iCenter + 2;
            userPrimitivesIndices[iCounter++] = iCenter + 3;
            userPrimitivesIndices[iCounter++] = iCenter;
            userPrimitivesIndices[iCounter++] = iCenter + 3;
            userPrimitivesIndices[iCounter++] = iCenter + 4;
            userPrimitivesIndices[iCounter++] = iCenter;
            userPrimitivesIndices[iCounter++] = iCenter + 4;
            userPrimitivesIndices[iCounter++] = iCenter + 1;

            userPrimitives[10] = new VertexPositionColorNormal();
            userPrimitives[10].Position = new Vector3(1, 0, 0);

            userPrimitivesIndices[iCounter++] = 10;
            userPrimitivesIndices[iCounter++] = 1;
            userPrimitivesIndices[iCounter++] = 6;
            userPrimitivesIndices[iCounter++] = 10;
            userPrimitivesIndices[iCounter++] = 6;
            userPrimitivesIndices[iCounter++] = 9;
            userPrimitivesIndices[iCounter++] = 10;
            userPrimitivesIndices[iCounter++] = 9;
            userPrimitivesIndices[iCounter++] = 2;
            userPrimitivesIndices[iCounter++] = 10;
            userPrimitivesIndices[iCounter++] = 2;
            userPrimitivesIndices[iCounter++] = 1;

            userPrimitives[11] = new VertexPositionColorNormal();
            userPrimitives[11].Position = new Vector3(0, 0, 1);

            userPrimitivesIndices[iCounter++] = 11;
            userPrimitivesIndices[iCounter++] = 1;
            userPrimitivesIndices[iCounter++] = 4;
            userPrimitivesIndices[iCounter++] = 11;
            userPrimitivesIndices[iCounter++] = 4;
            userPrimitivesIndices[iCounter++] = 7;
            userPrimitivesIndices[iCounter++] = 11;
            userPrimitivesIndices[iCounter++] = 7;
            userPrimitivesIndices[iCounter++] = 6;
            userPrimitivesIndices[iCounter++] = 11;
            userPrimitivesIndices[iCounter++] = 6;
            userPrimitivesIndices[iCounter++] = 1;

            userPrimitives[12] = new VertexPositionColorNormal();
            userPrimitives[12].Position = new Vector3(-1, 0, 0);

            userPrimitivesIndices[iCounter++] = 12;
            userPrimitivesIndices[iCounter++] = 3;
            userPrimitivesIndices[iCounter++] = 8;
            userPrimitivesIndices[iCounter++] = 12;
            userPrimitivesIndices[iCounter++] = 8;
            userPrimitivesIndices[iCounter++] = 7;
            userPrimitivesIndices[iCounter++] = 12;
            userPrimitivesIndices[iCounter++] = 7;
            userPrimitivesIndices[iCounter++] = 4;
            userPrimitivesIndices[iCounter++] = 12;
            userPrimitivesIndices[iCounter++] = 4;
            userPrimitivesIndices[iCounter++] = 3;

            userPrimitives[13] = new VertexPositionColorNormal();
            userPrimitives[13].Position = new Vector3(0, 0, -1);

            userPrimitivesIndices[iCounter++] = 13;
            userPrimitivesIndices[iCounter++] = 2;
            userPrimitivesIndices[iCounter++] = 9;
            userPrimitivesIndices[iCounter++] = 13;
            userPrimitivesIndices[iCounter++] = 9;
            userPrimitivesIndices[iCounter++] = 8;
            userPrimitivesIndices[iCounter++] = 13;
            userPrimitivesIndices[iCounter++] = 8;
            userPrimitivesIndices[iCounter++] = 3;
            userPrimitivesIndices[iCounter++] = 13;
            userPrimitivesIndices[iCounter++] = 3;
            userPrimitivesIndices[iCounter++] = 2;

            for (int i = 0; i < userPrimitives.Length; i++)
            {
                userPrimitives[i].Position.Normalize();
                userPrimitives[i].Position *= 100;

                userPrimitives[i].Color = new Color((userPrimitives[i].Position.X + 100) / 200, (userPrimitives[i].Position.Y + 100) / 200, (userPrimitives[i].Position.Z + 100) / 200);

                userPrimitives[i].Normal = new Vector3(0, 0, 0);
            }

            for (int i = 0; i < m_iTrianglesCount; i++)
            {
                int index1 = userPrimitivesIndices[i * 3];
                int index2 = userPrimitivesIndices[i * 3 + 1];
                int index3 = userPrimitivesIndices[i * 3 + 2];

                if (index1 == 0 && index2 == 0 && index3 == 0)
                    continue;

                Vector3 side1 = userPrimitives[index1].Position - userPrimitives[index3].Position;
                Vector3 side2 = userPrimitives[index1].Position - userPrimitives[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                userPrimitives[index1].Normal += normal;
                userPrimitives[index2].Normal += normal;
                userPrimitives[index3].Normal += normal;
            }

            for (int i = 0; i < userPrimitives.Length; i++)
                userPrimitives[i].Normal.Normalize();
        }

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

            m_pCamera = new FreeCamera(100,
                                    GraphicsDevice);

            SteadyView = Matrix.CreateLookAt(new Vector3(0, 0, 150), Vector3.Zero, Vector3.Down);
            
            FillPrimitives();

            // Start the animation timer.
            timer = Stopwatch.StartNew();
            lastTime = timer.Elapsed.TotalMilliseconds;

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        public float m_fScaling = 0;
        public bool m_bPanMode = false;
        
        public float m_fTarget = 0;
        public float m_fSelected = 0;
        public float m_fCurrent = 0;

        bool m_bPicked = false;

        Vector3? m_pCurrentPicking = null;
        Vector3? m_pLastPicking = null;

        public void ResetPanning()
        {
            m_pLastPicking = m_pCurrentPicking;
        }
        
        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            GraphicsDevice.Clear(eSkyColor);

            double fElapsedTime = timer.Elapsed.TotalMilliseconds - lastTime;
            lastTime = timer.Elapsed.TotalMilliseconds;

            float time = (float)timer.Elapsed.TotalSeconds;

            // Set transform matrices.
            float aspect = GraphicsDevice.Viewport.AspectRatio;

            if (m_fScaling != 0)
            {
                m_pCamera.ZoomIn(m_fScaling * (float)fElapsedTime);
                m_fScaling = 0;
            }
            m_pCamera.Update();
            if (m_bPanMode && m_pCurrentPicking != null)
            {
                if (m_pLastPicking != null)
                    m_pCamera.Target += (Vector3)m_pLastPicking - (Vector3)m_pCurrentPicking;

                // m_pLastPicking = m_pCurrentPicking;
                m_pCurrentPicking = null;
            }

            // Update the mouse state
            effect.View = m_pCamera.View;
            effect.Projection = m_pCamera.Projection;

            //            effect.Projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 150000);

            effect.LightingEnabled = true;
            //effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1, -1, -1));
            effect.AmbientLightColor = Microsoft.Xna.Framework.Color.Gray.ToVector3();

            effect.PreferPerPixelLighting = true;

            // Set renderstates.
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;//CullCounterClockwiseFace;
            //rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;

            effect.World = Matrix.Identity;

            // Draw the triangle.
            effect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorNormal>(PrimitiveType.TriangleList,
                                              userPrimitives, 0, userPrimitives.Length, userPrimitivesIndices, 0, m_iTrianglesCount);

            // Draw the outline of the triangle under the cursor.
            //DrawPickedTriangle();
        }

        // Vertex array that stores exactly which triangle was picked.
        VertexPositionColor[] pickedTriangle =
        {
            new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.Magenta),
            new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.Magenta),
            new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.Magenta),
        };

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
                RasterizerState rs = new RasterizerState();
                rs.CullMode = CullMode.CullCounterClockwiseFace;
                //rs.FillMode = FillMode.WireFrame;
                GraphicsDevice.RasterizerState = rs;
                GraphicsDevice.DepthStencilState = DepthStencilState.None;

                // Activate the line drawing BasicEffect.
                lineEffect.Projection = m_pCamera.Projection;
                lineEffect.View = m_pCamera.View;

                lineEffect.CurrentTechnique.Passes[0].Apply();

                // Draw the triangle.
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                                          pickedTriangle, 0, 1);


                VertexPositionColor[] camera = 
                {
                    new VertexPositionColor(m_pCamera.Target, Microsoft.Xna.Framework.Color.White),
                    new VertexPositionColor(m_pCamera.Position + m_pCamera.Top*5, Microsoft.Xna.Framework.Color.Blue),
                    new VertexPositionColor(m_pCamera.Position, Microsoft.Xna.Framework.Color.Red),
                };

                GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, camera, 0, 1);

                // Reset renderstates to their default values.
                GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
        }

        /// <summary>
        /// Runs a per-triangle picking algorithm over all the models in the scene,
        /// storing which triangle is currently under the cursor.
        /// </summary>
        public void UpdatePicking(int x, int y)
        {
            // Look up a collision ray based on the current cursor position. See the
            // Picking Sample documentation for a detailed explanation of this.
            Ray cursorRay = CalculateCursorRay(x, y, m_pCamera.Projection, m_pCamera.View);

            // Keep track of the closest object we have seen so far, so we can
            // choose the closest one if there are several models under the cursor.
            float closestIntersection = float.MaxValue;

            Vector3 vertex1, vertex2, vertex3;

            // Perform the ray to model intersection test.
            float? intersection = RayIntersectsLandscape(CullMode.CullCounterClockwiseFace, cursorRay, userPrimitives, userPrimitivesIndices,
                                                        out vertex1, out vertex2,
                                                        out vertex3);
            m_bPicked = false;

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
                    pickedTriangle[0].Position = vertex1;
                    pickedTriangle[1].Position = vertex2;
                    pickedTriangle[2].Position = vertex3;

                    m_bPicked = true;

                    m_pCurrentPicking = cursorRay.Position + Vector3.Normalize(cursorRay.Direction) * intersection;
                }
            }
            else
                m_pCurrentPicking = null;
        }
    }
}
