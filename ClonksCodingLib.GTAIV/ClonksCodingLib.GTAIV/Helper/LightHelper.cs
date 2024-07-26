using System;
using System.Drawing;
using System.Numerics;

using IVSDKDotNet;
using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Some light helper functions.
    /// </summary>
    public class LightHelper
    {

        /// <summary>
        /// Adds a point light to the world.
        /// </summary>
        /// <param name="pos">The light position in the world.</param>
        /// <param name="color">The color of the light.</param>
        /// <param name="intensity">The light intensity.</param>
        /// <param name="range">The light range.</param>
        /// <param name="castShadow">If the light casts a shadow.</param>
        /// <param name="lightId">If you want the light to cast a shadow, you should give it the pointer of the player ped. Otherwise this can be <see cref="UIntPtr.Zero"/>.</param>
        public static void AddPointLight(
            Vector3 pos,
            Color color,
            float intensity,
            float range,
            bool castShadow,
            UIntPtr lightId)
        {
            eLightFlags newFlags = eLightFlags.Vehicle | eLightFlags.InteriorOnly | eLightFlags.ExteriorOnly;

            uint id = 0;

            if (lightId != UIntPtr.Zero)
            {
                // Set flags
                if (castShadow)
                    newFlags = newFlags | eLightFlags.CastShadow;

                // Set light id
                id = lightId.ToUInt32();
            }

            // Get interior id at pos
            GET_INTERIOR_AT_COORDS(pos, out int interiorId);

            // Add light
            IVShadows.AddSceneLight(
                0,
                (uint)eLightType.Point,
                (uint)newFlags,
                -Vector3.UnitZ,
                Vector3.UnitY,
                pos,
                color,
                intensity,
                0,
                0,
                range,
                0f,
                0f,
                0f,
                0f,
                interiorId,
                0,
                id);
        }

        /// <summary>
        /// Adds a spot light to the world.
        /// </summary>
        /// <param name="source">The light source position.</param>
        /// <param name="target">The target position the light should be pointing at.</param>
        /// <param name="color">The color of the light.</param>
        /// <param name="intensity">The light intensity.</param>
        /// <param name="range">The light range.</param>
        /// <param name="innerConeAngle">The inner cone angle of the spot light.</param>
        /// <param name="outerConeAngle">The outer cone angle of the spot light.</param>
        /// <param name="volIntensity">If you want the light to be volumetric, you can set the intensity (brightness) here.</param>
        /// <param name="volSizeScale">The scale of the volume (1.0f means the same radius as the light).</param>
        /// <param name="lightId">The id of the light. This can be <see cref="UIntPtr.Zero"/>, but you can also give it the pointer of the player ped.</param>
        public static void AddSpotLight(
            Vector3 source,
            Vector3 target,
            Color color,
            float intensity,
            float range,
            float innerConeAngle,
            float outerConeAngle,
            float volIntensity,
            float volSizeScale,
            UIntPtr lightId)
        {
            eLightFlags newFlags = eLightFlags.Vehicle | eLightFlags.DrawVolume | eLightFlags.InteriorOnly | eLightFlags.ExteriorOnly;

            uint id = 0;

            if (lightId != UIntPtr.Zero)
            {
                // Set light id
                id = lightId.ToUInt32();
            }

            // Get interior id at pos
            GET_INTERIOR_AT_COORDS(source, out int interiorId);

            // Calculate direction
            Vector3 dir = Vector3.Normalize(target - source);

            // Tangent
            Vector3 tan = -Vector3.UnitZ;

            // Check if dir is equal to tan
            if (dir == tan)
            {
                // dir cannot be equal to tan (Light would disappear)
                tan = Vector3.Cross(target, Vector3.Abs(target));
                tan = Vector3.Normalize(tan);
            }

            // Add light
            IVShadows.AddSceneLight(
                0,
                (uint)eLightType.Spot,
                (uint)newFlags,
                dir,
                tan,
                source,
                color,
                intensity,
                0,
                0,
                range,
                innerConeAngle,
                outerConeAngle,
                volIntensity,
                volSizeScale,
                interiorId,
                0,
                id);
        }

    }
}
