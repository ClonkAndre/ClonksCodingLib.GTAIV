using System;
using System.Numerics;

using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{

    // SET_CAM_BEHIND_PED
    // SET_CAM_IN_FRONT_OF_PED
    // POINT_FIXED_CAM_AT_OBJ
    // POINT_FIXED_CAM_AT_VEHICLE
    // POINT_FIXED_CAM_AT_POS
    // POINT_FIXED_CAM_AT_PED
    // SET_CAM_POINT_DAMPING_PARAMS
    // IS_DEBUG_CAMERA_ON
    // SET_GAME_CAM_HEADING
    // SET_CAMERA_STATE
    // DESTROY_ALL_CAMS
    // IS_CAM_INTERPOLATING

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

    /// <summary>
    /// Gives you access to native functions that involve the camera.
    /// </summary>
    public class NativeCamera : HandleObject
    {
        #region Properties
        /// <summary>
        /// Gets or sets the position of the camera.
        /// </summary>
        public Vector3 Position
        {
            get {
                if (!IsValid)
                    return Vector3.Zero;

                GET_CAM_POS(Handle, out Vector3 pos);
                return pos;
            }
            set {
                if (!IsValid)
                    return;
                
                SET_CAM_POS(Handle, value.X, value.Y, value.Z);
            }
        }
        /// <summary>
        /// Sets the attach offset position of the camera.
        /// </summary>
        public Vector3 Offset
        {
            set {
                if (!IsValid)
                    return;

                SET_CAM_ATTACH_OFFSET(Handle, value.X, value.Y, value.Z);
            }
        }
        /// <summary>
        /// Gets or sets the rotation of the camera.
        /// </summary>
        public Vector3 Rotation
        {
            get {
                if (!IsValid)
                    return Vector3.Zero;

                GET_CAM_ROT(Handle, out Vector3 pos);
                return pos;
            }
            set {
                if (!IsValid)
                    return;

                SET_CAM_ROT(Handle, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the FOV of the camera.
        /// </summary>
        public float FOV
        {
            get {
                if (!IsValid)
                    return 0f;
                
                GET_CAM_FOV(Handle, out float fov);
                return fov;
            }
            set {
                if (!IsValid)
                    return;

                SET_CAM_FOV(Handle, value);
            }
        }
        /// <summary>
        /// Gets or sets near clip of the camera.
        /// </summary>
        public float NearClip
        {
            get {
                if (!IsValid)
                    return 0f;

                GET_CAM_NEAR_CLIP(Handle, out float nearClip);
                return nearClip;
            }
            set {
                if (!IsValid)
                    return;

                SET_CAM_NEAR_CLIP(Handle, value);
            }
        }

        /// <summary>
        /// Gets or sets the far DoF of the camera.
        /// </summary>
        public float FarDoF
        {
            get {
                if (!IsValid)
                    return 0f;

                GET_CAM_FAR_DOF(Handle, out float farDoF);
                return farDoF;
            }
            set {
                if (!IsValid)
                    return;

                SET_CAM_FAR_DOF(Handle, value);
            }
        }
        /// <summary>
        /// Gets or sets the near DoF of the camera.
        /// </summary>
        public float NearDoF
        {
            get {
                if (!IsValid)
                    return 0f;

                GET_CAM_NEAR_DOF(Handle, out float farDoF);
                return farDoF;
            }
            set {
                if (!IsValid)
                    return;

                SET_CAM_NEAR_DOF(Handle, value);
            }
        }

        /// <summary>
        /// Gets if the camera is active.
        /// </summary>
        public bool IsActive
        {
            get {
                if (!IsValid)
                    return false;

                return IS_CAM_ACTIVE(Handle);
            }
        }
        /// <summary>
        /// Gets or sets if the camera is propagating.
        /// </summary>
        public bool IsPropagating
        {
            get {
                if (!IsValid)
                    return false;

                return IS_CAM_PROPAGATING(Handle);
            }
            set {
                if (!IsValid)
                    return;

                SET_CAM_PROPAGATE(Handle, value);
            }
        }
        /// <summary>
        /// Sets if the <see cref="Offset"/> is relative.
        /// </summary>
        public bool AttachOffsetIsRelative
        {
            set {
                if (!IsValid)
                    return;

                SET_CAM_ATTACH_OFFSET_IS_RELATIVE(Handle, value);
            }
        }
        #endregion

        #region Constructor
        internal NativeCamera(int handle) : base(handle)
        {

        }
        #endregion

        #region Methods
        /// <inheritdoc/>
        public override void Dispose()
        {
            if (Exists())
                DESTROY_CAM(Handle);

            base.Dispose();
        }

        /// <summary>
        /// Activates the camera.
        /// </summary>
        public void Activate()
        {
            if (!IsValid)
                return;

            ACTIVATE_SCRIPTED_CAMS(true, true);
            SET_CAM_ACTIVE(Handle, true);
            IsPropagating = true;
        }
        /// <summary>
        /// Deactivates the camera.
        /// </summary>
        public void Deactivate()
        {
            if (!IsValid)
                return;

            SET_CAM_ACTIVE(Handle, false);
            IsPropagating = false;
            ACTIVATE_SCRIPTED_CAMS(false, false);
        }

        public void SetDoFFocusPoint(Vector3 pos, float unk)
        {
            if (!IsValid)
                return;

            SET_CAM_DOF_FOCUSPOINT(Handle, pos.X, pos.Y, pos.Z, unk);
        }
        public void SetTargetPed(int pedHandle)
        {
            if (!IsValid)
                return;

            SET_CAM_TARGET_PED(Handle, pedHandle);
        }

        /// <summary>
        /// Shakes the camera with the given attributes.
        /// </summary>
        /// <param name="shakeType">The shake type.</param>
        /// <param name="shakeBehaviour">The shake behaviour.</param>
        /// <param name="shakeDuration">The shake duration in milliseconds.</param>
        /// <param name="shakeAmplitude">The shake amplitude.</param>
        /// <param name="shakeFrequency">The shake frequency.</param>
        /// <param name="unknown1">Unknown. Usually 0.0f, Sometimes (rarely) up to 0.9f.</param>
        public void Shake(CameraShakeType shakeType, CameraShakeBehaviour shakeBehaviour, int shakeDuration, float shakeAmplitude, float shakeFrequency, float unknown1)
        {
            if (!IsValid)
                return;

            SET_CAM_COMPONENT_SHAKE(Handle, (int)shakeType, (int)shakeBehaviour, shakeDuration, shakeAmplitude, shakeFrequency, unknown1);
        }

        /// <summary>
        /// Points the camera at the given position.
        /// </summary>
        /// <param name="pos">The position to point at.</param>
        public void PointAtCoord(Vector3 pos)
        {
            if (!IsValid)
                return;

            POINT_CAM_AT_COORD(Handle, pos.X, pos.Y, pos.Z);
        }
        /// <summary>
        /// Points the camera at the given ped.
        /// </summary>
        /// <param name="pedHandle">The handle of the ped to point at.</param>
        public void PointAtPed(int pedHandle)
        {
            if (!IsValid)
                return;

            POINT_CAM_AT_PED(Handle, pedHandle);
        }
        /// <summary>
        /// Points the camera at the given vehicle.
        /// </summary>
        /// <param name="vehicleHandle">The handle of the vehicle to point at.</param>
        public void PointAtVehicle(int vehicleHandle)
        {
            if (!IsValid)
                return;

            POINT_CAM_AT_VEHICLE(Handle, vehicleHandle);
        }
        /// <summary>
        /// Points the camera at the given object.
        /// </summary>
        /// <param name="objectHandle">The handle of the object to point at.</param>
        public void PointAtObject(int objectHandle)
        {
            if (!IsValid)
                return;

            POINT_CAM_AT_OBJECT(Handle, objectHandle);
        }
        /// <summary>
        /// Points the camera at the given camera.
        /// </summary>
        /// <param name="camHandle">The handle of the camera to point at.</param>
        public void PointAtCam(int camHandle)
        {
            if (!IsValid)
                return;

            POINT_CAM_AT_CAM(Handle, camHandle);
        }
        /// <summary>
        /// Unpoints the camera.
        /// </summary>
        public void Unpoint()
        {
            if (!IsValid)
                return;
            
            UNPOINT_CAM(Handle);
        }

        /// <summary>
        /// Attaches the camera to the given vehicle.
        /// </summary>
        /// <param name="vehicleHandle">The handle of the vehicle.</param>
        public void AttachToVehicle(int vehicleHandle)
        {
            if (!IsValid)
                return;

            ATTACH_CAM_TO_VEHICLE(Handle, vehicleHandle);
        }
        /// <summary>
        /// Attaches the camera to the given object.
        /// </summary>
        /// <param name="objectHandle">The handle of the object.</param>
        public void AttachToObject(int objectHandle)
        {
            if (!IsValid)
                return;

            ATTACH_CAM_TO_OBJECT(Handle, objectHandle);
        }
        /// <summary>
        /// Attaches the camera to the given ped.
        /// </summary>
        /// <param name="pedHandle">The handle of the ped.</param>
        public void AttachToPed(int pedHandle)
        {
            if (!IsValid)
                return;

            ATTACH_CAM_TO_PED(Handle, pedHandle);
        }
        /// <summary>
        /// Attaches the camera to the given viewport.
        /// </summary>
        /// <param name="viewportHandle">The handle of the viewport.</param>
        public void AttachToViewport(int viewportHandle)
        {
            if (!IsValid)
                return;

            ATTACH_CAM_TO_VIEWPORT(Handle, viewportHandle);
        }
        /// <summary>
        /// Detaches the camera.
        /// </summary>
        public void Unattach()
        {
            if (!IsValid)
                return;

            UNATTACH_CAM(Handle);
        }

        // Statics
        public static void SetCameraControlsDisabledWithPlayerControls(bool value)
        {
            SET_CAMERA_CONTROLS_DISABLED_WITH_PLAYER_CONTROLS(value);
        }
        public static void SetGameCameraControlsActive(bool active)
        {
            SET_GAME_CAMERA_CONTROLS_ACTIVE(active);
        }
        public static void SetBlockCameraToggle(bool value)
        {
            SET_BLOCK_CAMERA_TOGGLE(value);
        }
        #endregion

        #region Functions
        /// <inheritdoc/>
        public override bool Exists()
        {
            if (!IsValid)
                return false;

            return DOES_CAM_EXIST(Handle);
        }

        /// <summary>
        /// Checks if the given position is visible on screen.
        /// </summary>
        /// <param name="pos">Target position.</param>
        /// <param name="radius">Unknown.</param>
        /// <returns>True if the position is visible. Otherwise, false.</returns>
        public bool IsSphereVisible(Vector3 pos, float radius)
        {
            if (!IsValid)
                return false;

            return CAM_IS_SPHERE_VISIBLE(Handle, pos, radius);
        }

        // Statics
        /// <summary>
        /// Creates a new camera.
        /// </summary>
        /// <param name="type">The camera type. Usually <see cref="eCamType.CAM_SCRIPTED"/>.</param>
        /// <returns>If successful, the newly created camera is returned. Otherwise, false.</returns>
        public static NativeCamera Create(eCamType type)
        {
            CREATE_CAM((uint)type, out int cam);

            if (cam != 0)
                return new NativeCamera(cam);

            return null;
        }
        /// <summary>
        /// Creates a new camera of type <see cref="eCamType.CAM_SCRIPTED"/>.
        /// </summary>
        /// <returns>If successful, the newly created camera is returned. Otherwise, false.</returns>
        public static NativeCamera Create()
        {
            CREATE_CAM((uint)eCamType.CAM_SCRIPTED, out int cam);

            if (cam != 0)
                return new NativeCamera(cam);

            return null;
        }

        /// <summary>
        /// Gets the game camera.
        /// </summary>
        /// <returns>If successful, the game camera is returned. Otherwise, false.</returns>
        public static NativeCamera GetGameCam()
        {
            GET_GAME_CAM(out int cam);

            if (cam != 0)
                return new NativeCamera(cam);

            return null;
        }
        /// <summary>
        /// Gets the games camera child.
        /// </summary>
        /// <returns>If successful, the games camera child is returned. Otherwise, false.</returns>
        public static NativeCamera GetGameCamChild()
        {
            GET_GAME_CAM_CHILD(out int cam);

            if (cam != 0)
                return new NativeCamera(cam);

            return null;
        }
        /// <summary>
        /// Gets the debug camera.
        /// </summary>
        /// <returns>If successful, the debug camera is returned. Otherwise, false.</returns>
        public static NativeCamera GetDebugCam()
        {
            GET_DEBUG_CAM(out int cam);

            if (cam != 0)
                return new NativeCamera(cam);

            return null;
        }
        /// <summary>
        /// Gets the cinematic camera.
        /// </summary>
        /// <returns>If successful, the cinematic camera is returned. Otherwise, false.</returns>
        public static NativeCamera GetCinematicCam()
        {
            GET_CINEMATIC_CAM(out int cam);

            if (cam != 0)
                return new NativeCamera(cam);

            return null;
        }
        #endregion
    }
}
