﻿#version 330 core
#extension GL_ARB_explicit_uniform_location : enable
in vec2 TexCoord;

//A uniform of the type sampler2D will have the storage value of our texture.
layout (location = 0) uniform sampler2D uTexture0;
layout (location = 1) uniform sampler2D uTexture1;
layout (location = 2) uniform sampler2D uTexture2;

out vec4 FragColor;

layout (location = 0) out vec4 color;
void main()
{
    vec2 yRotate = vec2(TexCoord.x,1 - TexCoord.y);
    //Here we sample the texture based on the Uv coordinates of the fragment
    vec4 a = texture(uTexture0, TexCoord);
    vec4 b = texture(uTexture1, TexCoord);
    vec4 c = texture(uTexture2, yRotate);

    if(c.w == 1){
        FragColor = b;
    }else{
        FragColor = a;
    }

    color = FragColor ;

    //FragColor = vec4(1,0,1,1);
}