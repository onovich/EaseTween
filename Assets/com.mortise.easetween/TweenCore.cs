using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public sealed class TweenCore : IDisposable {
    private NativeArray<TweenModel> activeTweens;
    private int tweenCount = 0;
    private int nextId = 1;

    private Dictionary<int, int> idToIndex = new Dictionary<int, int>();
    private Queue<int> freeIndices = new Queue<int>();

    private Dictionary<int, Delegate> updateCallbacks = new Dictionary<int, Delegate>();
    private Dictionary<int, Delegate> completeCallbacks = new Dictionary<int, Delegate>();

    public TweenCore(int capacity = 128) {
        activeTweens = new NativeArray<TweenModel>(capacity, Allocator.Persistent);
    }

    #region 创建Tween
    public int Create(float start, float end, float duration, EasingType easing, bool isLoop = false) {
        int id = nextId++;
        int index = AllocateIndex();

        TweenModel tween = new TweenModel {
            id = id,
            type = TweenType.Float,
            floatStart = start,
            floatEnd = end,
            floatValue = start,
            duration = duration,
            easing = easing,
            isLoop = isLoop
        };

        activeTweens[index] = tween;
        idToIndex[id] = index;
        return id;
    }

    public int Create(Vector3 start, Vector3 end, float duration, EasingType easing, bool isLoop = false) {
        int id = nextId++;
        int index = AllocateIndex();

        TweenModel tween = new TweenModel {
            id = id,
            type = TweenType.Vector3,
            vector3Start = start,
            vector3End = end,
            vector3Value = start,
            duration = duration,
            easing = easing,
            isLoop = isLoop
        };

        activeTweens[index] = tween;
        idToIndex[id] = index;
        return id;
    }

    public int Create(Vector2 start, Vector2 end, float duration, EasingType easing, bool isLoop = false) {
        int id = nextId++;
        int index = AllocateIndex();

        TweenModel tween = new TweenModel {
            id = id,
            type = TweenType.Vector2,
            vector2Start = start,
            vector2End = end,
            vector2Value = start,
            duration = duration,
            easing = easing,
            isLoop = isLoop
        };

        activeTweens[index] = tween;
        idToIndex[id] = index;
        return id;
    }

    public int Create(Color start, Color end, float duration, EasingType easing, bool isLoop = false) {
        int id = nextId++;
        int index = AllocateIndex();

        TweenModel tween = new TweenModel {
            id = id,
            type = TweenType.Color,
            colorStart = start,
            colorEnd = end,
            colorValue = start,
            duration = duration,
            easing = easing,
            isLoop = isLoop
        };

        activeTweens[index] = tween;
        idToIndex[id] = index;
        return id;
    }

    public int Create(Color32 start, Color32 end, float duration, EasingType easing, bool isLoop = false) {
        int id = nextId++;
        int index = AllocateIndex();

        TweenModel tween = new TweenModel {
            id = id,
            type = TweenType.Color32,
            color32Start = start,
            color32End = end,
            color32Value = start,
            duration = duration,
            easing = easing,
            isLoop = isLoop
        };

        activeTweens[index] = tween;
        idToIndex[id] = index;
        return id;
    }

    public int Create(Quaternion start, Quaternion end, float duration, EasingType easing, bool isLoop = false) {
        int id = nextId++;
        int index = AllocateIndex();

        TweenModel tween = new TweenModel {
            id = id,
            type = TweenType.Quaternion,
            quaternionStart = start,
            quaternionEnd = end,
            quaternionValue = start,
            duration = duration,
            easing = easing,
            isLoop = isLoop
        };

        activeTweens[index] = tween;
        idToIndex[id] = index;
        return id;
    }

    public int CreateWait(float duration) {
        int id = nextId++;
        int index = AllocateIndex();

        TweenModel tween = new TweenModel {
            id = id,
            type = TweenType.Wait,
            duration = duration,
        };

        activeTweens[index] = tween;
        idToIndex[id] = index;
        return id;
    }
    #endregion

    #region 更新逻辑
    public void Tick(float deltaTime) {
        if (tweenCount == 0) return;

        var job = new TweenUpdateJob {
            tweens = activeTweens,
            deltaTime = deltaTime
        };

        JobHandle handle = job.Schedule(tweenCount, 32);
        handle.Complete();

        for (int i = 0; i < tweenCount; i++) {
            TweenModel t = activeTweens[i];

            if ((t.flags & 0x2) != 0) // HasChanged
            {
                t.flags &= 0xFD; // Clear HasChanged Flag

                if (updateCallbacks.TryGetValue(t.id, out Delegate updateDel)) {
                    InvokeCallback(updateDel, t);
                }
            }

            if ((t.flags & 0x1) != 0) // NeedsChainStart
            {
                t.flags &= 0xFE; // Clear NeedsChainStart Flag
                if (t.nextId != -1 && idToIndex.TryGetValue(t.nextId, out int nextIndex)) {
                    TweenModel next = activeTweens[nextIndex];
                    next.isPlaying = true;
                    next.elapsedTime = 0f;
                    next.isComplete = false;
                    activeTweens[nextIndex] = next;
                }
            }

            if (t.isComplete && completeCallbacks.TryGetValue(t.id, out Delegate completeDel)) {
                InvokeCallback(completeDel, t);
                if (!t.isLoop) RemoveCallback(t.id);
            }

            activeTweens[i] = t;
        }
    }

    private void InvokeCallback(Delegate callback, TweenModel t) {
        switch (t.type) {
            case TweenType.Float:
                (callback as Action<float>)?.Invoke(t.floatValue);
                break;
            case TweenType.Vector3:
                (callback as Action<Vector3>)?.Invoke(t.vector3Value);
                break;
            case TweenType.Vector2:
                (callback as Action<Vector2>)?.Invoke(t.vector2Value);
                break;
            case TweenType.Color:
                (callback as Action<Color>)?.Invoke(t.colorValue);
                break;
            case TweenType.Color32:
                (callback as Action<Color32>)?.Invoke(t.color32Value);
                break;
            case TweenType.Quaternion:
                (callback as Action<Quaternion>)?.Invoke(t.quaternionValue);
                break;
            case TweenType.Wait:
                (callback as Action)?.Invoke();
                break;
        }
    }
    #endregion

    #region 回调注册
    public void OnUpdate<T>(int tweenId, Action<T> callback) where T : struct {
        if (updateCallbacks.TryGetValue(tweenId, out var existing)) {
            updateCallbacks[tweenId] = Delegate.Combine(existing, callback);
        } else {
            updateCallbacks[tweenId] = callback;
        }
    }

    public void OnComplete<T>(int tweenId, Action<T> callback) where T : struct {
        if (completeCallbacks.TryGetValue(tweenId, out var existing)) {
            completeCallbacks[tweenId] = Delegate.Combine(existing, callback);
        } else {
            completeCallbacks[tweenId] = callback;
        }
    }
    #endregion

    #region 内存管理
    private int AllocateIndex() {
        if (freeIndices.Count > 0) return freeIndices.Dequeue();

        if (tweenCount >= activeTweens.Length) {
            int newSize = activeTweens.Length * 2;
            var newTweens = new NativeArray<TweenModel>(newSize, Allocator.Persistent);
            var newChanged = new NativeArray<int>(newSize, Allocator.Persistent);

            NativeArray<TweenModel>.Copy(activeTweens, newTweens, activeTweens.Length);

            activeTweens.Dispose();

            activeTweens = newTweens;
        }

        return tweenCount++;
    }

    public void Remove(int tweenId) {
        if (!idToIndex.TryGetValue(tweenId, out int index)) return;

        activeTweens[index] = default;
        freeIndices.Enqueue(index);
        idToIndex.Remove(tweenId);
        RemoveCallback(tweenId);
    }

    private void RemoveCallback(int tweenId) {
        updateCallbacks.Remove(tweenId);
        completeCallbacks.Remove(tweenId);
    }

    public void Dispose() {
        if (activeTweens.IsCreated) activeTweens.Dispose();
        idToIndex.Clear();
        freeIndices.Clear();
        updateCallbacks.Clear();
        completeCallbacks.Clear();
    }
    #endregion

    #region 控制方法
    public void Play(int tweenId) {
        if (idToIndex.TryGetValue(tweenId, out int index)) {
            TweenModel t = activeTweens[index];
            t.isPlaying = true;
            t.isComplete = false;
            t.elapsedTime = 0;
            activeTweens[index] = t;
        }
    }

    public void Pause(int tweenId) {
        if (idToIndex.TryGetValue(tweenId, out int index)) {
            TweenModel t = activeTweens[index];
            t.isPlaying = false;
            activeTweens[index] = t;
        }
    }

    public void Stop(int tweenId) {
        if (idToIndex.TryGetValue(tweenId, out int index)) {
            TweenModel t = activeTweens[index];
            t.isPlaying = false;
            t.isComplete = true;
            t.elapsedTime = 0;
            activeTweens[index] = t;
        }
    }

    public void Link(int fromId, int toId) {
        if (idToIndex.TryGetValue(fromId, out int fromIndex) &&
            idToIndex.TryGetValue(toId, out int _)) {
            TweenModel t = activeTweens[fromIndex];
            t.nextId = toId;
            activeTweens[fromIndex] = t;
        }
    }
    #endregion

    #region 状态查询
    public bool IsPlaying(int tweenId) {
        return idToIndex.TryGetValue(tweenId, out int index) && activeTweens[index].isPlaying;
    }

    public bool IsComplete(int tweenId) {
        return idToIndex.TryGetValue(tweenId, out int index) && activeTweens[index].isComplete;
    }

    public float GetProgress(int tweenId) {
        if (idToIndex.TryGetValue(tweenId, out int index)) {
            TweenModel t = activeTweens[index];
            return Mathf.Clamp01(t.elapsedTime / t.duration);
        }
        return 0f;
    }
    #endregion
}