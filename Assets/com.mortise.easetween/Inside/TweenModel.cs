using System.Runtime.InteropServices;
using UnityEngine;
using RD = Unity.Mathematics.Random;

[StructLayout(LayoutKind.Explicit)]
internal struct TweenModel {

    // Public HEAD Info (28字节)
    [FieldOffset(0)] internal int id;
    [FieldOffset(4)] internal TweenType type;
    [FieldOffset(5)] internal bool isPlaying;
    [FieldOffset(6)] internal bool isLoop;
    [FieldOffset(7)] internal bool isComplete;
    [FieldOffset(8)] internal float elapsedTime;
    [FieldOffset(12)] internal float duration;
    [FieldOffset(16)] internal EasingType easing;
    [FieldOffset(17)] internal byte flags; // 0 = NeedsChainStart, 1 = NeedsCallback

    // Current Value Union (16字节)
    [FieldOffset(20)] internal float floatValue;
    [FieldOffset(20)] internal Vector2 vector2Value;
    [FieldOffset(20)] internal Vector3 vector3Value;
    [FieldOffset(20)] internal Color colorValue;
    [FieldOffset(20)] internal Quaternion quaternionValue;
    [FieldOffset(20)] internal Color32 color32Value;
    [FieldOffset(20)] internal int intValue;

    // Start Value Union (16字节)
    [FieldOffset(36)] internal float floatStart;
    [FieldOffset(36)] internal Vector2 vector2Start;
    [FieldOffset(36)] internal Vector3 vector3Start;
    [FieldOffset(36)] internal Color colorStart;
    [FieldOffset(36)] internal Quaternion quaternionStart;
    [FieldOffset(36)] internal Color32 color32Start;
    [FieldOffset(36)] internal int intStart;

    // End Value Union (16字节)
    [FieldOffset(52)] internal float floatEnd;
    [FieldOffset(52)] internal Vector2 vector2End;
    [FieldOffset(52)] internal Vector3 vector3End;
    [FieldOffset(52)] internal Color colorEnd;
    [FieldOffset(52)] internal Quaternion quaternionEnd;
    [FieldOffset(52)] internal Color32 color32End;
    [FieldOffset(52)] internal int intEnd;

    // Chain Info (4字节)
    [FieldOffset(68)] internal int nextId;

    // Random State (8字节)
    [FieldOffset(72)] internal RD randomState;

}