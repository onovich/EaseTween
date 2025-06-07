using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Explicit)]
internal struct TweenModel {

    // 公共头信息 (28字节)
    [FieldOffset(0)] public int id;
    [FieldOffset(4)] public TweenType type;
    [FieldOffset(8)] public bool shouldStartNext;
    [FieldOffset(12)] public float elapsedTime;
    [FieldOffset(16)] public float duration;
    [FieldOffset(20)] public EasingType easing;
    [FieldOffset(24)] public bool isPlaying;
    [FieldOffset(25)] public bool isLoop;
    [FieldOffset(26)] public bool isComplete;

    // 当前值联合体 (16字节)
    [FieldOffset(28)] public float floatValue;
    [FieldOffset(28)] public Vector2 vector2Value;
    [FieldOffset(28)] public Vector3 vector3Value;
    [FieldOffset(28)] public Color colorValue;
    [FieldOffset(28)] public Quaternion quaternionValue;
    [FieldOffset(28)] public Color32 color32Value;

    // 范围值联合体 (20字节)
    [FieldOffset(44)] public float floatStart;
    [FieldOffset(48)] public float floatEnd;
    [FieldOffset(44)] public Vector2 vector2Start;
    [FieldOffset(52)] public Vector2 vector2End;
    [FieldOffset(44)] public Vector3 vector3Start;
    [FieldOffset(56)] public Vector3 vector3End;
    [FieldOffset(44)] public Color colorStart;
    [FieldOffset(60)] public Color colorEnd;
    [FieldOffset(44)] public Quaternion quaternionStart;
    [FieldOffset(60)] public Quaternion quaternionEnd;
    [FieldOffset(44)] public Color32 color32Start;
    [FieldOffset(60)] public Color32 color32End;

}