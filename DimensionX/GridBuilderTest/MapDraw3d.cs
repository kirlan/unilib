using System;
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
using GridBuilderTest;

namespace XNAEngine
{
    public struct VertexPositionColorNormal: IVertexType
    {
        public Vector3 Position;
        public Microsoft.Xna.Framework.Color Color;
        public Vector3 Normal;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
    }
    
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, which allows it to
    /// render using a GraphicsDevice. This control shows how to draw animating
    /// 3D graphics inside a WinForms application. It hooks the Application.Idle
    /// event, using this to invalidate the control, which will cause the animation
    /// to constantly redraw.
    /// </summary>
    public class MapDraw3d : GraphicsDeviceControl
    {
        /// <summary>
        /// мир, карту которого мы рисуем
        /// </summary>
        internal LocationsGrid<Location> m_pGrid = null;

        BasicEffect effect;
        Stopwatch timer;

        VertexPositionColorNormal[] userPrimitives;
        int[] userPrimitivesIndices;
        int m_iTrianglesCount = 0;

        private readonly float m_fHeightMultiplier = 1;

        private void FillPrimitives()
        {
            int iPrimitivesCount = 0;
            foreach (Vertex pVertex in m_pGrid.m_aVertexes)
            {
                    iPrimitivesCount++;
            }

            // Create the verticies for our triangle
            userPrimitives = new VertexPositionColorNormal[iPrimitivesCount + m_pGrid.m_aLocations.Length];

            Dictionary<long, int> cVertexes = new Dictionary<long, int>();
            Dictionary<long, int> cLocations = new Dictionary<long, int>();

            int iCounter = 0;
            foreach (Vertex pVertex in m_pGrid.m_aVertexes)
            {
                //у нас x и y - это горизонтальная плоскость, причём y растёт в направлении вниз экрана, т.е. как бы к зрителю. а z - это высота.
                //в DX всё не как у людей. У них горизонтальная плоскость - это xz, причём z растёт к зрителю, а y - высота
                Vector3 pPosition = new Vector3(pVertex.m_fX, pVertex.m_fZ * m_fHeightMultiplier, pVertex.m_fY);

                userPrimitives[iCounter] = new VertexPositionColorNormal();
                userPrimitives[iCounter].Position = pPosition;
                userPrimitives[iCounter].Color = Microsoft.Xna.Framework.Color.Blue;

                cVertexes[pVertex.m_iID] = iCounter;

                iCounter++;
            }

            m_iTrianglesCount = 0;
            foreach (Location pLoc in m_pGrid.m_aLocations)
            {
                userPrimitives[iCounter] = new VertexPositionColorNormal();
                float fHeight = pLoc.m_fHeight > 0 ? pLoc.m_fHeight * m_fHeightMultiplier : pLoc.m_fHeight;

                userPrimitives[iCounter].Position = new Vector3(pLoc.X, fHeight, pLoc.Y);
                userPrimitives[iCounter].Color = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.Pink, Microsoft.Xna.Framework.Color.Lime, (float)(m_pGrid.RX - pLoc.m_iGridX) / (m_pGrid.RX * 2));

                cLocations[pLoc.m_iID] = iCounter;

                m_iTrianglesCount += pLoc.m_aBorderWith.Length;

                iCounter++;
            }

            // Create the indices used for each triangle
            userPrimitivesIndices = new int[m_iTrianglesCount * 3];

            iCounter = 0;

            foreach (Location pLoc in m_pGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.m_pFirstLine == null)
                    continue;

                Line pLine = pLoc.m_pFirstLine;
                //последовательно перебирает все связанные линии, пока круг не замкнётся.
                do
                {

                    userPrimitivesIndices[iCounter++] = cLocations[pLoc.m_iID];
                    userPrimitivesIndices[iCounter++] = cVertexes[pLine.m_pPoint2.m_iID];
                    userPrimitivesIndices[iCounter++] = cVertexes[pLine.m_pPoint1.m_iID];

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);
            }
        }

        private void CalculateNormals()
        {
            for (int i = 0; i < userPrimitives.Length; i++)
                userPrimitives[i].Normal = new Vector3(0, 0, 0);

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
        /// Привязать карту к миру.
        /// Строим контуры всего, что придётся рисовать в ОРИГИНАЛЬНЫХ координатах
        /// и раскидываем их по квадрантам
        /// </summary>
        /// <param name="pWorld">мир</param>
        public void Assign(LocationsGrid<Location> pGrid)
        {
            m_pGrid = pGrid;

            FillPrimitives();
            CalculateNormals();

            if (GraphicsDevice != null)
            {
                m_pCamera = new FreeCamera(new Vector3(0, 10, 0),
                                        MathHelper.ToRadians(0),
                                        MathHelper.ToRadians(45),
                                        MathHelper.ToRadians(0),
                                        GraphicsDevice);
            }
        }

        private Microsoft.Xna.Framework.Color eSkyColor = Microsoft.Xna.Framework.Color.DarkGray;

        public FreeCamera m_pCamera = null;

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            // Create our effect.

            effect = new BasicEffect(GraphicsDevice);

            effect.VertexColorEnabled = true;

            m_pCamera = new FreeCamera(new Vector3(0, 10, 0),
                                    MathHelper.ToRadians(0),
                                    MathHelper.ToRadians(45),
                                    MathHelper.ToRadians(0),
                                    GraphicsDevice);

            // Start the animation timer.
            timer = Stopwatch.StartNew();
            lastTime = timer.Elapsed.TotalMilliseconds;

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        private Microsoft.Xna.Framework.Color ConvertColor(System.Drawing.Color Value)
        {
            return new Microsoft.Xna.Framework.Color(Value.R, Value.G, Value.B, Value.A);
        }

        private System.Drawing.Color ConvertColor(Microsoft.Xna.Framework.Color Value) 
        {
            return System.Drawing.Color.FromArgb(Value.A, Value.R, Value.G, Value.B);
        }
        
        double lastTime = 0;

        public float m_fScaling = 0;

        public void PanCamera(float fLeft, float fUp)
        {
            m_pCamera.Pan(fLeft * m_pCamera.Position.Y * 0.00118f, fUp * m_pCamera.Position.Y * 0.00118f);
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            GraphicsDevice.Clear(eSkyColor);
            
            double fElapsedTime = timer.Elapsed.TotalMilliseconds - lastTime;
            lastTime = timer.Elapsed.TotalMilliseconds;

            if (m_pGrid == null)
                return;

            float time = (float)timer.Elapsed.TotalSeconds;

            // Set transform matrices.
            float aspect = GraphicsDevice.Viewport.AspectRatio;

            if (m_fScaling != 0)
            {
                m_pCamera.MoveForward(m_fScaling * (float)fElapsedTime);
                m_fScaling = 0;
            }
            m_pCamera.Update();

            // Update the mouse state
            effect.View = m_pCamera.View;
            effect.Projection = m_pCamera.Projection;

//            effect.Projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 150000);

            effect.LightingEnabled = true;
//            effect.DirectionalLight0.Direction = Vector3.Transform(Vector3.Normalize(new Vector3(0, 1, 0)), Matrix.CreateRotationX(time * 0.2f));
            //effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1, -1, -1));
            effect.AmbientLightColor = Microsoft.Xna.Framework.Color.Gray.ToVector3();

            effect.PreferPerPixelLighting = true;

            //effect.FogColor = eSkyColor.ToVector3();
            //effect.FogEnabled = true;
            //effect.FogStart = m_pCamera.Position.Y;//5000.0f;
            //effect.FogEnd = Math.Max(m_pCamera.m_fDistance, m_pGrid.RY/2 + m_pCamera.Position.Y * m_pCamera.Position.Y / 5000);// 180000.0f;
            
            // Set renderstates.
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullClockwiseFace;
            //rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;

            effect.World = Matrix.CreateTranslation(20000,0,-20000) ;

            // Draw the triangle.
            effect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorNormal>(PrimitiveType.TriangleList,
                                              userPrimitives, 0, userPrimitives.Length - 1, userPrimitivesIndices, 0, m_iTrianglesCount);
        }
    }
}
