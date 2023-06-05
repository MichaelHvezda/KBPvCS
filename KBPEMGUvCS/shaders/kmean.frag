#version 460 core
#extension GL_ARB_explicit_uniform_location : enable
in vec2 TexCoord;

uniform float Saturation;
uniform float KeyColor;
uniform float Brightness;
uniform float Hue;


//A uniform of the type sampler2D will have the storage value of our texture.
layout (location = 0) uniform sampler2D uTexture0;
layout (location = 1) uniform sampler2D uTexture1;

out vec4 FragColor;

float NaDruhou(float firstNumber, float scndNumber) {
	return ((firstNumber - scndNumber) * (firstNumber - scndNumber));
}

float Vzdalenost(vec3 col, vec3 cent) {
	float pomZaporna;
	float pomKladna = NaDruhou(col.x, cent.x) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
	if (col.x < cent.x) {
		pomZaporna = NaDruhou(col.x, cent.x - 360.0f) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
	}
	else {
		pomZaporna = NaDruhou(col.x - 360.0f, cent.x) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
	}

	if (pomKladna <= pomZaporna) {
		return pomKladna;
	}
	else {
		return pomZaporna;
	}
}

float MaxnIsZero(float maxn, float minn){

	if(maxn == 0.0f){
		return 0.0f;
	}
	return (1.0f - (minn / maxn));
}

vec3 RgbToHsb(vec3 a){
	float r = (a.b);
	float g = (a.g);
	float b = (a.r);

	float maxn = max(r, max(g, b));
	float minn = min(r, min(g, b));

	float h = 0.0f;
	if (maxn == r && g >= b)
	{
		if (maxn - minn == 0.0)
		{
			h = 0.0f;
		}else
		{
			h = 60.0f * ((g - b) / (maxn - minn));
		}
	}else if (maxn == r && g < b)
	{
		h = 60.0f * ((g - b) / (maxn - minn)) + 360.0f;
	}else if (maxn == g)
	{
		h = 60.0f * ((b - r) / (maxn - minn)) + 120.0f;
	}else if (maxn == b)
	{
		h = 60.0f * ((r - g) / (maxn - minn)) + 240.0f;
	}
	float s = MaxnIsZero(maxn,minn);
	//h - 0/360
	//s - 0/1
	//maxn - 0/1
	return vec3(h, s, maxn);
}

void main()
{
    //Here we sample the texture based on the Uv coordinates of the fragment
    vec4 a = texture(uTexture0, TexCoord);
    vec4 b = texture(uTexture1, TexCoord);

	vec3 hsb = RgbToHsb(a.xyz);
	vec3 keyColorPoint = vec3(KeyColor,0,0);
	float keyDistance = Vzdalenost(hsb, keyColorPoint);

    if(keyDistance < Hue && hsb.y > Saturation && hsb.z > Brightness){
        FragColor = b;
    }else{
        FragColor = vec4(a.b,a.g,a.r,a.a);
    }


    //FragColor = vec4(1,0,1,1);
}