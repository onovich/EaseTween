using System;
using UnityEngine;

internal struct TweenModel {

    public int id;
    public TweenType type;
    public int nextId;
    public float elapsedTime;
    public float duration;
    public EasingType easing;
    public bool isPlaying;
    public bool isLoop;
    public bool isComplete;

}