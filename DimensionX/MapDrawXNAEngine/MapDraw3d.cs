using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using Socium;
using LandscapeGeneration;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MapDrawXNAEngine
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
        internal World m_pWorld = null;

        BasicEffect effect;
        Stopwatch timer;

        VertexPositionColorNormal[] userPrimitives;
        int[] userPrimitivesIndices;
        int m_iTrianglesCount = 0;

        private readonly float m_fHeightMultiplier = 100;

        private void FillPrimitives1()
        {
            // Create the verticies for our triangle
            userPrimitives = new VertexPositionColorNormal[m_pWorld.m_pGrid.m_aVertexes.Length + m_pWorld.m_pGrid.m_aLocations.Length];

            Dictionary<long, int> cVertexes = new Dictionary<long, int>();
            Dictionary<long, int> cLocations = new Dictionary<long, int>();

            int iCounter = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                userPrimitives[iCounter] = new VertexPositionColorNormal();
                userPrimitives[iCounter].Position = new Vector3(pVertex.m_fX, pVertex.m_fY, pVertex.m_fZ > 0 ? pVertex.m_fZ * m_fHeightMultiplier : pVertex.m_fZ);

                LandTypeInfoX pLTI = null;
                Microsoft.Xna.Framework.Color pMixedColor = Microsoft.Xna.Framework.Color.White;
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    if (pLTI == null)
                    {
                        pLTI = (pLoc.Owner as LandX).Type;
                        pMixedColor = ConvertColor(pLTI.m_pColor);
                    }
                    else
                        pMixedColor = Microsoft.Xna.Framework.Color.Lerp(pMixedColor, ConvertColor((pLoc.Owner as LandX).Type.m_pColor), 0.5f);
                    //if (pLTI != (pLoc.Owner as LandX).Type)
                    //{
                    //    pLTI = null;
                    //    break;
                    //}
                }
                //if(pLTI != null)
                //    userPrimitives[iCounter].Color = ConvertColor(pLTI.m_pColor);
                userPrimitives[iCounter].Color = pMixedColor;

                cVertexes[pVertex.m_iID] = iCounter;

                iCounter++;
            }

            m_iTrianglesCount = 0;
            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                userPrimitives[iCounter] = new VertexPositionColorNormal();
                userPrimitives[iCounter].Position = new Vector3(pLoc.X, pLoc.Y, pLoc.m_fHeight > 0 ? pLoc.m_fHeight * m_fHeightMultiplier : pLoc.m_fHeight);
                if (pLoc.Forbidden || pLoc.Owner == null)
                    userPrimitives[iCounter].Color = Microsoft.Xna.Framework.Color.White;
                else
                    userPrimitives[iCounter].Color = ConvertColor((pLoc.Owner as LandX).Type.m_pColor);

                cLocations[pLoc.m_iID] = iCounter;

                m_iTrianglesCount += pLoc.m_aBorderWith.Length;

                iCounter++;
            }

            // Create the indices used for each triangle
            userPrimitivesIndices = new int[m_iTrianglesCount * 3];

            iCounter = 0;

            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
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

        private void FillPrimitives2()
        {
            int iPrimitivesCount = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                List<LandTypeInfoX> cTypes = new List<LandTypeInfoX>();
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    if (!cTypes.Contains((pLoc.Owner as LandX).Type))
                    {
                        cTypes.Add((pLoc.Owner as LandX).Type);
                        iPrimitivesCount++;
                    }
                }

            }

            // Create the verticies for our triangle
            userPrimitives = new VertexPositionColorNormal[iPrimitivesCount + m_pWorld.m_pGrid.m_aLocations.Length];

            Dictionary<long, Dictionary<LandTypeInfoX, int>> cVertexes = new Dictionary<long, Dictionary<LandTypeInfoX, int>>();
            Dictionary<long, int> cLocations = new Dictionary<long, int>();

            int iCounter = 0;
            foreach (Vertex pVertex in m_pWorld.m_pGrid.m_aVertexes)
            {
                cVertexes[pVertex.m_iID] = new Dictionary<LandTypeInfoX, int>();

                //у нас x и y - это горизонтальная плоскость, причём y растёт в направлении вниз экрана, т.е. как бы к зрителю. а z - это высота.
                //в DX всё не как у людей. У них горизонтальная плоскость - это xz, причём z растёт к зрителю, а y - высота
                Vector3 pPosition = new Vector3(pVertex.m_fX, pVertex.m_fZ > 0 ? pVertex.m_fZ * m_fHeightMultiplier : pVertex.m_fZ, pVertex.m_fY);

                List<LandTypeInfoX> cTypes = new List<LandTypeInfoX>();
                foreach (LocationX pLoc in pVertex.m_cLocations)
                {
                    if (pLoc.Forbidden || pLoc.Owner == null)
                        continue;

                    if (!cTypes.Contains((pLoc.Owner as LandX).Type))
                    {
                        cTypes.Add((pLoc.Owner as LandX).Type);
                        userPrimitives[iCounter] = new VertexPositionColorNormal();
                        userPrimitives[iCounter].Position = pPosition;
                        userPrimitives[iCounter].Color = ConvertColor((pLoc.Owner as LandX).Type.m_pColor);

                        cVertexes[pVertex.m_iID][(pLoc.Owner as LandX).Type] = iCounter;

                        iCounter++;
                    }
                }
            }

            m_iTrianglesCount = 0;
            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                userPrimitives[iCounter] = new VertexPositionColorNormal();
                float fHeight = pLoc.m_fHeight > 0 ? pLoc.m_fHeight * m_fHeightMultiplier : pLoc.m_fHeight;
                if (pLoc.m_eType == RegionType.Peak)
                    fHeight += 2 * m_fHeightMultiplier;
                if (pLoc.m_eType == RegionType.Volcano)
                    fHeight -= 10 * m_fHeightMultiplier;

                userPrimitives[iCounter].Position = new Vector3(pLoc.X, fHeight, pLoc.Y);
                if (pLoc.Forbidden || pLoc.Owner == null)
                    userPrimitives[iCounter].Color = Microsoft.Xna.Framework.Color.White;
                else
                    userPrimitives[iCounter].Color = ConvertColor((pLoc.Owner as LandX).Type.m_pColor);

                if (pLoc.m_eType == RegionType.Peak)
                    userPrimitives[iCounter].Color = Microsoft.Xna.Framework.Color.White;
                if (pLoc.m_eType == RegionType.Volcano)
                    userPrimitives[iCounter].Color = Microsoft.Xna.Framework.Color.Red;

                cLocations[pLoc.m_iID] = iCounter;

                m_iTrianglesCount += pLoc.m_aBorderWith.Length;

                iCounter++;
            }

            // Create the indices used for each triangle
            userPrimitivesIndices = new int[m_iTrianglesCount * 3];

            iCounter = 0;

            foreach (LocationX pLoc in m_pWorld.m_pGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.m_pFirstLine == null)
                    continue;

                bool bError = false;

                Line pLine = pLoc.m_pFirstLine;
                //последовательно перебирает все связанные линии, пока круг не замкнётся.
                do
                {

                    userPrimitivesIndices[iCounter++] = cLocations[pLoc.m_iID];
                    userPrimitivesIndices[iCounter++] = cVertexes[pLine.m_pPoint2.m_iID][(pLoc.Owner as LandX).Type];
                    userPrimitivesIndices[iCounter++] = cVertexes[pLine.m_pPoint1.m_iID][(pLoc.Owner as LandX).Type];

                    int index2 = userPrimitivesIndices[iCounter - 2];
                    int index3 = userPrimitivesIndices[iCounter - 1];

                    if ((userPrimitives[index2].Position.X == userPrimitives[index3].Position.X) && (userPrimitives[index2].Position.Z == userPrimitives[index3].Position.Z))
                        bError = true;

                    Vector3 pProjection2 = Vector3.Transform(userPrimitives[index2].Position, Matrix.CreateScale(1, 0, 1));
                    Vector3 pProjection3 = Vector3.Transform(userPrimitives[index3].Position, Matrix.CreateScale(1, 0, 1));

                    float fDistanceProjection = Vector3.Distance(pProjection2, pProjection3);
                    float fRealDistance = Vector3.Distance(userPrimitives[index2].Position, userPrimitives[index3].Position);
                    if (fDistanceProjection * 5 < fRealDistance)
                        bError = true;

                    pLine = pLine.m_pNext;
                }
                while (pLine != pLoc.m_pFirstLine);
            }
        }

        private void CalculateNormals()
        {
            bool bError = false;

            for (int i = 0; i < userPrimitives.Length; i++)
                userPrimitives[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < m_iTrianglesCount; i++)
            {
                int index1 = userPrimitivesIndices[i * 3];
                int index2 = userPrimitivesIndices[i * 3 + 1];
                int index3 = userPrimitivesIndices[i * 3 + 2];

                if (index1 == 0 && index2 == 0 && index3 == 0)
                    continue;

                if ((userPrimitives[index1].Position.X == userPrimitives[index2].Position.X) && (userPrimitives[index1].Position.Z == userPrimitives[index2].Position.Z))
                    bError = true;
                if ((userPrimitives[index1].Position.X == userPrimitives[index3].Position.X) && (userPrimitives[index1].Position.Z == userPrimitives[index3].Position.Z))
                    bError = true;
                if ((userPrimitives[index2].Position.X == userPrimitives[index3].Position.X) && (userPrimitives[index2].Position.Z == userPrimitives[index3].Position.Z))
                    bError = true;

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
        public void Assign(World pWorld)
        {
            m_pWorld = pWorld;

            FillPrimitives2();
            CalculateNormals();

            if (GraphicsDevice != null)
            {
                m_pCamera = new FreeCamera(new Vector3(0, m_pWorld.m_fMaxHeight * m_fHeightMultiplier, 0),
                                        MathHelper.ToRadians(0),
                                        MathHelper.ToRadians(0),
                                        MathHelper.ToRadians(0),
                                        GraphicsDevice);
            }
        }

        private Microsoft.Xna.Framework.Color eSkyColor = Microsoft.Xna.Framework.Color.Lavender;

        public FreeCamera m_pCamera = null;

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            // Create our effect.

            effect = new BasicEffect(GraphicsDevice);

            effect.VertexColorEnabled = true;

            if (m_pWorld == null)
            {
                m_pCamera = new FreeCamera(new Vector3(0, 100000, 0),
                                            MathHelper.ToRadians(180),
                                            MathHelper.ToRadians(0),
                                            MathHelper.ToRadians(180),
                                            GraphicsDevice);
            }
            else
            {
//                m_pCamera = new FreeCamera(new Vector3(0, m_pWorld.m_fMaxHeight * m_fHeightMultiplier, 0),
                m_pCamera = new FreeCamera(new Vector3(0, 100000, 0),
                                        MathHelper.ToRadians(0),
                                        MathHelper.ToRadians(270),
                                        MathHelper.ToRadians(0),
                                        GraphicsDevice);
            }

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

            if (m_pWorld == null)
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
            if (m_pCamera.m_pPOI.X < -m_pWorld.m_pGrid.RX)
                m_pCamera.m_pPOI += new Vector3(m_pWorld.m_pGrid.RX * 2, 0, 0);
            if (m_pCamera.m_pPOI.X > m_pWorld.m_pGrid.RX)
                m_pCamera.m_pPOI -= new Vector3(m_pWorld.m_pGrid.RX * 2, 0, 0);
            m_pCamera.Update();

            //if (m_pCamera.Position.Y < m_pWorld.m_fMaxHeight * m_fHeightMultiplier)
            //{
            //    Vector3 pReturn = new Vector3(0, m_pWorld.m_fMaxHeight * m_fHeightMultiplier - m_pCamera.Position.Y, 0);
            //    m_pCamera.Jump(pReturn);
            //    m_pCamera.Update();
            //}
            // Update the mouse state
            effect.View = m_pCamera.View;
            effect.Projection = m_pCamera.Projection;

//            effect.Projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 150000);

            effect.LightingEnabled = true;
//            effect.DirectionalLight0.Direction = Vector3.Transform(Vector3.Normalize(new Vector3(0, 1, 0)), Matrix.CreateRotationX(time * 0.2f));
            effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1, -1, -1));
            effect.AmbientLightColor = Microsoft.Xna.Framework.Color.Multiply(eSkyColor, 0.2f).ToVector3();

            effect.PreferPerPixelLighting = true;

            effect.FogColor = eSkyColor.ToVector3();
            effect.FogEnabled = true;
            effect.FogStart = m_pCamera.Position.Y;//5000.0f;
            effect.FogEnd = Math.Max(m_pCamera.m_fDistance, m_pWorld.m_pGrid.RY/2 + m_pCamera.Position.Y * m_pCamera.Position.Y / 5000);// 180000.0f;
            
            // Set renderstates.
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            if (m_pWorld.m_pGrid.m_bCycled)
            {
                effect.World = Matrix.CreateTranslation(-m_pWorld.m_pGrid.RX * 2, 0, 0) * Matrix.CreateScale(0.5f);

                // Draw the triangle.
                effect.CurrentTechnique.Passes[0].Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorNormal>(PrimitiveType.TriangleList,
                                                  userPrimitives, 0, userPrimitives.Length - 1, userPrimitivesIndices, 0, m_iTrianglesCount);

                effect.World = Matrix.CreateTranslation(m_pWorld.m_pGrid.RX * 2, 0, 0) * Matrix.CreateScale(0.5f);

                // Draw the triangle.
                effect.CurrentTechnique.Passes[0].Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorNormal>(PrimitiveType.TriangleList,
                                                  userPrimitives, 0, userPrimitives.Length - 1, userPrimitivesIndices, 0, m_iTrianglesCount);
            }
            effect.World = Matrix.CreateScale(0.5f);

            // Draw the triangle.
            effect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorNormal>(PrimitiveType.TriangleList,
                                              userPrimitives, 0, userPrimitives.Length - 1, userPrimitivesIndices, 0, m_iTrianglesCount);
        }
    }
}
