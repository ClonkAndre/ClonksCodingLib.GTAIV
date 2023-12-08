#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CCL.GTAIV
{

	/// <summary>
	/// Shoot modes
	/// </summary>
    public enum ShootMode
    {
        AimOnly = 0,
        SingleShot = 1,
		SingleShotKeepAim = 2,
		Burst = 3,
		Continuous = 4
	};

    /// <summary>
    /// Contains ped genders.
    /// </summary>
    public enum PedGender
    {
        /// <summary>
        /// Unknown gender. You will only see this appearing when for example the <see cref="CPed"/> is <see langword="null"/>.
        /// </summary>
        Unkown,
        /// <summary>
        /// Ped is a male.
        /// </summary>
        Male,
        /// <summary>
        /// Ped is a female.
        /// </summary>
        Female
    }

    /// <summary>
    /// Flags that can be used to play animations.
    /// </summary>
    public enum AnimationFlags
    {
        /// <summary>None.</summary>
        None = 0,
        /// <summary>Don't come back to initial position after animation ends, we use this for animations that result in position change for the ped, for example the land and roll animation</summary>
        StayAtNewPosition = 1,
        /// <summary>Unknown.</summary>
        Unknown02 = 2,
        /// <summary>Unknown.</summary>
        Unknown03 = 4,
        /// <summary>Not well tested but this might make the ped return to its original position.</summary>
		ReturnToOriginalPosition = 8,
        /// <summary>This loops the animation.</summary>
		Loop = 16,
        /// <summary>The ped will play the animation, and then freezes in the last frame of the animation.</summary>
		FreezeAtLastFrame = 32,
        /// <summary>Seems that makes the ped reset to standing idle animation after the animation ends.</summary>
		SwitchToIdleAnimAfterEnd = 64,
        /// <summary>Unknown.</summary>
        Unknown08 = 128,
        /// <summary>Plays the animation only in the upper body of the ped. Useful for when playing animation in cars.</summary>
		PlayInUpperBodyOnly = 256,
        /// <summary>Removes the sound of the animation. Some animations have sound while others dont.</summary>
		RemoveSound = 512,
        /// <summary>Unknown.</summary>
        Unknown11 = 1024,
        /// <summary>Unknown.</summary>
        Unknown12 = 2048,
        /// <summary>This makes the ped able to walk while an animation in playing in its upper body.</summary>
        PlayInUpperBodyWithWalk = PlayInUpperBodyOnly | Unknown11
    };

    /// <summary>
    /// Contains all common episodes that are in GTA IV.
    /// </summary>
    public enum Episode
    {
        /// <summary>GTA IV</summary>
        IV = 0,
        /// <summary>The Lost and Damned</summary>
        TLaD,
        /// <summary>The Ballad of Gay Tony</summary>
        TBoGT
    }

    /// <summary>
    /// Contains all common game modes that are in GTA IV and its Episodes.
    /// </summary>
    public enum NetworkGameMode
    {
        None = -1,
        Deathmatch = 0,
        TeamDeathmatch = 1,
        MafiyaWork = 2,
        TeamMafiyaWork = 3,
        TeamCarJackCity = 4,
        CarJackCity = 5,
        Race = 6,
        GTARace = 7,
        PartyMode = 8,
        CopsAndCrooks = 10,
        TurfWar = 12,
        DealBreaker = 13,
        HangmansNoose = 14,
        BombDaBaseII = 15,
        FreeMode = 16,
        TBoGT_Deathmatch = 26,
        TBoGT_TeamDeathmatch = 27,
        TBoGT_Race = 28,
        TBoGT_GTARace = 29,
        TLaD_Deathmatch = 24,
        TLaD_TeamDeathmatch = 21,
        TLaD_Race = 20,
        TLaD_ClubBusiness = 19,
        TLaD_LoneWolfBiker = 23,
        TLaD_ChopperVsChopper = 17,
        TLaD_WitnessProtection = 18,
        TLaD_OwnTheCity = 22
    }

    /// <summary>
    /// Contains ground types. Mainly used for the <see cref="NativeWorld.GetGroundZ(System.Numerics.Vector3, GroundType)"/> functions.
    /// </summary>
    public enum GroundType
    {
        Highest,
        Lowest,
        NextBelowCurrent,
        NextAboveCurrent,
        Closest
    }

    /// <summary>
    /// Controller buttons.
    /// </summary>
    public enum ControllerButton
    {
        NONE = 0,
        BUTTON_BACK = 13,
        BUTTON_START = 12,
        BUTTON_X = 14,
        BUTTON_Y = 15,
        BUTTON_A = 16,
        BUTTON_B = 17,
        BUTTON_DPAD_UP = 8,
        BUTTON_DPAD_DOWN = 9,
        BUTTON_DPAD_LEFT = 10,
        BUTTON_DPAD_RIGHT = 11,
        BUTTON_TRIGGER_LEFT = 5,
        BUTTON_TRIGGER_RIGHT = 7,
        BUTTON_BUMPER_LEFT = 4,
        BUTTON_BUMPER_RIGHT = 6,
        BUTTON_STICK_LEFT = 18,
        BUTTON_STICK_RIGHT = 19
    }

    /// <summary>
    /// Game keys.
    /// </summary>
    public enum GameKey
    {
        Sprint = 1,
		Jump = 2,
		EnterCar = 3,
		Attack = 4,
		LookBehind  = 7,
		NextWeapon = 8,
		LastWeapon = 9,

		Crouch = 20,
		Phone = 21,
		Action = 23,
		SeekCover = 28,
		Reload = 29,

		SoundHorn = 54,

		Esc = 61,

		NavDown = 64,
		NavUp = 65,
		NavLeft = 66,
		NavRight = 67,
		NavLeave = 76,
		NavEnter = 77,
		NavBack = 78,

		RadarZoom = 86,
		Aim = 87,

		MoveForward = 1090,
		MoveBackward = 1091,
		MoveLeft = 1092,
		MoveRight = 1093
	};

    /// <summary>
    /// Contains blip icons. Might miss episode icons.
    /// </summary>
    public enum BlipIcon
    {
        Misc_Destination,
		Misc_Destination1,
		Misc_Destination2,
		Misc_Objective,
		Misc_Objective4,
		Misc_Objective5,
		Misc_Player,
		Misc_North,
		Misc_Waypoint,
		Weapon_Pistol,
		Weapon_Shotgun,
		Weapon_SMG,
		Weapon_Rifle,
		Weapon_Rocket,
		Weapon_Grenade,
		Weapon_Molotov,
		Weapon_Sniper,
		Weapon_BaseballBat,
		Weapon_Knife,
		Pickup_Health,
		Pickup_Armor,
		Building_BurgerShot,
		Building_CluckinBell,
		Person_Vlad,
		Building_Internet,
		Person_Manny,
		Person_LittleJacob,
		Person_Roman,
		Person_Faustin,
		Building_Safehouse,
		Misc_TaxiRank,
		Person_Bernie,
		Person_Brucie,
		Person_Unknown,
		Person_Dwayne,
		Person_Elizabeta,
		Person_Gambetti,
		Person_JimmyPegorino,
		Person_Derrick,
		Person_Francis,
		Person_Gerry,
		Person_Katie,
		Person_Packie,
		Person_PhilBell,
		Person_PlayboyX,
		Person_RayBoccino,
		Misc_8BALL,
		Activity_Bar,
		Activity_BoatTour,
		Activity_Bowling,
		Building_ClothShop,
		Activity_Club,
		Activity_Darts,
		Person_Dwayne_Red,
		Activity_Date,
		Person_PlayboyX_Red,
		Activity_HeliTour,
		Activity_Restaurant,
		Building_TrainStation,
		Building_WeaponShop,
		Building_PoliceStation,
		Building_FireStation,
		Building_Hospital,
		Person_Male,
		Person_Female,
		Misc_FinishLine,
		Activity_StripClub,
		Misc_ConsoleGame,
		Misc_CopCar,
		Person_Dimitri,
		Activity_ComedyClub,
		Activity_CabaretClub,
		Misc_Ransom,
		Misc_CopHeli,
		Person_Michelle,
		Building_PayNSpray,
		Person_Assassin,
		Misc_Revenge,	
		Misc_Deal,
		Building_Garage,
		Person_Lawyer,
		Misc_Trophy,
		Misc_MultiplayerTutorial,
		Building_TrainStation3,
		Building_TrainStation8,
		Building_TrainStationA,
		Building_TrainStationB,
		Building_TrainStationC,
		Building_TrainStationE,
		Building_TrainStationJ,
		Building_TrainStationK,
		Building_CarWash,
		Person_UnitedLibertyPaper,
		Misc_Boss,
		Misc_Base
    };

    /// <summary>
    /// The indicators of a <see cref="IVSDKDotNet.IVVehicle"/>.
    /// </summary>
    public enum VehicleIndicator
    {
        Left = 0,
        Right = 1
    }

    /// <summary>
    /// Camera shake types.
    /// </summary>
    public enum CameraShakeType
    {
        PITCH_UP_DOWN = 0,
        ROLL_LEFT_RIGHT = 1,
        YAW_LEFT_RIGHT = 2,
        TRACK_FORWARD_BACK = 3,
        TRACK_LEFT_RIGHT = 4,
        TRACK_UP_DOWN = 5,
        TRACK_LEFT_RIGHT_2 = 6,
        TRACK_FORWARD_BACK_2 = 7,
        TRACK_UP_DOWN_2 = 8,
        PULSE_IN_OUT = 9
    }

    /// <summary>
    /// Camera shake behaviours.
    /// </summary>
    public enum CameraShakeBehaviour
    {
        CONSTANT_PLUS_FADE_IN_OUT = 1,
        CONSTANT_PLUS_FADE_IN = 2,
        EXPONENTIAL_PLUS_FADE_IN_OUT = 3,
        VERY_SLOW_EXPONENTIAL_PLUS_FADE_IN = 4,
        FAST_EXPONENTIAL_PLUS_FADE_IN_OUT = 5,
        MEDIUM_FAST_EXPONENTIAL_PLUS_FADE_IN_OUT = 6,
        SLOW_EXPONENTIAL_PLUS_FADE_IN = 7,
        MEDIUM_SLOW_EXPONENTIAL_PLUS_FADE_IN = 8
    }

}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member