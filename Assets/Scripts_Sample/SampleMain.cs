using UnityEngine;

public class SampleMain : MonoBehaviour {

    TweenCore tweenCore;
    public EasingType easingType;
    public GameObject startPoint;
    public GameObject endPoint;
    public GameObject currentPoint;
    public bool isLoop;
    public float duration;

    void Start() {
        tweenCore = new TweenCore();

        var startColor = startPoint.GetComponent<SpriteRenderer>().color;
        var endColor = endPoint.GetComponent<SpriteRenderer>().color;
        var colorTween = tweenCore.Create(startColor, endColor, duration, easingType, isLoop);
        colorTween.OnUpdate(value => {
            currentPoint.GetComponent<SpriteRenderer>().color = value;
        }).OnComplete(() => {
            currentPoint.GetComponent<SpriteRenderer>().color = endColor;
        }).Play();

        var startScale = startPoint.transform.localScale;
        var endScale = endPoint.transform.localScale;
        var scaleTween = tweenCore.Create(startScale, endScale, duration, easingType, isLoop);
        scaleTween.OnUpdate(value => {
            currentPoint.transform.localScale = value;
        }).OnComplete(() => {
            currentPoint.transform.localScale = endScale;
        }).Play();

        var startPos = startPoint.transform.position;
        var endPos = endPoint.transform.position;
        var posTween = tweenCore.Create(startPos, endPos, duration, easingType, isLoop);
        posTween.OnUpdate(value => {
            currentPoint.transform.position = value;
        }).OnComplete(() => {
            currentPoint.transform.position = endPos;
        }).Play();

    }

    void Update() {
        tweenCore.Tick(Time.deltaTime);
    }

}