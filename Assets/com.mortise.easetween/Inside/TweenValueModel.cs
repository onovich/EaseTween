using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Explicit)]
internal struct TweenValueModel {

    [FieldOffset(0)]
    public float floatValue;
    [FieldOffset(0)]
    public Vector2 vector2Value;
    [FieldOffset(0)]
    public Vector3 vector3Value;
    [FieldOffset(0)]
    public Color colorValue;
    [FieldOffset(0)]
    public Quaternion quaternionValue;
    [FieldOffset(0)]
    public Color32 color32Value;

}