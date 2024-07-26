using System;
using System.Collections.Generic;
using System.Windows.Forms;

using IVSDKDotNet;
using IVSDKDotNet.Enums;

namespace CCL.GTAIV
{
    /// <summary>
    /// Contains some helper functions for ImGui.
    /// </summary>
    public static class ImGuiHelper
    {

        #region Variables
        private static char[] keyModifierSplitChar = new char[] { '+' };
        private static Dictionary<string, eImGuiKey> keyMapping = new Dictionary<string, eImGuiKey>
        {
            // All keys from the System.Windows.Forms.Keys enum
            { "A", eImGuiKey.ImGuiKey_A },
            { "B", eImGuiKey.ImGuiKey_B },
            { "C", eImGuiKey.ImGuiKey_C },
            { "D", eImGuiKey.ImGuiKey_D },
            { "E", eImGuiKey.ImGuiKey_E },
            { "F", eImGuiKey.ImGuiKey_F },
            { "G", eImGuiKey.ImGuiKey_G },
            { "H", eImGuiKey.ImGuiKey_H },
            { "I", eImGuiKey.ImGuiKey_I },
            { "J", eImGuiKey.ImGuiKey_J },
            { "K", eImGuiKey.ImGuiKey_K },
            { "L", eImGuiKey.ImGuiKey_L },
            { "M", eImGuiKey.ImGuiKey_M },
            { "N", eImGuiKey.ImGuiKey_N },
            { "O", eImGuiKey.ImGuiKey_O },
            { "P", eImGuiKey.ImGuiKey_P },
            { "Q", eImGuiKey.ImGuiKey_Q },
            { "R", eImGuiKey.ImGuiKey_R },
            { "S", eImGuiKey.ImGuiKey_S },
            { "T", eImGuiKey.ImGuiKey_T },
            { "U", eImGuiKey.ImGuiKey_U },
            { "V", eImGuiKey.ImGuiKey_V }, 
            { "W", eImGuiKey.ImGuiKey_W },
            { "X", eImGuiKey.ImGuiKey_X },
            { "Y", eImGuiKey.ImGuiKey_Y },
            { "Z", eImGuiKey.ImGuiKey_Z },
            { "D0", eImGuiKey.ImGuiKey_0 },
            { "D1", eImGuiKey.ImGuiKey_1 },
            { "D2", eImGuiKey.ImGuiKey_2 },
            { "D3", eImGuiKey.ImGuiKey_3 },
            { "D4", eImGuiKey.ImGuiKey_4 },
            { "D5", eImGuiKey.ImGuiKey_5 },
            { "D6", eImGuiKey.ImGuiKey_6 },
            { "D7", eImGuiKey.ImGuiKey_7 },
            { "D8", eImGuiKey.ImGuiKey_8 },
            { "D9", eImGuiKey.ImGuiKey_9 },
            { "NumPad0", eImGuiKey.ImGuiKey_Keypad0 },
            { "NumPad1", eImGuiKey.ImGuiKey_Keypad1 },
            { "NumPad2", eImGuiKey.ImGuiKey_Keypad2 },
            { "NumPad3", eImGuiKey.ImGuiKey_Keypad3 },
            { "NumPad4", eImGuiKey.ImGuiKey_Keypad4 },
            { "NumPad5", eImGuiKey.ImGuiKey_Keypad5 },
            { "NumPad6", eImGuiKey.ImGuiKey_Keypad6 },
            { "NumPad7", eImGuiKey.ImGuiKey_Keypad7 },
            { "NumPad8", eImGuiKey.ImGuiKey_Keypad8 },
            { "NumPad9", eImGuiKey.ImGuiKey_Keypad9 },
            { "F1", eImGuiKey.ImGuiKey_F1 },
            { "F2", eImGuiKey.ImGuiKey_F2 },
            { "F3", eImGuiKey.ImGuiKey_F3 },
            { "F4", eImGuiKey.ImGuiKey_F4 },
            { "F5", eImGuiKey.ImGuiKey_F5 },
            { "F6", eImGuiKey.ImGuiKey_F6 },
            { "F7", eImGuiKey.ImGuiKey_F7 },
            { "F8", eImGuiKey.ImGuiKey_F8 },
            { "F9", eImGuiKey.ImGuiKey_F9 },
            { "F10", eImGuiKey.ImGuiKey_F10 },
            { "F11", eImGuiKey.ImGuiKey_F11 },
            { "F12", eImGuiKey.ImGuiKey_F12 },
            { "F13", eImGuiKey.ImGuiKey_F13 },
            { "F14", eImGuiKey.ImGuiKey_F14 },
            { "F15", eImGuiKey.ImGuiKey_F15 },
            { "F16", eImGuiKey.ImGuiKey_F16 },
            { "F17", eImGuiKey.ImGuiKey_F17 },
            { "F18", eImGuiKey.ImGuiKey_F18 },
            { "F19", eImGuiKey.ImGuiKey_F19 },
            { "F20", eImGuiKey.ImGuiKey_F20 },
            { "F21", eImGuiKey.ImGuiKey_F21 },
            { "F22", eImGuiKey.ImGuiKey_F22 },
            { "F23", eImGuiKey.ImGuiKey_F23 },
            { "F24", eImGuiKey.ImGuiKey_F24 },
            { "Left", eImGuiKey.ImGuiKey_LeftArrow },
            { "Right", eImGuiKey.ImGuiKey_RightArrow },
            { "Up", eImGuiKey.ImGuiKey_UpArrow },
            { "Down", eImGuiKey.ImGuiKey_DownArrow },
            { "Enter", eImGuiKey.ImGuiKey_Enter },
            { "Escape", eImGuiKey.ImGuiKey_Escape },
            { "Space", eImGuiKey.ImGuiKey_Space },
            { "Back", eImGuiKey.ImGuiKey_Backspace },
            { "Tab", eImGuiKey.ImGuiKey_Tab },
            { "Home", eImGuiKey.ImGuiKey_Home },
            { "End", eImGuiKey.ImGuiKey_End },
            { "PageUp", eImGuiKey.ImGuiKey_PageUp },
            { "PageDown", eImGuiKey.ImGuiKey_PageDown },
            { "Insert", eImGuiKey.ImGuiKey_Insert },
            { "Delete", eImGuiKey.ImGuiKey_Delete },
            { "Pause", eImGuiKey.ImGuiKey_Pause },
            { "CapsLock", eImGuiKey.ImGuiKey_CapsLock },
            { "Scroll", eImGuiKey.ImGuiKey_ScrollLock },
            { "PrintScreen", eImGuiKey.ImGuiKey_PrintScreen },
            { "NumLock", eImGuiKey.ImGuiKey_NumLock },

            { "ControlKey", eImGuiKey.ImGuiKey_LeftCtrl },  // Assuming left control
            { "LControlKey", eImGuiKey.ImGuiKey_LeftCtrl },
            { "RControlKey", eImGuiKey.ImGuiKey_RightCtrl },

            { "ShiftKey", eImGuiKey.ImGuiKey_LeftShift },  // Assuming left shift
            { "LShiftKey", eImGuiKey.ImGuiKey_LeftShift },
            { "RShiftKey", eImGuiKey.ImGuiKey_RightShift },

            { "Menu", eImGuiKey.ImGuiKey_LeftAlt },  // Assuming left alt
            { "LMenu", eImGuiKey.ImGuiKey_LeftAlt },
            { "RMenu", eImGuiKey.ImGuiKey_RightAlt },

            { "LWin", eImGuiKey.ImGuiKey_LeftSuper },
            { "RWin", eImGuiKey.ImGuiKey_RightSuper },

            { "OemSemicolon", eImGuiKey.ImGuiKey_Semicolon },
            { "OemPlus", eImGuiKey.ImGuiKey_Equal },
            { "OemComma", eImGuiKey.ImGuiKey_Comma },
            { "OemMinus", eImGuiKey.ImGuiKey_Minus },
            { "OemPeriod", eImGuiKey.ImGuiKey_Period },
            { "OemQuestion", eImGuiKey.ImGuiKey_Slash },
            { "Oemtilde", eImGuiKey.ImGuiKey_GraveAccent },
            { "OemOpenBrackets", eImGuiKey.ImGuiKey_LeftBracket },
            { "OemPipe", eImGuiKey.ImGuiKey_Backslash },
            { "OemCloseBrackets", eImGuiKey.ImGuiKey_RightBracket },
            { "OemQuotes", eImGuiKey.ImGuiKey_Apostrophe },

            // Gamepad
            { "GamepadStart", eImGuiKey.ImGuiKey_GamepadStart },
            { "GamepadBack", eImGuiKey.ImGuiKey_GamepadBack },
            { "GamepadFaceLeft", eImGuiKey.ImGuiKey_GamepadFaceLeft },
            { "GamepadFaceRight", eImGuiKey.ImGuiKey_GamepadFaceRight },
            { "GamepadFaceUp", eImGuiKey.ImGuiKey_GamepadFaceUp },
            { "GamepadFaceDown", eImGuiKey.ImGuiKey_GamepadFaceDown },
            { "GamepadDpadLeft", eImGuiKey.ImGuiKey_GamepadDpadLeft },
            { "GamepadDpadRight", eImGuiKey.ImGuiKey_GamepadDpadRight },
            { "GamepadDpadUp", eImGuiKey.ImGuiKey_GamepadDpadUp },
            { "GamepadDpadDown", eImGuiKey.ImGuiKey_GamepadDpadDown },
            { "GamepadL1", eImGuiKey.ImGuiKey_GamepadL1 },
            { "GamepadR1", eImGuiKey.ImGuiKey_GamepadR1 },
            { "GamepadL2", eImGuiKey.ImGuiKey_GamepadL2 },
            { "GamepadR2", eImGuiKey.ImGuiKey_GamepadR2 },
            { "GamepadL3", eImGuiKey.ImGuiKey_GamepadL3 },
            { "GamepadR3", eImGuiKey.ImGuiKey_GamepadR3 },
            { "GamepadLStickLeft", eImGuiKey.ImGuiKey_GamepadLStickLeft },
            { "GamepadLStickRight", eImGuiKey.ImGuiKey_GamepadLStickRight },
            { "GamepadLStickUp", eImGuiKey.ImGuiKey_GamepadLStickUp },
            { "GamepadLStickDown", eImGuiKey.ImGuiKey_GamepadLStickDown },
            { "GamepadRStickLeft", eImGuiKey.ImGuiKey_GamepadRStickLeft },
            { "GamepadRStickRight", eImGuiKey.ImGuiKey_GamepadRStickRight },
            { "GamepadRStickUp", eImGuiKey.ImGuiKey_GamepadRStickUp },
            { "GamepadRStickDown", eImGuiKey.ImGuiKey_GamepadRStickDown },

            // Mouse
            { "LButton", eImGuiKey.ImGuiKey_MouseLeft },
            { "RButton", eImGuiKey.ImGuiKey_MouseRight },
            { "MButton", eImGuiKey.ImGuiKey_MouseMiddle },
            { "XButton1", eImGuiKey.ImGuiKey_MouseX1 },
            { "XButton2", eImGuiKey.ImGuiKey_MouseX2 },
            { "MouseWheelX", eImGuiKey.ImGuiKey_MouseWheelX },
            { "MouseWheelY", eImGuiKey.ImGuiKey_MouseWheelY }
        };
        #endregion

        /// <summary>
        /// Tries to convert the given <paramref name="key"/> to an <see cref="eImGuiKey"/> key.
        /// </summary>
        /// <param name="key">The key you want to convert to a <see cref="eImGuiKey"/> key.</param>
        /// <returns>The converted key. Can return <see cref="eImGuiKey.ImGuiKey_None"/> if the given <paramref name="key"/> could not be converted.</returns>
        public static eImGuiKey ConvertKeyToImGuiKey(Keys key)
        {
            string keyName = key.ToString();

            if (keyMapping.TryGetValue(keyName, out eImGuiKey foundKey))
                return foundKey;

            return eImGuiKey.ImGuiKey_None;
        }
        /// <summary>
        /// Tries to convert the given <paramref name="key"/> to an <see cref="eImGuiKey"/> key.
        /// </summary>
        /// <param name="key">The name of the key you want to convert to a <see cref="eImGuiKey"/> key.</param>
        /// <returns>The converted key. Can return <see cref="eImGuiKey.ImGuiKey_None"/> if the given <paramref name="key"/> could not be converted.</returns>
        public static eImGuiKey ConvertKeyToImGuiKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return eImGuiKey.ImGuiKey_None;
            
            if (keyMapping.TryGetValue(key, out eImGuiKey foundKey))
                return foundKey;

            return eImGuiKey.ImGuiKey_None;
        }

        /// <summary>
        /// Helper function which checks if a key is pressed.
        /// <para>
        /// This also allows you to check if a key was pressed with a modifier key.<br/>
        /// Example <b>without</b> modifier: Passing "<b>P</b>" to <paramref name="key"/> would check if "<b>P</b>" was pressed.<br/>
        /// Example <b>with</b> modifier: Passing "<b>LControlKey+P</b>" to <paramref name="key"/> would check if "<b>P</b>" was pressed with "<b>LControlKey</b>".<br/>
        /// </para>
        /// </summary>
        /// <param name="key">The key that you want to check.</param>
        /// <param name="repeat">If set to <see langword="true"/>, this function keeps returning <see langword="true"/> if the specified <paramref name="key"/> is currently pressed. Otherwise it will only return <see langword="true"/> once.</param>
        /// <returns><see langword="true"/> if the specified <paramref name="key"/> was pressed. Otherwise, <see langword="false"/>.</returns>
        public static bool IsKeyPressed(string key, bool repeat = true)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            // Check if contains modifier or not
            if (key.Contains("+"))
            {
                string[] keys = key.Split(keyModifierSplitChar, StringSplitOptions.RemoveEmptyEntries);

                if (keys.Length != 2)
                {
                    eImGuiKey imGuiKey = ConvertKeyToImGuiKey(keys[0].Trim());

                    if (imGuiKey == eImGuiKey.ImGuiKey_None)
                        return false;

                    return ImGuiIV.IsKeyPressed(imGuiKey, repeat);
                }
                else
                {
                    eImGuiKey modifier =    ConvertKeyToImGuiKey(keys[0].Trim());
                    eImGuiKey imGuiKey =    ConvertKeyToImGuiKey(keys[1].Trim());

                    if (modifier == eImGuiKey.ImGuiKey_None || imGuiKey == eImGuiKey.ImGuiKey_None)
                        return false;

                    return ImGuiIV.IsKeyDown(modifier) && ImGuiIV.IsKeyPressed(imGuiKey, repeat);
                }
            }
            else
            {
                eImGuiKey imGuiKey = ConvertKeyToImGuiKey(key);

                if (imGuiKey == eImGuiKey.ImGuiKey_None)
                    return false;

                return ImGuiIV.IsKeyPressed(imGuiKey, repeat);
            }
        }

    }
}
