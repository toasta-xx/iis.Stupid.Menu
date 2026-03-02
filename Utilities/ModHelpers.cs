using iiMenu.Menu;

namespace iiMenu.Utilities
{
    public static class ModHelpers
    {
        public static string FormatModeLabel(string label, object value) =>
            $"{label} <color=grey>[</color><color=green>{value}</color><color=grey>]</color>";

        public static void CycleMode(ref int index, string[] names, string buttonKey, bool positive)
        {
            if (positive) index++; else index--;
            if (index >= names.Length) index = 0;
            if (index < 0) index = names.Length - 1;
            Buttons.GetIndex(buttonKey).overlapText = FormatModeLabel(buttonKey, names[index]);
        }

        public static void CycleMode(ref int index, string[] names, string buttonKey, string labelKey, bool positive)
        {
            if (positive) index++; else index--;
            if (index >= names.Length) index = 0;
            if (index < 0) index = names.Length - 1;
            Buttons.GetIndex(buttonKey).overlapText = FormatModeLabel(labelKey, names[index]);
        }

        public static void CycleInt(ref int value, int min, int max, string buttonKey, bool positive, string label = null)
        {
            if (positive) value++; else value--;
            if (value > max) value = min;
            if (value < min) value = max;
            Buttons.GetIndex(buttonKey).overlapText = FormatModeLabel(label ?? buttonKey, value);
        }
    }
}
