struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 texCoord : TEXCOORD0;
};

float2 textureSize;
float time;

sampler2D DecalSampler = sampler_state
{
};

float Random(float2 uv)
{
    return frac(sin(dot(uv.xy, float2(12.9898, 78.233))) * 43758.5453);
}

float2 Distort(float2 uv)
{
    float amount = 0.005 * sin(time * 2.0 + uv.y * 10.0);
    uv.x += amount * (Random(uv + time) - 0.5);
    return uv;
}

float3 GlitchEffect(float2 uv)
{
    float2 distortedUV = Distort(uv);
    float3 color;
    color.r = tex2D(DecalSampler, distortedUV + float2(0.005, 0.0)).r;
    color.g = tex2D(DecalSampler, distortedUV).g;
    color.b = tex2D(DecalSampler, distortedUV - float2(0.005, 0.0)).b;
    
    // Add scanlines
    float scanline = sin(uv.y * textureSize.y * 0.5 + time * 10.0) * 0.1;
    color *= 1.0 - scanline;

    return color;
}

float4 main_fragment(VertexShaderOutput VOUT) : COLOR0
{
    float2 uv = VOUT.texCoord;
    float3 glitchColor = GlitchEffect(uv);
    return float4(glitchColor, 1.0);
}

technique
{
    pass
    {
        PixelShader = compile ps_3_0 main_fragment();
    }
}
