using System;
using System.Drawing;
using System.Numerics;

using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Gives you easy access to native functions that involve drawing.
    /// </summary>
    public class NativeDrawing
    {

        #region Obsolete
        [Obsolete("Might be removed in the future. Use 'NativeScreen.GetAspectRatio' instead.")]
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
        [Obsolete("Might be removed in the future. Use 'NativeScreen.ProjectWorldCoordToScreenCoord' instead.")]
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
        #endregion

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
            float sizex = size * 0.01f * NativeScreen.GetAspectRatio();
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


        /// <summary>
        /// Displays a string on the screen at the specified screen coordinates.
        /// </summary>
        /// <param name="str">The string to display.</param>
        /// <param name="screenCoord">The screen coordinates which determine where the string will be displayed at.</param>
        /// <param name="color">The color of the displayed string.</param>
        /// <param name="size">The size of the displayed string.</param>
        public static void DisplayStringAtScreenPosition(string str, Vector2 screenCoord, Color color, float size = 10f)
        {
            GET_SCREEN_RESOLUTION(out Vector2 res);
            Vector2 finalPos = screenCoord / res;

            float w = size * 0.02f * NativeScreen.GetAspectRatio();
            SET_TEXT_BACKGROUND(false);
            SET_TEXT_FONT(0u);
            SET_TEXT_EDGE(true, 0u, 0u, 0u, 255u);
            SET_TEXT_CENTRE(false);
            SET_TEXT_DROPSHADOW(false, 0u, 0u, 0u, 0u);
            SET_TEXT_PROPORTIONAL(value: true);
            SET_TEXT_COLOUR(color.R, color.G, color.B, color.A);
            SET_TEXT_SCALE(w, size * 0.03f);
            SET_TEXT_EDGE(true, 0u, 0u, 0u, 255u);
            SET_TEXT_USE_UNDERSCORE(true);

            DISPLAY_TEXT_WITH_LITERAL_STRING(finalPos.X, finalPos.Y, "STRING", str);
        }

        /// <summary>
        /// Displays a string on the screen at the specified world coordinates.
        /// </summary>
        /// <param name="str">The string to display.</param>
        /// <param name="worldCoord">The world coordinates which determine where the string will be displayed at.</param>
        /// <param name="color">The color of the displayed string.</param>
        /// <param name="size">The size of the displayed string.</param>
        public static void DisplayStringAtWorldPosition(string str, Vector3 worldCoord, Color color, float size = 10f)
        {
            // World pos to screen pos
            Vector2 screenPos = NativeScreen.ProjectWorldCoordToScreenCoord(worldCoord);

            if (screenPos == Vector2.Zero)
                return;

            DisplayStringAtScreenPosition(str, screenPos, color, size);
        }

        /// <summary>
        /// Displays multiple strings, stacked from top to bottom, on the screen at the specified world coordinates.
        /// </summary>
        /// <remarks>Useful for displaying debug information, e.g. visualizing the ragdoll state of a ped.</remarks>
        /// <param name="strings">An array of strings to display.</param>
        /// <param name="worldCoord">The world coordinates which determine where the strings will be displayed at.</param>
        /// <param name="color">The color of the displayed strings.</param>
        /// <param name="size">The size of the displayed strings.</param>
        public static void DisplayListOfStringsAtWorldPosition(string[] strings, Vector3 worldCoord, Color color, float size = 10f)
        {
            // World pos to screen pos
            Vector2 screenPos = NativeScreen.ProjectWorldCoordToScreenCoord(worldCoord);

            if (screenPos == Vector2.Zero)
                return;

            for (int i = 0; i < strings.Length; i++)
            {
                DisplayStringAtScreenPosition(strings[i], screenPos + new Vector2(0f, i * 15.5f), color, size);
            }
        }

        /// <summary>
        /// Draws a rectangle on the screen with the specified dimensions, color, and origin point.
        /// </summary>
        /// <remarks>The rectangle's position and size are normalized to screen coordinates before being
        /// drawn. The <paramref name="origin"/> parameter determines how the rectangle's position is interpreted
        /// relative to its dimensions. For example, if <see cref="RectangleOrigin.Center"/> is used, the rectangle is
        /// centered at the specified position.</remarks>
        /// <param name="rect">The rectangle to draw, specified as a <see cref="RectangleF"/> structure containing the position and size.</param>
        /// <param name="color">The color of the rectangle, specified as a <see cref="Color"/> structure.</param>
        /// <param name="origin">The origin point of the rectangle, specified as a <see cref="RectangleOrigin"/> value. The default is <see cref="RectangleOrigin.Center"/>.</param>
        public static void DrawRectangle(RectangleF rect, Color color, RectangleOrigin origin = RectangleOrigin.Center)
        {
            Vector2 pos     = new Vector2(rect.X, rect.Y);
            Vector2 size    = new Vector2(rect.Width, rect.Height);

            switch (origin)
            {
                case RectangleOrigin.TopLeft:
                    pos = pos + (size * 0.5f);
                    break;
                case RectangleOrigin.TopRight:
                    pos = pos - new Vector2(size.X * 0.5f, 0f) + new Vector2(0f, size.Y * 0.5f);
                    break;
                case RectangleOrigin.BottomLeft:
                    pos = pos - new Vector2(0f, size.Y * 0.5f) + new Vector2(size.X * 0.5f, 0f);
                    break;
                case RectangleOrigin.BottomRight:
                    pos = pos - (size * 0.5f);
                    break;
            }

            // Convert to normalized coordinates
            GET_SCREEN_RESOLUTION(out Vector2 res);
            Vector2 vec1 = pos / res;
            Vector2 vec2 = size / res;

            DRAW_RECT(vec1.X, vec1.Y, vec2.X, vec2.Y, color.R, color.G, color.B, color.A);
        }


    }
}
