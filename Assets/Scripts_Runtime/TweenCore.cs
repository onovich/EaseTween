using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenCore {

    List<ITween> tweens;
    Queue<ITween> tweensToAdd;

    public TweenCore() {
        tweens = new List<ITween>();
        tweensToAdd = new Queue<ITween>();
    }

    public TweenModel_F Create(float startValue, float endValue, float duration, EasingType easingType, bool isLoop = false) {
        var easingFunction = EasingFunction_F.GetEasingFunction(easingType);
        var tween = new TweenModel_F(startValue, endValue, duration, easingFunction, isLoop);
        tweensToAdd.Enqueue(tween);
        return tween;
    }

    public TweenModel_V2 Create(Vector2 startValue, Vector2 endValue, float duration, EasingType easingType, bool isLoop = false) {
        var easingFunction = EasingFunction_F.GetEasingFunction(easingType);
        var tween = new TweenModel_V2(startValue, endValue, duration, easingFunction, isLoop);
        tweensToAdd.Enqueue(tween);
        return tween;
    }

    public TweenModel_V3 Create(Vector3 startValue, Vector3 endValue, float duration, EasingType easingType, bool isLoop = false) {
        var easingFunction = EasingFunction_F.GetEasingFunction(easingType);
        var tween = new TweenModel_V3(startValue, endValue, duration, easingFunction, isLoop);
        tweensToAdd.Enqueue(tween);
        return tween;
    }

    public TweenModel_C32 Create(Color32 startValue, Color32 endValue, float duration, EasingType easingType, bool isLoop = false) {
        var easingFunction = EasingFunction_F.GetEasingFunction(easingType);
        var tween = new TweenModel_C32(startValue, endValue, duration, easingFunction, isLoop);
        tweensToAdd.Enqueue(tween);
        return tween;
    }

    public TweenModel_C Create(Color startValue, Color endValue, float duration, EasingType easingType, bool isLoop = false) {
        var easingFunction = EasingFunction_F.GetEasingFunction(easingType);
        var tween = new TweenModel_C(startValue, endValue, duration, easingFunction, isLoop);
        tweensToAdd.Enqueue(tween);
        return tween;
    }

    public void Tick(float dt) {
        foreach (var tween in tweens) {
            tween.Tick(dt);
        }
        while (tweensToAdd.Count > 0) {
            tweens.Add(tweensToAdd.Dequeue());
        }
        tweens.RemoveAll(tween => tween.IsComplete);
    }

}