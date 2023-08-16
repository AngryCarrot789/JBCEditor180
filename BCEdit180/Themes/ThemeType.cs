using System;

namespace BCEdit180.Themes {
    public enum ThemeType {
        DeepDark,
        SoftDark,
        DarkGreyTheme,
        GreyTheme,
        RedBlackTheme,
        LightTheme,
    }

    public static class ThemeTypeExtension {
        public static string GetName(this ThemeType type) {
            switch (type) {
                case ThemeType.SoftDark: return "SoftDark";
                case ThemeType.RedBlackTheme: return "RedBlackTheme";
                case ThemeType.DeepDark: return "DeepDark";
                case ThemeType.GreyTheme: return "GreyTheme";
                case ThemeType.DarkGreyTheme: return "DarkGreyTheme";
                case ThemeType.LightTheme: return "LightTheme";
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}