using System;
using System.Numerics;

using IVSDKDotNet;
using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Gives you easy access to native functions that involve models.
    /// </summary>
    public struct NativeModel
    {

        #region Variables and Properties
        // Variables
        private uint _hash;
        private string _modelName;

        // Properties
        public uint Hash
        {
            get
            {
                return _hash;
            }
            private set
            {
                _hash = value;
            }
        }

        public bool IsInMemory
        {
            get
            {
                if (!IsValid())
                    return false;

                return HAS_MODEL_LOADED((int)Hash);
            }
        }
        public bool IsCollisionDataInMemory
        {
            get
            {
                if (!IsValid())
                    return false;

                return HAS_COLLISION_FOR_MODEL_LOADED(Hash);
            }
        }

        public bool IsBike
        {
            get
            {
                if (!IsValid())
                    return false;
                if (!IsInMemory)
                    return false;

                return IS_THIS_MODEL_A_BIKE(Hash);
            }
        }
        public bool IsBoat
        {
            get
            {
                if (!IsValid())
                    return false;
                if (!IsInMemory)
                    return false;

                return IS_THIS_MODEL_A_BOAT(Hash);
            }
        }
        public bool IsCar
        {
            get
            {
                if (!IsValid())
                    return false;
                if (!IsInMemory)
                    return false;

                return IS_THIS_MODEL_A_CAR(Hash);
            }
        }
        public bool IsHelicopter
        {
            get
            {
                if (!IsValid())
                    return false;
                if (!IsInMemory)
                    return false;

                return IS_THIS_MODEL_A_HELI(Hash);
            }
        }
        public bool IsPed
        {
            get
            {
                if (!IsValid())
                    return false;
                if (!IsInMemory)
                    return false;

                return IS_THIS_MODEL_A_PED(Hash);
            }
        }
        public bool IsPlane
        {
            get
            {
                if (!IsValid())
                    return false;
                if (!IsInMemory)
                    return false;

                return IS_THIS_MODEL_A_PLANE(Hash);
            }
        }
        public bool IsTrain
        {
            get
            {
                if (!IsValid())
                    return false;
                if (!IsInMemory)
                    return false;

                return IS_THIS_MODEL_A_TRAIN(Hash);
            }
        }
        public bool IsVehicle
        {
            get
            {
                if (!IsValid())
                    return false;
                if (!IsInMemory)
                    return false;

                return IS_THIS_MODEL_A_VEHICLE(Hash);
            }
        }
        #endregion

        #region Constructor
        public NativeModel(int hash)
        {
            _modelName = null;
            _hash = (uint)hash;
        }
        public NativeModel(uint hash)
        {
            _modelName = null;
            _hash = hash;
        }
        public NativeModel(string modelName)
        {
            if (string.IsNullOrWhiteSpace(modelName))
            {
                _modelName = null;
                _hash = 0;
                return;
            }

            _modelName = modelName;
            _hash = RAGE.AtStringHash(modelName);
        }
        #endregion

        #region Methods
        public void LoadToMemory()
        {
            if (!IsValid())
                return;

            REQUEST_MODEL((int)Hash);
        }
        public void LoadCollisionDataToMemory()
        {
            if (!IsValid())
                return;

            REQUEST_COLLISION_FOR_MODEL(Hash);
        }

        public void MarkAsNoLongerNeeded()
        {
            if (!IsValid())
                return;

            MARK_MODEL_AS_NO_LONGER_NEEDED((int)Hash);
        }

        public void GetDimensions(out Vector3 min, out Vector3 max)
        {
            if (!IsValid())
            {
                min = default;
                max = default;
                return;
            }
            if (!IsInMemory)
            {
                min = default;
                max = default;
                return;
            }

            GET_MODEL_DIMENSIONS(Hash, out min, out max);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Checks if the model is valid (<see cref="Hash"/> is not 0).
        /// </summary>
        /// <returns>True if the model is valid. Otherwise, false.</returns>
        public bool IsValid()
        {
            return Hash != 0;
        }

        public bool LoadToMemoryNow()
        {
            if (!IsValid())
                return false;

            IVStreaming.ScriptRequestModel((int)Hash);

            if (IsInMemory)
                return true;

            IVStreaming.LoadAllRequestedModels(false);

            return IsInMemory;
        }
        public bool LoadCollisionDataToMemoryNow()
        {
            if (!IsValid())
                return false;

            // First try
            REQUEST_COLLISION_FOR_MODEL(Hash);

            if (IsCollisionDataInMemory)
                return true;

            // Second try
            REQUEST_COLLISION_FOR_MODEL(Hash);
            return IsCollisionDataInMemory;
        }

        public Vector3 GetDimensions()
        {
            if (!IsValid())
                return Vector3.Zero;
            if (!IsInMemory)
                return Vector3.Zero;

            GET_MODEL_DIMENSIONS(Hash, out Vector3 min, out Vector3 max);
            return new Vector3(max.X - min.X, max.Y - min.Y, max.Z - min.Z);
        }

        // Static stuff
        public static NativeModel GetBasicCopModel()
        {
            GET_CURRENT_BASIC_COP_MODEL(out uint model);
            return new NativeModel(model);
        }
        public static NativeModel GetCurrentCopModel()
        {
            GET_CURRENT_COP_MODEL(out uint model);
            return new NativeModel(model);
        }
        public static NativeModel GetBasicPoliceCarModel()
        {
            GET_CURRENT_BASIC_POLICE_CAR_MODEL(out uint model);
            return new NativeModel(model);
        }
        public static NativeModel GetCurrentPoliceCarModel()
        {
            GET_CURRENT_POLICE_CAR_MODEL(out uint model);
            return new NativeModel(model);
        }
        public static NativeModel GetTaxiCarModel()
        {
            GET_CURRENT_TAXI_CAR_MODEL(out uint model);
            return new NativeModel(model);
        }

        public static NativeModel GetWeaponModel(eWeaponType weaponType)
        {
            GET_WEAPONTYPE_MODEL((int)weaponType, out uint model);
            return new NativeModel(model);
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_modelName))
                return "0x" + Helper.ToHex((int)Hash);
            else
                return _modelName;
        }
        #endregion

    }
}
