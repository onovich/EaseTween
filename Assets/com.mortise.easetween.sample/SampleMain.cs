using UnityEngine;

public class SampleMain : MonoBehaviour {

    TweenCore tweenCore;
    public EasingType easingType;
    public GameObject startPoint;
    public GameObject endPoint;
    public GameObject currentPoint;
    public bool isLoop;
    public float duration;
    public bool isTearedDown;

    void Start() {
        tweenCore = new TweenCore();

        var startPos = startPoint.transform.position;
        var endPos = endPoint.transform.position;
        var tween_move_from_start_to_end = tweenCore.Create(startPos, endPos, duration, easingType, isLoop);
        tweenCore.OnUpdate(tween_move_from_start_to_end, (Vector3 pos) => {
            currentPoint.transform.position = pos;
        });
        var tween_move_from_end_to_start = tweenCore.Create(endPos, startPos, duration, easingType, isLoop);
        tweenCore.OnUpdate(tween_move_from_end_to_start, (Vector3 pos) => {
            currentPoint.transform.position = pos;
        });

        var startScale = startPoint.transform.localScale;
        var endScale = endPoint.transform.localScale;
        var tween_scale_from_start_to_end = tweenCore.Create(startScale, endScale, duration, easingType, isLoop);
        tweenCore.OnUpdate(tween_scale_from_start_to_end, (Vector3 scale) => {
            currentPoint.transform.localScale = scale;
        });
        var tween_from_end_to_start = tweenCore.Create(endScale, startScale, duration, easingType, isLoop);
        tweenCore.OnUpdate(tween_from_end_to_start, (Vector3 scale) => {
            currentPoint.transform.localScale = scale;
        });

        var startColor = startPoint.GetComponent<SpriteRenderer>().color;
        var endColor = endPoint.GetComponent<SpriteRenderer>().color;
        var tween_color_from_start_to_end = tweenCore.Create(startColor, endColor, duration, easingType, isLoop);
        tweenCore.OnUpdate(tween_color_from_start_to_end, (Color color) => {
            currentPoint.GetComponent<SpriteRenderer>().color = color;
        });
        var tween_color_from_end_to_start = tweenCore.Create(endColor, startColor, duration, easingType, isLoop);
        tweenCore.OnUpdate(tween_color_from_end_to_start, (Color color) => {
            currentPoint.GetComponent<SpriteRenderer>().color = color;
        });

        tweenCore.Link(tween_move_from_start_to_end, tween_scale_from_start_to_end);
        tweenCore.Link(tween_scale_from_start_to_end, tween_color_from_start_to_end);
        tweenCore.Link(tween_color_from_start_to_end, tween_move_from_end_to_start);
        tweenCore.Link(tween_move_from_end_to_start, tween_from_end_to_start);
        tweenCore.Link(tween_from_end_to_start, tween_color_from_end_to_start);
        tweenCore.Link(tween_color_from_end_to_start, tween_move_from_start_to_end);

        tweenCore.Play(tween_move_from_start_to_end);
    }

    void Update() {
        tweenCore.Tick(Time.deltaTime);
    }

    void OnApplicationQuit() {
        if (isTearedDown) {
            return;
        }
        tweenCore.Dispose();
        isTearedDown = true;
    }

    void OnDestroy() {
        if (isTearedDown) {
            return;
        }
        tweenCore.Dispose();
        isTearedDown = true;
    }

}