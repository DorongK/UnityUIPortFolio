using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomExtentions
{
    public static class StringExtensions
    {
        public static void FancyDebug(this string str)
        {
            Debug.LogFormat("this string contatins {0} characters.", str.Length);
        }
    }
}
