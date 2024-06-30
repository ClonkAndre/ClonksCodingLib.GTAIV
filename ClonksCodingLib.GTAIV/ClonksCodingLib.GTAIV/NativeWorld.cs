using System;
using System.Numerics;

using IVSDKDotNet;
using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Gives you easy access to native functions that involve the world.
    /// </summary>
    public class NativeWorld
    {

        #region Properties
        /// <summary>
        /// Sets the ped density.
        /// <para>0.0 is off, 1.0 is default, 2.0 is twice as much as normal, etc.</para>
        /// </summary>
        public static float PedDensity
        {
            set
            {
                SET_PED_DENSITY_MULTIPLIER(value); 
            }
        }
        /// <summary>
        /// Sets the car density.
        /// <para>0.0 is off, 1.0 is default, 2.0 is twice as much as normal, etc</para>
        /// </summary>
        public static float CarDensity
        {
            set
            {
                SET_CAR_DENSITY_MULTIPLIER(value);
            }
        }
        /// <summary>
        /// Sets the parked car density.
        /// <para>0.0 is off, 1.0 is default, 2.0 is twice as much as normal, etc</para>
        /// </summary>
        public static float ParkedCarDensity
        {
            set
            {
                SET_PARKED_CAR_DENSITY_MULTIPLIER(value);
            }
        }
        /// <summary>
        /// Sets the random car density.
        /// <para>0.0 is off, 1.0 is default, 2.0 is twice as much as normal, etc</para>
        /// </summary>
        public static float RandomCarDensity
        {
            set
            {
                SET_RANDOM_CAR_DENSITY_MULTIPLIER(value);
            }
        }

        /// <summary>
        /// Sets if random trains are allowed to appear.
        /// </summary>
        public static bool AllowRandomTrains
        {
            set
            {
                SWITCH_RANDOM_TRAINS(value);
            }
        }
        /// <summary>
        /// Sets if random boats are allowed to appear.
        /// </summary>
        public static bool AllowRandomBoats
        {
            set
            {
                SWITCH_RANDOM_BOATS(value);
            }
        }
        /// <summary>
        /// Sets if ambient planes are allowed to appear.
        /// </summary>
        public static bool AllowAmbientPlanes
        {
            set
            {
                SWITCH_AMBIENT_PLANES(value);
            }
        }
        /// <summary>
        /// Sets if garbage trucks are allowed to appear.
        /// </summary>
        public static bool AllowGarbageTrucks
        {
            set
            {
                SWITCH_GARBAGE_TRUCKS(value);
            }
        }

        /// <summary>
        /// Sets if the gravity should be enabled or not. Some things in the world might not be affected by setting this to <see langword="false"/>.
        /// </summary>
        public static bool GravityEnabled
        {
            set
            {
                SET_GRAVITY_OFF(!value);
            }
        }

        /// <summary>
        /// Gets or sets the current weather in the world.
        /// </summary>
        public static eWeather CurrentWeather
        {
            get
            {
                GET_CURRENT_WEATHER(out int weather);
                return (eWeather)weather;
            }
            set
            {
                FORCE_WEATHER_NOW((uint)value);
            }
        }

        /// <summary>
        /// Gets or set the current day time in the world.
        /// </summary>
        public static TimeSpan CurrentDayTime
        {
            get
            {
                GET_TIME_OF_DAY(out int hour, out int min);
                return new TimeSpan(hour, min, 0);
            }
            set
            {
                SET_TIME_OF_DAY((uint)value.Hours, (uint)value.Minutes);
            }
        }

        /// <summary>
        /// Gets the current date in the world.
        /// </summary>
        public static DateTime CurrentDate
        {
            get
            {
                GET_TIME_OF_DAY(out int hour, out int min);
                GET_CURRENT_DATE(out uint day, out uint month);
                return new DateTime(2008, (int)month, (int)day, hour, min, 0);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// This function mutes almost all sounds in the game. Some "front end" sounds, like info beeps and cell phone beeps, car radios, cutscenes, and the TV aren't affected. 
        /// </summary>
        /// <param name="mute">Sets if the world should be muted or not.</param>
        public static void MuteGameworldAndPositionedRadioForTV(bool mute)
        {
            MUTE_GAMEWORLD_AND_POSITIONED_RADIO_FOR_TV(true);
        }

        public static void SetScenarioPedDensity(float density, float densityNext)
        {
            SET_SCENARIO_PED_DENSITY_MULTIPLIER(density, densityNext);
        }

        /// <summary>
        /// Creates an explosion at the given postition.
        /// </summary>
        /// <param name="pos">The explosion position.</param>
        public static void AddExplosion(Vector3 pos)
        {
            ADD_EXPLOSION(pos.X, pos.Y, pos.Z, (int)eExplosionType.EXPLOSION_TYPE_DEFAULT, 1f, true, false, 1f);
        }
        /// <summary>
        /// Creates an explosion at the given postition.
        /// </summary>
        /// <param name="pos">The explosion position.</param>
        /// <param name="type">The explosion type.</param>
        /// <param name="radius">The explosion radius.</param>
        public static void AddExplosion(Vector3 pos, eExplosion type, float radius)
        {
            ADD_EXPLOSION(pos.X, pos.Y, pos.Z, (int)type, radius, true, false, 1f);
        }
        /// <summary>
        /// Creates an explosion at the given postition.
        /// </summary>
        /// <param name="pos">The explosion position.</param>
        /// <param name="type">The explosion type.</param>
        /// <param name="radius">The explosion radius.</param>
        /// <param name="playSound">Sets if the explosion plays sound.</param>
        /// <param name="noVisual">Sets if the explosion should be invisible.</param>
        /// <param name="camShake">Sets how much the cam will shake.</param>
        public static void AddExplosion(Vector3 pos, eExplosion type, float radius, bool playSound, bool noVisual, float camShake)
        {
            ADD_EXPLOSION(pos.X, pos.Y, pos.Z, (int)type, radius, playSound, noVisual, camShake);
        }

        /// <summary>
        /// Sets the state of the closest door with the given model.
        /// </summary>
        /// <param name="model">Model of the nearby door.</param>
        /// <param name="pos">Position close to the door.</param>
        /// <param name="locked">The door cannot be moved when locked.</param>
        /// <param name="angle">The current door angle. 0 is closed, 1 is open, -1 is open to the other direction.</param>
        public static void SetDoorState(uint model, Vector3 pos, bool locked, float angle)
        {
            SET_STATE_OF_CLOSEST_DOOR_OF_TYPE(model, pos.X, pos.Y, pos.Z, locked ? true : false, angle);
        }

        /// <summary>
        /// Loads the environment at the given position.
        /// <para>Warning: Can cause a freeze for a few seconds.</para>
        /// </summary>
        /// <param name="pos">The target position.</param>
        public static void LoadEnvironmentNow(Vector3 pos)
        {
            REQUEST_COLLISION_AT_POSN(pos);
            LOAD_ALL_OBJECTS_NOW();
            LOAD_SCENE(pos);
            POPULATE_NOW();
        }

        /// <summary>
        /// Goes to the next day.
        /// </summary>
        public static void OneDayForward()
        {
            SET_TIME_ONE_DAY_FORWARD();
        }
        /// <summary>
        /// Goes back one day.
        /// </summary>
        public static void OneDayBack()
        {
            SET_TIME_ONE_DAY_BACK();
        }

        public static void LockDayTime(TimeSpan time)
        {
            if (time.Hours < 0)
                return;
            if (time.Minutes < 0)
                return;

            FORCE_TIME_OF_DAY((uint)time.Hours, (uint)time.Minutes);
        }
        public static void LockDayTime(uint hour, uint minute)
        {
            if (hour < 0)
                return;
            if (hour > 23)
                return;
            if (minute < 0)
                return;
            if (minute > 59)
                return;

            FORCE_TIME_OF_DAY(hour, minute);
        }

        /// <summary>
        /// The time of day will be released.
        /// </summary>
        public static void UnlockDayTime()
        {
            RELEASE_TIME_OF_DAY();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Creates a new ped with the given model name at the given position.
        /// </summary>
        /// <param name="modelName">The model name of the ped.</param>
        /// <param name="position">The position of the ped.</param>
        /// <param name="handle">Returns the handle of the ped if the function succeeded. The handle can be used with all sorts of native functions that want the handle of a ped.</param>
        /// <param name="addToWorld">Sets if the ped should be added to the world. Default is true.</param>
        /// <param name="setAsMissionPed">Sets if the ped should be marked as a mission ped. This will prevent the ped from despawning. Default is false.</param>
        /// <returns>If successful, the newly created <see cref="IVPed"/> is returned. Otherwise, null.</returns>
        public static IVPed SpawnPed(string modelName, IVMatrix position, out int handle, bool addToWorld = true, bool setAsMissionPed = false)
        {
            uint modelHash = RAGE.AtStringHash(modelName);
            IVModelInfo.GetModelInfo(modelHash, out int index);
            IVStreaming.ScriptRequestModel((int)modelHash);
            IVStreaming.LoadAllRequestedModels(false);

            IVPed ped = IVPedFactoryNY.ThePedFactory.CreatePed(IVSpawnData.Default(), index, position, true, true);

            if (ped == null)
            {
                handle = 0;
                return null;
            }

            if (addToWorld) 
                IVWorld.Add(ped.GetUIntPtr(), false);

            int pedHandle = (int)IVPools.GetPedPool().GetIndex(ped.GetUIntPtr());

            if (setAsMissionPed)
                SET_CHAR_AS_MISSION_CHAR(pedHandle);

            handle = pedHandle;
            return ped;
        }
        /// <summary>
        /// Creates a new ped with the given model name at the given position.
        /// </summary>
        /// <param name="modelName">The model name of the ped.</param>
        /// <param name="position">The position of the ped.</param>
        /// <param name="handle">Returns the handle of the ped if the function succeeded. The handle can be used with all sorts of native functions that want the handle of a ped.</param>
        /// <param name="addToWorld">Sets if the ped should be added to the world. Default is true.</param>
        /// <param name="setAsMissionPed">Sets if the ped should be marked as a mission ped. This will prevent the ped from despawning. Default is false.</param>
        /// <returns>If successful, the newly created <see cref="IVPed"/> is returned. Otherwise, null.</returns>
        public static IVPed SpawnPed(string modelName, Vector3 position, out int handle, bool addToWorld = true, bool setAsMissionPed = false)
        {
            uint modelHash = RAGE.AtStringHash(modelName);
            IVModelInfo.GetModelInfo(modelHash, out int index);
            IVStreaming.ScriptRequestModel((int)modelHash);
            IVStreaming.LoadAllRequestedModels(false);

            IVPed ped = IVPedFactoryNY.ThePedFactory.CreatePed(IVSpawnData.Default(), index, new IVMatrix(Vector3.Zero, Vector3.Zero, Vector3.Zero, position), true, true);

            if (ped == null)
            {
                handle = 0;
                return null;
            }

            if (addToWorld)
                IVWorld.Add(ped.GetUIntPtr(), false);

            int pedHandle = (int)IVPools.GetPedPool().GetIndex(ped.GetUIntPtr());

            if (setAsMissionPed)
                SET_CHAR_AS_MISSION_CHAR(pedHandle);

            handle = pedHandle;
            return ped;
        }
        /// <summary>
        /// Creates a new ped with the given model hash at the given position.
        /// </summary>
        /// <param name="model">The model hash of the ped.</param>
        /// <param name="position">The position of the ped.</param>
        /// <param name="handle">Returns the handle of the ped if the function succeeded. The handle can be used with all sorts of native functions that want the handle of a ped.</param>
        /// <param name="addToWorld">Sets if the ped should be added to the world. Default is true.</param>
        /// <param name="setAsMissionPed">Sets if the ped should be marked as a mission ped. This will prevent the ped from despawning. Default is false.</param>
        /// <returns>If successful, the newly created <see cref="IVPed"/> is returned. Otherwise, null.</returns>
        public static IVPed SpawnPed(uint model, IVMatrix position, out int handle, bool addToWorld = true, bool setAsMissionPed = false)
        {
            IVModelInfo.GetModelInfo(model, out int index);
            IVStreaming.ScriptRequestModel((int)model);
            IVStreaming.LoadAllRequestedModels(false);

            IVPed ped = IVPedFactoryNY.ThePedFactory.CreatePed(IVSpawnData.Default(), index, position, true, true);

            if (ped == null)
            {
                handle = 0;
                return null;
            }

            if (addToWorld)
                IVWorld.Add(ped.GetUIntPtr(), false);

            int pedHandle = (int)IVPools.GetPedPool().GetIndex(ped.GetUIntPtr());

            if (setAsMissionPed)
                SET_CHAR_AS_MISSION_CHAR(pedHandle);

            handle = pedHandle;
            return ped;
        }
        /// <summary>
        /// Creates a new ped with the given model hash at the given position.
        /// </summary>
        /// <param name="model">The model hash of the ped.</param>
        /// <param name="position">The position of the ped.</param>
        /// <param name="handle">Returns the handle of the ped if the function succeeded. The handle can be used with all sorts of native functions that want the handle of a ped.</param>
        /// <param name="addToWorld">Sets if the ped should be added to the world. Default is true.</param>
        /// <param name="setAsMissionPed">Sets if the ped should be marked as a mission ped. This will prevent the ped from despawning. Default is false.</param>
        /// <returns>If successful, the newly created <see cref="IVPed"/> is returned. Otherwise, null.</returns>
        public static IVPed SpawnPed(uint model, Vector3 position, out int handle, bool addToWorld = true, bool setAsMissionPed = false)
        {
            IVModelInfo.GetModelInfo(model, out int index);
            IVStreaming.ScriptRequestModel((int)model);
            IVStreaming.LoadAllRequestedModels(false);

            IVPed ped = IVPedFactoryNY.ThePedFactory.CreatePed(IVSpawnData.Default(), index, new IVMatrix(Vector3.Zero, Vector3.Zero, Vector3.Zero, position), true, true);

            if (ped == null)
            {
                handle = 0;
                return null;
            }

            if (addToWorld)
                IVWorld.Add(ped.GetUIntPtr(), false);

            int pedHandle = (int)IVPools.GetPedPool().GetIndex(ped.GetUIntPtr());

            if (setAsMissionPed)
                SET_CHAR_AS_MISSION_CHAR(pedHandle);

            handle = pedHandle;
            return ped;
        }

        /// <summary>
        /// Creates a new vehicle with the given model name at the given position.
        /// </summary>
        /// <param name="modelName">The model name of the vehicle.</param>
        /// <param name="position">The position of the vehicle.</param>
        /// <param name="handle">Returns the handle of the vehicle if the function succeeded. The handle can be used with all sorts of native functions that want the handle of a vehicle.</param>
        /// <param name="addToWorld">Sets if the vehicle should be added to the world. Default is true.</param>
        /// <param name="setAsMissionVehicle">Sets if the vehicle should be marked as a mission vehicle. This will prevent the vehicle from despawning. Default is false.</param>
        /// <returns>If successful, the newly created <see cref="IVVehicle"/> is returned. Otherwise, null.</returns>
        public static IVVehicle SpawnVehicle(string modelName, IVMatrix position, out int handle, bool addToWorld = true, bool setAsMissionVehicle = false)
        {
            uint modelHash = RAGE.AtStringHash(modelName);
            IVModelInfo.GetModelInfo(modelHash, out int index);
            IVStreaming.ScriptRequestModel((int)modelHash);
            IVStreaming.LoadAllRequestedModels(false);

            IVVehicle veh = IVVehicleFactoryNY.TheVehicleFactory.CreateVehicle(index, (int)eVehicleCreatedBy.RANDOM_VEHICLE, position, true);

            if (veh == null)
            {
                handle = 0;
                return null;
            }

            if (addToWorld)
                IVWorld.Add(veh.GetUIntPtr(), false);

            int vehHandle = (int)IVPools.GetVehiclePool().GetIndex(veh.GetUIntPtr());

            if (setAsMissionVehicle)
                SET_CAR_AS_MISSION_CAR(vehHandle);

            handle = vehHandle;
            return veh;
        }
        /// <summary>
        /// Creates a new vehicle with the given model name at the given position.
        /// </summary>
        /// <param name="modelName">The model name of the vehicle.</param>
        /// <param name="position">The position of the vehicle.</param>
        /// <param name="handle">Returns the handle of the vehicle if the function succeeded. The handle can be used with all sorts of native functions that want the handle of a vehicle.</param>
        /// <param name="addToWorld">Sets if the vehicle should be added to the world. Default is true.</param>
        /// <param name="setAsMissionVehicle">Sets if the vehicle should be marked as a mission vehicle. This will prevent the vehicle from despawning. Default is false.</param>
        /// <returns>If successful, the newly created <see cref="IVVehicle"/> is returned. Otherwise, null.</returns>
        public static IVVehicle SpawnVehicle(string modelName, Vector3 position, out int handle, bool addToWorld = true, bool setAsMissionVehicle = false)
        {
            uint modelHash = RAGE.AtStringHash(modelName);
            IVModelInfo.GetModelInfo(modelHash, out int index);
            IVStreaming.ScriptRequestModel((int)modelHash);
            IVStreaming.LoadAllRequestedModels(false);
            
            IVVehicle veh = IVVehicleFactoryNY.TheVehicleFactory.CreateVehicle(index, (int)eVehicleCreatedBy.RANDOM_VEHICLE, new IVMatrix(Vector3.Zero, Vector3.Zero, Vector3.Zero, position), true);

            if (veh == null)
            {
                handle = 0;
                return null;
            }

            if (addToWorld)
                IVWorld.Add(veh.GetUIntPtr(), false);

            int vehHandle = (int)IVPools.GetVehiclePool().GetIndex(veh.GetUIntPtr());

            if (setAsMissionVehicle)
                SET_CAR_AS_MISSION_CAR(vehHandle);

            handle = vehHandle;
            return veh;
        }
        /// <summary>
        /// Creates a new vehicle with the given model hash at the given position.
        /// </summary>
        /// <param name="model">The model hash of the vehicle.</param>
        /// <param name="position">The position of the vehicle.</param>
        /// <param name="handle">Returns the handle of the vehicle if the function succeeded. The handle can be used with all sorts of native functions that want the handle of a vehicle.</param>
        /// <param name="addToWorld">Sets if the vehicle should be added to the world. Default is true.</param>
        /// <param name="setAsMissionVehicle">Sets if the vehicle should be marked as a mission vehicle. This will prevent the vehicle from despawning. Default is false.</param>
        /// <returns>If successful, the newly created <see cref="IVVehicle"/> is returned. Otherwise, null.</returns>
        public static IVVehicle SpawnVehicle(uint model, IVMatrix position, out int handle, bool addToWorld = true, bool setAsMissionVehicle = false)
        {
            IVModelInfo.GetModelInfo(model, out int index);
            IVStreaming.ScriptRequestModel((int)model);
            IVStreaming.LoadAllRequestedModels(false);

            IVVehicle veh = IVVehicleFactoryNY.TheVehicleFactory.CreateVehicle(index, (int)eVehicleCreatedBy.RANDOM_VEHICLE, position, true);

            if (veh == null)
            {
                handle = 0;
                return null;
            }

            if (addToWorld)
                IVWorld.Add(veh.GetUIntPtr(), false);

            int vehHandle = (int)IVPools.GetVehiclePool().GetIndex(veh.GetUIntPtr());

            if (setAsMissionVehicle)
                SET_CAR_AS_MISSION_CAR(vehHandle);

            handle = vehHandle;
            return veh;
        }
        /// <summary>
        /// Creates a new vehicle with the given model hash at the given position.
        /// </summary>
        /// <param name="model">The model hash of the vehicle.</param>
        /// <param name="position">The position of the vehicle.</param>
        /// <param name="handle">Returns the handle of the vehicle if the function succeeded. The handle can be used with all sorts of native functions that want the handle of a vehicle.</param>
        /// <param name="addToWorld">Sets if the vehicle should be added to the world. Default is true.</param>
        /// <param name="setAsMissionVehicle">Sets if the vehicle should be marked as a mission vehicle. This will prevent the vehicle from despawning. Default is false.</param>
        /// <returns>If successful, the newly created <see cref="IVVehicle"/> is returned. Otherwise, null.</returns>
        public static IVVehicle SpawnVehicle(uint model, Vector3 position, out int handle, bool addToWorld = true, bool setAsMissionVehicle = false)
        {
            IVModelInfo.GetModelInfo(model, out int index);
            IVStreaming.ScriptRequestModel((int)model);
            IVStreaming.LoadAllRequestedModels(false);

            IVVehicle veh = IVVehicleFactoryNY.TheVehicleFactory.CreateVehicle(index, (int)eVehicleCreatedBy.RANDOM_VEHICLE, new IVMatrix(Vector3.Zero, Vector3.Zero, Vector3.Zero, position), true);

            if (veh == null)
            {
                handle = 0;
                return null;
            }

            if (addToWorld)
                IVWorld.Add(veh.GetUIntPtr(), false);

            int vehHandle = (int)IVPools.GetVehiclePool().GetIndex(veh.GetUIntPtr());

            if (setAsMissionVehicle)
                SET_CAR_AS_MISSION_CAR(vehHandle);

            handle = vehHandle;
            return veh;
        }

        /// <summary>
        /// Doesn't always work so well.
        /// </summary>
        /// <param name="position">The postition to search for peds.</param>
        /// <param name="radius">The radius to search for the closest ped.</param>
        /// <param name="unk1">Undocumented. Usually 0 or 1.</param>
        /// <param name="unk2">Undocumented. Usually 0 or 1.</param>
        /// <returns>If successful, the closest <see cref="IVPed"/> is returned. Otherwise, null.</returns>
        public static IVPed GetClosestPed(Vector3 position, float radius, int unk1, int unk2)
        {
            //ALLOW_SCENARIO_PEDS_TO_BE_RETURNED_BY_NEXT_COMMAND(true);
            BEGIN_CHAR_SEARCH_CRITERIA();
            END_CHAR_SEARCH_CRITERIA();

            GET_CLOSEST_CHAR(position.X, position.Y, position.Z, radius, unk1, unk2, out int handle);

            if (handle == 0)
                return null;

            return IVPed.FromUIntPtr(IVPools.GetPedPool().GetAt((uint)handle));
        }

        /// <summary>
        /// Gets the <see cref="IVPed"/> IV-SDK instance from the given <paramref name="pedHandle"/>.
        /// </summary>
        /// <param name="pedHandle">The ped handle to get the <see cref="IVPed"/> instance from.</param>
        /// <returns>If successful the <see cref="IVPed"/> is returned. Otherwise, null.</returns>
        public static IVPed GetPedInstaceFromHandle(int pedHandle)
        {
            UIntPtr ptr = IVPools.GetPedPool().GetAt((uint)pedHandle);

            if (ptr != UIntPtr.Zero)
                return IVPed.FromUIntPtr(ptr);

            return null;
        }
        // Whoops
        /// <summary>
        /// Gets the <see cref="IVVehicle"/> IV-SDK instance from the given <paramref name="vehicleHandle"/>.
        /// </summary>
        /// <param name="vehicleHandle">The vehicle handle to get the <see cref="IVVehicle"/> instance from.</param>
        /// <returns>If successful the <see cref="IVVehicle"/> is returned. Otherwise, null.</returns>
        public static IVVehicle GetVehicleInstaceFromHandle(int vehicleHandle)
        {
            UIntPtr ptr = IVPools.GetVehiclePool().GetAt((uint)vehicleHandle);

            if (ptr != UIntPtr.Zero)
                return IVVehicle.FromUIntPtr(ptr);

            return null;
        }
        /// <summary>
        /// Gets the <see cref="IVObject"/> IV-SDK instance from the given <paramref name="objectHandle"/>.
        /// </summary>
        /// <param name="objectHandle">The object handle to get the <see cref="IVObject"/> instance from.</param>
        /// <returns>If successful the <see cref="IVObject"/> is returned. Otherwise, null.</returns>
        public static IVObject GetObjectInstaceFromHandle(int objectHandle)
        {
            UIntPtr ptr = IVPools.GetObjectPool().GetAt((uint)objectHandle);

            if (ptr != UIntPtr.Zero)
                return (IVObject)IVEntity.FromUIntPtr(ptr);

            return null;
        }

        /// <summary>
        /// Gets the name of the current zone at this position.
        /// </summary>
        /// <param name="pos">The position to get the zone name of.</param>
        /// <returns>The name of the zone.</returns>
        public static string GetZoneName(Vector3 pos)
        {
            return GET_NAME_OF_ZONE(pos.X, pos.Y, pos.Z);
        }
        /// <summary>
        /// Gets the name of the current info zone at this position.
        /// </summary>
        /// <param name="pos">The position to get the info zone name of.</param>
        /// <returns>The name of the info zone.</returns>
        public static string GetInfoZoneName(Vector3 pos)
        {
            return GET_NAME_OF_INFO_ZONE(pos.X, pos.Y, pos.Z);
        }
        /// <summary>
        /// This gets the street name at the given position.
        /// </summary>
        /// <param name="pos">The target position to get the street name from.</param>
        /// <returns>The street name.</returns>
        public static string GetStreetName(Vector3 pos)
        {
            FIND_STREET_NAME_AT_POSITION(pos.X, pos.Y, pos.Z, out uint hash1, out uint hash2);
            string str1 = GET_STRING_FROM_HASH_KEY(hash1).Trim();
            string str2 = GET_STRING_FROM_HASH_KEY(hash2).Trim();

            if (string.IsNullOrEmpty(str1))
                return str2;
            if (string.IsNullOrEmpty(str2))
                return str1;

            return string.Concat(str1, ", ", str2);
        }
        /// <summary>
        /// This gets the street name at the given position.
        /// </summary>
        /// <param name="pos">The target position to get the street name from.</param>
        /// <param name="name1">Not guaranteed to have something in it.</param>
        /// <returns>Guaranteed to return something.</returns>
        public static string GetStreetName(Vector3 pos, out string name1)
        {
            FIND_STREET_NAME_AT_POSITION(pos.X, pos.Y, pos.Z, out uint hash1, out uint hash2);
            name1 = GET_STRING_FROM_HASH_KEY(hash2).Trim();
            return GET_STRING_FROM_HASH_KEY(hash1).Trim();
        }

        private static float GetGroundZBelow(float x, float y, float z)
        {
            GET_GROUND_Z_FOR_3D_COORD(x, y, z, out float gZ);
            return gZ;
        }
        private static float GetGroundZAbove(float x, float y, float z)
        {
            if (z < 0.0f) 
                z = 0.0f;

            float lastZ, resZ;
            for (int i = 0; i <= 10; i++)
            {
                lastZ = z + (float)Math.Pow(2.0, i);
                resZ = GetGroundZBelow(x, y, lastZ);
                if ((resZ < lastZ) && (resZ > z)) return resZ;
            }

            return z;
        }
        private static float GetGroundZNext(float x, float y, float z)
        {
            if (z < 0.0f)
                z = 0.0f;

            float lastZ, resZ;
            for (int i = 0; i <= 10; i++)
            {
                lastZ = z + (float)Math.Pow(2.0, i);
                resZ = GetGroundZBelow(x, y, lastZ);
                if ((resZ < lastZ) && (resZ > 0.0f)) return resZ;
            }

            return z;
        }
        public static float GetGroundZ(Vector3 pos, GroundType groundType)
        {
            switch (groundType)
            {
                case GroundType.Highest:            return GetGroundZBelow(pos.X, pos.Y, 1024.0f);
                case GroundType.Lowest:             return GetGroundZAbove(pos.X, pos.Y, 0f);
                case GroundType.NextBelowCurrent:   return GetGroundZBelow(pos.X, pos.Y, pos.Z);
                case GroundType.NextAboveCurrent:   return GetGroundZAbove(pos.X, pos.Y, pos.Z);

                // Closest
                default: return GetGroundZNext(pos.X, pos.Y, pos.Z);
            }
        }
        public static float GetGroundZ(Vector3 pos)
        {
            return GetGroundZ(pos, GroundType.Closest);
        }

        public static Vector3 GetGroundPosition(Vector3 pos, GroundType groundType)
        {
            float z = GetGroundZ(pos, groundType);
            return new Vector3(pos.X, pos.Y, z);
        }
        public static Vector3 GetGroundPosition(Vector3 pos)
        {
            return GetGroundPosition(pos, GroundType.Closest);
        }

        public static Vector3 GetPositionAround(Vector3 pos, float distance)
        {
            return pos + Vector3.Zero.RandomXY() * distance;
        }

        /// <summary>
        /// Gets the next position on street at the given <paramref name="pos"/>.
        /// </summary>
        /// <param name="pos">The position to get the next street position from.</param>
        /// <param name="radius">The radius to check for if the new position if obscured by a mission entity. If the position is obscured, this function will check for the next nearest position on street.</param>
        /// <returns>The position.</returns>
        public static Vector3 GetPositionOnStreet(Vector3 pos, float radius = 5f)
        {
            for (uint i = 1; i < 40; i++)
            {
                GET_NTH_CLOSEST_CAR_NODE_WITH_HEADING(pos, i, out Vector3 newPos, out float h);
                if (!IS_POINT_OBSCURED_BY_A_MISSION_ENTITY(newPos, new Vector3(radius)))
                    return newPos;
            }

            return Vector3.Zero;
        }

        /// <summary>
        /// Checks if the given position is visible on screen.
        /// </summary>
        /// <param name="pos">The position to check for.</param>
        /// <param name="radius">Unknown.</param>
        /// <returns><see langword="true"/> if visible. Otherwise, <see langword="false"/>.</returns>
        public static bool IsPositionVisibleOnScreen(Vector3 pos, float radius = 5f)
        {
            GET_GAME_VIEWPORT_ID(out int viewportId);
            return CAM_IS_SPHERE_VISIBLE(viewportId, pos, radius);
        }
        #endregion

    }
}
