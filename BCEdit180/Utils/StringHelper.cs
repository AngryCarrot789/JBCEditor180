namespace BCEdit180.Utils {
    public static class StringHelper {
        public static bool IsEmpty(this string value) {
            return string.IsNullOrEmpty(value);
        }
    }
}