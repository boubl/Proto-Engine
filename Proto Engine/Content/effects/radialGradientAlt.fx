#if OPENGL
	//#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float intensity;

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 pixel = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    float test = intensity;
    float distFromCenter = distance(float2(intensity, intensity), input.TextureCoordinates);
    float4 rColor = lerp(float4(1, 1, 1, 1), float4(1, 1, 1, 1), saturate(distFromCenter));
    return rColor;
}

technique RadialGradient
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};