using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class TweenCore : IDisposable {

    SortedList<int, TweenModel> tweens;
    Dictionary<int, TweenValueRangeModel> ranges;
    Dictionary<int, TweenValueModel> values;
    Dictionary<int, Action> onCompleteCallbacks;
    Dictionary<int, Action> onUpdateCallbacks;
    int nextId = 1;

    public TweenCore() {
        tweens = new SortedList<int, TweenModel>();
        ranges = new Dictionary<int, TweenValueRangeModel>();
        onCompleteCallbacks = new Dictionary<int, Action>();
        onUpdateCallbacks = new Dictionary<int, Action>();
    }

    #region Create Tweens
    public int Create(float start, float end, float duration, EasingType easing, bool isLoop = false) {
        int id = AllocateTween(TweenType.Float, duration, easing, isLoop);
        var range = new TweenValueRangeModel();
        range.floatStart = start;
        range.floatEnd = end;
        ranges.Add(tweens[id].id, range);
        var value = new TweenValueModel();
        value.floatValue = start;
        values.Add(tweens[id].id, value);
        return id;
    }

    public int Create(Vector2 start, Vector2 end, float duration, EasingType easing, bool isLoop = false) {
        int id = AllocateTween(TweenType.Vector2, duration, easing, isLoop);
        var range = new TweenValueRangeModel();
        range.vector2Start = start;
        range.vector2End = end;
        ranges.Add(tweens[id].id, range);
        var value = new TweenValueModel();
        value.vector2Value = start;
        values.Add(tweens[id].id, value);
        return id;
    }

    public int Create(Vector3 start, Vector3 end, float duration, EasingType easing, bool isLoop = false) {
        int id = AllocateTween(TweenType.Vector3, duration, easing, isLoop);
        var range = new TweenValueRangeModel();
        range.vector3Start = start;
        range.vector3End = end;
        ranges.Add(tweens[id].id, range);
        var value = new TweenValueModel();
        value.vector3Value = start;
        values.Add(tweens[id].id, value);
        return id;
    }

    public int Create(Color start, Color end, float duration, EasingType easing, bool isLoop = false) {
        int id = AllocateTween(TweenType.Color, duration, easing, isLoop);
        var range = new TweenValueRangeModel();
        range.colorStart = start;
        range.colorEnd = end;
        ranges.Add(tweens[id].id, range);
        var value = new TweenValueModel();
        value.colorValue = start;
        values.Add(tweens[id].id, value);
        return id;
    }

    public int Create(Color32 start, Color32 end, float duration, EasingType easing, bool isLoop = false) {
        int id = AllocateTween(TweenType.Color32, duration, easing, isLoop);
        var range = new TweenValueRangeModel();
        range.color32Start = start;
        range.color32End = end;
        ranges.Add(tweens[id].id, range);
        var value = new TweenValueModel();
        value.color32Value = start;
        values.Add(tweens[id].id, value);
        return id;
    }

    public int Create(Quaternion start, Quaternion end, float duration, EasingType easing, bool isLoop = false) {
        int id = AllocateTween(TweenType.Quaternion, duration, easing, isLoop);
        var range = new TweenValueRangeModel();
        range.quaternionStart = start;
        range.quaternionEnd = end;
        ranges.Add(tweens[id].id, range);
        var value = new TweenValueModel();
        value.quaternionValue = start;
        values.Add(tweens[id].id, value);
        return id;
    }

    public int CreateTween_Wait(float duration, bool isLoop = false) {
        return AllocateTween(TweenType.Wait, duration, EasingType.Linear, isLoop);
    }

    int AllocateTween(TweenType type, float duration, EasingType easing, bool isLoop) {
        var model = new TweenModel();

        model.id = nextId++;
        model.type = type;
        model.duration = duration;
        model.easing = easing;
        model.isLoop = isLoop;
        model.elapsedTime = 0;
        model.isPlaying = true;
        model.isComplete = false;
        model.nextId = -1;

        tweens.Add(model.id, model);

        return model.id;
    }
    #endregion

    #region Callbacks
    public void OnComplete(int tweenId, Action callback) {
        if (onCompleteCallbacks.ContainsKey(tweenId)) {
            onCompleteCallbacks[tweenId] += callback;
        } else {
            onCompleteCallbacks[tweenId] = callback;
        }
    }

    public void OnUpdate(int tweenId, Action callback) {
        if (onUpdateCallbacks.ContainsKey(tweenId)) {
            onUpdateCallbacks[tweenId] += callback;
        } else {
            onUpdateCallbacks[tweenId] = callback;
        }
    }
    #endregion

    // 每帧更新
    public void Update(float deltaTime) {
        for (int i = 0; i < tweens.Values.Count; i++) {
            var tween = tweens[i];
            int id = tween.id;

            if (!tween.isPlaying) continue;

            tween.elapsedTime += deltaTime;

            // 处理更新
            if (tween.elapsedTime >= tween.duration) {
                tween.elapsedTime = tween.duration;
                tween.isPlaying = false;
                tween.isComplete = true;

                // 触发完成回调
                if (onCompleteCallbacks.TryGetValue(id, out var completeCallback)) {
                    completeCallback?.Invoke();
                }

                if (tween.isLoop) {
                    Restart(id);
                } else if (tween.nextId != -1) {
                    Play(tween.nextId);
                }
            } else {
                // 触发更新回调
                if (onUpdateCallbacks.TryGetValue(id, out var updateCallback)) {
                    updateCallback?.Invoke();
                }

                // 计算当前值
                switch (tween.type) {
                    case TweenType.Float:
                        UpdateFloat(id);
                        break;
                    case TweenType.Vector2:
                        UpdateVector2(id);
                        break;
                    case TweenType.Vector3:
                        UpdateVector3(id);
                        break;
                    case TweenType.Color:
                        UpdateColor(id);
                        break;
                    case TweenType.Color32:
                        UpdateColor32(id);
                        break;
                    case TweenType.Quaternion:
                        UpdateQuaternion(id);
                        break;
                    case TweenType.Wait:
                        break;
                }
            }
            tweens[i] = tween;
        }
    }

    #region Update Tweens
    void UpdateFloat(int id) {
        var tween = tweens[id];
        var range = ranges[id];
        var value = values[id];

        value.floatValue = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.floatStart,
            range.floatEnd - range.floatStart,
            tween.duration);

        values[id] = value;
    }

    void UpdateVector2(int id) {
        var tween = tweens[id];
        var range = ranges[id];
        var value = values[id];

        value.vector2Value.x = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.vector2Start.x,
            range.vector2End.x - range.vector2Start.x,
            tween.duration);
        value.vector2Value.y = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.vector2Start.y,
            range.vector2End.y - range.vector2Start.y,
            tween.duration);

        values[id] = value;
    }

    void UpdateVector3(int id) {
        var tween = tweens[id];
        var range = ranges[id];
        var value = values[id];

        value.vector3Value.x = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.vector3Start.x,
            range.vector3End.x - range.vector3Start.x,
            tween.duration);
        value.vector3Value.y = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.vector3Start.y,
            range.vector3End.y - range.vector3Start.y,
            tween.duration);
        value.vector3Value.z = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.vector3Start.z,
            range.vector3End.z - range.vector3Start.z,
            tween.duration);

        values[id] = value;
    }

    void UpdateColor(int id) {
        var tween = tweens[id];
        var range = ranges[id];
        var value = values[id];

        value.colorValue.r = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.colorStart.r,
            range.colorEnd.r - range.colorStart.r,
            tween.duration);
        value.colorValue.g = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.colorStart.g,
            range.colorEnd.g - range.colorStart.g,
            tween.duration);
        value.colorValue.b = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.colorStart.b,
            range.colorEnd.b - range.colorStart.b,
            tween.duration);
        value.colorValue.a = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.colorStart.a,
            range.colorEnd.a - range.colorStart.a,
            tween.duration);

        values[id] = value;
    }

    void UpdateColor32(int id) {
        var tween = tweens[id];
        var range = ranges[id];
        var value = values[id];

        value.color32Value.r = (byte)EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.color32Start.r,
            range.color32End.r - range.color32Start.r,
            tween.duration);
        value.color32Value.g = (byte)EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.color32Start.g,
            range.color32End.g - range.color32Start.g,
            tween.duration);
        value.color32Value.b = (byte)EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.color32Start.b,
            range.color32End.b - range.color32Start.b,
            tween.duration);
        value.color32Value.a = (byte)EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.color32Start.a,
            range.color32End.a - range.color32Start.a,
            tween.duration);

        values[id] = value;
    }

    void UpdateQuaternion(int id) {
        var tween = tweens[id];
        var range = ranges[id];
        var value = values[id];

        value.quaternionValue.x = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.quaternionStart.x,
            range.quaternionEnd.x - range.quaternionStart.x,
            tween.duration);
        value.quaternionValue.y = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.quaternionStart.y,
            range.quaternionEnd.y - range.quaternionStart.y,
            tween.duration);
        value.quaternionValue.z = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.quaternionStart.z,
            range.quaternionEnd.z - range.quaternionStart.z,
            tween.duration);
        value.quaternionValue.w = EasingFunction_F.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.quaternionStart.w,
            range.quaternionEnd.w - range.quaternionStart.w,
            tween.duration);

        values[id] = value;
    }
    #endregion

    #region Control Tweens
    public void Stop(int id) {
        var tween = tweens[id];
        tween.isPlaying = false;
        tween.isComplete = true;
        tweens[id] = tween;
    }

    void Restart(int id) {
        var tween = tweens[id];
        tween.elapsedTime = 0f;
        tween.isPlaying = true;
        tween.isComplete = false;
        tweens[id] = tween;
    }

    public void Play(int id) {
        var tween = tweens[id];
        if (tween.isPlaying) return;
        tween.isPlaying = true;
        tween.elapsedTime = 0f;
        tween.isComplete = false;
        tweens[id] = tween;
    }

    public void Pause(int id) {
        var tween = tweens[id];
        tween.isPlaying = false;
        tweens[id] = tween;
    }

    public void Resume(int id) {
        var tween = tweens[id];
        if (tween.isPlaying) return;
        tween.isPlaying = true;
        tweens[id] = tween;
    }
    #endregion

    #region Linking Tweens
    public void Link(int fromId, int toId) {
        if (!tweens.ContainsKey(fromId) || !tweens.ContainsKey(toId)) return;

        var fromTween = tweens[fromId];
        fromTween.nextId = toId;
        tweens[fromId] = fromTween;
    }

    public void Unlink(int fromId) {
        if (!tweens.ContainsKey(fromId)) return;

        var fromTween = tweens[fromId];
        fromTween.nextId = -1;
        tweens[fromId] = fromTween;
    }
    #endregion

    public void Dispose() {
        tweens.Clear();
        ranges.Clear();
        values.Clear();
        onCompleteCallbacks.Clear();
        onUpdateCallbacks.Clear();
        nextId = 1;
    }
}