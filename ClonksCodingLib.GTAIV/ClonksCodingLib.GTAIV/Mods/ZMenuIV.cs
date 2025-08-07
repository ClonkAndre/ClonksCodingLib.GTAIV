using System;
using System.Runtime.InteropServices;

using CCL.GTAIV.Win32;

namespace CCL.GTAIV.Mods
{
    /// <summary>
    /// Exposes some functions of ZMenuIV.
    /// <para>
    /// Supported ZMenuIV versions:<br/>
    /// - 23.09.18.2
    /// </para>
    /// </summary>
    public static unsafe class ZMenuIV
    {

        #region Enums
        /// <summary>
        /// Specifies the radar styles for the GTA V Radar feature.
        /// </summary>
        public enum eVRadarStyle : int
        {
            /// <summary>
            /// Only used when the radar style could not be determined or is not set.
            /// </summary>
            UNKNOWN = -1,
            /// <summary>
            /// Last Gen radar style.
            /// </summary>
            LG = 0,
            /// <summary>
            /// Next Gen radar style.
            /// </summary>
            NG = 1,
            /// <summary>
            /// Beta radar style.
            /// </summary>
            Beta = 2
        }
        #endregion

        #region Delegates
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] internal delegate int IntDelegate();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] internal delegate void GetZMenuVersionDelegate(ref uint major, ref uint minor);
        #endregion

        #region Functions
        /// <summary>
        /// Attempts to retrieve the base address of ZMenuIV if present.
        /// </summary>
        /// <remarks>This method checks for the presence of the module "ZMenuIV.asi" and retrieves its base address if found.
        /// If the module is not found, the method returns <see langword="false"/> and sets <paramref name="baseAddress"/> to <see cref="IntPtr.Zero"/>.</remarks>
        /// <param name="baseAddress">When this method returns, contains the base address of the module if the operation is successful; otherwise, <see cref="IntPtr.Zero"/>.</param>
        /// <param name="subtract">Subtracts the returned base address by 0x10000000.</param>
        /// <returns><see langword="true"/> if the base address was successfully retrieved; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetBaseAddress(out IntPtr baseAddress, bool subtract = true)
        {
            IntPtr b = Win32Natives.GetModuleHandle("ZMenuIV.asi", subtract ? 0x10000000 : 0x0);

            if (b == IntPtr.Zero)
            {
                baseAddress = IntPtr.Zero;
                return false;
            }

            baseAddress = b;
            return true;
        }

        /// <summary>
        /// Gets if ZMenuIV is present.
        /// </summary>
        /// <returns><see langword="true"/> if it is. Otherwise, <see langword="false"/>.</returns>
        public static bool IsPresent()
        {
            return Win32Natives.GetModuleHandle("ZMenuIV.asi") != IntPtr.Zero;
        }

        /// <summary>
        /// Gets the version of the installed ZMenuIV.
        /// <para>Could return something like this: 211017 major, 1 minor = 21.10.17.1</para>
        /// </summary>
        /// <param name="puiMajorVersion">
        /// The major version of ZMenuIV.
        /// <para>e.g.: 211017 = [21.10.17].1</para>
        /// </param>
        /// <param name="puiMinorVersion">
        /// The minor version of ZMenuIV.
        /// <para>e.g.: 1 = 21.10.17.[1]</para>
        /// </param>
        /// <returns><see langword="true"/> if the function was successful at getting the current version. Otherwise, <see langword="false"/>.</returns>
        public static bool GetVersion(out uint puiMajorVersion, out uint puiMinorVersion)
        {
            if (!TryGetBaseAddress(out IntPtr baseAddr, false))
            {
                puiMajorVersion = 0;
                puiMinorVersion = 0;
                return false;
            }

            // Get address of function
            IntPtr ptr = Win32Natives.GetProcAddress(baseAddr, "GetZMenuVersion");

            if (ptr == IntPtr.Zero)
            {
                puiMajorVersion = 0;
                puiMinorVersion = 0;
                return false;
            }

            GetZMenuVersionDelegate func = Marshal.GetDelegateForFunctionPointer<GetZMenuVersionDelegate>(ptr);

            if (func == null)
            {
                puiMajorVersion = 0;
                puiMinorVersion = 0;
                return false;
            }

            uint major = 0;
            uint minor = 0;

            func.Invoke(ref major, ref minor);

            puiMajorVersion = major;
            puiMinorVersion = minor;

            return true;
        }
        #endregion

        #region First Person View

        // - - - Properties - - -
        /// <summary>
        /// Gets or sets if the First Person View is enabled within the "Options -> Camera Options -> Custom Cameras -> First Person View" menu.
        /// </summary>
        public static bool FirstPersonViewEnabled
        {
            get
            {
                if (!TryGetBaseAddress(out IntPtr baseAddress))
                    return false;

                try
                {
                    return *(bool*)IntPtr.Add(baseAddress, 0x1086437F);
                }
                catch (AccessViolationException)
                {
                    return false;
                }
            }
            set
            {
                if (!TryGetBaseAddress(out IntPtr baseAddress))
                    return;

                try
                {
                    *(bool*)IntPtr.Add(baseAddress, 0x1086437F) = value;
                }
                catch (AccessViolationException)
                {
                    return;
                }
            }
        }

        // - - - Functions - - -
        // TODO: Need function addresses
        /// <summary>
        /// Toggles the First Person View.
        /// </summary>
        public static int ToggleFirstPersonView()
        {
            if (!TryGetBaseAddress(out IntPtr baseAddress))
                return -1;

            return (Marshal.GetDelegateForFunctionPointer<IntDelegate>(IntPtr.Add(baseAddress, 0x0))?.Invoke()).GetValueOrDefault(-1);
        }

        #endregion

        #region V Radar

        // - - - Properties - - -
        /// <summary>
        /// Gets or sets if the V Radar is enabled within the "Options -> Fun -> HUDs -> V Radar" menu.
        /// </summary>
        public static bool VRadarEnabled
        {
            get
            {
                if (!TryGetBaseAddress(out IntPtr baseAddress))
                    return false;

                try
                {
                    return *(bool*)IntPtr.Add(baseAddress, 0x10866D31);
                }
                catch (AccessViolationException)
                {
                    return false;
                }
            }
            set
            {
                if (!TryGetBaseAddress(out IntPtr baseAddress))
                    return;

                try
                {
                    *(bool*)IntPtr.Add(baseAddress, 0x10866D31) = value;
                }
                catch (AccessViolationException)
                {
                    return;
                }
            }
        }
        /// <summary>
        /// Gets or sets the selected V Radar Style within the "Options -> Fun -> HUDs -> V Radar" menu.
        /// </summary>
        public static eVRadarStyle VRadarStyle
        {
            get
            {
                if (!TryGetBaseAddress(out IntPtr baseAddress))
                    return eVRadarStyle.UNKNOWN;

                try
                {
                    return (eVRadarStyle)(*(int*)IntPtr.Add(baseAddress, 0x10866D34));
                }
                catch (AccessViolationException)
                {
                    return eVRadarStyle.UNKNOWN;
                }
            }
            set
            {
                if (!TryGetBaseAddress(out IntPtr baseAddress))
                    return;

                try
                {
                    *(int*)IntPtr.Add(baseAddress, 0x10866D34) = (int)value;
                }
                catch (AccessViolationException)
                {
                    return;
                }
            }
        }

        // - - - Functions - - -
        // TODO: Need function addresses
        /// <summary>
        /// Toggles the V Radar.
        /// </summary>
        public static int ToggleVRadar()
        {
            if (!TryGetBaseAddress(out IntPtr baseAddress))
                return -1;

            return (Marshal.GetDelegateForFunctionPointer<IntDelegate>(IntPtr.Add(baseAddress, 0x0))?.Invoke()).GetValueOrDefault(-1);
        }
        /// <summary>
        /// Toggles the next V Radar Style.
        /// </summary>
        public static int ToggleNextVRadarStyle()
        {
            if (!TryGetBaseAddress(out IntPtr baseAddress))
                return -1;

            return (Marshal.GetDelegateForFunctionPointer<IntDelegate>(IntPtr.Add(baseAddress, 0x0))?.Invoke()).GetValueOrDefault(-1);
        }

        #endregion

    }
}
