using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ContentLoader
{
    public enum SettlementSizes
    {
        Hamlet,
        Village,
        Fort,
        Town,
        City,
        Capital
    }

    public struct VertexMultitextured : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector4 TextureCoordinate;
        public Vector4 TexWeights;
        public Vector4 TexWeights2;
        public Vector3 Tangent;
        public Microsoft.Xna.Framework.Color Color;

        public readonly static long Size = sizeof(float) * 21 + sizeof(byte) * 4;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
         (
             new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
             new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
             new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0),
             new VertexElement(sizeof(float) * 10, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1),
             new VertexElement(sizeof(float) * 14, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 2),
             new VertexElement(sizeof(float) * 18, VertexElementFormat.Vector3, VertexElementUsage.Tangent, 0),
             new VertexElement(sizeof(float) * 21, VertexElementFormat.Color, VertexElementUsage.Color, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
    }

    public static class Shader
    {
        private static ContentManager LibContent;

        public static Dictionary<SettlementSizes, Dictionary<int, Model>> m_cSettlementModels = new Dictionary<SettlementSizes, Dictionary<int, Model>>();
        public static Dictionary<SettlementSizes, Dictionary<int, Texture2D>> m_cSettlementTextures = new Dictionary<SettlementSizes, Dictionary<int, Texture2D>>();

        private static RenderTarget2D refractionRenderTarget;
        private static RenderTarget2D celTarget;

        private static BasicEffect m_pBasicEffect;
        private static Effect m_pMyEffect;

        public static void DrawLandscape(VertexBuffer pVBuffer, IndexBuffer pIBuffer, int iVBufferSize, int iIBufferSize)
        {
            m_pGraphicsDevice.SetVertexBuffer(pVBuffer);
            m_pGraphicsDevice.Indices = pIBuffer;
            m_pGraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, iVBufferSize, 0, iIBufferSize);
        }

        public static void PrepareDrawUnderwater(Color eBackground)
        {
            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Land"];
            m_pMyEffect.CurrentTechnique.Passes[0].Apply();

            m_pGraphicsDevice.SetRenderTarget(refractionRenderTarget);
            m_pGraphicsDevice.DepthStencilState = DepthStencilState.Default;

            m_pGraphicsDevice.Clear(eBackground);
        }

        public static void PrepareDrawLand(Color eBackground)
        {
            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Land"];
            m_pMyEffect.CurrentTechnique.Passes[0].Apply();

            if (m_pMyEffect.Parameters["UseCelShading"].GetValueBoolean())
            {
                m_pGraphicsDevice.SetRenderTarget(celTarget);
                m_pGraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else
            {
                m_pGraphicsDevice.SetRenderTarget(null);
                m_pGraphicsDevice.Viewport = pPort;
            }

            m_pGraphicsDevice.Clear(eBackground);
        }

        public static void PrepareDrawWater(float time, Vector3 pCamDir)
        {
            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Water"];
            //effect.Parameters["xReflectionView"].SetValue(reflectionViewMatrix);
            //effect.Parameters["xReflectionMap"].SetValue(reflectionMap);
            m_pMyEffect.Parameters["xRefractionMap"].SetValue(refractionRenderTarget);
            m_pWavesTime.SetValue(time);
            m_pMyEffect.Parameters["xWindDirection"].SetValue(pCamDir);

            m_pMyEffect.CurrentTechnique.Passes[0].Apply();
        }

        public static void PrepareDrawLines(bool bDepthStencil)
        {
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            //rs.CullMode = CullMode.CullClockwiseFace;
            rs.FillMode = FillMode.WireFrame;
            m_pGraphicsDevice.RasterizerState = rs;
            m_pGraphicsDevice.DepthStencilState = bDepthStencil ? DepthStencilState.None : DepthStencilState.Default;

            m_pLineEffect.CurrentTechnique.Passes[0].Apply();
        }

        public static void DrawLines(VertexPositionColor[] bbvertices, int[] bbindices)
        {
            m_pGraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList,
                                        bbvertices, 0, bbvertices.Length, bbindices, 0, bbindices.Length / 2);
        }

        public static void DrawLines(VertexPositionColor[] bbvertices)
        {
            m_pGraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList,
                                        bbvertices, 0, bbvertices.Length / 2);
        }

        public static void DrawLines(VertexMultitextured[] bbvertices, int[] bbindices)
        {
            m_pGraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList,
                                        bbvertices, 0, bbvertices.Length, bbindices, 0, bbindices.Length / 2);
        }

        public static void SetMatrices(Matrix pWorld, Matrix pView, Matrix pProjection, Vector3 pCamera)
        {
            m_pWorld.SetValue(pWorld);
            m_pView.SetValue(pView);
            m_pProjection.SetValue(pProjection);
            m_pCameraPosition.SetValue(pCamera);

            m_pLineEffect.World = pWorld;
            m_pLineEffect.View = pView;
            m_pLineEffect.Projection = pProjection;
        }
        
        public static void SetDirectionalLight(Vector3 pDirection, float fIntesity, Color eColor)
        {
            m_pDirectionalLightDirection.SetValue(pDirection);
            m_pDirectionalLightIntensity.SetValue(fIntesity);//0.8f
            m_pDirectionalLightColor.SetValue(eColor.ToVector4());
        }

        public static void SetAmbientLight(Color pColor, float fIntensity)
        {
            m_pMyEffect.Parameters["AmbientLightColor"].SetValue(pColor.ToVector4());
            m_pMyEffect.Parameters["AmbientLightIntensity"].SetValue(fIntensity);
        }

        public static void SetFog(Color pColor, float fMaxHeight, float fDensity)
        {
            m_pMyEffect.Parameters["FogColor"].SetValue(pColor.ToVector4());
            m_pMyEffect.Parameters["FogHeight"].SetValue(fMaxHeight);
            m_pMyEffect.Parameters["FogDensity"].SetValue(fDensity);
        }

        static EffectParameter m_pSpecularColor;

        static EffectParameter m_pBlendDistance;
        static EffectParameter m_pBlendWidth;
        static EffectParameter m_pUseCelShading;

        static EffectParameter m_pDirectionalLightDirection;
        static EffectParameter m_pDirectionalLightIntensity;
        static EffectParameter m_pDirectionalLightColor;

        static EffectParameter m_pWorld;
        static EffectParameter m_pView;
        static EffectParameter m_pProjection;
        static EffectParameter m_pCameraPosition;

        static EffectParameter m_pModelBoneWorld;

        static EffectParameter m_pWavesTime;

        private static Viewport pPort;

        private static bool m_bUseCelShading = false;

        public static void BeginDraw(bool bUseCelShading, bool bWireFrame)
        {
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullClockwiseFace;
            if (bWireFrame)
            {
                rs.CullMode = CullMode.None;
                rs.FillMode = FillMode.WireFrame;
            }
            m_pGraphicsDevice.RasterizerState = rs;
            m_pGraphicsDevice.DepthStencilState = DepthStencilState.Default;
            
            pPort = m_pGraphicsDevice.Viewport;

            if (m_bUseCelShading != bUseCelShading && !bWireFrame)
            {
                m_bUseCelShading = bUseCelShading && !bWireFrame;

                m_pUseCelShading.SetValue(m_bUseCelShading);
            }

            if (m_bUseCelShading)
            {
                if (m_pGraphicsDevice.Viewport.Width != celTarget.Width || m_pGraphicsDevice.Viewport.Height != celTarget.Height)
                    celTarget = new RenderTarget2D(m_pGraphicsDevice, m_pGraphicsDevice.Viewport.Width, m_pGraphicsDevice.Viewport.Height,
                        false, SurfaceFormat.Color, DepthFormat.Depth24);
            }
        }

        public static void FinishDraw(Color eBackground)
        {
            if (m_pMyEffect.Parameters["UseCelShading"].GetValueBoolean())
            {
                /* We are done with the render target so set it back to null.
                 * This will get us back to rendering to the default render target
                 */
                m_pGraphicsDevice.SetRenderTarget(null);
                m_pGraphicsDevice.Viewport = pPort;

                m_pGraphicsDevice.Clear(eBackground);
                /* Also in XNA 4.0 applying effects to a sprite is a little different
                 * Use an overload of Begin that takes the effect as a parameter.  Also make
                 * sure to set the sprite batch blend state to Opaque or we will not get black
                 * outlines.
                 */
                m_pSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, m_pOutlineShader);
                m_pSpriteBatch.Draw(celTarget, Vector2.Zero, Microsoft.Xna.Framework.Color.White);
                m_pSpriteBatch.End();
            }
        }

        static Texture2D grassTexture;
        static Texture2D sandTexture;
        static Texture2D rockTexture;
        static Texture2D snowTexture;
        static Texture2D forestTexture;
        static Texture2D roadTexture;
        static Texture2D swampTexture;
        static Texture2D lavaTexture;
        
        static Texture2D waterBumpMap;
        
        static Texture2D grassBump;
        static Texture2D sandBump;
        static Texture2D rockBump;
        static Texture2D snowBump;
        static Texture2D forestBump;
        static Texture2D roadBump;
        static Texture2D swampBump;
        static Texture2D lavaBump;

        public static Texture2D m_pTreeTexture;

        public static Model[] m_aTreeModels = new Model[13];
        public static Model[] m_aPalmModels = new Model[4];
        public static Model[] m_aPineModels = new Model[4];

        public static SpriteFont m_pVillageNameFont;
        public static SpriteFont m_pTownNameFont;
        public static SpriteFont m_pCityNameFont;
        public static SpriteBatch m_pSpriteBatch;

        static Effect m_pOutlineShader;   // Outline shader effect
        static float defaultThickness = 1.5f;  // default outline thickness
        static float defaultThreshold = 0.2f;  // default edge detection threshold
        static float outlineThickness = 0.5f;  // current outline thickness
        static float outlineThreshold = 0.12f;  // current edge detection threshold
        static float tStep = 0.01f;    // Ammount to step the line thickness by
        static float hStep = 0.001f;   // Ammount to step the threshold by

        /* Render target to capture cel-shaded render for edge detection
         * post processing
         */
        static Texture2D celMap;       // Texture map for cell shading

        static BasicEffect m_pLineEffect;

        static BasicEffect m_pTextEffect;

        public static Model m_pSunModel;
        
        static GraphicsDevice m_pGraphicsDevice;
        
        public static void Init(GraphicsDevice GraphicsDevice, IServiceProvider Services)
        {
            m_pGraphicsDevice = GraphicsDevice;

            m_pBasicEffect = new BasicEffect(GraphicsDevice);
            m_pBasicEffect.VertexColorEnabled = true;
            m_pSpriteBatch = new SpriteBatch(GraphicsDevice);

            LibContent = new ContentManager(Services);

            // Create our effect.
            LoadTerrainTextures();

            m_pVillageNameFont = LibContent.Load<SpriteFont>("content/villagename");
            m_pTownNameFont = LibContent.Load<SpriteFont>("content/townname");
            m_pCityNameFont = LibContent.Load<SpriteFont>("content/cityname");

            m_pMyEffect = LibContent.Load<Effect>("content/Effect1");

            celMap = LibContent.Load<Texture2D>("content/celMap");
            m_pMyEffect.Parameters["CelMap"].SetValue(celMap);

            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Land"];

            m_pMyEffect.Parameters["xTexture0"].SetValue(sandTexture);
            m_pMyEffect.Parameters["xTexture1"].SetValue(grassTexture);
            m_pMyEffect.Parameters["xTexture2"].SetValue(rockTexture);
            m_pMyEffect.Parameters["xTexture3"].SetValue(snowTexture);
            m_pMyEffect.Parameters["xTexture4"].SetValue(forestTexture);
            m_pMyEffect.Parameters["xTexture5"].SetValue(roadTexture);
            m_pMyEffect.Parameters["xTexture6"].SetValue(swampTexture);
            m_pMyEffect.Parameters["xTexture7"].SetValue(lavaTexture);

            m_pMyEffect.Parameters["BumpMap0"].SetValue(rockBump);

            m_pMyEffect.Parameters["xWaterBumpMap"].SetValue(waterBumpMap);

            m_pMyEffect.Parameters["xWaveLength"].SetValue(0.1f);
            m_pMyEffect.Parameters["xWaveHeight"].SetValue(3f);
            m_pMyEffect.Parameters["xWindForce"].SetValue(0.002f);
            m_pMyEffect.Parameters["xWindDirection"].SetValue(new Vector3(0, 0, 1));

            m_pMyEffect.Parameters["GridColor1"].SetValue(Microsoft.Xna.Framework.Color.Black.ToVector4());
            m_pMyEffect.Parameters["GridColor2"].SetValue(Microsoft.Xna.Framework.Color.Pink.ToVector4());
            m_pMyEffect.Parameters["GridColor3"].SetValue(Microsoft.Xna.Framework.Color.White.ToVector4());
            m_pMyEffect.Parameters["GridColor4"].SetValue(Microsoft.Xna.Framework.Color.Goldenrod.ToVector4());
            m_pMyEffect.Parameters["GridColor5"].SetValue(Microsoft.Xna.Framework.Color.Yellow.ToVector4());
            m_pMyEffect.Parameters["GridColor6"].SetValue(Microsoft.Xna.Framework.Color.Black.ToVector4());

            // create the effect and vertex declaration for drawing the
            // picked triangle.
            m_pLineEffect = new BasicEffect(GraphicsDevice);
            m_pLineEffect.VertexColorEnabled = true;

            LoadTrees();

            LoadSettlements();

            BindEffectParameters();

            m_pSpecularColor.SetValue(0);

            m_pBlendDistance.SetValue(20);//2
            m_pBlendWidth.SetValue(40);

            m_pSunModel = LoadModel("content/fbx/SphereLowPoly");

            m_pTextEffect = new BasicEffect(GraphicsDevice);

            /* Load and initialize the outline shader effect
             */
            m_pOutlineShader = LibContent.Load<Effect>("content/OutlineShader");
            m_pOutlineShader.Parameters["Thickness"].SetValue(outlineThickness);
            m_pOutlineShader.Parameters["Threshold"].SetValue(outlineThreshold);
            m_pOutlineShader.Parameters["ScreenSize"].SetValue(
                new Vector2(GraphicsDevice.Viewport.Bounds.Width, GraphicsDevice.Viewport.Bounds.Height));

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            refractionRenderTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat);

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
        }
        private static void LoadTrees()
        {
            m_pTreeTexture = LibContent.Load<Texture2D>("content/dds/trees");

            m_aTreeModels[0] = LoadModel("content/fbx/tree1");
            m_aTreeModels[1] = LoadModel("content/fbx/tree2");
            m_aTreeModels[2] = LoadModel("content/fbx/tree3");
            m_aTreeModels[3] = LoadModel("content/fbx/tree4");
            m_aTreeModels[4] = LoadModel("content/fbx/tree6");
            m_aTreeModels[5] = LoadModel("content/fbx/tree7");
            m_aTreeModels[6] = LoadModel("content/fbx/tree8");
            m_aTreeModels[7] = LoadModel("content/fbx/tree9");
            m_aTreeModels[8] = LoadModel("content/fbx/tree10");
            m_aTreeModels[9] = LoadModel("content/fbx/tree11");
            m_aTreeModels[10] = LoadModel("content/fbx/tree12");
            m_aTreeModels[11] = LoadModel("content/fbx/tree15");
            m_aTreeModels[12] = LoadModel("content/fbx/tree16");

            m_aPalmModels[0] = LoadModel("content/fbx/palm1");
            m_aPalmModels[1] = LoadModel("content/fbx/palm2");
            m_aPalmModels[2] = LoadModel("content/fbx/palm3");
            m_aPalmModels[3] = LoadModel("content/fbx/palm4");

            m_aPineModels[0] = LoadModel("content/fbx/tree5");
            m_aPineModels[1] = LoadModel("content/fbx/tree13");
            m_aPineModels[2] = LoadModel("content/fbx/tree14");
            m_aPineModels[3] = LoadModel("content/fbx/tree16");

            m_pMyEffect.Parameters["xTextureModel"].SetValue(m_pTreeTexture);
        }

        private static void LoadSettlements()
        {
            foreach (SettlementSizes eSize in Enum.GetValues(typeof(SettlementSizes)))
            {
                m_cSettlementModels[eSize] = new Dictionary<int, Model>();
                m_cSettlementTextures[eSize] = new Dictionary<int, Texture2D>();
            }

            Texture2D pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T0");
            m_cSettlementModels[SettlementSizes.Hamlet][0] = LoadModel("content/fbx/hamlet_T0");
            m_cSettlementTextures[SettlementSizes.Hamlet][0] = pTexture;
            m_cSettlementModels[SettlementSizes.Village][0] = LoadModel("content/fbx/village_T0");
            m_cSettlementTextures[SettlementSizes.Village][0] = pTexture;
            m_cSettlementModels[SettlementSizes.Town][0] = LoadModel("content/fbx/village_T0");
            m_cSettlementTextures[SettlementSizes.Town][0] = pTexture;
            m_cSettlementModels[SettlementSizes.City][0] = LoadModel("content/fbx/village_T0");
            m_cSettlementTextures[SettlementSizes.City][0] = pTexture;
            m_cSettlementModels[SettlementSizes.Capital][0] = LoadModel("content/fbx/village_T0");
            m_cSettlementTextures[SettlementSizes.Capital][0] = pTexture;
            m_cSettlementModels[SettlementSizes.Fort][0] = LoadModel("content/fbx/fort_T1");
            m_cSettlementTextures[SettlementSizes.Fort][0] = LibContent.Load<Texture2D>("content/dds/Fort_T1");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T1");
            m_cSettlementModels[SettlementSizes.Hamlet][1] = LoadModel("content/fbx/hamlet_T1");
            m_cSettlementTextures[SettlementSizes.Hamlet][1] = pTexture;
            m_cSettlementModels[SettlementSizes.Village][1] = LoadModel("content/fbx/village_T1");
            m_cSettlementTextures[SettlementSizes.Village][1] = pTexture;
            m_cSettlementModels[SettlementSizes.Town][1] = LoadModel("content/fbx/town_T1");
            m_cSettlementTextures[SettlementSizes.Town][1] = pTexture;
            m_cSettlementModels[SettlementSizes.City][1] = LoadModel("content/fbx/city_T1");
            m_cSettlementTextures[SettlementSizes.City][1] = pTexture;
            m_cSettlementModels[SettlementSizes.Capital][1] = LoadModel("content/fbx/city_T1");
            m_cSettlementTextures[SettlementSizes.Capital][1] = pTexture;
            m_cSettlementModels[SettlementSizes.Fort][1] = LoadModel("content/fbx/fort_T1");
            m_cSettlementTextures[SettlementSizes.Fort][1] = LibContent.Load<Texture2D>("content/dds/Fort_T1");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T2_small");
            m_cSettlementModels[SettlementSizes.Hamlet][2] = LoadModel("content/fbx/hamlet_T2");
            m_cSettlementTextures[SettlementSizes.Hamlet][2] = pTexture;
            m_cSettlementModels[SettlementSizes.Village][2] = LoadModel("content/fbx/village_T2");
            m_cSettlementTextures[SettlementSizes.Village][2] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T2_big");
            m_cSettlementModels[SettlementSizes.Town][2] = LoadModel("content/fbx/town_T2");
            m_cSettlementTextures[SettlementSizes.Town][2] = pTexture;
            m_cSettlementModels[SettlementSizes.City][2] = LoadModel("content/fbx/city_T2");
            m_cSettlementTextures[SettlementSizes.City][2] = pTexture;
            m_cSettlementModels[SettlementSizes.Capital][2] = LoadModel("content/fbx/city_T2");
            m_cSettlementTextures[SettlementSizes.Capital][2] = pTexture;
            m_cSettlementModels[SettlementSizes.Fort][2] = LoadModel("content/fbx/fort_T2");
            m_cSettlementTextures[SettlementSizes.Fort][2] = LibContent.Load<Texture2D>("content/dds/Fort_T2");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T2_small");
            m_cSettlementModels[SettlementSizes.Hamlet][3] = LoadModel("content/fbx/hamlet_T2");
            m_cSettlementTextures[SettlementSizes.Hamlet][3] = pTexture;
            m_cSettlementModels[SettlementSizes.Village][3] = LoadModel("content/fbx/village_T2");
            m_cSettlementTextures[SettlementSizes.Village][3] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T3_big");
            m_cSettlementModels[SettlementSizes.Town][3] = LoadModel("content/fbx/town_T3");
            m_cSettlementTextures[SettlementSizes.Town][3] = pTexture;
            m_cSettlementModels[SettlementSizes.City][3] = LoadModel("content/fbx/city_T3");
            m_cSettlementTextures[SettlementSizes.City][3] = pTexture;
            m_cSettlementModels[SettlementSizes.Capital][3] = LoadModel("content/fbx/city_T3");
            m_cSettlementTextures[SettlementSizes.Capital][3] = pTexture;
            m_cSettlementModels[SettlementSizes.Fort][3] = LoadModel("content/fbx/fort_T3");
            m_cSettlementTextures[SettlementSizes.Fort][3] = LibContent.Load<Texture2D>("content/dds/Fort_T3");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T4_small");
            m_cSettlementModels[SettlementSizes.Hamlet][4] = LoadModel("content/fbx/hamlet_T4");
            m_cSettlementTextures[SettlementSizes.Hamlet][4] = pTexture;
            m_cSettlementModels[SettlementSizes.Village][4] = LoadModel("content/fbx/village_T4");
            m_cSettlementTextures[SettlementSizes.Village][4] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T4_big");
            m_cSettlementModels[SettlementSizes.Town][4] = LoadModel("content/fbx/town_T4");
            m_cSettlementTextures[SettlementSizes.Town][4] = pTexture;
            m_cSettlementModels[SettlementSizes.City][4] = LoadModel("content/fbx/city_T4");
            m_cSettlementTextures[SettlementSizes.City][4] = pTexture;
            m_cSettlementModels[SettlementSizes.Capital][4] = LoadModel("content/fbx/city_T4");
            m_cSettlementTextures[SettlementSizes.Capital][4] = pTexture;
            m_cSettlementModels[SettlementSizes.Fort][4] = LoadModel("content/fbx/fort_T3");
            m_cSettlementTextures[SettlementSizes.Fort][4] = LibContent.Load<Texture2D>("content/dds/Fort_T3");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T4_small");
            m_cSettlementModels[SettlementSizes.Hamlet][5] = LoadModel("content/fbx/hamlet_T4");
            m_cSettlementTextures[SettlementSizes.Hamlet][5] = pTexture;
            m_cSettlementModels[SettlementSizes.Village][5] = LoadModel("content/fbx/village_T4");
            m_cSettlementTextures[SettlementSizes.Village][5] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T5_big");
            m_cSettlementModels[SettlementSizes.Town][5] = LoadModel("content/fbx/town_T5");
            m_cSettlementTextures[SettlementSizes.Town][5] = pTexture;
            m_cSettlementModels[SettlementSizes.City][5] = LoadModel("content/fbx/city_T5");
            m_cSettlementTextures[SettlementSizes.City][5] = pTexture;
            m_cSettlementModels[SettlementSizes.Capital][5] = LoadModel("content/fbx/city_T5");
            m_cSettlementTextures[SettlementSizes.Capital][5] = pTexture;
            m_cSettlementModels[SettlementSizes.Fort][5] = LoadModel("content/fbx/fort_T5");
            m_cSettlementTextures[SettlementSizes.Fort][5] = LibContent.Load<Texture2D>("content/dds/Fort_T5");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T6_hamlet");
            m_cSettlementModels[SettlementSizes.Hamlet][6] = LoadModel("content/fbx/hamlet_T6");
            m_cSettlementTextures[SettlementSizes.Hamlet][6] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T6_village");
            m_cSettlementModels[SettlementSizes.Village][6] = LoadModel("content/fbx/village_T6");
            m_cSettlementTextures[SettlementSizes.Village][6] = pTexture;
            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T6");
            m_cSettlementModels[SettlementSizes.Town][6] = LoadModel("content/fbx/town_T6");
            m_cSettlementTextures[SettlementSizes.Town][6] = pTexture;
            m_cSettlementModels[SettlementSizes.City][6] = LoadModel("content/fbx/city_T6");
            m_cSettlementTextures[SettlementSizes.City][6] = pTexture;
            m_cSettlementModels[SettlementSizes.Capital][6] = LoadModel("content/fbx/city_T6");
            m_cSettlementTextures[SettlementSizes.Capital][6] = pTexture;
            m_cSettlementModels[SettlementSizes.Fort][6] = LoadModel("content/fbx/fort_T6");
            m_cSettlementTextures[SettlementSizes.Fort][6] = LibContent.Load<Texture2D>("content/dds/Fort_T6");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T7");
            m_cSettlementModels[SettlementSizes.Hamlet][7] = LoadModel("content/fbx/hamlet_T7");
            m_cSettlementTextures[SettlementSizes.Hamlet][7] = pTexture;
            m_cSettlementModels[SettlementSizes.Village][7] = LoadModel("content/fbx/village_T7");
            m_cSettlementTextures[SettlementSizes.Village][7] = pTexture;
            m_cSettlementModels[SettlementSizes.Town][7] = LoadModel("content/fbx/town_T7");
            m_cSettlementTextures[SettlementSizes.Town][7] = pTexture;
            m_cSettlementModels[SettlementSizes.City][7] = LoadModel("content/fbx/city_T7");
            m_cSettlementTextures[SettlementSizes.City][7] = pTexture;
            m_cSettlementModels[SettlementSizes.Capital][7] = LoadModel("content/fbx/city_T7");
            m_cSettlementTextures[SettlementSizes.Capital][7] = pTexture;
            m_cSettlementModels[SettlementSizes.Fort][7] = LoadModel("content/fbx/fort_T7");
            m_cSettlementTextures[SettlementSizes.Fort][7] = LibContent.Load<Texture2D>("content/dds/Fort_T7");

            pTexture = LibContent.Load<Texture2D>("content/dds/Settlements_T8");
            m_cSettlementModels[SettlementSizes.Hamlet][8] = LoadModel("content/fbx/hamlet_T8");
            m_cSettlementTextures[SettlementSizes.Hamlet][8] = pTexture;
            m_cSettlementModels[SettlementSizes.Village][8] = LoadModel("content/fbx/village_T8");
            m_cSettlementTextures[SettlementSizes.Village][8] = pTexture;
            m_cSettlementModels[SettlementSizes.Town][8] = LoadModel("content/fbx/town_T8");
            m_cSettlementTextures[SettlementSizes.Town][8] = pTexture;
            m_cSettlementModels[SettlementSizes.City][8] = LoadModel("content/fbx/city_T8");
            m_cSettlementTextures[SettlementSizes.City][8] = pTexture;
            m_cSettlementModels[SettlementSizes.Capital][8] = LoadModel("content/fbx/city_T8");
            m_cSettlementTextures[SettlementSizes.Capital][8] = pTexture;
            m_cSettlementModels[SettlementSizes.Fort][8] = LoadModel("content/fbx/fort_T7");
            m_cSettlementTextures[SettlementSizes.Fort][8] = LibContent.Load<Texture2D>("content/dds/Fort_T7");
        }

        /// <summary>
        /// m_pMyEffect должен уже быть создан и настроен!
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        private static Model LoadModel(string sPath)
        {
            Model pModel = LibContent.Load<Model>(sPath);
            //foreach (ModelMesh mesh in pModel.Meshes)
            //    foreach (ModelMeshPart meshPart in mesh.MeshParts)
            //        meshPart.Effect = m_pMyEffect.Clone();

            return pModel;
        }

        private static void BindEffectParameters()
        {
            m_pSpecularColor = m_pMyEffect.Parameters["SpecularColor"];

            m_pBlendDistance = m_pMyEffect.Parameters["BlendDistance"];
            m_pBlendWidth = m_pMyEffect.Parameters["BlendWidth"];

            m_pUseCelShading = m_pMyEffect.Parameters["UseCelShading"];

            m_pDirectionalLightDirection = m_pMyEffect.Parameters["DirectionalLightDirection"];
            m_pDirectionalLightIntensity = m_pMyEffect.Parameters["DirectionalLightIntensity"];
            m_pDirectionalLightColor = m_pMyEffect.Parameters["DirectionalLightColor"];

            m_pWorld = m_pMyEffect.Parameters["World"];
            m_pView = m_pMyEffect.Parameters["View"];
            m_pProjection = m_pMyEffect.Parameters["Projection"];
            m_pCameraPosition = m_pMyEffect.Parameters["CameraPosition"];

            m_pModelBoneWorld = m_pMyEffect.Parameters["ModelBoneWorld"];

            m_pWavesTime = m_pMyEffect.Parameters["xTime"];
        }

        private static void LoadTerrainTextures()
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

            waterBumpMap = LibContent.Load<Texture2D>("content/dds/waterbump");
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
        private static DynamicVertexBuffer instanceVertexBuffer;

        /// <summary>
        /// Efficiently draws several copies of a piece of geometry using hardware instancing.
        /// </summary>
        public static void DrawModelHardwareInstancing(Model model, Matrix[] modelBones,
                                         Matrix[] instances, Matrix view, Matrix projection, Vector3 pCamera)
        {
            if (instances.Length == 0)
                return;

            // If we have more instances than room in our vertex buffer, grow it to the neccessary size.
            if ((instanceVertexBuffer == null) ||
                (instances.Length > instanceVertexBuffer.VertexCount))
            {
                if (instanceVertexBuffer != null)
                    instanceVertexBuffer.Dispose();

                instanceVertexBuffer = new DynamicVertexBuffer(m_pGraphicsDevice, instanceVertexDeclaration,
                                                               instances.Length, BufferUsage.WriteOnly);
            }

            // Transfer the latest instance transform matrices into the instanceVertexBuffer.
            instanceVertexBuffer.SetData(instances, 0, instances.Length, SetDataOptions.Discard);

            m_pMyEffect.CurrentTechnique = m_pMyEffect.Techniques["Tree"];

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Tell the GPU to read from both the model vertex buffer plus our instanceVertexBuffer.
                    m_pGraphicsDevice.SetVertexBuffers(
                        new VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset, 0),
                        new VertexBufferBinding(instanceVertexBuffer, 0, 1)
                    );

                    m_pGraphicsDevice.Indices = meshPart.IndexBuffer;

                    m_pModelBoneWorld.SetValue(modelBones[mesh.ParentBone.Index]);

                    // Draw all the instance copies in a single call.
                    foreach (EffectPass pass in m_pMyEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        m_pGraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                                               meshPart.NumVertices, meshPart.StartIndex,
                                                               meshPart.PrimitiveCount, instances.Length);
                    }
                }
            }
        }
    }
}
