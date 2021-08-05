#if OPENGL
	//#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4 color;
float intensity;
float2 position;
float radius;

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
    
    float2 p = (input.Position.xy - position) / radius;
    float r = sqrt(dot(p, p));
    if (r < 1.0)
    {
        return lerp(color, pixel, (r - intensity) / (1 - intensity) );
    }
    else
    {
        return pixel;
    }
}

technique RadialGradient
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};