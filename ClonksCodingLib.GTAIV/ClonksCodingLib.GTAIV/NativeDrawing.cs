using System;
using System.Drawing;
using System.Numerics;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Gives you easy access to native functions that involve drawing.
    /// </summary>
    public class NativeDrawing
    {

        public static float GetAspectRatio()
        {
            GET_SCREEN_RESOLUTION(out int x, out int y);
            return (float)x / y;
        }

        /// <summary>
        /// Projects the given <see cref="Vector3"/> position in the world to a <see cref="Vector2"/> screen position.
        /// </summary>
        /// <param name="pos">The world pos to project.</param>
        /// <returns>The projected world position of <paramref name="pos"/>.</returns>
        public static Vector2 CoordToScreen(Vector3 pos)
        {
            GET_GAME_VIEWPORT_ID(out int viewportID);

            // Check if position is visible on screen
            if (!CAM_IS_SPHERE_VISIBLE(viewportID, pos, 2f))
                return Vector2.Zero;

            // Get 2D position of 3D coordinate
            GET_VIEWPORT_POSITION_OF_COORD(pos, viewportID, out Vector2 screenPos);

            return screenPos;
        }

        /// <summary>
        /// Draws some text on the screen using native functions.
        /// </summary>
        /// <param name="pos">The position of the text to be drawn on screen.</param>
        /// <param name="str">The string to display.</param>
        /// <param name="color">The color of the text.</param>
        /// <param name="size">The size of the text.</param>
        /// <param name="centeredText">If the text should be centered or not.</param>
        public static void DisplayText(Vector2 pos, string str, Color color, float size, bool centeredText)
        {
            float sizex = size * 0.01f * GetAspectRatio();
            SET_TEXT_BACKGROUND(false);
            SET_TEXT_FONT(0);
            SET_TEXT_EDGE(true, 0, 0, 0, 255);
            SET_TEXT_CENTRE(centeredText);
            SET_TEXT_DROPSHADOW(false, 0, 0, 0, 0);
            SET_TEXT_PROPORTIONAL(true);
            SET_TEXT_COLOUR(color.R, color.G, color.B, color.A);
            SET_TEXT_SCALE(sizex, size * 0.01f);
            SET_TEXT_EDGE(true, 0, 0, 0, 255);

            SET_TEXT_USE_UNDERSCORE(true);
            DISPLAY_TEXT_WITH_LITERAL_STRING(pos.X, pos.Y, "STRING", str);
        }

    }
}
