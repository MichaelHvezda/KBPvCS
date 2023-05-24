// See https://aka.ms/new-console-template for more information
using System.Drawing;

Console.WriteLine("Hello, World!");

MainFce(out var col01, out var col11, out var col21);
MainFceJava(out var col32);
MainNew(out var col0, out var col1, out var col2, out var col3);

Console.WriteLine(col0);
Console.WriteLine(col1);
Console.WriteLine(col2);
Console.WriteLine(col3);

Console.WriteLine(col01);
Console.WriteLine(col11);
Console.WriteLine(col21);

Console.WriteLine(col32);

void MainFce(out vec3 color0, out vec3 color1, out vec3 color2)
{

    vec3 cent1 = new vec3(0.7f, 0.2f, 0.5f);
    vec3 cent2 = new vec3(1f, 0.5f, 0.7f);
    vec3 cent3 = new vec3(0.5f, 0.7f, 0.2f);

    vec4 a = new vec4(32, 168, 199, 1);
    vec3 hsb = RgbToHsb(a.xyz);

    vec3 cent1hsb = ToShaderRange(cent1);
    vec3 cent2hsb = ToShaderRange(cent2);
    vec3 cent3hsb = ToShaderRange(cent3);

    vec2 jedna = Vzdalenost(hsb, cent1hsb);
    vec2 dva = Vzdalenost(hsb, cent2hsb);
    vec2 tri = Vzdalenost(hsb, cent3hsb);


    float smal = min(min(jedna.x, dva.x), tri.x);

    if (jedna.x == smal)
    {
        color0 = ToTextureRange(hsb, jedna.y);
        color2 = new vec4(0, 0, 0, 0);
        color1 = new vec4(0, 0, 0, 0);
        return;
        //color0 = vec4(a.x, a.y, a.z, 1);

        //color0 = vec4(-a.x, -a.y, -a.z, 1);
        //color0 = vec4(-1, 0, -1, 1);
    }
    else
    {
        //color0 = vec4(1, 0, 1, 0);
        //color0 = vec4(a.x, a.y, a.z, 0);
        color0 = new vec4(0, 0, 0, 0);
    }

    if (dva.x == smal)
    {
        color1 = ToTextureRange(hsb, dva.y);
        color2 = new vec4(0, 0, 0, 0);
        return;
        //color1 = vec4(a.x, a.y, a.z, 1);
        //color1 = vec4(1, 0, 1, 0);
    }
    else
    {
        //color1 = vec4(1, 0, 1, 0);
        //color1 = vec4(a.x, a.y, a.z, 0);
        color1 = new vec4(0, 0, 0, 0);
    }

    if (tri.x == smal)
    {
        color2 = ToTextureRange(hsb, tri.y);
        //color2 = vec4(a.x, a.y, a.z, 1);
        //color2 = vec4(1, 0, 1, 0);
    }
    else
    {
        //color2 = vec4(1,0,1, 0);
        //color2 = vec4(a.x, a.y, a.z, 0);
        color2 = new vec4(0, 0, 0, 0);
    }



}

void MainFceJava(out vec3 color0)
{
    vec3 cent1 = new vec3(50.280f / 255f, 19.13f / 255f, 49.56f / 255f);
    vec3 cent2 = new vec3(200.46f / 255f, 87.40f / 255f, 71.90f / 255f);
    vec3 cent3 = new vec3(192.81f / 255f, 24.81f / 255f, 26.75f / 255f);
    //zjisteni barvy popredi a pozadi
    vec4 outColor = new vec4(50, 66, 62, 1);
    vec4 colorBack = new vec4(0.05f * 255, 0.4f * 255, 0.596f * 255, 1);

    //prepocet popredi
    vec3 hsb = RgbToHsb(outColor.xyz);
    vec3 colBack = RgbToHsb(colorBack);

    //zjisteni vzdalenosti od centroid
    float jednaBack = vzdalenost(colBack, cent1);
    float dvaBack = vzdalenost(colBack, cent2);
    float triBack = vzdalenost(colBack, cent3);

    //zjisteni vzdalenosti od centroid pozadi
    float jedna = vzdalenost(hsb, cent1);
    float dva = vzdalenost(hsb, cent2);
    float tri = vzdalenost(hsb, cent3);

    //zjisteni nejmensi vzdalenosti
    float smal = small(jedna, dva, tri);
    float smalBack = small(jednaBack, dvaBack, triBack);

    //urceni jestli zobrazovat popredi nebo pozadi
    if (jedna == smal)
    {
        if (jednaBack == smalBack)
        {
            color0 = new vec4(255, 0, 0, 1);
            return;
        }
    }

    if (dva == smal)
    {
        if (dvaBack == smalBack)
        {
            color0 = new vec4(0, 255, 0, 1);
            return;
        }
    }

    if (tri == smal)
    {
        if (triBack == smalBack)
        {
            color0 = new vec4(0, 0, 255, 1);
            return;
        }
    }
    color0 = outColor;
}

void MainNew(out vec3 color0, out vec3 color1, out vec3 color2, out vec3 color3)
{
    vec3 cent1 = new vec3(0.7f, 0.2f, 0.5f);
    vec3 cent2 = new vec3(1f, 0.5f, 0.7f);
    vec3 cent3 = new vec3(0.5f, 0.7f, 0.2f); ;
    vec3 cent4 = new vec3(0.5555f, 0.0f, 0.0f); ;

    //zjisteni barvy popredi a pozadi
    vec4 outColor = new vec4(50, 66, 62, 1);
    vec4 colorBack = new vec4(0.05f * 255, 0.4f * 255, 0.596f * 255, 1);

    vec4 a = new vec4(32, 168, 199, 1);
    vec3 hsb = RgbToHsb(a.xyz);

    vec3 cent1hsb = ToShaderRange(cent1);
    vec3 cent2hsb = ToShaderRange(cent2);
    vec3 cent3hsb = ToShaderRange(cent3);

    vec3 colBack = ToShaderRange(cent4);


    vec2 jednaBack = Vzdalenost(colBack, cent1hsb);
    vec2 dvaBack = Vzdalenost(colBack, cent2hsb);
    vec2 triBack = Vzdalenost(colBack, cent3hsb);

    vec2 jedna = Vzdalenost(hsb, cent1hsb);
    vec2 dva = Vzdalenost(hsb, cent2hsb);
    vec2 tri = Vzdalenost(hsb, cent3hsb);

    float smal = min(min(jedna.x, dva.x), tri.x);
    float smalBack = min(min(jednaBack.x, dvaBack.x), triBack.x);
    color3 = new vec4(0, 0, 0, 0);
    if (jedna.x == smal)
    {
        color0 = ToTextureRange(hsb, jedna.y);
        color1 = new vec4(0, 0, 0, 0);
        color2 = new vec4(0, 0, 0, 0);

        if (jednaBack.x == smalBack)
        {
            color3 = ToTextureRange(hsb, jednaBack.y); ;
        }
    }
    else
    {
        color0 = new vec4(0, 0, 0, 0);
    }

    if (dva.x == smal)
    {
        color1 = ToTextureRange(hsb, dva.y);
        color2 = new vec4(0, 0, 0, 0);

        if (dvaBack.x == smalBack)
        {
            color3 = ToTextureRange(hsb, dvaBack.y); ;
        }
    }
    else
    {
        color1 = new vec4(0, 0, 0, 0);
    }

    if (tri.x == smal)
    {
        color2 = ToTextureRange(hsb, tri.y);

        if (triBack.x == smalBack)
        {
            color3 = ToTextureRange(hsb, triBack.y); ;
        }
    }
    else
    {
        color2 = new vec4(0, 0, 0, 0);
    }
}
float small(float a, float b, float c)
{
    return min(min(a, b), c);
}

float NaDruhou(float firstNumber, float scndNumber)
{
    return ((firstNumber - scndNumber) * (firstNumber - scndNumber));
}

vec2 Vzdalenost(vec3 col, vec3 cent)
{
    float pomZaporna;
    float pomKladna = NaDruhou(col.x, cent.x) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
    if (col.x < cent.x)
    {
        pomZaporna = NaDruhou(col.x, cent.x - 360) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
    }
    else
    {
        pomZaporna = NaDruhou(col.x - 360, cent.x) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
    }

    if (pomKladna <= pomZaporna)
    {
        return new vec2(pomKladna, 1);
    }
    else
    {
        return new vec2(pomZaporna, -1);
    }
}
//vypocet vzdalenosti dvou bodu
float vzdalenost(vec3 col, vec3 cent)
{
    float pomZaporna;
    float pomKladna = NaDruhou(col.x, cent.x) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
    if (col.x < cent.x)
    {
        pomZaporna = NaDruhou(col.x, cent.x - 360) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
    }
    else
    {
        pomZaporna = NaDruhou(col.x - 360, cent.x) + NaDruhou(col.y, cent.y) + NaDruhou(col.z, cent.z);
    }

    if (pomKladna <= pomZaporna)
    {
        return pomKladna;
    }
    else
    {
        return pomZaporna;
    }
}

float MaxnIsZero(float maxn, float minn)
{

    if (maxn == 0.0f)
    {
        return 0.0f;
    }
    return (1.0f - (minn / maxn));
}

vec3 RgbToHsb(vec3 a)
{
    float r = (a.x) / 255;
    float g = (a.y) / 255;
    float b = (a.z) / 255;

    float maxn = max(r, max(g, b));
    float minn = min(r, min(g, b));

    float h = 0.0f;
    if (maxn == r && g >= b)
    {
        if (maxn - minn == 0.0)
        {
            h = 0.0f;
        }
        else
        {
            h = 60.0f * ((g - b) / (maxn - minn));
        }
    }
    else if (maxn == r && g < b)
    {
        h = 60.0f * ((g - b) / (maxn - minn)) + 360.0f;
    }
    else if (maxn == g)
    {
        h = 60.0f * ((b - r) / (maxn - minn)) + 120.0f;
    }
    else if (maxn == b)
    {
        h = 60.0f * ((r - g) / (maxn - minn)) + 240.0f;
    }
    float s = MaxnIsZero(maxn, minn);
    //h - 0/360
    //s - 0/1
    //maxn - 0/1
    return new vec3(h, s, maxn);
}

vec3 ToShaderRange(vec3 a)
{
    //h - 0/360
    //s - 0/1
    //maxn - 0/1
    return new vec3(mod((a.x + 1.0f), 1.0f) * 360.0f, a.y, a.z);
}

vec4 ToTextureRange(vec3 a, float b)
{
    //h - -1/1
    //s - 0/1
    //maxn - 0/1
    return new vec4(mod((b + (a.x / 360.0f)), 1), a.y, a.z, 1);
}

float mod(float firstNumber, float scndNumber)
{
    return firstNumber % scndNumber;
}

float min(float firstNumber, float scndNumber)
{
    return Math.Min(firstNumber, scndNumber);
}
float max(float firstNumber, float scndNumber)
{
    return Math.Max(firstNumber, scndNumber);
}
public class vec2
{
    public vec2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public float x { get; set; }
    public float y { get; set; }
    public vec2 xy => new vec2(x, y);
}

public class vec3 : vec2
{
    public vec3(float x, float y, float z) : base(x, y)
    {
        this.z = z;
    }

    public float z { get; set; }
    public vec3 xyz => new vec3(x, y, z);
}
public class vec4 : vec3
{
    public vec4(float x, float y, float z, float w) : base(x, y, z)
    {
        this.w = w;
    }

    public float w { get; set; }

    public override string ToString()
    {
        return $"{x} {y} {z} {w}";
    }
}