using System;
using UnityEngine;

public class TweenModel_C : ITween {

    Color startValue;
    Color endValue;

    float duration;
    Func<float, float, float, float, float> easingFunction;

    float elapsedTime;
    bool isPlaying;

    Action<Color> onUpdate;
    Action onComplete;

    bool isComplete;
    public bool IsComplete => isComplete;

    bool isLoop;

    public TweenModel_C(Color startValue, Color endValue, float duration, Func<float, float, float, float, float> easingFunction, bool isLoop) {
        this.startValue = startValue;
        this.endValue = endValue;
        this.duration = duration;
        this.easingFunction = easingFunction;
        this.isLoop = isLoop;
        this.elapsedTime = 0;
        this.isPlaying = false;
        this.isComplete = false;
    }

    public TweenModel_C OnUpdate(Action<Color> onUpdate) {
        this.onUpdate = onUpdate;
        return this;
    }

    public TweenModel_C OnComplete(Action onComplete) {
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

        float r = easingFunction(elapsedTime, startValue.r, (endValue - startValue).r, duration);
        float g = easingFunction(elapsedTime, startValue.g, (endValue - startValue).g, duration);
        float b = easingFunction(elapsedTime, startValue.b, (endValue - startValue).b, duration);
        float a = easingFunction(elapsedTime, startValue.a, (endValue - startValue).a, duration);
        Color value = new Color(r, g, b, a);
        onUpdate?.Invoke(value);
    }

}