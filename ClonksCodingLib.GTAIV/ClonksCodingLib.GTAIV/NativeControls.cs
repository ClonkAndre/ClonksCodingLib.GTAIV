using System.Numerics;

using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    public class NativeControls
    {

        // IS_BUTTON_JUST_PRESSED
        // IS_BUTTON_PRESSED
        // SHAKE_PAD
        // GET_PAD_STATE
        // IS_MOUSE_BUTTON_JUST_PRESSED
        // IS_MOUSE_BUTTON_PRESSED
        // IS_MOUSE_USING_VERTICAL_INVERSION

        #region Properties
        public static Vector2 MousePosition
        {
            get {
                GET_MOUSE_POSITION(out int x, out int y);
                return new Vector2(x, y);
            }
        }
        public static Vector2 MouseInput
        {
            get {
                GET_MOUSE_INPUT(out int x, out int y);
                return new Vector2(x, y);
            }
        }
        public static int MouseWheel
        {
            get {
                GET_MOUSE_WHEEL(out int wheel);
                return wheel;
            }
        }
        public static float MouseSensitivity
        {
            get {
                return GET_MOUSE_SENSITIVITY();
            }
        }
        #endregion

        #region Methods
        private static void GetMovement(uint padIndex, out float x, out float y)
        {
            GetAnalogueStickPositions(padIndex, out Vector2 leftStick, out Vector2 rightStick);

            if (leftStick.X == 0f && leftStick.Y == 0f)
            {
                GetKeyboardMoveInput(out int x2, out int y2);
                x = x2;
                y = y2;
                return;
            }

            x = leftStick.X;
            y = leftStick.Y;
        }

        public static void GetAnalogueStickPositions(uint padIndex, out Vector2 stickLeft, out Vector2 stickRight)
        {
            GET_POSITION_OF_ANALOGUE_STICKS(padIndex, out int l1, out int r1, out int l2, out int r2);
            stickLeft = new Vector2(l1, r1);
            stickRight = new Vector2(l2, r2);
        }
        public static void GetKeyboardMoveInput(out int x, out int y)
        {
            GET_KEYBOARD_MOVE_INPUT(out x, out y);
        }
        public static bool GetPadPitchRoll(uint padIndex, out float pitch, out float roll)
        {
            return GET_PAD_PITCH_ROLL(padIndex, out pitch, out roll);
        }
        #endregion

        #region Functions
        public static bool IsUsingJoypad()
        {
            return IS_PC_USING_JOYPAD();
        }
        public static bool IsUsingController()
        {
            return IS_USING_CONTROLLER();
        }

        /// <summary>
        /// Gets if the given controller button was pressed.
        /// </summary>
        /// <param name="padIndex">Target controller. Default is 0.</param>
        /// <param name="button">Target button.</param>
        /// <returns>True if the given controller button was pressed. Otherwise, false.</returns>
        public static bool IsControllerButtonPressed(uint padIndex, ControllerButton button)
        {
            if (button == ControllerButton.NONE)
                return false;

            return IS_BUTTON_PRESSED(padIndex, (uint)button);
        }

        /// <summary>
        /// Gets the currently pressed controller key.
        /// </summary>
        /// <param name="padIndex">Target controller. Default is 0.</param>
        /// <returns>The pressed controller key. Returns <see cref="ControllerButton.NONE"/> if no button was pressed.</returns>
        public static ControllerButton GetPressedControllerButton(uint padIndex)
        {
            for (int i = 4; i <= 19; i++)
            {
                switch ((ControllerButton)i)
                {
                    case ControllerButton.BUTTON_BACK:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_BACK)) 
                            return ControllerButton.BUTTON_BACK;
                        break;
                    case ControllerButton.BUTTON_START:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_START)) 
                            return ControllerButton.BUTTON_START;
                        break;
                    case ControllerButton.BUTTON_A:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_A)) 
                            return ControllerButton.BUTTON_A;
                        break;
                    case ControllerButton.BUTTON_B:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_B)) 
                            return ControllerButton.BUTTON_B;
                        break;
                    case ControllerButton.BUTTON_X:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_X)) 
                            return ControllerButton.BUTTON_X;
                        break;
                    case ControllerButton.BUTTON_Y:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_Y)) 
                            return ControllerButton.BUTTON_Y;
                        break;
                    case ControllerButton.BUTTON_DPAD_UP:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_DPAD_UP)) 
                            return ControllerButton.BUTTON_DPAD_UP;
                        break;
                    case ControllerButton.BUTTON_DPAD_DOWN:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_DPAD_DOWN)) 
                            return ControllerButton.BUTTON_DPAD_DOWN;
                        break;
                    case ControllerButton.BUTTON_DPAD_LEFT:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_DPAD_LEFT)) 
                            return ControllerButton.BUTTON_DPAD_LEFT;
                        break;
                    case ControllerButton.BUTTON_DPAD_RIGHT:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_DPAD_RIGHT)) 
                            return ControllerButton.BUTTON_DPAD_RIGHT;
                        break;
                    case ControllerButton.BUTTON_TRIGGER_LEFT:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_TRIGGER_LEFT)) 
                            return ControllerButton.BUTTON_TRIGGER_LEFT;
                        break;
                    case ControllerButton.BUTTON_TRIGGER_RIGHT:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_TRIGGER_RIGHT)) 
                            return ControllerButton.BUTTON_TRIGGER_RIGHT;
                        break;
                    case ControllerButton.BUTTON_BUMPER_LEFT:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_BUMPER_LEFT)) 
                            return ControllerButton.BUTTON_BUMPER_LEFT;
                        break;
                    case ControllerButton.BUTTON_BUMPER_RIGHT:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_BUMPER_RIGHT)) 
                            return ControllerButton.BUTTON_BUMPER_RIGHT;
                        break;
                    case ControllerButton.BUTTON_STICK_LEFT:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_STICK_LEFT)) 
                            return ControllerButton.BUTTON_STICK_LEFT;
                        break;
                    case ControllerButton.BUTTON_STICK_RIGHT:
                        if (IsControllerButtonPressed(padIndex, ControllerButton.BUTTON_STICK_RIGHT)) 
                            return ControllerButton.BUTTON_STICK_RIGHT;
                        break;
                }
            }

            return ControllerButton.NONE;
        }

        /// <summary>
        /// Gets if the given game key is pressed.
        /// </summary>
        /// <param name="padIndex">Target controller. Default is 0.</param>
        /// <param name="key">Target key.</param>
        /// <returns>True if the given key was pressed. Otherwise, false.</returns>
        public static bool IsGameKeyPressed(uint padIndex, GameKey key)
        {
            if ((int)key < 1000)
            {
                return IS_CONTROL_PRESSED((int)padIndex, (int)key);
            }
            else
            {
                GetMovement(padIndex, out float x, out float y);
                switch (key)
                {
                    case GameKey.MoveForward:   return (y < -32);
                    case GameKey.MoveBackward:  return (y > 32);
                    case GameKey.MoveLeft:      return (x < -32);
                    case GameKey.MoveRight:     return (x > 32);
                }
                return false;
            }
        }
        #endregion

    }
}
