float4x4 World;
float4x4 View;
float4x4 Projection;

float3 CameraPosition;

float4 AmbientLightColor;
float AmbientLightIntensity;

float3 DirectionalLightDirection;
float4 DirectionalLightColor;
float DirectionalLightIntensity;

float4 SpecularColor;

float4 FogColor = float4(0, 0, 0, 0);
float FogDensity = 0.025;
float FogHeight = 10;

float PlanetRadius = 150;

//int FogMode = 0;
bool FogModePlain = true;
bool FogModeRing = false;
bool FogModeSphere = false;
	
float BlendDistance;
float BlendWidth;

bool GridFog;
float4 GridColor1;
float4 GridColor2;
float4 GridColor3;
float4 GridColor4;
float4 GridColor5;
float4 GridColor6;

Texture xTextureModel;
sampler TextureSamplerModel = sampler_state { texture = <xTextureModel> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

Texture xTexture0;
sampler TextureSampler0 = sampler_state { texture = <xTexture0> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

Texture xTexture1;
sampler TextureSampler1 = sampler_state { texture = <xTexture1> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

Texture xTexture2;
sampler TextureSampler2 = sampler_state { texture = <xTexture2> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

Texture xTexture3;
sampler TextureSampler3 = sampler_state { texture = <xTexture3> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

Texture xTexture4;
sampler TextureSampler4 = sampler_state { texture = <xTexture4> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

Texture xTexture5;
sampler TextureSampler5 = sampler_state { texture = <xTexture5> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

Texture xTexture6;
sampler TextureSampler6 = sampler_state { texture = <xTexture6> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

Texture xTexture7;
sampler TextureSampler7 = sampler_state { texture = <xTexture7> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

Texture xRefractionMap;
sampler RefractionSampler = sampler_state { texture = <xRefractionMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

Texture BumpMap0; 
sampler BumpMap0Sampler = sampler_state 
{ 
   texture = <BumpMap0>; 
   minfilter = LINEAR; 
   magfilter = LINEAR; 
   mipfilter = LINEAR; 
   AddressU = mirror; 
   AddressV = mirror; 
}; 

bool UseCelShading = true;

texture CelMap;
/* Cel Shader effect map
 * Mapping to the cel shader texture is what
 * gives us that classic cel shaded effect
 */
sampler2D CelMapSampler = sampler_state { Texture = <CelMap>; MinFilter = LINEAR; MagFilter = LINEAR; MipFilter = LINEAR; AddressU = mirror; AddressV = mirror; };

float DistFogPlain(float3 Position)
{
	//return 1;

	float d = length(Position - CameraPosition);
	//заморачиваемся только, если камера НАД границей атмосферы
	if(CameraPosition.y > FogHeight && CameraPosition.y != Position.y)
	{
		d = (FogHeight - Position.y) * d / (CameraPosition.y - Position.y);
	}

	return d;
}

float DistFogRingworld(float3 Position)
{
	//return 1;

	float HCam = length(CameraPosition.xy);

	float d = length(Position - CameraPosition);
	//заморачиваемся только, если камера НАД границей атмосферы
	//на кольце атмосфера расположена на внутренней стороне кольца, т.е. "выше" - это значит ближе к центру кольца
	if(HCam < FogHeight)
	{
		float HPos = length(Position.xy);
		if(HPos != HCam)
			d = (HPos - FogHeight) * d / (HPos - HCam);
	}

	return d;
}

float DistFogSphere(float3 Position)
{
	//return 1;

	float HCam = length(CameraPosition);

	float d = length(Position - CameraPosition);
	//заморачиваемся только, если камера НАД границей атмосферы
	if(HCam > FogHeight)
	{
		float HPos = length(Position);
		if(HPos != HCam)
			d = (FogHeight - HPos) * d / (HCam - HPos);
	}

	return d;
}

float4 ApplyFog(float4 Color, float3 Position, float koeff)
{
	float d = 0;

	if(FogModePlain)
		d = DistFogPlain(Position);

	if(FogModeRing)
		d = DistFogRingworld(Position);

	if(FogModeSphere)
		d = DistFogSphere(Position);

	float l = exp( - pow( d * d * FogDensity * koeff, 2 ) );
	l = saturate(1 - l);

	return lerp(Color, FogColor, l);
}

float4 GetLight(float3 inNormal, float3 inView)
{
	float4 normal = float4(inNormal, 1.0);

	float diff = saturate(dot(-DirectionalLightDirection,normal));
	float4 diffuse = diff;
	
	if(UseCelShading)
	{
		// Look up the cel shading light color
		float2 celTexCoord = float2(diff, 0.0f);
		diffuse = tex2D(CelMapSampler, celTexCoord);
	}	
	float4 reflect = normalize(2*diffuse*normal-float4(DirectionalLightDirection,1.0));
	float4 specular = pow(saturate(dot(reflect,inView)),15);

	float4 light = AmbientLightColor*AmbientLightIntensity + 
		   DirectionalLightIntensity*DirectionalLightColor*diffuse + 
		   SpecularColor*specular; 

    return float4(light.xyz, 1);
}

//------- Technique: Land --------
struct VertexShaderInput
{
    float4 Position : POSITION0;
    //float4 Color        : COLOR0;
    float2 TextureCoords: TEXCOORD0;
	float4 TexWeights: TEXCOORD1;
	float4 TexWeights2: TEXCOORD2;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    //float4 Color        : COLOR0;
    float3 Position3D    : TEXCOORD0;
    float3 Normal : TEXCOORD1;
    float3 View : TEXCOORD2;
    float2 TextureCoords: TEXCOORD3;
    float4 TextureWeights    : TEXCOORD4;
    float4 TextureWeights2    : TEXCOORD5;
    float3 Tangent : TEXCOORD6;
    float3 Binormal : TEXCOORD7;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input, float3 Normal : NORMAL, float3 Tangent : TANGENT)
{
    VertexShaderOutput output;
	output.Position3D = input.Position;
	//output.Color = input.Color;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	float3 normal = normalize(mul(Normal, World));
	output.Normal = normal;

	float3 tangent = normalize(mul(Tangent, World));
	output.Tangent = tangent;

	float3 binormal = normalize(mul(cross(Tangent,Normal), World));
	output.Binormal = binormal;
   
    // Calculate tangent space. 
    //float4x4 worldToTangentSpace; 
    //worldToTangentSpace[0] = mul(Tangent,World); 
    //worldToTangentSpace[1] = mul(cross(Tangent,Normal),World); 
    //worldToTangentSpace[2] = mul(Normal,World); 
    //worldToTangentSpace[3] = float4(0, 0, 0, 1); 
    
    //output.View = normalize(mul(worldToTangentSpace,float4(CameraPosition,1.0) - worldPosition));    

	output.View = normalize(float4(CameraPosition,1.0) - worldPosition);
	
	output.TextureCoords = input.TextureCoords;
	output.TextureWeights = input.TexWeights;
	output.TextureWeights2 = input.TexWeights2;
    
    return output;
}

float4 GetLandTexture(float2 TextureCoords, float4 TextureWeights, float4 TextureWeights2, float d)
{
    float blendFactor = clamp((d-BlendDistance)/BlendWidth, 0, 1);
    float blendFactor2 = clamp((d-BlendDistance/10)/(BlendDistance/5), 0, 1);

	float4 texColor = float4(0, 0, 0, 0);
	if(blendFactor >= 0.2)
	{
		float4 farColor = tex2D(TextureSampler0, TextureCoords)*TextureWeights.x;
		farColor += tex2D(TextureSampler1, TextureCoords)*TextureWeights.y;
		farColor += tex2D(TextureSampler2, TextureCoords)*TextureWeights.z;
		farColor += tex2D(TextureSampler3, TextureCoords)*TextureWeights.w;
		farColor += tex2D(TextureSampler4, TextureCoords)*TextureWeights2.x;
		farColor += tex2D(TextureSampler5, TextureCoords)*TextureWeights2.y;
		farColor += tex2D(TextureSampler6, TextureCoords)*TextureWeights2.z;
		farColor += tex2D(TextureSampler7, TextureCoords)*TextureWeights2.w;
	
		texColor = farColor;
	}

	if(blendFactor < 1 && blendFactor2 >= 0.1)
	{
		float2 nearTextureCoords = TextureCoords*5;
		float4 nearColor = tex2D(TextureSampler0, nearTextureCoords)*TextureWeights.x;
		nearColor += tex2D(TextureSampler1, nearTextureCoords)*TextureWeights.y;
		nearColor += tex2D(TextureSampler2, nearTextureCoords)*TextureWeights.z;
		nearColor += tex2D(TextureSampler3, nearTextureCoords)*TextureWeights.w;
		nearColor += tex2D(TextureSampler4, nearTextureCoords)*TextureWeights2.x;
		nearColor += tex2D(TextureSampler5, nearTextureCoords)*TextureWeights2.y;
		nearColor += tex2D(TextureSampler6, nearTextureCoords)*TextureWeights2.z;
		nearColor += tex2D(TextureSampler7, nearTextureCoords)*TextureWeights2.w;

		if(blendFactor < 0.2)
			texColor = nearColor;
		else
			texColor = lerp(nearColor, texColor, blendFactor);
	}

	if(blendFactor2 < 1)
	{
		float2 closeTextureCoords = TextureCoords*15;
		float4 closeColor = tex2D(TextureSampler0, closeTextureCoords)*TextureWeights.x;
		closeColor += tex2D(TextureSampler1, closeTextureCoords)*TextureWeights.y;
		closeColor += tex2D(TextureSampler2, closeTextureCoords)*TextureWeights.z;
		closeColor += tex2D(TextureSampler3, closeTextureCoords)*TextureWeights.w;
		closeColor += tex2D(TextureSampler4, closeTextureCoords)*TextureWeights2.x;
		closeColor += tex2D(TextureSampler5, closeTextureCoords)*TextureWeights2.y;
		closeColor += tex2D(TextureSampler6, closeTextureCoords)*TextureWeights2.z;
		closeColor += tex2D(TextureSampler7, closeTextureCoords)*TextureWeights2.w;

		if(blendFactor2 < 0.1)
			texColor = closeColor;
		else
			texColor = lerp(closeColor, texColor, blendFactor2);
	}

	return texColor;
}

float4 PixelShaderFunctionPlain(VertexShaderOutput input) : COLOR0
{
    // Calculate the normal, including the information in the bump map
    //float3 bump = tex2D(BumpMap0Sampler, input.TextureCoords*5);
    //float3 bumpNormal = input.Normal + (bump.x * input.Tangent + bump.y * input.Binormal);
    //bumpNormal = normalize(bumpNormal);
	//bump = 2 * bump - 1.0;  

	//float4 normal = float4(normalize(input.Normal + bump/5), 1.0);
	//float4 normal = float4(input.Normal, 1.0);
	//float4 diffuse = saturate(dot(-DirectionalLightDirection,normal));
	//float4 reflect = normalize(2*diffuse*normal-float4(DirectionalLightDirection,1.0));
	//float4 specular = pow(saturate(dot(reflect,input.View)),15);

    float4 light = GetLight(input.Normal, input.View);

	float d = length(input.Position3D - CameraPosition);

	//==============================================================================================================================
	// Determine the blend weights for the 3 planar projections.  
	// N_orig is the vertex-interpolated normal vector.  
	float3 blend_weights = abs( input.Normal.xyz );   // Tighten up the blending zone:  
	blend_weights = (blend_weights - 0.2) * 7;  
	blend_weights = max(blend_weights, 0);      // Force weights to sum to 1.0 (very important!)  
	blend_weights /= (blend_weights.x + blend_weights.y + blend_weights.z ).xxx;   
	// Now determine a color value and bump vector for each of the 3  
	// projections, blend them, and store blended results in these two  
	// vectors:  
	float4 blended_color; // .w hold spec value  
	float tex_scale = 0.05;
	//float3 blended_bump_vec;  
	{  
		// Compute the UV coords for each of the 3 planar projections.  
		// tex_scale (default ~ 1.0) determines how big the textures appear.  
		float2 coord1 = input.Position3D.yz * tex_scale;  
		float2 coord2 = input.Position3D.zx * tex_scale;  
		float2 coord3 = input.Position3D.xy * tex_scale;  
		// This is where you would apply conditional displacement mapping.  
		//if (blend_weights.x > 0) coord1 = . . .  
		//if (blend_weights.y > 0) coord2 = . . .  
		//if (blend_weights.z > 0) coord3 = . . .  
		// Sample color maps for each projection, at those UV coords.  
		float4 col1 = float4(0, 0, 0, 0);
		float4 col2 = float4(0, 0, 0, 0);
		float4 col3 = float4(0, 0, 0, 0);  

		if(blend_weights.x != 0)
			col1 = GetLandTexture(coord1, input.TextureWeights, input.TextureWeights2, d);  
		if(blend_weights.y != 0)
			col2 = GetLandTexture(coord2, input.TextureWeights, input.TextureWeights2, d); 
		if(blend_weights.z != 0)
			col3 = GetLandTexture(coord3, input.TextureWeights, input.TextureWeights2, d);   
		// Sample bump maps too, and generate bump vectors.  
		// (Note: this uses an oversimplified tangent basis.)  
		//float2 bumpFetch1 = bumpTex1.Sample(coord1).xy - 0.5;  
		//float2 bumpFetch2 = bumpTex2.Sample(coord2).xy - 0.5;  
		//float2 bumpFetch3 = bumpTex3.Sample(coord3).xy - 0.5;  
		//float3 bump1 = float3(0, bumpFetch1.x, bumpFetch1.y);  
		//float3 bump2 = float3(bumpFetch2.y, 0, bumpFetch2.x);  
		//float3 bump3 = float3(bumpFetch3.x, bumpFetch3.y, 0);  
		 // Finally, blend the results of the 3 planar projections.  
		blended_color = col1.xyzw * blend_weights.xxxx +  
						col2.xyzw * blend_weights.yyyy +  
						col3.xyzw * blend_weights.zzzz;  
		//blended_bump_vec = bump1.xyz * blend_weights.xxx +  
		//				   bump2.xyz * blend_weights.yyy +  
		//				   bump3.xyz * blend_weights.zzz;  
	}  
	// Apply bump vector to vertex-interpolated normal vector.  
	//float3 N_for_lighting = normalize(N_orig + blended_bump);
	//==============================================================================================================================
    float4 texColor = blended_color;//GetLandTexture(input.TextureCoords, input.TextureWeights, input.TextureWeights2, d);
 	 
	return ApplyFog(texColor*light, input.Position3D, 1);
}

technique Land
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunctionPlain();
    }
}

//------- Technique: Grid --------
struct GridVertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color        : COLOR0;
};

struct GridVertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color        : COLOR0;
    float3 Position3D    : TEXCOORD0;
    float3 View : TEXCOORD2;
};

GridVertexShaderOutput GridVSCommon(GridVertexShaderInput input, float3 Normal, float GridLayer)
{
    GridVertexShaderOutput output;
	output.Position3D = input.Position;
	output.Color = input.Color;

    //float4 fakeNormal = float4(0, 0, 0, 0);

	float4 layerPos = input.Position + float4(Normal, 0)*GridLayer;

    float4 worldPosition = mul(layerPos, World);
    float3 worldNormal = mul(Normal, World);
	float4 viewPosition = mul(worldPosition, View);
    
	output.Position = mul(viewPosition, Projection);

	output.View = normalize(float4(CameraPosition,1.0) - worldPosition);

    return output;
}

GridVertexShaderOutput GridVS1(GridVertexShaderInput input, float3 Normal : NORMAL)
{
	return GridVSCommon(input, Normal, 0.001);
}

GridVertexShaderOutput GridVS2(GridVertexShaderInput input, float3 Normal : NORMAL)
{
	return GridVSCommon(input, Normal, 0.002);
}

GridVertexShaderOutput GridVS3(GridVertexShaderInput input, float3 Normal : NORMAL)
{
	return GridVSCommon(input, Normal, 0.003);
}

GridVertexShaderOutput GridVS4(GridVertexShaderInput input, float3 Normal : NORMAL)
{
	return GridVSCommon(input, Normal, 0.004);
}

GridVertexShaderOutput GridVS5(GridVertexShaderInput input, float3 Normal : NORMAL)
{
	return GridVSCommon(input, Normal, 0.005);
}

GridVertexShaderOutput GridVS6(GridVertexShaderInput input, float3 Normal : NORMAL)
{
	return GridVSCommon(input, Normal, 0.006);
}

GridVertexShaderOutput GridVS7(GridVertexShaderInput input, float3 Normal : NORMAL)
{
	return GridVSCommon(input, Normal, 0.0005);
}

float4 GridPSCommon(GridVertexShaderOutput input, float4 GridColor)
{
    // TODO: add your pixel shader code here.
	if(GridFog) 
		return ApplyFog(GridColor, input.Position3D, 1);
	else
		return GridColor;
}

float4 GridPS1(GridVertexShaderOutput input) : COLOR0
{
	return GridPSCommon(input, GridColor1);
}

float4 GridPS2(GridVertexShaderOutput input) : COLOR0
{
	return GridPSCommon(input, GridColor2);
}

float4 GridPS3(GridVertexShaderOutput input) : COLOR0
{
	return GridPSCommon(input, GridColor3);
}

float4 GridPS4(GridVertexShaderOutput input) : COLOR0
{
	return GridPSCommon(input, GridColor4);
}

float4 GridPS5(GridVertexShaderOutput input) : COLOR0
{
	return GridPSCommon(input, GridColor5);
}

float4 GridPS6(GridVertexShaderOutput input) : COLOR0
{
	return GridPSCommon(input, GridColor6);
}

float4 GridPS7(GridVertexShaderOutput input) : COLOR0
{
	return GridPSCommon(input, input.Color);
}

technique Grid
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_3_0 GridVS1();
        PixelShader = compile ps_3_0 GridPS1();
    }
    pass Pass2
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_3_0 GridVS2();
        PixelShader = compile ps_3_0 GridPS2();
    }
    pass Pass3
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_3_0 GridVS3();
        PixelShader = compile ps_3_0 GridPS3();
    }
    pass Pass4
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_3_0 GridVS4();
        PixelShader = compile ps_3_0 GridPS4();
    }
    pass Pass5
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_3_0 GridVS5();
        PixelShader = compile ps_3_0 GridPS5();
    }
    pass Pass6
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_3_0 GridVS6();
        PixelShader = compile ps_3_0 GridPS6();
    }

    pass Pass7
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_3_0 GridVS7();
        PixelShader = compile ps_3_0 GridPS7();
    }
}

//------- Technique: Model --------
struct InstancedModelVertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TextureCoordinate : TEXCOORD0;
};


struct InstancedModelVertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinate : TEXCOORD0;
};

struct ModelVertexShaderInput
{
    float4 Position : POSITION0;
    //float4 Color        : COLOR0;
    float2 TextureCoords: TEXCOORD0;
};

struct ModelVertexShaderOutput
{
    float4 Position : POSITION0;
    float3 Position3D    : TEXCOORD0;
    float3 Normal : TEXCOORD1;
    float3 View : TEXCOORD2;
    float2 TextureCoords: TEXCOORD3;
	//float Fog : FOG0;
};

// Vertex shader helper function shared between the two techniques.
ModelVertexShaderOutput InstancedModelVSCommon(InstancedModelVertexShaderInput input, float4x4 instanceTransform)
{
    ModelVertexShaderOutput output;

    // Apply the world and camera matrices to compute the output position.
    float4 worldPosition = mul(input.Position, instanceTransform);
	output.Position3D = worldPosition;
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    // Compute lighting, using a simple Lambert model.
    float3 worldNormal = mul(input.Normal, instanceTransform);
	output.Normal = worldNormal;

	output.View = normalize(float4(CameraPosition,1.0) - worldPosition);
    
	//float d = length(worldPosition - CameraPosition);
	//float l = exp( - pow( d * FogDensity , 2 ) );
	//output.Fog = saturate(1 - l);

    // Copy across the input texture coordinate.
    output.TextureCoords = input.TextureCoordinate;

    return output;
}

// Hardware instancing reads the per-instance world transform from a secondary vertex stream.
ModelVertexShaderOutput HardwareInstancingVertexShader(InstancedModelVertexShaderInput input,
                                                  float4x4 instanceTransform : BLENDWEIGHT)
{
    return InstancedModelVSCommon(input, mul(World, transpose(instanceTransform)));
}

ModelVertexShaderOutput ModelVS(ModelVertexShaderInput input, float3 Normal : NORMAL)
{
    ModelVertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
	output.Position3D = worldPosition;
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	float3 normal = normalize(mul(Normal, World));
	output.Normal = normal;

	output.View = normalize(float4(CameraPosition,1.0) - worldPosition);

	output.TextureCoords = input.TextureCoords;
    
	//output.Fog = 0;

    return output;
}

float alphaReference = .9f;

float4 TreePSPlain(ModelVertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float4 normal = float4(input.Normal, 1.0);
	float4 diffuse = 1;//saturate(dot(-DirectionalLightDirection,normal));
	//float4 diffuse = float4(0.66, 0.66, 0.66, 1.0) + saturate(dot(-DirectionalLightDirection,normal))/3;
	float4 reflect = normalize(2*diffuse*normal-float4(DirectionalLightDirection,1.0));
	float4 specular = pow(saturate(dot(reflect,input.View)),15);

	//float l = d / 20;
	 
	float4 texColor = tex2D(TextureSamplerModel, input.TextureCoords);

	float4 output = texColor;

	clip(output.a < alphaReference ? -1:1); 	 

	float3 lightColor = texColor.rgb*AmbientLightColor.rgb*AmbientLightIntensity + 
		   texColor.rgb*DirectionalLightIntensity*DirectionalLightColor.rgb*diffuse + 
		   texColor.rgb*SpecularColor.rgb*specular;

	output.rgb = ApplyFog(float4(lightColor, FogColor.a), input.Position3D, 1.2).rgb*output.a;

	return output;
}

float4 ModelPSPlain(ModelVertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float4 normal = float4(input.Normal, 1.0);
	float4 diffuse = float4(0.66, 0.66, 0.66, 1.0) + saturate(dot(-DirectionalLightDirection,normal))/3;
	//float4 reflect = normalize(2*diffuse*normal-float4(DirectionalLightDirection,1.0));
	//float4 specular = pow(saturate(dot(reflect,input.View)),15);

	float4 texColor = tex2D(TextureSamplerModel, input.TextureCoords);

	return ApplyFog(texColor*AmbientLightColor*AmbientLightIntensity + 
		   texColor*DirectionalLightIntensity*DirectionalLightColor*diffuse, input.Position3D, 1);//*output.a;
}

technique Tree
{
    pass Pass1
    {
	    VertexShader = compile vs_3_0 HardwareInstancingVertexShader();
        //PixelShader = compile ps_3_0 InstancedModelPS();
        PixelShader = compile ps_3_0 TreePSPlain();
    }
}

technique Model
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_3_0 ModelVS();
        PixelShader = compile ps_3_0 ModelPSPlain();
    }
}

//------- Technique: Water --------
struct WVertexToPixel
{
    float4 Position                 : POSITION;
    float3 Normal : TEXCOORD1;
    //float4 ReflectionMapSamplingPos    : TEXCOORD2;
    float4 RefractionMapSamplingPos : TEXCOORD3;
    float4 Position3D                : TEXCOORD4;
    float3 View : TEXCOORD5;
};

WVertexToPixel WaterVS(float4 inPos : POSITION)
{    
    WVertexToPixel Output = (WVertexToPixel)0;

    //float4x4 preReflectionViewProjection = mul (xReflectionView, xProjection);
    //float4x4 preWorldReflectionViewProjection = mul (xWorld, preReflectionViewProjection);

	float4 worldPosition = mul(inPos, World);
    float4 viewPosition = mul(worldPosition, View);
    Output.Position = mul(viewPosition, Projection);
	Output.Normal = normalize(inPos);

    //Output.ReflectionMapSamplingPos = mul(inPos, preWorldReflectionViewProjection);
     
	Output.RefractionMapSamplingPos = mul(viewPosition, Projection);
    Output.Position3D = mul(inPos, World);
	
	Output.View = normalize(float4(CameraPosition,1.0) - worldPosition);

    return Output;
}

float4 WaterPS(WVertexToPixel PSIn) : COLOR0
{
    //float4 bumpColor = tex2D(WaterBumpMapSampler, PSIn.BumpMapSamplingPos);
    float2 perturbation = (float2)1;//xWaveHeight*(bumpColor.rg - 0.5f)*2.0f;
	
    //float2 ProjectedTexCoords;
    //ProjectedTexCoords.x = PSIn.ReflectionMapSamplingPos.x/PSIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;
    //ProjectedTexCoords.y = -PSIn.ReflectionMapSamplingPos.y/PSIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;        
    //float2 perturbatedTexCoords = ProjectedTexCoords + perturbation;
    //float4 reflectiveColor = tex2D(ReflectionSampler, perturbatedTexCoords);   

	//float4 normal = float4(PSIn.Normal, 1.0);
	//float4 diffuse = saturate(dot(-DirectionalLightDirection,normal));
	//float4 reflect = normalize(2*diffuse*normal-float4(DirectionalLightDirection,1.0));
	//float4 specular = pow(saturate(dot(reflect,PSIn.View)),15);

    //float4 light = GetLight(input.Normal, input.View);
	
	float4 reflectiveColor = float4(0.1f, 0.1f, 0.3f, 1.0f);
	//reflectiveColor = reflectiveColor*light;

    float2 ProjectedRefrTexCoords = (float2)0;
    ProjectedRefrTexCoords.x = -PSIn.RefractionMapSamplingPos.x/PSIn.RefractionMapSamplingPos.w/2.0f + 0.5f;
    ProjectedRefrTexCoords.y = PSIn.RefractionMapSamplingPos.y/PSIn.RefractionMapSamplingPos.w/2.0f + 0.5f;    
    float2 perturbatedRefrTexCoords = ProjectedRefrTexCoords + perturbation;    
    float4 refractiveColor = tex2D(RefractionSampler, perturbatedRefrTexCoords);
     
    float3 eyeVector = normalize(CameraPosition - PSIn.Position3D);
    //float3 normalVector = float3(0,1,0);
    float fresnelTerm = dot(eyeVector, PSIn.Normal);    
    float4 combinedColor = lerp(reflectiveColor, refractiveColor, fresnelTerm*fresnelTerm);//*fresnelTerm);
     
    //float4 dullColor = float4(0.3f, 0.3f, 0.5f, 1.0f);
     
//    Output.Color = lerp(combinedColor, dullColor, 0.2f);
	
	return ApplyFog(lerp(combinedColor, FogColor, 0.2f), PSIn.Position3D, 1);
}

technique Water
{
    pass Pass0
    {
        VertexShader = compile vs_3_0 WaterVS();
        PixelShader = compile ps_3_0 WaterPS();
    }
}