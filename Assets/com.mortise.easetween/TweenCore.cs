using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Mortise.EaseTween {

    public sealed class TweenCore : IDisposable {
        int tweenCount;
        int nextId;

        NativeArray<TweenModel> activeTweens;
        Dictionary<int, int> idToIndex;
        Queue<int> freeIndices;

        Dictionary<int, Action<float>> floatUpdateCallbacks;
        Dictionary<int, Action<Vector3>> vector3UpdateCallbacks;
        Dictionary<int, Action<Vector2>> vector2UpdateCallbacks;
        Dictionary<int, Action<Color>> colorUpdateCallbacks;
        Dictionary<int, Action<Color32>> color32UpdateCallbacks;
        Dictionary<int, Action<Quaternion>> quaternionUpdateCallbacks;
        Dictionary<int, Action<int>> intUpdateCallbacks;
        Dictionary<int, Action> waitUpdateCallbacks;

        Dictionary<int, Action<float>> floatCompleteCallbacks;
        Dictionary<int, Action<Vector3>> vector3CompleteCallbacks;
        Dictionary<int, Action<Vector2>> vector2CompleteCallbacks;
        Dictionary<int, Action<Color>> colorCompleteCallbacks;
        Dictionary<int, Action<Color32>> color32CompleteCallbacks;
        Dictionary<int, Action<Quaternion>> quaternionCompleteCallbacks;
        Dictionary<int, Action<int>> intCompleteCallbacks;
        Dictionary<int, Action> waitCompleteCallbacks;

        public TweenCore(int capacity = 128) {
            activeTweens = new NativeArray<TweenModel>(capacity, Allocator.Persistent);
            idToIndex = new Dictionary<int, int>();
            freeIndices = new Queue<int>();

            floatUpdateCallbacks = new Dictionary<int, Action<float>>();
            vector3UpdateCallbacks = new Dictionary<int, Action<Vector3>>();
            vector2UpdateCallbacks = new Dictionary<int, Action<Vector2>>();
            colorUpdateCallbacks = new Dictionary<int, Action<Color>>();
            color32UpdateCallbacks = new Dictionary<int, Action<Color32>>();
            quaternionUpdateCallbacks = new Dictionary<int, Action<Quaternion>>();
            intUpdateCallbacks = new Dictionary<int, Action<int>>();
            waitUpdateCallbacks = new Dictionary<int, Action>();

            floatCompleteCallbacks = new Dictionary<int, Action<float>>();
            vector3CompleteCallbacks = new Dictionary<int, Action<Vector3>>();
            vector2CompleteCallbacks = new Dictionary<int, Action<Vector2>>();
            colorCompleteCallbacks = new Dictionary<int, Action<Color>>();
            color32CompleteCallbacks = new Dictionary<int, Action<Color32>>();
            quaternionCompleteCallbacks = new Dictionary<int, Action<Quaternion>>();
            intCompleteCallbacks = new Dictionary<int, Action<int>>();
            waitCompleteCallbacks = new Dictionary<int, Action>();

            nextId = 1;
            tweenCount = 0;
        }

        #region Create
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

        public int Create(int start, int end, float duration, EasingType easing, bool isLoop = false) {
            int id = nextId++;
            int index = AllocateIndex();

            TweenModel tween = new TweenModel {
                id = id,
                type = TweenType.Int,
                intStart = start,
                intEnd = end,
                intValue = start,
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

        #region Tick
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

                    InvokeUpdateCallback(t);

                    if (t.isComplete) {
                        InvokeCompleteCallback(t);
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

                activeTweens[i] = t;
            }
        }
        #endregion

        #region Invoke
        void InvokeUpdateCallback(TweenModel t) {
            switch (t.type) {
                case TweenType.Float:
                    if (floatUpdateCallbacks.TryGetValue(t.id, out var floatUpdate)) floatUpdate(t.floatValue);
                    break;
                case TweenType.Vector3:
                    if (vector3UpdateCallbacks.TryGetValue(t.id, out var vector3Update)) vector3Update(t.vector3Value);
                    break;
                case TweenType.Vector2:
                    if (vector2UpdateCallbacks.TryGetValue(t.id, out var vector2Update)) vector2Update(t.vector2Value);
                    break;
                case TweenType.Color:
                    if (colorUpdateCallbacks.TryGetValue(t.id, out var colorUpdate)) colorUpdate(t.colorValue);
                    break;
                case TweenType.Color32:
                    if (color32UpdateCallbacks.TryGetValue(t.id, out var color32Update)) color32Update(t.color32Value);
                    break;
                case TweenType.Quaternion:
                    if (quaternionUpdateCallbacks.TryGetValue(t.id, out var quaternionUpdate)) quaternionUpdate(t.quaternionValue);
                    break;
                case TweenType.Int:
                    if (intUpdateCallbacks.TryGetValue(t.id, out var intUpdate)) intUpdate(t.intValue);
                    break;
                case TweenType.Wait:
                    if (waitUpdateCallbacks.TryGetValue(t.id, out var waitUpdate)) waitUpdate();
                    break;
            }
        }

        void InvokeCompleteCallback(TweenModel t) {
            switch (t.type) {
                case TweenType.Float:
                    if (floatCompleteCallbacks.TryGetValue(t.id, out var floatComplete)) floatComplete(t.floatValue);
                    break;
                case TweenType.Vector3:
                    if (vector3CompleteCallbacks.TryGetValue(t.id, out var vector3Complete)) vector3Complete(t.vector3Value);
                    break;
                case TweenType.Vector2:
                    if (vector2CompleteCallbacks.TryGetValue(t.id, out var vector2Complete)) vector2Complete(t.vector2Value);
                    break;
                case TweenType.Color:
                    if (colorCompleteCallbacks.TryGetValue(t.id, out var colorComplete)) colorComplete(t.colorValue);
                    break;
                case TweenType.Color32:
                    if (color32CompleteCallbacks.TryGetValue(t.id, out var color32Complete)) color32Complete(t.color32Value);
                    break;
                case TweenType.Quaternion:
                    if (quaternionCompleteCallbacks.TryGetValue(t.id, out var quaternionComplete)) quaternionComplete(t.quaternionValue);
                    break;
                case TweenType.Int:
                    if (intCompleteCallbacks.TryGetValue(t.id, out var intComplete)) intComplete(t.intValue);
                    break;
                case TweenType.Wait:
                    if (waitCompleteCallbacks.TryGetValue(t.id, out var waitComplete)) waitComplete();
                    break;
            }
        }
        #endregion

        #region Binding
        public void OnUpdate(int tweenId, Action<float> callback) {
            AddCallback(floatUpdateCallbacks, tweenId, callback);
        }

        public void OnUpdate(int tweenId, Action<Vector3> callback) {
            AddCallback(vector3UpdateCallbacks, tweenId, callback);
        }

        public void OnUpdate(int tweenId, Action<Vector2> callback) {
            AddCallback(vector2UpdateCallbacks, tweenId, callback);
        }

        public void OnUpdate(int tweenId, Action<Color> callback) {
            AddCallback(colorUpdateCallbacks, tweenId, callback);
        }

        public void OnUpdate(int tweenId, Action<Color32> callback) {
            AddCallback(color32UpdateCallbacks, tweenId, callback);
        }

        public void OnUpdate(int tweenId, Action<Quaternion> callback) {
            AddCallback(quaternionUpdateCallbacks, tweenId, callback);
        }

        public void OnUpdate(int tweenId, Action<int> callback) {
            AddCallback(intUpdateCallbacks, tweenId, callback);
        }

        public void OnUpdate(int tweenId, Action callback) {
            AddCallback(waitUpdateCallbacks, tweenId, callback);
        }

        public void OnComplete(int tweenId, Action<float> callback) {
            AddCallback(floatCompleteCallbacks, tweenId, callback);
        }

        public void OnComplete(int tweenId, Action<Vector3> callback) {
            AddCallback(vector3CompleteCallbacks, tweenId, callback);
        }

        public void OnComplete(int tweenId, Action<Vector2> callback) {
            AddCallback(vector2CompleteCallbacks, tweenId, callback);
        }

        public void OnComplete(int tweenId, Action<Color> callback) {
            AddCallback(colorCompleteCallbacks, tweenId, callback);
        }

        public void OnComplete(int tweenId, Action<Color32> callback) {
            AddCallback(color32CompleteCallbacks, tweenId, callback);
        }

        public void OnComplete(int tweenId, Action<Quaternion> callback) {
            AddCallback(quaternionCompleteCallbacks, tweenId, callback);
        }

        public void OnComplete(int tweenId, Action<int> callback) {
            AddCallback(intCompleteCallbacks, tweenId, callback);
        }

        public void OnComplete(int tweenId, Action callback) {
            AddCallback(waitCompleteCallbacks, tweenId, callback);
        }

        void AddCallback<T>(Dictionary<int, T> dictionary, int tweenId, T callback) where T : Delegate {
            if (dictionary.TryGetValue(tweenId, out var existing)) {
                dictionary[tweenId] = (T)Delegate.Combine(existing, callback);
            } else {
                dictionary[tweenId] = callback;
            }
        }
        #endregion

        #region Context 
        int AllocateIndex() {
            if (freeIndices.Count > 0) return freeIndices.Dequeue();

            if (tweenCount >= activeTweens.Length) {
                int newSize = activeTweens.Length * 2;
                var newTweens = new NativeArray<TweenModel>(newSize, Allocator.Persistent);

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

        void RemoveCallback(int tweenId) {
            floatUpdateCallbacks.Remove(tweenId);
            vector3UpdateCallbacks.Remove(tweenId);
            vector2UpdateCallbacks.Remove(tweenId);
            colorUpdateCallbacks.Remove(tweenId);
            color32UpdateCallbacks.Remove(tweenId);
            quaternionUpdateCallbacks.Remove(tweenId);
            intUpdateCallbacks.Remove(tweenId);
            waitUpdateCallbacks.Remove(tweenId);

            floatCompleteCallbacks.Remove(tweenId);
            vector3CompleteCallbacks.Remove(tweenId);
            vector2CompleteCallbacks.Remove(tweenId);
            colorCompleteCallbacks.Remove(tweenId);
            color32CompleteCallbacks.Remove(tweenId);
            quaternionCompleteCallbacks.Remove(tweenId);
            intCompleteCallbacks.Remove(tweenId);
            waitCompleteCallbacks.Remove(tweenId);
        }

        public void Dispose() {
            if (activeTweens.IsCreated) activeTweens.Dispose();
            idToIndex.Clear();
            freeIndices.Clear();

            floatUpdateCallbacks.Clear();
            vector3UpdateCallbacks.Clear();
            vector2UpdateCallbacks.Clear();
            colorUpdateCallbacks.Clear();
            color32UpdateCallbacks.Clear();
            quaternionUpdateCallbacks.Clear();
            intUpdateCallbacks.Clear();
            waitUpdateCallbacks.Clear();

            floatCompleteCallbacks.Clear();
            vector3CompleteCallbacks.Clear();
            vector2CompleteCallbacks.Clear();
            colorCompleteCallbacks.Clear();
            color32CompleteCallbacks.Clear();
            quaternionCompleteCallbacks.Clear();
            intCompleteCallbacks.Clear();
            waitCompleteCallbacks.Clear();
        }
        #endregion

        #region Play/Resume/Pause/Stop/Link
        public void Play(int tweenId) {
            if (idToIndex.TryGetValue(tweenId, out int index)) {
                TweenModel t = activeTweens[index];
                t.isPlaying = true;
                t.isComplete = false;
                t.elapsedTime = 0;
                activeTweens[index] = t;
            }
        }

        public void Resume(int tweenId) {
            if (idToIndex.TryGetValue(tweenId, out int index)) {
                TweenModel t = activeTweens[index];
                if (!t.isPlaying && !t.isComplete) {
                    t.isPlaying = true;
                    activeTweens[index] = t;
                }
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

}