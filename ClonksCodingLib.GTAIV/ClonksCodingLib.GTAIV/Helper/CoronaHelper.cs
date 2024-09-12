using System;
using System.Drawing;
using System.Numerics;

using IVSDKDotNet;

namespace CCL.GTAIV
{
    /// <summary>
    /// Some corona helper functions.
    /// </summary>
    public class CoronaHelper
    {

        /// <summary>
        /// Renders a corona in the world.
        /// </summary>
        /// <param name="id">The id of the corona.</param>
        /// <param name="pos">The position of the corona.</param>
        /// <param name="color">The color of the corona.</param>
        /// <param name="size">The size of the corona.</param>
        public static void RenderCorona(int id, Vector3 pos, Color color, float size)
        {
            IVShadows.RenderCorona(
                id,
                color,
                20f,
                pos,
                size * 10f,
                450.0f,
                1.5f,
                0,
                4.0f,
                0,
                0,
                0);
        }

        /// <summary>
        /// Renders a corona in the world.
        /// </summary>
        /// <param name="pos">The position of the corona.</param>
        /// <param name="color">The color of the corona.</param>
        /// <param name="size">The size of the corona.</param>
        public static void RenderCorona(Vector3 pos, Color color, float size)
        {
            RenderCorona((int)(pos.X - -3000.0f) - (int)((pos.Y - -3000.0f) * -12001.0f), pos, color, size);
        }

    }
}
