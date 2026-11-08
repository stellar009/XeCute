using UnityEngine;

namespace XeCute.Utils
{
    /// <summary>
    /// Static helper for clean-minimal style colors. Use in inspectors or at runtime.
    /// Clean minimal palette: white bg, single accent (deep teal), black text.
    /// </summary>
    public static class XeCutePalette
    {
        public static readonly Color Background = HexColor("#FFFFFF");
        public static readonly Color CardBack = HexColor("#F0F0F0");
        public static readonly Color CardFace = HexColor("#FFFFFF");
        public static readonly Color Accent = HexColor("#1A6E7E");       // deep teal
        public static readonly Color TextPrimary = HexColor("#1A1A1A");
        public static readonly Color TextSecondary = HexColor("#666666");
        public static readonly Color Win = HexColor("#2E8B57");          // sea green
        public static readonly Color Lose = HexColor("#C0392B");         // firebrick
        public static readonly Color Bonus = HexColor("#D4875A");        // graphite orange
        public static readonly Color PanelBg = HexColor("#F8F8F8");
        public static readonly Color PanelBorder = HexColor("#DDDDDD");

        public static Color HexColor(string hex)
        {
            if (string.IsNullOrEmpty(hex) || hex[0] != '#') return Color.white;
            hex = hex.Substring(1);
            if (hex.Length != 6) return Color.white;
            byte r = System.Convert.ToByte(hex.Substring(0, 2), 16);
            byte g = System.Convert.ToByte(hex.Substring(2, 2), 16);
            byte b = System.Convert.ToByte(hex.Substring(4, 2), 16);
            return new Color32(r, g, b, 255);
        }
    }
}
