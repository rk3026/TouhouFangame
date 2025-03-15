struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 texCoord : TEXCOORD0;
};

#define brightboost 1.0
#define hardPix -10.0
#define hardScan -6.0

float2 textureSize;

sampler2D DecalSampler = sampler_state
{
};

float3 ToSrgb(float3 c)
{
    return pow(c, 1 / 2.2);
}

float3 Fetch(float2 pos, float2 off, float2 texture_size)
{
    pos = (floor(pos * texture_size.xy + off) + float2(0.5, 0.5)) / texture_size.xy;
    return brightboost * pow(tex2D(DecalSampler, pos.xy).rgb, 2.2);
}

float2 Dist(float2 pos, float2 texture_size)
{
    pos = pos * texture_size.xy;
    return -(frac(pos) - float2(0.5, 0.5));
}

float Gaus(float pos, float scale)
{
    return exp2(scale * pos * pos);
}

float3 Horz3(float2 pos, float off, float2 texture_size)
{
    float3 b = Fetch(pos, float2(-1.0, off), texture_size);
    float3 c = Fetch(pos, float2(0.0, off), texture_size);
    float3 d = Fetch(pos, float2(1.0, off), texture_size);
    float dst = Dist(pos, texture_size).x;
    float scale = hardPix;
    float wb = Gaus(dst - 1.0, scale);
    float wc = Gaus(dst + 0.0, scale);
    float wd = Gaus(dst + 1.0, scale);
    return (b * wb + c * wc + d * wd) / (wb + wc + wd);
}

float3 Horz5(float2 pos, float off, float2 texture_size)
{
    float3 a = Fetch(pos, float2(-2.0, off), texture_size);
    float3 b = Fetch(pos, float2(-1.0, off), texture_size);
    float3 c = Fetch(pos, float2(0.0, off), texture_size);
    float3 d = Fetch(pos, float2(1.0, off), texture_size);
    float3 e = Fetch(pos, float2(2.0, off), texture_size);
    float dst = Dist(pos, texture_size).x;
    float scale = hardPix;
    float wa = Gaus(dst - 2.0, scale);
    float wb = Gaus(dst - 1.0, scale);
    float wc = Gaus(dst + 0.0, scale);
    float wd = Gaus(dst + 1.0, scale);
    float we = Gaus(dst + 2.0, scale);
    return (a * wa + b * wb + c * wc + d * wd + e * we) / (wa + wb + wc + wd + we);
}

float Scan(float2 pos, float off, float2 texture_size)
{
    float dst = Dist(pos, texture_size).y;
    return Gaus(dst + off, hardScan);
}

float3 Tri(float2 pos, float2 texture_size)
{
    float3 a = Horz3(pos, -1.0, texture_size);
    float3 b = Horz5(pos, 0.0, texture_size);
    float3 c = Horz3(pos, 1.0, texture_size);
    float wa = Scan(pos, -1.0, texture_size);
    float wb = Scan(pos, 0.0, texture_size);
    float wc = Scan(pos, 1.0, texture_size);
    return a * wa + b * wb + c * wc;
}

float4 crt_lottes(float2 tex)
{
    float2 pos = tex.xy;
    pos = pos * 2.0 - 1.0;
    pos = pos * 0.5 + 0.5;
    float3 outColor = Tri(pos, textureSize);
    return float4(ToSrgb(outColor.rgb), 1.0);
}

float4 main_fragment(VertexShaderOutput VOUT) : COLOR0
{
    return crt_lottes(VOUT.texCoord);
}

technique
{
    pass
    {
        PixelShader = compile ps_3_0 main_fragment();
    }
}

