using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    public static class CObjectExtensions
    {
        #region Functions
        /// <summary>
        /// Gets the handle for this <see cref="CObject"/>.
        /// <para>Can be used for calling native functions like <see cref="DOES_OBJECT_EXIST(int)"/> which requires a ped handle.</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>If successful, the handle of this <see cref="CObject"/> is returned. Otherwise, 0.</returns>
        public static int GetHandle(this CObject obj)
        {
            if (obj == null)
                return 0;

            return (int)CPools.GetObjectPool().GetIndex(obj.GetUIntPtr());
        }
        #endregion
    }
}
