using System;

namespace BCEdit180.Core.Utils {
    public static class RenderUtils {
        public static byte MultiplyByte(byte a, double b) {
            return (byte) Maths.Clamp((int) Math.Round(a / 255d * b * 255d), 0, 255);
        }

        public static byte DoubleToByte(double value) {
            return (byte) Maths.Clamp((int) Math.Round(value / 255d), 0, 255);
        }
    }
}