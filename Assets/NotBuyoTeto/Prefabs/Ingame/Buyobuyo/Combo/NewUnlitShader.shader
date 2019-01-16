Shader "Unlit/NewUnlitShader"
{
 Properties {
 _MainTex ("Texture", 2D) = "white" {}
 }
 SubShader {
        Tags { "RenderType"="Opaque" }

        CGINCLUDE

        #define PI 3.14159

        ENDCG

        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        void surf (Input IN, inout SurfaceOutput  o) {
            half3 c;

            float x = IN.uv_MainTex.x * 2 * PI;

            float a = 3 / PI;
            float b = PI / 3;

            if (0 <= x && x <  PI / 3.0f) {
                c = half3(1, a * x, 0);
            } else if (PI / 3.0f <= x && x < 2 * PI / 3.0f) {
                c = half3(-a * (x - 2 * b), 1, 0);
            } else if (2 * PI / 3 <= x && x < PI) {
                c = half3(0, 1, a * (x - 2 * b));
            } else if (PI <= x && x < 4 * PI / 3.0f) {
                c = half3(0, -a * (x - 4 * b), 1);
            } else if (4 * PI / 3.0f <= x && x < 5 * PI / 3.0f) {
                c = half3(a * (x - 4 * b), 0, 1);
            } else {
                c = half3(1, 0, -a * (x - 6 * b));
            }

            o.Albedo = c;
            // o.Albedo = c + tex2D (_MainTex, IN.uv_MainTex).rgb; // テキスチャベースに着色するならこっち 明るくなりすぎるので適宜パラメータで調整する
        }
        ENDCG
    }
 FallBack "Diffuse"
}