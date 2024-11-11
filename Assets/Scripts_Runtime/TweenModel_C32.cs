using System;
using UnityEngine;

public class TweenModel_C32: ITween {

    Color32 startValue;
    Color32 endValue;

    float duration;
    Func<float, float, float, float, float> easingFunction;

    float elapsedTime;
    bool isPlaying;

    Action<Color32> onUpdate;
    Action onComplete;

    bool isComplete;
    public bool IsComplete => isComplete;

    bool isLoop;

    public TweenModel_C32(Color32 startValue, Color32 endValue, float duration, Func<float, float, float, float, float> easingFunction, bool isLoop) {
        this.startValue = startValue;
        this.endValue = endValue;
        this.duration = duration;
        this.easingFunction = easingFunction;
        this.isLoop = isLoop;
        this.elapsedTime = 0;
        this.isPlaying = false;
        this.isComplete = false;
    }

    public TweenModel_C32 OnUpdate(Action<Color32> onUpdate) {
        this.onUpdate = onUpdate;
        return this;
    }

    public TweenModel_C32 OnComplete(Action onComplete) {
        this.onComplete = onComplete;
        return this;
    }

    public void Play() => isPlaying = true;
    public void Pause() => isPlaying = false;
    public void Restart() {
        elapsedTime = 0;
        isPlaying = true;
        isComplete = false;
    }

    public void Tick(float dt) {
        TickPlay(dt);
    }

    void TickPlay(float dt) {
        if (!isPlaying) return;

        elapsedTime += dt;
        if (elapsedTime >= duration) {
            elapsedTime = duration;
            isPlaying = false;
            onComplete?.Invoke();
            isComplete = true;

            if (isLoop) {
                Restart();
            }
            return;
        }

        byte r = (byte)easingFunction(elapsedTime, startValue.r, endValue.r - startValue.r, duration);
        byte g = (byte)easingFunction(elapsedTime, startValue.g, endValue.g - startValue.g, duration);
        byte b = (byte)easingFunction(elapsedTime, startValue.b, endValue.b - startValue.b, duration);
        byte a = (byte)easingFunction(elapsedTime, startValue.a, endValue.a - startValue.a, duration);
        Color32 value = new Color32(r, g, b, a);
        onUpdate?.Invoke(value);
    }

}