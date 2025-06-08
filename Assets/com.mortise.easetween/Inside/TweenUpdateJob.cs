using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Mortise.EaseTween {

    internal struct TweenUpdateJob : IJobParallelFor {
        internal NativeArray<TweenModel> tweens;
        internal float deltaTime;

        public void Execute(int index) => Execute(index);

        void IJobParallelFor.Execute(int index) {
            TweenModel t = tweens[index];
            if (!t.isPlaying || t.isComplete) return;

            t.elapsedTime += deltaTime;

            t.flags |= 0x2; // Set HasChanged Flag

            switch (t.type) {
                case TweenType.Float:
                    t.floatValue = Ease(t.easing, t.floatStart, t.floatEnd, t.elapsedTime, t.duration);
                    break;

                case TweenType.Vector2:
                    t.vector2Value.x = Ease(t.easing, t.vector2Start.x, t.vector2End.x, t.elapsedTime, t.duration);
                    t.vector2Value.y = Ease(t.easing, t.vector2Start.y, t.vector2End.y, t.elapsedTime, t.duration);
                    break;

                case TweenType.Vector3:
                    t.vector3Value.x = Ease(t.easing, t.vector3Start.x, t.vector3End.x, t.elapsedTime, t.duration);
                    t.vector3Value.y = Ease(t.easing, t.vector3Start.y, t.vector3End.y, t.elapsedTime, t.duration);
                    t.vector3Value.z = Ease(t.easing, t.vector3Start.z, t.vector3End.z, t.elapsedTime, t.duration);
                    break;

                case TweenType.Color:
                    t.colorValue.r = Ease(t.easing, t.colorStart.r, t.colorEnd.r, t.elapsedTime, t.duration);
                    t.colorValue.g = Ease(t.easing, t.colorStart.g, t.colorEnd.g, t.elapsedTime, t.duration);
                    t.colorValue.b = Ease(t.easing, t.colorStart.b, t.colorEnd.b, t.elapsedTime, t.duration);
                    t.colorValue.a = Ease(t.easing, t.colorStart.a, t.colorEnd.a, t.elapsedTime, t.duration);
                    break;

                case TweenType.Quaternion:
                    t.quaternionValue = Quaternion.Slerp(t.quaternionStart, t.quaternionEnd,
                        Ease(t.easing, 0, 1, t.elapsedTime, t.duration));
                    break;

                case TweenType.Color32:
                    t.color32Value.r = (byte)Mathf.RoundToInt(Ease(t.easing, t.color32Start.r, t.color32End.r, t.elapsedTime, t.duration));
                    t.color32Value.g = (byte)Mathf.RoundToInt(Ease(t.easing, t.color32Start.g, t.color32End.g, t.elapsedTime, t.duration));
                    t.color32Value.b = (byte)Mathf.RoundToInt(Ease(t.easing, t.color32Start.b, t.color32End.b, t.elapsedTime, t.duration));
                    t.color32Value.a = (byte)Mathf.RoundToInt(Ease(t.easing, t.color32Start.a, t.color32End.a, t.elapsedTime, t.duration));
                    break;

                case TweenType.Int:
                    t.intValue = Mathf.RoundToInt(Ease(t.easing, t.intStart, t.intEnd - t.intStart, t.elapsedTime, t.duration));
                    break;
            }

            if (t.elapsedTime >= t.duration) {
                t.isComplete = true;
                if (t.nextId != -1 && !t.isLoop) {
                    t.flags |= 0x1; // Set NeedsChainStart Flag
                } else if (t.isLoop) {
                    t.elapsedTime = 0;
                    t.isComplete = false;
                    ResetTweenValues(ref t);
                }
            }

            tweens[index] = t;
        }

        private float Ease(EasingType easing, float start, float end, float time, float duration) {
            return EasingFunction.Easing(easing, time, start, end - start, duration);
        }

        private void ResetTweenValues(ref TweenModel t) {
            switch (t.type) {
                case TweenType.Float: t.floatValue = t.floatStart; break;
                case TweenType.Vector2: t.vector2Value = t.vector2Start; break;
                case TweenType.Vector3: t.vector3Value = t.vector3Start; break;
                case TweenType.Quaternion: t.quaternionValue = t.quaternionStart; break;
                case TweenType.Color32: t.color32Value = t.color32Start; break;
                case TweenType.Color: t.colorValue = t.colorStart; break;
            }
        }
    }

}