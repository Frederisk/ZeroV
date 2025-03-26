#ifndef SPIN_RAINBOW_FS
#define SPIN_RAINBOW_FS

#undef PI
#define PI 3.14159265359

#undef SQRT2
#define SQRT2 1.41421356237

#undef HIGH_PRECISION_VERTEX
#define HIGH_PRECISION_VERTEX

#include "sh_Utils.h"
#include "sh_Masking.h"

layout(location = 2) in highp vec2 v_TexCoord;
//layout(location = 3) in highp vec4 v_TexRect;

layout(std140, set = 0, binding = 0) uniform m_SpinRanbowFrameParameters
{
    vec4 hsvaColour;
    float sizeRatio;
    float borderRatio;
};

layout(location = 0) out vec4 o_Colour;

float atan2(float y, float x){
    float basicAtan = atan(y/x);
    if (x > 0.0) return basicAtan;
    if (x < 0.0 && y >=0.0) return basicAtan + PI;
    if (x < 0.0 && y < 0.0) return basicAtan - PI;
    if (x == 0.0 && y > 0.0) return PI / 2.0;
    if (x == 0.0 && y < 0.0) return -PI / 2.0;
    return 0.0;
}

float fadeOutEdge(float x, float width) {
    if (abs(x) > width) return 0.0;
    return (width - x) * (width + x) / (width * width);
}

void main(void)
{
    vec2 uv = v_TexCoord / vec2(v_TexRect[2] - v_TexRect[0], v_TexRect[3] - v_TexRect[1]);
    vec2 uvCentered = uv + vec2(-0.5, -0.5);

    float angleRad = atan2(uvCentered.y, uvCentered.x);
    if (angleRad < 0.0) angleRad += 2.0 * PI;
    float roundRad = angleRad / (2.0 * PI);

    //float sizeRatio = sizeRatio - w * SQRT2;
    float effect = 0.0;

    vec2 uvAbs = abs(uvCentered);
    if (abs(uvAbs.x - uvAbs.y) < sizeRatio)
    if (abs(uvAbs.x + uvAbs.y - sizeRatio) < borderRatio * SQRT2)
    effect = fadeOutEdge((uvAbs.x + uvAbs.y - sizeRatio) / SQRT2, borderRatio);
    if (abs(uvAbs.x - uvAbs.y) > sizeRatio) {
        effect  = 1.0;
        vec2 temp = uvAbs.x > uvAbs.y ? uvAbs : vec2(uvAbs.y, uvAbs.x);
        effect = fadeOutEdge(distance(temp, vec2(sizeRatio, 0)), borderRatio);
    }

    if (hsvaColour.x < 0.0) {
        o_Colour = getRoundedColor(hsv2rgb(vec4(roundRad, hsvaColour.yz, hsvaColour.w * effect)), v_TexCoord);
    } else {
        o_Colour = effect * getRoundedColor(vec4(hsvaColour.xyz , hsvaColour.w * effect), v_TexCoord);
    }
}

#endif
