#version 330 core
in vec2 TexCoord;

//A uniform of the type sampler2D will have the storage value of our texture.
uniform sampler2D uTexture0;

uniform vec3 cent1;
uniform vec3 cent2;
uniform vec3 cent3;

//out vec4 FragColor;
layout (location = 0) out vec4 color0;
layout (location = 1) out vec4 color1;
layout (location = 2) out vec4 color2;


float NaDruhou(float firstNumber, float scndNumber) {
	return ((firstNumber - scndNumber) * (firstNumber - scndNumber));
}

float Vzdalenost(vec3 col, vec3 cent) {
	float pomZaporna;
	float pomKladna = NaDruhou(col.x, cent.x) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
	if (col.x < cent.x) {
		pomZaporna = NaDruhou(col.x, cent.x - 360) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
	}
	else {
		pomZaporna = NaDruhou(col.x - 360, cent.x) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
	}

	if (pomKladna <= pomZaporna) {
		return pomKladna;
	}
	else {
		return pomZaporna;
	}
}

float MaxnIsZero(float maxn, float minn){

	if(maxn == 0.0){
		return 0.0f;
	}
	return (1.0 - (minn / maxn));
}

vec3 RgbToHsb(vec3 a)
{
	float r = (a.r);
	float g = (a.g);
	float b = (a.b);

	float maxn = max(r, max(g, b));
	float minn = min(r, min(g, b));

	float h = 0.0;
	if (maxn == r && g >= b)
	{
		if (maxn - minn == 0.0)
		{
			h = 0.0;
		}else
		{
			h = 60.0 * ((g - b) / (maxn - minn));
		}
	}else if (maxn == r && g < b)
	{
		h = 60.0 * ((g - b) / (maxn - minn)) + 360.0;
	}else if (maxn == g)
	{
		h = 60.0 * ((b - r) / (maxn - minn)) + 120.0;
	}else if (maxn == b)
	{
		h = 60.0 * ((r - g) / (maxn - minn)) + 240.0;
	}
	float s = MaxnIsZero(maxn,minn);
	//h - 0/360
	//s - 0/1
	//maxn - 0/1
	return vec3(h, s, maxn);
}

void main()
{

    vec4 a = texture(uTexture0, TexCoord);
	vec3 hsb = RgbToHsb(a.xyz);

	vec3 cent1hsb = RgbToHsb(cent1);
	vec3 cent2hsb = RgbToHsb(cent2);
	vec3 cent3hsb = RgbToHsb(cent3);

	float jedna = Vzdalenost(hsb, cent1hsb);
	float dva = Vzdalenost(hsb, cent2hsb);
	float tri = Vzdalenost(hsb, cent3hsb);


	float smal = min(min(jedna, dva), tri);

	if (jedna == smal) {
		color0 = vec4(a.x, a.y, a.z, 1);

		//color0 = vec4(1, 0, 1, 0);
	}
	else {
		//color0 = vec4(1, 0, 1, 0);
		//color0 = vec4(a.x, a.y, a.z, 0);
		color0 = vec4(0, 0, 0, 0);
	}

	if (dva == smal) {
		color1 = vec4(a.x, a.y, a.z, 1);
		//color1 = vec4(1, 0, 1, 0);
	}
	else {
		//color1 = vec4(1, 0, 1, 0);
		//color1 = vec4(a.x, a.y, a.z, 0);
		color1 = vec4(0, 0, 0, 0);
	}

	if (tri == smal) {
		color2 = vec4(a.x, a.y, a.z, 1);
		//color2 = vec4(1, 0, 1, 0);
	}
	else {
		//color2 = vec4(1,0,1, 0);
		//color2 = vec4(a.x, a.y, a.z, 0);
		color2 = vec4(0, 0, 0, 0);
	}
}
