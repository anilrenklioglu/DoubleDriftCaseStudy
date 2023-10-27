﻿using UnityEngine;

namespace Development.Scripts.Utilities
{
    public class InputReader
    {
        private readonly Joystick _floatingJoystick;
        
        public InputReader()
        {
            _floatingJoystick = Object.FindObjectOfType<Joystick>();
        }
        
        /// <summary>
        /// Z input is 0 because it's not necessary.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetCurrentInput()
        {
            Vector3 input = new(_floatingJoystick.Horizontal, 0f, 0f);
            return input;
        }
        
        /// <summary>
        /// Checks if the screen is being touched.
        /// </summary>
        public bool IsScreenTouched()
        {
            // For simplicity, consider the screen is touched if there is at least one touch detected.
            return Input.touchCount > 0;
        }
    }
}