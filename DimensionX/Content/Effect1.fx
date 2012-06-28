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

float4 FogColor;
float FogDensity;
	
float BlendDistance;
float BlendWidth;

Texture xTextureModel;
sampler TextureSamplerModel = sampler_state { texture = <xTextureModel> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

Texture xTexture0;
sampler TextureSampler0 = sampler_state { texture = <xTexture0> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

Texture xTexture1;
sampler TextureSampler1 = sampler_state { texture = <xTexture1> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

Texture xTexture2;
sampler TextureSampler2 = sampler_state { texture = <xTexture2> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

Texture xTexture3;
sampler TextureSampler3 = sampler_state { texture = <xTexture3> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

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
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input, float3 Normal : NORMAL)
{
    VertexShaderOutput output;
	output.Position3D = input.Position;
	//output.Color = input.Color;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	float3 normal = normalize(mul(Normal, World));
	output.Normal = normal;

	output.View = normalize(float4(CameraPosition,1.0) - worldPosition);

	output.TextureCoords = input.TextureCoords;
	output.TextureWeights = input.TexWeights;
	output.TextureWeights2 = input.TexWeights2;
    
    return output;
}

float4 PixelShaderFunctionPlain(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float4 normal = float4(input.Normal, 1.0);
	float4 diffuse = saturate(dot(-DirectionalLightDirection,normal));
	float4 reflect = normalize(2*diffuse*normal-float4(DirectionalLightDirection,1.0));
	float4 specular = pow(saturate(dot(reflect,input.View)),15);

	float d = length(input.Position3D - CameraPosition);
	float l = exp( - pow( d * FogDensity , 2 ) );
	l = saturate(1 - l);
     
    float blendFactor = clamp((d-BlendDistance)/BlendWidth, 0, 1);
    //float blendFactor2 = clamp((d-BlendDistance/10)/(BlendDistance/10), 0, 1);

	float4 farColor = tex2D(TextureSampler0, input.TextureCoords)*input.TextureWeights.x;
	farColor += tex2D(TextureSampler1, input.TextureCoords)*input.TextureWeights.y;
	farColor += tex2D(TextureSampler2, input.TextureCoords)*input.TextureWeights.z;
	farColor += tex2D(TextureSampler3, input.TextureCoords)*input.TextureWeights.w;
	farColor += tex2D(TextureSampler4, input.TextureCoords)*input.TextureWeights2.x;
	farColor += tex2D(TextureSampler5, input.TextureCoords)*input.TextureWeights2.y;
	farColor += tex2D(TextureSampler6, input.TextureCoords)*input.TextureWeights2.z;
	farColor += tex2D(TextureSampler7, input.TextureCoords)*input.TextureWeights2.w;

    float2 nearTextureCoords = input.TextureCoords*5;
    float4 nearColor = tex2D(TextureSampler0, nearTextureCoords)*input.TextureWeights.x;
    nearColor += tex2D(TextureSampler1, nearTextureCoords)*input.TextureWeights.y;
    nearColor += tex2D(TextureSampler2, nearTextureCoords)*input.TextureWeights.z;
    nearColor += tex2D(TextureSampler3, nearTextureCoords)*input.TextureWeights.w;
    nearColor += tex2D(TextureSampler4, nearTextureCoords)*input.TextureWeights2.x;
    nearColor += tex2D(TextureSampler5, nearTextureCoords)*input.TextureWeights2.y;
    nearColor += tex2D(TextureSampler6, nearTextureCoords)*input.TextureWeights2.z;
    nearColor += tex2D(TextureSampler7, nearTextureCoords)*input.TextureWeights2.w;

    //float2 closeTextureCoords = input.TextureCoords*100;
    //float4 closeColor = tex2D(TextureSampler0, closeTextureCoords)*input.TextureWeights.x;
    //closeColor += tex2D(TextureSampler1, closeTextureCoords)*input.TextureWeights.y;
    //closeColor += tex2D(TextureSampler2, closeTextureCoords)*input.TextureWeights.z;
    //closeColor += tex2D(TextureSampler3, closeTextureCoords)*input.TextureWeights.w;
    //closeColor += tex2D(TextureSampler4, closeTextureCoords)*input.TextureWeights2.x;
    //closeColor += tex2D(TextureSampler5, closeTextureCoords)*input.TextureWeights2.y;
    //closeColor += tex2D(TextureSampler6, closeTextureCoords)*input.TextureWeights2.z;
    //closeColor += tex2D(TextureSampler7, closeTextureCoords)*input.TextureWeights2.w;

    float4 texColor = lerp(nearColor, farColor, blendFactor);
	//texColor = lerp(closeColor, texColor, blendFactor2);
 	 
	return lerp(texColor*AmbientLightColor*AmbientLightIntensity + 
		   texColor*DirectionalLightIntensity*DirectionalLightColor*diffuse + 
		   texColor*SpecularColor*specular, FogColor, l);
}

float4 PixelShaderFunctionPoint(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float4 normal = float4(input.Normal, 1.0);

	float3 lightDirection = normalize(input.Position3D - DirectionalLightDirection);

	float4 diffuse = saturate(dot(-lightDirection,normal));
	float4 reflect = normalize(2*diffuse*normal-float4(DirectionalLightDirection,1.0));
	float4 specular = pow(saturate(dot(reflect,input.View)),15);

	float d = length(input.Position3D - CameraPosition);
	float l = exp( - pow( d * FogDensity , 2 ) );
	l = saturate(1 - l);

    float blendFactor = clamp((d-BlendDistance)/BlendWidth, 0, 1);

	float4 farColor = tex2D(TextureSampler0, input.TextureCoords)*input.TextureWeights.x;
	farColor += tex2D(TextureSampler1, input.TextureCoords)*input.TextureWeights.y;
	farColor += tex2D(TextureSampler2, input.TextureCoords)*input.TextureWeights.z;
	farColor += tex2D(TextureSampler3, input.TextureCoords)*input.TextureWeights.w;
	farColor += tex2D(TextureSampler4, input.TextureCoords)*input.TextureWeights2.x;
	farColor += tex2D(TextureSampler5, input.TextureCoords)*input.TextureWeights2.y;
	farColor += tex2D(TextureSampler6, input.TextureCoords)*input.TextureWeights2.z;
	farColor += tex2D(TextureSampler7, input.TextureCoords)*input.TextureWeights2.w;

    float2 nearTextureCoords = input.TextureCoords*5;
    float4 nearColor = tex2D(TextureSampler0, nearTextureCoords)*input.TextureWeights.x;
    nearColor += tex2D(TextureSampler1, nearTextureCoords)*input.TextureWeights.y;
    nearColor += tex2D(TextureSampler2, nearTextureCoords)*input.TextureWeights.z;
    nearColor += tex2D(TextureSampler3, nearTextureCoords)*input.TextureWeights.w;
    nearColor += tex2D(TextureSampler4, nearTextureCoords)*input.TextureWeights2.x;
    nearColor += tex2D(TextureSampler5, nearTextureCoords)*input.TextureWeights2.y;
    nearColor += tex2D(TextureSampler6, nearTextureCoords)*input.TextureWeights2.z;
    nearColor += tex2D(TextureSampler7, nearTextureCoords)*input.TextureWeights2.w;

    float4 texColor = lerp(nearColor, farColor, blendFactor);

	return lerp(texColor*AmbientLightColor*AmbientLightIntensity + 
		   texColor*DirectionalLightIntensity*DirectionalLightColor*diffuse + 
		   texColor*SpecularColor*specular, FogColor, l);
}

technique Land
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunctionPlain();
    }
}

technique LandRingworld
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunctionPoint();
    }
}

//------- Technique: Model --------
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
};

ModelVertexShaderOutput ModelVS(ModelVertexShaderInput input, float3 Normal : NORMAL)
{
    ModelVertexShaderOutput output;
	//output.Position3D = input.Position;
	//output.Color = input.Color;

    float4 worldPosition = mul(input.Position, World);
	output.Position3D = worldPosition;
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	float3 normal = normalize(mul(Normal, World));
	output.Normal = normal;

	output.View = normalize(float4(CameraPosition,1.0) - worldPosition);

	output.TextureCoords = input.TextureCoords;
    
    return output;
}

float alphaReference = .8f;

float4 TreePSPlain(ModelVertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float4 normal = float4(input.Normal, 1.0);
	float4 diffuse = 1;//saturate(dot(-DirectionalLightDirection,normal));
	//float4 diffuse = float4(0.66, 0.66, 0.66, 1.0) + saturate(dot(-DirectionalLightDirection,normal))/3;
	float4 reflect = normalize(2*diffuse*normal-float4(DirectionalLightDirection,1.0));
	float4 specular = pow(saturate(dot(reflect,input.View)),15);

	float d = length(input.Position3D - CameraPosition);
	float l = exp( - pow( d * FogDensity , 2 ) );
	l = saturate(1 - l);
     
    float blendFactor = clamp((d-BlendDistance)/BlendWidth, 0, 1);

	float4 texColor = tex2D(TextureSamplerModel, input.TextureCoords);

	float4 output = texColor;

	clip(output.a - alphaReference); 	 

	output.rgb = lerp(texColor.rgb*AmbientLightColor.rgb*AmbientLightIntensity + 
		   texColor.rgb*DirectionalLightIntensity*DirectionalLightColor.rgb*diffuse + 
		   texColor.rgb*SpecularColor.rgb*specular, FogColor.rgb, l)*output.a;

	//if(output.r == 1 && output.g == 1 && output.b == 1 && output.a == 1)
	//	output = 0;
	//output = 0;

	return output;
}

float4 TreePSPoint(ModelVertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float4 normal = float4(input.Normal, 1.0);

	float3 lightDirection = normalize(input.Position3D - DirectionalLightDirection);

	float4 diffuse = saturate(dot(-lightDirection,normal));
	float4 reflect = normalize(2*diffuse*normal-float4(DirectionalLightDirection,1.0));
	float4 specular = pow(saturate(dot(reflect,input.View)),15);

	float d = length(input.Position3D - CameraPosition);
	float l = exp( - pow( d * FogDensity , 2 ) );
	l = saturate(1 - l);

    float blendFactor = clamp((d-BlendDistance)/BlendWidth, 0, 1);

	float4 texColor = tex2D(TextureSamplerModel, input.TextureCoords);

	return lerp(texColor*AmbientLightColor*AmbientLightIntensity + 
		   texColor*DirectionalLightIntensity*DirectionalLightColor*diffuse + 
		   texColor*SpecularColor*specular, FogColor, l);
}

float4 ModelPSPlain(ModelVertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float4 normal = float4(input.Normal, 1.0);
	float4 diffuse = float4(0.66, 0.66, 0.66, 1.0) + saturate(dot(-DirectionalLightDirection,normal))/3;
	//float4 reflect = normalize(2*diffuse*normal-float4(DirectionalLightDirection,1.0));
	//float4 specular = pow(saturate(dot(reflect,input.View)),15);

	float d = length(input.Position3D - CameraPosition);
	float l = exp( - pow( d * FogDensity , 2 ) );
	l = saturate(1 - l);
     
	float4 texColor = tex2D(TextureSamplerModel, input.TextureCoords);

	return lerp(texColor*AmbientLightColor*AmbientLightIntensity + 
		   texColor*DirectionalLightIntensity*DirectionalLightColor*diffuse, FogColor, l);//*output.a;
}

float4 ModelPSPoint(ModelVertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float4 normal = float4(input.Normal, 1.0);

	float3 lightDirection = normalize(input.Position3D - DirectionalLightDirection);

	float4 diffuse = saturate(dot(-lightDirection,normal));
	float4 reflect = normalize(2*diffuse*normal-float4(DirectionalLightDirection,1.0));
	float4 specular = pow(saturate(dot(reflect,input.View)),15);

	float d = length(input.Position3D - CameraPosition);
	float l = exp( - pow( d * FogDensity , 2 ) );
	l = saturate(1 - l);

    float blendFactor = clamp((d-BlendDistance)/BlendWidth, 0, 1);

	float4 texColor = tex2D(TextureSamplerModel, input.TextureCoords);

	return lerp(texColor*AmbientLightColor*AmbientLightIntensity + 
		   texColor*DirectionalLightIntensity*DirectionalLightColor*diffuse + 
		   texColor*SpecularColor*specular, FogColor, l);
}

technique Tree
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 ModelVS();
        PixelShader = compile ps_2_0 TreePSPlain();
    }
}

technique TreeRingworld
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 ModelVS();
        PixelShader = compile ps_2_0 TreePSPoint();
    }
}

technique Model
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 ModelVS();
        PixelShader = compile ps_2_0 ModelPSPlain();
    }
}

technique ModelRingworld
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 ModelVS();
        PixelShader = compile ps_2_0 ModelPSPoint();
    }
}

//------- Technique: Water --------
struct WVertexToPixel
{
    float4 Position                 : POSITION;
    //float4 ReflectionMapSamplingPos    : TEXCOORD1;
    float4 RefractionMapSamplingPos : TEXCOORD3;
    float4 Position3D                : TEXCOORD4;
};

WVertexToPixel WaterVS(float4 inPos : POSITION, float2 inTex: TEXCOORD)
{    
    WVertexToPixel Output = (WVertexToPixel)0;

    //float4x4 preReflectionViewProjection = mul (xReflectionView, xProjection);
    //float4x4 preWorldReflectionViewProjection = mul (xWorld, preReflectionViewProjection);

	float4 worldPosition = mul(inPos, World);
    float4 viewPosition = mul(worldPosition, View);
    Output.Position = mul(viewPosition, Projection);

    //Output.ReflectionMapSamplingPos = mul(inPos, preWorldReflectionViewProjection);
     
	Output.RefractionMapSamplingPos = mul(viewPosition, Projection);
    Output.Position3D = mul(inPos, World);

    return Output;
}

float4 WaterPS(WVertexToPixel PSIn) : COLOR0
{
    //float4 bumpColor = tex2D(WaterBumpMapSampler, PSIn.BumpMapSamplingPos);
    float2 perturbation = 1;//xWaveHeight*(bumpColor.rg - 0.5f)*2.0f;
	
    //float2 ProjectedTexCoords;
    //ProjectedTexCoords.x = PSIn.ReflectionMapSamplingPos.x/PSIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;
    //ProjectedTexCoords.y = -PSIn.ReflectionMapSamplingPos.y/PSIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;        
    //float2 perturbatedTexCoords = ProjectedTexCoords + perturbation;
    //float4 reflectiveColor = tex2D(ReflectionSampler, perturbatedTexCoords);   

	float4 reflectiveColor = float4(0.3f, 0.3f, 0.5f, 0.5f);

    float2 ProjectedRefrTexCoords;
    ProjectedRefrTexCoords.x = -PSIn.RefractionMapSamplingPos.x/PSIn.RefractionMapSamplingPos.w/2.0f + 0.5f;
    ProjectedRefrTexCoords.y = PSIn.RefractionMapSamplingPos.y/PSIn.RefractionMapSamplingPos.w/2.0f + 0.5f;    
    float2 perturbatedRefrTexCoords = ProjectedRefrTexCoords + perturbation;    
    float4 refractiveColor = tex2D(RefractionSampler, perturbatedRefrTexCoords);
     
    float3 eyeVector = normalize(CameraPosition - PSIn.Position3D);
    float3 normalVector = float3(0,1,0);
    float fresnelTerm = dot(eyeVector, normalVector);    
    float4 combinedColor = lerp(reflectiveColor, refractiveColor, fresnelTerm);
     
    //float4 dullColor = float4(0.3f, 0.3f, 0.5f, 1.0f);
     
//    Output.Color = lerp(combinedColor, dullColor, 0.2f);
	
	float d = length(PSIn.Position3D - CameraPosition);
	float l = exp( - pow( d * FogDensity , 2 ) );
	l = saturate(1 - l);
		 
	return lerp(lerp(combinedColor, FogColor, 0.2f), FogColor, l);
}

technique Water
{
    pass Pass0
    {
        VertexShader = compile vs_1_1 WaterVS();
        PixelShader = compile ps_2_0 WaterPS();
    }
}