using System;

namespace CCL.GTAIV.Extensions
{
    /// <summary>
    /// Contains extensions for the <see cref="float"/> struct.
    /// </summary>
    public static class FloatExtensions
    {

        /// <summary>
        /// Checks if the <see cref="float"/> value is in between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to check for.</param>
        /// <param name="min">The minimum <see cref="float"/> value.</param>
        /// <param name="max">The maximum <see cref="float"/> value.</param>
        /// <returns><see langword="true"/> if the <see cref="float"/> value is in between <paramref name="min"/> and <paramref name="max"/>. Otherwise, <see langword="false"/>.</returns>
        public static bool InBetween(this float value, float min, float max)
        {
            return value > min && value < max;
        }

        /// <summary>
        /// Performs a linear interpolation between two floats based on the given weighting.
        /// </summary>
        /// <param name="a">Start</param>
        /// <param name="b">End</param>
        /// <param name="t">Weighting</param>
        /// <returns>The lerped <see cref="float"/>.</returns>
        public static float Lerp(this float a, float b, float t)
        {
            // Clamp t between 0 and 1
            t = Math.Max(0.0f, Math.Min(1.0f, t));

            return a + (b - a) * t;
        }

        /// <summary>
        /// Eases in and out the <see cref="float"/> value smoothly, meaning that the animation starts slowly, accelerates, then decelerates towards the end.
        /// </summary>
        /// <param name="t">The value that should be eased.</param>
        /// <param name="accelerationAmount">The acceleration amount.</param>
        /// <param name="decelerationAmount">The deceleration amount.</param>
        /// <returns>The eased value.</returns>
        public static float EaseInOut(this float t, float accelerationAmount, float decelerationAmount)
        {
            float accelerationEasing = (float)Math.Pow(t, accelerationAmount) / (float)(Math.Pow(t, accelerationAmount) + Math.Pow(1f - t, accelerationAmount));
            float decelerationEasing = (float)Math.Pow(t, decelerationAmount) / (float)(Math.Pow(t, decelerationAmount) + Math.Pow(1f - t, decelerationAmount));

            return accelerationEasing * (1f - decelerationAmount) + decelerationEasing * decelerationAmount;
        }

        /// <summary>
        /// Eases in the given <see cref="float"/> value smoothly, meaning that the animation starts slowly and then accelerates.
        /// </summary>
        /// <param name="t">The value that should be eased.</param>
        /// <param name="amount">The acceleration amount.</param>
        /// <returns>The eased value.</returns>
        public static float EaseIn(this float t, float amount)
        {
            return (float)Math.Pow(t, amount);
        }

        /// <summary>
        /// Eases out the given <see cref="float"/> value smoothly, meaning that the animation decelerates towards the end.
        /// </summary>
        /// <param name="t">The value that should be eased.</param>
        /// <param name="amount">The deceleration amount.</param>
        /// <returns>The eased value.</returns>
        public static float EaseOut(this float t, float amount)
        {
            return 1.0f - (float)Math.Pow(1.0f - t, amount);
        }

    }
}
