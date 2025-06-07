using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class TweenCore : IDisposable {

    SortedList<int, TweenModel> tweens;
    Dictionary<int, TweenValueRangeModel> ranges;
    Dictionary<int, TweenValueModel> values;
    Dictionary<int, Action<float>> onCompleteCallbacks_float;
    Dictionary<int, Action<float>> onUpdateCallbacks_float;
    Dictionary<int, Action<Vector2>> onCompleteCallbacks_vector2;
    Dictionary<int, Action<Vector2>> onUpdateCallbacks_vector2;
    Dictionary<int, Action<Vector3>> onCompleteCallbacks_vector3;
    Dictionary<int, Action<Vector3>> onUpdateCallbacks_vector3;
    Dictionary<int, Action<Color>> onCompleteCallbacks_color;
    Dictionary<int, Action<Color>> onUpdateCallbacks_color;
    Dictionary<int, Action<Color32>> onCompleteCallbacks_color32;
    Dictionary<int, Action<Color32>> onUpdateCallbacks_color32;
    Dictionary<int, Action<Quaternion>> onCompleteCallbacks_quaternion;
    Dictionary<int, Action<Quaternion>> onUpdateCallbacks_quaternion;
    Dictionary<int, Action> onWaiteCompleteCallbacks;
    int nextId = 1;

    public TweenCore() {
        tweens = new SortedList<int, TweenModel>();
        ranges = new Dictionary<int, TweenValueRangeModel>();
        values = new Dictionary<int, TweenValueModel>();
        onCompleteCallbacks_float = new Dictionary<int, Action<float>>();
        onUpdateCallbacks_float = new Dictionary<int, Action<float>>();
        onCompleteCallbacks_vector2 = new Dictionary<int, Action<Vector2>>();
        onUpdateCallbacks_vector2 = new Dictionary<int, Action<Vector2>>();
        onCompleteCallbacks_vector3 = new Dictionary<int, Action<Vector3>>();
        onUpdateCallbacks_vector3 = new Dictionary<int, Action<Vector3>>();
        onCompleteCallbacks_color = new Dictionary<int, Action<Color>>();
        onUpdateCallbacks_color = new Dictionary<int, Action<Color>>();
        onCompleteCallbacks_color32 = new Dictionary<int, Action<Color32>>();
        onUpdateCallbacks_color32 = new Dictionary<int, Action<Color32>>();
        onCompleteCallbacks_quaternion = new Dictionary<int, Action<Quaternion>>();
        onUpdateCallbacks_quaternion = new Dictionary<int, Action<Quaternion>>();
        onWaiteCompleteCallbacks = new Dictionary<int, Action>();
        nextId = 1;
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
    public void OnComplete(int tweenId, Action<float> callback) {
        if (onCompleteCallbacks_float.ContainsKey(tweenId)) {
            onCompleteCallbacks_float[tweenId] += callback;
        } else {
            onCompleteCallbacks_float[tweenId] = callback;
        }
    }

    public void OnUpdate(int tweenId, Action<float> callback) {
        if (onUpdateCallbacks_float.ContainsKey(tweenId)) {
            onUpdateCallbacks_float[tweenId] += callback;
        } else {
            onUpdateCallbacks_float[tweenId] = callback;
        }
    }

    public void OnComplete(int tweenId, Action<Vector2> callback) {
        if (onCompleteCallbacks_vector2.ContainsKey(tweenId)) {
            onCompleteCallbacks_vector2[tweenId] += callback;
        } else {
            onCompleteCallbacks_vector2[tweenId] = callback;
        }
    }

    public void OnUpdate(int tweenId, Action<Vector2> callback) {
        if (onUpdateCallbacks_vector2.ContainsKey(tweenId)) {
            onUpdateCallbacks_vector2[tweenId] += callback;
        } else {
            onUpdateCallbacks_vector2[tweenId] = callback;
        }
    }

    public void OnComplete(int tweenId, Action<Vector3> callback) {
        if (onCompleteCallbacks_vector3.ContainsKey(tweenId)) {
            onCompleteCallbacks_vector3[tweenId] += callback;
        } else {
            onCompleteCallbacks_vector3[tweenId] = callback;
        }
    }

    public void OnUpdate(int tweenId, Action<Vector3> callback) {
        if (onUpdateCallbacks_vector3.ContainsKey(tweenId)) {
            onUpdateCallbacks_vector3[tweenId] += callback;
        } else {
            onUpdateCallbacks_vector3[tweenId] = callback;
        }
    }

    public void OnComplete(int tweenId, Action<Color> callback) {
        if (onCompleteCallbacks_color.ContainsKey(tweenId)) {
            onCompleteCallbacks_color[tweenId] += callback;
        } else {
            onCompleteCallbacks_color[tweenId] = callback;
        }
    }

    public void OnUpdate(int tweenId, Action<Color> callback) {
        if (onUpdateCallbacks_color.ContainsKey(tweenId)) {
            onUpdateCallbacks_color[tweenId] += callback;
        } else {
            onUpdateCallbacks_color[tweenId] = callback;
        }
    }

    public void OnComplete(int tweenId, Action<Color32> callback) {
        if (onCompleteCallbacks_color32.ContainsKey(tweenId)) {
            onCompleteCallbacks_color32[tweenId] += callback;
        } else {
            onCompleteCallbacks_color32[tweenId] = callback;
        }
    }

    public void OnUpdate(int tweenId, Action<Color32> callback) {
        if (onUpdateCallbacks_color32.ContainsKey(tweenId)) {
            onUpdateCallbacks_color32[tweenId] += callback;
        } else {
            onUpdateCallbacks_color32[tweenId] = callback;
        }
    }

    public void OnComplete(int tweenId, Action<Quaternion> callback) {
        if (onCompleteCallbacks_quaternion.ContainsKey(tweenId)) {
            onCompleteCallbacks_quaternion[tweenId] += callback;
        } else {
            onCompleteCallbacks_quaternion[tweenId] = callback;
        }
    }

    public void OnUpdate(int tweenId, Action<Quaternion> callback) {
        if (onUpdateCallbacks_quaternion.ContainsKey(tweenId)) {
            onUpdateCallbacks_quaternion[tweenId] += callback;
        } else {
            onUpdateCallbacks_quaternion[tweenId] = callback;
        }
    }

    public void OnWaitComplete(int tweenId, Action callback) {
        if (onWaiteCompleteCallbacks.ContainsKey(tweenId)) {
            onWaiteCompleteCallbacks[tweenId] += callback;
        } else {
            onWaiteCompleteCallbacks[tweenId] = callback;
        }
    }

    void OnComplete(int tweenId) {
        var tween = tweens[tweenId];
        var type = tween.type;
        switch (type) {
            case TweenType.Float:
                if (onCompleteCallbacks_float.TryGetValue(tweenId, out var callback_float)) {
                    callback_float?.Invoke(values[tweenId].floatValue);
                }
                break;
            case TweenType.Vector2:
                if (onCompleteCallbacks_vector2.TryGetValue(tweenId, out var callback_vector2)) {
                    callback_vector2?.Invoke(values[tweenId].vector2Value);
                }
                break;
            case TweenType.Vector3:
                if (onCompleteCallbacks_vector3.TryGetValue(tweenId, out var callback_vector3)) {
                    callback_vector3?.Invoke(values[tweenId].vector3Value);
                }
                break;
            case TweenType.Color:
                if (onCompleteCallbacks_color.TryGetValue(tweenId, out var callback_color)) {
                    callback_color?.Invoke(values[tweenId].colorValue);
                }
                break;
            case TweenType.Color32:

                if (onCompleteCallbacks_color32.TryGetValue(tweenId, out var callback_color32)) {
                    callback_color32?.Invoke(values[tweenId].color32Value);
                }
                break;
            case TweenType.Quaternion:
                if (onCompleteCallbacks_quaternion.TryGetValue(tweenId, out var callback_quaternion)) {
                    callback_quaternion?.Invoke(values[tweenId].quaternionValue);
                }
                break;
            case TweenType.Wait:
                if (onWaiteCompleteCallbacks.TryGetValue(tweenId, out var callback_wait)) {
                    callback_wait?.Invoke();
                }
                break;
            default:
                Debug.LogWarning($"Unsupported tween type: {type}");
                break;
        }
    }

    void OnUpdate(int tweenId) {
        var tween = tweens[tweenId];
        var type = tween.type;
        switch (type) {
            case TweenType.Float:
                if (onUpdateCallbacks_float.TryGetValue(tweenId, out var callback_float)) {
                    callback_float?.Invoke(values[tweenId].floatValue);
                }
                break;
            case TweenType.Vector2:
                if (onUpdateCallbacks_vector2.TryGetValue(tweenId, out var callback_vector2)) {
                    callback_vector2?.Invoke(values[tweenId].vector2Value);
                }
                break;
            case TweenType.Vector3:
                if (onUpdateCallbacks_vector3.TryGetValue(tweenId, out var callback_vector3)) {
                    callback_vector3?.Invoke(values[tweenId].vector3Value);
                }
                break;
            case TweenType.Color:
                if (onUpdateCallbacks_color.TryGetValue(tweenId, out var callback_color)) {
                    callback_color?.Invoke(values[tweenId].colorValue);
                }
                break;
            case TweenType.Color32:
                if (onUpdateCallbacks_color32.TryGetValue(tweenId, out var callback_color32)) {
                    callback_color32?.Invoke(values[tweenId].color32Value);
                }
                break;
            case TweenType.Quaternion:
                if (onUpdateCallbacks_quaternion.TryGetValue(tweenId, out var callback_quaternion)) {
                    callback_quaternion?.Invoke(values[tweenId].quaternionValue);
                }
                break;
            case TweenType.Wait:
                break;
            default:
                Debug.LogWarning($"Unsupported tween type: {type}");
                break;
        }
    }
    #endregion

    #region Tick
    public void Tick(float deltaTime) {
        for (int i = 0; i < tweens.Values.Count; i++) {
            var tween = tweens.Values[i];
            int id = tween.id;

            if (!tween.isPlaying) continue;

            tween.elapsedTime += deltaTime;

            // 处理更新
            if (tween.elapsedTime >= tween.duration) {
                tween.elapsedTime = tween.duration;
                tween.isPlaying = false;
                tween.isComplete = true;

                // 触发完成回调
                OnComplete(id);

                if (tween.isLoop) {
                    Restart(id);
                } else if (tween.nextId != -1) {
                    Play(tween.nextId);
                }
            } else {
                // 触发更新回调
                OnUpdate(id);

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
    #endregion

    #region Update Tweens
    void UpdateFloat(int id) {
        var tween = tweens[id];
        var range = ranges[id];
        var value = values[id];

        value.floatValue = EasingFunction.GetEasingFunction(tween.easing)(
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

        value.vector2Value.x = EasingFunction.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.vector2Start.x,
            range.vector2End.x - range.vector2Start.x,
            tween.duration);
        value.vector2Value.y = EasingFunction.GetEasingFunction(tween.easing)(
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

        value.vector3Value.x = EasingFunction.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.vector3Start.x,
            range.vector3End.x - range.vector3Start.x,
            tween.duration);
        value.vector3Value.y = EasingFunction.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.vector3Start.y,
            range.vector3End.y - range.vector3Start.y,
            tween.duration);
        value.vector3Value.z = EasingFunction.GetEasingFunction(tween.easing)(
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

        value.colorValue.r = EasingFunction.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.colorStart.r,
            range.colorEnd.r - range.colorStart.r,
            tween.duration);
        value.colorValue.g = EasingFunction.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.colorStart.g,
            range.colorEnd.g - range.colorStart.g,
            tween.duration);
        value.colorValue.b = EasingFunction.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.colorStart.b,
            range.colorEnd.b - range.colorStart.b,
            tween.duration);
        value.colorValue.a = EasingFunction.GetEasingFunction(tween.easing)(
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

        value.color32Value.r = (byte)EasingFunction.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.color32Start.r,
            range.color32End.r - range.color32Start.r,
            tween.duration);
        value.color32Value.g = (byte)EasingFunction.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.color32Start.g,
            range.color32End.g - range.color32Start.g,
            tween.duration);
        value.color32Value.b = (byte)EasingFunction.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.color32Start.b,
            range.color32End.b - range.color32Start.b,
            tween.duration);
        value.color32Value.a = (byte)EasingFunction.GetEasingFunction(tween.easing)(
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

        value.quaternionValue.x = EasingFunction.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.quaternionStart.x,
            range.quaternionEnd.x - range.quaternionStart.x,
            tween.duration);
        value.quaternionValue.y = EasingFunction.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.quaternionStart.y,
            range.quaternionEnd.y - range.quaternionStart.y,
            tween.duration);
        value.quaternionValue.z = EasingFunction.GetEasingFunction(tween.easing)(
            tween.elapsedTime,
            range.quaternionStart.z,
            range.quaternionEnd.z - range.quaternionStart.z,
            tween.duration);
        value.quaternionValue.w = EasingFunction.GetEasingFunction(tween.easing)(
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
        onCompleteCallbacks_float.Clear();
        onUpdateCallbacks_float.Clear();
        onCompleteCallbacks_vector2.Clear();
        onUpdateCallbacks_vector2.Clear();
        onCompleteCallbacks_vector3.Clear();
        onUpdateCallbacks_vector3.Clear();
        onCompleteCallbacks_color.Clear();
        onUpdateCallbacks_color.Clear();
        onCompleteCallbacks_color32.Clear();
        onUpdateCallbacks_color32.Clear();
        onCompleteCallbacks_quaternion.Clear();
        onUpdateCallbacks_quaternion.Clear();
        onWaiteCompleteCallbacks.Clear();
        tweens = null;
        ranges = null;
        values = null;
        onCompleteCallbacks_float = null;
        onUpdateCallbacks_float = null;
        onCompleteCallbacks_vector2 = null;
        onUpdateCallbacks_vector2 = null;
        onCompleteCallbacks_vector3 = null;
        onUpdateCallbacks_vector3 = null;
        onCompleteCallbacks_color = null;
        onUpdateCallbacks_color = null;
        onCompleteCallbacks_color32 = null;
        onUpdateCallbacks_color32 = null;
        onCompleteCallbacks_quaternion = null;
        onUpdateCallbacks_quaternion = null;
        onWaiteCompleteCallbacks = null;
        nextId = 1;
    }
}