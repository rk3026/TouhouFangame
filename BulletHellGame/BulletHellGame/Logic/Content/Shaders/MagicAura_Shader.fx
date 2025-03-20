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

float2 Distort(float2 uv)
{
    float strength = 0.01 * sin(time * 3.0 + uv.y * 15.0) * cos(time * 2.0 + uv.x * 10.0);
    uv.x += strength * sin(time + uv.y * 10.0);
    uv.y += strength * cos(time + uv.x * 10.0);
    return uv;
}

float3 MagicalAura(float2 uv)
{
    float2 distortedUV = Distort(uv);
    float3 color = tex2D(DecalSampler, distortedUV).rgb;

    // Add pulsating color aura
    float aura = sin(time * 2.0 + uv.y * 20.0) * 0.5 + 0.5;
    color.r += 0.2 * aura;
    color.b += 0.4 * (1.0 - aura);

    // Subtle radial gradient for magical effect
    float dist = length(uv - 0.5);
    color *= 1.0 - dist * 0.5;

    return color;
}

float4 main_fragment(VertexShaderOutput VOUT) : COLOR0
{
    float2 uv = VOUT.texCoord;
    float3 auraColor = MagicalAura(uv);
    return float4(auraColor, 1.0);
}

technique
{
    pass
    {
        PixelShader = compile ps_3_0 main_fragment();
    }
}