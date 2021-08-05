#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

float4 palette[4] : COLOR;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 currentPixel = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	float distance = 500;
	float4 resultPixel;
	for (int i = 0; i < 4; i++)
	{
		float red = pow(palette[i].r * 255 - currentPixel.r * 255, 2);
		float green = pow(palette[i].g * 255 - currentPixel.g * 255, 2);
		float blue = pow(palette[i].b * 255 - currentPixel.b * 255, 2);
		float alpha = pow(palette[i].a * 255 - currentPixel.a * 255, 2);

		float temp = sqrt(blue + green + red + alpha);

		if (temp == 0.0)
		{
			resultPixel = float4(palette[i].r / 255, palette[i].g / 255, palette[i].b / 255, palette[i].a / 255);
			resultPixel = palette[i];
			break;
		}
		else if (temp < distance)
		{
			resultPixel = float4(palette[i].r / 255, palette[i].g / 255, palette[i].b / 255, palette[i].a / 255);
			resultPixel = palette[i];
			distance = temp;
		}
	}
	return resultPixel;
}

technique PaletteShift
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};