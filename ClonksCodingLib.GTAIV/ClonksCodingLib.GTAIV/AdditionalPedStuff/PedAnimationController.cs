using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// A simple animation controller for <see cref="IVPed"/>'s. Uses native functions.
    /// </summary>
    public struct PedAnimationController
    {
        #region Variables and Properties
        // Variables
        private IVPed ped;
        private int handle;

        // Properties
        /// <summary>
        /// Gets if this <see cref="PedAnimationController"/> is valid or not.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return handle != 0;
            }
        }
        #endregion

        #region Constructor
        internal PedAnimationController(IVPed targetPed)
        {
            ped = targetPed;
            handle = ped.GetHandle();
        }
        internal PedAnimationController(int pedHandle)
        {
            ped = null;
            handle = pedHandle;
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
        /// <summary>
        /// Returns an invalid <see cref="PedAnimationController"/> which cannot be used to play any animations on a <see cref="IVPed"/>.
        /// </summary>
        /// <returns>An invalid <see cref="PedAnimationController"/>.</returns>
        public static PedAnimationController Empty()
        {
            return new PedAnimationController(0);
        }

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
