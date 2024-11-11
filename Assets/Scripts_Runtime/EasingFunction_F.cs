using System;
using UnityEngine;

internal static class EasingFunction_F {

    internal static Func<float, float, float, float, float> GetEasingFunction(EasingType mode) {
        if (mode == EasingType.Linear) {
            return Linear;
        }
        if (mode == EasingType.SineIn) {
            return SineIn;
        }
        if (mode == EasingType.SineOut) {
            return SineOut;
        }
        if (mode == EasingType.SineInOut) {
            return SineInOut;
        }
        if (mode == EasingType.QuadIn) {
            return QuadIn;
        }
        if (mode == EasingType.QuadOut) {
            return QuadOut;
        }
        if (mode == EasingType.QuadInOut) {
            return QuadInOut;
        }
        if (mode == EasingType.CubicIn) {
            return CubicIn;
        }
        if (mode == EasingType.CubicOut) {
            return CubicOut;
        }
        if (mode == EasingType.CubicInOut) {
            return CubicInOut;
        }
        if (mode == EasingType.QuartIn) {
            return QuartIn;
        }
        if (mode == EasingType.QuartOut) {
            return QuartOut;
        }
        if (mode == EasingType.QuartInOut) {
            return QuartInOut;
        }
        if (mode == EasingType.QuintIn) {
            return QuintIn;
        }
        if (mode == EasingType.QuintOut) {
            return QuintOut;
        }
        if (mode == EasingType.QuintInOut) {
            return QuintInOut;
        }
        if (mode == EasingType.ExpoIn) {
            return ExpoIn;
        }
        if (mode == EasingType.ExpoOut) {
            return ExpoOut;
        }
        if (mode == EasingType.ExpoInOut) {
            return ExpoInOut;
        }
        if (mode == EasingType.CircIn) {
            return CircIn;
        }
        if (mode == EasingType.CircOut) {
            return CircOut;
        }
        if (mode == EasingType.CircInOut) {
            return CircInOut;
        }
        if (mode == EasingType.BackIn) {
            return BackIn;
        }
        if (mode == EasingType.BackOut) {
            return BackOut;
        }
        if (mode == EasingType.BackInOut) {
            return BackInOut;
        }
        if (mode == EasingType.ElasticIn) {
            return ElasticIn;
        }
        if (mode == EasingType.ElasticOut) {
            return ElasticOut;
        }
        if (mode == EasingType.ElasticInOut) {
            return ElasticInOut;
        }
        if (mode == EasingType.BounceIn) {
            return BounceIn;
        }
        if (mode == EasingType.BounceOut) {
            return BounceOut;
        }
        if (mode == EasingType.BounceInOut) {
            return BounceInOut;
        }
        return Linear;
    }

    static float Linear(float t, float b, float c, float d) {
        return c * t / d + b;
    }

    static float SineIn(float t, float b, float c, float d) {
        return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
    }

    static float SineOut(float t, float b, float c, float d) {
        return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
    }

    static float SineInOut(float t, float b, float c, float d) {
        return -c / 2 * ((float)Mathf.Cos(Mathf.PI * t / d) - 1) + b;
    }

    static float QuadIn(float t, float b, float c, float d) {
        return c * (t /= d) * t + b;
    }

    static float QuadOut(float t, float b, float c, float d) {
        return -c * (t /= d) * (t - 2) + b;
    }

    static float QuadInOut(float t, float b, float c, float d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t + b;
        return -c / 2 * ((--t) * (t - 2) - 1) + b;
    }

    static float CubicIn(float t, float b, float c, float d) {
        return c * (t /= d) * t * t + b;
    }

    static float CubicOut(float t, float b, float c, float d) {
        return c * ((t = t / d - 1) * t * t + 1) + b;
    }

    static float CubicInOut(float t, float b, float c, float d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t + 2) + b;
    }

    static float QuartIn(float t, float b, float c, float d) {
        return c * (t /= d) * t * t * t + b;
    }

    static float QuartOut(float t, float b, float c, float d) {
        return -c * ((t = t / d - 1) * t * t * t - 1) + b;
    }

    static float QuartInOut(float t, float b, float c, float d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
        return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
    }

    static float QuintIn(float t, float b, float c, float d) {
        return c * (t /= d) * t * t * t * t + b;
    }

    static float QuintOut(float t, float b, float c, float d) {
        return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
    }

    static float QuintInOut(float t, float b, float c, float d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
    }

    static float ExpoIn(float t, float b, float c, float d) {
        return (t == 0) ? b : c * (float)Mathf.Pow(2, 10 * (t / d - 1)) + b;
    }

    static float ExpoOut(float t, float b, float c, float d) {
        return (t == d) ? b + c : c * (-(float)Mathf.Pow(2, -10 * t / d) + 1) + b;

    }

    static float ExpoInOut(float t, float b, float c, float d) {
        if (t == 0) return b;
        if (t == d) return b + c;
        if ((t /= d / 2) < 1) return c / 2 * (float)Mathf.Pow(2, 10 * (t - 1)) + b;
        return c / 2 * (-(float)Mathf.Pow(2, -10 * --t) + 2) + b;
    }

    static float CircIn(float t, float b, float c, float d) {
        return -c * ((float)Mathf.Sqrt(1 - (t /= d) * t) - 1) + b;
    }

    static float CircOut(float t, float b, float c, float d) {
        return c * (float)Mathf.Sqrt(1 - (t = t / d - 1) * t) + b;
    }

    static float CircInOut(float t, float b, float c, float d) {
        if ((t /= d / 2) < 1) return -c / 2 * ((float)Mathf.Sqrt(1 - t * t) - 1) + b;
        return c / 2 * ((float)Mathf.Sqrt(1 - (t -= 2) * t) + 1) + b;
    }

    static float BackIn(float t, float b, float c, float d) {
        float s = 1.70158f;
        return c * (t /= d) * t * ((s + 1) * t - s) + b;
    }

    static float BackOut(float t, float b, float c, float d) {
        float s = 1.70158f;
        return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
    }

    static float BackInOut(float t, float b, float c, float d) {
        float s = 1.70158f;
        if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
        return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
    }

    static float ElasticIn(float t, float b, float c, float d) {
        if (t == 0) return b; if ((t /= d) == 1) return b + c;
        float p = d * .3f;
        float a = c;
        float s = p / 4;
        return -(a * (float)Mathf.Pow(2, 10 * (t -= 1)) * (float)Mathf.Sin((t * d - s) * (2 * (float)Mathf.PI) / p)) + b;
    }

    static float ElasticOut(float t, float b, float c, float d) {
        if (t == 0) return b; if ((t /= d) == 1) return b + c;
        float p = d * .3f;
        float a = c;
        float s = p / 4;
        return (a * (float)Mathf.Pow(2, -10 * t) * (float)Mathf.Sin((t * d - s) * (2 * (float)Mathf.PI) / p) + c + b);
    }

    static float ElasticInOut(float t, float b, float c, float d) {
        if (t == 0) return b; if ((t /= d / 2) == 2) return b + c;
        float p = d * (.3f * 1.5f);
        float a = c;
        float s = p / 4;
        if (t < 1) return -.5f * (a * (float)Mathf.Pow(2, 10 * (t -= 1)) * (float)Mathf.Sin((t * d - s) * (2 * (float)Mathf.PI) / p)) + b;
        return a * (float)Mathf.Pow(2, -10 * (t -= 1)) * (float)Mathf.Sin((t * d - s) * (2 * (float)Mathf.PI) / p) * .5f + c + b;
    }

    static float BounceIn(float t, float b, float c, float d) {
        return c - BounceOut(d - t, 0, c, d) + b;
    }

    static float BounceOut(float t, float b, float c, float d) {
        if ((t /= d) < (1 / 2.75f)) {
            return c * (7.5625f * t * t) + b;
        } else if (t < (2 / 2.75f)) {
            return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
        } else if (t < (2.5 / 2.75)) {
            return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
        } else {
            return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
        }
    }

    static float BounceInOut(float t, float b, float c, float d) {
        if (t < d / 2) return BounceIn(t * 2, 0, c, d) * .5f + b;
        else return BounceOut(t * 2 - d, 0, c, d) * .5f + c * .5f + b;
    }

}