﻿using System;
using UnityEngine;

namespace LDtkUnity
{
    internal static class LDtkInterfaceEvent
    {
        public static void TryEvent<T>(MonoBehaviour[] behaviors, Action<T> action)
        {
            foreach (MonoBehaviour component in behaviors)
            {
                if (!(component is T thing))
                {
                    continue;
                }
                
                try
                {
                    action.Invoke(thing);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
    }
}