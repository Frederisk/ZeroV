#ifndef SPIN_RAINBOW_FS
#define SPIN_RAINBOW_FS

#include "sh_Utils.h"

#ifndef PI
#define PI 3.14159265359
#endif

layout(location = 2) in highp vec2 v_TexCoord;
layout(location = 3) in highp vec4 v_TexRect;

layout(location = 0) out vec4 o_Colour;

/*
float SampleGaussian(float x, float width)
{
    // return exp(-x * x / (2.0 * width * width));
    return (width - x) * (width + x) / (width * width);
}
*/

float atan2(float y, float x){
    float basicAtan = atan(y/x);
    if (x > 0.0) return basicAtan;
    else if (x < 0.0 && y >=0.0) return basicAtan + PI;
    else if (x < 0.0 && y < 0.0) return basicAtan - PI;
    else if (x == 0.0 && y > 0.0) return PI / 2.0;
    else if (x == 0.0 && y < 0.0) return -PI / 2.0;
    return 0.0;
}

void main(void)
{
    vec2 uv = v_TexCoord / vec2(v_TexRect[2] - v_TexRect[0], v_TexRect[3] - v_TexRect[1]);
    vec2 uvCentered = uv + vec2(-0.5, -0.5);

    float angleRad = atan2(uvCentered.y, uvCentered.x);
    if (angleRad < 0.0) angleRad += 2.0 * PI;
    float roundRad = angleRad / (2.0 * PI);

    o_Colour = hsv2rgb(vec4(roundRad, 0.7, 1.0, 1.0));
}

#endif
