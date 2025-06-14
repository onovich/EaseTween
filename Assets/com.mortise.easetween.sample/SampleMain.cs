using UnityEngine;
using UnityEngine.UI;

namespace MortiseFrame.EaseTween.Sample {

    public class SampleMain : MonoBehaviour {

        TweenCore tweenCore;
        public EasingType easingType;
        public GameObject startPoint;
        public GameObject endPoint;
        public GameObject currentPoint;
        public bool isLoop;
        public float duration;
        public float textDuration;
        public bool textIsLoop;
        bool isTearedDown;
        public string[] strings = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public Text text;
        int textIndex;

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

            tweenCore.OnComplete(tween_color_from_end_to_start, (Color color) => {
                Debug.Log("Color Changed End");
            });

            tweenCore.Link(tween_move_from_start_to_end, tween_scale_from_start_to_end);
            tweenCore.Link(tween_scale_from_start_to_end, tween_color_from_start_to_end);
            tweenCore.Link(tween_color_from_start_to_end, tween_move_from_end_to_start);
            tweenCore.Link(tween_move_from_end_to_start, tween_from_end_to_start);
            tweenCore.Link(tween_from_end_to_start, tween_color_from_end_to_start);
            tweenCore.Link(tween_color_from_end_to_start, tween_move_from_start_to_end);

            tweenCore.Play(tween_move_from_start_to_end);


            int stringIndexStart = 0;
            int stringIndexEnd = strings.Length - 1;
            int tween_int = tweenCore.Create(stringIndexStart, stringIndexEnd, textDuration, EasingType.Linear, textIsLoop);
            tweenCore.OnUpdate(tween_int, (int index) => {
                if (index < 0 || index >= strings.Length) {
                    Debug.LogError("Index out of bounds: " + index);
                    return;
                }
                text.text = strings[index];
            });
            tweenCore.Play(tween_int);
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

}