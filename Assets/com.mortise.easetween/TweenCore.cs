using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;

public sealed class TweenCore : IDisposable {
    NativeArray<TweenModel> activeTweens;
    NativeArray<int> nextIdArray;
    int tweenCount = 0;
    Dictionary<int, int> idToIndex = new Dictionary<int, int>();
    Queue<int> freeIndices = new Queue<int>();
    int nextId = 1;

    Dictionary<int, Delegate> updateCallbacks = new Dictionary<int, Delegate>();
    Dictionary<int, Delegate> completeCallbacks = new Dictionary<int, Delegate>();

    public TweenCore(int initialCapacity = 128) {
        activeTweens = new NativeArray<TweenModel>(initialCapacity, Allocator.Persistent);
        nextIdArray = new NativeArray<int>(initialCapacity, Allocator.Persistent);
        for (int i = 0; i < initialCapacity; i++) {
            nextIdArray[i] = -1;
        }
    }

    #region Create
    public int Create(float start, float end, float duration, EasingType easing, bool isLoop = false) {
        int id = nextId++;
        int index = AllocateIndex();

        TweenModel tween = activeTweens[index];
        tween.id = id;
        tween.type = TweenType.Float;
        tween.floatStart = start;
        tween.floatEnd = end;
        tween.floatValue = start;
        tween.duration = duration;
        tween.easing = easing;
        tween.isLoop = isLoop;
        activeTweens[index] = tween;

        idToIndex[id] = index;
        return id;
    }

    public int Create(Vector3 start, Vector3 end, float duration, EasingType easing, bool isLoop = false) {
        int id = nextId++;
        int index = AllocateIndex();

        TweenModel tween = activeTweens[index];
        tween.id = id;
        tween.type = TweenType.Vector3;
        tween.vector3Start = start;
        tween.vector3End = end;
        tween.vector3Value = start;
        tween.duration = duration;
        tween.easing = easing;
        tween.isLoop = isLoop;
        activeTweens[index] = tween;

        idToIndex[id] = index;
        return id;
    }

    public int Create(Color start, Color end, float duration, EasingType easing, bool isLoop = false) {
        int id = nextId++;
        int index = AllocateIndex();

        TweenModel tween = activeTweens[index];
        tween.id = id;
        tween.type = TweenType.Color;
        tween.colorStart = start;
        tween.colorEnd = end;
        tween.colorValue = start;
        tween.duration = duration;
        tween.easing = easing;
        tween.isLoop = isLoop;
        activeTweens[index] = tween;

        idToIndex[id] = index;
        return id;
    }

    public int Create(Quaternion start, Quaternion end, float duration, EasingType easing, bool isLoop = false) {
        int id = nextId++;
        int index = AllocateIndex();

        TweenModel tween = activeTweens[index];
        tween.id = id;
        tween.type = TweenType.Quaternion;
        tween.quaternionStart = start;
        tween.quaternionEnd = end;
        tween.quaternionValue = start;
        tween.duration = duration;
        tween.easing = easing;
        tween.isLoop = isLoop;
        activeTweens[index] = tween;

        idToIndex[id] = index;
        return id;
    }

    public int Create(Color32 start, Color32 end, float duration, EasingType easing, bool isLoop = false) {
        int id = nextId++;
        int index = AllocateIndex();

        TweenModel tween = activeTweens[index];
        tween.id = id;
        tween.type = TweenType.Color32;
        tween.color32Start = start;
        tween.color32End = end;
        tween.color32Value = start;
        tween.duration = duration;
        tween.easing = easing;
        tween.isLoop = isLoop;
        activeTweens[index] = tween;

        idToIndex[id] = index;
        return id;
    }
    #endregion

    #region Job
    [BurstCompile]
    struct TweenUpdateJob : IJobParallelFor {
        public NativeArray<TweenModel> Tweens;
        public NativeArray<int> NextIds;
        public float DeltaTime;

        public void Execute(int index) {
            TweenModel t = Tweens[index];
            if (!t.isPlaying || t.isComplete) return;

            t.elapsedTime += DeltaTime;

            switch (t.type) {
                case TweenType.Float:
                    t.floatValue = EasingFunction.Easing(t.easing, t.elapsedTime, t.floatStart, t.floatEnd - t.floatStart, t.duration);
                    break;

                case TweenType.Vector2:
                    t.vector2Value.x = EasingFunction.Easing(t.easing, t.elapsedTime, t.vector2Start.x, t.vector2End.x - t.vector2Start.x, t.duration);
                    t.vector2Value.y = EasingFunction.Easing(t.easing, t.elapsedTime, t.vector2Start.y, t.vector2End.y - t.vector2Start.y, t.duration);
                    break;

                case TweenType.Vector3:
                    t.vector3Value.x = EasingFunction.Easing(t.easing, t.elapsedTime, t.vector3Start.x, t.vector3End.x - t.vector3Start.x, t.duration);
                    t.vector3Value.y = EasingFunction.Easing(t.easing, t.elapsedTime, t.vector3Start.y, t.vector3End.y - t.vector3Start.y, t.duration);
                    t.vector3Value.z = EasingFunction.Easing(t.easing, t.elapsedTime, t.vector3Start.z, t.vector3End.z - t.vector3Start.z, t.duration);
                    break;

                case TweenType.Color:
                    t.colorValue.r = EasingFunction.Easing(t.easing, t.elapsedTime, t.colorStart.r, t.colorEnd.r - t.colorStart.r, t.duration);
                    t.colorValue.g = EasingFunction.Easing(t.easing, t.elapsedTime, t.colorStart.g, t.colorEnd.g - t.colorStart.g, t.duration);
                    t.colorValue.b = EasingFunction.Easing(t.easing, t.elapsedTime, t.colorStart.b, t.colorEnd.b - t.colorStart.b, t.duration);
                    t.colorValue.a = EasingFunction.Easing(t.easing, t.elapsedTime, t.colorStart.a, t.colorEnd.a - t.colorStart.a, t.duration);
                    break;

                case TweenType.Color32:
                    t.color32Value.r = (byte)Mathf.Clamp(EasingFunction.Easing(t.easing, t.elapsedTime, t.color32Start.r, t.color32End.r - t.color32Start.r, t.duration), 0, 255);
                    t.color32Value.g = (byte)Mathf.Clamp(EasingFunction.Easing(t.easing, t.elapsedTime, t.color32Start.g, t.color32End.g - t.color32Start.g, t.duration), 0, 255);
                    t.color32Value.b = (byte)Mathf.Clamp(EasingFunction.Easing(t.easing, t.elapsedTime, t.color32Start.b, t.color32End.b - t.color32Start.b, t.duration), 0, 255);
                    t.color32Value.a = (byte)Mathf.Clamp(EasingFunction.Easing(t.easing, t.elapsedTime, t.color32Start.a, t.color32End.a - t.color32Start.a, t.duration), 0, 255);
                    break;

                case TweenType.Quaternion:
                    t.quaternionValue.x = EasingFunction.Easing(t.easing, t.elapsedTime, t.quaternionStart.x, t.quaternionEnd.x - t.quaternionStart.x, t.duration);
                    t.quaternionValue.y = EasingFunction.Easing(t.easing, t.elapsedTime, t.quaternionStart.y, t.quaternionEnd.y - t.quaternionStart.y, t.duration);
                    t.quaternionValue.z = EasingFunction.Easing(t.easing, t.elapsedTime, t.quaternionStart.z, t.quaternionEnd.z - t.quaternionStart.z, t.duration);
                    t.quaternionValue.w = EasingFunction.Easing(t.easing, t.elapsedTime, t.quaternionStart.w, t.quaternionEnd.w - t.quaternionStart.w, t.duration);
                    t.quaternionValue = NormalizeQuaternion(t.quaternionValue);
                    break;

                case TweenType.Wait:
                    break;

            }

            if (t.elapsedTime >= t.duration) {
                t.isComplete = true;
                int nextId = NextIds[index]; // 获取链式目标

                if (nextId != -1 && !t.isLoop) {
                    t.shouldStartNext = true;
                } else if (t.isLoop) {
                    ResetTween(ref t);
                }
            }

            Tweens[index] = t;
        }

        Quaternion NormalizeQuaternion(Quaternion q) {
            float mag = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
            if (mag > 0) {
                q.x /= mag;
                q.y /= mag;
                q.z /= mag;
                q.w /= mag;
            }
            return q;
        }

        void ResetTween(ref TweenModel t) {
            t.elapsedTime = 0;
            t.isComplete = false;
            switch (t.type) {
                case TweenType.Float: t.floatValue = t.floatStart; break;
                case TweenType.Vector3: t.vector3Value = t.vector3Start; break;
                case TweenType.Vector2: t.vector2Value = t.vector2Start; break;
                case TweenType.Quaternion: t.quaternionValue = t.quaternionStart; break;
                case TweenType.Color32: t.color32Value = t.color32Start; break;
                case TweenType.Color: t.colorValue = t.colorStart; break;
                case TweenType.Wait: break;
            }
        }
    }

    #region Tick
    public void Tick(float deltaTime) {
        if (tweenCount == 0) return;

        var job = new TweenUpdateJob {
            Tweens = activeTweens.GetSubArray(0, tweenCount),
            NextIds = nextIdArray,
            DeltaTime = deltaTime
        };

        JobHandle handle = job.Schedule(tweenCount, 32);
        handle.Complete();

        // 处理完成状态和链式调用
        for (int i = 0; i < tweenCount; i++) {
            TweenModel t = activeTweens[i];
            if (t.shouldStartNext) {
                t.shouldStartNext = false;
                int nextId = nextIdArray[i];
                if (idToIndex.TryGetValue(nextId, out int nextIndex)) {
                    TweenModel next = activeTweens[nextIndex];
                    next.isPlaying = true;
                    next.elapsedTime = 0f;
                    next.isComplete = false;
                    activeTweens[nextIndex] = next;
                }
            }
            activeTweens[i] = t;
        }

        ProcessCallbacks();
    }
    #endregion

    void ProcessCallbacks() {
        for (int i = 0; i < tweenCount; i++) {
            TweenModel t = activeTweens[i];
            if (!t.isPlaying) continue;

            if (updateCallbacks.TryGetValue(t.id, out Delegate updateDel)) {
                InvokeCallback(updateDel, ref t);
            }

            if (t.isComplete && completeCallbacks.TryGetValue(t.id, out Delegate completeDel)) {
                InvokeCallback(completeDel, ref t);
                if (!t.isLoop) RemoveCallback(t.id);
            }
            activeTweens[i] = t;
        }
    }

    void InvokeCallback(Delegate callback, ref TweenModel t) {
        switch (t.type) {
            case TweenType.Float:
                (callback as Action<float>)?.Invoke(t.floatValue);
                break;
            case TweenType.Vector3:
                (callback as Action<Vector3>)?.Invoke(t.vector3Value);
                break;
        }
    }
    #endregion

    #region 回调注册
    public void OnUpdate<T>(int tweenId, Action<T> callback) where T : struct {
        if (updateCallbacks.TryGetValue(tweenId, out var existing)) {
            updateCallbacks[tweenId] = (Action<T>)existing + callback;
        } else {
            updateCallbacks[tweenId] = callback;
        }
    }

    public void OnComplete<T>(int tweenId, Action<T> callback) where T : struct {
        if (completeCallbacks.TryGetValue(tweenId, out var existing)) {
            completeCallbacks[tweenId] = (Action<T>)existing + callback;
        } else {
            completeCallbacks[tweenId] = callback;
        }
    }

    public void OnWaitComplete(int tweenId, Action callback) {
        OnComplete<int>(tweenId, _ => callback());
    }
    #endregion

    #region 状态查询
    public bool IsPlaying(int id) {
        return idToIndex.TryGetValue(id, out int index) && activeTweens[index].isPlaying;
    }

    public bool IsComplete(int id) {
        return idToIndex.TryGetValue(id, out int index) && activeTweens[index].isComplete;
    }

    public float GetProgress(int id) {
        if (idToIndex.TryGetValue(id, out int index)) {
            TweenModel t = activeTweens[index];
            return Mathf.Clamp01(t.elapsedTime / t.duration);
        }
        return 0f;
    }
    #endregion

    #region Link
    public void Link(int fromId, int toId) {
        if (idToIndex.TryGetValue(fromId, out int fromIndex) &&
        idToIndex.TryGetValue(toId, out int _)) {
            nextIdArray[fromIndex] = toId;
        }
    }
    #endregion

    #region Play/Pause/Stop
    public void Play(int id) {
        if (idToIndex.TryGetValue(id, out int index)) {
            TweenModel tween = activeTweens[index];
            tween.isPlaying = true;
            tween.isComplete = false;
            tween.elapsedTime = 0;
            activeTweens[index] = tween;
        } else {
            Debug.LogWarning($"Tween with ID {id} does not exist.");
        }
    }

    public void Pause(int id) {
        if (idToIndex.TryGetValue(id, out int index)) {
            TweenModel tween = activeTweens[index];
            tween.isPlaying = false;
            activeTweens[index] = tween;
        } else {
            Debug.LogWarning($"Tween with ID {id} does not exist.");
        }
    }

    public void Stop(int id) {
        if (idToIndex.TryGetValue(id, out int index)) {
            TweenModel tween = activeTweens[index];
            tween.isPlaying = false;
            tween.isComplete = true;
            tween.elapsedTime = 0;
            activeTweens[index] = tween;
        } else {
            Debug.LogWarning($"Tween with ID {id} does not exist.");
        }
    }

    public void StopAll() {
        for (int i = 0; i < tweenCount; i++) {
            TweenModel tween = activeTweens[i];
            tween.isPlaying = false;
            tween.isComplete = true;
            tween.elapsedTime = 0;
            activeTweens[i] = tween;
        }
    }
    #endregion

    #region 内存管理
    int AllocateIndex() {
        if (freeIndices.Count > 0) return freeIndices.Dequeue();

        if (tweenCount >= activeTweens.Length) {
            int newSize = activeTweens.Length * 2;
            var newArray = new NativeArray<TweenModel>(newSize, Allocator.Persistent);
            NativeArray<TweenModel>.Copy(activeTweens, newArray, activeTweens.Length);
            activeTweens.Dispose();
            activeTweens = newArray;
        }

        return tweenCount++;
    }

    public void Remove(int id) {
        if (!idToIndex.TryGetValue(id, out int index)) return;

        activeTweens[index] = default;
        freeIndices.Enqueue(index);
        idToIndex.Remove(id);
        RemoveCallback(id);
    }

    void RemoveCallback(int id) {
        updateCallbacks.Remove(id);
        completeCallbacks.Remove(id);
    }
    #endregion

    public void Dispose() {
        if (activeTweens.IsCreated) {
            activeTweens.Dispose();
        }
        idToIndex.Clear();
        freeIndices.Clear();
        updateCallbacks.Clear();
        completeCallbacks.Clear();
    }
}