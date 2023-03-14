#version 330 core
#extension GL_ARB_explicit_uniform_location : enable
in vec2 TexCoord;

//A uniform of the type sampler2D will have the storage value of our texture.
layout (location = 0) uniform sampler2D uTexture0;

out vec4 FragColor;

layout (location = 0) out vec4 color;
//layout (location = 1) out vec4 color1;
//layout (location = 2) out vec4 color2;
void main()
{
    //Here we sample the texture based on the Uv coordinates of the fragment
    FragColor = texture(uTexture0, TexCoord);
    //color = ;
    color =FragColor ;
//    color1 = vec4(0,1,0,1);
//    color2 = vec4(0,0,1,1);
}