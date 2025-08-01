using System;
using System.Drawing;
using System.Numerics;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    public static class NativeScreen
    {

        /// <summary>
        /// Retrieves the current screen resolution of the primary display in pixels.
        /// </summary>
        /// <remarks>The returned resolution corresponds to the primary display's current settings. This
        /// method does not account for multiple displays or scaling factors.</remarks>
        /// <returns>A <see cref="Size"/> structure representing the width and height, in pixels, of the primary display's resolution.</returns>
        public static Size GetScreenResolution()
        {
            GET_SCREEN_RESOLUTION(out int x, out int y);
            return new Size(x, y);
        }

        /// <summary>
        /// Retrieves the physical screen resolution of the primary display in pixels.
        /// </summary>
        /// <remarks>This method returns the actual physical resolution of the screen.</remarks>
        /// <returns>A <see cref="Size"/> structure representing the width and height of the primary display in pixels.</returns>
        public static Size GetPhysicalScreenResolution()
        {
            GET_PHYSICAL_SCREEN_RESOLUTION(out int x, out int y);
            return new Size(x, y);
        }

        /// <summary>
        /// Calculates the aspect ratio of the current screen resolution.
        /// </summary>
        /// <remarks>The aspect ratio is determined based on the current screen resolution retrieved by
        /// the <see cref="GetScreenResolution"/> method.</remarks>
        /// <returns>A <see cref="float"/> representing the aspect ratio, calculated as the width divided by the height of the screen resolution.</returns>
        public static float GetAspectRatio()
        {
            Size res = GetScreenResolution();
            return (float)res.Width / res.Height;
        }

        /// <summary>
        /// Gets whether the current display is in widescreen mode.
        /// </summary>
        /// <returns><see langword="true"/> if the display is in widescreen mode; otherwise, <see langword="false"/>.</returns>
        public static bool GetIsWidescreen()
        {
            return GET_IS_WIDESCREEN();
        }

        /// <summary>
        /// Determines whether widescreen borders are currently active.
        /// </summary>
        /// <returns><see langword="true"/> if widescreen borders are active; otherwise, <see langword="false"/>.</returns>
        public static bool AreWidescreenBordersActive()
        {
            return ARE_WIDESCREEN_BORDERS_ACTIVE();
        }

        /// <summary>
        /// Fades the screen out over the specified duration and checks if the screen is fully faded out.
        /// </summary>
        /// <remarks>If the screen is already faded out, or in the process of fading out, this method does not initiate a new fade-out.</remarks>
        /// <param name="timeInMs">The duration, in milliseconds, over which the screen should fade out.</param>
        /// <returns><see langword="true"/> if the screen is fully faded out; otherwise, <see langword="false"/>.</returns>
        public static bool DoAndCheckFadeScreenOut(uint timeInMs)
        {
            if (!IS_SCREEN_FADED_OUT() && !IS_SCREEN_FADING_OUT())
            {
                DO_SCREEN_FADE_OUT(timeInMs);
            }

            return IS_SCREEN_FADED_OUT();
        }
        /// <summary>
        /// Fades the screen in over the specified duration and checks if the screen is fully faded in.
        /// </summary>
        /// <remarks>If the screen is already faded in, or in the process of fading in, this method does not initiate a new fade-in.</remarks>
        /// <param name="timeInMs">The duration, in milliseconds, over which the screen should fade in.</param>
        /// <returns><see langword="true"/> if the screen is fully faded in; otherwise, <see langword="false"/>.</returns>
        public static bool DoAndCheckFadeScreenIn(uint timeInMs)
        {
            if (!IS_SCREEN_FADED_IN() && !IS_SCREEN_FADING_IN())
            {
                DO_SCREEN_FADE_IN(timeInMs);
            }

            return IS_SCREEN_FADED_IN();
        }

        /// <summary>
        /// Projects a 3D world coordinate to a 2D screen coordinate.
        /// </summary>
        /// <remarks>This method checks if the specified 3D coordinate is visible within the current
        /// viewport before projecting it. If the coordinate is not visible, the method returns <see
        /// cref="Vector2.Zero"/>.</remarks>
        /// <param name="worldCoord">The 3D world coordinate to project.</param>
        /// <returns>A <see cref="Vector2"/> representing the 2D screen coordinate corresponding to the given 3D world
        /// coordinate. Returns <see cref="Vector2.Zero"/> if the position is not visible on the screen.</returns>
        public static Vector2 ProjectWorldCoordToScreenCoord(Vector3 worldCoord)
        {
            GET_GAME_VIEWPORT_ID(out int viewportID);

            // Check if position is visible on screen
            if (!CAM_IS_SPHERE_VISIBLE(viewportID, worldCoord, 2f))
                return Vector2.Zero;

            // Get 2D position of 3D coordinate
            GET_VIEWPORT_POSITION_OF_COORD(worldCoord, viewportID, out Vector2 screenCoord);

            return screenCoord;
        }

        /// <summary>
        /// Attempts to project a 3D world coordinate to a 2D screen coordinate.
        /// </summary>
        /// <remarks>This method returns <see langword="false"/> if the projection results in an invalid
        /// screen coordinate, in which case <paramref name="screenCoord"/> will be set to <see cref="Vector2.Zero"/>.</remarks>
        /// <param name="worldCoord">The 3D world coordinate to project.</param>
        /// <param name="screenCoord">When this method returns, contains the 2D screen coordinate corresponding to the specified world
        /// coordinate, if the projection is successful; otherwise, contains <see cref="Vector2.Zero"/>.</param>
        /// <returns><see langword="true"/> if the projection is successful; otherwise, <see langword="false"/>.</returns>
        public static bool TryProjectWorldCoordToScreenCoord(Vector3 worldCoord, out Vector2 screenCoord)
        {
            Vector2 sPos = ProjectWorldCoordToScreenCoord(worldCoord);

            if (sPos == Vector2.Zero)
            {
                screenCoord = Vector2.Zero;
                return false;
            }

            screenCoord = sPos;
            return true;
        }

        /// <summary>
        /// Projects a 2D screen coordinate into a 3D world coordinate based on the camera's position, rotation, and a specified range.
        /// </summary>
        /// <remarks>This method calculates the 3D world position that corresponds to a given 2D screen
        /// coordinate, taking into account the camera's position, rotation, and a specified projection range. The
        /// method assumes a perspective projection and applies adjustments for camera roll. If the projection cannot be
        /// calculated, the method defaults to returning a point directly in front of the camera at the specified range.</remarks>
        /// <param name="camPos">The position of the camera in world space.</param>
        /// <param name="camRot">The rotation of the camera in world space, represented as a vector of Euler angles (in degrees).</param>
        /// <param name="screenCoord">The 2D screen coordinate to project.</param>
        /// <param name="range">The distance from the camera at which the projection occurs. Defaults to 10 units.</param>
        /// <returns>A <see cref="Vector3"/> representing the 3D world coordinate corresponding to the given screen coordinate.</returns>
        public static Vector3 ProjectScreenCoordToWorldCoord(Vector3 camPos, Vector3 camRot, Vector2 screenCoord, float range = 10f)
        {
            Vector3 camForward = Helper.RotationToDirection(camRot);
            Vector3 rotUp = camRot + new Vector3(10, 0, 0);
            Vector3 rotDown = camRot + new Vector3(-10, 0, 0);
            Vector3 rotLeft = camRot + new Vector3(0, 0, -10);
            Vector3 rotRight = camRot + new Vector3(0, 0, 10);

            Vector3 camRight = Helper.RotationToDirection(rotRight) - Helper.RotationToDirection(rotLeft);
            Vector3 camUp = Helper.RotationToDirection(rotUp) - Helper.RotationToDirection(rotDown);

            float rollRad = -Helper.DegreeToRadian(camRot.Y);

            Vector3 camRightRoll = camRight * (float)Math.Cos(rollRad) - camUp * (float)Math.Sin(rollRad);
            Vector3 camUpRoll = camRight * (float)Math.Sin(rollRad) + camUp * (float)Math.Cos(rollRad);

            Vector3 point3D = camPos + camForward * range + camRightRoll + camUpRoll;

            if (!TryProjectWorldCoordToScreenCoord(point3D, out Vector2 point2D))
                return camPos + camForward * range;

            Vector3 point3DZero = camPos + camForward * range;

            if (!TryProjectWorldCoordToScreenCoord(point3DZero, out Vector2 point2DZero))
                return camPos + camForward * range;

            const double eps = 0.001;
            if (Math.Abs(point2D.X - point2DZero.X) < eps || Math.Abs(point2D.Y - point2DZero.Y) < eps)
                return camPos + camForward * range;

            float scaleX = (screenCoord.X - point2DZero.X) / (point2D.X - point2DZero.X);
            float scaleY = (screenCoord.Y - point2DZero.Y) / (point2D.Y - point2DZero.Y);

            Vector3 point3Dret = camPos + camForward * range + camRightRoll * scaleX + camUpRoll * scaleY;
            return point3Dret;
        }

    }
}
