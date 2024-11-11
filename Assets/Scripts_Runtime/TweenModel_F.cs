using System;

public class TweenModel_F : ITween {

    float startValue;
    float endValue;

    float duration;
    Func<float, float, float, float, float> easingFunction;

    float elapsedTime;
    bool isPlaying;

    Action<float> onUpdate;
    Action onComplete;

    bool isComplete;
    public bool IsComplete => isComplete;

    bool isLoop;

    public TweenModel_F(float startValue, float endValue, float duration, Func<float, float, float, float, float> easingFunction, bool isLoop) {
        this.startValue = startValue;
        this.endValue = endValue;
        this.duration = duration;
        this.easingFunction = easingFunction;
        this.isLoop = isLoop;
        this.elapsedTime = 0;
        this.isPlaying = false;
        this.isComplete = false;
    }

    public TweenModel_F OnUpdate(Action<float> onUpdate) {
        this.onUpdate = onUpdate;
        return this;
    }

    public TweenModel_F OnComplete(Action onComplete) {
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

        float value = easingFunction(elapsedTime, startValue, endValue - startValue, duration);
        onUpdate?.Invoke(value);
    }

}