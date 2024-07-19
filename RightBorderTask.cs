using System;
using System.Collections.Generic;

namespace Autocomplete

{
    public class RightBorderTask
    {
        public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix,
                                              int left, int right)
        {
            while (right - left > 1)
            {
                var i = (right - left) / 2 + left;

                if (string.Compare(prefix, phrases[i], StringComparison.OrdinalIgnoreCase) >= 0
                    || phrases[i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    left = i;
                }
                else
                {
                    right = i;
                }
            }

            return right;
        }
    }
}