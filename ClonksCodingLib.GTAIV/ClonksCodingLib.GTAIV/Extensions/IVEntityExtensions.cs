using System;

using IVSDKDotNet;
using IVSDKDotNet.Enums;

namespace CCL.GTAIV.Extensions
{
    /// <summary>
    /// Contains extensions for the <see cref="IVEntity"/> class.
    /// </summary>
    public static class IVEntityExtensions
    {

        #region Consts
        private const uint OBJECT_TYPE_MASK = 0x3C0;
        private const int OBJECT_TYPE_SHIFT = 6;
        #endregion

        #region Functions
        /// <summary>
        /// Gets the entity type of the specified <see cref="IVEntity"/> instance.
        /// </summary>
        /// <param name="ent">The <see cref="IVEntity"/> instance for which to determine the entity type. Cannot be <see langword="null"/>.</param>
        /// <returns>An <see cref="eEntityType"/> value representing the type of the entity. Returns <see cref="eEntityType.ENTITY_TYPE_NOTHING"/> 
        /// if <paramref name="ent"/> is <see langword="null"/>.</returns>
        public unsafe static eEntityType GetEntityType(this IVEntity ent)
        {
            if (ent == null)
                return eEntityType.ENTITY_TYPE_NOTHING;

            // Calculate the memory address of the m_dwFlags2 field.
            uint* flagsAddress = (uint*)(ent.GetUIntPtr().ToUInt32() + 0x28);

            // Dereference the pointer to get the value of m_dwFlags2.
            uint m_dwFlags2 = *flagsAddress;

            // Isolate the object type bits using the bitwise AND operator with the mask.
            uint isolatedObjectBits = m_dwFlags2 & OBJECT_TYPE_MASK;

            // Right-shift the isolated bits to get the actual value.
            uint objectTypeValue = isolatedObjectBits >> OBJECT_TYPE_SHIFT;

            return (eEntityType)objectTypeValue;
        }
        #endregion

    }
}
