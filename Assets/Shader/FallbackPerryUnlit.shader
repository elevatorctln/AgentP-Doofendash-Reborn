Shader "Perry/Fallback Unlit" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "RenderType"="Opaque" }
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant double }
 }
}
}