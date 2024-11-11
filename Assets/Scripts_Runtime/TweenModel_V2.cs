using System;
using UnityEngine;

public class TweenModel_V2: ITween {

    Vector2 startValue;
    Vector2 endValue;

    float duration;
    Func<float, float, float, float, float> easingFunction;

    float elapsedTime;
    bool isPlaying;

    Action<Vector2> onUpdate;
    Action onComplete;

    bool isComplete;
    public bool IsComplete => isComplete;

    bool isLoop;

    public TweenModel_V2(Vector2 startValue, Vector2 endValue, float duration, Func<float, float, float, float, float> easingFunction, bool isLoop) {
        this.startValue = startValue;
        this.endValue = endValue;
        this.duration = duration;
        this.easingFunction = easingFunction;
        this.isLoop = isLoop;
        this.elapsedTime = 0;
        this.isPlaying = false;
        this.isComplete = false;
    }

    public TweenModel_V2 OnUpdate(Action<Vector2> onUpdate) {
        this.onUpdate = onUpdate;
        return this;
    }

    public TweenModel_V2 OnComplete(Action onComplete) {
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

        float x = easingFunction(elapsedTime, startValue.x, (endValue - startValue).x, duration);
        float y = easingFunction(elapsedTime, startValue.y, (endValue - startValue).y, duration);
        Vector2 value = new Vector2(x, y);
        onUpdate?.Invoke(value);
    }

}