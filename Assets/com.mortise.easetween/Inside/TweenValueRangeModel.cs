using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Explicit)]
internal struct TweenValueRangeModel {

    [FieldOffset(0)]
    public float floatStart;
    [FieldOffset(4)]
    public float floatEnd;

    [FieldOffset(0)]
    public Vector2 vector2Start;
    [FieldOffset(8)]
    public Vector2 vector2End;

    [FieldOffset(0)]
    public Vector3 vector3Start;
    [FieldOffset(12)]
    public Vector3 vector3End;

    [FieldOffset(0)]
    public Color colorStart;
    [FieldOffset(16)]
    public Color colorEnd;

    [FieldOffset(0)]
    public Quaternion quaternionStart;
    [FieldOffset(16)]
    public Quaternion quaternionEnd;

    [FieldOffset(0)]
    public Color32 color32Start;
    [FieldOffset(16)]
    public Color32 color32End;

}