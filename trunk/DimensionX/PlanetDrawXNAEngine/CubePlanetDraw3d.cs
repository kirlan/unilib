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
using Microsoft.VisualBasic.Devices;

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

        private Microsoft.Xna.Framework.Color m_eSkyColor = Microsoft.Xna.Framework.Color.AliceBlue;
        private Microsoft.Xna.Framework.Color m_eRealSkyColor = Microsoft.Xna.Framework.Color.AliceBlue;

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

        Dictionary<SettlementSize, Dictionary<int, Model>> m_cSettlementModels = new Dictionary<SettlementSize, Dictionary<int, Model>>();
        Dictionary<SettlementSize, Dictionary<int, Texture2D>> m_cSettlementTextures = new Dictionary<SettlementSize, Dictionary<int, Texture2D>>();

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
                    m_aFaces[index++] = new Face(GraphicsDevice, pFace.Value, pCube.R, treeModel, palmModel, pineModel, treeTexture);

                    float fFogHeight = 10;
                    fFogHeight = pCube.R + 15;

                    foreach (Model pModel in treeModel)
                        foreach (ModelMesh mesh in pModel.Meshes)
                            foreach (ModelMeshPart meshPart in mesh.MeshParts)
                            {
                                meshPart.Effect.Parameters["FogHeight"].SetValue(fFogHeight);
                            }

                    foreach (Model pModel in palmModel)
                        foreach (ModelMesh mesh in pModel.Meshes)
                            foreach (ModelMeshPart meshPart in mesh.MeshParts)
                            {
                                meshPart.Effect.Parameters["FogHeight"].SetValue(fFogHeight);
                            }

                    foreach (Model pModel in pineModel)
                        foreach (ModelMesh mesh in pModel.Meshes)
                            foreach (ModelMeshPart meshPart in mesh.MeshParts)
                            {
                                meshPart.Effect.Parameters["FogHeight"].SetValue(fFogHeight);
                            }

                    foreach (var vSettlementSize in m_cSettlementModels)
                        foreach (var vSettlement in vSettlementSize.Value)
                            foreach (ModelMesh mesh in vSettlement.Value.Meshes)
                                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                                {
                                    meshPart.Effect.Parameters["FogHeight"].SetValue(fFogHeight);
                                }

                    pEffectFogHeight.SetValue(fFogHeight);
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
        private Model m_pSunModel;

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

            pEffectAmbientLightColor.SetValue(m_eSkyColor.ToVector4());
            pEffectAmbientLightIntensity.SetValue(0.07f);

            pEffectDirectionalLightColor.SetValue(m_eSunColor.ToVector4());
            pEffectDirectionalLightDirection.SetValue(m_pSunCurrent);
            pEffectDirectionalLightIntensity.SetValue(0.9f);
            //pEffectDirectionalLightIntensity.SetValue(0.8f);

            pEffectSpecularColor.SetValue(0);

            pEffectFogColor.SetValue(m_eSkyColor.ToVector4());
            pEffectFogHeight.SetValue(165);
            pEffectFogModePlain.SetValue(false);
            pEffectFogModeRing.SetValue(false);
            pEffectFogModeSphere.SetValue(true);
//            pEffectFogDensity.SetValue(0.05f);
            pEffectFogDensity.SetValue(0.001f);

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

            LoadTrees();

            LoadSettlements();

            m_pSunModel = LoadModel("content/fbx/SphereLowPoly"); 
            
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

        private void LoadTrees()
        {
            treeTexture = LibContent.Load<Texture2D>("content/dds/trees");

            treeModel[0] = LoadModel("content/fbx/tree1");
            treeModel[1] = LoadModel("content/fbx/tree2");
            treeModel[2] = LoadModel("content/fbx/tree3");
            treeModel[3] = LoadModel("content/fbx/tree4");
            treeModel[4] = LoadModel("content/fbx/tree6");
            treeModel[5] = LoadModel("content/fbx/tree7");
            treeModel[6] = LoadModel("content/fbx/tree8");
            treeModel[7] = LoadModel("content/fbx/tree9");
            treeModel[8] = LoadModel("content/fbx/tree10");
            treeModel[9] = LoadModel("content/fbx/tree11");
            treeModel[10] = LoadModel("content/fbx/tree12");
            treeModel[11] = LoadModel("content/fbx/tree15");
            treeModel[12] = LoadModel("content/fbx/tree16");

            palmModel[0] = LoadModel("content/fbx/palm1");
            palmModel[1] = LoadModel("content/fbx/palm2");
            palmModel[2] = LoadModel("content/fbx/palm3");
            palmModel[3] = LoadModel("content/fbx/palm4");

            pineModel[0] = LoadModel("content/fbx/tree5");
            pineModel[1] = LoadModel("content/fbx/tree13");
            pineModel[2] = LoadModel("content/fbx/tree14");
            pineModel[3] = LoadModel("content/fbx/tree16");
        }

        private void LoadSettlements()
        {
            foreach (SettlementSize eSize in Enum.GetValues(typeof(SettlementSize)))
            {
                m_cSettlementModels[eSize] = new Dictionary<int, Model>();
                m_cSettlementTextures[eSize] = new Dictionary<int, Texture2D>();
            }

            Texture2D pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T0");
            m_cSettlementModels[SettlementSize.Hamlet][0] = LoadModel("content/fbx/hamlet_T0");
            m_cSettlementTextures[SettlementSize.Hamlet][0] = pTexture;
            m_cSettlementModels[SettlementSize.Village][0] = LoadModel("content/fbx/village_T0");
            m_cSettlementTextures[SettlementSize.Village][0] = pTexture;
            m_cSettlementModels[SettlementSize.Town][0] = LoadModel("content/fbx/village_T0");
            m_cSettlementTextures[SettlementSize.Town][0] = pTexture;
            m_cSettlementModels[SettlementSize.City][0] = LoadModel("content/fbx/village_T0");
            m_cSettlementTextures[SettlementSize.City][0] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][0] = LoadModel("content/fbx/village_T0");
            m_cSettlementTextures[SettlementSize.Capital][0] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][0] = LoadModel("content/fbx/fort_T1");
            m_cSettlementTextures[SettlementSize.Fort][0] = LibContent.Load<Texture2D>("content/dds/Fort_T1");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T1");
            m_cSettlementModels[SettlementSize.Hamlet][1] = LoadModel("content/fbx/hamlet_T1");
            m_cSettlementTextures[SettlementSize.Hamlet][1] = pTexture;
            m_cSettlementModels[SettlementSize.Village][1] = LoadModel("content/fbx/village_T1");
            m_cSettlementTextures[SettlementSize.Village][1] = pTexture;
            m_cSettlementModels[SettlementSize.Town][1] = LoadModel("content/fbx/town_T1");
            m_cSettlementTextures[SettlementSize.Town][1] = pTexture;
            m_cSettlementModels[SettlementSize.City][1] = LoadModel("content/fbx/city_T1");
            m_cSettlementTextures[SettlementSize.City][1] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][1] = LoadModel("content/fbx/city_T1");
            m_cSettlementTextures[SettlementSize.Capital][1] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][1] = LoadModel("content/fbx/fort_T1");
            m_cSettlementTextures[SettlementSize.Fort][1] = LibContent.Load<Texture2D>("content/dds/Fort_T1");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T2_small");
            m_cSettlementModels[SettlementSize.Hamlet][2] = LoadModel("content/fbx/hamlet_T2");
            m_cSettlementTextures[SettlementSize.Hamlet][2] = pTexture;
            m_cSettlementModels[SettlementSize.Village][2] = LoadModel("content/fbx/village_T2");
            m_cSettlementTextures[SettlementSize.Village][2] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T2_big");
            m_cSettlementModels[SettlementSize.Town][2] = LoadModel("content/fbx/town_T2");
            m_cSettlementTextures[SettlementSize.Town][2] = pTexture;
            m_cSettlementModels[SettlementSize.City][2] = LoadModel("content/fbx/city_T2");
            m_cSettlementTextures[SettlementSize.City][2] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][2] = LoadModel("content/fbx/city_T2");
            m_cSettlementTextures[SettlementSize.Capital][2] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][2] = LoadModel("content/fbx/fort_T2");
            m_cSettlementTextures[SettlementSize.Fort][2] = LibContent.Load<Texture2D>("content/dds/Fort_T2");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T2_small");
            m_cSettlementModels[SettlementSize.Hamlet][3] = LoadModel("content/fbx/hamlet_T2");
            m_cSettlementTextures[SettlementSize.Hamlet][3] = pTexture;
            m_cSettlementModels[SettlementSize.Village][3] = LoadModel("content/fbx/village_T2");
            m_cSettlementTextures[SettlementSize.Village][3] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T3_big");
            m_cSettlementModels[SettlementSize.Town][3] = LoadModel("content/fbx/town_T3");
            m_cSettlementTextures[SettlementSize.Town][3] = pTexture;
            m_cSettlementModels[SettlementSize.City][3] = LoadModel("content/fbx/city_T3");
            m_cSettlementTextures[SettlementSize.City][3] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][3] = LoadModel("content/fbx/city_T3");
            m_cSettlementTextures[SettlementSize.Capital][3] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][3] = LoadModel("content/fbx/fort_T3");
            m_cSettlementTextures[SettlementSize.Fort][3] = LibContent.Load<Texture2D>("content/dds/Fort_T3");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T4_small");
            m_cSettlementModels[SettlementSize.Hamlet][4] = LoadModel("content/fbx/hamlet_T4");
            m_cSettlementTextures[SettlementSize.Hamlet][4] = pTexture;
            m_cSettlementModels[SettlementSize.Village][4] = LoadModel("content/fbx/village_T4");
            m_cSettlementTextures[SettlementSize.Village][4] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T4_big");
            m_cSettlementModels[SettlementSize.Town][4] = LoadModel("content/fbx/town_T4");
            m_cSettlementTextures[SettlementSize.Town][4] = pTexture;
            m_cSettlementModels[SettlementSize.City][4] = LoadModel("content/fbx/city_T4");
            m_cSettlementTextures[SettlementSize.City][4] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][4] = LoadModel("content/fbx/city_T4");
            m_cSettlementTextures[SettlementSize.Capital][4] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][4] = LoadModel("content/fbx/fort_T3");
            m_cSettlementTextures[SettlementSize.Fort][4] = LibContent.Load<Texture2D>("content/dds/Fort_T3");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T4_small");
            m_cSettlementModels[SettlementSize.Hamlet][5] = LoadModel("content/fbx/hamlet_T4");
            m_cSettlementTextures[SettlementSize.Hamlet][5] = pTexture;
            m_cSettlementModels[SettlementSize.Village][5] = LoadModel("content/fbx/village_T4");
            m_cSettlementTextures[SettlementSize.Village][5] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T5_big");
            m_cSettlementModels[SettlementSize.Town][5] = LoadModel("content/fbx/town_T5");
            m_cSettlementTextures[SettlementSize.Town][5] = pTexture;
            m_cSettlementModels[SettlementSize.City][5] = LoadModel("content/fbx/city_T5");
            m_cSettlementTextures[SettlementSize.City][5] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][5] = LoadModel("content/fbx/city_T5");
            m_cSettlementTextures[SettlementSize.Capital][5] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][5] = LoadModel("content/fbx/fort_T5");
            m_cSettlementTextures[SettlementSize.Fort][5] = LibContent.Load<Texture2D>("content/dds/Fort_T5");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T6_hamlet");
            m_cSettlementModels[SettlementSize.Hamlet][6] = LoadModel("content/fbx/hamlet_T6");
            m_cSettlementTextures[SettlementSize.Hamlet][6] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T6_village");
            m_cSettlementModels[SettlementSize.Village][6] = LoadModel("content/fbx/village_T6");
            m_cSettlementTextures[SettlementSize.Village][6] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T6");
            m_cSettlementModels[SettlementSize.Town][6] = LoadModel("content/fbx/town_T6");
            m_cSettlementTextures[SettlementSize.Town][6] = pTexture;
            m_cSettlementModels[SettlementSize.City][6] = LoadModel("content/fbx/city_T6");
            m_cSettlementTextures[SettlementSize.City][6] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][6] = LoadModel("content/fbx/city_T6");
            m_cSettlementTextures[SettlementSize.Capital][6] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][6] = LoadModel("content/fbx/fort_T6");
            m_cSettlementTextures[SettlementSize.Fort][6] = LibContent.Load<Texture2D>("content/dds/Fort_T6");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T7");
            m_cSettlementModels[SettlementSize.Hamlet][7] = LoadModel("content/fbx/hamlet_T7");
            m_cSettlementTextures[SettlementSize.Hamlet][7] = pTexture;
            m_cSettlementModels[SettlementSize.Village][7] = LoadModel("content/fbx/village_T7");
            m_cSettlementTextures[SettlementSize.Village][7] = pTexture;
            m_cSettlementModels[SettlementSize.Town][7] = LoadModel("content/fbx/town_T7");
            m_cSettlementTextures[SettlementSize.Town][7] = pTexture;
            m_cSettlementModels[SettlementSize.City][7] = LoadModel("content/fbx/city_T7");
            m_cSettlementTextures[SettlementSize.City][7] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][7] = LoadModel("content/fbx/city_T7");
            m_cSettlementTextures[SettlementSize.Capital][7] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][7] = LoadModel("content/fbx/fort_T7");
            m_cSettlementTextures[SettlementSize.Fort][7] = LibContent.Load<Texture2D>("content/dds/Fort_T7");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T8");
            m_cSettlementModels[SettlementSize.Hamlet][8] = LoadModel("content/fbx/hamlet_T8");
            m_cSettlementTextures[SettlementSize.Hamlet][8] = pTexture;
            m_cSettlementModels[SettlementSize.Village][8] = LoadModel("content/fbx/village_T8");
            m_cSettlementTextures[SettlementSize.Village][8] = pTexture;
            m_cSettlementModels[SettlementSize.Town][8] = LoadModel("content/fbx/town_T8");
            m_cSettlementTextures[SettlementSize.Town][8] = pTexture;
            m_cSettlementModels[SettlementSize.City][8] = LoadModel("content/fbx/city_T8");
            m_cSettlementTextures[SettlementSize.City][8] = pTexture;
            m_cSettlementModels[SettlementSize.Capital][8] = LoadModel("content/fbx/city_T8");
            m_cSettlementTextures[SettlementSize.Capital][8] = pTexture;
            m_cSettlementModels[SettlementSize.Fort][8] = LoadModel("content/fbx/fort_T7");
            m_cSettlementTextures[SettlementSize.Fort][8] = LibContent.Load<Texture2D>("content/dds/Fort_T7");
        }

        /// <summary>
        /// m_pMyEffect должен уже быть создан и настроен!
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        private Model LoadModel(string sPath)
        {
            Model pModel = LibContent.Load<Model>(sPath);
            foreach (ModelMesh mesh in pModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = m_pMyEffect.Clone();

            return pModel;
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
        
        public void ResetFPS()
        {
            m_iFrame = 0;
        }

        public Vector3? m_pTarget = null;
        private Square m_pFocusedSquare = null;

        private Matrix m_pWorldMatrix = Matrix.Identity;
        private Vector3 m_pHorizon;

        public int VisibleQueue { get { return Square.s_pVisibleQueue.Count; } }
        public int InvisibleQueue { get { return Square.s_pInvisibleQueue.Count; } }

        private void UpdateCamera()
        {
            if (m_fScaling != 0)
            {
                m_pCamera.ZoomIn(m_fScaling * (float)Math.Sqrt(m_fFrameTime));
                m_fScaling = 0;
            }

            if (m_pTarget.HasValue)
                m_pCamera.MoveTarget(m_pTarget.Value, 0.005f * (float)m_fFrameTime);

            if (m_pCamera.Update())
            {
                bool bCameraChanged = false;

                Vector3? pCharacter = GetSurface(m_pCamera.FocusPoint, out m_pFocusedSquare);
                if (pCharacter.HasValue)
                {
                    pCharacter += Vector3.Normalize(pCharacter.Value);// *1.2f;
                    m_pCamera.Position += pCharacter.Value - m_pCamera.FocusPoint;
                    m_pCamera.FocusPoint = pCharacter.Value;

                    bCameraChanged = true;
                }
                else
                {
                    pCharacter = GetSurface(m_pCamera.FocusPoint, out m_pFocusedSquare);
                    //throw new Exception();
                }

                //Убедимся, что камера достаточно высоко над землёй
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
                            bCameraChanged = true;
                        }
                    }
                }

                if (bCameraChanged)
                    m_pCamera.View = Matrix.CreateLookAt(m_pCamera.Position, m_pCamera.FocusPoint, m_pCamera.Top);

                float h = m_pCamera.Position.Length();
                float d = h * (float)Math.Sqrt(h * h / (m_pCube.R * m_pCube.R) - 1);
                m_pHorizon = Vector3.Normalize(m_pCamera.Position - Vector3.Normalize(Vector3.Cross(m_pCamera.Position, m_pCamera.Left)) * d);
                //pHorizon = Vector3.Normalize(pHorizon) * m_pCube.R;

                Vector3 pTop = Vector3.Normalize(m_pCamera.FocusPoint);
                Vector3 pForward = Vector3.Cross(pTop, m_pCamera.Left);

                Matrix T = new Matrix(m_pCamera.Left.X, pTop.X, pForward.X, 0,
                                      m_pCamera.Left.Y, pTop.Y, pForward.Y, 0,
                                      m_pCamera.Left.Z, pTop.Z, pForward.Z, 0,
                                      0, 0, 0, 1);
                m_pWorldMatrix = Matrix.Multiply(Matrix.Multiply(T, Matrix.CreateScale(1, 0.5f, 1)), Matrix.Invert(T));
                m_pWorldMatrix = Matrix.Multiply(m_pWorldMatrix, Matrix.CreateTranslation(pTop * m_pCube.R / 2));

                BoundingFrustum pFrustrum = new BoundingFrustum(Matrix.Multiply(m_pCamera.View, m_pCamera.Projection));

                foreach (var pFace in m_aFaces)
                    foreach (var pSquare in pFace.m_aSquares)
                        pSquare.UpdateVisible(GraphicsDevice, pFrustrum, m_pCamera.Position, m_pCamera.Direction, m_pCamera.FocusPoint, m_pWorldMatrix);

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

            float diff = Vector3.Dot(-m_pSunCurrent, m_pHorizon) + 0.3f;
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

            //double startTime = timer.Elapsed.TotalMilliseconds;

            m_iFrame++; 
            
            m_fFrameTime = timer.Elapsed.TotalMilliseconds - lastTime;
            lastTime = timer.Elapsed.TotalMilliseconds; 
            
            if (m_pCube == null)
                return;

            UpdateCamera();

            UpdateLight();

            m_pDebugInfo = new VertexPositionColor[4];
            m_pDebugInfo[0] = new VertexPositionColor(Vector3.Zero, Microsoft.Xna.Framework.Color.Black);
            m_pDebugInfo[1] = new VertexPositionColor(m_pCamera.FocusPoint, Microsoft.Xna.Framework.Color.Black);
            m_pDebugInfo[2] = new VertexPositionColor(-m_pPole * 200, Microsoft.Xna.Framework.Color.DarkRed);
            m_pDebugInfo[3] = new VertexPositionColor(m_pPole * 200, Microsoft.Xna.Framework.Color.Violet);

            pEffectView.SetValue(m_pCamera.View);
            pEffectProjection.SetValue(m_pCamera.Projection);
            pEffectCameraPosition.SetValue(m_pCamera.Position);
            pEffectWorld.SetValue(m_pWorldMatrix);

            // Set renderstates.
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullClockwiseFace;
            if (m_bWireFrame)
            {
                rs.CullMode = CullMode.None;
                rs.FillMode = FillMode.WireFrame;
            }
            GraphicsDevice.RasterizerState = rs;

            Viewport pPort = GraphicsDevice.Viewport;

            m_pMyEffect.Parameters["UseCelShading"].SetValue(m_bUseCelShading);
            if (m_bUseCelShading && !m_bWireFrame)
            {
                if (GraphicsDevice.Viewport.Width != celTarget.Width || GraphicsDevice.Viewport.Height != celTarget.Height)
                    celTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height,
                        false, SurfaceFormat.Color, DepthFormat.Depth24);
            }

            //pEffectDirectionalLightDirection.SetValue(-Vector3.Normalize(m_pCamera.FocusPoint));
            pEffectDirectionalLightDirection.SetValue(m_pSunCurrent);
            pEffectDirectionalLightIntensity.SetValue(0.9f);//0.8f
            pEffectDirectionalLightColor.SetValue(m_eSunColor.ToVector4());

            int iCount = 0;
            m_iTrianglesCount = 0;

            GraphicsDevice.SetRenderTarget(refractionRenderTarget);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Land"];
            m_pMyEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.Clear(m_eRealSkyColor);

            foreach (var pSquare in Square.s_pVisibleQueue)
            {
                if (pSquare.m_iUnderwaterTrianglesCount > 0)
                {
                    GraphicsDevice.SetVertexBuffer(pSquare.m_pVertexBuffer);
                    GraphicsDevice.Indices = pSquare.m_pUnderwaterIndexBuffer;
                    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, pSquare.g.m_aLandPoints.Length, 0, pSquare.m_iUnderwaterTrianglesCount);

                    m_iTrianglesCount += (uint)pSquare.m_iUnderwaterTrianglesCount;
                    iCount++;
                }
            }

            if (m_bUseCelShading && !m_bWireFrame)
            {
                GraphicsDevice.SetRenderTarget(celTarget);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else
            {
                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Viewport = pPort;
            }

            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Land"];
            m_pMyEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.Clear(m_eRealSkyColor);

            foreach (var pSquare in Square.s_pVisibleQueue)
            {
                GraphicsDevice.SetVertexBuffer(pSquare.m_pVertexBuffer);
                if (pSquare.m_fVisibleDistance < m_fLODDistance)
                {
                    GraphicsDevice.Indices = pSquare.m_pLandIndexBufferHR;
                    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, pSquare.g.m_aLandPoints.Length, 0, pSquare.m_iLandTrianglesCountHR);
                    m_iTrianglesCount += (uint)pSquare.m_iLandTrianglesCountHR;
                }
                else
                {
                    GraphicsDevice.Indices = pSquare.m_pLandIndexBufferLR;
                    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, pSquare.g.m_aLandPoints.Length, 0, pSquare.m_iLandTrianglesCountLR);
                    m_iTrianglesCount += (uint)pSquare.m_iLandTrianglesCountLR;
                }
                iCount++;
            }

            if (m_bDrawTrees)
            {
                DrawTrees();
            }

            //DrawSun();

            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Water"];
            //effect.Parameters["xReflectionView"].SetValue(reflectionViewMatrix);
            //effect.Parameters["xReflectionMap"].SetValue(reflectionMap);
            m_pMyEffect.Parameters["xRefractionMap"].SetValue(refractionRenderTarget);

            m_pMyEffect.CurrentTechnique.Passes[0].Apply();

            foreach (var pSquare in Square.s_pVisibleQueue)
            {
                if (pSquare.m_iUnderwaterTrianglesCount > 0)
                {
                    GraphicsDevice.SetVertexBuffer(pSquare.m_pWaterVertexBuffer);
                    GraphicsDevice.Indices = pSquare.m_pWaterIndexBuffer;
                    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, pSquare.g.m_aWaterPoints.Length, 0, pSquare.m_iWaterTrianglesCount);

                    m_iTrianglesCount += (uint)pSquare.m_iWaterTrianglesCount;
                    iCount++;
                }
            }

            if (m_bShowBounds)
            {
                rs = new RasterizerState();
                rs.CullMode = CullMode.None;
                //rs.CullMode = CullMode.CullClockwiseFace;
                rs.FillMode = FillMode.WireFrame;
                GraphicsDevice.RasterizerState = rs;

                lineEffect.World = m_pWorldMatrix;
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
            }

            // Draw the outline of the triangle under the cursor.
            DrawPickedTriangle();
            DrawDebugInfo();

            if (m_bUseCelShading && !m_bWireFrame)
            {
                /* We are done with the render target so set it back to null.
                 * This will get us back to rendering to the default render target
                 */
                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Viewport = pPort;

                GraphicsDevice.Clear(m_eRealSkyColor);
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

        private void DrawSun()
        { 
            ModelMesh pSunMesh = m_pSunModel.Meshes[0];
            ModelMeshPart pSunMeshPart = pSunMesh.MeshParts[0];
            GraphicsDevice.SetVertexBuffer(pSunMeshPart.VertexBuffer, pSunMeshPart.VertexOffset);
            GraphicsDevice.Indices = pSunMeshPart.IndexBuffer;

            lineEffect.World = Matrix.CreateTranslation(-m_pSunCurrent * 200)*m_pWorldMatrix;
            lineEffect.Projection = m_pCamera.Projection;
            lineEffect.View = m_pCamera.View;

            lineEffect.CurrentTechnique.Passes[0].Apply();

            //sampleMesh contains all of the information required to draw
            //the current mesh
            GraphicsDevice.DrawIndexedPrimitives(
                PrimitiveType.TriangleList, 0, 0,
                pSunMeshPart.NumVertices, pSunMeshPart.StartIndex, pSunMeshPart.PrimitiveCount);

            //m_pSunModel.Draw(Matrix.CreateTranslation(-m_pSun * 200), m_pCamera.View, m_pCamera.Projection);
        }

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

                DrawModelHardwareInstancing(pTreeModel.Key, instancedModelBones,
                                         pTreeModel.Value.ToArray(), m_pCamera.View, m_pCamera.Projection);
            }
        }

        // To store instance transform matrices in a vertex buffer, we use this custom
        // vertex type which encodes 4x4 matrices as a set of four Vector4 values.
        static VertexDeclaration instanceVertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 1),
            new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 2),
            new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 3)
        );
        private DynamicVertexBuffer instanceVertexBuffer;

        /// <summary>
        /// Efficiently draws several copies of a piece of geometry using hardware instancing.
        /// </summary>
        private void DrawModelHardwareInstancing(Model model, Matrix[] modelBones,
                                         Matrix[] instances, Matrix view, Matrix projection)
        {
            if (instances.Length == 0)
                return;

            // If we have more instances than room in our vertex buffer, grow it to the neccessary size.
            if ((instanceVertexBuffer == null) ||
                (instances.Length > instanceVertexBuffer.VertexCount))
            {
                if (instanceVertexBuffer != null)
                    instanceVertexBuffer.Dispose();

                instanceVertexBuffer = new DynamicVertexBuffer(GraphicsDevice, instanceVertexDeclaration,
                                                               instances.Length, BufferUsage.WriteOnly);
            }

            // Transfer the latest instance transform matrices into the instanceVertexBuffer.
            instanceVertexBuffer.SetData(instances, 0, instances.Length, SetDataOptions.Discard);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Tell the GPU to read from both the model vertex buffer plus our instanceVertexBuffer.
                    GraphicsDevice.SetVertexBuffers(
                        new VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset, 0),
                        new VertexBufferBinding(instanceVertexBuffer, 0, 1)
                    );

                    GraphicsDevice.Indices = meshPart.IndexBuffer;

                    // Set up the instance rendering effect.
                    Effect effect = meshPart.Effect;

                    //effect.CurrentTechnique = effect.Techniques["HardwareInstancing"];

                    effect.Parameters["World"].SetValue(modelBones[mesh.ParentBone.Index]);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["CameraPosition"].SetValue(m_pCamera.Position);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["DirectionalLightDirection"].SetValue(m_pSunCurrent);
                    effect.Parameters["DirectionalLightColor"].SetValue(m_eSunColor.ToVector4());
                    
                    // Draw all the instance copies in a single call.
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                                               meshPart.NumVertices, meshPart.StartIndex,
                                                               meshPart.PrimitiveCount, instances.Length);
                    }
                }
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
                RasterizerState rs = new RasterizerState();
                rs.CullMode = CullMode.None;
                rs.FillMode = FillMode.WireFrame;
                GraphicsDevice.RasterizerState = rs;
                GraphicsDevice.DepthStencilState = DepthStencilState.None;

                // Activate the line drawing BasicEffect.
                lineEffect.World = m_pWorldMatrix;
                lineEffect.Projection = m_pCamera.Projection;
                lineEffect.View = m_pCamera.View;

                lineEffect.CurrentTechnique.Passes[0].Apply();

                // Draw the triangle.
                //GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                //                          m_aPickedTriangle, 0, 1);
                if (m_pSelectedSquare != null)
                {
                    m_pSelectedSquare.Rebuild(GraphicsDevice);
                    GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList,
                                          m_pSelectedSquare.g.m_aLandPoints, 0, m_pSelectedSquare.g.m_aLandPoints.Length - 1, m_pSelectedSquare.g.m_aLocations[m_iFocusedLocation], 0, m_pSelectedSquare.g.m_aLocations[m_iFocusedLocation].Length / 2);
                }

                // Reset renderstates to their default values.
                GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
        }

        /// <summary>
        /// Helper for drawing the outline of the triangle currently under the cursor.
        /// </summary>
        void DrawDebugInfo()
        {
            // Activate the line drawing BasicEffect.
            lineEffect.World = m_pWorldMatrix;
            lineEffect.Projection = m_pCamera.Projection;
            lineEffect.View = m_pCamera.View;

            lineEffect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                PrimitiveType.LineList,
                m_pDebugInfo,
                0,  // index of the first vertex to draw
                m_pDebugInfo.Length / 2   // number of primitives
            );
        }

        VertexPositionColor[] m_pDebugInfo;

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
