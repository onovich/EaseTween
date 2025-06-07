using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Explicit)]
public struct TweenModel {

    // 公共头信息 (28字节)
    [FieldOffset(0)] public int id;
    [FieldOffset(4)] public TweenType type;
    [FieldOffset(5)] public bool isPlaying;
    [FieldOffset(6)] public bool isLoop;
    [FieldOffset(7)] public bool isComplete;
    [FieldOffset(8)] public float elapsedTime;
    [FieldOffset(12)] public float duration;
    [FieldOffset(16)] public EasingType easing;
    [FieldOffset(17)] public byte flags; // 0 = NeedsChainStart, 1 = NeedsCallback

    // 当前值联合体 (16字节)
    [FieldOffset(20)] public float floatValue;
    [FieldOffset(20)] public Vector2 vector2Value;
    [FieldOffset(20)] public Vector3 vector3Value;
    [FieldOffset(20)] public Color colorValue;
    [FieldOffset(20)] public Quaternion quaternionValue;
    [FieldOffset(20)] public Color32 color32Value;

    // 起始值联合体 (16字节)
    [FieldOffset(36)] public float floatStart;
    [FieldOffset(36)] public Vector2 vector2Start;
    [FieldOffset(36)] public Vector3 vector3Start;
    [FieldOffset(36)] public Color colorStart;
    [FieldOffset(36)] public Quaternion quaternionStart;
    [FieldOffset(36)] public Color32 color32Start;

    // 结束值联合体 (16字节)
    [FieldOffset(52)] public float floatEnd;
    [FieldOffset(52)] public Vector2 vector2End;
    [FieldOffset(52)] public Vector3 vector3End;
    [FieldOffset(52)] public Color colorEnd;
    [FieldOffset(52)] public Quaternion quaternionEnd;
    [FieldOffset(52)] public Color32 color32End;

    // 链式调用信息 (4字节)
    [FieldOffset(68)] public int nextId;

}