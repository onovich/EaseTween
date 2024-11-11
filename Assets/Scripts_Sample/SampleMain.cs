using UnityEngine;

public class SampleMain : MonoBehaviour {

    TweenCore tweenCore;
    public EasingType easingType;
    public float startValue;
    public float endValue;
    public float duration;

    void Start() {
        tweenCore = new TweenCore();
        var tween = tweenCore.Create(startValue, endValue, duration, easingType);
        tween.OnUpdate(value => {
            Debug.Log($"OnTick, v = {value}");
        }).OnComplete(() => {
            Debug.Log("Complete!");
        }).Play();
    }

    void Update() {
        tweenCore.Tick(Time.deltaTime);
    }

}