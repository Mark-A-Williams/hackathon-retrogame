using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public static class Colours
    {
        public static List<string> _colours = new List<string>()
        {
            "#000000", "#FF0000", "#00FF00", "#0000FF", "#800000", "#FFFF00", "#00FFFF", "#F0F0F0", "#101010"
        };

        public static string GetColour(int index)
        {
            if (index < _colours.Count - 1)
            {
                return _colours[index];
            } else
            {
                return GetColour(index - _colours.Count - 1);
            }
        }

    }
}
