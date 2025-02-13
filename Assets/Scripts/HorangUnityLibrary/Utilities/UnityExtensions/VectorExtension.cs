using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Horang.HorangUnityLibrary.Utilities.UnityExtensions
{
    public static class VectorExtension
    {
        public static float[] Convolve(this Vector2 @this, Vector2 other)
        {
            var e1 = @this.x * other.y;
            var e2 = @this.x * other.y + @this.y * other.x;
            var e3 = @this.y * other.x;

            return new[] { e1, e2, e3 };
        }

        public static Vector2 xx(this Vector2 @this) => new (@this.x, @this.x);

        public static Vector3 xxx(this Vector2 @this) => new(@this.x, @this.x, @this.x);
        public static Vector3 xyx(this Vector2 @this) => new(@this.x, @this.y, @this.x);
        public static Vector3 xyy(this Vector2 @this) => new(@this.x, @this.y, @this.y);

        public static Vector4 xxxx(this Vector2 @this) => new(@this.x, @this.x, @this.x, @this.x);
        public static Vector4 xxxy(this Vector2 @this) => new(@this.x, @this.x, @this.x, @this.y);
        public static Vector4 xxyx(this Vector2 @this) => new(@this.x, @this.x, @this.y, @this.x);
        public static Vector4 xxyy(this Vector2 @this) => new(@this.x, @this.x, @this.y, @this.y);
        public static Vector4 xyxx(this Vector2 @this) => new(@this.x, @this.y, @this.x, @this.x);
        public static Vector4 xyxy(this Vector2 @this) => new(@this.x, @this.y, @this.x, @this.y);
        public static Vector4 xyyx(this Vector2 @this) => new(@this.x, @this.y, @this.y, @this.x);
        public static Vector4 xyyy(this Vector2 @this) => new(@this.x, @this.y, @this.y, @this.y);

        public static Vector2 yy(this Vector2 @this) => new (@this.y, @this.y);
        public static Vector2 yx(this Vector2 @this) => new(@this.y, @this.x);

        public static Vector3 yyy(this Vector2 @this) => new(@this.y, @this.y, @this.y);

        public static Vector4 yyyy(this Vector2 @this) => new(@this.y, @this.y, @this.y, @this.y);

        public static float[] Convolve(this Vector3 @this, Vector3 other)
        {
            var e1 = @this.x * other.x;
            var e2 = @this.x * other.y + @this.y * other.x;
            var e3 = @this.x * other.z + @this.y * other.y + @this.z * other.x;
            var e4 = @this.y * other.z + @this.z * other.y;
            var e5 = @this.z * other.z;

            var v = other.xx();
            
            return new[] { e1, e2, e3, e4, e5 };
        }

        public static Vector2 xx(this Vector3 @this) => new (@this.x, @this.x);
        public static Vector3 xxx(this Vector3 @this) => new(@this.x, @this.x, @this.x);

        public static float[] Convolve(this Vector4 @this, Vector4 other)
        {
            var e1 = @this.x * other.x;
            var e2 = @this.x * other.y + @this.y * other.x;
            var e3 = @this.x * other.z + @this.y * other.y + @this.z * other.x;
            var e4 = @this.x * other.w + @this.y * other.z + @this.z * other.y + @this.w * other.x;
            var e5 = @this.y * other.w + @this.z * other.z + @this.w * other.y;
            var e6 = @this.z * other.w + @this.w * other.z;
            var e7 = @this.w * other.w;

            return new[] { e1, e2, e3, e4, e5, e6, e7 };
        }
    }
}