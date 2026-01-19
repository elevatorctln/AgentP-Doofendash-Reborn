Shader "Perry/Environment" {
Properties {
 _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
 _Emission ("Emmisive Color", Color) = (0,0,0,0)
 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
 _LayerTex ("Layer Texture", 2D) = "black" {}
}
SubShader { 
 LOD 1000
 Pass {
  Material {
   Emission [_Emission]
  }
  ColorMask RGB
  ColorMaterial AmbientAndDiffuse
  SetTexture [_MainTex] { combine texture * primary, texture alpha * primary alpha }
  SetTexture [_MainTex] { ConstantColor [_Color] combine previous * constant double, previous alpha * constant alpha }
  SetTexture [_LayerTex] { combine texture lerp(texture) previous }
 }
}
Fallback "Perry/Fallback Unlit"
}