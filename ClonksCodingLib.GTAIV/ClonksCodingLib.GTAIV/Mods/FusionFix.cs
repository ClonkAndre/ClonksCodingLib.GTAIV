using System;
using System.Runtime.InteropServices;

using CCL.GTAIV.Win32;

namespace CCL.GTAIV.Mods
{
    /// <summary>
    /// Exposes some functions of Fusion Fix.
    /// </summary>
    public static class FusionFix
    {

        #region Delegates
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] internal delegate void VoidDelegate();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] [return: MarshalAs(UnmanagedType.I1)] internal delegate bool BoolDelegate();
        #endregion

        #region Methods
        /// <summary>
        /// Toggles the snow in Fusion Fix.
        /// </summary>
        public static void ToggleSnow()
        {
            if (!TryGetBaseAddress(out IntPtr baseAddr, false))
                return;

            // Get address of function
            IntPtr ptr = Win32Natives.GetProcAddress(baseAddr, "ToggleSnow");

            if (ptr == IntPtr.Zero)
                return;

            VoidDelegate func = Marshal.GetDelegateForFunctionPointer<VoidDelegate>(ptr);

            if (func == null)
                return;

            func.Invoke();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Attempts to retrieve the base address of Fusion Fix if present.
        /// </summary>
        /// <remarks>This method checks for the presence of the module "GTAIV.EFLC.FusionFix.asi" and retrieves its base address if found.
        /// If the module is not found, the method returns <see langword="false"/> and sets <paramref name="baseAddress"/> to <see cref="IntPtr.Zero"/>.</remarks>
        /// <param name="baseAddress">When this method returns, contains the base address of the module if the operation is successful; otherwise, <see cref="IntPtr.Zero"/>.</param>
        /// <param name="subtract">Subtracts the returned base address by 0x10000000.</param>
        /// <returns><see langword="true"/> if the base address was successfully retrieved; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetBaseAddress(out IntPtr baseAddress, bool subtract = true)
        {
            IntPtr b = Win32Natives.GetModuleHandle("GTAIV.EFLC.FusionFix.asi", subtract ? 0x10000000 : 0x0);

            if (b == IntPtr.Zero)
            {
                baseAddress = IntPtr.Zero;
                return false;
            }

            baseAddress = b;
            return true;
        }

        /// <summary>
        /// Gets if Fusion Fix is present.
        /// </summary>
        /// <returns><see langword="true"/> if it is. Otherwise, <see langword="false"/>.</returns>
        public static bool IsPresent()
        {
            return Win32Natives.GetModuleHandle("GTAIV.EFLC.FusionFix.asi") != IntPtr.Zero;
        }

        /// <summary>
        /// Gets if snow is enabled in Fusion Fix.
        /// </summary>
        /// <returns><see langword="true"/> if snow is enabled. Otherwise, <see langword="false"/>.</returns>
        public static bool IsSnowEnabled()
        {
            if (!TryGetBaseAddress(out IntPtr baseAddr, false))
                return false;

            // Get address of function
            IntPtr ptr = Win32Natives.GetProcAddress(baseAddr, "IsSnowEnabled");

            if (ptr == IntPtr.Zero)
                return false;

            BoolDelegate func = Marshal.GetDelegateForFunctionPointer<BoolDelegate>(ptr);

            if (func == null)
                return false;

            return func.Invoke();
        }
        /// <summary>
        /// Gets if the current weather can include snow.
        /// </summary>
        /// <returns><see langword="true"/> if the current weather can include snow. Otherwise, <see langword="false"/> e.g. when current weather is lightning.</returns>
        public static bool IsWeatherSnow()
        {
            if (!TryGetBaseAddress(out IntPtr baseAddr, false))
                return false;

            // Get address of function
            IntPtr ptr = Win32Natives.GetProcAddress(baseAddr, "IsWeatherSnow");

            if (ptr == IntPtr.Zero)
                return false;

            BoolDelegate func = Marshal.GetDelegateForFunctionPointer<BoolDelegate>(ptr);

            if (func == null)
                return false;

            return func.Invoke();
        }
        #endregion

    }
}
