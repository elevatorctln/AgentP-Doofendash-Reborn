Shader "Perry/Unlit - No Fog" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader { 
 Pass {
  Fog { Mode Off }
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant, texture alpha * constant alpha }
 }
}
}