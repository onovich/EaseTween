using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenCore {

    Dictionary<int, ITween> all;
    List<int> task;
    List<int> toAdd;
    List<int> toStop;

    int nextTweenId;

    public TweenCore() {
        all = new Dictionary<int, ITween>();
        task = new List<int>();
        toStop = new List<int>();
        toAdd = new List<int>();
        nextTweenId = 0;
    }

    public void Link(int tweenId, int nextTweenId) {
        var tween = all[tweenId];
        tween.SetNextId(nextTweenId);
    }

    public void ListenComplete(int tweenId, System.Action onComplete) {
        var tween = all[tweenId];
        if (tween is TweenModel_F tweenF) {
            tweenF.OnComplete += onComplete;
        } else if (tween is TweenModel_V2 tweenV2) {
            tweenV2.OnComplete += onComplete;
        } else if (tween is TweenModel_V3 tweenV3) {
            tweenV3.OnComplete += onComplete;
        } else if (tween is TweenModel_C32 tweenC32) {
            tweenC32.OnComplete += onComplete;
        } else if (tween is TweenModel_C tweenC) {
            tweenC.OnComplete += onComplete;
        } else {
            Debug.LogError("TweenModel is not found");
        }
    }

    public void ListenTick(int tweenId, System.Action<float> onUpdate) {
        var tween = all[tweenId];
        if (tween is TweenModel_F tweenV2) {
            tweenV2.OnUpdate += onUpdate;
        } else {
            Debug.LogError("TweenModel_F is not found");
        }
    }

    public void ListenTick(int tweenId, System.Action<Vector2> onUpdate) {
        var tween = all[tweenId];
        if (tween is TweenModel_V2 tweenV2) {
            tweenV2.OnUpdate += onUpdate;
        } else {
            Debug.LogError("TweenModel_V2 is not found");
        }
    }

    public void ListenTick(int tweenId, System.Action<Vector3> onUpdate) {
        var tween = all[tweenId];
        if (tween is TweenModel_V3 tweenV3) {
            tweenV3.OnUpdate += onUpdate;
        } else {
            Debug.LogError("TweenModel_V3 is not found");
        }
    }

    public void ListenTick(int tweenId, System.Action<Color32> onUpdate) {
        var tween = all[tweenId];
        if (tween is TweenModel_C32 tweenC32) {
            tweenC32.OnUpdate += onUpdate;
        } else {
            Debug.LogError("TweenModel_C32 is not found");
        }
    }

    public void ListenTick(int tweenId, System.Action<Color> onUpdate) {
        var tween = all[tweenId];
        if (tween is TweenModel_C tweenC) {
            tweenC.OnUpdate += onUpdate;
        } else {
            Debug.LogError("TweenModel_C is not found");
        }
    }

    public int Create(float startValue, float endValue, float duration, EasingType easingType, bool isLoop = false) {
        var easingFunction = EasingFunction_F.GetEasingFunction(easingType);
        var tween = new TweenModel_F(startValue, endValue, duration, easingFunction, isLoop);
        nextTweenId++;
        all.Add(nextTweenId, tween);
        return nextTweenId;
    }

    public int Create(float duration, EasingType easingType) {
        var easingFunction = EasingFunction_F.GetEasingFunction(easingType);
        var tween = new TweenModel_Wait(duration, easingFunction, false);
        nextTweenId++;
        all.Add(nextTweenId, tween);
        return nextTweenId;
    }

    public int Create(Vector2 startValue, Vector2 endValue, float duration, EasingType easingType, bool isLoop = false) {
        var easingFunction = EasingFunction_F.GetEasingFunction(easingType);
        var tween = new TweenModel_V2(startValue, endValue, duration, easingFunction, isLoop);
        nextTweenId++;
        all.Add(nextTweenId, tween);
        return nextTweenId;
    }

    public int Create(Vector3 startValue, Vector3 endValue, float duration, EasingType easingType, bool isLoop = false) {
        var easingFunction = EasingFunction_F.GetEasingFunction(easingType);
        var tween = new TweenModel_V3(startValue, endValue, duration, easingFunction, isLoop);
        nextTweenId++;
        all.Add(nextTweenId, tween);
        return nextTweenId;
    }

    public int Create(Color32 startValue, Color32 endValue, float duration, EasingType easingType, bool isLoop = false) {
        var easingFunction = EasingFunction_F.GetEasingFunction(easingType);
        var tween = new TweenModel_C32(startValue, endValue, duration, easingFunction, isLoop);
        nextTweenId++;
        all.Add(nextTweenId, tween);
        return nextTweenId;
    }

    public int Create(Color startValue, Color endValue, float duration, EasingType easingType, bool isLoop = false) {
        var easingFunction = EasingFunction_F.GetEasingFunction(easingType);
        var tween = new TweenModel_C(startValue, endValue, duration, easingFunction, isLoop);
        nextTweenId++;
        all.Add(nextTweenId, tween);
        return nextTweenId;
    }

    public void Play(int id) {
        var tween = all[id];
        tween.Play();
        task.Add(id);
    }

    public void Pause(int id) {
        var tween = all[id];
        tween.Pause();
    }

    public void Remove(int id) {
        var tween = all[id];
        tween.Pause();
        toStop.Add(id);
    }

    public void Reset(int id) {
        var tween = all[id];
        tween.Restart();
    }

    public void ResetAll() {
        foreach (var tween in all.Values) {
            tween.Restart();
        }
    }

    public void Dispose() {
        foreach (var tween in all.Values) {
            tween.Dispose();
        }
        all.Clear();
        task.Clear();
        toAdd.Clear();
        toStop.Clear();
        nextTweenId = 0;
    }

    public void Tick(float dt) {
        foreach (var id in task) {
            var tween = all[id];
            tween.Tick(dt);
            if (tween.IsComplete) {
                toStop.Add(id);
                if (tween.NextId != -1) {
                    toAdd.Add(tween.NextId);
                }
            }
        }

        foreach (var id in toAdd) {
            task.Add(id);
            var tween = all[id];
            tween.Play();
        }

        foreach (var id in toStop) {
            task.Remove(id);
        }
        toAdd.Clear();
        toStop.Clear();
    }

}