using System;
using UnityEngine;

public class TweenCallbackHandler<T> {
    private struct TypedCallback {
        public int TweenId;
        public Action<T> Callback;
    }

    private TypedCallback[] _callbacks;
    private int _count;

    public void Add(int tweenId, Action<T> callback) {
        if (_callbacks == null || _count >= _callbacks.Length)
            Array.Resize(ref _callbacks, Mathf.Max(4, _count * 2));

        _callbacks[_count++] = new TypedCallback {
            TweenId = tweenId,
            Callback = callback
        };
    }

    public void Invoke(int tweenId, T value) {
        for (int i = 0; i < _count; i++) {
            if (_callbacks[i].TweenId == tweenId) {
                _callbacks[i].Callback(value);
            }
        }
    }
}