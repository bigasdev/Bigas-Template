﻿using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkParsedBool : ILDtkValueParser
    {
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsBool;

        public object ImportString(object input)
        {
            //bool can never be null but just in case
            if (input == null)
            {
                Debug.LogWarning("LDtk: Bool field was unexpectedly null");
                return false;
            }
            
            return Convert.ToBoolean(input);
        }
    }
}