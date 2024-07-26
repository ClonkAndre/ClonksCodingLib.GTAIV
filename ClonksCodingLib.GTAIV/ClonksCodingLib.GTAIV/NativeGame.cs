using System;

using IVSDKDotNet;
using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Gives you easy access to native functions that are all about the game.
    /// </summary>
    public class NativeGame
    {

        #region Variables and Properties
        // Variables
        /// <summary>A collection of all GTA IV sco script names. Taken from version 1080 of GTA IV.</summary>
        public static readonly string[] IV_GameScripts;
        /// <summary>A collection of all GTA IV Multiplayer sco script names. Taken from version 1080 of GTA IV.</summary>
        public static readonly string[] IV_MP_GameScripts;
        /// <summary>A collection of all TBoGT sco script names. Taken from version 1080 of GTA IV.</summary>
        public static readonly string[] TBOGT_GameScripts;
        /// <summary>A collection of all TBoGT Multiplayer sco script names. Taken from version 1080 of GTA IV.</summary>
        public static readonly string[] TBOGT_MP_GameScripts;
        /// <summary>A collection of all TLaD sco script names. Taken from version 1080 of GTA IV.</summary>
        public static readonly string[] TLAD_GameScripts;
        /// <summary>A collection of all TLaD Multiplayer sco script names. Taken from version 1080 of GTA IV.</summary>
        public static readonly string[] TLAD_MP_GameScripts;

        /// <summary>A collection of all GTA IV Ped Animation sets and all the animations inside that set.</summary>
        public static readonly IVAnimSet[] IV_Animations;

        // Properties
        /// <summary>
        /// Gets the current episode.
        /// </summary>
        public static Episode CurrentEpisode
        {
            get
            {
                return (Episode)GET_CURRENT_EPISODE();
            }
        }

        /// <summary>
        /// Gets if the game is in multiplayer.
        /// </summary>
        public static bool IsMultiplayer
        {
            get
            {
                return NETWORK_IS_SESSION_STARTED();
            }
        }
        /// <summary>
        /// Gets if the game is in ranked multiplayer.
        /// </summary>
        public static bool IsRankedMultiplayer
        {
            get
            {
                return NETWORK_IS_GAME_RANKED();
            }
        }

        /// <summary>
        /// Gets the current network mode.
        /// </summary>
        public static eGameType NetworkMode
        {
            get
            {
                if (!IsMultiplayer)
                    return eGameType.GAME_TYPE_SINGLEPLAYER;
                if (IsRankedMultiplayer)
                    return eGameType.GAME_TYPE_MULTIPLAYER_LIVE_RANKED;

                return IS_IN_LAN_MODE() ? eGameType.GAME_TYPE_MULTIPLAYER_LAN : eGameType.GAME_TYPE_MULTIPLAYER_LIVE;
            }
        }
        /// <summary>
        /// Gets the current game mode.
        /// </summary>
        public static NetworkGameMode NetworkGameMode
        {
            get
            {
                if (!IsMultiplayer)
                    return NetworkGameMode.None;

                return (NetworkGameMode)NETWORK_GET_GAME_MODE();
            }
        }

        /// <summary>
        /// Gets the number of players.
        /// </summary>
        public static uint PlayerCount
        {
            get
            {
                return GET_NUMBER_OF_PLAYERS();
            }
        }

        /// <summary>
        /// Gets how many players there are currently.
        /// </summary>
        public static uint GameTime
        {
            get
            {
                GET_GAME_TIMER(out uint t);
                return t;
            }
        }
        /// <summary>
        /// Gets the frame time.
        /// </summary>
        public static float FrameTime
        {
            get
            {
                GET_FRAME_TIME(out float t);
                return t;
            }
        }
        /// <summary>
        /// Gets the fps based on the frame time.
        /// </summary>
        public static float FPS
        {
            get
            {
                return 1.0f / FrameTime;
            }
        }

        /// <summary>
        /// Gets or sets the current radio station.
        /// </summary>
        public static eRadioStation RadioStation
        {
            get
            {
                return (eRadioStation)GET_PLAYER_RADIO_STATION_INDEX();
            }
            set
            {
                RETUNE_RADIO_TO_STATION_INDEX((uint)value);
            }
        }

        /// <summary>
        /// Gets or sets if a minigame is currently in progress.
        /// </summary>
        public static bool IsMinigameInProgress
        {
            get
            {
                return IS_MINIGAME_IN_PROGRESS();
            }
            set
            {
                SET_MINIGAME_IN_PROGRESS(value);
            }
        }
        /// <summary>
        /// Sets if emergency services are allowed.
        /// </summary>
        public static bool AllowEmergencyServices
        {
            set
            {
                ALLOW_EMERGENCY_SERVICES(value);
            }
        }
        /// <summary>
        /// Sets if the max ammo cap is disabled.
        /// </summary>
        public static bool DisableMaxAmmoLimit
        {
            set
            {
                ENABLE_MAX_AMMO_CAP(!value);
            }
        }
        /// <summary>
        /// Sets if mad drivers should be switched on.
        /// </summary>
        public static bool MadDrivers
        {
            set
            {
                SWITCH_MAD_DRIVERS(value);
            }
        }

        /// <summary>
        /// Sets the zoom of the radar.
        /// </summary>
        public static int RadarZoom
        {
            set
            {
                SET_RADAR_ZOOM(value);
            }
        }
        /// <summary>
        /// Sets the time scale.
        /// </summary>
        public static float TimeScale
        {
            set
            {
                SET_TIME_SCALE(value);
            }
        }
        /// <summary>
        /// Sets the wanted multiplier.
        /// </summary>
        public static float WantedMultiplier
        {
            set
            {
                SET_WANTED_MULTIPLIER(value);
            }
        }
        #endregion

        #region Constructor
        static NativeGame()
        {
            // Populate static arrays
            IV_GameScripts =        Properties.Resources.IV_Scripts.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            IV_MP_GameScripts =     Properties.Resources.IV_MP_Scripts.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            TBOGT_GameScripts =     Properties.Resources.TBOGT_Scripts.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            TBOGT_MP_GameScripts =  Properties.Resources.TBOGT_MP_Scripts.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            TLAD_GameScripts =      Properties.Resources.TLAD_Scripts.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            TLAD_MP_GameScripts =   Properties.Resources.TLAD_MP_Scripts.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            IV_Animations = new IVAnimSet[761];

            #region Populate animations array
            IV_Animations[0] = new IVAnimSet("amb@arcade", new string[] { "play_pinball", "play_videogame", "use_vendmac" });
            IV_Animations[1] = new IVAnimSet("amb@argue", new string[] { "angryman", "argue_a", "argue_b", "crazy_rant_01", "crazy_rant_02", "crazy_rant_03", "street_argue_f_a", "street_argue_f_b" });
            IV_Animations[2] = new IVAnimSet("amb@atm", new string[] { "f_enternumbers", "f_getoutpurse", "f_insertcard", "f_putpurseaway", "f_takecash", "f_wait", "m_enternumbers", "m_getoutwallet_chest", "m_getoutwallet_pocket", "m_insertcard", "m_putwalletaway_chest", "m_putwalletaway_pocket", "m_takecash", "m_wait" });
            IV_Animations[3] = new IVAnimSet("amb@bag_a_create", new string[] { "walk_create" });
            IV_Animations[4] = new IVAnimSet("amb@bag_a_hold", new string[] { "walk_bag" });
            IV_Animations[5] = new IVAnimSet("amb@bag_a_idles", new string[] { "walk_bag" });
            IV_Animations[6] = new IVAnimSet("amb@bag_b_create", new string[] { "walk_create" });
            IV_Animations[7] = new IVAnimSet("amb@bag_b_hold", new string[] { "walk_bag" });
            IV_Animations[8] = new IVAnimSet("amb@bag_b_idles", new string[] { "walk_bag" });
            IV_Animations[9] = new IVAnimSet("amb@bar", new string[] { "clean_glass", "wipe_counter" });
            IV_Animations[10] = new IVAnimSet("amb@baseball", new string[] { "swing", "swing_idle" });
            IV_Animations[11] = new IVAnimSet("amb@beg_sitting", new string[] { "beggar_beg", "beggar_sit", "give_obj" });
            IV_Animations[12] = new IVAnimSet("amb@beg_standing", new string[] { "argue_b", "crazy_rant_01", "give_obj", "take_obj", "walk_hassle_money" });
            IV_Animations[13] = new IVAnimSet("amb@bnch_bum_idl", new string[] { "groan", "scratch" });
            IV_Animations[14] = new IVAnimSet("amb@bnch_dnk_idl", new string[] { "sit_idle_a", "sit_idle_b" });
            IV_Animations[15] = new IVAnimSet("amb@bnch_dnk_idl_f", new string[] { "sit_idle_a" });
            IV_Animations[16] = new IVAnimSet("amb@bnch_eat_idl", new string[] { "sit_idle_a" });
            IV_Animations[17] = new IVAnimSet("amb@bnch_eat_idl_f", new string[] { "sit_idle_a" });
            IV_Animations[18] = new IVAnimSet("amb@bnch_read_idl", new string[] { "turn_page" });
            IV_Animations[19] = new IVAnimSet("amb@bnch_read_idl_f", new string[] { "turn_page" });
            IV_Animations[20] = new IVAnimSet("amb@bnch_smk_idl", new string[] { "sit_idle_a", "sit_idle_b" });
            IV_Animations[21] = new IVAnimSet("amb@bnch_smk_idl_f", new string[] { "sit_idle_a", "sit_idle_b" });
            IV_Animations[22] = new IVAnimSet("amb@bnch_std_idl", new string[] { "sit_idle_a", "sit_idle_b", "sit_idle_c", "sit_idle_d" });
            IV_Animations[23] = new IVAnimSet("amb@bnch_std_idl_f", new string[] { "sit_idle_a", "sit_idle_b", "sit_idle_c", "sit_idle_d" });
            IV_Animations[24] = new IVAnimSet("amb@book", new string[] { "walk_read" });
            IV_Animations[25] = new IVAnimSet("amb@boot_idles", new string[] { "idle_a" });
            IV_Animations[26] = new IVAnimSet("amb@bottle_create", new string[] { "stand_create", "walk_create" });
            IV_Animations[27] = new IVAnimSet("amb@bottle_destroy", new string[] { "destroy_stand", "destroy_walk" });
            IV_Animations[28] = new IVAnimSet("amb@bottle_hold", new string[] { "hold_stand", "hold_walk" });
            IV_Animations[29] = new IVAnimSet("amb@bottle_idle", new string[] { "drink_stand", "drink_walk" });
            IV_Animations[30] = new IVAnimSet("amb@bouncer_idles_a", new string[] { "crack_knuckles", "lookaround_b", "lookaround_c", "lookaround_d", "shake_legs" });
            IV_Animations[31] = new IVAnimSet("amb@bouncer_idles_b", new string[] { "lookaround_a" });
            IV_Animations[32] = new IVAnimSet("amb@brazier", new string[] { "braziera", "brazierb", "brazierc" });
            IV_Animations[33] = new IVAnimSet("amb@bridgecops", new string[] { "car_chat_outside", "car_chat_outside_2", "close_boot", "open_boot" });
            IV_Animations[34] = new IVAnimSet("amb@broken_d_idles_a", new string[] { "idle_a", "idle_b" });
            IV_Animations[35] = new IVAnimSet("amb@broken_d_idles_b", new string[] { "idle_c" });
            IV_Animations[36] = new IVAnimSet("amb@bum_a", new string[] { "stand_rant_a", "stand_rant_b" });
            IV_Animations[37] = new IVAnimSet("amb@bum_b", new string[] { "walkcycle_drunk_a" });
            IV_Animations[38] = new IVAnimSet("amb@bum_c", new string[] { "walkcycle_drunk_b" });
            IV_Animations[39] = new IVAnimSet("amb@burgercart", new string[] { "buy_burger", "buy_burger_plyr", "eat_burger_plyr", "hotdog_buy", "hotdog_vend", "sell_burger", "sell_burger_plyr", "stand_eat_fastfood_2" });
            IV_Animations[40] = new IVAnimSet("amb@burger_create", new string[] { "create_walk" });
            IV_Animations[41] = new IVAnimSet("amb@burger_destroy", new string[] { "destroy_stand", "destroy_walk" });
            IV_Animations[42] = new IVAnimSet("amb@burger_hold", new string[] { "hold_stand", "hold_walk" });
            IV_Animations[43] = new IVAnimSet("amb@burger_idle", new string[] { "eat_stand", "eat_walk" });
            IV_Animations[44] = new IVAnimSet("amb@busker", new string[] { "sax_loop_a", "sax_loop_b", "throw_right" });
            IV_Animations[45] = new IVAnimSet("amb@cafe_eat_idles", new string[] { "sit_eat" });
            IV_Animations[46] = new IVAnimSet("amb@cafe_empty_idl", new string[] { "sit_a", "sit_b" });
            IV_Animations[47] = new IVAnimSet("amb@cafe_empty_idl_f", new string[] { "sit_a", "sit_b" });
            IV_Animations[48] = new IVAnimSet("amb@cafe_idles", new string[] { "sit_drink", "sit_drink_b" });
            IV_Animations[49] = new IVAnimSet("amb@cafe_idles_f", new string[] { "sit_idle_a" });
            IV_Animations[50] = new IVAnimSet("amb@cafe_read_idl", new string[] { "turn_page" });
            IV_Animations[51] = new IVAnimSet("amb@cafe_read_idl_f", new string[] { "turn_page" });
            IV_Animations[52] = new IVAnimSet("amb@cafe_smk_create", new string[] { "smoke_create" });
            IV_Animations[53] = new IVAnimSet("amb@cafe_smk_idl_f", new string[] { "sit_idle_a", "sit_idle_b" });
            IV_Animations[54] = new IVAnimSet("amb@cafe_smoke_idl_a", new string[] { "smoke_idle_a" });
            IV_Animations[55] = new IVAnimSet("amb@cafe_smoke_idl_b", new string[] { "smoke_idle_b", "smoke_idle_c" });
            IV_Animations[56] = new IVAnimSet("amb@carry_create", new string[] { "walk_create" });
            IV_Animations[57] = new IVAnimSet("amb@carry_hold", new string[] { "walk_hold" });
            IV_Animations[58] = new IVAnimSet("amb@carry_idles", new string[] { "walk_idle_a", "walk_idle_b", "walk_idle_c" });
            IV_Animations[59] = new IVAnimSet("amb@cartcommon", new string[] { "give_hotdog", "give_obj", "take_hotdog", "take_obj" });
            IV_Animations[60] = new IVAnimSet("amb@carwash", new string[] { "carwash_c", "give_money", "plyr_licenseintro_ncar", "plyr_licenseoutro_ncar" });
            IV_Animations[61] = new IVAnimSet("amb@car_cell_crte_ds", new string[] { "create_cell" });
            IV_Animations[62] = new IVAnimSet("amb@car_cell_crte_ps", new string[] { "create_cell" });
            IV_Animations[63] = new IVAnimSet("amb@car_cell_dsty_ds", new string[] { "cell_destroy" });
            IV_Animations[64] = new IVAnimSet("amb@car_cell_dsty_ps", new string[] { "cell_destroy" });
            IV_Animations[65] = new IVAnimSet("amb@car_cell_idle_ds", new string[] { "idle01" });
            IV_Animations[66] = new IVAnimSet("amb@car_cell_idle_ps", new string[] { "idle01" });
            IV_Animations[67] = new IVAnimSet("amb@car_low_ps_loops", new string[] { "alt_sit_ps_a", "alt_sit_ps_b" });
            IV_Animations[68] = new IVAnimSet("amb@car_stash", new string[] { "boot_withdraw", "idle", "open_boot" });
            IV_Animations[69] = new IVAnimSet("amb@car_std_bk_seat", new string[] { "bored", "bored_b", "bored_c", "convo_a", "pick_nose", "sing", "sing_b" });
            IV_Animations[70] = new IVAnimSet("amb@car_std_ds_a", new string[] { "change_radio", "dance", "mirror_a" });
            IV_Animations[71] = new IVAnimSet("amb@car_std_ds_b", new string[] { "mirror_b", "mirror_c", "scratch_a", "scratch_b" });
            IV_Animations[72] = new IVAnimSet("amb@car_std_ds_c", new string[] { "scratch_c", "sing_a", "sing_c" });
            IV_Animations[73] = new IVAnimSet("amb@car_std_ds_d", new string[] { "sing_b" });
            IV_Animations[74] = new IVAnimSet("amb@car_std_ds_trash", new string[] { "rubbish_a" });
            IV_Animations[75] = new IVAnimSet("amb@car_std_f_id_ds", new string[] { "flee_idle_a", "flee_idle_b" });
            IV_Animations[76] = new IVAnimSet("amb@car_std_f_id_ps", new string[] { "flee_a", "flee_b" });
            IV_Animations[77] = new IVAnimSet("amb@car_std_ps_b", new string[] { "conversation", "scratch_a" });
            IV_Animations[78] = new IVAnimSet("amb@car_std_ps_c", new string[] { "bored", "lookaround_d", "pick_nose" });
            IV_Animations[79] = new IVAnimSet("amb@car_std_ps_d", new string[] { "sing_b", "stretch_a" });
            IV_Animations[80] = new IVAnimSet("amb@car_std_ps_e", new string[] { "sing" });
            IV_Animations[81] = new IVAnimSet("amb@car_std_ps_loops", new string[] { "alt_sit_ps_a", "alt_sit_ps_b" });
            IV_Animations[82] = new IVAnimSet("amb@car_std_ps_trash", new string[] { "rubbish_a" });
            IV_Animations[83] = new IVAnimSet("amb@club_", new string[] { "clap", "point" });
            IV_Animations[84] = new IVAnimSet("amb@coffee_hold", new string[] { "hold_coffee" });
            IV_Animations[85] = new IVAnimSet("amb@coffee_idle_f", new string[] { "drink_a" });
            IV_Animations[86] = new IVAnimSet("amb@coffee_idle_m", new string[] { "drink_a" });
            IV_Animations[87] = new IVAnimSet("amb@cold", new string[] { "pull_up_collar", "stand_blowhands", "stand_lookatsky", "stand_rubarms", "walk_blowhands", "walk_rubarms" });
            IV_Animations[88] = new IVAnimSet("amb@comedy", new string[] { "agree_a", "bored", "cheer_a", "cheer_b", "clap_a", "clap_b", "clap_c", "idle01", "jeer", "jeer_a", "jeer_b", "jeer_c", "laugh_a", "laugh_b", "laugh_c", "laugh_d", "laugh_e", "laugh_f" });
            IV_Animations[89] = new IVAnimSet("amb@dance_femidl_a", new string[] { "loop_a" });
            IV_Animations[90] = new IVAnimSet("amb@dance_femidl_b", new string[] { "loop_b" });
            IV_Animations[91] = new IVAnimSet("amb@dance_femidl_c", new string[] { "loop_c" });
            IV_Animations[92] = new IVAnimSet("amb@dance_maleidl_a", new string[] { "loop_a" });
            IV_Animations[93] = new IVAnimSet("amb@dance_maleidl_b", new string[] { "loop_b" });
            IV_Animations[94] = new IVAnimSet("amb@dance_maleidl_c", new string[] { "loop_c" });
            IV_Animations[95] = new IVAnimSet("amb@dance_maleidl_d", new string[] { "loop_d" });
            IV_Animations[96] = new IVAnimSet("amb@dating", new string[] { "car_kiss_ds", "car_kiss_ps", "flinch", "girl_hug", "niko_incar_partial", "partial_smoke", "pass_text", "player_kiss", "reach_open_door" });
            IV_Animations[97] = new IVAnimSet("amb@default", new string[] { "bnch_bum_a_default", "bnch_bum_b_default", "bnch_dnk_default", "bnch_dnk_f_default", "bnch_eat_default", "bnch_eat_f_default", "bnch_read_default", "bnch_read_f_default", "bnch_smk_default", "bnch_smk_f_default", "bnch_std_default", "bnch_std_f_default", "boot_default", "bouncer_default", "broken_d_default", "busker_default", "cafe_default", "cafe_eat_default", "cafe_empty_default", "cafe_f_default", "cafe_f_empty_default", "cafe_read_default", "cafe_read_f_default", "cafe_smk_default", "cafe_smk_f_default", "dance_f_default", "dance_m_default", "drill_default", "driver_flee_low", "driver_flee_std", "driver_flee_truck", "driver_flee_van", "drugd_default", "ff_sweep_default", "ff_sweep_default_f", "guard_fat_default", "hang_str_default", "hang_str_fat_default", "hang_str_f_default", "hang_str_thin_default", "hooker_def", "hooker_fat_def", "hospital_lie_a_def", "hospital_lie_b_def", "hospital_sit_def", "int_cafe_default", "lean_balc_smoke_def", "lean_balc_stand_def", "lean_default", "lean_default_b", "pass_flee_low", "pass_flee_rear_std", "pass_flee_std", "pass_flee_truck", "pass_flee_van", "payphone_default", "postman_default", "preacher_default", "rake_default", "rake_f_default", "service_default", "sledge_default", "smoke_f_def", "smoke_m_def", "spade_default", "standing_f_default", "standing_m_default", "step_default", "step_default_b", "stop_default", "strip_chr_default", "strip_rail_default", "strip_stool_default", "super_default", "taichi_default", "taxi_f_default", "taxi_m_default", "telegraph_default", "telescope_default", "telescope_f_default", "wall_default", "wall_f_default", "wall_read_default", "wall_read_f_default", "wasted_a_default", "wasted_b_default", "wcan_default", "winclean_default", "winshop_default" });
            IV_Animations[98] = new IVAnimSet("amb@drill_create", new string[] { "drill_create" });
            IV_Animations[99] = new IVAnimSet("amb@drill_idles", new string[] { "drill_stand", "drill_stand_b" });
            IV_Animations[100] = new IVAnimSet("amb@drink_bottle", new string[] { "bottle_stand", "bottle_walk" });
            IV_Animations[101] = new IVAnimSet("amb@drink_can", new string[] { "can_stand", "can_walk" });
            IV_Animations[102] = new IVAnimSet("amb@drink_fbottle", new string[] { "bottle_stand", "bottle_walk" });
            IV_Animations[103] = new IVAnimSet("amb@drink_fcan", new string[] { "can_stand", "can_walk" });
            IV_Animations[104] = new IVAnimSet("amb@drink_f_fat", new string[] { "drink_bottle", "drink_bottle_walk", "drink_can", "drink_can_walk" });
            IV_Animations[105] = new IVAnimSet("amb@drugd_idl_a", new string[] { "idle_a", "idle_b" });
            IV_Animations[106] = new IVAnimSet("amb@drugd_idl_b", new string[] { "idle_c" });
            IV_Animations[107] = new IVAnimSet("amb@drugd_sell", new string[] { "buy_drugs", "sell_drugs" });
            IV_Animations[108] = new IVAnimSet("amb@drunk", new string[] { "beggar_sitdown", "bum_fight", "dazed_getup_back", "lay_down", "wasteda", "wastedb", "wastedc", "wastedd", "wasted_seated" });
            IV_Animations[109] = new IVAnimSet("amb@eat_chocolate", new string[] { "choc_stand", "choc_walk" });
            IV_Animations[110] = new IVAnimSet("amb@eat_fruit", new string[] { "eat_stand", "eat_walk" });
            IV_Animations[111] = new IVAnimSet("amb@ffood_server", new string[] { "buy_burger_plyr", "eat_burger_plyr", "sell_burger_female", "sell_burger_male" });
            IV_Animations[112] = new IVAnimSet("amb@flee", new string[] { "stand_into_ball", "stand_into_ball_b" });
            IV_Animations[113] = new IVAnimSet("amb@flee_a", new string[] { "run_flee_lookaround_b", "run_flee_lookaround_c", "run_tripup" });
            IV_Animations[114] = new IVAnimSet("amb@garbage", new string[] { "hangontruck", "pickuprubbish", "throwrubbish" });
            IV_Animations[115] = new IVAnimSet("amb@hang_fat_cl_a", new string[] { "stand_idle_a" });
            IV_Animations[116] = new IVAnimSet("amb@hang_fat_cl_b", new string[] { "stand_idle_e" });
            IV_Animations[117] = new IVAnimSet("amb@hang_fat_idls", new string[] { "stand_idle_d", "stand_idle_g" });
            IV_Animations[118] = new IVAnimSet("amb@hang_str_f_idls", new string[] { "stand_idle_a", "stand_idle_a1", "stand_idle_a2", "stand_idle_a3", "stand_idle_a4", "stand_idle_d", "stand_idle_k", "stand_idle_k1", "stand_idle_k2", "stand_idle_k3" });
            IV_Animations[119] = new IVAnimSet("amb@hang_str_idls", new string[] { "stand_idle_a", "stand_idle_d", "stand_idle_k" });
            IV_Animations[120] = new IVAnimSet("amb@hang_str_i_cl_a", new string[] { "stand_idle_b", "stand_idle_b1", "stand_idle_b2", "stand_idle_b3" });
            IV_Animations[121] = new IVAnimSet("amb@hang_str_i_cl_c", new string[] { "stand_idle_g", "stand_idle_h" });
            IV_Animations[122] = new IVAnimSet("amb@hang_st_i_f_cl_a", new string[] { "stand_idle_b" });
            IV_Animations[123] = new IVAnimSet("amb@hang_st_i_f_cl_c", new string[] { "stand_idle_g", "stand_idle_h", "stand_idle_j" });
            IV_Animations[124] = new IVAnimSet("amb@hang_thin_cl", new string[] { "idle_a", "idle_b" });
            IV_Animations[125] = new IVAnimSet("amb@hang_thin_idls", new string[] { "idle_a", "idle_b", "idle_c", "idle_d", "idle_d1", "idle_d2", "idle_d3" });
            IV_Animations[126] = new IVAnimSet("amb@homeless_a", new string[] { "stand_blowhands", "walkcycle_ground", "walk_lookaround" });
            IV_Animations[127] = new IVAnimSet("amb@homeless_b", new string[] { "stand_look_ground", "stand_stretchneck" });
            IV_Animations[128] = new IVAnimSet("amb@hooker", new string[] { "idle_a", "idle_b", "idle_c" });
            IV_Animations[129] = new IVAnimSet("amb@hooker_fat", new string[] { "idle_a", "idle_b", "idle_c" });
            IV_Animations[130] = new IVAnimSet("amb@hospital_idles_a", new string[] { "cough", "hold_head" });
            IV_Animations[131] = new IVAnimSet("amb@hospital_idles_b", new string[] { "sneeze", "stomach" });
            IV_Animations[132] = new IVAnimSet("amb@hot", new string[] { "fan_stand", "fan_walk", "sun_shield", "sweaty_stand", "sweaty_walk" });
            IV_Animations[133] = new IVAnimSet("amb@hotdogcart", new string[] { "buy_hotdog", "buy_hotdog_plyr", "eat_hotdog", "eat_hotdog_plyr", "sell_hotdog", "sell_hotdog_plyr", "stvendor_custmr_pay", "stvendor_pay" });
            IV_Animations[134] = new IVAnimSet("amb@hotdog_destroy", new string[] { "destroy_stand", "destroy_walk" });
            IV_Animations[135] = new IVAnimSet("amb@hotdog_destroy_f", new string[] { "destroy_stand", "destroy_walk" });
            IV_Animations[136] = new IVAnimSet("amb@hotdog_hold", new string[] { "hold_stand", "hold_stand_f", "hold_walk", "hold_walk_f" });
            IV_Animations[137] = new IVAnimSet("amb@hotdog_idle", new string[] { "eat_stand", "eat_walk" });
            IV_Animations[138] = new IVAnimSet("amb@hotdog_idle_f", new string[] { "eat_stand", "eat_walk" });
            IV_Animations[139] = new IVAnimSet("amb@hurry_head", new string[] { "walk_a", "walk_b" });
            IV_Animations[140] = new IVAnimSet("amb@hurry_walk", new string[] { "walk_a", "walk_b", "walk_c" });
            IV_Animations[141] = new IVAnimSet("amb@icecream_default", new string[] { "stand_hold" });
            IV_Animations[142] = new IVAnimSet("amb@icecream_destroy", new string[] { "walk_destroy" });
            IV_Animations[143] = new IVAnimSet("amb@icecream_idles", new string[] { "stand_eat", "walk_eat" });
            IV_Animations[144] = new IVAnimSet("amb@ice_vendor", new string[] { "give_obj", "idle_stvendor", "stand_eat_fastfood_2", "stvendor_custmr_pay", "stvendor_pay", "take_obj", "walk_destroy", "walk_eat" });
            IV_Animations[145] = new IVAnimSet("amb@injured_front", new string[] { "idle01" });
            IV_Animations[146] = new IVAnimSet("amb@injured_side", new string[] { "idle01" });
            IV_Animations[147] = new IVAnimSet("amb@inquisitive", new string[] { "shock_a", "shock_b", "shock_c", "shock_d", "shock_e" });
            IV_Animations[148] = new IVAnimSet("amb@int_cafe_idles", new string[] { "sit_idle_a", "sit_idle_b", "sit_idle_c" });
            IV_Animations[149] = new IVAnimSet("amb@kiosk", new string[] { "customer_cigs", "customer_juice", "customer_paper", "player_drink", "vendor_cigs", "vendor_juice", "vendor_paper" });
            IV_Animations[150] = new IVAnimSet("amb@lean_balc_cre", new string[] { "smoke_create" });
            IV_Animations[151] = new IVAnimSet("amb@lean_balc_idl_b", new string[] { "smoke", "smoke_b" });
            IV_Animations[152] = new IVAnimSet("amb@lean_balc_i_a", new string[] { "crack_knuckles", "lookaround_a" });
            IV_Animations[153] = new IVAnimSet("amb@lean_balc_i_b", new string[] { "lookaround_b" });
            IV_Animations[154] = new IVAnimSet("amb@lean_balc_i_b_f", new string[] { "smoke", "smoke_b" });
            IV_Animations[155] = new IVAnimSet("amb@lean_idles", new string[] { "lean_idle_a" });
            IV_Animations[156] = new IVAnimSet("amb@lean_idl_b", new string[] { "look_at_watch", "scratch_head" });
            IV_Animations[157] = new IVAnimSet("amb@lean_phone_idles", new string[] { "lean_phone" });
            IV_Animations[158] = new IVAnimSet("amb@lean_smoke_idles", new string[] { "lean_stand" });
            IV_Animations[159] = new IVAnimSet("amb@look_dead_ped", new string[] { "shakes_head", "shakes_head_b" });
            IV_Animations[160] = new IVAnimSet("amb@mag_vendor", new string[] { "give_obj", "take_obj" });
            IV_Animations[161] = new IVAnimSet("amb@misc", new string[] { "aches_back", "lost", "scratch_head_stand", "scratch_head_walk", "scratch_neck", "stand_look_at_watch", "walk_look_at_watch", "yawn_long", "yawn_rub_eyes", "yawn_rub_eyes_walk", "yawn_short_stand", "yawn_short_walk" });
            IV_Animations[162] = new IVAnimSet("amb@mission_end", new string[] { "partial_wave_a" });
            IV_Animations[163] = new IVAnimSet("amb@music_create", new string[] { "walk_start_mp3" });
            IV_Animations[164] = new IVAnimSet("amb@music_destroy", new string[] { "walk_put_mp3_away" });
            IV_Animations[165] = new IVAnimSet("amb@music_hold", new string[] { "walk_hold_mp3" });
            IV_Animations[166] = new IVAnimSet("amb@music_idles", new string[] { "walk_hold_mp3_idle" });
            IV_Animations[167] = new IVAnimSet("amb@newspaper_create", new string[] { "create_walk" });
            IV_Animations[168] = new IVAnimSet("amb@newspaper_destry", new string[] { "walk_destroy" });
            IV_Animations[169] = new IVAnimSet("amb@newspaper_hold", new string[] { "walk_hold" });
            IV_Animations[170] = new IVAnimSet("amb@newspaper_idles", new string[] { "walk_read" });
            IV_Animations[171] = new IVAnimSet("amb@newspaper_vend", new string[] { "get_paper" });
            IV_Animations[172] = new IVAnimSet("amb@nightclub_ext", new string[] { "bouncer_a_checkid", "bouncer_a_frisk", "bouncer_a_refuse_entry", "bouncer_b_welcome", "clubber_frisked", "clubber_id_check", "smoke_light_up", "smoke_stand_a", "smoke_stand_b", "smoke_stub_out", "street_argue_a", "street_argue_b", "street_argue_f_a", "street_argue_f_b", "street_chat_a", "street_chat_b", "street_chat_f_a", "street_chat_f_b", "wasteda" });
            IV_Animations[173] = new IVAnimSet("amb@nutcart", new string[] { "buy_nuts", "buy_nuts_plyr", "eat_nuts_plyr", "eat_walk", "sell_nuts", "sell_nuts_plyr", "stvendor_custmr_pay", "stvendor_pay" });
            IV_Animations[174] = new IVAnimSet("amb@nuts_create", new string[] { "create_walk" });
            IV_Animations[175] = new IVAnimSet("amb@nuts_destroy", new string[] { "destroy_stand", "destroy_walk" });
            IV_Animations[176] = new IVAnimSet("amb@nuts_hold", new string[] { "hold_stand", "hold_walk" });
            IV_Animations[177] = new IVAnimSet("amb@nuts_idle", new string[] { "eat_stand", "eat_walk" });
            IV_Animations[178] = new IVAnimSet("amb@park_taichi_a", new string[] { "taichi01" });
            IV_Animations[179] = new IVAnimSet("amb@park_taichi_b", new string[] { "taichi02" });
            IV_Animations[180] = new IVAnimSet("amb@payphone", new string[] { "angryman", "cellphone_in", "cellphone_out", "cellphone_talk", "duck_cower", "m_enternumbers", "pick_up_phone", "put_down_phone" });
            IV_Animations[181] = new IVAnimSet("amb@payphone_create", new string[] { "stand_create" });
            IV_Animations[182] = new IVAnimSet("amb@payphone_idl_a", new string[] { "idle_a", "idle_b" });
            IV_Animations[183] = new IVAnimSet("amb@payphone_idl_b", new string[] { "idle_c", "idle_d", "idle_e" });
            IV_Animations[184] = new IVAnimSet("amb@phone_f", new string[] { "idle_quick_call", "stand_text", "walk_quick_call", "walk_text" });
            IV_Animations[185] = new IVAnimSet("amb@phone_m_a", new string[] { "idle_quick_call", "stand_text" });
            IV_Animations[186] = new IVAnimSet("amb@phone_m_b", new string[] { "walk_quick_call", "walk_text" });
            IV_Animations[187] = new IVAnimSet("amb@pimps_pros", new string[] { "argue_a", "argue_b", "car_chat_b", "car_proposition", "girl_hug", "give_obj", "rejection", "smoke_light_up", "smoke_stand_a", "smoke_stand_b", "smoke_stub_out", "street_argue_f_a", "street_argue_f_b", "take_obj", "you_and_me" });
            IV_Animations[188] = new IVAnimSet("amb@plank_create", new string[] { "stand_create" });
            IV_Animations[189] = new IVAnimSet("amb@plank_hold", new string[] { "stand_carry" });
            IV_Animations[190] = new IVAnimSet("amb@postman_idles", new string[] { "search_letterbox", "sort_letters" });
            IV_Animations[191] = new IVAnimSet("amb@preach_idl_a", new string[] { "idle_a", "idle_b" });
            IV_Animations[192] = new IVAnimSet("amb@preach_idl_b", new string[] { "idle_c", "idle_d" });
            IV_Animations[193] = new IVAnimSet("amb@preen", new string[] { "comb_hair" });
            IV_Animations[194] = new IVAnimSet("amb@preen_bsness", new string[] { "brushoff_suit_stand", "tie_adjust_stand", "tie_adjust_walk" });
            IV_Animations[195] = new IVAnimSet("amb@rain_newspaper", new string[] { "hold_above_head" });
            IV_Animations[196] = new IVAnimSet("amb@rain_newspaper_b", new string[] { "hold_above_head" });
            IV_Animations[197] = new IVAnimSet("amb@rake_create", new string[] { "rake_create" });
            IV_Animations[198] = new IVAnimSet("amb@rake_f_create", new string[] { "rake_create" });
            IV_Animations[199] = new IVAnimSet("amb@rake_f_idles", new string[] { "stand_idle_a", "stand_idle_b", "stand_idle_c" });
            IV_Animations[200] = new IVAnimSet("amb@rake_f_walk", new string[] { "walk_rake" });
            IV_Animations[201] = new IVAnimSet("amb@rake_hold", new string[] { "walk_rake" });
            IV_Animations[202] = new IVAnimSet("amb@rake_idles", new string[] { "stand_idle_a", "stand_idle_b", "stand_idle_c" });
            IV_Animations[203] = new IVAnimSet("amb@rake_walk", new string[] { "walk_rake" });
            IV_Animations[204] = new IVAnimSet("amb@roadcross", new string[] { "wait_at_kerb" });
            IV_Animations[205] = new IVAnimSet("amb@roadcross_head", new string[] { "lookaround_a", "lookaround_b", "walk_lookaround_a", "walk_lookaround_b" });
            IV_Animations[206] = new IVAnimSet("amb@roadcross_rain", new string[] { "run_across_road", "wait_at_kerb", "walk_across_road" });
            IV_Animations[207] = new IVAnimSet("amb@roadcross_rain_b", new string[] { "walk_across_road_b" });
            IV_Animations[208] = new IVAnimSet("amb@rubbish", new string[] { "throw_right" });
            IV_Animations[209] = new IVAnimSet("amb@savegame", new string[] { "angry_get_out_bed_l", "get_out_bed_l", "get_out_bed_r", "lie_on_bed_l", "lie_on_bed_r", "upset_get_out_bed_l", "upset_in_bed_idle" });
            IV_Animations[210] = new IVAnimSet("amb@security_idles_a", new string[] { "idle_answer_radio_a", "idle_answer_radio_b" });
            IV_Animations[211] = new IVAnimSet("amb@security_idles_b", new string[] { "idle_blow_hands", "idle_hear_noise", "walk", "walk_answer_radio_a" });
            IV_Animations[212] = new IVAnimSet("amb@security_idles_c", new string[] { "idle_lookaround_a", "idle_lookaround_b", "idle_stretch_a" });
            IV_Animations[213] = new IVAnimSet("amb@security_idles_d", new string[] { "idle_play_with_belt", "walk_answer_radio_b" });
            IV_Animations[214] = new IVAnimSet("amb@service_idles_a", new string[] { "hit", "kick" });
            IV_Animations[215] = new IVAnimSet("amb@service_idles_b", new string[] { "scratch_head", "work_a" });
            IV_Animations[216] = new IVAnimSet("amb@shock", new string[] { "shock_a", "shock_b", "shock_c", "shock_d", "shock_e", "shock_f" });
            IV_Animations[217] = new IVAnimSet("amb@shock_events", new string[] { "head_down_look_straight", "inquisitive", "look_over_shoulder", "look_shocked", "point_nervously", "shake_fist" });
            IV_Animations[218] = new IVAnimSet("amb@shoes_a", new string[] { "brush_shoe", "look_at_shoes", "stand_in_crap" });
            IV_Animations[219] = new IVAnimSet("amb@shoes_b", new string[] { "tie_shoe", "walk_tie_shoe" });
            IV_Animations[220] = new IVAnimSet("amb@shop_int", new string[] { "f_insertcard", "f_wait", "m_getoutwallet_pocket", "m_insertcard", "m_putwalletaway_pocket", "m_takecash", "m_wait" });
            IV_Animations[221] = new IVAnimSet("amb@sledge_create", new string[] { "stand_create" });
            IV_Animations[222] = new IVAnimSet("amb@sledge_idles", new string[] { "rest_stand", "swing_stand", "wipe_stand" });
            IV_Animations[223] = new IVAnimSet("amb@smk_scn_create", new string[] { "stand_create", "walk_create" });
            IV_Animations[224] = new IVAnimSet("amb@smk_scn_create_f", new string[] { "stand_create", "walk_create" });
            IV_Animations[225] = new IVAnimSet("amb@smk_scn_destroy", new string[] { "walk_destroy" });
            IV_Animations[226] = new IVAnimSet("amb@smk_scn_dest_f", new string[] { "walk_destroy" });
            IV_Animations[227] = new IVAnimSet("amb@smk_scn_idles", new string[] { "stand_smoke", "walk_smoke" });
            IV_Animations[228] = new IVAnimSet("amb@smk_scn_idles_f", new string[] { "stand_smoke", "walk_smoke" });
            IV_Animations[229] = new IVAnimSet("amb@smoking", new string[] { "stand_smoke", "walk_smoke" });
            IV_Animations[230] = new IVAnimSet("amb@smoking_create", new string[] { "stand_create", "walk_create" });
            IV_Animations[231] = new IVAnimSet("amb@smoking_create_f", new string[] { "stand_create", "walk_create" });
            IV_Animations[232] = new IVAnimSet("amb@smoking_destroy", new string[] { "walk_destroy" });
            IV_Animations[233] = new IVAnimSet("amb@smoking_dest_f", new string[] { "walk_destroy" });
            IV_Animations[234] = new IVAnimSet("amb@smoking_f", new string[] { "stand_smoke", "walk_smoke" });
            IV_Animations[235] = new IVAnimSet("amb@smoking_idles", new string[] { "stand_smoke", "walk_smoke" });
            IV_Animations[236] = new IVAnimSet("amb@smoking_idles_f", new string[] { "stand_smoke", "walk_smoke" });
            IV_Animations[237] = new IVAnimSet("amb@smoking_spliff", new string[] { "create_spliff", "partial_smoke", "partial_smoke_car" });
            IV_Animations[238] = new IVAnimSet("amb@spade_idles", new string[] { "dig_rest_stand", "dig_stand" });
            IV_Animations[239] = new IVAnimSet("amb@sprunk_ped", new string[] { "buy_drink" });
            IV_Animations[240] = new IVAnimSet("amb@sprunk_plyr", new string[] { "buy_drink", "partial_drink" });
            IV_Animations[241] = new IVAnimSet("amb@standing_female", new string[] { "look_around", "look_at_nails", "look_at_watch" });
            IV_Animations[242] = new IVAnimSet("amb@standing_male", new string[] { "crick_neck", "look_around", "look_at_watch" });
            IV_Animations[243] = new IVAnimSet("amb@standing_vendor", new string[] { "idle_a", "idle_b", "idle_c" });
            IV_Animations[244] = new IVAnimSet("amb@step_idles", new string[] { "sit_idle_a" });
            IV_Animations[245] = new IVAnimSet("amb@step_idles_b", new string[] { "idle_a", "idle_c", "idle_d" });
            IV_Animations[246] = new IVAnimSet("amb@step_idl_a", new string[] { "sit_idle_a", "sit_idle_b" });
            IV_Animations[247] = new IVAnimSet("amb@step_idl_b", new string[] { "sit_idle_c", "sit_idle_d" });
            IV_Animations[248] = new IVAnimSet("amb@stoop_argue", new string[] { "argue_a", "argue_b", "sit_down", "sit_loop" });
            IV_Animations[249] = new IVAnimSet("amb@stop_create", new string[] { "stand_create" });
            IV_Animations[250] = new IVAnimSet("amb@stop_idles", new string[] { "stand_idle_a", "stand_idle_b" });
            IV_Animations[251] = new IVAnimSet("amb@strip_chr_idls_a", new string[] { "clap_hands", "clap_hands_b", "leer" });
            IV_Animations[252] = new IVAnimSet("amb@strip_chr_idls_b", new string[] { "wave_over" });
            IV_Animations[253] = new IVAnimSet("amb@strip_rail_idl", new string[] { "lean_rail_a", "lean_rail_b", "lean_rail_c", "lean_rail_d", "lean_rail_e" });
            IV_Animations[254] = new IVAnimSet("amb@strip_stool_i_a", new string[] { "clap_hands", "clap_hands_b", "leer" });
            IV_Animations[255] = new IVAnimSet("amb@strip_stool_i_b", new string[] { "wave_over" });
            IV_Animations[256] = new IVAnimSet("amb@super_create", new string[] { "stand_create" });
            IV_Animations[257] = new IVAnimSet("amb@super_idles_a", new string[] { "stand_idle_a" });
            IV_Animations[258] = new IVAnimSet("amb@super_idles_b", new string[] { "stand_idle_b", "stand_idle_c" });
            IV_Animations[259] = new IVAnimSet("amb@super_part", new string[] { "hold" });
            IV_Animations[260] = new IVAnimSet("amb@sweep_create", new string[] { "walk_create" });
            IV_Animations[261] = new IVAnimSet("amb@sweep_ffood_idl", new string[] { "sweep_idle_a", "sweep_idle_b" });
            IV_Animations[262] = new IVAnimSet("amb@sweep_ffood_i_f", new string[] { "sweep_idle_a", "sweep_idle_b" });
            IV_Animations[263] = new IVAnimSet("amb@sweep_hold", new string[] { "walk_hold" });
            IV_Animations[264] = new IVAnimSet("amb@sweep_idles", new string[] { "sweep_stand", "sweep_stand_b", "sweep_walkcycle" });
            IV_Animations[265] = new IVAnimSet("amb@taxi", new string[] { "taxi_driver_chat", "taxi_driver_chat_in", "taxi_driver_chat_out", "taxi_driver_meter_press" });
            IV_Animations[266] = new IVAnimSet("amb@taxi_hail_f", new string[] { "hail_left", "hail_right" });
            IV_Animations[267] = new IVAnimSet("amb@taxi_hail_m", new string[] { "hail_left", "hail_right" });
            IV_Animations[268] = new IVAnimSet("amb@taxi_wait_i_f_a", new string[] { "failed_hail_a", "failed_hail_b" });
            IV_Animations[269] = new IVAnimSet("amb@taxi_wait_i_f_b", new string[] { "lean_look", "look_around", "look_watch", "tiptoe" });
            IV_Animations[270] = new IVAnimSet("amb@taxi_wait_i_m_a", new string[] { "failed_hail", "failed_hail_b" });
            IV_Animations[271] = new IVAnimSet("amb@taxi_wait_i_m_b", new string[] { "failed_hail_c", "lean_look", "look_around" });
            IV_Animations[272] = new IVAnimSet("amb@taxi_wait_i_m_c", new string[] { "look_watch", "tiptoe" });
            IV_Animations[273] = new IVAnimSet("amb@telegraph_idles", new string[] { "hammer", "look_down", "look_down_b" });
            IV_Animations[274] = new IVAnimSet("amb@telescope_idles", new string[] { "stand_focus", "stand_money", "stand_point" });
            IV_Animations[275] = new IVAnimSet("amb@telescope_idl_f", new string[] { "stand_focus", "stand_money", "stand_point" });
            IV_Animations[276] = new IVAnimSet("amb@telescope_player", new string[] { "look_intro" });
            IV_Animations[277] = new IVAnimSet("amb@tollbooth", new string[] { "give_money", "idle", "indicate_left_a", "indicate_left_b", "toss_money_chopper", "toss_money_dirt", "toss_money_freeway", "toss_money_scooter", "toss_money_spt" });
            IV_Animations[278] = new IVAnimSet("amb@tourist_camera", new string[] { "take_pictures" });
            IV_Animations[279] = new IVAnimSet("amb@tourist_create", new string[] { "stand_create" });
            IV_Animations[280] = new IVAnimSet("amb@tourist_f", new string[] { "lookaround", "lost", "take_pictures" });
            IV_Animations[281] = new IVAnimSet("amb@tourist_hold", new string[] { "hold_camera_f", "hold_camera_m" });
            IV_Animations[282] = new IVAnimSet("amb@tourist_idles_a", new string[] { "lookaround", "lost" });
            IV_Animations[283] = new IVAnimSet("amb@tourist_idles_b", new string[] { "take_pictures", "walk" });
            IV_Animations[284] = new IVAnimSet("amb@tripup", new string[] { "trip_up" });
            IV_Animations[285] = new IVAnimSet("amb@umbrella_close", new string[] { "stand_close", "walk_close" });
            IV_Animations[286] = new IVAnimSet("amb@umbrella_f_fat", new string[] { "umbrella_check_for_rain", "umbrella_close", "umbrella_idle_hold", "umbrella_idle_hold_walk", "umbrella_open" });
            IV_Animations[287] = new IVAnimSet("amb@umbrella_hold", new string[] { "stand_hold", "walk_hold" });
            IV_Animations[288] = new IVAnimSet("amb@umbrella_idl_a", new string[] { "walk_check_rain", "walk_collar" });
            IV_Animations[289] = new IVAnimSet("amb@umbrella_idl_b", new string[] { "walk_lookaround", "wind_struggle" });
            IV_Animations[290] = new IVAnimSet("amb@umbrella_open_a", new string[] { "run_for_cover", "stand_lookatsky", "stand_open" });
            IV_Animations[291] = new IVAnimSet("amb@umbrella_open_b", new string[] { "walk_collar_up", "walk_open" });
            IV_Animations[292] = new IVAnimSet("amb@vendor", new string[] { "eat_hotdog", "eat_walk", "give_obj", "hotdog_buy", "hotdog_vend", "idle_stvendor", "idle_stvendor_scratch", "stand_eat_fastfood_2", "stvendor_custmr_pay", "stvendor_pay", "take_obj", "vendor_give", "vendor_recieve" });
            IV_Animations[293] = new IVAnimSet("amb@wallet", new string[] { "stand_check_wallet" });
            IV_Animations[294] = new IVAnimSet("amb@wall_idles", new string[] { "sit_idle_a", "sit_idle_b", "sit_idle_c", "sit_idle_d" });
            IV_Animations[295] = new IVAnimSet("amb@wall_idles_f", new string[] { "sit_idle_a", "sit_idle_b", "sit_idle_c" });
            IV_Animations[296] = new IVAnimSet("amb@wall_read_idl", new string[] { "turn_page" });
            IV_Animations[297] = new IVAnimSet("amb@wall_read_idl_f", new string[] { "turn_page" });
            IV_Animations[298] = new IVAnimSet("amb@wasted_a", new string[] { "idle_a" });
            IV_Animations[299] = new IVAnimSet("amb@wasted_b", new string[] { "idle_a" });
            IV_Animations[300] = new IVAnimSet("amb@watch_melee", new string[] { "cheer_on_fight_a", "cheer_on_fight_b", "cheer_on_fight_c", "cheer_on_fight_d", "cheer_on_fight_e" });
            IV_Animations[301] = new IVAnimSet("amb@wcan_create", new string[] { "can_create" });
            IV_Animations[302] = new IVAnimSet("amb@wcan_idles", new string[] { "stand_idle_a", "stand_idle_b" });
            IV_Animations[303] = new IVAnimSet("amb@wcan_part", new string[] { "hold_can" });
            IV_Animations[304] = new IVAnimSet("amb@winclean_idl", new string[] { "clean_a", "clean_b", "clean_c" });
            IV_Animations[305] = new IVAnimSet("amb@winshop_idles", new string[] { "stand_idle_a", "stand_idle_b", "stand_idle_c" });
            IV_Animations[306] = new IVAnimSet("amb@yawn", new string[] { "yawn" });
            IV_Animations[307] = new IVAnimSet("amb_sit_chair_f", new string[] { "cower", "flee_front", "flee_l", "flee_r", "get_up_front", "get_up_l", "get_up_r", "sit_down_front", "sit_down_front_b", "sit_down_idle_01", "sit_down_l", "sit_down_r" });
            IV_Animations[308] = new IVAnimSet("amb_sit_chair_m", new string[] { "cower", "flee_front", "flee_l", "flee_r", "get_up_front", "get_up_l", "get_up_r", "sit_down_front", "sit_down_front_b", "sit_down_idle_01", "sit_down_l", "sit_down_r" });
            IV_Animations[309] = new IVAnimSet("amb_sit_couch_f", new string[] { "cower", "flee_front", "get_up_front", "sit_down_front", "sit_down_front_b", "sit_down_idle_01" });
            IV_Animations[310] = new IVAnimSet("amb_sit_couch_m", new string[] { "cower", "flee_front", "get_up_front", "sit_down_front", "sit_down_front_b", "sit_down_idle_01" });
            IV_Animations[311] = new IVAnimSet("amb_sit_step_m", new string[] { "flee_front", "get_up_front", "sit_down_front", "sit_down_front_b", "sit_down_idle_01" });
            IV_Animations[312] = new IVAnimSet("amb_sit_stool_f", new string[] { "cower", "flee_l", "flee_r", "get_up_l", "get_up_r", "sit_down_idle_01", "sit_down_l", "sit_down_r" });
            IV_Animations[313] = new IVAnimSet("amb_sit_stool_m", new string[] { "cower", "flee_l", "flee_r", "get_up_l", "get_up_r", "sit_down_idle_01", "sit_down_l", "sit_down_r" });
            IV_Animations[314] = new IVAnimSet("amb_sit_wall_m", new string[] { "flee_front", "get_up_front", "sit_down_front", "sit_down_idle_01" });
            IV_Animations[315] = new IVAnimSet("arrest", new string[] { "cop_std_arrest", "cop_std_arrest_in" });
            IV_Animations[316] = new IVAnimSet("audio", new string[] { "walk" });
            IV_Animations[317] = new IVAnimSet("audio_motionbuilder", new string[] { "walk" });
            IV_Animations[318] = new IVAnimSet("audio_voiceomatic", new string[] { "walk" });
            IV_Animations[319] = new IVAnimSet("avoids", new string[] { "avoid_bl", "avoid_br", "avoid_fl", "avoid_fr", "avoid_lb", "avoid_lf", "avoid_rb", "avoid_rf" });
            IV_Animations[320] = new IVAnimSet("busted", new string[] { "busted_on_bike", "idle_2_hands_up" });
            IV_Animations[321] = new IVAnimSet("car_bomb", new string[] { "car_set_bomb" });
            IV_Animations[322] = new IVAnimSet("car_boot", new string[] { "close_boot", "open_boot" });
            IV_Animations[323] = new IVAnimSet("cellphone", new string[] { "cellphone_in", "cellphone_out", "cellphone_talk", "cellphone_text", "cell_text_to_ear" });
            IV_Animations[324] = new IVAnimSet("climb_std", new string[] { "climb_idle", "climb_jump", "fall_back", "fall_collapse", "fall_fall", "fall_front", "fall_glide", "fall_land", "hang_drop_rifle", "hang_drop_unarmed", "hang_to_waist", "ladder_climb", "ladder_climb_down", "ladder_climb_down_run", "ladder_climb_run", "ladder_getoff_top", "ladder_geton", "ladder_geton_top", "ladder_geton_topb", "ladder_idle", "ladder_idle_ambient", "ladder_jumpoff", "ladder_slide", "landing_head_height", "landing_stretch_height", "landing_waist_height", "shimmy_l", "shimmy_r", "vault_end", "vault_end_r", "vault_start", "vault_start_r", "vault_start_standing", "vault_start_standing_r", "vault_to_stand_rifle", "vault_to_stand_rifle_r", "vault_to_stand_unarmed", "vault_to_stand_unarmed_r", "waist_to_hang", "waist_to_stand_rifle", "waist_to_stand_unarmed", "waist_to_vault", "waist_to_vault_shallow" });
            IV_Animations[325] = new IVAnimSet("clothing", new string[] { "brushoff_suit_stand", "examine glasses", "examine glasses_b", "examine hat", "examine hat_b", "examine legs", "examine shirt", "examine shoes", "hat_put_on_l", "hat_put_on_r", "hat_russian_put_on_l", "hat_russian_put_on_r", "hat_russian_take_off_l", "hat_russian_take_off_r", "hat_take_off_l", "hat_take_off_r", "lookaround_a", "reach_high", "reach_low", "reach_med", "specs_put_on_l", "specs_put_on_r", "specs_take_off_l", "specs_take_off_r", "tie_adjust_stand", "turn_l180", "turn_r180" });
            IV_Animations[326] = new IVAnimSet("config_screen", new string[] { "ak47_aim_f", "ak47_fire_f", "ak47_holdster_f", "ak47_idle_f", "ak47_reload_f", "ak47_unholdster_f", "handgun_aim_f", "handgun_fire_f", "handgun_holdster_f", "handgun_idle_f", "handgun_reload_f", "handgun_unholdster_f" });
            IV_Animations[327] = new IVAnimSet("config_screen_f", new string[] { "hgun_in", "hgun_loop", "hgun_out", "rifle_in", "rifle_loop", "rifle_out", "unarmed_loop" });
            IV_Animations[328] = new IVAnimSet("config_screen_m", new string[] { "hgun_in", "hgun_loop", "hgun_out", "rifle_in", "rifle_loop", "rifle_out", "unarmed_loop" });
            IV_Animations[329] = new IVAnimSet("cop", new string[] { "armsup_2_searched_pose", "armsup_loop", "copm_arrest_ground", "copm_bonnetarrest", "copm_gunstance", "copm_idle_2_gunstance", "copm_licenseintro_ncar", "copm_licenseintro_truck", "copm_licenseloop_ncar", "copm_licenseloop_truck", "copm_licenseoutro_ncar", "copm_licenseoutro_truck", "copm_searchboot", "cop_cuff", "cop_search", "crim_cuffed", "crim_searched", "dir_traffic_gofwd_in", "dir_traffic_gofwd_loop", "dir_traffic_gofwd_out", "dir_traffic_goright_in", "dir_traffic_goright_loop", "dir_traffic_goright_out", "dir_traffic_idle", "dir_traffic_in", "dir_traffic_out", "gunstance_2_copm_idle", "idle_guard_m_clnshoe", "idle_guard_m_pointl", "idle_guard_m_pointr", "idle_guard_m_scratch", "plyr_bonnetarrest", "plyr_idle_2_armsup", "plyr_licenseintro_ncar", "plyr_licenseintro_truck", "plyr_licenseloop_ncar", "plyr_licenseloop_truck", "plyr_licenseoutro_ncar", "plyr_licenseoutro_truck", "searched_pose" });
            IV_Animations[330] = new IVAnimSet("cop_search_idles", new string[] { "idle_answer_radio_a", "idle_answer_radio_b", "idle_check_ground", "idle_check_under_vehicle", "idle_hear_noise", "idle_lookaround", "idle_point", "idle_wave_over_cops", "walk_answer_radio_a", "walk_answer_radio_b" });
            IV_Animations[331] = new IVAnimSet("cop_wander_idles", new string[] { "idle_adjust_hat", "idle_answer_radio_a", "idle_answer_radio_b", "idle_lookaround_a", "idle_lookaround_b", "idle_look_at_watch", "idle_look_left", "idle_look_right", "idle_play_with_belt", "idle_shake_feet", "idle_stretch_a", "idle_stretch_back", "walk_lookaround" });
            IV_Animations[332] = new IVAnimSet("cop_wander_idles_fat", new string[] { "idle_lookaround_a", "idle_lookaround_b", "idle_look_at_watch", "idle_look_left", "idle_look_right", "idle_play_with_belt", "idle_shake_feet", "idle_stretch_a", "idle_stretch_back", "walk_lookaround" });
            IV_Animations[333] = new IVAnimSet("cop_wander_radio", new string[] { "idle_answer_radio_a", "idle_answer_radio_b", "walk_answer_radio_a", "walk_answer_radio_b" });
            IV_Animations[334] = new IVAnimSet("cop_wander_radio_fat", new string[] { "idle_answer_radio_a", "idle_answer_radio_b", "walk_answer_radio_a", "walk_answer_radio_b" });
            IV_Animations[335] = new IVAnimSet("cover_dive", new string[] { "high_l_pistol", "high_l_pistol_short", "high_l_rifle", "high_l_rifle_short", "high_r_pistol", "high_r_pistol_short", "high_r_rifle", "high_r_rifle_short", "low_l_pistol", "low_l_pistol_short", "low_l_rifle", "low_l_rifle_short", "low_r_pistol", "low_r_pistol_short", "low_r_rifle", "low_r_rifle_short" });
            IV_Animations[336] = new IVAnimSet("cover_l_high_centre", new string[] { "pistol_cower", "pistol_flip_180", "pistol_idle", "rifle_cower", "rifle_flip_180", "rifle_idle", "unarmed_flip_180", "unarmed_idle" });
            IV_Animations[337] = new IVAnimSet("cover_l_high_corner", new string[] { "ak47_blindfire", "lowerhalf_fire", "pistol_blindfire", "pistol_cower", "pistol_flip_180", "pistol_idle", "pistol_normal_fire_intro", "pistol_normal_fire_outro", "pistol_peek", "rifle_blindfire", "rifle_cower", "rifle_flip_180", "rifle_idle", "rifle_normal_fire_intro", "rifle_normal_fire_outro", "rifle_peek", "rocket_blindfire", "shotgun_blindfire", "throw_molotov", "unarmed_flip_180", "unarmed_idle", "unarmed_peek", "uzi_blindfire" });
            IV_Animations[338] = new IVAnimSet("cover_l_low_centre", new string[] { "ak47_blindfire", "lowerhalf_fire", "pistol_blindfire", "pistol_cower", "pistol_flip_180", "pistol_idle", "pistol_normal_fire_intro", "pistol_normal_fire_outro", "pistol_peek", "rifle_blindfire", "rifle_cower", "rifle_flip_180", "rifle_idle", "rifle_normal_fire_intro", "rifle_normal_fire_outro", "rifle_peek", "rocket_blindfire", "shotgun_blindfire", "throw_molotov", "unarmed_flip_180", "unarmed_idle", "unarmed_peek", "uzi_blindfire" });
            IV_Animations[339] = new IVAnimSet("cover_l_low_corner", new string[] { "ak47_blindfire", "lowerhalf_fire", "pistol_blindfire", "pistol_cower", "pistol_flip_180", "pistol_idle", "pistol_normal_fire_intro", "pistol_normal_fire_outro", "pistol_peek", "rifle_blindfire", "rifle_cower", "rifle_flip_180", "rifle_idle", "rifle_normal_fire_intro", "rifle_normal_fire_outro", "rifle_peek", "rocket_blindfire", "shotgun_blindfire", "throw_molotov", "unarmed_flip_180", "unarmed_idle", "unarmed_peek", "uzi_blindfire" });
            IV_Animations[340] = new IVAnimSet("cover_r_high_centre", new string[] { "pistol_cower", "pistol_flip_180", "pistol_idle", "rifle_cower", "rifle_flip_180", "rifle_idle", "unarmed_flip_180", "unarmed_idle" });
            IV_Animations[341] = new IVAnimSet("cover_r_high_corner", new string[] { "ak47_blindfire", "lowerhalf_fire", "pistol_blindfire", "pistol_cower", "pistol_flip_180", "pistol_idle", "pistol_normal_fire_intro", "pistol_normal_fire_outro", "pistol_peek", "rifle_blindfire", "rifle_cower", "rifle_flip_180", "rifle_idle", "rifle_normal_fire_intro", "rifle_normal_fire_outro", "rifle_peek", "rocket_blindfire", "shotgun_blindfire", "throw_molotov", "unarmed_flip_180", "unarmed_idle", "unarmed_peek", "uzi_blindfire" });
            IV_Animations[342] = new IVAnimSet("cover_r_low_centre", new string[] { "ak47_blindfire", "lowerhalf_fire", "pistol_blindfire", "pistol_cower", "pistol_flip_180", "pistol_idle", "pistol_normal_fire_intro", "pistol_normal_fire_outro", "pistol_peek", "rifle_blindfire", "rifle_cower", "rifle_flip_180", "rifle_idle", "rifle_normal_fire_intro", "rifle_normal_fire_outro", "rifle_peek", "rocket_blindfire", "shotgun_blindfire", "throw_molotov", "unarmed_flip_180", "unarmed_idle", "unarmed_peek", "uzi_blindfire" });
            IV_Animations[343] = new IVAnimSet("cover_r_low_corner", new string[] { "ak47_blindfire", "lowerhalf_fire", "pistol_blindfire", "pistol_cower", "pistol_flip_180", "pistol_idle", "pistol_normal_fire_intro", "pistol_normal_fire_outro", "pistol_peek", "rifle_blindfire", "rifle_cower", "rifle_flip_180", "rifle_idle", "rifle_normal_fire_intro", "rifle_normal_fire_outro", "rifle_peek", "rocket_blindfire", "shotgun_blindfire", "throw_molotov", "unarmed_flip_180", "unarmed_idle", "unarmed_peek", "uzi_blindfire" });
            IV_Animations[344] = new IVAnimSet("dam_ad", new string[] { "back", "front", "left", "right" });
            IV_Animations[345] = new IVAnimSet("dam_ko", new string[] { "drown", "ko_back", "ko_collapse", "ko_front", "ko_left", "ko_right" });
            IV_Animations[346] = new IVAnimSet("dam_rec_civi", new string[] { "floor_back", "floor_front" });
            IV_Animations[347] = new IVAnimSet("dam_rec_player", new string[] { "abdomen", "arm_left_upper_front", "arm_right_upper_front", "back_lower", "back_upper", "chest", "dam_partial_aim_2hands", "dam_partial_aim_2hands_b", "dam_partial_aim_2hands_f", "dam_partial_aim_2hands_l", "dam_partial_aim_2hands_r", "dam_partial_armed_back", "dam_partial_armed_fwd", "dam_partial_armed_l", "dam_partial_armed_r", "dam_partial_b", "dam_partial_f", "dam_partial_l", "dam_partial_r", "head_front", "head_left", "head_right", "leg_left_lower", "leg_right_lower" });
            IV_Animations[348] = new IVAnimSet("defend@gen_1h", new string[] { "idle_blow_hands", "idle_check_ground", "idle_hear_noise", "idle_lookaround_a", "idle_lookaround_b", "idle_lookaround_c", "idle_look_at_watch", "idle_play_with_belt", "idle_smoke", "idle_stretch_a", "walk", "walk_pistol" });
            IV_Animations[349] = new IVAnimSet("defend@gen_2h", new string[] { "idle_checkgun", "idle_lookaround", "idle_lookback", "idle_lookleft", "idle_shakelegs", "idle_shoulder", "idle_stretch", "idle_weapondown", "walk_lookaround", "walk_lowerweapon" });
            IV_Animations[350] = new IVAnimSet("doors", new string[] { "door_knock", "plyr_shldropen" });
            IV_Animations[351] = new IVAnimSet("ev_dives", new string[] { "avoid_bl", "avoid_br", "avoid_fl", "avoid_fr", "avoid_lb", "avoid_lf", "avoid_rb", "avoid_rf", "plyr_roll_left", "plyr_roll_right" });
            IV_Animations[352] = new IVAnimSet("facials@f_hi", new string[] { "angry_a" });
            IV_Animations[353] = new IVAnimSet("facials@f_lo", new string[] { "abe_angry_b", "angry_a", "angry_b", "blow", "chew", "dead_a", "dead_b", "gest_angry_intro", "gest_angry_loop", "gest_angry_outro", "gest_normal_loop", "gest_surprised_intro", "gest_surprised_loop", "gest_surprised_outro", "gest_think_intro", "gest_think_loop", "gest_think_outro", "gun_aim", "keystart", "lj_create_spliff", "lj_smoke_spliff", "lookaround", "look_down", "look_left", "look_right", "look_up", "mood_angry", "mood_injured", "mood_normal", "mood_scared", "music_listen", "pain_a", "pain_b", "pain_c", "shocked", "skinning", "yawn" });
            IV_Animations[354] = new IVAnimSet("facials@m_hi", new string[] { "aim_cue", "angry_a", "angry_b", "angry_c", "blow", "chew", "dead_a", "dead_b", "die_a", "gest_angry_intro", "gest_angry_loop", "gest_angry_outro", "gest_normal_loop", "gest_surprised_intro", "gest_surprised_loop", "gest_surprised_outro", "gest_think_intro", "gest_think_loop", "gest_think_outro", "gun_aim", "heavybreath", "keystart", "lookaround", "look_down", "look_left", "look_right", "pain_a", "pain_b", "pain_c", "plyr_mood_angry", "plyr_mood_happy", "plyr_mood_normal", "police_chase", "shocked", "whatever" });
            IV_Animations[355] = new IVAnimSet("facials@m_lo", new string[] { "angry_a", "angry_b", "blow", "chew", "dead_a", "dead_b", "gest_angry_intro", "gest_angry_loop", "gest_angry_outro", "gest_normal_loop", "gest_surprised_intro", "gest_surprised_loop", "gest_surprised_outro", "gest_think_intro", "gest_think_loop", "gest_think_outro", "gun_aim", "keystart", "lj_create_spliff", "lj_smoke_spliff", "lookaround", "look_down", "look_left", "look_right", "look_up", "mood_angry", "mood_injured", "mood_normal", "mood_scared", "music_listen", "pain_a", "pain_b", "pain_c", "shocked", "skinning", "yawn" });
            IV_Animations[356] = new IVAnimSet("food", new string[] { "player_pay", "serve_fastfood", "serve_fastfood_obj", "sick", "sit_diner_intro", "sit_diner_loop", "sit_diner_outro", "sit_eat_diner_intro", "sit_eat_diner_loop", "sit_eat_diner_outro", "stand_eat_fastfood", "stand_eat_fastfood_2", "stand_eat_food_obj", "waitress_stand" });
            IV_Animations[357] = new IVAnimSet("gestures@car", new string[] { "are_you_in", "but_why", "come_on", "despair", "disbelief", "dont_hit_me", "dont_know", "easy_now", "forget_it", "goddamn", "good", "how_could_you", "im talking_2_you", "im_not_sure", "im_telling_you", "indicate_listener", "its_done", "i_get_it", "i_said_no", "i_will", "let_me_think", "nod_no", "nod_yes", "of_course", "oh_shit", "ok_ok", "please", "shit", "shock", "shut_up", "smoke", "stop", "uptight", "u_thin_i'm_stupid", "whatever", "yeah_i_got_it", "yes", "youre_right", "you_dig", "you_will_love this" });
            IV_Animations[358] = new IVAnimSet("gestures@car_f", new string[] { "absolutely", "all_gone", "cant_be", "dont", "dont_u_dare", "get_this_straight", "gimme", "its_so_like", "i_cant", "i_dont_care", "i_oughta", "just_go", "leave_it", "look_at_me", "no", "no_thanks", "no_way", "of_coarse", "oh_come_on", "oh_my_god", "oh_no", "ok", "possibly", "shocked", "so_tough", "this_high", "threaten", "upset", "u_should", "what!_u_wish", "yeah_sure", "your_right" });
            IV_Animations[359] = new IVAnimSet("gestures@female", new string[] { "absolutely", "agree", "all_gone", "cant_be", "clap", "definitely_not", "dont", "dont_even", "dont_know", "dont_u_dare", "get_lost", "get_this_straight", "gimme", "go_over", "hi_ya", "indicate_bwd", "indicate_fwd_a", "indicate_fwd_b", "indicate_left", "indicate_right", "its_so_like", "i_cant", "i_dont_care", "i_dont_think_so", "i_oughta", "just_go", "leave_it", "let_me_tell_u", "look_at_me", "maybe_u", "me", "never_on_your_life", "no", "no_thanks", "no_u_cant", "no_way", "no_your_wrong", "of_coarse", "oh_come_on", "oh_my_god", "oh_no", "ok", "over_there", "possibly", "shocked", "so_tough", "sure", "take_it", "thank_u", "their_so_fat", "this_big", "this_high", "threaten", "upset", "up_yours", "u_just_watch", "u_should", "what!_u_wish", "why_not", "yeah_ok", "yeah_sure", "your_right" });
            IV_Animations[360] = new IVAnimSet("gestures@male", new string[] { "absolutely", "agree", "amazing", "anger_a", "are_you_in", "bring_it_on", "bring_it_to_me", "but_why", "come_here", "come_on", "damn", "despair", "disbelief", "dont_hit_me", "dont_know", "do_it", "easy_now", "enough", "exactly", "fold_arms_oh_yeah", "forget_it", "give_me_a_break", "goddamn", "good", "go_away", "hello", "hey", "how", "how_could_you", "how_much", "if_u_say_so", "ill_do_it", "im talking_2_you", "im_begging_you", "im_not_sure", "im_sorry", "im_telling_you", "indicate_back", "indicate_left", "indicate_listener", "indicate_right_b", "indicate_right_c", "is_this_it", "its_done", "its_mine", "its_ok", "ive_forgot", "i_cant_say", "i_couldnt", "i_dont_have", "i_dont_think_so", "i_get_it", "i_give_up", "i_said_no", "i_will", "kiss_my_ass", "later", "leave_it_2_me", "let_me_think", "like_this", "me", "natuarally", "negative", "nod_no", "nod_yes", "not_me", "not_sure", "no_chance", "no_really", "numbnuts", "of_course", "oh_shit", "ok", "ok_ok", "over_there", "piss_off", "please", "point_fwd", "point_right", "positive", "raise_hands", "say_again", "screw_you", "shit", "shock", "shut_up", "stop", "sure", "tell_me_about_it", "that", "that_way", "this_and_that", "this_big", "threaten", "time", "tosser", "touch_face", "to_hell_with_it", "unbelievable", "uptight", "u_cant_do_that", "u_serious", "u_talkin_2_me", "u_thin_i'm_stupid", "u_understand", "want_some_of_this", "we", "well", "well_alright", "we_can_do_it", "what", "whatever", "whatever_c", "why", "wot_the_fuck", "yeah_i_got_it", "yes", "youre_right", "you_dig", "you_will_love this" });
            IV_Animations[361] = new IVAnimSet("gestures@mp_female", new string[] { "finger", "rock", "salute", "wave" });
            IV_Animations[362] = new IVAnimSet("gestures@mp_male", new string[] { "finger", "rock", "salute", "wave" });
            IV_Animations[363] = new IVAnimSet("gestures@m_seated", new string[] { "absolutely", "agree", "amazing", "anger_a", "are_you_in", "bring_it_on", "bring_it_to_me", "but_why", "come_here", "come_on", "damn", "despair", "disbelief", "dont_hit_me", "dont_know", "do_it", "easy_now", "enough", "exactly", "fold_arms_oh_yeah", "forget_it", "goddamn", "good", "go_away", "how_could_you", "how_much", "im_telling_you", "indicate_listener", "its_ok", "i_cant_say", "i_couldnt", "i_give_up", "i_said_no", "i_will", "let_me_think", "negative", "nod_no", "not_me", "no_chance", "oh_shit", "ok", "please", "raise_hands", "shit", "shock", "stop", "that_way", "this_and_that", "unbelievable", "u_serious", "u_thin_i'm_stupid", "u_understand", "we", "we_can_do_it", "whatever", "yeah_i_got_it", "yes", "youre_right", "you_dig", "you_will_love this" });
            IV_Animations[364] = new IVAnimSet("gestures@niko", new string[] { "absolutely", "agree", "amazing", "anger_a", "are_you_in", "bring_it_on", "bring_it_to_me", "but_why", "come_here", "come_on", "damn", "despair", "disbelief", "dont_hit_me", "dont_know", "do_it", "easy_now", "enough", "exactly", "fold_arms_oh_yeah", "forget_it", "give_me_a_break", "goddamn", "good", "go_away", "hello", "hey", "holds_up_fingers", "how", "how_could_you", "how_much", "if_u_say_so", "ill_do_it", "im talking_2_you", "im_begging_you", "im_not_sure", "im_sorry", "im_telling_you", "indicate_back", "indicate_left", "indicate_listener", "indicate_right_b", "indicate_right_c", "is_this_it", "its_done", "its_mine", "its_ok", "ive_forgot", "i_cant_say", "i_couldnt", "i_dont_have", "i_dont_think_so", "i_get_it", "i_give_up", "i_said_no", "i_will", "kiss_my_ass", "later", "leave_it_2_me", "let_me_think", "like_this", "me", "natuarally", "negative", "nod_no", "nod_yes", "not_me", "not_sure", "no_chance", "no_really", "numbnuts", "of_course", "oh_shit", "ok", "ok_ok", "over_there", "piss_off", "please", "point_fwd", "point_right", "positive", "raise_hands", "say_again", "screw_you", "shit", "shock", "shut_up", "stop", "sure", "tell_me_about_it", "that", "that_way", "this_and_that", "this_big", "threaten", "time", "tosser", "touch_face", "to_hell_with_it", "unbelievable", "uptight", "u_cant_do_that", "u_serious", "u_talkin_2_me", "u_thin_i'm_stupid", "u_understand", "want_some_of_this", "we", "well", "well_alright", "we_can_do_it", "what", "whatever", "whatever_c", "why", "wot_the_fuck", "yeah_i_got_it", "yes", "youre_right", "you_dig", "you_will_love this" });
            IV_Animations[365] = new IVAnimSet("get_up", new string[] { "get_up_fast", "get_up_injured", "get_up_normal", "get_up_slow" });
            IV_Animations[366] = new IVAnimSet("get_up_back", new string[] { "get_up_fast", "get_up_normal", "get_up_slow" });
            IV_Animations[367] = new IVAnimSet("gun@aim_idles", new string[] { "handgun" });
            IV_Animations[368] = new IVAnimSet("gun@ak47", new string[] { "aim_2_holster", "dbfire", "discard", "discard_crouch", "fire", "fire_crouch", "fire_down", "fire_up", "holster", "holster_2_aim", "holster_crouch", "melee", "melee_crouch", "p_load", "reload", "reload_crouch", "unholster", "unholster_crouch", "wall_block_idle" });
            IV_Animations[369] = new IVAnimSet("gun@baretta", new string[] { "discard", "discard_crouch", "fire", "fire_crouch", "holster", "holster_2_aim", "holster_crouch", "melee", "melee_crouch", "reload", "reload_crouch", "unholster", "unholster_crouch", "wall_block_idle" });
            IV_Animations[370] = new IVAnimSet("gun@cops", new string[] { "pistol_partial_a", "pistol_partial_b", "swat_rifle", "swat_rifle_crouch" });
            IV_Animations[371] = new IVAnimSet("gun@deagle", new string[] { "dbfire", "dbfire_l", "discard", "discard_crouch", "fire", "fire_crouch", "holster", "holster_2_aim", "holster_crouch", "melee", "melee_crouch", "reload", "reload_crouch", "unholster", "unholster_crouch", "wall_block_idle" });
            IV_Animations[372] = new IVAnimSet("gun@handgun", new string[] { "dbfire", "dbfire_l", "discard", "discard_crouch", "fire", "fire_crouch", "holster", "holster_2_aim", "holster_crouch", "melee", "melee_crouch", "reload", "reload_crouch", "unholster", "unholster_crouch", "wall_block_idle" });
            IV_Animations[373] = new IVAnimSet("gun@mp5k", new string[] { "dbfire", "dbfire_l", "discard", "discard_crouch", "fire", "fire_crouch", "holster", "holster_2_aim", "holster_crouch", "melee", "melee_crouch", "p_load", "reload", "reload_crouch", "unholster", "unholster_crouch", "wall_block_idle" });
            IV_Animations[374] = new IVAnimSet("gun@partials", new string[] { "rifle_a", "rifle_b", "rocket_a", "rocket_b", "shotgun", "shotgun_a", "shotgun_b", "swat_rifle", "swat_rifle_crouch" });
            IV_Animations[375] = new IVAnimSet("gun@rifle", new string[] { "aim_2_holster", "dbfire", "discard", "discard_crouch", "fire", "fire_alt", "fire_crouch", "fire_crouch_alt", "holster", "holster_2_aim", "holster_crouch", "melee", "melee_crouch", "p_load", "reload", "reload_crouch", "unholster", "unholster_crouch", "wall_block_idle" });
            IV_Animations[376] = new IVAnimSet("gun@rocket", new string[] { "discard", "discard_crouch", "fire", "fire_crouch", "holster", "holster_2_aim", "holster_crouch", "melee", "melee_crouch", "reload", "reload_crouch", "unholster", "unholster_crouch", "wall_block_idle" });
            IV_Animations[377] = new IVAnimSet("gun@shotgun", new string[] { "discard", "discard_crouch", "fire", "fire_crouch", "holster", "holster_2_aim", "holster_crouch", "melee", "melee_crouch", "reload", "reload_crouch", "unholster", "unholster_crouch", "wall_block_idle" });
            IV_Animations[378] = new IVAnimSet("gun@uzi", new string[] { "dbfire", "dbfire_l", "discard", "discard_crouch", "fire", "fire_crouch", "holster", "holster_2_aim", "holster_crouch", "melee", "melee_crouch", "reload", "reload_crouch", "unholster", "unholster_crouch", "wall_block_idle" });
            IV_Animations[379] = new IVAnimSet("gunlocker", new string[] { "open_door" });
            IV_Animations[380] = new IVAnimSet("injured", new string[] { "inj_back_idle", "inj_default_to_back", "inj_default_to_rside", "inj_rside_idle" });
            IV_Animations[381] = new IVAnimSet("jump_rifle", new string[] { "jump_inair_l", "jump_inair_r", "jump_land_l", "jump_land_r", "jump_land_roll", "jump_land_squat", "jump_on_spot", "jump_takeoff_l", "jump_takeoff_r" });
            IV_Animations[382] = new IVAnimSet("jump_std", new string[] { "jump_inair_l", "jump_inair_r", "jump_land_l", "jump_land_r", "jump_land_roll", "jump_land_squat", "jump_on_spot", "jump_takeoff_l", "jump_takeoff_r" });
            IV_Animations[383] = new IVAnimSet("lift_box", new string[] { "crry_prtial", "liftup", "putdwn" });
            IV_Animations[384] = new IVAnimSet("medic", new string[] { "medic_cpr_in", "medic_cpr_loop", "medic_cpr_out" });
            IV_Animations[385] = new IVAnimSet("melee_baseball_core", new string[] { "block", "ground_attack_a", "ground_attack_a_recoil", "hit_hook_l", "hit_hook_r", "hit_low_kick", "hook_l", "hook_l_recoil", "hook_r", "hook_r_recoil", "idle", "idle_outro", "low_kick_r", "low_kick_recoil", "p_punch", "run", "run_strafe_b", "run_strafe_l", "run_strafe_r", "walk", "walk_strafe_b", "walk_strafe_l", "walk_strafe_r" });
            IV_Animations[386] = new IVAnimSet("melee_baseball_extra", new string[] { "batbutt", "batbutt_recoil", "counter_back", "counter_left", "counter_right", "dam_block_front", "dam_block_left", "dam_block_right", "dodge_back", "dodge_l", "dodge_r", "headbutt", "hit_counter_back", "hit_counter_left", "hit_counter_right", "hit_headbutt", "hit_knee", "hit_lowblow_l", "hit_lowblow_r", "hit_low_kick_long", "hit_uppercut_l", "hit_uppercut_r", "hook_r_long", "knee", "knee_recoil", "lowblow_l", "lowblow_l_recoil", "lowblow_r", "lowblow_r_recoil", "low_kick_r_long", "low_kick_r_long_recoil", "shove", "uppercut_l", "uppercut_l_recoil", "uppercut_r", "uppercut_r_recoil" });
            IV_Animations[387] = new IVAnimSet("melee_counters", new string[] { "counter_back", "counter_back_2", "counter_back_3", "counter_left", "counter_left_2", "counter_left_3", "counter_right", "counter_right_2", "counter_right_3", "disarmed_bat", "disarmed_knife_b", "disarmed_knife_l", "disarmed_knife_r", "disarm_bat", "disarm_knife_b", "disarm_knife_l", "disarm_knife_r", "dodge_back", "dodge_l", "dodge_r", "hit_counter_back", "hit_counter_back_2", "hit_counter_back_3", "hit_counter_left", "hit_counter_left_2", "hit_counter_left_3", "hit_counter_right", "hit_counter_right_2", "hit_counter_right_3" });
            IV_Animations[388] = new IVAnimSet("melee_gang_unarmed", new string[] { "headbutt", "headbutt_recoil", "idle", "jab", "jab_recoil", "knee", "knee_recoil", "low_kick_long", "low_kick_long_recoil", "low_kick_nuts", "low_kick_nuts_recoil", "l_elbow", "l_elbow_recoil", "l_hook", "l_hook_recoil", "l_lowblow", "l_lowblow_recoil", "l_uppercut", "l_uppercut_recoil", "move_away", "r_cross", "r_cross_recoil", "r_elbow", "r_elbow_recoil", "r_hook", "r_hook_recoil", "r_lowblow", "r_lowblow_recoil", "r_uppercut", "r_uppercut_recoil", "shove" });
            IV_Animations[389] = new IVAnimSet("melee_gun", new string[] { "hold_pistol", "melee", "melee_ak", "melee_ak_crouch", "melee_crouch" });
            IV_Animations[390] = new IVAnimSet("melee_hits_common", new string[] { "dam_block_front", "dam_block_left", "dam_block_right", "hit_back_lower", "hit_back_upper", "hit_elbow_l", "hit_elbow_r", "hit_headbutt", "hit_hook_l", "hit_hook_l_long", "hit_hook_r", "hit_hook_r_long", "hit_knee", "hit_lowblow_l", "hit_lowblow_r", "hit_low_kick_long", "hit_side_left_lo", "hit_side_right_lo", "hit_uppercut_l", "hit_uppercut_r", "shoved_b", "shoved_f", "shoved_l", "shoved_r" });
            IV_Animations[391] = new IVAnimSet("melee_holsters", new string[] { "holster", "holster_crouch", "unholster", "unholster_crouch" });
            IV_Animations[392] = new IVAnimSet("melee_knife_core", new string[] { "block", "hit_low_kick", "hit_med_swipe_start_a", "hit_med_swipe_start_b", "idle", "idle_outro", "low_kick", "low_kick_recoil", "med_swipe_start_a", "med_swipe_start_a_recoil", "med_swipe_start_b", "med_swipe_start_b_recoil", "partial_swipe", "run", "run_strafe_b", "run_strafe_l", "run_strafe_r", "walk", "walk_strafe_b", "walk_strafe_l", "walk_strafe_r" });
            IV_Animations[393] = new IVAnimSet("melee_knife_extra", new string[] { "counter_back", "counter_left", "counter_right", "dam_block_front", "dam_block_left", "dam_block_right", "dodge_back", "dodge_left", "dodge_right", "fight_idle_intro", "fight_idle_outro", "ground_attack", "ground_attack_b", "ground_attack_b_recoil", "ground_attack_recoil", "hit_counter_back", "hit_counter_left", "hit_counter_right", "hit_knee", "hit_long_swipe_a", "hit_long_swipe_b", "hit_low_kick_long", "hit_med_swipe_finish_a", "hit_med_swipe_finish_b", "hit_med_swipe_link_a", "hit_med_swipe_link_b", "hit_short_swipe_finish_a", "hit_short_swipe_finish_b", "hit_short_swipe_link_a", "hit_short_swipe_link_b", "hit_short_swipe_start_a", "hit_short_swipe_start_b", "hit_side_left_hi", "hit_side_left_lo", "hit_side_right_hi", "hit_side_right_lo", "knee", "knee_recoil", "long_swipe_a", "long_swipe_b", "long_swipe_b_recoil", "low_kick_long", "low_kick_long_recoil", "low_kick_recoil", "med_swipe_finish_a", "med_swipe_finish_b", "med_swipe_link_a", "med_swipe_link_b", "short_swipe_finish_a", "short_swipe_finish_a_recoil", "short_swipe_finish_b", "short_swipe_finish_b_recoil", "short_swipe_link_a", "short_swipe_link_a_recoil", "short_swipe_link_b", "short_swipe_start_a", "short_swipe_start_a_recoil", "short_swipe_start_b", "short_swipe_start_b_recoil", "shove", "slow_long_swipe_a", "slow_long_swipe_a_recoil", "slow_long_swipe_b", "slow_long_swipe_b_recoil", "slow_short_swipe_a", "slow_short_swipe_a_recoil" });
            IV_Animations[394] = new IVAnimSet("melee_ped_unarmed", new string[] { "hook_l", "hook_l_long", "hook_l_long_recoil", "hook_l_recoil", "hook_r", "hook_r_long", "hook_r_long_recoil", "hook_r_recoil", "idle", "idle_outro", "jab", "jab_recoil", "knee", "knee_recoil", "long_kick", "long_kick_recoil", "move_away", "r_lowblow", "r_lowblow_recoil", "shove" });
            IV_Animations[395] = new IVAnimSet("melee_player_ground", new string[] { "ground_attack_b", "ground_attack_c", "ground_attack_c_recoil", "ground_attack_d", "ground_attack_d_recoil" });
            IV_Animations[396] = new IVAnimSet("melee_player_unarmed", new string[] { "elbow_l", "elbow_l_recoil", "elbow_r", "elbow_r_recoil", "fight_intro_03", "fight_intro_04", "headbutt", "headbutt_recoil", "hook_l", "hook_l_long", "hook_l_long_recoil", "hook_l_recoil", "hook_r", "hook_r_long", "hook_r_long_recoil", "hook_r_recoil", "knee", "knee_recoil", "lowblow_l", "lowblow_l_recoil", "lowblow_r", "lowblow_r_recoil", "low_kick_r_long", "low_kick_r_long_recoil", "stun_a", "taunt_03", "uppercut_l", "uppercut_l_recoil", "uppercut_r", "uppercut_r_recoil" });
            IV_Animations[397] = new IVAnimSet("melee_unarmed_base", new string[] { "block", "cross_r", "cross_r_recoil", "ground_attack_a", "hit_back", "hit_cross_r", "hit_jab", "hit_left", "hit_low_kick", "hit_melee_gun", "hit_right", "jab", "jab_recoil", "low_kick_r", "low_kick_recoil", "partial_punch_r", "stun_punch" });
            IV_Animations[398] = new IVAnimSet("mini_bowling", new string[] { "average", "celeb_a", "celeb_b", "celeb_c", "curse_a", "curse_b", "curse_c", "f_celeb_a", "f_celeb_b", "f_curse_a", "f_curse_b", "f_ped_strafe_l", "f_ped_strafe_r", "idle01", "idle02", "idle03", "idle04", "idle05", "idle_action", "idle_relaxed", "niko_pick_up_l", "niko_pick_up_r", "ped_strafe_l", "ped_strafe_r", "pick_up_l", "pick_up_r", "shot", "shot_b", "strafe_l", "strafe_r", "walk_celeb", "walk_fail" });
            IV_Animations[399] = new IVAnimSet("mini_golf", new string[] { "golf_chip", "golf_chip_short", "golf_curse_b", "golf_drive", "golf_drive_curse", "golf_drive_curse_b", "golf_drive_left_handed", "golf_leave_tee", "golf_limber_left_handed", "golf_limber_up", "golf_limber_up2", "golf_putt_lose", "golf_putt_win", "golf_putt_win_b", "putt", "putt_limber", "putt_lose", "putt_setup", "putt_win" });
            IV_Animations[400] = new IVAnimSet("mini_pool", new string[] { "chalk_cue", "f_long_shot_end", "f_long_shot_fire", "f_long_shot_idle", "f_long_shot_start", "f_med_shot_end", "f_med_shot_fire", "f_med_shot_idle", "f_med_shot_start", "f_short_shot_end", "f_short_shot_fire", "f_short_shot_idle", "f_short_shot_start", "f_xlong_shot_end", "f_xlong_shot_fire", "f_xlong_shot_idle", "f_xlong_shot_start", "idle", "long_shot_end", "long_shot_fire", "long_shot_idle", "long_shot_start", "med_shot_end", "med_shot_fire", "med_shot_idle", "med_shot_start", "obj_idle", "obj_long_in", "obj_long_out", "obj_med_in", "obj_med_out", "obj_short_in", "obj_short_out", "obj_xlong_in", "obj_xlong_out", "short_shot_end", "short_shot_fire", "short_shot_idle", "short_shot_start", "xlong_shot_end", "xlong_shot_fire", "xlong_shot_idle", "xlong_shot_start" });
            IV_Animations[401] = new IVAnimSet("missambtv", new string[] { "sit_down", "sit_loop" });
            IV_Animations[402] = new IVAnimSet("missbadman_1", new string[] { "argue_b", "mcivi_flagtaxi_in", "street_chat_b" });
            IV_Animations[403] = new IVAnimSet("missbankjob", new string[] { "dufflebag_drop", "dufflebag_walk", "fem_downloop", "hndcuff_lieloop", "holster_2_aim", "idle", "idle_hot_wipe_face", "idle_look_back", "idle_look_r", "indicate_left_a", "remove_balaclave_a", "remove_balaclave_b", "searchped_intro", "searchped_loop", "see_heli_a", "see_heli_b", "see_heli_c", "sheild_eyes" });
            IV_Animations[404] = new IVAnimSet("missbdb_2", new string[] { "bomb", "bomb_unarmed", "open_door", "press_button", "pull_lever" });
            IV_Animations[405] = new IVAnimSet("missbell2", new string[] { "gbge_smoke", "street_chat_a", "street_chat_b", "van_close_doors" });
            IV_Animations[406] = new IVAnimSet("missbell4", new string[] { "chubby_turn_180", "crouch_roll_l", "crouch_roll_r", "fall", "hang_idle", "hang_on_heli", "heli_jump", "jump_fail", "jump_success", "land_on_heli", "panic_screamturn_f", "reach_up", "street_chat_a", "street_chat_b", "wave_down" });
            IV_Animations[407] = new IVAnimSet("missbell6", new string[] { "boot_withdraw", "garage_door_prop", "load_moneybags", "open_garage_door", "open_van_door", "open_van_doors", "plead_idle" });
            IV_Animations[408] = new IVAnimSet("missbernie1", new string[] { "attacker_beatthenrun", "attacker_wait", "crossarmnlookaway", "default_idle", "inj_rside_idle", "run", "victim_hit", "warmup" });
            IV_Animations[409] = new IVAnimSet("missbernie2", new string[] { "bernie_waiting" });
            IV_Animations[410] = new IVAnimSet("missbernie3", new string[] { "point", "take_cover_bernie", "take_cover_niko" });
            IV_Animations[411] = new IVAnimSet("missbrian_1", new string[] { "brian", "friendly_idle", "mcivi_flagtaxi_in", "niko" });
            IV_Animations[412] = new IVAnimSet("missbrian_2", new string[] { "argue_b", "cantstandstill_idle", "drugs_deal", "give_obj", "hand_package", "mcivi_flagtaxi_in", "plyr_sit_down", "recieve_package", "street_chat_b", "take_obj" });
            IV_Animations[413] = new IVAnimSet("missbrian_3", new string[] { "back_loop", "back_outro", "baseball_attack", "cover_cower", "cower_flinch", "cower_idle", "ev_step", "fight_intro_03", "fight_intro_04", "friendly_idle", "get_up_slow", "give_obj", "ground_attack_a", "hand_package", "injured_idle", "mcivi_flagtaxi_in", "plyr_sit_down", "street_chat_b", "take_obj", "taunt_03" });
            IV_Animations[414] = new IVAnimSet("missbrian_a", new string[] { "argue_a", "argue_b", "drugs_deal", "mcivi_flagtaxi_in", "niko_recieve_phone", "roman_give_phone", "street_chat_b", "what_a" });
            IV_Animations[415] = new IVAnimSet("missbrucie1", new string[] { "climb_out_window", "door_knock", "piss_interupted", "plyr_shldropen" });
            IV_Animations[416] = new IVAnimSet("missbrucie2", new string[] { "car_look_r", "look_left_car_intro", "look_left_car_loop", "mechanic_look_at_car" });
            IV_Animations[417] = new IVAnimSet("missbrucie3", new string[] { "carwash_d", "f_attention_wave", "niko_diselief", "niko_is_this_it", "niko_i_cant_say", "niko_i_couldnt", "niko_i_will", "niko_let_me_think", "niko_nod_no", "niko_nod_yes", "niko_not_me", "niko_tell_me_about_it", "niko_well_alright", "niko_yes", "tom_absolutely", "tom_agree", "tom_all_gone", "tom_cant_be", "tom_definitely_not", "tom_dont_even", "tom_dont_know", "tom_dont_u_dare", "tom_hi_ya", "tom_i_cant", "tom_i_dont_care", "tom_i_dont_think_so", "tom_let_me_tell_u", "tom_maybe_u", "tom_me", "tom_no", "tom_no_way", "tom_of_coarse", "tom_oh_come_on", "tom_oh_my_god", "tom_shocked", "tom_thank_u", "tom_this_high", "tom_u_just_watch", "tom_what!_u_wish", "tom_your_right" });
            IV_Animations[418] = new IVAnimSet("missbrucie4", new string[] { "niko_incar_partial" });
            IV_Animations[419] = new IVAnimSet("misscabaret", new string[] { "clean_glass", "idle_stretch_a", "pull_pint", "sit_down_idle_01", "sit_idle_a", "sit_idle_b", "use_optic", "wipe_counter" });
            IV_Animations[420] = new IVAnimSet("misscar_sex", new string[] { "f_blowjob_intro", "f_blowjob_intro_low", "f_blowjob_loop", "f_blowjob_loop_low", "f_blowjob_outro", "f_blowjob_outro_low", "f_handjob_intro", "f_handjob_intro_low", "f_handjob_loop", "f_handjob_loop_low", "f_handjob_outro", "f_handjob_outro_low", "f_sex_intro", "f_sex_intro_low", "f_sex_loop", "f_sex_loop_low", "f_sex_outro", "f_sex_outro_low", "m_blowjob_intro", "m_blowjob_intro_low", "m_blowjob_loop", "m_blowjob_loop_low", "m_blowjob_outro", "m_blowjob_outro_low", "m_handjob_intro", "m_handjob_intro_low", "m_handjob_loop", "m_handjob_loop_low", "m_handjob_outro", "m_handjob_outro_low", "m_sex_intro", "m_sex_intro_low", "m_sex_loop", "m_sex_loop_low", "m_sex_outro", "m_sex_outro_low" });
            IV_Animations[421] = new IVAnimSet("misscherise", new string[] { "female_ilde", "stand_smoke" });
            IV_Animations[422] = new IVAnimSet("misscia2", new string[] { "car_chat", "car_chat_outside", "gbge_smoke", "plead" });
            IV_Animations[423] = new IVAnimSet("misscia3", new string[] { "smoke_stand_a", "smoke_stub_out", "street_chat_a", "street_chat_b" });
            IV_Animations[424] = new IVAnimSet("misscia4", new string[] { "heli_enter", "heli_exit", "heli_fire", "heli_idle", "heli_reload" });
            IV_Animations[425] = new IVAnimSet("misscopbootsearch", new string[] { "car_chat_outside", "car_chat_outside_2", "close_boot" });
            IV_Animations[426] = new IVAnimSet("missderrick2", new string[] { "pass_text" });
            IV_Animations[427] = new IVAnimSet("missderrick3", new string[] { "back", "come_on", "conversation", "crim_searched", "do_it", "idle01", "idle_answer_radio_a", "idle_lookaround_a", "idle_quick_call", "idle_scratch_balls", "indicate_right_b", "indicate_right_c", "in_car_panic", "mirror_c", "plead", "point_fwd", "prisoner_pass", "scratch_neck", "shockturnplead", "sit_drive", "that_way", "yawn_short_walk" });
            IV_Animations[428] = new IVAnimSet("missdrug_fact", new string[] { "stash" });
            IV_Animations[429] = new IVAnimSet("missdwayne1", new string[] { "avoid_br", "cherise_avoid", "girl_hug", "girl_hug2", "guy_hug2", "nervous_idle", "niko_jump", "player_execute", "player_kiss", "plead", "scared_female", "unholster_shoot_run", "victim" });
            IV_Animations[430] = new IVAnimSet("missdwayne2", new string[] { "crowd_chant_f", "drop_knees", "play_pinball", "play_videogame", "plead_idle", "use_vendmac" });
            IV_Animations[431] = new IVAnimSet("missdwayne3", new string[] { "chat_a", "chat_b", "chat_outro_l", "chat_outro_r", "dance_loop_b", "lean_balcony_a", "lean_balcony_b", "lean_balcony_c", "lean_rail_a", "lean_rail_b", "lean_rail_c", "play_videogame", "thug_1", "thug_2" });
            IV_Animations[432] = new IVAnimSet("misseddie1", new string[] { "bag_idle", "bag_run", "eddie_idle", "throw_object" });
            IV_Animations[433] = new IVAnimSet("misseddie2", new string[] { "eddie_idle", "getup_back_fast" });
            IV_Animations[434] = new IVAnimSet("misselizabeta1", new string[] { "abuse", "angry", "angryman", "annoyed", "door_knock", "friendly", "get_outa_here_a", "jam_intro", "jam_loop", "keystart", "packie_struggle_intro2", "packie_struggle_loop", "packie_struggle_outro2", "perp_struggle_intro", "perp_struggle_loop", "perp_struggle_outro2", "sit_drive", "smoke_light_up", "smoke_stand_a", "smoke_stand_b", "smoke_stub_out", "street_chat_a", "thankyou" });
            IV_Animations[435] = new IVAnimSet("misselizabeta2", new string[] { "dont_know_d" });
            IV_Animations[436] = new IVAnimSet("misselizabeta3", new string[] { "abseil_copter_in", "argue_b", "badguy_talk_loop", "car_chat_a", "car_chat_b", "crchsignal_moveout", "crouchidle2idle", "idle", "idle_guard_m_clnshoe", "idle_guard_m_scratch", "smoke_stand_a" });
            IV_Animations[437] = new IVAnimSet("misselizabeta4", new string[] { "arm_bounce", "close_boot", "deadped_a", "deadped_b" });
            IV_Animations[438] = new IVAnimSet("missemergencycall", new string[] { "idle_adjust_hat", "idle_answer_radio_a", "idle_lookaround_b", "medic_health_inject", "player_health_recieve" });
            IV_Animations[439] = new IVAnimSet("missfaustin1", new string[] { "van_open_doors", "van_pullover_npc", "van_pullover_plyr" });
            IV_Animations[440] = new IVAnimSet("missfaustin2", new string[] { "cower_idle", "drop_knees", "give_n_pistolwhip", "give_obj", "nod_yes_a", "pistolwhip_standing", "pistolwhip_standingv2", "plead_idle", "point_fwd", "reaction_fear", "reaction_shock", "shock_to_plead", "take_n_pistolwhip", "take_obj" });
            IV_Animations[441] = new IVAnimSet("missfaustin3", new string[] { "180l_a", "180l_b", "90l_a", "90l_b", "abuse", "argue_a", "climb_trainplatform", "point_fwd", "smoke_light_up" });
            IV_Animations[442] = new IVAnimSet("missfaustin4", new string[] { "are_you_crazy", "smoke_light_up", "smoke_stand_a", "smoke_stand_b", "smoke_stub_out" });
            IV_Animations[443] = new IVAnimSet("missfaustin5", new string[] { "argue_a", "argue_b", "dive_from_explosion", "getup_back_fast" });
            IV_Animations[444] = new IVAnimSet("missfaustin6", new string[] { "argue_a", "assistant_catch_bullet", "cocky_plead", "magician_catch_bullet", "player_execute", "plead", "smoke_stand_a", "victim" });
            IV_Animations[445] = new IVAnimSet("missfaustin7", new string[] { "sit_diner_loop", "waitress_stand" });
            IV_Animations[446] = new IVAnimSet("missfaustin8", new string[] { "bye", "carsmoke_passenger", "crchsignal_gofwd", "partial_wave_d", "run_away_a", "run_away_b" });
            IV_Animations[447] = new IVAnimSet("missfinale1a", new string[] { "escapedownstairs", "escaperun", "niko_incar_partial", "plyr_shldropen", "searchped_loop" });
            IV_Animations[448] = new IVAnimSet("missfinale1b", new string[] { "gbge_smoke", "player_execute", "plead", "pull_lever", "street_chat_a", "street_chat_b", "victim" });
            IV_Animations[449] = new IVAnimSet("missfinale2a", new string[] { "boat_get_out", "exhausted_loop", "passback_carexit_intro", "passback_carexit_loop", "passfront_carexit_intro", "passfront_carexit_loop", "plyr_carexit_intro", "plyr_carexit_loop", "rom_chopper", "rom_out" });
            IV_Animations[450] = new IVAnimSet("missfinale2b", new string[] { "hostage_execution", "hostage_idle", "perp_execution", "perp_idle" });
            IV_Animations[451] = new IVAnimSet("missfinale2d", new string[] { "hang_on_heli", "heli_fire", "heli_idle", "jump_on_heli", "jump_on_heli_alt", "kicked_from_heli", "kick_inside_heli", "land_on_heli", "panic_a", "panic_b" });
            IV_Animations[452] = new IVAnimSet("missfinale2p", new string[] { "bike2heli", "bike2heli_climb1", "bike2heli_climb1success", "bike2heli_climb2", "bike2heli_climb2success", "bike2heli_idle1", "bike2heli_idle2", "leg_swing" });
            IV_Animations[453] = new IVAnimSet("missfinale2p_boat", new string[] { "boat2heli_p1", "boat2heli_p2", "boat2heli_p3", "boat2heli_p4" });
            IV_Animations[454] = new IVAnimSet("missfrancis1", new string[] { "cellphone_in", "cellphone_out", "handover_francis", "handover_niko", "look_shoulder_l", "look_shoulder_r", "scared_lookaround", "search_phone" });
            IV_Animations[455] = new IVAnimSet("missfrancis2", new string[] { "argue_b", "cellphone_talk", "cower_idle", "drop_tray_scream", "examine legs", "examine shirt", "fall_from_window", "front_intro", "front_loop", "front_outro", "give_obj", "holdtray_partial", "holdtray_walk", "idle", "indicate_right", "indicate_right_b", "law_walk_out_office", "m_takecash", "niko_seated", "nod_yes_a", "plyr_agree", "plyr_dont_know", "plyr_me", "plyr_nod", "plyr_seat_idle", "plyr_sit_idle_getup", "plyr_sit_loop", "plyr_sit_up", "plyr_what", "press_button", "press_coms", "push_trolley", "seated_read_doc", "see_gun_cower", "shakehands_lawyer", "shakehands_niko", "shakehands_ped", "shakehands_plyr", "sit_loop", "sure", "take_obj", "tie_adjust_stand", "walk_in_to_lawyer" });
            IV_Animations[456] = new IVAnimSet("missfrancis3", new string[] { "abuse", "lean_balcony_loopa", "no_way_c", "player_execute", "plead", "plead_stand_lower", "plead_turn_lower", "plead_upperbody", "point_fwd", "smoke_stand_a", "smoke_stand_b", "sprint", "sstop_l", "sstop_r", "stairs_chat_b", "street_chat_a", "turn_l", "turn_r", "victim" });
            IV_Animations[457] = new IVAnimSet("missfrancis4", new string[] { "get_out_bed_r", "hit_tv", "lie_on_bed_loop", "lie_on_bed_r", "phone_loop", "phone_paranoid", "phone_pickup", "phone_put_down", "plyr_bedlie_loop_rhs", "plyr_bedsit_in_rhs", "plyr_sit_2_lie_rhs", "slumped_couch", "spooked", "spooked_2nd_half", "spooked_outside" });
            IV_Animations[458] = new IVAnimSet("missfrancis5", new string[] { "broatdestin_lives", "bromeet_atdestination", "bromeet_atdestin_chat", "bromeet_todestination", "bromeet_todestin_chat", "brotodestin_lives", "car_chat", "car_chat_outside", "chatting_on_bench_a", "chatting_on_bench_b", "fake_hug_a", "fake_hug_b", "idle_bench_a", "idle_bench_b", "kneel_crying", "kneel_crying_intro", "kneel_dead_brother", "kneel_dead_brotherv2", "seat_chat_a", "seat_chat_b", "sit_shocked_l", "sit_shocked_r", "stand_greet", "stand_wait", "street_chat_a", "street_chat_b" });
            IV_Animations[459] = new IVAnimSet("missfrancis6", new string[] { "dead_coffin", "funeral_female1", "funeral_female2", "funeral_male1", "funeral_male2", "niko_lookabout", "packie_lookabout" });
            IV_Animations[460] = new IVAnimSet("missgambetti1", new string[] { "argue_a", "ev_dive", "handsup", "idle_stvendor", "player_die", "reload_crouch", "street_chat_a", "street_chat_b" });
            IV_Animations[461] = new IVAnimSet("missgambetti2", new string[] { "cleave", "drop_knees", "duck_cower", "kick_door", "plead", "plead_idle", "tell_off", "veg_chop" });
            IV_Animations[462] = new IVAnimSet("missgambetti3", new string[] { "pickup_obj", "push_trolley", "reload" });
            IV_Animations[463] = new IVAnimSet("missgerry1", new string[] { "car_chat_outside", "car_chat_outside_2", "duck_cower", "fixcar", "getofftrolley", "getontrolley", "piss_loop", "shakehands_ped", "shakehands_plyr", "slideout", "slideundercar", "smoke_stand_a", "street_chat_a", "street_chat_b", "wave_through" });
            IV_Animations[464] = new IVAnimSet("missgerry2", new string[] { "bike_point_r", "keystart", "prone_suffering", "turn_2_look" });
            IV_Animations[465] = new IVAnimSet("missgerry3", new string[] { "carwash_d", "dont_u_dare_v2", "girl_ko", "girl_ko_brake", "girl_ko_loop", "girl_ko_turn_l", "girl_ko_turn_r", "girl_phone_grabbed", "girl_scream", "girl_slap_player", "girl_threaten_gun", "girl_try2escape", "girl_try_escape", "girl_upset_idle", "girl_wheel_grab_l", "girl_wheel_grab_r", "player_slapped_girl", "plyr_grab_girl", "plyr_grab_phone", "plyr_ko_girl", "plyr_threaten_gun", "plyr_try2escape", "plyr_wheel_grabbed_l", "plyr_wheel_grabbed_r", "you_and_me" });
            IV_Animations[466] = new IVAnimSet("missgerry3c", new string[] { "girl_ko", "girl_tiedup_01", "girl_tiedup_02", "give_slap", "sit_loop", "take_slap" });
            IV_Animations[467] = new IVAnimSet("missgerry4", new string[] { "arm_right_upper_back", "car_chat_a", "despair_b", "hostage", "hostage_taker", "in_boot_dead", "numbnuts", "shove" });
            IV_Animations[468] = new IVAnimSet("missgerry4b", new string[] { "girl_tiedup_01", "girl_tiedup_02", "sit_loop" });
            IV_Animations[469] = new IVAnimSet("missgerry5", new string[] { "girl_tied_brake", "girl_tied_in_car", "girl_tied_in_car_shout", "girl_tied_lean_l", "girl_tied_lean_r", "lean_balcony_intro", "lean_balcony_loopa", "lean_balcony_loopd", "lean_balcony_outro", "niko_greet_packie", "packie_greet_niko", "passenger_waves", "throw_diamonds_intro", "throw_diamonds_loop", "throw_diamonds_outro" });
            IV_Animations[470] = new IVAnimSet("missgracie", new string[] { "smoke_default" });
            IV_Animations[471] = new IVAnimSet("missgunlockup", new string[] { "grenade_intro", "grenade_loop", "grenade_outro", "over_shoulder", "pistol_intro", "pistol_loop", "pistol_outro", "rifle_intro", "rifle_loop", "rifle_outro", "rpg_intro", "rpg_loop", "rpg_outro", "shotgun_intro", "shotgun_loop", "shotgun_outro", "stand_smoke", "uzi_intro", "uzi_loop", "uzi_outro" });
            IV_Animations[472] = new IVAnimSet("missgun_car", new string[] { "argue_a", "no_way_a", "no_way_b", "no_way_c", "open_trunk", "shut_trunk", "turist_pose_loop" });
            IV_Animations[473] = new IVAnimSet("missheli_tour", new string[] { "indicate_left_a", "indicate_right_a" });
            IV_Animations[474] = new IVAnimSet("misshossan1", new string[] { "argue_b", "back_intro", "back_loop", "back_outro", "friendly_idle", "give_obj", "hand_package", "headbutt", "mcivi_flagtaxi_in", "recieve_package", "take_obj" });
            IV_Animations[475] = new IVAnimSet("missilyena", new string[] { "female_ilde", "hostage_beat_up", "piss_off", "point_fwd", "stand_smoke" });
            IV_Animations[476] = new IVAnimSet("missint_ped", new string[] { "beckon_idle", "beckon_long", "beckon_med", "beckon_short" });
            IV_Animations[477] = new IVAnimSet("missivan_1", new string[] { "argue_b", "friendly_idle", "give_obj", "hand_package", "indicate_listener", "i_cant_say", "i_couldnt", "let_me_think", "mcivi_flagtaxi_in", "me", "recieve_package", "street_chat_b", "take_obj", "thuga_backoff", "u_thin_i'm_stupid", "walkback_wgun" });
            IV_Animations[478] = new IVAnimSet("missjacob1", new string[] { "bust_door_idle", "bust_open_door_2", "door_knock" });
            IV_Animations[479] = new IVAnimSet("missjacob2", new string[] { "argue_a", "argue_b", "beggar_beg", "beggar_sit", "braziera", "brazierb", "crates", "drugs_buy", "drugs_deal", "idle", "idle_look_back", "idle_look_l", "idle_look_r", "piss_loop", "run_lookback_l", "run_lookback_r", "street_chat_a", "street_chat_b", "vault_bonnet_stop", "walking_shove_l_" });
            IV_Animations[480] = new IVAnimSet("missjacob3", new string[] { "drugs_buy", "drugs_deal", "stash", "stash_lookaround" });
            IV_Animations[481] = new IVAnimSet("missjacobgc", new string[] { "argue_a", "no_way_a", "no_way_b", "no_way_c", "open_trunk", "shut_trunk" });
            IV_Animations[482] = new IVAnimSet("missjeff1", new string[] { "argue_a", "duck_behind_car", "duck_idle", "mcivi_flagtaxi_in", "sad_idle", "stand_up" });
            IV_Animations[483] = new IVAnimSet("missjeff2", new string[] { "fem_dead_in_car", "impatient_idle", "jeff_upset" });
            IV_Animations[484] = new IVAnimSet("missjeff3", new string[] { "jeff_hit", "sit_binocular_idle" });
            IV_Animations[485] = new IVAnimSet("missjimmy1", new string[] { "180l", "90l", "90l_bendover_enter", "90l_bendover_exit", "90l_bendover_loop", "backoff_intro", "backoff_loop", "back_lower", "badguy_talk_loop", "boss_pickup_injured", "boss_walk_injured", "car_talk_boss", "car_talk_player", "front_loop", "getup_front", "give_obj", "goon_idle", "idle_armed", "pegorino", "pegorino_idle", "player", "plyr_pickup_injured", "plyr_walk_injured", "point_fwd", "rifle_idle", "rifle_intro", "rifle_loop", "rifle_outro", "run", "showcase_intro", "showcase_loop", "takeoff_bag_from_shoulder", "take_obj", "turn_flee", "wasteda" });
            IV_Animations[486] = new IVAnimSet("missjimmy2", new string[] { "fuck_u", "smoke_loop" });
            IV_Animations[487] = new IVAnimSet("missjimmy3", new string[] { "buy_drink", "doc_lean_on_desk", "examine shirt", "goon_lean_on_wall", "goon_sit_crackknuck", "goon_sit_idle", "locker_reach", "reach_high", "recp_seated_point_bwd", "recp_seated_point_fwd", "recp_usecomp_lookinleft", "recp_usecomp_lookinup", "recp_usingcomp_a", "situp_bed_heartattack", "situp_bed_shot", "situp_bed_woozy", "wash_hands" });
            IV_Animations[488] = new IVAnimSet("missjimmy4", new string[] { "give_money" });
            IV_Animations[489] = new IVAnimSet("misskate_1", new string[] { "argue_a", "car_chat_outside", "dont_know", "driver_look_left", "ground_attack_a", "obvious_shrug", "offended", "pass_look_left", "sleazebag", "taunt_03", "taunt_04", "taunt_ground_03", "taunt_ground_04" });
            IV_Animations[490] = new IVAnimSet("misskbtruck", new string[] { "back_geton", "back_jump2hang", "climb_belly", "crawl_fwd_loop", "crawl_idle", "crawl_roll_left", "crawl_roll_right", "crawl_slide_left", "crawl_slide_right", "idle_checkleg_l", "idle_look_l", "idle_shakelegs", "jump_grab", "lean", "roof_falloff_left", "roof_falloff_right", "roof_leaponcab_right", "shocked", "side_succeed_climbup_l", "side_succeed_climbup_r", "sit_drive", "smoke_light_up", "smoke_stand_a", "struggle_driver", "struggle_player" });
            IV_Animations[491] = new IVAnimSet("misslift", new string[] { "get_in_lift_bottom", "get_in_lift_top", "operate_lift_intro", "operate_lift_outro", "operate_lift_switch" });
            IV_Animations[492] = new IVAnimSet("missmaniac3", new string[] { "idle", "point_fwd" });
            IV_Animations[493] = new IVAnimSet("missmanny1", new string[] { "door_knock", "player_execute", "plead", "street_argue_a", "street_argue_b", "street_chat_a", "street_chat_b", "victim", "whatever" });
            IV_Animations[494] = new IVAnimSet("missmanny2", new string[] { "smoke_light_up", "smoke_stand_a", "smoke_stand_b", "smoke_stub_out", "street_chat_a", "street_chat_b" });
            IV_Animations[495] = new IVAnimSet("missmarnie", new string[] { "give_obj", "take_obj", "toss_money_chopper", "toss_money_dirt", "toss_money_freeway", "toss_money_scooter", "toss_money_spt", "wasted_seated" });
            IV_Animations[496] = new IVAnimSet("missmarnie2", new string[] { "give_obj", "lean_idle_a", "take_obj", "toss_money_chopper", "toss_money_dirt", "toss_money_freeway", "toss_money_scooter", "toss_money_spt" });
            IV_Animations[497] = new IVAnimSet("missmel", new string[] { "curious_ilde" });
            IV_Animations[498] = new IVAnimSet("missnet1", new string[] { "car_chat_outside", "car_chat_outside_2", "cower_in_car", "cower_in_car_1off", "enter_room", "idle2lean", "look_back_turn_run", "look_left", "look_left_in_car", "look_rear_view_idle", "look_right", "look_right_loop", "open_door", "open_doorv2", "plead_in_car", "plead_in_car_1off", "walk_up_to_car" });
            IV_Animations[499] = new IVAnimSet("missnet2", new string[] { "smoke_stand_b" });
            IV_Animations[500] = new IVAnimSet("missnet3", new string[] { "stairs_chat_a" });
            IV_Animations[501] = new IVAnimSet("missnet_4", new string[] { "climb_idle", "ladder_climb", "ladder_geton" });
            IV_Animations[502] = new IVAnimSet("misspackie1", new string[] { "bouncer_a_refuse_entry", "mngr_scare_getdown", "move_junk", "reload", "street_chat_a" });
            IV_Animations[503] = new IVAnimSet("misspackie2", new string[] { "box_catch", "box_obj", "box_throw", "crouch_partial", "crry_prtial", "crry_prtial_b", "give_obj", "jump_a", "jump_b", "liftup", "move_junk", "niko_incar_partial", "roof_chat2_a", "roof_chat2_b", "roof_chat_a", "roof_chat_b", "shut_van_doors", "smoke_stand_a", "smoke_stand_b", "take_obj" });
            IV_Animations[504] = new IVAnimSet("misspackie3", new string[] { "argue_a", "argue_b", "give_obj", "searchped_intro", "searchped_loop", "smoke_light_up", "smoke_stand_a", "smoke_stand_b", "smoke_stub_out", "take_obj" });
            IV_Animations[505] = new IVAnimSet("misspackie7", new string[] { "drag_from_car_f", "drag_from_car_m", "f_bound_walk", "show_brief_case" });
            IV_Animations[506] = new IVAnimSet("misspass", new string[] { "bye", "partial_bye_r", "partial_incar_bye", "partial_wave_a", "partial_wave_b", "partial_wave_c", "partial_wave_d", "partial_wave_e", "partial_wave_female", "wave", "wave_in_car" });
            IV_Animations[507] = new IVAnimSet("misspathos1", new string[] { "hh_lp_01" });
            IV_Animations[508] = new IVAnimSet("misspathos2", new string[] { "back_lower", "default_idle", "get_up_injured", "hh_lp_01", "idle", "inj_front_to_default", "startled", "walk" });
            IV_Animations[509] = new IVAnimSet("misspickup_brucie", new string[] { "idle01", "preen", "wave" });
            IV_Animations[510] = new IVAnimSet("misspickup_dwayne", new string[] { "idle01", "idle02", "wave" });
            IV_Animations[511] = new IVAnimSet("misspickup_jacob", new string[] { "idle_01", "smoking", "wall_2_walk", "wave" });
            IV_Animations[512] = new IVAnimSet("misspickup_packie", new string[] { "idle01", "idle02", "wave" });
            IV_Animations[513] = new IVAnimSet("misspickup_roman", new string[] { "lean01", "lean02", "lean_2_walk", "wave" });
            IV_Animations[514] = new IVAnimSet("missplayboy1", new string[] { "bouncer_a_stand", "bouncer_b_stand", "car_chat_a", "car_chat_b", "d_locked_ps", "hh_dwn_01", "hh_dwn_02", "hh_lft_01", "hh_lft_02", "hh_lp_01", "hh_lp_02", "hh_rgt_01", "hh_rgt_02", "hh_up_01", "hh_up_02", "pull_pint", "street_chat_a", "street_chat_b" });
            IV_Animations[515] = new IVAnimSet("missplayboy2", new string[] { "argue_b", "indicate_front", "indicate_right", "lean_balcony_loopc", "look_watch", "smoke_stand_b", "street_argue_b", "street_chat_a", "street_chat_b" });
            IV_Animations[516] = new IVAnimSet("missplayboy3", new string[] { "lean_balcony_loop_nowave", "look_intro", "look_intro_idle", "playboy_exit_lift", "pull_pin", "pull_pin_l", "smoke_stand_b" });
            IV_Animations[517] = new IVAnimSet("missplayboy4", new string[] { "player_execute", "plead", "victim" });
            IV_Animations[518] = new IVAnimSet("misspxdf", new string[] { "beggar_sit", "crowd_chant_d", "door_knock", "indicate_back", "indicate_listener", "nod_no", "nod_yes", "out_of_ammo", "player_execute", "player_execute_dwayne", "plead", "plead_dwayne", "plyr_shldropen", "pool_chalkcue", "shoulder_door_intro", "shoulder_door_loop", "victim", "victim_dwayne", "walk" });
            IV_Animations[519] = new IVAnimSet("missrandom_idle", new string[] { "agressive_idle", "cantstandstill_idle", "curious_ilde", "female_ilde", "female_ilde2", "female_ilde3", "friendly_idle", "impatient_idle", "sad_idle" });
            IV_Animations[520] = new IVAnimSet("missray1", new string[] { "back_off", "crouch_roll_l", "crouch_roll_r", "d_locked_ds", "getup_back_fast", "idle2door", "idle_armed_lookback", "locked_door_loop", "press_button", "reload", "scared_02", "scared_03", "stand_smoke", "stumble_fall", "trans_to_scared02" });
            IV_Animations[521] = new IVAnimSet("missray2", new string[] { "gbge_getoff", "gbge_getoff_l", "gbge_getoff_r", "gbge_geton", "gbge_geton_l", "gbge_geton_r", "gbge_hangl", "gbge_hangon", "gbge_hangr", "gbge_leanl", "gbge_leanr", "gbge_pickuprubbish", "gbge_scratchneck", "gbge_shoot", "gbge_shoot_l", "gbge_shoot_r", "gbge_smoke", "gbge_standrubbish", "gbge_stretchback", "gbge_throwrubbish", "gbge_walkwithrubbish", "gbge_warn" });
            IV_Animations[522] = new IVAnimSet("missray3", new string[] { "car_driver", "car_passenger_bl", "car_passenger_br", "car_passenger_f", "drink_coffee", "driver_intro", "driver_loop", "ex_niko_shoot", "ex_victim_intro", "ex_victim_loop", "ex_victim_shot", "fright_hndsup", "fright_loop", "handover", "hndsup_loop", "idle_2_fright", "injured_walk", "look_right_car_intro", "look_right_car_loop", "luca_line1", "luca_line2", "luca_line3", "niko_line1", "niko_line2", "niko_stuble_from_car", "niko_walk_to_car", "on_toilet_idle", "pass_ds", "pass_ps", "piss_loop", "player_execute", "plead", "romin_injured_walk", "take_package", "talk_coffee", "toilet_shock", "tuna_line1", "victim", "walk" });
            IV_Animations[523] = new IVAnimSet("missray4", new string[] { "cower_idle", "stvendor_pay" });
            IV_Animations[524] = new IVAnimSet("missray5", new string[] { "bikers_gbik_turnl", "bikers_idle", "bikers_jim_turnr", "drop_knees", "get_in_lift_bottom", "get_in_lift_top", "idle_fix_hair", "idle_hot_wipe_face", "idle_look_r", "idle_scratch_head", "operate_lift_intro", "operate_lift_loop", "operate_lift_outro", "operate_lift_switch", "plead_idle", "plyr_bedsit_out_lhs", "plyr_lie_2_sit_lhs", "plyr_sit_2_lie_lhs", "press_button", "smoke_stand_b", "street_chat_a", "street_chat_b", "tries_to_catch_bikers", "walk" });
            IV_Animations[525] = new IVAnimSet("missray6", new string[] { "drop_knees", "get_in_lift_bottom", "get_in_lift_top", "idle_fix_hair", "idle_hot_wipe_face", "idle_look_r", "idle_scratch_head", "operate_lift_intro", "operate_lift_outro", "operate_lift_switch", "player_execute", "plead", "plead_idle", "plyr_bedsit_out_lhs", "plyr_lie_2_sit_lhs", "plyr_sit_2_lie_lhs", "press_button", "smoke_stand_b", "victim", "walk" });
            IV_Animations[526] = new IVAnimSet("missroman10", new string[] { "agree", "carwash_a", "carwash_c", "cop_search", "explain_a", "idle", "indicate_front", "remote_unlock", "searched_pose", "sit_loop", "smoke_dirt_bike", "smoke_ds", "smoke_sports_bike", "street_argue_f_a" });
            IV_Animations[527] = new IVAnimSet("missroman11", new string[] { "look_right_car_loop", "niko_stop_flat_car", "niko_stop_flat_foot", "roman_stop_flat_car", "roman_stop_flat_foot" });
            IV_Animations[528] = new IVAnimSet("missroman12", new string[] { "execute_perp", "execute_rom", "hostage", "hostage_duck", "hostage_hide_loop", "hostage_intro", "hostage_let_go", "hostage_peek", "hostage_struggle", "hostage_taker", "hostage_taker_duck", "hostage_taker_fire", "hostage_taker_hide_loop", "hostage_taker_intro", "hostage_taker_peek", "hostage_taker_shot", "hostage_taker_struggle", "hostage_tied2chair", "hug_niko", "hug_roman", "indicate_left", "indicate_listener", "lean_balcony_loopc", "look_watch", "smoke_stand_b", "stand_look_at_watch", "street_argue_b", "street_chat_a", "street_chat_b" });
            IV_Animations[529] = new IVAnimSet("missroman13", new string[] { "absolutely", "direct_driver", "dont_hit_me", "handsup", "over_there", "point_forward", "point_left", "point_right", "roman_lean", "roman_walk_part", "rom_lean_to_walk", "rom_tap_shoulder_exit", "slow_down" });
            IV_Animations[530] = new IVAnimSet("missroman14", new string[] { "drop_knees", "player_execute", "plead", "plead_idle", "roman_kick_machine", "roman_wait_idle", "sneak_away", "victim" });
            IV_Animations[531] = new IVAnimSet("missroman2", new string[] { "180l", "conversation", "lookaround_d", "mirror_a", "niko_recieve_phone", "reach_open_door", "roman_give_phone", "sit_pass", "stretch_a" });
            IV_Animations[532] = new IVAnimSet("missroman3", new string[] { "mallory_chat", "mallory_horn", "michelle_chat", "michelle_horn", "michelle_wave", "niko_wave" });
            IV_Animations[533] = new IVAnimSet("missroman4", new string[] { "beat_up_a", "beat_up_b", "boss_idle_injured", "boss_pickup_injured", "damage_onsidel", "fight_idle", "fight_intro_01", "ground_attack", "hostage_beat_up", "idle", "injured_run", "injured_walk", "plyr_pickup_injured", "reaction_angry_a", "reaction_angry_c", "roman_killed", "scared_02", "taunt_03", "taunt_ground_05", "wasteda" });
            IV_Animations[534] = new IVAnimSet("missroman5", new string[] { "walk_destroy" });
            IV_Animations[535] = new IVAnimSet("missroman6", new string[] { "bike_give_gun", "bike_take_gun", "car_give_gun_back", "car_give_gun_l", "car_give_gun_side", "car_take_gun_back", "car_take_gun_back_l", "car_take_gun_side", "idle_01", "partial_bye_r", "partial_wave_a", "wall_2_walk", "wall_lean", "wave_in_car" });
            IV_Animations[536] = new IVAnimSet("missroman7", new string[] { "player_execute", "plead", "scared_in_car", "victim" });
            IV_Animations[537] = new IVAnimSet("missroman8", new string[] { "partial_wave_d" });
            IV_Animations[538] = new IVAnimSet("misssara2", new string[] { "female_ilde", "give_obj", "pace_around", "take_obj" });
            IV_Animations[539] = new IVAnimSet("misssara_1", new string[] { "argue_b", "female_ilde", "female_ilde2", "mcivi_flagtaxi_in", "stash_lookaround" });
            IV_Animations[540] = new IVAnimSet("missstripclub", new string[] { "clean_glass", "dance_ragga_2", "dance_rick", "dance_wisper_2", "niko_dance", "pole_dance_a", "rejection", "sitting_proposition", "standing_proposition", "watch_lap_dance_loop", "wipe_counter" });
            IV_Animations[541] = new IVAnimSet("missstripclubhi", new string[] { "idle_a", "idle_loop", "lap_dance_loop", "lap_honkerz_a2_niko", "lap_honkerz_a2_woman", "lap_honkerz_b1_niko", "lap_honkerz_b1_woman", "lap_triangle_a3_niko", "lap_triangle_a3_woman", "lap_triangle_b3_niko", "lap_triangle_b3_woman", "tri_c6_3way_niko", "tri_c6_3way_woman1", "tri_c6_3way_woman2" });
            IV_Animations[542] = new IVAnimSet("missstripclublo", new string[] { "clean_glass", "pole_dance_a", "wipe_counter" });
            IV_Animations[543] = new IVAnimSet("misssuzanne_a", new string[] { "argue_a", "what_c" });
            IV_Animations[544] = new IVAnimSet("missswat_assault", new string[] { "car_chat_b", "crazy_rant_01", "crazy_rant_02", "crazy_rant_03", "crchsignal_attention", "crchsignal_gofwd", "crchsignal_roger", "crchsignal_stop", "i_cant", "plane_exit", "threaten", "what" });
            IV_Animations[545] = new IVAnimSet("misstaxidepot", new string[] { "car_chat", "car_chat_outside", "copm_searchboot", "fixcar", "getofftrolley", "getontrolley", "slideout", "slideundercar", "workunderbonnet" });
            IV_Animations[546] = new IVAnimSet("missttkill", new string[] { "abuse", "agree_a", "piss_loop", "piss_shake_off" });
            IV_Animations[547] = new IVAnimSet("missvlad1", new string[] { "angry01", "door_shake", "fold_arms_oh_yeah", "give_obj", "lockandunlock_door", "oldman_upset", "take_obj", "turn180_lookatbrick", "turn_to_face_niko", "winclean_default" });
            IV_Animations[548] = new IVAnimSet("missvlad2", new string[] { "hit_basket", "laundry_loop", "sit_van", "throw_basket" });
            IV_Animations[549] = new IVAnimSet("missvlad3", new string[] { "carwash_a", "smoke_stand_a" });
            IV_Animations[550] = new IVAnimSet("missvlad4", new string[] { "crates", "drill_stand", "emptygun", "ev_dive", "exhausted_intro", "exhausted_loop", "exhausted_outro", "hang_on_ledge", "ledge_climb", "ledge_helped_up", "ledge_help_up", "ledge_stamp", "look_about", "look_down_intro", "look_down_loop", "look_down_outro", "run_fall_off_roof", "scratch_neck", "stand_idle_a", "stomp_idle", "street_chat_a", "street_chat_b" });
            IV_Animations[551] = new IVAnimSet("missvlad5", new string[] { "argue_a", "argue_b", "smoke_light_up", "smoke_stand_a" });
            IV_Animations[552] = new IVAnimSet("misswedding", new string[] { "tie_adjust_stand" });
            IV_Animations[553] = new IVAnimSet("move_combat_strafe", new string[] { "idle", "run", "run_strafe_b", "run_strafe_bl45", "run_strafe_br45", "run_strafe_l", "run_strafe_l45", "run_strafe_r", "run_strafe_r45", "shuffle_stop", "turn_360_l", "turn_360_r", "walk", "walk_strafe_b", "walk_strafe_bl45", "walk_strafe_br45", "walk_strafe_l", "walk_strafe_l45", "walk_strafe_r", "walk_strafe_r45", "wstart" });
            IV_Animations[554] = new IVAnimSet("move_combat_strafe_c", new string[] { "idle", "run", "run_strafe_b", "run_strafe_bl45", "run_strafe_br45", "run_strafe_l", "run_strafe_l45", "run_strafe_r", "run_strafe_r45", "shuffle_stop", "turn_360_l", "turn_360_r", "walk", "walk_strafe_b", "walk_strafe_bl45", "walk_strafe_br45", "walk_strafe_l", "walk_strafe_l45", "walk_strafe_r", "walk_strafe_r45", "wstart" });
            IV_Animations[555] = new IVAnimSet("move_cop", new string[] { "idle", "run", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[556] = new IVAnimSet("move_cop_fat", new string[] { "idle" });
            IV_Animations[557] = new IVAnimSet("move_cop_search", new string[] { "idle", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[558] = new IVAnimSet("move_crouch", new string[] { "crouchidle2idle", "idle", "idle2crouchidle", "rstop_l", "rstop_r", "run", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "turn_360_l", "turn_360_r", "walk", "walk_turn_l", "walk_turn_r", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[559] = new IVAnimSet("move_crouch_hgun", new string[] { "fire", "idle" });
            IV_Animations[560] = new IVAnimSet("move_crouch_rifle", new string[] { "crouchidle2idle", "crouch_roll_bwd", "crouch_roll_fwd", "crouch_roll_l", "crouch_roll_r", "idle", "idle2crouchidle", "rstop_l", "rstop_r", "run", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "turn_360_l", "turn_360_r", "walk", "walk_turn_l", "walk_turn_r", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[561] = new IVAnimSet("move_crouch_rpg", new string[] { "crouchidle2idle", "idle", "idle2crouchidle", "rstop_l", "rstop_r", "run", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "turn_360_l", "turn_360_r", "walk", "walk_turn_l", "walk_turn_r", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[562] = new IVAnimSet("move_f@armed", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "runstart_l_180", "runstart_l_90", "runstart_r_180", "runstart_r_90", "run_turn_180", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "sprint", "sprint_turn_l", "sprint_turn_r", "sstop_l", "sstop_r", "turn_360_l", "turn_360_r", "walk", "walk_turn_l", "walk_turn_l2", "walk_turn_l3", "walk_turn_r", "walk_turn_r2", "walk_turn_r3", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[563] = new IVAnimSet("move_f@bness_a", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[564] = new IVAnimSet("move_f@bness_b", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[565] = new IVAnimSet("move_f@bness_c", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[566] = new IVAnimSet("move_f@bness_d", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[567] = new IVAnimSet("move_f@bness_e", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[568] = new IVAnimSet("move_f@casual", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "walk", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[569] = new IVAnimSet("move_f@casual_b", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[570] = new IVAnimSet("move_f@casual_c", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[571] = new IVAnimSet("move_f@cower", new string[] { "idle", "rstop_l", "rstop_r", "run", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[572] = new IVAnimSet("move_f@fat", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "run_panic_a", "run_panic_b", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "sprint", "sprint_turn_l", "sprint_turn_r", "sstop_l", "sstop_r", "turn_360_l", "turn_360_r", "walk", "walk_turn_l", "walk_turn_l2", "walk_turn_l3", "walk_turn_r", "walk_turn_r2", "walk_turn_r3", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[573] = new IVAnimSet("move_f@generic", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "runstart_l_180", "runstart_l_90", "runstart_r_180", "runstart_r_90", "run_down", "run_strafe_b", "run_strafe_l", "run_strafe_r", "run_turn_180", "run_turn_180_l", "run_turn_180_r", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "run_up", "shuffle_stop", "sprint", "sprint_turn_180_l", "sprint_turn_180_r", "sprint_turn_l", "sprint_turn_r", "sstop_l", "sstop_r", "turn_360_l", "turn_360_r", "walk", "walk_down", "walk_strafe_b", "walk_strafe_bl45", "walk_strafe_br45", "walk_strafe_l", "walk_strafe_l45", "walk_strafe_r", "walk_strafe_r45", "walk_turn_180_l", "walk_turn_180_r", "walk_turn_l", "walk_turn_l2", "walk_turn_l3", "walk_turn_r", "walk_turn_r2", "walk_turn_r3", "walk_up", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[574] = new IVAnimSet("move_f@michelle", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "run_strafe_b", "run_strafe_l", "run_strafe_r", "turn_360_l", "turn_360_r", "walk", "walk_strafe_b", "walk_strafe_bl45", "walk_strafe_br45", "walk_strafe_l", "walk_strafe_l45", "walk_strafe_r", "walk_strafe_r45", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[575] = new IVAnimSet("move_f@multiplyr", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "run_strafe_b", "run_strafe_l", "run_strafe_r", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "sprint", "sprint_turn_l", "sprint_turn_r", "turn_360_l", "turn_360_r", "walk", "walk_strafe_b", "walk_strafe_bl45", "walk_strafe_br45", "walk_strafe_l", "walk_strafe_l45", "walk_strafe_r", "walk_strafe_r45", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[576] = new IVAnimSet("move_f@m_p", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "runstart_l_180", "runstart_l_90", "runstart_r_180", "runstart_r_90", "run_strafe_b", "run_strafe_l", "run_strafe_r", "run_turn_180", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "sprint", "sprint_turn_l", "sprint_turn_r", "sstop_l", "sstop_r", "turn_360_l", "turn_360_r", "walk", "walk_strafe_b", "walk_strafe_bl45", "walk_strafe_br45", "walk_strafe_l", "walk_strafe_l45", "walk_strafe_r", "walk_strafe_r45", "walk_turn_l", "walk_turn_l2", "walk_turn_l3", "walk_turn_r", "walk_turn_r2", "walk_turn_r3", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[577] = new IVAnimSet("move_f@old_a", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[578] = new IVAnimSet("move_f@old_b", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[579] = new IVAnimSet("move_f@old_c", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[580] = new IVAnimSet("move_f@old_d", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[581] = new IVAnimSet("move_f@puffer", new string[] { "idle", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[582] = new IVAnimSet("move_f@sexy", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "turn_360_l", "turn_360_r", "walk", "walk_turn_l", "walk_turn_l2", "walk_turn_l3", "walk_turn_r", "walk_turn_r2", "walk_turn_r3", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[583] = new IVAnimSet("move_gng@afro_a", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[584] = new IVAnimSet("move_gng@afro_b", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[585] = new IVAnimSet("move_gng@afro_c", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[586] = new IVAnimSet("move_gng@generic_a", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[587] = new IVAnimSet("move_gng@generic_b", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[588] = new IVAnimSet("move_gng@generic_c", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[589] = new IVAnimSet("move_gng@jam_a", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[590] = new IVAnimSet("move_gng@jam_b", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[591] = new IVAnimSet("move_gng@jam_c", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[592] = new IVAnimSet("move_gng@latino_a", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[593] = new IVAnimSet("move_gng@latino_b", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[594] = new IVAnimSet("move_gng@latino_c", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[595] = new IVAnimSet("move_injured_generic", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "turn_360_l", "turn_360_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[596] = new IVAnimSet("move_injured_ground", new string[] { "back_outro" });
            IV_Animations[597] = new IVAnimSet("move_injured_lower", new string[] { "idle", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[598] = new IVAnimSet("move_injured_upper", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "turn_360_l", "turn_360_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[599] = new IVAnimSet("move_m@bernie", new string[] { "run", "walk" });
            IV_Animations[600] = new IVAnimSet("move_m@bness_a", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[601] = new IVAnimSet("move_m@bness_b", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[602] = new IVAnimSet("move_m@bness_c", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[603] = new IVAnimSet("move_m@bum", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "turn_360_l", "turn_360_r", "walk", "walk_turn_l", "walk_turn_l2", "walk_turn_l3", "walk_turn_r", "walk_turn_r2", "walk_turn_r3", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[604] = new IVAnimSet("move_m@case", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[605] = new IVAnimSet("move_m@casual", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[606] = new IVAnimSet("move_m@casual_b", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[607] = new IVAnimSet("move_m@casual_c", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[608] = new IVAnimSet("move_m@cower", new string[] { "idle", "rstop_l", "rstop_r", "run", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[609] = new IVAnimSet("move_m@cs_swat", new string[] { "run" });
            IV_Animations[610] = new IVAnimSet("move_m@eddie", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[611] = new IVAnimSet("move_m@fat", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "run_strafe_b", "run_strafe_bl45", "run_strafe_br45", "run_strafe_l", "run_strafe_l45", "run_strafe_r", "run_strafe_r45", "run_turn_180", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "sprint", "sprint_turn_l", "sprint_turn_r", "sstop_l", "sstop_r", "turn_360_l", "turn_360_r", "walk", "walk_strafe_b", "walk_strafe_bl45", "walk_strafe_br45", "walk_strafe_l", "walk_strafe_l45", "walk_strafe_r", "walk_strafe_r45", "walk_turn_l", "walk_turn_l2", "walk_turn_l3", "walk_turn_r", "walk_turn_r2", "walk_turn_r3", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[612] = new IVAnimSet("move_m@generic", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "runstart_l_180", "runstart_l_90", "runstart_r_180", "runstart_r_90", "run_down", "run_strafe_b", "run_strafe_l", "run_strafe_r", "run_turn_180", "run_turn_180_l", "run_turn_180_r", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "run_up", "shuffle_stop", "sprint", "sprint_turn_180_l", "sprint_turn_180_r", "sprint_turn_l", "sprint_turn_r", "sstop_l", "sstop_r", "turn_360_l", "turn_360_r", "walk", "walk_down", "walk_strafe_b", "walk_strafe_l", "walk_strafe_r", "walk_turn_l", "walk_turn_l2", "walk_turn_l3", "walk_turn_r", "walk_turn_r2", "walk_turn_r3", "walk_up", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[613] = new IVAnimSet("move_m@h_cuffed", new string[] { "idle", "run", "sprint", "walk" });
            IV_Animations[614] = new IVAnimSet("move_m@multiplyr", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "run_strafe_b", "run_strafe_l", "run_strafe_r", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "sprint", "sprint_turn_l", "sprint_turn_r", "turn_360_l", "turn_360_r", "walk", "walk_strafe_b", "walk_strafe_l", "walk_strafe_r", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[615] = new IVAnimSet("move_m@old_a", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[616] = new IVAnimSet("move_m@old_b", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[617] = new IVAnimSet("move_m@old_c", new string[] { "idle", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[618] = new IVAnimSet("move_m@playboy", new string[] { "idle", "run", "walk" });
            IV_Animations[619] = new IVAnimSet("move_m@roman", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "sprint", "sstop_l", "sstop_r", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[620] = new IVAnimSet("move_m@roman_inj", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "walk", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[621] = new IVAnimSet("move_m@swat", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "run_strafe_b", "run_strafe_l", "run_strafe_r", "walk", "walk_strafe_b", "walk_strafe_l", "walk_strafe_r", "walk_turn_l", "walk_turn_r", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[622] = new IVAnimSet("move_m@tourist", new string[] { "idle" });
            IV_Animations[623] = new IVAnimSet("move_melee", new string[] { "idle", "idle_outro", "run", "run_strafe_b", "run_strafe_l", "run_strafe_r", "walk", "walk_strafe_b", "walk_strafe_l", "walk_strafe_r" });
            IV_Animations[624] = new IVAnimSet("move_melee_baseball", new string[] { "idle", "idle_outro", "run", "run_strafe_b", "run_strafe_l", "run_strafe_r", "walk", "walk_strafe_b", "walk_strafe_l", "walk_strafe_r" });
            IV_Animations[625] = new IVAnimSet("move_melee_knife", new string[] { "idle", "idle_outro", "run", "run_strafe_b", "run_strafe_l", "run_strafe_r", "walk", "walk_strafe_b", "walk_strafe_l", "walk_strafe_r" });
            IV_Animations[626] = new IVAnimSet("move_player", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "runstart_l_180", "runstart_l_90", "runstart_r_180", "runstart_r_90", "run_down", "run_strafe_b", "run_strafe_l", "run_strafe_r", "run_turn_180", "run_turn_180_l", "run_turn_180_r", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "run_up", "sprint", "sprint_turn_180_l", "sprint_turn_180_r", "sprint_turn_l", "sprint_turn_r", "sstop_l", "sstop_r", "turn_360_l", "turn_360_r", "walk", "walk_b", "walk_c", "walk_down", "walk_strafe_b", "walk_strafe_l", "walk_strafe_r", "walk_turn_180_l", "walk_turn_180_r", "walk_turn_l", "walk_turn_l2", "walk_turn_l3", "walk_turn_r", "walk_turn_r2", "walk_turn_r3", "walk_up", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[627] = new IVAnimSet("move_rifle", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "runstart_l_180", "runstart_l_90", "runstart_r_180", "runstart_r_90", "run_down", "run_turn_180", "run_turn_180_l", "run_turn_180_r", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "run_up", "shuffle_stop", "sprint", "sprint_turn_180_l", "sprint_turn_180_r", "sprint_turn_l", "sprint_turn_r", "sstop_l", "sstop_r", "turn_360_l", "turn_360_r", "walk", "walk_down", "walk_turn_180_l", "walk_turn_180_r", "walk_turn_l", "walk_turn_l2", "walk_turn_l3", "walk_turn_r", "walk_turn_r2", "walk_turn_r3", "walk_up", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[628] = new IVAnimSet("move_roman_inj", new string[] { "run", "walk" });
            IV_Animations[629] = new IVAnimSet("move_rpg", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "runstart_l_180", "runstart_l_90", "runstart_r_180", "runstart_r_90", "run_turn_180_l", "run_turn_180_r", "run_turn_l", "run_turn_l2", "run_turn_r", "run_turn_r2", "sprint", "sprint_turn_180_l", "sprint_turn_180_r", "sprint_turn_l", "sprint_turn_r", "sstop_l", "sstop_r", "turn_360_l", "turn_360_r", "walk", "walk_b", "walk_c", "walk_turn_180_l", "walk_turn_180_r", "walk_turn_l", "walk_turn_l2", "walk_turn_l3", "walk_turn_r", "walk_turn_r2", "walk_turn_r3", "wstart", "wstart_turn_l180", "wstart_turn_l90", "wstart_turn_r180", "wstart_turn_r90", "wstop_l", "wstop_r" });
            IV_Animations[630] = new IVAnimSet("ped", new string[] { "cellphone_in", "cellphone_text", "duck_cower", "fuck_u", "hail_taxi", "handsup", "helmet_off", "hit_wall", "idle_tired", "mcivi_flagtaxi_in", "nm_melee", "open_door", "open_door_alt", "open_door_r", "open_door_r_alt", "open_door_shove", "plead", "run_open_door", "run_open_door_alt", "run_open_door_r", "run_open_door_r_alt", "run_open_door_shove", "searchped_intro", "searchped_loop", "swim_idle" });
            IV_Animations[631] = new IVAnimSet("pickup_object", new string[] { "pickup_high", "pickup_low", "pickup_med", "putdown_high", "putdown_low", "putdown_med" });
            IV_Animations[632] = new IVAnimSet("playidles_bat", new string[] { "checkleg_r", "over_shoulder", "shift_weight" });
            IV_Animations[633] = new IVAnimSet("playidles_cold", new string[] { "bang_ear", "hold_out_hand", "play_collar", "rub_head", "shake_rain_a", "stamp_feet", "stand_lookatsky", "wipe_face" });
            IV_Animations[634] = new IVAnimSet("playidles_f_rifle", new string[] { "idle_a", "idle_b", "idle_c" });
            IV_Animations[635] = new IVAnimSet("playidles_f_std", new string[] { "idle_a", "idle_b", "idle_c" });
            IV_Animations[636] = new IVAnimSet("playidles_hgun", new string[] { "idle_armed_throwgun", "idle_object_throwgun" });
            IV_Animations[637] = new IVAnimSet("playidles_injured", new string[] { "hands_on_hips", "heavy_breathing", "out_of_breath", "wipe_face", "wipe_forehead" });
            IV_Animations[638] = new IVAnimSet("playidles_injured_r", new string[] { "heavy_breathing", "out_of_breath" });
            IV_Animations[639] = new IVAnimSet("playidles_rifle", new string[] { "checkgun", "idle_armed", "idle_armed_lookback", "lookleft", "shakelegs", "shoulder", "stretch", "weapondown" });
            IV_Animations[640] = new IVAnimSet("playidles_std", new string[] { "gun_aim", "gun_cock_weapon", "gun_scratch", "idle_checkarm_r", "idle_checkleg_l", "idle_checkleg_r", "idle_crackknuckles", "idle_fix_hair", "idle_hot_wipe_face", "idle_long_lookaround", "idle_look_at_sky", "idle_look_back", "idle_look_l", "idle_look_l_urgent", "idle_look_r", "idle_scratch_balls", "idle_scratch_head", "idle_shakelegs", "idle_tired" });
            IV_Animations[641] = new IVAnimSet("playidles_tired_1h", new string[] { "heavy_breathing" });
            IV_Animations[642] = new IVAnimSet("playidles_tired_2h", new string[] { "heavy_breathing" });
            IV_Animations[643] = new IVAnimSet("playidles_tired_rpg", new string[] { "heavy_breathing" });
            IV_Animations[644] = new IVAnimSet("playidles_wet_1h", new string[] { "brush_off_stand", "stamp_feet" });
            IV_Animations[645] = new IVAnimSet("playidles_wet_2h", new string[] { "stamp_feet" });
            IV_Animations[646] = new IVAnimSet("plead", new string[] { "plead" });
            IV_Animations[647] = new IVAnimSet("ragdoll_trans", new string[] { "inj_front_to_default", "inj_lside_to_default", "inj_rside_to_default", "preacher_trans", "recover_abdomen", "recover_balance", "seated_trans" });
            IV_Animations[648] = new IVAnimSet("ragdoll_trans_back", new string[] { "inj_back_to_default" });
            IV_Animations[649] = new IVAnimSet("reaction@male_flee", new string[] { "back", "back_upperbody", "front", "front_upperbody", "left", "left_upperbody", "pistol_shot_near", "rifle_shot_near", "right", "right_upperbody" });
            IV_Animations[650] = new IVAnimSet("reaction_f_seat_flee", new string[] { "f_seat_flee_l", "f_seat_flee_r" });
            IV_Animations[651] = new IVAnimSet("reaction_m_seat_flee", new string[] { "m_seat_cower_flee_l", "m_seat_cower_flee_r", "m_seat_flee_l", "m_seat_flee_r" });
            IV_Animations[652] = new IVAnimSet("searchped", new string[] { "searchped_intro", "searchped_loop" });
            IV_Animations[653] = new IVAnimSet("sit", new string[] { "plyr_bedlie_loop_lhs", "plyr_bedlie_loop_rhs", "plyr_bedsit_in_lhs", "plyr_bedsit_in_rhs", "plyr_bedsit_loop_lhs", "plyr_bedsit_loop_rhs", "plyr_bedsit_out_lhs", "plyr_bedsit_out_rhs", "plyr_lie_2_sit_lhs", "plyr_lie_2_sit_rhs", "plyr_sit_2_lie_lhs", "plyr_sit_2_lie_rhs", "plyr_sit_down", "plyr_sit_down_180", "plyr_sit_loop", "plyr_sit_loop_180", "plyr_sit_up", "plyr_sit_up_180", "sit_down", "sit_down_180", "sit_loop", "sit_loop_180", "sit_up", "sit_up_180", "vidgame_in", "vidgame_loop", "vidgame_out", "vidgame_playa" });
            IV_Animations[654] = new IVAnimSet("swat", new string[] { "signal_advance", "signal_assault", "signal_attention", "signal_goback", "signal_gofwd", "signal_goleft", "signal_goright", "signal_look", "signal_negative", "signal_roger", "signal_sayagain", "signal_stop" });
            IV_Animations[655] = new IVAnimSet("swimming", new string[] { "idle", "rstop_l", "rstop_r", "run", "runstart_fwd", "run_turn_l", "run_turn_r", "sprint", "sprint_turn_l", "sprint_turn_r", "walk", "walk_strafe_b", "walk_strafe_l", "walk_strafe_r", "walk_turn_l", "walk_turn_r", "wstart", "wstop_l", "wstop_r" });
            IV_Animations[656] = new IVAnimSet("taxi_hail", new string[] { "aknowledge_l", "aknowledge_r", "forget_it", "fuck_u", "hail_taxi" });
            IV_Animations[657] = new IVAnimSet("throw_grenade", new string[] { "aim_partial", "aim_partial_trigger", "drop_partial", "grenade_throw_crouch", "grenade_throw_drop", "grenade_throw_overarm", "grenade_throw_short", "grenade_throw_underarm", "grenade_throw_underarm_stand", "holster", "holster_crouch", "light_molotov", "unholster", "unholster_crouch" });
            IV_Animations[658] = new IVAnimSet("veh@base", new string[] { "cop_std_arrest_in", "crash_exit_on_roof_l", "crash_exit_on_roof_r", "crash_exit_on_side_l", "crash_exit_on_side_r", "in_car_dam" });
            IV_Animations[659] = new IVAnimSet("veh@bike_chopper", new string[] { "align_ds", "align_ps", "die_ds", "die_horn_ds", "die_ps", "get_in_bike_jack_ds", "get_in_bike_jack_ps", "get_in_ds", "get_in_ps", "get_out_ds", "get_out_ps", "helmet_on", "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps", "jump", "jump_out_ds", "jump_out_ps", "keystart", "lean_left_ps", "lean_right_ps", "pass_helmet_on", "pickup_lhs", "pickup_rhs", "pullup_lhs", "pullup_rhs", "reverse", "shunt_ds", "shunt_ps", "sit_drive", "sit_pass", "steer_bwd", "steer_fwd", "steer_l", "steer_r", "still", "throw_grenade" });
            IV_Animations[660] = new IVAnimSet("veh@bike_dirt", new string[] { "align_ds", "align_ps", "die_ds", "die_horn_ds", "die_ps", "get_in_bike_jack_ds", "get_in_bike_jack_ps", "get_in_ds", "get_in_ps", "get_out_ds", "get_out_ps", "helmet_on", "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps", "jump", "jump_out_ds", "jump_out_ps", "keystart", "lean_left_ps", "lean_right_ps", "pass_helmet_on", "pickup_lhs", "pickup_rhs", "pullup_lhs", "pullup_rhs", "reverse", "shunt_ds", "shunt_ps", "sit_drive", "sit_pass", "steer_bwd", "steer_fwd", "steer_l", "steer_r", "still", "throw_grenade" });
            IV_Animations[661] = new IVAnimSet("veh@bike_freeway", new string[] { "align_ds", "align_ps", "die_ds", "die_horn_ds", "die_ps", "get_in_bike_jack_ds", "get_in_bike_jack_ps", "get_in_ds", "get_in_ps", "get_out_ds", "get_out_ps", "helmet_on", "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps", "jump", "jump_out_ds", "jump_out_ps", "keystart", "lean_left_ps", "lean_right_ps", "pass_helmet_on", "pickup_lhs", "pickup_rhs", "pullup_lhs", "pullup_rhs", "reverse", "shunt_ds", "shunt_ps", "sit_drive", "sit_pass", "steer_bwd", "steer_fwd", "steer_l", "steer_r", "still", "throw_grenade" });
            IV_Animations[662] = new IVAnimSet("veh@bike_scooter", new string[] { "align_ds", "align_ps", "die_ds", "die_horn_ds", "die_ps", "get_in_bike_jack_ds", "get_in_bike_jack_ps", "get_in_ds", "get_in_ps", "get_out_ds", "get_out_ps", "helmet_on", "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps", "jump", "jump_out_ds", "jump_out_ps", "keystart", "lean_left_ps", "lean_right_ps", "pass_helmet_on", "pickup_lhs", "pickup_rhs", "pullup_lhs", "pullup_rhs", "reverse", "shunt_ds", "shunt_ps", "sit_drive", "sit_pass", "steer_bwd", "steer_fwd", "steer_l", "steer_r", "still", "throw_grenade" });
            IV_Animations[663] = new IVAnimSet("veh@bike_spt", new string[] { "align_ds", "align_ps", "die_ds", "die_horn_ds", "die_ps", "get_in_bike_jack_ds", "get_in_bike_jack_ps", "get_in_ds", "get_in_ps", "get_out_ds", "get_out_ps", "helmet_off", "helmet_on", "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps", "jump", "jump_out_ds", "jump_out_ps", "keystart", "lean_left_ps", "lean_right_ps", "pass_helmet_on", "pickup_lhs", "pickup_rhs", "pullup_lhs", "pullup_rhs", "reverse", "shunt_ds", "shunt_ps", "sit_drive", "sit_pass", "steer_bwd", "steer_fwd", "steer_l", "steer_r", "still", "throw_grenade" });
            IV_Animations[664] = new IVAnimSet("veh@boat_speed", new string[] { "align_ds", "align_ps", "get_in_ds", "get_in_ps", "get_out_ds", "get_out_ps", "jump" });
            IV_Animations[665] = new IVAnimSet("veh@boat_standing", new string[] { "align_ds", "align_ps", "get_in_ds", "get_in_ps", "get_out_ds", "get_out_ps", "keystart", "lean_left_ds", "lean_left_ps", "lean_right_ds", "lean_right_ps", "reverse", "shunt_ds", "shunt_ps", "sit_drive", "sit_pass", "sit_pass_back_left", "sit_pass_back_right", "steer_l", "steer_r" });
            IV_Animations[666] = new IVAnimSet("veh@boat_stand_big", new string[] { "align_ds", "align_ps", "get_in_ds", "get_in_ps", "get_out_ds", "get_out_ps", "keystart", "reverse", "shunt_ds", "sit_drive", "steer_l", "steer_r" });
            IV_Animations[667] = new IVAnimSet("veh@bus", new string[] { "align_ds", "align_ps", "align_rear_ds", "align_rear_ps", "change_station", "die_ds", "die_horn_ds", "die_ps", "d_close_in_ds", "d_open_out_ds", "d_open_out_ps", "d_open_out_rear_ds", "d_open_out_rear_ps", "get_in_ds", "get_in_ps", "get_in_rear_ds", "get_in_rear_ps", "get_out_ds", "get_out_ps", "get_out_rear_ds", "get_out_rear_ps", "heavy_brake_ds", "horn", "hotwire", "keystart", "lean_left_ds", "lean_left_ps", "lean_right_ds", "lean_right_ps", "reverse", "shock_back", "shock_front", "shock_left", "shock_right", "shunt_ds", "shunt_ps", "sit_drive", "sit_pass", "sit_pass_back_left", "sit_pass_back_right", "smash_windscreen_ds", "steer_l", "steer_r", "through_windscreen_ds" });
            IV_Animations[668] = new IVAnimSet("veh@busted_low", new string[] { "jack_driver_ds", "jack_perp_ds" });
            IV_Animations[669] = new IVAnimSet("veh@busted_std", new string[] { "jack_driver_ds", "jack_perp_ds" });
            IV_Animations[670] = new IVAnimSet("veh@busted_truck", new string[] { "jack_driver_ds", "jack_perp_ds" });
            IV_Animations[671] = new IVAnimSet("veh@busted_van", new string[] { "jack_driver_ds", "jack_perp_ds" });
            IV_Animations[672] = new IVAnimSet("veh@cablecar", new string[] { "align_ds", "align_ps", "d_open_out_ds", "d_open_out_ps", "get_in_ds", "get_in_ps", "get_out_ds", "get_out_ps", "sit_drive", "sit_pass" });
            IV_Animations[673] = new IVAnimSet("veh@drivebyairtug", new string[] { "ds_aim_in", "ds_aim_loop", "ds_aim_out", "ps_aim_in", "ps_aim_loop", "ps_aim_out", "ps_throw_grenade", "throw_grenade" });
            IV_Animations[674] = new IVAnimSet("veh@drivebybike_chop", new string[] { "ds_aim_in", "ds_aim_loop", "ds_aim_out", "ps_aim_in", "ps_aim_loop", "ps_aim_out", "ps_throw_grenade", "throw_grenade" });
            IV_Animations[675] = new IVAnimSet("veh@drivebybike_dirt", new string[] { "ds_aim_in", "ds_aim_loop", "ds_aim_out", "ps_aim_in", "ps_aim_loop", "ps_aim_out", "ps_throw_grenade", "throw_grenade" });
            IV_Animations[676] = new IVAnimSet("veh@drivebybike_free", new string[] { "ds_aim_in", "ds_aim_loop", "ds_aim_out", "ps_aim_in", "ps_aim_loop", "ps_aim_out", "ps_throw_grenade", "throw_grenade" });
            IV_Animations[677] = new IVAnimSet("veh@drivebybike_scot", new string[] { "ds_aim_in", "ds_aim_loop", "ds_aim_out", "ps_aim_in", "ps_aim_loop", "ps_aim_out", "ps_throw_grenade", "throw_grenade" });
            IV_Animations[678] = new IVAnimSet("veh@drivebybike_spt", new string[] { "ds_aim_in", "ds_aim_loop", "ds_aim_out", "ps_aim_in", "ps_aim_loop", "ps_aim_out", "ps_throw_grenade", "throw_grenade" });
            IV_Animations[679] = new IVAnimSet("veh@drivebyboat_big", new string[] { "ds_aim_in", "ds_aim_loop", "ds_aim_out", "throw_grenade" });
            IV_Animations[680] = new IVAnimSet("veh@drivebyboat_spee", new string[] { "bl_throw_grenade", "br_throw_grenade", "ds_aim_in", "ds_aim_loop", "ds_aim_out", "ps_aim_in", "ps_aim_in_1h", "ps_aim_loop", "ps_aim_loop_1h", "ps_aim_out", "ps_aim_out_1h", "ps_throw_grenade", "throw_grenade" });
            IV_Animations[681] = new IVAnimSet("veh@drivebyboat_stnd", new string[] { "bl_aim_in", "bl_aim_in_1h", "bl_aim_loop", "bl_aim_loop_1h", "bl_aim_out", "bl_aim_out_1h", "bl_throw_grenade", "br_aim_in", "br_aim_in_1h", "br_aim_loop", "br_aim_loop_1h", "br_aim_out", "br_aim_out_1h", "br_throw_grenade", "ds_aim_in", "ds_aim_loop", "ds_aim_out", "ps_aim_in_1h", "ps_aim_loop_1h", "ps_aim_out_1h", "ps_throw_grenade", "throw_grenade" });
            IV_Animations[682] = new IVAnimSet("veh@drivebycop_std", new string[] { "ps_aim_in", "ps_aim_loop", "ps_aim_out" });
            IV_Animations[683] = new IVAnimSet("veh@drivebyheli", new string[] { "bl_aim_in", "bl_aim_in_1h", "bl_aim_loop", "bl_aim_loop_1h", "bl_aim_out", "bl_aim_out_1h", "bl_throw_grenade", "br_aim_in", "br_aim_in_1h", "br_aim_loop", "br_aim_loop_1h", "br_aim_out", "br_aim_out_1h", "br_throw_grenade", "ps_aim_in", "ps_aim_in_1h", "ps_aim_loop", "ps_aim_loop_1h", "ps_aim_out", "ps_aim_out_1h", "ps_throw_grenade" });
            IV_Animations[684] = new IVAnimSet("veh@drivebylow", new string[] { "ds_aim_in", "ds_aim_loop", "ds_aim_out", "ps_aim_in", "ps_aim_loop", "ps_aim_out", "ps_throw_grenade", "throw_grenade" });
            IV_Animations[685] = new IVAnimSet("veh@drivebylow_conv", new string[] { "ps_aim_in", "ps_aim_loop", "ps_aim_out", "ps_throw_grenade", "throw_grenade" });
            IV_Animations[686] = new IVAnimSet("veh@drivebystd", new string[] { "bl_aim_in", "bl_aim_loop", "bl_aim_out", "bl_throw_grenade", "br_aim_in", "br_aim_loop", "br_aim_out", "br_throw_grenade", "ds_aim_in", "ds_aim_loop", "ds_aim_out", "ps_aim_in", "ps_aim_loop", "ps_aim_out", "ps_throw_grenade", "throw_grenade" });
            IV_Animations[687] = new IVAnimSet("veh@drivebytruck", new string[] { "bl_aim_in", "bl_aim_loop", "bl_aim_out", "bl_throw_grenade", "br_aim_in", "br_aim_loop", "br_aim_out", "br_throw_grenade", "ds_aim_in", "ds_aim_loop", "ds_aim_out", "ps_aim_in", "ps_aim_loop", "ps_aim_out", "ps_throw_grenade", "throw_grenade" });
            IV_Animations[688] = new IVAnimSet("veh@drivebyvan", new string[] { "bl_aim_in", "bl_aim_loop", "bl_aim_out", "bl_throw_grenade", "br_aim_in", "br_aim_loop", "br_aim_out", "br_throw_grenade", "ds_aim_in", "ds_aim_loop", "ds_aim_out", "ps_aim_in", "ps_aim_loop", "ps_aim_out", "ps_throw_grenade", "throw_grenade" });
            IV_Animations[689] = new IVAnimSet("veh@helicopter", new string[] { "align_ds", "align_ps", "align_rear_ds", "align_rear_ps", "d_close_in_ds", "d_close_in_ps", "d_close_out_ds", "d_close_out_ps", "d_open_out_ds", "d_open_out_ps", "get_in_ds", "get_in_ps", "get_in_rear_ds", "get_in_rear_ps", "get_out_ds", "get_out_jacked_ds", "get_out_ps", "get_out_rear_ds", "get_out_rear_ps", "jack_dead_driver_ds", "jack_dead_driver_ds_e", "jack_dead_driver_ps", "jack_dead_perp_ds", "jack_dead_perp_ps", "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps", "keystart", "lean_left_ds", "lean_right_ds", "sit_drive", "sit_pass", "sit_pass_back_left", "sit_pass_back_right", "steer_l", "steer_r" });
            IV_Animations[690] = new IVAnimSet("veh@helicopter_xx_h", new string[] { "d_open_out_ds", "d_open_out_ps", "jump_out_ds" });
            IV_Animations[691] = new IVAnimSet("veh@heli_annih", new string[] { "d_open_out_ds", "d_open_out_ps", "jump_out_ds" });
            IV_Animations[692] = new IVAnimSet("veh@low", new string[] { "align_ds", "align_ps", "change_station", "die_ds", "die_horn_ds", "die_ps", "d_close_in_ds", "d_close_in_ps", "d_close_out_ds", "d_close_out_ds_angry", "d_close_out_ps", "d_force_entry_ds", "d_force_entry_ps", "d_locked_ds", "d_locked_ps", "d_open_out_ds", "d_open_out_ps", "get_in_ds", "get_in_jack_ps", "get_in_ps", "get_out_ds", "get_out_jacked_ds", "get_out_ps", "heavy_brake_ds", "heavy_brake_ps", "horn", "hotwire", "jack_dead_driver_ds", "jack_dead_driver_ps", "jack_dead_perp_ds", "jack_dead_perp_ps", "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_perp_ds", "jack_perp_ps", "jump_out_ds", "jump_out_ps", "keystart", "lean_left_ds", "lean_left_ps", "lean_right_ds", "lean_right_ps", "reverse", "shock_back", "shock_front", "shock_left", "shock_right", "shuffle_seat_ds", "shuffle_seat_ps", "shunt_alt_ds", "shunt_ds", "shunt_ps", "sit_drive", "sit_pass", "smash_window_ds", "smash_window_ps", "smash_window_r_ds", "smash_window_r_ps", "smash_windscreen_ds", "steer_l", "steer_r", "through_windscreen_ds", "through_windscreen_ps", "throw_grenade" });
            IV_Animations[693] = new IVAnimSet("veh@low_conv", new string[] { "align_ds", "align_ps", "get_in_ds", "get_in_ps" });
            IV_Animations[694] = new IVAnimSet("veh@low_hi2_hi2", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[695] = new IVAnimSet("veh@low_hi3_hi3", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[696] = new IVAnimSet("veh@low_hi4_hi4", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[697] = new IVAnimSet("veh@low_hi_hi", new string[] { "align_ds", "align_ps", "d_open_out_ds", "d_open_out_ps", "get_in_ds", "get_in_ps" });
            IV_Animations[698] = new IVAnimSet("veh@low_infernus", new string[] { "d_close_out_ds", "d_close_out_ps", "d_open_out_ds", "d_open_out_ps", "get_out_ds", "get_out_ps" });
            IV_Animations[699] = new IVAnimSet("veh@low_jack_pistol", new string[] { "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps" });
            IV_Animations[700] = new IVAnimSet("veh@low_jack_rifle", new string[] { "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps" });
            IV_Animations[701] = new IVAnimSet("veh@low_jack_rpg", new string[] { "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps" });
            IV_Animations[702] = new IVAnimSet("veh@low_le1_ri1", new string[] { "align_ds", "align_ps", "d_open_out_ds", "d_open_out_ps", "get_in_ds", "get_in_ps" });
            IV_Animations[703] = new IVAnimSet("veh@low_le2_ri2", new string[] { "align_ds", "align_ps", "d_open_out_ds", "d_open_out_ps", "get_in_ds", "get_in_ps" });
            IV_Animations[704] = new IVAnimSet("veh@low_lu1_ri1", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[705] = new IVAnimSet("veh@low_lu2_ri2", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[706] = new IVAnimSet("veh@low_xx_lo1", new string[] { "d_open_out_ps" });
            IV_Animations[707] = new IVAnimSet("veh@std", new string[] { "align_ds", "align_ps", "change_station", "die_ds", "die_horn_ds", "die_ps", "d_close_in_ds", "d_close_in_ps", "d_close_out_ds", "d_close_out_ds_angry", "d_close_out_ps", "d_force_entry_ds", "d_force_entry_ps", "d_locked_ds", "d_locked_ps", "d_open_out_ds", "d_open_out_ps", "get_in_ds", "get_in_jack_ps", "get_in_ps", "get_out_ds", "get_out_jacked_ds", "get_out_jacked_ds_e", "get_out_ps", "heavy_brake_ds", "heavy_brake_ps", "horn", "horn_normal", "hotwire", "jack_dead_driver_ds", "jack_dead_driver_ps", "jack_dead_perp_ds", "jack_dead_perp_ps", "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps", "jump_out_ds", "jump_out_ps", "keystart", "lean_left_ds", "lean_left_ps", "lean_right_ds", "lean_right_ps", "relaxed_idle_a", "reverse", "shock_back", "shock_front", "shock_left", "shock_right", "shuffle_seat_ds", "shuffle_seat_ps", "shunt_alt_ds", "shunt_ds", "shunt_ps", "sit_drive", "sit_pass", "sit_pass_back_right", "smash_window_ds", "smash_window_ps", "smash_window_r_ds", "smash_window_r_ps", "smash_windscreen_ds", "steer_l", "steer_r", "through_windscreen_ds", "through_windscreen_ps", "throw_grenade" });
            IV_Animations[708] = new IVAnimSet("veh@std_conv", new string[] { "align_ds", "align_ps", "get_in_ds", "get_in_ps" });
            IV_Animations[709] = new IVAnimSet("veh@std_df8", new string[] { "d_open_out_ds" });
            IV_Animations[710] = new IVAnimSet("veh@std_fo1", new string[] { "d_open_out_ds", "get_in_ds", "get_in_ps" });
            IV_Animations[711] = new IVAnimSet("veh@std_jack_pistol", new string[] { "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps" });
            IV_Animations[712] = new IVAnimSet("veh@std_jack_pistolb", new string[] { "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps" });
            IV_Animations[713] = new IVAnimSet("veh@std_jack_rifle", new string[] { "d_force_entry_ds", "d_force_entry_ps", "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps" });
            IV_Animations[714] = new IVAnimSet("veh@std_jack_rifle_b", new string[] { "d_force_entry_ds", "d_force_entry_ps", "jack_driver_ds", "jack_driver_ds_e", "jack_perp_ds" });
            IV_Animations[715] = new IVAnimSet("veh@std_jack_rifle_c", new string[] { "d_force_entry_ds", "d_force_entry_ps", "jack_driver_ds", "jack_driver_ds_e", "jack_perp_ds" });
            IV_Animations[716] = new IVAnimSet("veh@std_jack_rpg", new string[] { "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps" });
            IV_Animations[717] = new IVAnimSet("veh@std_jack_unarmdb", new string[] { "get_in_ps", "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps" });
            IV_Animations[718] = new IVAnimSet("veh@std_jack_unarmdc", new string[] { "jack_driver_ds", "jack_driver_ds_e", "jack_perp_ds" });
            IV_Animations[719] = new IVAnimSet("veh@std_jack_unarmdd", new string[] { "jack_driver_ds", "jack_driver_ds_e", "jack_perp_ds" });
            IV_Animations[720] = new IVAnimSet("veh@std_jack_unarmed", new string[] { "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps" });
            IV_Animations[721] = new IVAnimSet("veh@std_le1_ri1", new string[] { "d_close_out_ds", "d_open_out_ds", "d_open_out_ps", "get_out_ds" });
            IV_Animations[722] = new IVAnimSet("veh@std_lo1_lo1", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[723] = new IVAnimSet("veh@std_lo2_lo2", new string[] { "d_close_in_ds", "d_force_entry_ds", "d_locked_ds", "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[724] = new IVAnimSet("veh@std_lo3_lo3", new string[] { "d_close_in_ds", "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[725] = new IVAnimSet("veh@std_lo4_lo4", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[726] = new IVAnimSet("veh@std_lo5_lo5", new string[] { "d_close_in_ds", "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[727] = new IVAnimSet("veh@std_ri1", new string[] { "d_close_in_ds", "d_open_out_ds" });
            IV_Animations[728] = new IVAnimSet("veh@std_ri1_lo1", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[729] = new IVAnimSet("veh@std_ri2", new string[] { "d_close_in_ds", "d_close_out_ds", "d_open_out_ds", "d_open_out_ps", "get_out_ds" });
            IV_Animations[730] = new IVAnimSet("veh@std_ri2_lo2", new string[] { "d_close_in_ds", "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[731] = new IVAnimSet("veh@std_ri3", new string[] { "d_open_out_ds" });
            IV_Animations[732] = new IVAnimSet("veh@std_ri3_lo3", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[733] = new IVAnimSet("veh@std_xx_ri1", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[734] = new IVAnimSet("veh@std_xx_ri2", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[735] = new IVAnimSet("veh@train", new string[] { "align_ds", "align_ps", "align_rear_ds", "align_rear_ps", "d_open_out_ds", "d_open_out_ps", "d_open_out_rear_ds", "d_open_out_rear_ps", "get_in_ds", "get_in_ps", "get_in_rear_ds", "get_in_rear_ps", "get_out_ds", "get_out_ps", "get_out_rear_ds", "get_out_rear_ps", "sit_drive", "sit_pass", "sit_pass_back_left", "sit_pass_back_right" });
            IV_Animations[736] = new IVAnimSet("veh@truck", new string[] { "align_ds", "align_ps", "align_rear_ds", "align_rear_ps", "change_station", "die_ds", "die_horn_ds", "die_ps", "d_close_in_ds", "d_close_in_ps", "d_close_in_rear_ds", "d_close_in_rear_ps", "d_close_out_ds", "d_close_out_ds_angry", "d_close_out_ps", "d_close_out_rear_ds", "d_close_out_rear_ps", "d_open_out_ds", "d_open_out_ps", "d_open_out_rear_ds", "d_open_out_rear_ps", "get_in_ds", "get_in_jack_ps", "get_in_ps", "get_in_rear_ds", "get_in_rear_ps", "get_out_ds", "get_out_jacked_ds", "get_out_ps", "get_out_rear_ds", "get_out_rear_ps", "heavy_brake_ds", "heavy_brake_ps", "horn", "hotwire", "jack_dead_driver_ds", "jack_dead_driver_ps", "jack_dead_perp_ds", "jack_dead_perp_ps", "jack_driver_ds", "jack_driver_ps", "jack_perp_ds", "jack_perp_ps", "jump_out_ds", "jump_out_ps", "keystart", "lean_left_ds", "lean_left_ps", "lean_right_ds", "lean_right_ps", "reverse", "shock_back", "shock_front", "shock_left", "shock_right", "shuffle_seat_ds", "shuffle_seat_ps", "shunt_alt_ds", "shunt_ds", "shunt_ps", "sit_drive", "sit_pass", "sit_pass_back_left", "sit_pass_back_right", "smash_window_ds", "smash_window_ps", "smash_window_r_ds", "smash_window_r_ps", "smash_windscreen_ds", "steer_l", "steer_r", "through_windscreen_ds", "through_windscreen_ps", "throw_grenade" });
            IV_Animations[737] = new IVAnimSet("veh@truck_ba_xx", new string[] { "d_open_out_ds" });
            IV_Animations[738] = new IVAnimSet("veh@truck_le1_ri1", new string[] { "d_close_in_ds", "d_open_out_ds", "d_open_out_ps", "get_in_ds", "get_out_ds", "keystart", "sit_drive", "steer_l", "steer_r" });
            IV_Animations[739] = new IVAnimSet("veh@truck_lo1_lo1", new string[] { "crash_exit_on_side_l", "crash_exit_on_side_r", "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[740] = new IVAnimSet("veh@truck_phantom", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[741] = new IVAnimSet("veh@tru_jack_pistol", new string[] { "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps" });
            IV_Animations[742] = new IVAnimSet("veh@tru_jack_rifle", new string[] { "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps" });
            IV_Animations[743] = new IVAnimSet("veh@van", new string[] { "align_ds", "align_ps", "align_rear_ds", "align_rear_ps", "change_station", "die_ds", "die_horn_ds", "die_ps", "d_close_in_ds", "d_close_in_ps", "d_close_in_rear_ds", "d_close_in_rear_ps", "d_close_out_ds", "d_close_out_ds_angry", "d_close_out_ps", "d_close_out_rear_ds", "d_close_out_rear_ps", "d_force_entry_ds", "d_force_entry_ps", "d_locked_ds", "d_locked_ps", "d_open_out_ds", "d_open_out_ps", "d_open_out_rear_ds", "d_open_out_rear_ps", "get_in_ds", "get_in_jack_ps", "get_in_ps", "get_in_rear_ds", "get_in_rear_ps", "get_out_ds", "get_out_jacked_ds", "get_out_jacked_ds_e", "get_out_ps", "get_out_rear_ds", "get_out_rear_ps", "heavy_brake_ds", "heavy_brake_ps", "horn", "hotwire", "jack_dead_driver_ds", "jack_dead_driver_ps", "jack_dead_perp_ds", "jack_dead_perp_ps", "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps", "jump_out_ds", "jump_out_ps", "keystart", "lean_left_ds", "lean_left_ps", "lean_right_ds", "lean_right_ps", "reverse", "shock_back", "shock_front", "shock_left", "shock_right", "shuffle_seat_ds", "shuffle_seat_ps", "shunt_alt_ds", "shunt_ds", "shunt_ps", "sit_drive", "sit_pass", "sit_pass_back_left", "sit_pass_back_right", "smash_window_ds", "smash_window_ps", "smash_window_r_ds", "smash_window_r_ps", "smash_windscreen_ds", "steer_l", "steer_r", "through_windscreen_ds", "through_windscreen_ps", "throw_grenade" });
            IV_Animations[744] = new IVAnimSet("veh@van_ba1", new string[] { "d_close_out_ds", "d_open_out_ds", "d_open_out_ps", "get_out_ds" });
            IV_Animations[745] = new IVAnimSet("veh@van_fo1_fo1", new string[] { "align_ps", "d_close_out_ds", "d_open_out_ds", "d_open_out_ps", "get_out_ds" });
            IV_Animations[746] = new IVAnimSet("veh@van_fo2_fo2", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[747] = new IVAnimSet("veh@van_hl1_hi1", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[748] = new IVAnimSet("veh@van_jack_pistol", new string[] { "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps" });
            IV_Animations[749] = new IVAnimSet("veh@van_jack_rifle", new string[] { "get_in_ds", "get_in_ps", "jack_driver_ds", "jack_driver_ds_e", "jack_driver_ps", "jack_driver_ps_e", "jack_perp_ds", "jack_perp_ps" });
            IV_Animations[750] = new IVAnimSet("veh@van_jack_unarmed", new string[] { "jack_driver_ds", "jack_driver_ds_e", "jack_perp_ds" });
            IV_Animations[751] = new IVAnimSet("veh@van_ll_lo", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[752] = new IVAnimSet("veh@van_lr1_lf1", new string[] { "d_close_out_ds", "d_open_out_ds", "d_open_out_ps", "get_out_ds" });
            IV_Animations[753] = new IVAnimSet("veh@van_ri1_le1", new string[] { "d_close_out_ds", "d_open_out_ds", "d_open_out_ps", "get_out_ds" });
            IV_Animations[754] = new IVAnimSet("veh@van_ri2_le2", new string[] { "d_close_out_ds", "d_open_out_ds", "d_open_out_ps", "get_out_ds" });
            IV_Animations[755] = new IVAnimSet("veh@van_ri3_le3", new string[] { "d_open_out_ds", "d_open_out_ps" });
            IV_Animations[756] = new IVAnimSet("veh@van_xx_le1", new string[] { "d_close_out_ds", "d_open_out_ds", "d_open_out_ps", "get_out_ds" });
            IV_Animations[757] = new IVAnimSet("visemes@f_hi", new string[] { "rest" });
            IV_Animations[758] = new IVAnimSet("visemes@f_lo", new string[] { "a", "e", "fv", "lth", "mbp", "ouwq", "rest", "s" });
            IV_Animations[759] = new IVAnimSet("visemes@m_hi", new string[] { "a", "e", "fv", "lth", "mbp", "ouwq", "rest", "s" });
            IV_Animations[760] = new IVAnimSet("visemes@m_lo", new string[] { "a", "e", "fv", "lth", "mbp", "ouwq", "rest", "s" });
            #endregion
        }
        #endregion

        #region Methods
        /// <summary>
        /// Does an auto save.
        /// </summary>
        public static void DoAutoSave()
        {
            DO_AUTO_SAVE();
        }
        /// <summary>
        /// Shows the save menu.
        /// </summary>
        public static void ShowSaveMenu()
        {
            ACTIVATE_SAVE_MENU();
        }

        /// <summary>
        /// Plays an audio event with the given name.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        public static void PlayAudioEvent(string eventName)
        {
            PLAY_AUDIO_EVENT(eventName);
        }
        /// <summary>
        /// Starts playing the credits music.
        /// </summary>
        public static void PlayCreditsMusic()
        {
            IVTheScripts.SetDummyThread();
            START_END_CREDITS_MUSIC();
            IVTheScripts.RestorePreviousThread();
        }
        /// <summary>
        /// Stops playing the credits music.
        /// </summary>
        public static void StopCreditsMusic()
        {
            IVTheScripts.SetDummyThread();
            STOP_END_CREDITS_MUSIC();
            IVTheScripts.RestorePreviousThread();
        }
        /// <summary>
        /// Previews a ringtone with the given id.
        /// </summary>
        /// <param name="ringtoneId">The ringtone id to preview.</param>
        public static void PreviewRingtone(int ringtoneId)
        {
            IVTheScripts.SetDummyThread();
            PREVIEW_RINGTONE(ringtoneId);
            IVTheScripts.RestorePreviousThread();
        }
        /// <summary>
        /// Stops the current ringtone preview.
        /// </summary>
        public static void StopRingtonePreview()
        {
            IVTheScripts.SetDummyThread();
            STOP_PREVIEW_RINGTONE();
            IVTheScripts.RestorePreviousThread();
        }

        /// <summary>
        /// Pauses the game. Used when you open the Games for Windows Live overlay.
        /// </summary>
        public static void Pause()
        {
            PAUSE_GAME();
        }
        /// <summary>
        /// Unpauses the game.
        /// </summary>
        public static void Unpause()
        {
            UNPAUSE_GAME();
        }

        /// <summary>
        /// Requests a game script (sco).
        /// </summary>
        /// <param name="name">The script name without extension.</param>
        public static void RequestScript(string name)
        {
            if (!DoesScriptExists(name))
                return;

            REQUEST_SCRIPT(name);
        }
        /// <summary>
        /// Marks a game script (sco) as no longer needed.
        /// </summary>
        /// <param name="name">The script name without extension.</param>
        public static void MarkScriptAsNoLongerNeeded(string name)
        {
            if (!DOES_SCRIPT_EXIST(name))
                return;

            MARK_SCRIPT_AS_NO_LONGER_NEEDED(name);
        }
        /// <summary>
        /// Terminates all game scripts (sco) with the given name.
        /// </summary>
        /// <param name="name">The script name without extension.</param>
        public static void TerminateScriptsWithThisName(string name)
        {
            if (!DoesScriptExists(name))
                return;

            TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME(name);
        }

        /// <summary>
        /// Displays a custom help message at the top left of the screen.
        /// </summary>
        /// <param name="text">The text you want to display.</param>
        /// <param name="noSound">If the message should display without any sound.</param>
        /// <param name="forever">If the message should be visible all the time. Make sure to call <see cref="CLEAR_HELP"/> once you're done.</param>
        /// <param name="targetReplacementGXT">
        /// This method works by replacing the text of an unused GXT entry to show your own custom <paramref name="text"/>.<br/>
        /// If you like to specify which GXT entry should be replaced, you can do that here.<br/>
        /// This can be useful to check if a help message is currently being displayed by using <see cref="IS_THIS_HELP_MESSAGE_BEING_DISPLAYED(string)"/>.
        /// </param>
        public static void DisplayCustomHelpMessage(string text, bool noSound = false, bool forever = false, string targetReplacementGXT = "PLACEHOLDER_1")
        {
            IVText.TheIVText.ReplaceTextOfTextLabel(targetReplacementGXT, text);
            
            if (noSound)
            {
                if (forever)
                    PRINT_HELP_FOREVER_WITH_STRING_NO_SOUND(targetReplacementGXT, "STRING");
                else
                    PRINT_HELP_WITH_STRING_NO_SOUND(targetReplacementGXT, "STRING");
            }
            else
            {
                if (forever)
                    PRINT_HELP_FOREVER(targetReplacementGXT);
                else
                    PRINT_HELP(targetReplacementGXT);
            }
        }
        #endregion

        #region Functions
        public static int GetIntegerStatistic(eIntStatistic stat)
        {
            return GET_INT_STAT((int)stat);
        }
        public static void SetIntegerStatistic(eIntStatistic stat, int value)
        {
            SET_INT_STAT((int)stat, value);
        }

        public static float GetFloatStatistic(eFloatStatistic stat)
        {
            return GET_FLOAT_STAT((int)stat);
        }
        public static void SetFloatStatistic(eFloatStatistic stat, float value)
        {
            SET_FLOAT_STAT((int)stat, value);
        }

        /// <summary>
        /// Starts a new game script (sco).
        /// </summary>
        /// <param name="name">The script name without extension.</param>
        /// <param name="stackSize">Usually 1024?</param>
        /// <returns>Unknown.</returns>
        public static uint StartNewScript(string name, uint stackSize)
        {
            if (!DoesScriptExists(name))
                return 0;

            return START_NEW_SCRIPT(name, stackSize);
        }
        /// <summary>
        /// Gets how many game scripts (sco) are running.
        /// </summary>
        /// <param name="name">The script name without extension.</param>
        /// <returns>The number of scripts running.</returns>
        public static uint GetScriptInstances(string name)
        {
            return GET_NUMBER_OF_INSTANCES_OF_STREAMED_SCRIPT(name);
        }
        /// <summary>
        /// Checks if the given game script (sco) exists.
        /// </summary>
        /// <param name="name">The script name without extension.</param>
        /// <returns>True if the script exists. Otherwise, false.</returns>
        public static bool DoesScriptExists(string name)
        {
            return DOES_SCRIPT_EXIST(name);
        }
        /// <summary>
        /// Gets if the game script (sco) is running.
        /// </summary>
        /// <param name="name">The script name without extension.</param>
        /// <returns>True if the script is running. Otherwise, false.</returns>
        public static bool IsScriptRunning(string name)
        {
            if (!DoesScriptExists(name))
                return false;
            if (GetScriptInstances(name) <= 0)
                return false;

            return true;
        }
        /// <summary>
        /// Gets if the game script (sco) is loaded.
        /// </summary>
        /// <param name="name">The script name without extension.</param>
        /// <returns>True if the script is loaded. Otherwise, false.</returns>
        public static bool HasScriptLoaded(string name)
        {
            if (!DoesScriptExists(name))
                return false;

            return HAS_SCRIPT_LOADED(name);
        }

        /// <summary>
        /// Generates a hash from the given string.
        /// <para>Useful for getting the hash for a model from the model's name.</para>
        /// </summary>
        /// <param name="str">The string to generate the hash from.</param>
        /// <returns>The hash from the given string.</returns>
        public static int GenerateHash(string str)
        {
            return GET_HASH_KEY(str);
        }
        #endregion

    }
}
