using System;

namespace Myd.Common
{
    public static class Ease
    {
        public static readonly Easer Linear = t => t;
        public static readonly Easer SineIn = t => (float)(-Math.Cos(1.57079637050629 * t) + 1.0);
        public static readonly Easer SineOut = t => (float)Math.Sin(1.57079637050629 * t);
        public static readonly Easer SineInOut = t => (float)(-Math.Cos(3.14159274101257 * t) / 2.0 + 0.5);
        public static readonly Easer QuadIn = t => t * t;
        public static readonly Easer QuadOut = Invert(QuadIn);
        public static readonly Easer QuadInOut = Follow(QuadIn, QuadOut);
        public static readonly Easer CubeIn = t => t * t * t;
        public static readonly Easer CubeOut = Invert(CubeIn);
        public static readonly Easer CubeInOut = Follow(CubeIn, CubeOut);
        public static readonly Easer QuintIn = t => t * t * t * t * t;
        public static readonly Easer QuintOut = Invert(QuintIn);
        public static readonly Easer QuintInOut = Follow(QuintIn, QuintOut);
        public static readonly Easer ExpoIn = t => (float)Math.Pow(2.0, 10.0 * (t - 1.0));
        public static readonly Easer ExpoOut = Invert(ExpoIn);
        public static readonly Easer ExpoInOut = Follow(ExpoIn, ExpoOut);
        public static readonly Easer BackIn = t => (float)(t * (double)t * (2.70158004760742 * t - 1.70158004760742));
        public static readonly Easer BackOut = Invert(BackIn);
        public static readonly Easer BackInOut = Follow(BackIn, BackOut);
        public static readonly Easer BigBackIn = t => (float)(t * (double)t * (4.0 * t - 3.0));
        public static readonly Easer BigBackOut = Invert(BigBackIn);
        public static readonly Easer BigBackInOut = Follow(BigBackIn, BigBackOut);
        public static readonly Easer ElasticIn = t =>
        {
            float num1 = t * t;
            float num2 = num1 * t;
            return (float)(33.0 * num2 * num1 + -59.0 * num1 * num1 + 32.0 * num2 + -5.0 * num1);
        };
        public static readonly Easer ElasticOut = t =>
        {
            float num1 = t * t;
            float num2 = num1 * t;
            return (float)(33.0 * num2 * num1 + -106.0 * num1 * num1 + 126.0 * num2 + -67.0 * num1 + 15.0 * t);
        };
        public static readonly Easer ElasticInOut = Follow(ElasticIn, ElasticOut);
        public static readonly Easer BounceIn = t =>
        {
            t = 1f - t;
            if (t < 0.363636374473572)
                return (float)(1.0 - 121.0 / 16.0 * t * t);
            if (t < 0.727272748947144)
                return (float)(1.0 - (121.0 / 16.0 * (t - 0.545454561710358) * (t - 0.545454561710358) + 0.75));
            return t < 0.909090936183929 ? (float)(1.0 - (121.0 / 16.0 * (t - 0.818181812763214) * (t - 0.818181812763214) + 15.0 / 16.0)) : (float)(1.0 - (121.0 / 16.0 * (t - 0.954545438289642) * (t - 0.954545438289642) + 63.0 / 64.0));
        };
        public static readonly Easer BounceOut = t =>
        {
            if (t < 0.363636374473572)
                return 121f / 16f * t * t;
            if (t < 0.727272748947144)
                return (float)(121.0 / 16.0 * (t - 0.545454561710358) * (t - 0.545454561710358) + 0.75);
            return t < 0.909090936183929 ? (float)(121.0 / 16.0 * (t - 0.818181812763214) * (t - 0.818181812763214) + 15.0 / 16.0) : (float)(121.0 / 16.0 * (t - 0.954545438289642) * (t - 0.954545438289642) + 63.0 / 64.0);
        };
        public static readonly Easer BounceInOut = t =>
        {
            if (t < 0.5)
            {
                t = (float)(1.0 - t * 2.0);
                if (t < 0.363636374473572)
                    return (float)((1.0 - 121.0 / 16.0 * t * t) / 2.0);
                if (t < 0.727272748947144)
                    return (float)((1.0 - (121.0 / 16.0 * (t - 0.545454561710358) * (t - 0.545454561710358) + 0.75)) / 2.0);
                return t < 0.909090936183929 ? (float)((1.0 - (121.0 / 16.0 * (t - 0.818181812763214) * (t - 0.818181812763214) + 15.0 / 16.0)) / 2.0) : (float)((1.0 - (121.0 / 16.0 * (t - 0.954545438289642) * (t - 0.954545438289642) + 63.0 / 64.0)) / 2.0);
            }
            t = (float)(t * 2.0 - 1.0);
            if (t < 0.363636374473572)
                return (float)(121.0 / 16.0 * t * t / 2.0 + 0.5);
            if (t < 0.727272748947144)
                return (float)((121.0 / 16.0 * (t - 0.545454561710358) * (t - 0.545454561710358) + 0.75) / 2.0 + 0.5);
            return t < 0.909090936183929 ? (float)((121.0 / 16.0 * (t - 0.818181812763214) * (t - 0.818181812763214) + 15.0 / 16.0) / 2.0 + 0.5) : (float)((121.0 / 16.0 * (t - 0.954545438289642) * (t - 0.954545438289642) + 63.0 / 64.0) / 2.0 + 0.5);
        };
        private const float B1 = 0.3636364f;
        private const float B2 = 0.7272727f;
        private const float B3 = 0.5454546f;
        private const float B4 = 0.9090909f;
        private const float B5 = 0.8181818f;
        private const float B6 = 0.9545454f;

        public static Easer Invert(Easer easer)
        {
            return t => 1f - easer(1f - t);
        }

        public static Easer Follow(Easer first, Easer second)
        {
            return t => t > 0.5 ? (float)(second((float)(t * 2.0 - 1.0)) / 2.0 + 0.5) : first(t * 2f) / 2f;
        }

        public static float UpDown(float eased)
        {
            return eased <= 0.5 ? eased * 2f : (float)(1.0 - (eased - 0.5) * 2.0);
        }

        public delegate float Easer(float t);
    }
}
