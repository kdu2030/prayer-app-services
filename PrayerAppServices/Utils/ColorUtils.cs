using System.Text.RegularExpressions;

namespace PrayerAppServices.Utils {
    public static class ColorUtils {
        public static int ColorHexStringToInt(string colorHex) {
            if (!Regex.IsMatch(colorHex, "#[0-9a-fA-F]{6}")) {
                throw new ArgumentException("colorHex argument must be a valid hex string");
            }
            string digitsStr = colorHex.Substring(1);
            return int.Parse(digitsStr, System.Globalization.NumberStyles.HexNumber);
        }

        public static string ColorIntToHexString(int colorInt) {
            int red = (colorInt & 0xFF0000) >> 16;
            int green = (colorInt & 0xFF00) >> 8;
            int blue = colorInt & 0xFF;
            return $"#{red:x2}{green:x2}{blue:x2}";
        }
    }
}
