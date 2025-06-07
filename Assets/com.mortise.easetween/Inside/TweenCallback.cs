using System;

public struct TweenCallback<T> where T : struct
{
    public Action<T> action;
    public int tweenId;
}