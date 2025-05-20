using System;

internal class TweenModel_F : ITween {

    float startValue;
    float endValue;

    float duration;
    Func<float, float, float, float, float> easingFunction;

    float elapsedTime;
    bool isPlaying;

    internal Action<float> OnUpdate;
    internal Action OnComplete;

    bool isComplete;
    bool ITween.IsComplete => isComplete;

    bool isLoop;

    int nextId;
    int ITween.NextId => nextId;
    void ITween.SetNextId(int id) => nextId = id;

    internal TweenModel_F(float startValue, float endValue, float duration, Func<float, float, float, float, float> easingFunction, bool isLoop) {
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

    void ITween.Play() => Restart();
    void ITween.Pause() => isPlaying = false;
    void ITween.Restart() => Restart();
    void Restart() {
        elapsedTime = 0;
        isPlaying = true;
        isComplete = false;
    }

    void ITween.Tick(float dt) {
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

        float value = easingFunction(elapsedTime, startValue, endValue - startValue, duration);
        OnUpdate?.Invoke(value);
    }

    void ITween.Dispose() {
        OnUpdate = null;
        OnComplete = null;
    }

}