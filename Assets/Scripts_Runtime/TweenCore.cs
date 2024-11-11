using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class TweenCore {

    List<TweenModel> tweens;

    public TweenCore() {
        tweens = new List<TweenModel>();
    }

    public TweenModel Create(float startValue, float endValue, float duration, EasingType easingType, bool isLoop = false) {
        var easingFunction = EasingFunction.GetEasingFunction(easingType);
        var tween = new TweenModel(startValue, endValue, duration, easingFunction, isLoop);
        tweens.Add(tween);
        return tween;
    }

    public void TryPause(TweenModel tween) {
        if (tween != null) {
            tween.Pause();
        }
    }

    public void TryRestart(TweenModel tween) {
        if (tween != null) {
            tween.Restart();
        }
    }

    public void TryPlay(TweenModel tween) {
        if (tween != null) {
            tween.Play();
        }
    }

    public void Tick(float dt) {
        foreach (var tween in tweens) {
            tween.Tick(dt);
        }
        tweens.RemoveAll(tween => tween.IsComplete);
    }

}