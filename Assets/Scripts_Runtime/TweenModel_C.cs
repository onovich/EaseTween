using System;
using UnityEngine;

public class TweenModel_C : ITween {

    Color startValue;
    Color endValue;

    float duration;
    Func<float, float, float, float, float> easingFunction;

    float elapsedTime;
    bool isPlaying;

    public Action<Color> OnUpdate;
    public Action OnComplete;

    bool isComplete;
    public bool IsComplete => isComplete;

    bool isLoop;

    int nextId;
    public int NextId => nextId;
    public void SetNextId(int id) => nextId = id;

    public TweenModel_C(Color startValue, Color endValue, float duration, Func<float, float, float, float, float> easingFunction, bool isLoop) {
        this.startValue = startValue;
        this.endValue = endValue;
        this.duration = duration;
        this.easingFunction = easingFunction;
        this.isLoop = isLoop;
        this.elapsedTime = 0;
        this.isPlaying = true;
        this.isComplete = false;
        nextId = -1;
    }

    public void Play() => Restart();
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
            OnComplete?.Invoke();
            isComplete = true;

            if (isLoop) {
                Restart();
            }
            return;
        }

        float r = easingFunction(elapsedTime, startValue.r, endValue.r - startValue.r, duration);
        float g = easingFunction(elapsedTime, startValue.g, endValue.g - startValue.g, duration);
        float b = easingFunction(elapsedTime, startValue.b, endValue.b - startValue.b, duration);
        float a = easingFunction(elapsedTime, startValue.a, endValue.a - startValue.a, duration);
        Color value = new Color(r, g, b, a);
        OnUpdate?.Invoke(value);
    }

    public void Dispose() {
        OnUpdate = null;
        OnComplete = null;
    }

}