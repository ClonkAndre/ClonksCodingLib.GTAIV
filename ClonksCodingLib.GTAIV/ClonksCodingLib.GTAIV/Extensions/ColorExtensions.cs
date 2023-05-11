using System;
using System.Drawing;

namespace CCL.GTAIV
{
    /// <summary>
    /// Contains extensions for the <see cref="Color"/> struct.
    /// </summary>
    public static class ColorExtensions
    {

        /// <summary>
        /// Converts the given <see cref="Color"/> struct to a 32-bit RGBA value for GTA IV.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static int ToRgba(this Color color)
        {
            return Color.FromArgb(color.R, color.G, color.B, color.A).ToArgb();
        }

        /// <summary>
        /// Gives a random ARGB <see cref="Color"/>.
        /// </summary>
        /// <param name="color"></param>
        /// <returns>The random <see cref="Color"/>.</returns>
        public static Color RandomARGB(this Color color)
        {
            Random rnd = new Random();
            return Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
        }
        /// <summary>
        /// Gives a random RGB <see cref="Color"/>.
        /// </summary>
        /// <param name="color"></param>
        /// <returns>The random <see cref="Color"/>.</returns>
        public static Color RandomRGB(this Color color)
        {
            Random rnd = new Random();
            return Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
        }

    }
}
