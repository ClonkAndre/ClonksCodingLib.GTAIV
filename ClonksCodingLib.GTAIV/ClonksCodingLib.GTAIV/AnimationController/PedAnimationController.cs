using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV.AnimationController
{
    public class PedAnimationController
    {
        #region Variables
        private IVPed ped;
        #endregion

        #region Constructor
        internal PedAnimationController(IVPed targetPed)
        {
            ped = targetPed;
        }
        #endregion

        #region Methods
        public void Play(string animSet, string animName, float speed, int unknown, AnimationFlags flags)
        {
            if (ped == null)
                return;

            ped.GetTaskController().PlayAnimation(animSet, animName, speed, unknown, flags);
        }
        public void Play(string animSet, string animName, float speed, AnimationFlags flags)
        {
            if (ped == null)
                return;

            ped.GetTaskController().PlayAnimation(animSet, animName, speed, flags);
        }
        public void Play(string animSet, string animName, float speed)
        {
            if (ped == null)
                return;
            
            ped.GetTaskController().PlayAnimation(animSet, animName, speed);
        }

        public void SetCurrentAnimationTime(string animSet, string animName, float time)
        {
            if (ped == null)
                return;

            SET_CHAR_ANIM_CURRENT_TIME(ped.GetHandle(), animSet, animName, time);
        }
        #endregion

        #region Functions
        public bool IsPlaying(string animSet, string animName)
        {
            if (ped == null)
                return false;

            return IS_CHAR_PLAYING_ANIM(ped.GetHandle(), animSet, animName);
        }
        public float GetCurrentAnimationTime(string animSet, string animName)
        {
            if (ped == null)
                return 0f;
            GET_CHAR_ANIM_CURRENT_TIME(ped.GetHandle(), animSet, animName, out float time);
            return time;
        }
        #endregion
    }
}
