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
using Microsoft.VisualBasic.Devices;
using ContentLoader;

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
        private Microsoft.Xna.Framework.Color m_eSkyColor = Microsoft.Xna.Framework.Color.AliceBlue;
        private Microsoft.Xna.Framework.Color m_eRealSkyColor = Microsoft.Xna.Framework.Color.AliceBlue;

        Shaders m_pShader = null;

        private Face[] m_aFaces = new Face[6];

        private Cube m_pCube = null;

        private void MyInit(Cube pCube)
        {
            if (GraphicsDevice != null)
            {
                m_pCamera = new ArcBallCamera(GraphicsDevice);
                m_pCamera.Initialize(pCube.R);

                m_pShader.SetFog(m_eSkyColor, pCube.R + 15, 0.001f);

                int index = 0;
                foreach (var pFace in pCube.m_cFaces)
                {
                    m_aFaces[index++] = new Face(GraphicsDevice, pFace.Value, pCube.R, m_pShader.m_aTreeModels, m_pShader.m_aPalmModels, m_pShader.m_aPineModels, m_pShader.m_pTreeTexture);
                }
            }
        }

        public void Clear()
        {
            m_pCube = null;
            for (int iFace = 0; iFace < m_aFaces.Length; iFace++ )
            {
                if (m_aFaces[iFace] != null)
                {
                    for (int iSquare = 0; iSquare < m_aFaces[iFace].m_aSquares.Length; iSquare++)
                    {
                        if (m_aFaces[iFace].m_aSquares[iSquare] != null)
                        {
                            m_aFaces[iFace].m_aSquares[iSquare].Clear();
                            m_aFaces[iFace].m_aSquares[iSquare] = null;
                        }
                    }
                    m_aFaces[iFace] = null;
                }
            }
        }

        public void Assign(Cube pCube)
        {
            m_pCube = null;

            MyInit(pCube);
            
            Square.ClearQueues();

            m_pCube = pCube;
        }

        //DumbCamera m_pCamera = null;
        public ArcBallCamera m_pCamera = null;
        public static Vector3 m_pPole = Vector3.Normalize(Vector3.Backward + Vector3.Up + Vector3.Right);
        public static Vector3 m_pSunOriginal = Vector3.Normalize(Vector3.Cross(Vector3.Normalize(Vector3.Forward + Vector3.Up + Vector3.Right), m_pPole));
        public Vector3 m_pSunCurrent = m_pSunOriginal;
        public Microsoft.Xna.Framework.Color m_eSunColor = Microsoft.Xna.Framework.Color.Yellow;

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            m_pShader = new Shaders(GraphicsDevice, Services);
            m_pShader.SetAmbientLight(m_eSkyColor, 0.07f);

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };

            if (m_pCube != null)
            {
                MyInit(m_pCube);
            }

            timer = Stopwatch.StartNew();
            lastTime = timer.Elapsed.TotalMilliseconds;
        }

        public float m_fScaling = 0;
        Stopwatch timer;
        double lastTime = 0;
        
        private int m_iFrame = 0;
        public int FPS { get { return m_iFrame; } }
        public void ResetFPS()
        {
            m_iFrame = 0;
        }

        private double m_fDrawingTime = 0;
        public double DrawingTime { get { return m_fDrawingTime; } }

        private double m_fFrameTime = 0;
        public double FrameTime { get { return m_fFrameTime; } }

        private uint m_iTrianglesCount = 0;
        public uint TrianglesCount { get { return m_iTrianglesCount; } }

        private bool m_bUseCelShading = true;
        public bool UseCelShading
        {
            get { return m_bUseCelShading; }
            set { m_bUseCelShading = value; }
        }

        private bool m_bShowBounds = false;
        public bool ShowBounds
        {
            get { return m_bShowBounds; }
            set { m_bShowBounds = value; }
        }

        private bool m_bWireFrame = false;
        public bool WireFrame
        {
            get { return m_bWireFrame; }
            set { m_bWireFrame = value; }
        }

        private bool m_bDrawTrees = true;
        public bool NeedDrawTrees
        {
            get { return m_bDrawTrees; }
            set 
            { 
                m_bDrawTrees = value;
                if (m_bDrawTrees)
                    RecalckTrees();
            }
        }

        private float m_fLODDistance = 50;
        public float LODDistance
        {
            get { return m_fLODDistance; }
            set { m_fLODDistance = value; }
        }
        
        public Vector3? m_pTarget = null;
        private Square m_pFocusedSquare = null;

        private Matrix m_pWorldMatrix = Matrix.Identity;
        /// <summary>
        /// Нормаль к планетарной сфере в точке на горизонте, максимально близкой к направлению взгляда камеры
        /// </summary>
        private Vector3 m_pHorizonNormal;

        public int VisibleQueue { get { return Square.s_pVisibleQueue.Count; } }
        public int InvisibleQueue { get { return Square.s_pInvisibleQueue.Count; } }

        private void UpdateCamera()
        {
            //Удаление камеры от точки фокуса
            if (m_fScaling != 0)
            {
                m_pCamera.ZoomIn(m_fScaling * (float)Math.Sqrt(m_fFrameTime));
                m_fScaling = 0;
            }

            //Смещение точки фокуса
            if (m_pTarget.HasValue)
                m_pCamera.MoveTarget(m_pTarget.Value, 0.005f * (float)m_fFrameTime);

            if (m_pCamera.Update())
            {
                //Точка фокуса должна быть над поверхностью ландшафта
                Vector3? pCharacter = GetSurface(m_pCamera.FocusPoint, out m_pFocusedSquare);
                if (pCharacter.HasValue)
                {
                    pCharacter += Vector3.Normalize(pCharacter.Value);// *1.2f;
                    m_pCamera.Position += pCharacter.Value - m_pCamera.FocusPoint;
                    m_pCamera.FocusPoint = pCharacter.Value;
                }
                else
                {
                    pCharacter = GetSurface(m_pCamera.FocusPoint, out m_pFocusedSquare);
                    //throw new Exception();
                }

                //Сама камера тоже должна быть над поверхностью ландшафта
                if (m_pCamera.Position.Length() < m_pCube.R + 10)
                {
                    float fMinHeight = 0.1f;
                    Square pCameraSquare;
                    Vector3? pSurface = GetSurface(m_pCamera.Position, out pCameraSquare);
                    if (pSurface.HasValue)
                    {
                        if ((pSurface.Value - m_pCamera.Position).Length() < fMinHeight)
                        {
                            //камера слишком низко - принудительно поднимаем её на минимальную допустимую высоту
                            m_pCamera.Position = pSurface.Value + Vector3.Normalize(pSurface.Value) * fMinHeight;
                        }
                    }
                }

                //Обновим матрицу вида с учётом изменившегося положения камеры и её фокуса
                m_pCamera.View = Matrix.CreateLookAt(m_pCamera.Position, m_pCamera.FocusPoint, m_pCamera.Top);

                //Вычислим нормаль на горизонте, чтобы потом использовать её при вычислении цвета неба
                float h = m_pCamera.Position.Length();
                float d = h * (float)Math.Sqrt(h * h / (m_pCube.R * m_pCube.R) - 1);
                m_pHorizonNormal = Vector3.Normalize(m_pCamera.Position - Vector3.Normalize(Vector3.Cross(m_pCamera.Position, m_pCamera.Left)) * d);

                //Создадим такую мировую матрицу, которая сжимет мир по вертикали (относительно камеры!)
                //и затем поднимает его вверх так, чтобы сохранить мировые коодинаты фокуса камеры
                Vector3 pTop = Vector3.Normalize(m_pCamera.FocusPoint);
                Vector3 pForward = Vector3.Cross(pTop, m_pCamera.Left);
                Matrix T = new Matrix(m_pCamera.Left.X, pTop.X, pForward.X, 0,
                                      m_pCamera.Left.Y, pTop.Y, pForward.Y, 0,
                                      m_pCamera.Left.Z, pTop.Z, pForward.Z, 0,
                                      0, 0, 0, 1);
                m_pWorldMatrix = Matrix.Multiply(Matrix.Multiply(T, Matrix.CreateScale(1, 0.5f, 1)), Matrix.Invert(T));
                m_pWorldMatrix = Matrix.Multiply(m_pWorldMatrix, Matrix.CreateTranslation(pTop * m_pCube.R / 2));

                //Вычисляем видимость квадратов
                BoundingFrustum pFrustrum = new BoundingFrustum(Matrix.Multiply(m_pCamera.View, m_pCamera.Projection));
                foreach (var pFace in m_aFaces)
                    foreach (var pSquare in pFace.m_aSquares)
                        pSquare.UpdateVisible(GraphicsDevice, pFrustrum, m_pCamera.Position, m_pCamera.Direction, m_pCamera.FocusPoint, m_pWorldMatrix);

                //Обновляем массив отображаемых деревьев
                if (m_bDrawTrees)
                    RecalckTrees();
            }
        }

        private float m_fSunAngle = 0;

        private float m_fTimeSpeed = 1.0f / 16000;

        public float TimeSpeed
        {
            get { return m_fTimeSpeed; }
            set { m_fTimeSpeed = value; }
        }

        private bool m_bTimeWarp = true;

        public bool TimeWarp
        {
            get { return m_bTimeWarp; }
            set { m_bTimeWarp = value; }
        }

        private TimeSpan m_fDayTime = TimeSpan.Zero;
        public TimeSpan DayTime { get { return m_fDayTime; } }

        private void UpdateLight()
        {
            Vector3 pSunHighestPos = Vector3.Normalize(Vector3.Cross(m_pPole, Vector3.Cross(m_pSunOriginal, m_pPole)));
            pSunHighestPos = Vector3.Normalize(Vector3.Cross(m_pPole, Vector3.Cross(m_pCamera.FocusPoint, m_pPole)));

            //1 в полдень, -1 в полночь
            float fDayTime = Vector3.Dot(pSunHighestPos, -m_pSunCurrent);

            if (fDayTime > 1)
                fDayTime = 1;
            if (fDayTime < -1)
                fDayTime = -1;

            float fK = 1;

            if (m_bTimeWarp)
            {
                fK = 1 - fDayTime;
                //fK = fK * fK;

                if (fK < 0.1)
                    fK = 0.1f;

                fK *= 5;
            }

            m_fSunAngle += (float)(fK * m_fFrameTime * m_fTimeSpeed);
            while (m_fSunAngle > Math.PI * 2)
                m_fSunAngle -= (float)(Math.PI * 2);
            m_pSunCurrent = Vector3.Transform(m_pSunOriginal, Matrix.CreateFromAxisAngle(m_pPole, m_fSunAngle));

            //если fDayTime2 < fDayTime - значит сейчас вторая половина суток, когда солнце уже миновало верхнюю точку.
            float fDayTime2 = Vector3.Dot(pSunHighestPos, -m_pSunCurrent);
            if (fDayTime2 > 1)
                fDayTime2 = 1;
            if (fDayTime2 < -1)
                fDayTime2 = -1;

            //если это первая половина суток, то Пи - это полночь, а 0 - это полдень.
            //иначе - наоборот
            float fRelativeSunAngle = (float)Math.Acos(fDayTime2);
            if (fDayTime2 > fDayTime)
                m_fDayTime = TimeSpan.FromMinutes(12 * 60 * (Math.PI - fRelativeSunAngle) / Math.PI);
            else
                m_fDayTime = TimeSpan.FromMinutes(12 * 60 * (Math.PI + fRelativeSunAngle) / Math.PI);

            float fCos = Vector3.Dot(Vector3.Normalize(m_pCamera.Position), -m_pSunCurrent) + 0.3f;
            //float fSin = (float)Math.Sqrt(1 - fCos*fCos);
            if (fCos > 1)
                fCos = 1;

            fCos = 1 - (1 - fCos) * (1 - fCos);

            if (fCos < 0)
                m_eSunColor = Microsoft.Xna.Framework.Color.LightPink;
            else
                m_eSunColor = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.LightPink, Microsoft.Xna.Framework.Color.LightYellow, fCos);

            float diff = Vector3.Dot(-m_pSunCurrent, m_pHorizonNormal) + 0.3f;
            if (diff > 1)
                diff = 1;
            diff = 1 - (1 - diff) * (1 - diff);//*(1-diff)*(1-diff);

            Vector3 diffuse = new Vector3(diff);

            //if (UseCelShading)
            //{
            //    // Look up the cel shading light color
            //    float2 celTexCoord = float2(diff, 0.0f);
            //    diffuse = tex2D(CelMapSampler, celTexCoord);
            //}
            //Vector3 reflect = Vector3.Normalize(2 * diffuse * normal - DirectionalLightDirection);
            //float d = Vector3.Dot(reflect, inView);
            //if (d > 1)
            //    d = 1;
            //Vector3 specular = new Vector3((float)Math.Pow(d, 15));

            Vector3 light = m_eSkyColor.ToVector3() * 0.02f +
                   m_eSunColor.ToVector3() * diffuse;// *0.8f;// +
            //SpecularColor * specular;

            light = m_eSkyColor.ToVector3() * light;

            m_eRealSkyColor = Microsoft.Xna.Framework.Color.FromNonPremultiplied((int)(light.X * 255), (int)(light.Y * 255), (int)(light.Z * 255), 255);
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            if (timer == null)
                return;

            m_iFrame++; 
            m_fFrameTime = timer.Elapsed.TotalMilliseconds - lastTime;
            lastTime = timer.Elapsed.TotalMilliseconds; 
            
            if (m_pCube == null)
                return;

            UpdateCamera();

            UpdateLight();

            m_aDebugInfo = new VertexPositionColor[4];
            m_aDebugInfo[0] = new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.Black);
            m_aDebugInfo[1] = new VertexPositionColor(m_pCamera.FocusPoint, Microsoft.Xna.Framework.Color.Black);
            m_aDebugInfo[2] = new VertexPositionColor(-m_pPole * 200, Microsoft.Xna.Framework.Color.DarkRed);
            m_aDebugInfo[3] = new VertexPositionColor(m_pPole * 200, Microsoft.Xna.Framework.Color.Violet);

            m_pShader.BeginDraw(m_bUseCelShading, m_bWireFrame);

            m_pShader.SetMatrices(m_pWorldMatrix, m_pCamera.View, m_pCamera.Projection, m_pCamera.Position);
            m_pShader.SetDirectionalLight(m_pSunCurrent, 0.9f, m_eSunColor);

            int iCount = 0;
            m_iTrianglesCount = 0;

            //Рисуем подводный мир - в refractionRenderTarget
            m_pShader.PrepareDrawUnderwater(m_eRealSkyColor);
            foreach (var pSquare in Square.s_pVisibleQueue)
            {
                if (pSquare.m_iUnderwaterTrianglesCount > 0)
                {
                    m_pShader.DrawLandscape(pSquare.m_pVertexBuffer, pSquare.m_pUnderwaterIndexBuffer, pSquare.g.m_aLandPoints.Length, pSquare.m_iUnderwaterTrianglesCount);

                    m_iTrianglesCount += (uint)pSquare.m_iUnderwaterTrianglesCount;
                    iCount++;
                }
            }

            //Рисуем надводный мир
            m_pShader.PrepareDrawLand(m_eRealSkyColor);
            foreach (var pSquare in Square.s_pVisibleQueue)
            {
                if (pSquare.m_fVisibleDistance < m_fLODDistance)
                {
                    m_pShader.DrawLandscape(pSquare.m_pVertexBuffer, pSquare.m_pLandIndexBufferHR, pSquare.g.m_aLandPoints.Length, pSquare.m_iLandTrianglesCountHR);
                    m_iTrianglesCount += (uint)pSquare.m_iLandTrianglesCountHR;
                }
                else
                {
                    m_pShader.DrawLandscape(pSquare.m_pVertexBuffer, pSquare.m_pLandIndexBufferLR, pSquare.g.m_aLandPoints.Length, pSquare.m_iLandTrianglesCountLR);
                    m_iTrianglesCount += (uint)pSquare.m_iLandTrianglesCountLR;
                }
                iCount++;
            }

            if (m_bDrawTrees)
            {
                DrawTrees();
            }

            //DrawSun();

            //Рисуем водную поверхность и видимый через неё подводный мир
            m_pShader.PrepareDrawWater();
            foreach (var pSquare in Square.s_pVisibleQueue)
            {
                if (pSquare.m_iUnderwaterTrianglesCount > 0)
                {
                    m_pShader.DrawLandscape(pSquare.m_pWaterVertexBuffer, pSquare.m_pWaterIndexBuffer, pSquare.g.m_aWaterPoints.Length, pSquare.m_iWaterTrianglesCount);

                    m_iTrianglesCount += (uint)pSquare.m_iWaterTrianglesCount;
                    iCount++;
                }
            }

            if (m_bShowBounds)
            {
                m_pShader.PrepareDrawLines(false);

                if (m_pSelectedSquare != null)
                {
                    m_pShader.DrawLines(m_pSelectedSquare.m_pBounds8.GetVertices(), m_pSelectedSquare.m_pBounds8.GetIndices());
                }
            }

            // Draw the outline of the triangle under the cursor.
            DrawPickedTriangle();
            DrawDebugInfo();

            m_pShader.FinishDraw(m_eRealSkyColor);

            m_fDrawingTime = timer.Elapsed.TotalMilliseconds - lastTime;
        }

        //private void DrawSun()
        //{
        //    ModelMesh pSunMesh = m_pContent.m_pSunModel.Meshes[0];
        //    ModelMeshPart pSunMeshPart = pSunMesh.MeshParts[0];
        //    GraphicsDevice.SetVertexBuffer(pSunMeshPart.VertexBuffer, pSunMeshPart.VertexOffset);
        //    GraphicsDevice.Indices = pSunMeshPart.IndexBuffer;

        //    m_pContent.m_pLineEffect.World = Matrix.CreateTranslation(-m_pSunCurrent * 200) * m_pWorldMatrix;
        //    m_pContent.m_pLineEffect.Projection = m_pCamera.Projection;
        //    m_pContent.m_pLineEffect.View = m_pCamera.View;

        //    m_pContent.m_pLineEffect.CurrentTechnique.Passes[0].Apply();

        //    //sampleMesh contains all of the information required to draw
        //    //the current mesh
        //    GraphicsDevice.DrawIndexedPrimitives(
        //        PrimitiveType.TriangleList, 0, 0,
        //        pSunMeshPart.NumVertices, pSunMeshPart.StartIndex, pSunMeshPart.PrimitiveCount);

        //    //m_pSunModel.Draw(Matrix.CreateTranslation(-m_pSun * 200), m_pCamera.View, m_pCamera.Projection);
        //}

        private Dictionary<Model, List<Matrix>> m_cTreeInstances = new Dictionary<Model, List<Matrix>>();

        private void RecalckTrees()
        {
            foreach (var pTreeModel in m_cTreeInstances)
                pTreeModel.Value.Clear();            
            
            float fMaxDistanceSquared = 1600;
            //fMaxDistanceSquared *= (float)(70 / Math.Sqrt(m_pWorld.m_pGrid.m_iLocationsCount));

            foreach (var pSquare in Square.s_pVisibleQueue)
            {
                if (pSquare.m_fVisibleDistance < m_fLODDistance)
                {
                    foreach (var vTree in pSquare.g.m_aTrees)
                    {
                        Model pTreeModel = vTree.Key;

                        if (!m_cTreeInstances.ContainsKey(pTreeModel))
                            m_cTreeInstances[pTreeModel] = new List<Matrix>();
                        for (int i = 0; i < vTree.Value.Length; i++)
                        {
                            TreeModel pTree = vTree.Value[i];

                            Vector3 pViewVector = (pTree.worldMatrix * m_pWorldMatrix).Translation - m_pCamera.Position;

                            //float fCos = Vector3.Dot(Vector3.Normalize(pViewVector), m_pCamera.Direction);
                            //if (fCos < 0.6) //cos(45) = 0,70710678118654752440084436210485...
                            //    continue;

                            if (pViewVector.LengthSquared() > fMaxDistanceSquared)
                                continue;

                            m_cTreeInstances[pTreeModel].Add(pTree.worldMatrix * m_pWorldMatrix);
                        }
                    }
                }
            }
        }

        private void DrawTrees()
        {
            foreach (var pTreeModel in m_cTreeInstances)
            {
                Matrix[] instancedModelBones = new Matrix[pTreeModel.Key.Bones.Count];
                pTreeModel.Key.CopyAbsoluteBoneTransformsTo(instancedModelBones);

                m_pShader.DrawModelHardwareInstancing(pTreeModel.Key, instancedModelBones,
                                         pTreeModel.Value.ToArray(), m_pCamera.View, m_pCamera.Projection, m_pCamera.Position);
            }
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
                projectionMatrix, viewMatrix, m_pWorldMatrix);

            Vector3 farPoint = GraphicsDevice.Viewport.Unproject(farSource,
                projectionMatrix, viewMatrix, m_pWorldMatrix);

            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // and then create a new ray using nearPoint as the source.
            return new Ray(nearPoint, direction);
        }

        /// <summary>
        /// Helper for drawing the outline of the triangle currently under the cursor.
        /// </summary>
        void DrawPickedTriangle()
        {
            if (m_iFocusedLocation != -1)
            {
                // Set line drawing renderstates. We disable backface culling
                // and turn off the depth buffer because we want to be able to
                // see the picked triangle outline regardless of which way it is
                // facing, and even if there is other geometry in front of it.
                m_pShader.PrepareDrawLines(true);

                // Draw the triangle.
                //GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                //                          m_aPickedTriangle, 0, 1);
                if (m_pSelectedSquare != null)
                {
                    m_pSelectedSquare.Rebuild(GraphicsDevice);
                    m_pShader.DrawLines(m_pSelectedSquare.g.m_aLandPoints, m_pSelectedSquare.g.m_aLocations[m_iFocusedLocation]);
                }
            }
        }

        /// <summary>
        /// Helper for drawing the outline of the triangle currently under the cursor.
        /// </summary>
        void DrawDebugInfo()
        {
            // Activate the line drawing BasicEffect.
            m_pShader.PrepareDrawLines(false);
            m_pShader.DrawLines(m_aDebugInfo);
        }

        VertexPositionColor[] m_aDebugInfo;

        Square m_pSelectedSquare = null;
        public Vector3? m_pCurrentPicking = null;

        Ray m_pCursorRay;

        private Vector3? GetSurface(Vector3 pPos, out Square pOwnerSquare)
        {
            pOwnerSquare = null;

            if (m_pCube == null)
                return null;

            Ray upRay = new Ray(Vector3.Zero, Vector3.Normalize(pPos));

            foreach (var pFace in m_aFaces)
                foreach (var pSquare in pFace.m_aSquares)
                {
                    if (pSquare.m_pBounds8.Intersects(upRay).HasValue)
                    {
                        pSquare.Rebuild(GraphicsDevice);

                        int iLoc = -1;

                        // Perform the ray to model intersection test.
                        float? intersection = pSquare.RayIntersectsLandscape(upRay, Matrix.Identity,//m_pWorldMatrix,//CreateScale(0.5f),
                                                                    ref iLoc);
                        // Do we have a per-triangle intersection with this model?
                        if (intersection != null)
                        {
                            pOwnerSquare = pSquare;
                            return Vector3.Normalize(upRay.Direction) * intersection;
                        }
                    }
                }

            return null;
        }

        /// <summary>
        /// Индекс локации в выбранном квадрате, над которой находится указатель мыши.
        /// </summary>
        private int m_iFocusedLocation = -1;

        /// <summary>
        /// Runs a per-triangle picking algorithm over all the models in the scene,
        /// storing which triangle is currently under the cursor.
        /// </summary>
        public void UpdatePicking(int x, int y)
        {
            if (m_pCube == null)
                return;

            // Look up a collision ray based on the current cursor position. See the
            // Picking Sample documentation for a detailed explanation of this.
            m_pCursorRay = CalculateCursorRay(x, y, m_pCamera.Projection, m_pCamera.View);


            if (m_pSelectedSquare != null && m_pSelectedSquare.m_fVisibleDistance > 0 &&
                m_pSelectedSquare.m_pBounds8.Intersects(m_pCursorRay).HasValue)
            {
                m_pSelectedSquare.Rebuild(GraphicsDevice);
                float? intersection = m_pSelectedSquare.RayIntersectsLandscape(m_pCursorRay, Matrix.Identity,//m_pWorldMatrix,//CreateScale(0.5f),
                                                            ref m_iFocusedLocation);
                // Do we have a per-triangle intersection with this model?
                if (intersection != null)
                {
                    m_pCurrentPicking = m_pCursorRay.Position + Vector3.Normalize(m_pCursorRay.Direction) * intersection;
                    return;
                }
            }

            m_iFocusedLocation = -1;

            // Keep track of the closest object we have seen so far, so we can
            // choose the closest one if there are several models under the cursor.
            float closestIntersection = float.MaxValue;

            foreach (var pFace in m_aFaces)
                foreach (var pSquare in pFace.m_aSquares)
                {
                    if (pSquare.m_fVisibleDistance > 0 && pSquare.m_pBounds8.Intersects(m_pCursorRay).HasValue)
                    {
                        pSquare.Rebuild(GraphicsDevice);
                        int iLoc = -1;

                        // Perform the ray to model intersection test.
                        float? intersection = pSquare.RayIntersectsLandscape(m_pCursorRay, Matrix.Identity,//m_pWorldMatrix,//CreateScale(0.5f),
                                                                    ref iLoc);
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
                                m_iFocusedLocation = iLoc;

                                m_pSelectedSquare = pSquare;

                                m_pCurrentPicking = m_pCursorRay.Position + Vector3.Normalize(m_pCursorRay.Direction) * intersection;
                            }
                        }
                    }
                }

            if (m_iFocusedLocation == -1)
                m_pCurrentPicking = null;
        }

    }
}
