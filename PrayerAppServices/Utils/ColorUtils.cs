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


    }
}
