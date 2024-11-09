using System;

public class TweenModel {

    float duration;
    TweenType tweenType;
    Func<float, float> easingFunction;

    float elapsedTime;
    bool isPlaying;

    float waitTime;
    bool hasWaited;

    Action<float> onUpdate;
    Action onComplete;
    Action onWaitFor;

    public TweenModel(float duration, TweenType tweenType, Func<float, float> easingFunction) {
        this.duration = duration;
        this.tweenType = tweenType;
        this.easingFunction = easingFunction;
    }

    public TweenModel OnUpdate(Action<float> onUpdate) {
        this.onUpdate = onUpdate;
        return this;
    }

    public TweenModel OnComplete(Action onComplete) {
        this.onComplete = onComplete;
        return this;
    }

    public TweenModel WaitFor(float waitTime, Action onWaitFor) {
        this.waitTime = waitTime;
        this.onWaitFor = onWaitFor;
        return this;
    }

    public void Play() => isPlaying = true;
    public void Pause() => isPlaying = false;
    public void Restart() {
        elapsedTime = 0;
        isPlaying = true;
    }

    public void Tick(float dt) {
        TickWait(dt);
        TickPlay(dt);
    }

    void TickWait(float dt) {
        if (hasWaited) return;

        waitTime -= dt;
        if (waitTime <= 0) {
            waitTime = 0;
            hasWaited = true;
            onWaitFor?.Invoke();
        }
    }

    void TickPlay(float dt) {
        if (!isPlaying) return;

        elapsedTime += dt;
        if (elapsedTime >= duration) {
            elapsedTime = duration;
            isPlaying = false;
            onComplete?.Invoke();
        }

        float t = elapsedTime / duration;
        float value = easingFunction(t);
        onUpdate?.Invoke(value);
    }

}