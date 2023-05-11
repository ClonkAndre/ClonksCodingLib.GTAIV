using System;
using System.Numerics;

using IVSDKDotNet;

namespace CCL.GTAIV
{
    /// <summary>
    /// Contains extensions for the <see cref="Vector2"/> or <see cref="Vector3"/> structs.
    /// </summary>
    public static class VectorExtensions
    {

        /// <summary>
        /// Returns a <see cref="Vector3"/> with random X, Y and Z values.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns>A <see cref="Vector3"/> with random X, Y and Z values.</returns>
        public static Vector3 RandomXYZ(this Vector3 vec)
        {
            Random rnd = new Random();
            float x = (float)(rnd.NextDouble() - 0.5);
            float y = (float)(rnd.NextDouble() - 0.5);
            float z = (float)(rnd.NextDouble() - 0.5);
            return Vector3.Normalize(new Vector3(x, y, z));
        }

        /// <summary>
        /// Returns a <see cref="Vector3"/> with random X and Y values.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns>A <see cref="Vector3"/> with random X and Y values.</returns>
        public static Vector3 RandomXY(this Vector3 vec)
        {
            Random rnd = new Random();
            float x = (float)(rnd.NextDouble() - 0.5);
            float y = (float)(rnd.NextDouble() - 0.5);
            return Vector3.Normalize(new Vector3(x, y, 0f));
        }

        /// <summary>
        /// Calculate a position between the points specified by this <see cref="Vector3"/> and <paramref name="target"/>, moving no farther than the distance specified by <paramref name="maxDistanceDelta"/>.
        /// </summary>
        /// <param name="vec">The position to move from.</param>
        /// <param name="target">The position to move towards.</param>
        /// <param name="maxDistanceDelta">Distance to move per call.</param>
        /// <returns>The new position.</returns>
        public static Vector3 MoveTowards(this Vector3 vec, Vector3 target, float maxDistanceDelta)
        {
            Vector3 vector3 = target - vec;
            float magnitude = vector3.Length();
            return magnitude <= maxDistanceDelta || magnitude == 0.0 ? target : vec + vector3 / magnitude * maxDistanceDelta;
        }

        /// <summary>
        /// Calculates the yaw (Look Left/Right) angle from this <see cref="Vector3"/> and the given <paramref name="targetPos"/>.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="targetPos">The target pos to look at.</param>
        /// <returns>The calculated yaw angle.</returns>
        public static float LookAtYaw(this Vector3 vec, Vector3 targetPos)
        {
            float x = vec.X - targetPos.X;
            float y = vec.Y - targetPos.Y;
            //float z = vec.Z - targetPos.Z;
            float yawAngle = (float)Math.Atan2(-x, y);
            //float upDownAngle = (float)Math.Atan(-z);
            return Helper.RadianToDegree(yawAngle);
        }

        /// <summary>
        /// Gets a position around this <see cref="Vector3"/> by the given <paramref name="distance"/>.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="distance">The around distance.</param>
        /// <returns>The position around the current <see cref="Vector3"/>.</returns>
        public static Vector3 Around(this Vector3 vec, float distance)
        {
            return vec + Vector3.Zero.RandomXY() * distance;
        }

        /// <summary>
        /// Converts this <see cref="Vector3"/> to a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns>The new <see cref="Vector2"/>.</returns>
        public static Vector2 ToVector2(this Vector3 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

    }
}
