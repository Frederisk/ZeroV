#ifndef ORBIT_SELF_LUMINOUS_LEFT_FS
#define ORBIT_SELF_LUMINOUS_LEFT_FS

#undef HIGH_PRECISION_VERTEX
#define HIGH_PRECISION_VERTEX

#include "sh_Utils.h"
#include "sh_Masking.h"

layout(location = 2) in highp vec2 v_TexCoord;
// layout(location = 3) in highp vec4 v_TexRect;

layout(location = 0) out vec4 o_Colour;

const vec3 glowColor = vec3(1.0, 1.0, 0.95);
const float uEffectForce = 0.1;

void main(void)
{
    vec2 uv = v_TexCoord / vec2(v_TexRect[2] - v_TexRect[0], v_TexRect[3] - v_TexRect[1]);
    vec2 uvCentered = uv + vec2(-1.5, 0.0);

    float distanceToBottom = uvCentered.y + uEffectForce * 0.25 - 1.0 * (uvCentered.x + 0.5) * (uvCentered.x + 0.5);
    float glowStrength = smoothstep(0.5, 0.9, distanceToBottom);
    o_Colour = getRoundedColor(vec4(glowColor, 0.9 * glowStrength), v_TexCoord);
}

#endif
