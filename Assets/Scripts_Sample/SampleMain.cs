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

        var startPos = startPoint.transform.position;
        var endPos = endPoint.transform.position;
        var posTween1 = tweenCore.Create(startPos, endPos, duration, easingType, isLoop);
        tweenCore.ListenTick(posTween1, (Vector3 pos) => {
            currentPoint.transform.position = pos;
        });
        var posTween2 = tweenCore.Create(endPos, startPos, duration, easingType, isLoop);
        tweenCore.ListenTick(posTween2, (Vector3 pos) => {
            currentPoint.transform.position = pos;
        });

        var startScale = startPoint.transform.localScale;
        var endScale = endPoint.transform.localScale;
        var scaleTween1 = tweenCore.Create(startScale, endScale, duration, easingType, isLoop);
        tweenCore.ListenTick(scaleTween1, (Vector3 scale) => {
            currentPoint.transform.localScale = scale;
        });
        var scaleTween2 = tweenCore.Create(endScale, startScale, duration, easingType, isLoop);
        tweenCore.ListenTick(scaleTween2, (Vector3 scale) => {
            currentPoint.transform.localScale = scale;
        });

        var startColor = startPoint.GetComponent<SpriteRenderer>().color;
        var endColor = endPoint.GetComponent<SpriteRenderer>().color;
        var colorTween1 = tweenCore.Create(startColor, endColor, duration, easingType, isLoop);
        tweenCore.ListenTick(colorTween1, (Color color) => {
            currentPoint.GetComponent<SpriteRenderer>().color = color;
        });
        var colorTween2 = tweenCore.Create(endColor, startColor, duration, easingType, isLoop);
        tweenCore.ListenTick(colorTween2, (Color color) => {
            currentPoint.GetComponent<SpriteRenderer>().color = color;
        });

        tweenCore.Link(posTween1, scaleTween1);
        tweenCore.Link(scaleTween1, colorTween1);
        tweenCore.Link(colorTween1, posTween2);
        tweenCore.Link(posTween2, scaleTween2);
        tweenCore.Link(scaleTween2, colorTween2);
        tweenCore.Link(colorTween2, posTween1);

        tweenCore.Play(posTween1);
    }

    void Update() {
        tweenCore.Tick(Time.deltaTime);
    }

    void OnApplicationQuit() {
        tweenCore.Dispose();
    }

    void OnDestroy() {
        tweenCore.Dispose();
    }

}