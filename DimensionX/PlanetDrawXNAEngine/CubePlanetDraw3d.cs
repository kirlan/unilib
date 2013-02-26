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
        BasicEffect m_pBasicEffect;
        Effect m_pMyEffect;

        private Microsoft.Xna.Framework.Color eSkyColor = Microsoft.Xna.Framework.Color.Lavender;

        public static ContentManager LibContent;

        EffectParameter pEffectWorld;
        EffectParameter pEffectView;
        EffectParameter pEffectProjection;
        EffectParameter pEffectAmbientLightColor;
        EffectParameter pEffectAmbientLightIntensity;
        EffectParameter pEffectDirectionalLightDirection;
        EffectParameter pEffectDirectionalLightColor;
        EffectParameter pEffectDirectionalLightIntensity;
        EffectParameter pEffectCameraPosition;
        EffectParameter pEffectSpecularColor;
        EffectParameter pEffectFogColor;
        EffectParameter pEffectFogDensity;
        EffectParameter pEffectFogHeight;
        EffectParameter pEffectFogModePlain;
        EffectParameter pEffectFogModeRing;
        EffectParameter pEffectFogModeSphere;
        EffectParameter pEffectBlendDistance;
        EffectParameter pEffectBlendWidth;

        EffectParameter pEffectTexture0;
        EffectParameter pEffectTexture1;
        EffectParameter pEffectTexture2;
        EffectParameter pEffectTexture3;
        EffectParameter pEffectTexture4;
        EffectParameter pEffectTexture5;
        EffectParameter pEffectTexture6;
        EffectParameter pEffectTexture7;

        EffectParameter pEffectBumpMap0;

        EffectParameter pEffectTextureModel;

        Texture2D grassTexture;
        Texture2D sandTexture;
        Texture2D rockTexture;
        Texture2D snowTexture;
        Texture2D forestTexture;
        Texture2D roadTexture;
        Texture2D swampTexture;
        Texture2D lavaTexture;

        Texture2D grassBump;
        Texture2D sandBump;
        Texture2D rockBump;
        Texture2D snowBump;
        Texture2D forestBump;
        Texture2D roadBump;
        Texture2D swampBump;
        Texture2D lavaBump;

        Texture2D treeTexture;

        Model[] treeModel = new Model[13];
        Model[] palmModel = new Model[4];
        Model[] pineModel = new Model[4];

        SpriteFont villageNameFont;
        SpriteFont townNameFont;
        SpriteFont cityNameFont;
        SpriteBatch m_pSpriteBatch;

        Effect outlineShader;   // Outline shader effect
        float defaultThickness = 1.5f;  // default outline thickness
        float defaultThreshold = 0.2f;  // default edge detection threshold
        float outlineThickness = 0.5f;  // current outline thickness
        float outlineThreshold = 0.12f;  // current edge detection threshold
        float tStep = 0.01f;    // Ammount to step the line thickness by
        float hStep = 0.001f;   // Ammount to step the threshold by

        /* Render target to capture cel-shaded render for edge detection
         * post processing
         */
        Texture2D celMap;       // Texture map for cell shading
        RenderTarget2D celTarget;       // render target for main game object

        // Vertex array that stores exactly which triangle was picked.
        // Effect and vertex declaration for drawing the picked triangle.
        BasicEffect lineEffect;

        BasicEffect textEffect;

        RenderTarget2D refractionRenderTarget;

        private Face[] m_aFaces = new Face[6];

        private Cube m_pCube = null;

        private void MyInit(Cube pCube)
        {
            if (GraphicsDevice != null)
            {
                m_pCamera = new ArcBallCamera(GraphicsDevice);
                m_pCamera.Initialize(pCube.R);

                int index = 0;
                foreach (var pFace in pCube.m_cFaces)
                {
                    m_aFaces[index++] = new Face(GraphicsDevice, pFace.Value);

                    float fFogHeight = 10;
                    fFogHeight = pCube.R + 5;

                    //foreach (Model pModel in treeModel)
                    //    foreach (ModelMesh mesh in pModel.Meshes)
                    //        foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    //        {
                    //            meshPart.Effect.Parameters["FogHeight"].SetValue(fFogHeight);
                    //        }

                    //foreach (Model pModel in palmModel)
                    //    foreach (ModelMesh mesh in pModel.Meshes)
                    //        foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    //        {
                    //            meshPart.Effect.Parameters["FogHeight"].SetValue(fFogHeight);
                    //        }

                    //foreach (Model pModel in pineModel)
                    //    foreach (ModelMesh mesh in pModel.Meshes)
                    //        foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    //        {
                    //            meshPart.Effect.Parameters["FogHeight"].SetValue(fFogHeight);
                    //        }

                    pEffectFogHeight.SetValue(fFogHeight);
                }
            }
        }

        public void Assign(Cube pCube)
        {
            m_pCube = null;

            MyInit(pCube);

            m_pCube = pCube;
        }

        //DumbCamera m_pCamera = null;
        public ArcBallCamera m_pCamera = null;

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            m_pBasicEffect = new BasicEffect(GraphicsDevice);
            m_pBasicEffect.VertexColorEnabled = true;
            m_pSpriteBatch = new SpriteBatch(GraphicsDevice);

            LibContent = new ContentManager(Services);

            // Create our effect.
            LoadTerrainTextures();

            villageNameFont = LibContent.Load<SpriteFont>("content/villagename");
            townNameFont = LibContent.Load<SpriteFont>("content/townname");
            cityNameFont = LibContent.Load<SpriteFont>("content/cityname");

            m_pMyEffect = LibContent.Load<Effect>("content/Effect1");
            BindEffectParameters();

            celMap = LibContent.Load<Texture2D>("content/celMap");
            m_pMyEffect.Parameters["CelMap"].SetValue(celMap);

            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Land"];
            pEffectWorld.SetValue(Matrix.Identity);

            pEffectAmbientLightColor.SetValue(eSkyColor.ToVector4());
            pEffectAmbientLightIntensity.SetValue(0.02f);

            pEffectDirectionalLightColor.SetValue(eSkyColor.ToVector4());
            pEffectDirectionalLightDirection.SetValue(new Vector3(0, 0, -150));
            pEffectDirectionalLightIntensity.SetValue(1);

            pEffectSpecularColor.SetValue(0);

            pEffectFogColor.SetValue(eSkyColor.ToVector4());
            pEffectFogHeight.SetValue(155);
            pEffectFogModePlain.SetValue(false);
            pEffectFogModeRing.SetValue(false);
            pEffectFogModeSphere.SetValue(true);
            pEffectFogDensity.SetValue(0.007f);

            pEffectBlendDistance.SetValue(20);//2
            pEffectBlendWidth.SetValue(40);

            pEffectTexture0.SetValue(sandTexture);
            pEffectTexture1.SetValue(grassTexture);
            pEffectTexture2.SetValue(rockTexture);
            pEffectTexture3.SetValue(snowTexture);
            pEffectTexture4.SetValue(forestTexture);
            pEffectTexture5.SetValue(roadTexture);
            pEffectTexture6.SetValue(swampTexture);
            pEffectTexture7.SetValue(lavaTexture);

            pEffectBumpMap0.SetValue(rockBump);

            m_pMyEffect.Parameters["GridColor1"].SetValue(Microsoft.Xna.Framework.Color.Black.ToVector4());
            m_pMyEffect.Parameters["GridColor2"].SetValue(Microsoft.Xna.Framework.Color.Pink.ToVector4());
            m_pMyEffect.Parameters["GridColor3"].SetValue(Microsoft.Xna.Framework.Color.White.ToVector4());
            m_pMyEffect.Parameters["GridColor4"].SetValue(Microsoft.Xna.Framework.Color.Goldenrod.ToVector4());
            m_pMyEffect.Parameters["GridColor5"].SetValue(Microsoft.Xna.Framework.Color.Yellow.ToVector4());
            m_pMyEffect.Parameters["GridColor6"].SetValue(Microsoft.Xna.Framework.Color.Black.ToVector4());

            // create the effect and vertex declaration for drawing the
            // picked triangle.
            lineEffect = new BasicEffect(GraphicsDevice);
            lineEffect.VertexColorEnabled = true;

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            refractionRenderTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat);

            textEffect = new BasicEffect(GraphicsDevice);

            /* Load and initialize the outline shader effect
             */
            outlineShader = LibContent.Load<Effect>("content/OutlineShader");
            outlineShader.Parameters["Thickness"].SetValue(outlineThickness);
            outlineShader.Parameters["Threshold"].SetValue(outlineThreshold);
            outlineShader.Parameters["ScreenSize"].SetValue(
                new Vector2(GraphicsDevice.Viewport.Bounds.Width, GraphicsDevice.Viewport.Bounds.Height));

            /* Here is the first significant difference between XNA 3.0 and XNA 4.0
             * Render targets have been significantly revamped in this version of XNA
             * this constructor creates a render target with the given width and height, 
             * no MipMap, the standard Color surface format and a depth format that provides
             * space for depth information.  The key bit here is the depth format.  If
             * we do not specify this here we will get the default DepthFormat for a render
             * target which is None.  Without a depth buffer we will not get propper culling.
             */
            celTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height,
                false, SurfaceFormat.Color, DepthFormat.Depth24);

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };

            if (m_pCube != null)
            {
                MyInit(m_pCube);
            }

            timer = Stopwatch.StartNew();
            lastTime = timer.Elapsed.TotalMilliseconds;
        }

        private void BindEffectParameters()
        {
            pEffectWorld = m_pMyEffect.Parameters["World"];
            pEffectView = m_pMyEffect.Parameters["View"];
            pEffectProjection = m_pMyEffect.Parameters["Projection"];

            pEffectCameraPosition = m_pMyEffect.Parameters["CameraPosition"];

            pEffectAmbientLightColor = m_pMyEffect.Parameters["AmbientLightColor"];
            pEffectAmbientLightIntensity = m_pMyEffect.Parameters["AmbientLightIntensity"];

            pEffectDirectionalLightDirection = m_pMyEffect.Parameters["DirectionalLightDirection"];
            pEffectDirectionalLightColor = m_pMyEffect.Parameters["DirectionalLightColor"];
            pEffectDirectionalLightIntensity = m_pMyEffect.Parameters["DirectionalLightIntensity"];

            pEffectSpecularColor = m_pMyEffect.Parameters["SpecularColor"];

            pEffectFogColor = m_pMyEffect.Parameters["FogColor"];
            pEffectFogDensity = m_pMyEffect.Parameters["FogDensity"];
            pEffectFogHeight = m_pMyEffect.Parameters["FogHeight"];
            pEffectFogModePlain = m_pMyEffect.Parameters["FogModePlain"];
            pEffectFogModeRing = m_pMyEffect.Parameters["FogModeRing"];
            pEffectFogModeSphere = m_pMyEffect.Parameters["FogModeSphere"];

            pEffectBlendDistance = m_pMyEffect.Parameters["BlendDistance"];
            pEffectBlendWidth = m_pMyEffect.Parameters["BlendWidth"];

            pEffectTexture0 = m_pMyEffect.Parameters["xTexture0"];
            pEffectTexture1 = m_pMyEffect.Parameters["xTexture1"];
            pEffectTexture2 = m_pMyEffect.Parameters["xTexture2"];
            pEffectTexture3 = m_pMyEffect.Parameters["xTexture3"];
            pEffectTexture4 = m_pMyEffect.Parameters["xTexture4"];
            pEffectTexture5 = m_pMyEffect.Parameters["xTexture5"];
            pEffectTexture6 = m_pMyEffect.Parameters["xTexture6"];
            pEffectTexture7 = m_pMyEffect.Parameters["xTexture7"];
            pEffectTextureModel = m_pMyEffect.Parameters["xTextureModel"];

            pEffectBumpMap0 = m_pMyEffect.Parameters["BumpMap0"];
        }

        private void LoadTerrainTextures()
        {
            grassTexture = LibContent.Load<Texture2D>("content/dds/1-plain");
            sandTexture = LibContent.Load<Texture2D>("content/dds/1-desert");
            //            rockTexture = LibContent.Load<Texture2D>("content/dds/mountain");
            rockTexture = LibContent.Load<Texture2D>("content/dds/rock");
            snowTexture = LibContent.Load<Texture2D>("content/dds/snow");
            forestTexture = LibContent.Load<Texture2D>("content/dds/grass");
            roadTexture = LibContent.Load<Texture2D>("content/dds/sand");
            swampTexture = LibContent.Load<Texture2D>("content/dds/river");
            lavaTexture = LibContent.Load<Texture2D>("content/dds/2-lava");

            rockBump = LibContent.Load<Texture2D>("content/dds/bump-rock");
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

        private bool m_bUseCelShading = true;

        public bool UseCelShading
        {
            get { return m_bUseCelShading; }
            set { m_bUseCelShading = value; }
        }

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
            if (timer == null)
                return;

            //double startTime = timer.Elapsed.TotalMilliseconds;

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

            pEffectView.SetValue(m_pCamera.View);
            pEffectProjection.SetValue(m_pCamera.Projection);

            pEffectCameraPosition.SetValue(m_pCamera.Position);

            pEffectWorld.SetValue(Matrix.Identity);

            // Set renderstates.
            RasterizerState rs = new RasterizerState();
            //rs.CullMode = CullMode.None;
            rs.CullMode = CullMode.CullClockwiseFace;
            //rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;

            Viewport pPort = GraphicsDevice.Viewport;

            m_pMyEffect.Parameters["UseCelShading"].SetValue(m_bUseCelShading);
            if (m_bUseCelShading)
            {
                if (GraphicsDevice.Viewport.Width != celTarget.Width || GraphicsDevice.Viewport.Height != celTarget.Height)
                    celTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height,
                        false, SurfaceFormat.Color, DepthFormat.Depth24);
            }

            pEffectDirectionalLightDirection.SetValue(-m_pCamera.Position);
            pEffectDirectionalLightIntensity.SetValue(0.8f);//0.8f

            GraphicsDevice.SetRenderTarget(celTarget);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default; 
            
            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Land"];
            m_pMyEffect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.Clear(eSkyColor);

            //var pFace = m_aFaces[2];

            BoundingFrustum pFrustrum = new BoundingFrustum(Matrix.Multiply(m_pCamera.View, m_pCamera.Projection));

            int iCount = 0;
            int iGotit = 0;
            m_iTrianglesCount = 0;
            foreach (var pFace in m_aFaces)
                foreach (var pSquare in pFace.m_aSquares)
                {
                    if (Vector3.Dot(pSquare.m_pBounds8.Normal, m_pCamera.Direction) > 0.1)
                        continue;

                    Vector3 pViewVector = pSquare.m_pBounds8.Center - m_pCamera.Position;

                    float fCos = Vector3.Dot(Vector3.Normalize(pViewVector), m_pCamera.Direction);
                    if (fCos < 0.6) //cos(45) = 0,70710678118654752440084436210485...
                    {
                        //if (//pFrustrum.Contains(pSquare.m_pBounds8.BottomBackLeft) == ContainmentType.Disjoint &&
                        //    //pFrustrum.Contains(pSquare.m_pBounds8.BottomBackRight) == ContainmentType.Disjoint &&
                        //    //pFrustrum.Contains(pSquare.m_pBounds8.BottomFrontLeft) == ContainmentType.Disjoint &&
                        //    //pFrustrum.Contains(pSquare.m_pBounds8.BottomFrontRight) == ContainmentType.Disjoint &&
                        //    pFrustrum.Contains(pSquare.m_pBounds8.TopBackLeft) == ContainmentType.Disjoint &&
                        //    pFrustrum.Contains(pSquare.m_pBounds8.TopBackRight) == ContainmentType.Disjoint &&
                        //    pFrustrum.Contains(pSquare.m_pBounds8.TopFrontLeft) == ContainmentType.Disjoint &&
                        //    pFrustrum.Contains(pSquare.m_pBounds8.TopFrontRight) == ContainmentType.Disjoint)
                        {
                            if (pFrustrum.Contains(pSquare.m_pBounds8.m_pSphere) == ContainmentType.Disjoint)
                            {
                                iGotit++;
                                continue;
                            }
                        }
                    }

                    GraphicsDevice.SetVertexBuffer(pSquare.myVertexBuffer);
                    if (pViewVector.Length() < 50)
                    {
                        GraphicsDevice.Indices = pSquare.myIndexBufferHR;
                        GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, pSquare.userPrimitives.Length, 0, pSquare.m_iTrianglesCountHR);

                        //GraphicsDevice.DrawUserIndexedPrimitives<VertexMultitextured>(PrimitiveType.TriangleList,
                        //                            pSquare.userPrimitives, 0, pSquare.userPrimitives.Length - 1, pSquare.userPrimitivesIndicesHR, 0, pSquare.m_iTrianglesCountHR);
                        m_iTrianglesCount += (uint)pSquare.m_iTrianglesCountHR;
                    }
                    else
                    {
                        GraphicsDevice.Indices = pSquare.myIndexBufferLR;
                        GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, pSquare.userPrimitives.Length, 0, pSquare.m_iTrianglesCountLR);

                        //GraphicsDevice.DrawUserIndexedPrimitives<VertexMultitextured>(PrimitiveType.TriangleList,
                        //                            pSquare.userPrimitives, 0, pSquare.userPrimitives.Length - 1, pSquare.userPrimitivesIndicesLR, 0, pSquare.m_iTrianglesCountLR);
                        m_iTrianglesCount += (uint)pSquare.m_iTrianglesCountLR;
                    }
                    iCount++;
                }

            rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            //rs.CullMode = CullMode.CullClockwiseFace;
            rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;

            lineEffect.World = Matrix.Identity;
            lineEffect.View = m_pCamera.View;
            lineEffect.Projection = m_pCamera.Projection;
            lineEffect.CurrentTechnique.Passes[0].Apply();

            if (m_pSelectedSquare != null)
            {
                var bbvertices = m_pSelectedSquare.m_pBounds8.GetVertices();
                var bbindices = m_pSelectedSquare.m_pBounds8.GetIndices();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList,
                                            bbvertices, 0, bbvertices.Length, bbindices, 0, bbindices.Length / 2);
            }
        
            // Draw the outline of the triangle under the cursor.
            DrawPickedTriangle();

            if (m_bUseCelShading)
            {
                /* We are done with the render target so set it back to null.
                 * This will get us back to rendering to the default render target
                 */
                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Viewport = pPort;

                GraphicsDevice.Clear(eSkyColor);
                /* Also in XNA 4.0 applying effects to a sprite is a little different
                 * Use an overload of Begin that takes the effect as a parameter.  Also make
                 * sure to set the sprite batch blend state to Opaque or we will not get black
                 * outlines.
                 */
                m_pSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, outlineShader);
                m_pSpriteBatch.Draw(celTarget, Vector2.Zero, Microsoft.Xna.Framework.Color.White);
                m_pSpriteBatch.End();
            }

            m_fDrawingTime = timer.Elapsed.TotalMilliseconds - lastTime;
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

    }
}
