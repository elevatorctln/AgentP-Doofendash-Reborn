Shader "Perry/Diffuse, Alpha, Emmisive,  Vert Color" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _Emission ("Emmisive Color", Color) = (0,0,0,0)
 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" }
  Lighting On
  Material {
   Emission [_Emission]
  }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  AlphaTest Greater 0
  ColorMask RGB
  ColorMaterial AmbientAndDiffuse
  SetTexture [_MainTex] { combine texture * primary, texture alpha * primary alpha }
  SetTexture [_MainTex] { ConstantColor [_Color] combine previous * constant double, previous alpha * constant alpha }
 }
}
Fallback "Alpha/VertexLit"
}