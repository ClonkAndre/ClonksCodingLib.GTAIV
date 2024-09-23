using System;
using System.Runtime.InteropServices;

namespace CCL.GTAIV.Win32
{
    /// <summary>
    /// Some native Win32 and helper functions.
    /// </summary>
    public class Win32Natives
    {

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);

        /// <summary>
        /// Gets the base address of a module by the given <paramref name="moduleName"/> and subtracts it by the given <paramref name="subtract"/> value.
        /// </summary>
        /// <param name="moduleName">
        /// The name of the module to get the base address of.
        /// <para><b>Example</b>: IVSDKDotNet.asi</para>
        /// </param>
        /// <param name="subtract">
        /// Subtracts the base address by the given value.
        /// <para><b>Example for dll files</b>: 0x10000000</para>
        /// <para><b>Example for exe files</b>: 0x40000000</para>
        /// </param>
        /// <returns>The subtracted base address of the module. Otherwise, <see cref="IntPtr.Zero"/>.</returns>
        public static IntPtr GetModuleHandle(string moduleName, int subtract)
        {
            IntPtr baseAddr = GetModuleHandle(moduleName);

            if (baseAddr == IntPtr.Zero)
                return IntPtr.Zero;

            return baseAddr - subtract;
        }

    }
}
