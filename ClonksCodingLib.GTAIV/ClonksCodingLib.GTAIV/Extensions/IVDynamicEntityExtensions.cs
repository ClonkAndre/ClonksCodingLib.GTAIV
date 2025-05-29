using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using IVSDKDotNet;
using IVSDKDotNet.Enums;

namespace CCL.GTAIV.Extensions
{
    /// <summary>
    /// Contains extensions for the <see cref="IVDynamicEntity"/> class.
    /// </summary>
    public static class IVDynamicEntityExtensions
    {

        #region Variables
        private static SetAnimTimeDelegate setAnimTimeFunc;
        private static SetAnimSpeedDelegate setAnimSpeedFunc;
        #endregion

        #region Delegates
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)] private unsafe delegate void SetAnimTimeDelegate(uint* pThis, float time);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)] private unsafe delegate void SetAnimSpeedDelegate(uint* pThis, float speed);
        #endregion

        #region Static Constructor
        static IVDynamicEntityExtensions()
        {
            switch (MemoryAccess.GameVersion)
            {
                case eGameVersion.VERSION_1070: // TODO: Addresses need to be tested
                    setAnimTimeFunc =   Marshal.GetDelegateForFunctionPointer<SetAnimTimeDelegate>(new IntPtr(MemoryAccess.GetAbsoluteAddress(0x64DB40)));
                    setAnimSpeedFunc =  Marshal.GetDelegateForFunctionPointer<SetAnimSpeedDelegate>(new IntPtr(MemoryAccess.GetAbsoluteAddress(0x64DBC0)));
                    break;
                case eGameVersion.VERSION_1080:
                    setAnimTimeFunc =   Marshal.GetDelegateForFunctionPointer<SetAnimTimeDelegate>(new IntPtr(MemoryAccess.GetAbsoluteAddress(0x633E00)));
                    setAnimSpeedFunc =  Marshal.GetDelegateForFunctionPointer<SetAnimSpeedDelegate>(new IntPtr(MemoryAccess.GetAbsoluteAddress(0x633E80)));
                    break;
            }
        }
        #endregion

        #region Functions
        private unsafe static int GetAnimation(uint* pThis, bool next, int a2, int a3)
        {
            int v3;
            int result;

            if (!next)
            {
                v3 = (int)pThis[1674];
                pThis[1676] = 0;
            }
            else
            {
                v3 = (int)pThis[1676];
            }

            if (v3 == 0)
                return 0;

            while (true)
            {
                bool condition = *(ushort*)(v3 + 72) == 1; // *(WORD*)(v3 + 72)
                result = v3 + 4;
                v3 = *(int*)(v3 + 140); // *(DWORD*)(v3 + 140)

                if (condition)
                {
                    if (*(int*)(result + 64) != 0) // *(DWORD*)(result + 64)
                    {
                        switch (a3)
                        {
                            case 0:
                                if (*(int*)(result + 8) >= a2)
                                    continue;
                                break;
                            case 1:
                                if (*(int*)(result + 8) > a2)
                                    continue;
                                break;
                            case 2:
                                if (*(int*)(result + 8) != a2)
                                    continue;
                                break;
                            case 3:
                                if (*(int*)(result + 8) < a2)
                                    continue;
                                break;
                            case 4:
                                if (*(int*)(result + 8) <= a2)
                                    continue;
                                break;
                            default:
                                continue;
                        }

                        pThis[1676] = (uint)v3;
                        return result;
                    }
                }

                if (v3 == 0)
                    return 0;
            }
        }
        private unsafe static float GetCurrentAnimTime(int pThis, out float normalized)
        {
            float v2 = *(float*)(pThis + 0x4C); // Gets the current animation time like how the "GET_CHAR_ANIM_CURRENT_TIME" native would get it
            normalized = v2;

            if (*(ushort*)(pThis + 0x44) == 1) // WORD comparison
            {
                int pointer = *(int*)(pThis + 0x40); // Dereference address at offset 0x40
                return *(float*)(pointer + 0xC) * v2; // Multiply value at pointer+0xC by current time
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Attempts to retrieve a list of animations associated with the specified dynamic entity.
        /// </summary>
        /// <param name="dynEntity">The dynamic entity from which to retrieve animations. Cannot be null.</param>
        /// <returns>A list of <see cref="AnimDetails"/> objects representing the animations associated with the dynamic entity.
        /// Returns <see langword="null"/> if <paramref name="dynEntity"/> happens to be <see langword="null"/>.</returns>
        public static unsafe List<AnimDetails> TryGetAnimations(this IVDynamicEntity dynEntity)
        {
            if (dynEntity == null)
                return null;

            List<AnimDetails> list = new List<AnimDetails>();

            // Try get the first animation
            int sourceAnim = GetAnimation((uint*)dynEntity.Anim.ToPointer(), false, 0, 3);

            while ((IntPtr)sourceAnim != IntPtr.Zero)
            {
                // Get animation details
                uint animGroupId = *(uint*)((byte*)sourceAnim + 20);
                uint animId = *(uint*)((byte*)sourceAnim + 24);
                uint flags = *(uint*)((byte*)sourceAnim + 4) | 0x2000000;
                uint blendDelta = *(uint*)((byte*)sourceAnim + 8);

                uint off12 = *(uint*)((byte*)sourceAnim + 12);
                uint off16 = *(uint*)((byte*)sourceAnim + 16);
                float off88 = *(float*)((byte*)sourceAnim + 88);

                float currentAnimTime = GetCurrentAnimTime(sourceAnim, out float currentAnimTimeNormalized);

                // Add anim to list
                list.Add(new AnimDetails(animGroupId, animId, flags, blendDelta, off12, off16, off88, currentAnimTime, currentAnimTimeNormalized));

                // Try get the next animation
                sourceAnim = GetAnimation((uint*)dynEntity.Anim.ToPointer(), true, 0, 3);
            }

            return list;
        }

        /// <summary>
        /// Plays an animation on the specified dynamic entity using the provided animation details.
        /// </summary>
        /// <param name="dynEntity">The dynamic entity on which the animation will be played. Cannot be null.</param>
        /// <param name="details">The animation details, including the animation group, animation ID, flags, blend delta,  current animation
        /// time, and other parameters required to configure the animation.</param>
        /// <param name="animSpeed">The speed at which the animation should play. Defaults to -8.0f if not specified.</param>
        public static unsafe void PlayAnimationFromAnimDetails(this IVDynamicEntity dynEntity, AnimDetails details, float animSpeed = -8.0f)
        {
            if (dynEntity == null)
                return;

            IntPtr anim = dynEntity.PlayAnimation((int)details.AnimGroupId, (int)details.AnimId, (int)details.Flags, (int)details.BlendDelta, 1000.0F);

            // TODO: Animation seems a bit choppy. Look into it.
            if (anim != IntPtr.Zero)
            {
                uint* animPtr = (uint*)anim.ToPointer();

                setAnimTimeFunc(animPtr, details.CurrentAnimTime);

                float f = details.Offset88;
                int raw_bits = *(int*)&f;
                animPtr[22] = (uint)raw_bits;

                setAnimSpeedFunc(animPtr, animSpeed);

                animPtr[3] = details.Offset12;
                animPtr[4] = details.Offset16;
            }
        }
        #endregion

    }
}



// IVDynamicEntity.h
//int sub_A7B680(IntPtr ptr, float animTime)
//{
//	return ((int (__thiscall*)(uint32_t*, float))(AddressSetter::Get(0x633E00)))((uint32_t*)ptr.ToPointer(), animTime);
//}
//void sub_A79FB0(IntPtr ptr, float a2)
//{
//	int raw_bits = *(int*)&a2;
//	((uint32_t*)ptr.ToPointer())[22] = raw_bits;
//}
//void sub_A7B700(IntPtr ptr, float speed)
//{
//	((void(__thiscall*)(uint32_t*, float))(AddressSetter::Get(0x633E80)))((uint32_t*)ptr.ToPointer(), speed);
//}
//void idk(IntPtr ptr, uint32_t a1, uint32_t a2)
//{
//	((uint32_t*)ptr.ToPointer())[3] = a1;
//	((uint32_t*)ptr.ToPointer())[4] = a2;
//}

// IVPed.h
//public value struct AnimDetails
//{
//public:
//	uint32_t AnimGroupId;
//	uint32_t AnimId;
//	uint32_t Flags;
//	uint32_t BlendDelta;
//	uint32_t Off12;
//	uint32_t Off16;
//	float Off88;
//	float CurrentAnimTime;
//	float CurrentAnimTimeNormalized;

//public:
//	AnimDetails(uint32_t animGroupId, uint32_t animId, uint32_t flags, uint32_t blendDelta, uint32_t off12, uint32_t off16, float off88, float currentAnimTime, float currentAnimTimeNormalized)
//	{
//		AnimGroupId = animGroupId;
//		AnimId = animId;
//		Flags = flags;
//		BlendDelta = blendDelta;
//		Off12 = off12;
//		Off16 = off16;
//		Off88 = off88;
//		CurrentAnimTime = currentAnimTime;
//		CurrentAnimTimeNormalized = currentAnimTimeNormalized;
//	}
//};
//static int sub_GetAnimation(uint32_t* pThis, bool next, int a2, int a3)
//{
//	int v3; // edx
//	BOOL v4; // zf
//	int result; // eax
//
//	if (!next)
//	{
//		v3 = pThis[1674];
//		pThis[1676] = 0;
//	}
//	else
//	{
//		v3 = pThis[1676];
//	}
//
//	if (!v3)
//		return 0;
//	while (1)
//	{
//		v4 = *(WORD*)(v3 + 72) == 1;
//		result = v3 + 4;
//		v3 = *(DWORD*)(v3 + 140);
//		if (v4)
//		{
//			if (*(DWORD*)(result + 64))
//				break;
//		}
//LABEL_14:
//		if (!v3)
//			return 0;
//	}
//	switch (a3)
//	{
//		case 0:
//			if (*(DWORD*)(result + 8) >= a2)
//				goto LABEL_14;
//			break;
//		case 1:
//			if (*(DWORD*)(result + 8) > a2)
//				goto LABEL_14;
//			break;
//		case 2:
//			if (*(DWORD*)(result + 8) != a2)
//				goto LABEL_14;
//			break;
//		case 3:
//			if (*(DWORD*)(result + 8) < a2)
//				goto LABEL_14;
//			break;
//		case 4:
//			if (*(DWORD*)(result + 8) <= a2)
//				goto LABEL_14;
//			break;
//		default:
//			goto LABEL_14;
//	}
//	pThis[1676] = v3;
//	return result;
//}
//
//static float GetCurrentAnimTime(int pThis, float* normalizedCurrentAnimTime)
//{
//	float v2 = *(float*)(pThis + 0x4C); // Gets the current animation time like how the "GET_CHAR_ANIM_CURRENT_TIME" native would get it
//	*normalizedCurrentAnimTime = v2;
//
//	if (*(WORD*)(pThis + 0x44) == 1)
//		return *(float*)(*(DWORD*)(pThis + 0x40) + 0xC) * v2;
//	else
//		return 0.0F;
//}
//List<AnimDetails>^ TryGetAnimations()
//{
//	List<AnimDetails>^ list = gcnew List<AnimDetails>();

//	int sourceAnim = sub_GetAnimation(NativePed->m_pAnim, false, 0, 3);

//	while (sourceAnim)
//	{
//		// Get the animation details
//		uint32_t animGroupId = *(uint32_t*)((uintptr_t)sourceAnim + 20);
//		uint32_t animId = *(uint32_t*)((uintptr_t)sourceAnim + 24);
//		uint32_t flags = *(uint32_t*)((uintptr_t)sourceAnim + 4) | 0x2000000;
//		uint32_t blendDelta = *(uint32_t*)((uintptr_t)sourceAnim + 8);

//		uint32_t off12 = *(uint32_t*)((uintptr_t)sourceAnim + 12);
//		uint32_t off16 = *(uint32_t*)((uintptr_t)sourceAnim + 16);
//		float off88 = *(float*)((uintptr_t)sourceAnim + 88);

//		float currentAnimTimeNormalized;
//		float currentAnimTime = GetCurrentAnimTime(sourceAnim, &currentAnimTimeNormalized);

//		list->Add(AnimDetails(animGroupId, animId, flags, blendDelta, off12, off16, off88, currentAnimTime, currentAnimTimeNormalized));

//		// Get the next animation
//		sourceAnim = sub_GetAnimation(NativePed->m_pAnim, true, 0, 3);
//	}

//	return list;
//}