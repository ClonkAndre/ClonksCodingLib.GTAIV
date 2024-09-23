using System;
using System.Runtime.InteropServices;

using CCL.GTAIV.Win32;

using IVSDKDotNet.Hooking;

namespace CCL.GTAIV.Mods
{
    // TODO: Not complete yet. Needs some more work with functional memory patterns etc to support as many versions as possible.
    // This is basically just a memory address/pattern playground of mine.

    /// <summary>
    /// Exposes some functions of ZMenuIV.
    /// <para>
    /// Supported versions<br/>
    /// - 23.02.02.2
    /// </para>
    /// </summary>
    internal class ZMenuIV
    {

        #region Enums
        private enum Version
        {
            v2302022 = 2302022,
        }
        #endregion

        #region Delegates
        internal delegate void MethodDelegate();
        internal delegate void GetZMenuVersionDelegate(ref uint major, ref uint minor);
        #endregion

        #region Functions
        /// <summary>
        /// Gets if ZMenuIV is present.
        /// </summary>
        /// <returns><see langword="true"/> if it is. Otherwise, <see langword="false"/>.</returns>
        public static bool IsZMenuPresent()
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
        public static bool GetZMenuVersion(out uint puiMajorVersion, out uint puiMinorVersion)
        {
            // Get base address of "ZMenuIV.asi"
            IntPtr baseAddr = Win32Natives.GetModuleHandle("ZMenuIV.asi");

            if (baseAddr == IntPtr.Zero)
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

            uint major = 0;
            uint minor = 0;

            func.Invoke(ref major, ref minor);

            puiMajorVersion = major;
            puiMinorVersion = minor;

            return true;
        }
        #endregion

        #region First Person View
        /// <summary>
        /// Gets or sets if the first person option is enabled.
        /// <para>
        /// <b>Note</b>: When you try to set this to <see langword="true"/>, without first manually enabling first person view in the menu,
        /// first person view will not actually get activated.
        /// </para>
        /// </summary>
        public unsafe static bool FirstPersonViewEnabled
        {
            get
            {
                IntPtr baseAddr = Win32Natives.GetModuleHandle("ZMenuIV.asi", 0x10000000);

                if (baseAddr == IntPtr.Zero)
                    return false;

                try
                {
                    return *(bool*)IntPtr.Add(baseAddr, 0x108AB89B);
                }
                catch (AccessViolationException)
                {
                    return false;
                }
            }
            set
            {
                IntPtr baseAddr = Win32Natives.GetModuleHandle("ZMenuIV.asi", 0x10000000);

                if (baseAddr == IntPtr.Zero)
                    return;

                try
                {
                    *(bool*)IntPtr.Add(baseAddr, 0x108AB89B) = value;
                }
                catch (AccessViolationException)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Toggles the first person view on or off.
        /// </summary>
        public static void ToggleFirstPersonView()
        {
            // Get base address of "ZMenuIV.asi"
            IntPtr baseAddr = Win32Natives.GetModuleHandle("ZMenuIV.asi");

            if (baseAddr == IntPtr.Zero)
                return;

            // Get the address to the function
            IntPtr ptr = new IntPtr(Patterns.GetAddress(baseAddr, "55 8B EC 83 EC 18 6A 01", 0, 0).ToUInt32());

            if (ptr == IntPtr.Zero)
                return;

            MethodDelegate func = Marshal.GetDelegateForFunctionPointer<MethodDelegate>(ptr);
            func.Invoke();
        }
        #endregion

        #region GTA V Radar
        /// <summary>
        /// Gets or sets if the GTA V Radar is enabled.
        /// <para>
        /// <b>Note</b>: When you try to set this to <see langword="true"/>, the radar will not get updated correctly.
        /// </para>
        /// </summary>
        public unsafe static int GTAVRadarEnabled
        {
            get
            {
                //IntPtr baseAddr = Win32Natives.GetModuleHandle("ZMenuIV.asi", 0x10000000);

                //if (baseAddr == IntPtr.Zero)
                //    return false;

                // Get base address of "ZMenuIV.asi"
                IntPtr baseAddr = Win32Natives.GetModuleHandle("ZMenuIV.asi");

                if (baseAddr == IntPtr.Zero)
                    return 0;

                // Get the address to the function
                IntPtr ptr = new IntPtr(Patterns.GetAddress(baseAddr, "0f b6 15 ? ? ? ? 85 d2 0f 84 ? ? ? ? c7 45 ? ? ? ? ? eb ? 8b 45 ? 83 c0 ? 89 45 ? 83 7d ? ? 7d", 0, 0).ToUInt32());

                if (ptr == IntPtr.Zero)
                    return 0;

                try
                {
                    return Marshal.ReadInt32(ptr);
                    //return *(bool*)ptr/*IntPtr.Add(baseAddr, 0x107CB1AD)*/;
                }
                catch (AccessViolationException)
                {
                    return 0;
                }
            }
            set
            {
                IntPtr baseAddr = Win32Natives.GetModuleHandle("ZMenuIV.asi", 0x10000000);

                if (baseAddr == IntPtr.Zero)
                    return;

                try
                {
                    //*(bool*)IntPtr.Add(baseAddr, 0x107CB1AD) = value;
                }
                catch (AccessViolationException)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected GTA V Hud Style.
        /// <para>
        /// <b>Note</b>: When you try to set this to another style, this new style might not be applied correctly.
        /// </para>
        /// </summary>
        public unsafe static byte SelectedGTAVHudStyle
        {
            get
            {
                IntPtr baseAddr = Win32Natives.GetModuleHandle("ZMenuIV.asi", 0x10000000);

                if (baseAddr == IntPtr.Zero)
                    return 0;

                try
                {
                    return *(byte*)IntPtr.Add(baseAddr, 0x107CB1AE);
                }
                catch (AccessViolationException)
                {
                    return 0;
                }
            }
            set
            {
                IntPtr baseAddr = Win32Natives.GetModuleHandle("ZMenuIV.asi", 0x10000000);

                if (baseAddr == IntPtr.Zero)
                    return;

                try
                {
                    *(byte*)IntPtr.Add(baseAddr, 0x107CB1AE) = value;
                }
                catch (AccessViolationException)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Toggles the GTA V Radar on or off.
        /// </summary>
        public static void ToggleGTAVRadar()
        {
            // Get base address of "ZMenuIV.asi"
            IntPtr baseAddr = Win32Natives.GetModuleHandle("ZMenuIV.asi");

            if (baseAddr == IntPtr.Zero)
                return;

            // Get the address to the function
            IntPtr ptr = new IntPtr(Patterns.GetAddress(baseAddr, "55 8B EC 0F B6 05 ? ? ? ? 85 C0 74 05 E8 ? ? ? ? E8", 0, 0).ToUInt32());

            if (ptr == IntPtr.Zero)
                return;

            MethodDelegate func = Marshal.GetDelegateForFunctionPointer<MethodDelegate>(ptr);
            func.Invoke();
        }

        /// <summary>
        /// Toggles the next GTA V Radar Style.
        /// <para><b>Warning</b>: Should be called from within the <see cref="IVSDKDotNet.Script.Tick"/> event.</para>
        /// </summary>
        public static void ToggleNextGTAVRadarStyle()
        {
            // Get base address of "ZMenuIV.asi"
            IntPtr baseAddr = Win32Natives.GetModuleHandle("ZMenuIV.asi");

            if (baseAddr == IntPtr.Zero)
                return;

            // Get the address to the function
            IntPtr ptr = new IntPtr(Patterns.GetAddress(baseAddr, "55 8B EC 83 EC 10 A0", 0, 0).ToUInt32());

            if (ptr == IntPtr.Zero)
                return;

            MethodDelegate func = Marshal.GetDelegateForFunctionPointer<MethodDelegate>(ptr);
            func.Invoke();
        }
        #endregion

    }
}
