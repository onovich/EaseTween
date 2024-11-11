using System;

public class TweenModel_Wait : ITween {

    float duration;
    Func<float, float, float, float, float> easingFunction;

    float elapsedTime;
    bool isPlaying;

    public Action<float> OnUpdate;
    public Action OnComplete;

    bool isComplete;
    public bool IsComplete => isComplete;

    bool isLoop;

    int nextId;
    public int NextId => nextId;
    public void SetNextId(int id) => nextId = id;

    public TweenModel_Wait(float duration, Func<float, float, float, float, float> easingFunction, bool isLoop) {
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

        float value = easingFunction(elapsedTime, 0, 100, duration);
        OnUpdate?.Invoke(value);
    }

    public void Dispose() {
        OnUpdate = null;
        OnComplete = null;
    }

}