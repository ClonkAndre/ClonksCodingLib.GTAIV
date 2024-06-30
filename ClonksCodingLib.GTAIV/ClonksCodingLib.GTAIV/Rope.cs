using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;

using CCL.GTAIV.Extensions;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Allows you to create a <see cref="Rope"/> within the game world.
    /// <para>
    /// <b>WARNING:</b> For this to work correctly, you need to make sure you have the model "cj_e1_rope" in GTA IV because it does not exist there.<br/>
    /// It does exists in TLAD and TBOGT. In TBOGT this model is also called "<b>cj_e1_rope</b>".<br/>
    /// You can also change the model if you like, but this is basically the best rope model you can find and it's from Rockstar.
    /// </para>
    /// </summary>
    public class Rope
    {

        #region Variables and Properties
        // Variables
        private Vector3 _startingPosition;
        private List<IVObject> ropeObjects;
        private ReadOnlyCollection<IVObject> readonlyRopeObjects;

        // Model
        private uint _ropeModel;
        private Vector3 _ropeSize;

        // Attach stuff
        private bool _isAttached;
        private AttachedTo _attachedTo;
        private int _attachedObjectHandle;
        private Vector3 _attachOffset;

        // Animation
        private RopeDeploymentState _deploymentState;
        private float _deploymentAnimEasingValue = 1.5f;
        private float _deploymentTime;

        private float _windTime;
        private float _windFrequency = 0.015f;
        private float _windAmplitude = 0.025f;

        // Other
        private bool _showRopePoints;

        // Properties
        /// <summary>
        /// A collection of all current objects that make up the rope.
        /// </summary>
        public ReadOnlyCollection<IVObject> RopeObjects
        {
            get
            {
                return readonlyRopeObjects;
            }
        }

        /// <summary>
        /// Gets the model hash of the model that this <see cref="Rope"/> was created with.
        /// </summary>
        public uint RopeModel
        {
            get
            {
                return _ropeModel;
            }
            private set
            {
                _ropeModel = value;
            }
        }
        /// <summary>
        /// Gets the size of the rope model this <see cref="Rope"/> was created with.
        /// </summary>
        public Vector3 RopeModelSize
        {
            get
            {
                return _ropeSize;
            }
        }

        /// <summary>
        /// Gets if this <see cref="Rope"/> is attached somewhere.
        /// </summary>
        public bool IsAttached
        {
            get
            {
                return _isAttached;
            }
            private set
            {
                _isAttached = value;
            }
        }
        /// <summary>
        /// Gets or sets the attachment offset.
        /// </summary>
        public Vector3 AttachOffset
        {
            get
            {
                return _attachOffset;
            }
            set
            {
                _attachOffset = value;
            }
        }

        /// <summary>
        /// Gets the currently deployment state of the <see cref="Rope"/>.
        /// <para>
        /// 
        /// - <see cref="RopeDeploymentState.Instant"/><br/>
        /// This basically means that the <see cref="Rope"/> instantly reached the ground without using any animation which makes it look like it is being deployed from a helicopter or something.
        /// 
        /// </para>
        /// <para>
        /// 
        /// - <see cref="RopeDeploymentState.Deploying"/><br/>
        /// This is set when the <see cref="Rope"/> is currently playing its deployment animation from the starting point to the ground.<br/>
        /// Will be <see cref="RopeDeploymentState.Deployed"/> if the animation is done.
        /// 
        /// </para>
        /// <para>
        /// 
        /// - <see cref="RopeDeploymentState.Undeploying"/><br/>
        /// This is set when the <see cref="Rope"/> is currently playing its undeployment animation from the ground back to the starting point.<br/>
        /// Will be <see cref="RopeDeploymentState.Undeployed"/> if the animation is done.
        /// 
        /// </para>
        /// </summary>
        public RopeDeploymentState CurrentDeploymentState
        {
            get
            {
                return _deploymentState;
            }
            private set
            {
                _deploymentState = value;
            }
        }
        /// <summary>
        /// Gets or sets the easing value for the <see cref="Rope"/> animations.
        /// </summary>
        public float DeploymentAnimEasingValue
        {
            get
            {
                return _deploymentAnimEasingValue;
            }
            set
            {
                _deploymentAnimEasingValue = value;
            }
        }

        /// <summary>
        /// Gets or set the frequency of the wind effect for the <see cref="Rope"/>.
        /// <para>The default is: 0.015f</para>
        /// </summary>
        public float WindFrequency
        {
            get
            {
                return _windFrequency;
            }
            set
            {
                _windFrequency = value;
            }
        }
        /// <summary>
        /// Gets or set the amplitude of the wind effect for the <see cref="Rope"/>.
        /// <para>The default is: 0.025f</para>
        /// </summary>
        public float WindAmplitude
        {
            get
            {
                return _windAmplitude;
            }
            set
            {
                _windAmplitude = value;
            }
        }

        /// <summary>
        /// This visually shows where all the rope objects are connected to each other. Useful for debugging.
        /// </summary>
        public bool ShowRopePoints
        {
            get
            {
                return _showRopePoints;
            }
            set
            {
                _showRopePoints = value;
            }
        }
        #endregion

        #region Constructor
        private Rope(Vector3 startingPos)
        {
            _startingPosition = startingPos;
            ropeObjects = new List<IVObject>();
        }
        #endregion

        #region Methods

        // Private stuff
        private void LoadModel(uint ropeModel)
        {
            RopeModel = ropeModel;

            if (!HAS_MODEL_LOADED((int)RopeModel))
            {
                IVStreaming.ScriptRequestModel((int)RopeModel);
                IVStreaming.LoadAllRequestedModels(false);
            }

            GET_MODEL_DIMENSIONS(RopeModel, out Vector3 minVec, out _ropeSize);
        }
        private void CreateReadonlyCollection()
        {
            readonlyRopeObjects = new ReadOnlyCollection<IVObject>(ropeObjects);
        }

        // Attach
        /// <summary>
        /// This attaches the <see cref="Rope"/> to the given vehicle.
        /// </summary>
        /// <param name="handle">The handle of the vehicle to attach the <see cref="Rope"/> to.</param>
        public void AttachToVehicle(int handle)
        {
            if (handle <= 0)
                return;

            _attachedTo = AttachedTo.Vehicle;
            _attachedObjectHandle = handle;
            IsAttached = true;
        }
        /// <summary>
        /// This attaches the <see cref="Rope"/> to the given ped.
        /// </summary>
        /// <param name="handle">The handle of the ped to attach the <see cref="Rope"/> to.</param>
        public void AttachToPed(int handle)
        {
            if (handle <= 0)
                return;

            _attachedTo = AttachedTo.Ped;
            _attachedObjectHandle = handle;
            IsAttached = true;
        }
        /// <summary>
        /// Detaches the <see cref="Rope"/> from everything.
        /// </summary>
        public void Detach()
        {
            IsAttached = false;
            _attachedTo = AttachedTo.Nothing;
            _attachedObjectHandle = 0;
        }

        // Other
        /// <summary>
        /// Deletes this <see cref="Rope"/>.
        /// </summary>
        public void Delete()
        {
            for (int i = 0; i < ropeObjects.Count; i++)
            {
                IVObject obj = ropeObjects[i];

                int handle = obj.GetHandle();
                MARK_OBJECT_AS_NO_LONGER_NEEDED(handle);
                DELETE_OBJECT(ref handle);

                ropeObjects.RemoveAt(i);
                i--;
            }
        }

        /// <summary>
        /// Starts the deployment animation which makes it look like the <see cref="Rope"/> is currently being deployed from a Helicopter.
        /// <para>
        /// Sets the <see cref="Rope.CurrentDeploymentState"/> to <see cref="RopeDeploymentState.Deploying"/>.
        /// </para>
        /// </summary>
        public void StartDeploying()
        {
            CurrentDeploymentState = RopeDeploymentState.Deploying;
        }
        /// <summary>
        /// Starts the undeployment animation which makes it look like the <see cref="Rope"/> is currently being undeployed from a Helicopter.
        /// <para>
        /// Sets the <see cref="Rope.CurrentDeploymentState"/> to <see cref="RopeDeploymentState.Undeploying"/>.
        /// </para>
        /// </summary>
        public void StartUndeploying()
        {
            CurrentDeploymentState = RopeDeploymentState.Undeploying;
        }

        /// <summary>
        /// Processes all the rope logic like movement, animation and wind effect.
        /// </summary>
        /// <param name="overridePos">If you want to override the position the rope was originally created at you can do this here.</param>
        public void Process(Vector3 overridePos = default)
        {
            // If rope is attached somewhere then get the position of the attached object
            if (IsAttached)
            {
                switch (_attachedTo)
                {
                    case AttachedTo.Vehicle:
                        {
                            if (!DOES_VEHICLE_EXIST(_attachedObjectHandle))
                                return;

                            GET_CAR_COORDINATES(_attachedObjectHandle, out overridePos);
                        }
                        break;
                    case AttachedTo.Ped:
                        {
                            if (!DOES_CHAR_EXIST(_attachedObjectHandle))
                                return;

                            GET_CHAR_COORDINATES(_attachedObjectHandle, out overridePos);
                        }
                        break;
                }
            }

            // Increase wind time for wind effect
            _windTime += WindFrequency;

            // Increase/Decrease value for deployment animation
            switch (CurrentDeploymentState)
            {
                case RopeDeploymentState.Deploying:
                    _deploymentTime += IVTimer.TimeStep;
                    break;
                case RopeDeploymentState.Undeploying:
                    _deploymentTime -= IVTimer.TimeStep;
                    break;
            }

            if (_deploymentTime > 1.0f)
                _deploymentTime = 1.0f;
            if (_deploymentTime < 0.0f)
                _deploymentTime = 0.0f;

            Vector3 firstRopePos = _startingPosition;

            if (overridePos != default)
                firstRopePos = overridePos;

            // Go through all rope objects and update their position and wind effect
            for (int i = 0; i < ropeObjects.Count; i++)
            {
                IVObject rope = ropeObjects[i];

                if (ShowRopePoints)
                    DRAW_CORONA(rope.Matrix.Pos, 40f, 0, 0, Color.Red);

                // Add a swinging in wind effect
                float windOffsetX = (float)Math.Sin(_windTime) * i * WindAmplitude;
                float windOffsetY = (float)Math.Cos(_windTime) * i * WindAmplitude;
                Vector3 windEffect = new Vector3(windOffsetX, windOffsetY, 0f);

                // Calculate position of rope
                Vector3 targetPosition = new Vector3(firstRopePos.X, firstRopePos.Y, (firstRopePos.Z - RopeModelSize.Z) - RopeModelSize.Z * i) + windEffect + AttachOffset;

                // Set position of rope
                switch (CurrentDeploymentState)
                {
                    case RopeDeploymentState.Instant:
                    case RopeDeploymentState.Deployed:
                        SET_OBJECT_COORDINATES(rope.GetHandle(), Vector3.Lerp(firstRopePos - new Vector3(0f, 0f, RopeModelSize.Z), targetPosition, 1f));
                        break;
                    case RopeDeploymentState.Undeployed:
                        SET_OBJECT_COORDINATES(rope.GetHandle(), Vector3.Lerp(firstRopePos - new Vector3(0f, 0f, RopeModelSize.Z), targetPosition, 0f));
                        break;

                    case RopeDeploymentState.Deploying:
                        SET_OBJECT_COORDINATES(rope.GetHandle(), Vector3.Lerp(firstRopePos - new Vector3(0f, 0f, RopeModelSize.Z), targetPosition + AttachOffset, _deploymentTime.EaseOut(DeploymentAnimEasingValue)));
                        break;
                    case RopeDeploymentState.Undeploying:
                        SET_OBJECT_COORDINATES(rope.GetHandle(), Vector3.Lerp(firstRopePos - new Vector3(0f, 0f, RopeModelSize.Z), targetPosition + AttachOffset, _deploymentTime.EaseIn(DeploymentAnimEasingValue)));
                        break;
                }
            }

            // Check and change deployment state
            switch (CurrentDeploymentState)
            {
                case RopeDeploymentState.Deploying:
                    {
                        // Check if last rope object reached its target pos
                        Vector3 targetPosition = new Vector3(firstRopePos.X, firstRopePos.Y, (firstRopePos.Z - RopeModelSize.Z) - RopeModelSize.Z * (ropeObjects.Count - 1)) + AttachOffset;

                        if (Vector3.Distance(ropeObjects[ropeObjects.Count - 1].Matrix.Pos, targetPosition) <= 0.15f)
                            CurrentDeploymentState = RopeDeploymentState.Deployed;
                    }
                    break;
                case RopeDeploymentState.Undeploying:
                    {
                        // Check if last rope object reached its target pos
                        Vector3 targetPosition = new Vector3(firstRopePos.X, firstRopePos.Y, (firstRopePos.Z - RopeModelSize.Z) - RopeModelSize.Z * 0) + AttachOffset;

                        if (Vector3.Distance(ropeObjects[ropeObjects.Count - 1].Matrix.Pos, targetPosition) <= 0.1f)
                            CurrentDeploymentState = RopeDeploymentState.Undeployed;
                    }
                    break;
            }

        }

        #endregion

        #region Functions

        // Attach
        /// <summary>
        /// Gets information about what the <see cref="Rope"/> is currently attached to.
        /// </summary>
        /// <param name="attachedTo">To what the <see cref="Rope"/> is currently attached to.</param>
        /// <param name="attachedObjectHandle">The handle of the entity the <see cref="Rope"/> is currently attached to.</param>
        /// <returns><see langword="true"/> if the <see cref="Rope"/> is currently attached somewhere. Otherwise, <see langword="false"/>.</returns>
        public bool GetAttachmentInfo(out AttachedTo attachedTo, out int attachedObjectHandle)
        {
            if (!IsAttached)
            {
                attachedTo = AttachedTo.Nothing;
                attachedObjectHandle = 0;
                return false;
            }

            attachedTo = _attachedTo;
            attachedObjectHandle = _attachedObjectHandle;
            return true;
        }

        // Other
        /// <summary>
        /// Gets the starting position of the first rope object.
        /// </summary>
        /// <returns>The starting position of the first rope object.</returns>
        public Vector3 GetRopeStartingPos()
        {
            if (ropeObjects.Count == 0)
                return Vector3.Zero;

            return ropeObjects[0].Matrix.Pos + new Vector3(0f, 0f, RopeModelSize.Z);
        }
        /// <summary>
        /// Gets the position of the last rope object.
        /// </summary>
        /// <returns>The position of the last rope object.</returns>
        public Vector3 GetRopeEndPos()
        {
            if (ropeObjects.Count == 0)
                return Vector3.Zero;

            return ropeObjects[ropeObjects.Count - 1].Matrix.Pos + new Vector3(0f, 0f, RopeModelSize.Z);
        }

        public IVObject GetFirstRopeObject()
        {
            if (ropeObjects.Count == 0)
                return null;

            return ropeObjects[0];
        }
        public IVObject GetLastRopeObject()
        {
            if (ropeObjects.Count == 0)
                return null;

            return ropeObjects[ropeObjects.Count - 1];
        }

        #endregion

        /// <summary>
        /// Creates a new <see cref="Rope"/> in the world.
        /// </summary>
        /// <param name="startPos">The position where to spawn the <see cref="Rope"/>.</param>
        /// <param name="startingState">Determines the starting state of the <see cref="Rope"/>. Default is <see cref="RopeDeploymentState.Instant"/>, which means the <see cref="Rope"/> instantly reached the ground without using any animations.</param>
        /// <param name="model">The model to use for the <see cref="Rope"/>. Default and recommended is "cj_e1_rope" WHICH NEEDS TO EXIST IN YOUR GAME! OTHERWISE THIS WILL ALWAYS RETURN NULL!</param>
        /// <returns>
        /// The created <see cref="Rope"/> if successful.
        /// <para>
        /// Otherwise <see langword="null"/> if:<br/>
        /// - The given <paramref name="model"/> does not exist within the game.<br/>
        /// - The <paramref name="startPos"/> is below the ground position at this position.
        /// </para>
        /// </returns>
        /// <exception cref="Exception">Thrown when the given <paramref name="model"/> does not exists within the game.</exception>
        public static Rope Create(Vector3 startPos, RopeDeploymentState startingState = RopeDeploymentState.Instant, string model = "cj_e1_rope")
        {
            uint modelHash = RAGE.AtStringHash(model);
            
            if (!IS_MODEL_IN_CDIMAGE((int)modelHash))
                throw new Exception(string.Format("Failed to create rope. Model {0} does not exists in game.", model));

            Rope rope = new Rope(startPos);
            rope.LoadModel(modelHash);

            // Get the ground position of startPos
            startPos = startPos - new Vector3(0f, 0f, rope.RopeModelSize.Z);
            float groundPos = NativeWorld.GetGroundZ(startPos);

            // Do not create rope if the starting pos is below ground or if it is the same height as the ground pos
            if (startPos.Z < groundPos || startPos.Z == groundPos)
            {
                MARK_MODEL_AS_NO_LONGER_NEEDED((int)rope.RopeModel);
                rope = null;
                return null;
            }

            List<Vector3> ropeTargetPoints = new List<Vector3>();

            // Create rope objects until they reach the ground
            while (true)
            {
                // Create rope object
                CREATE_OBJECT((int)rope.RopeModel, startPos, out int ropeObjHandle, false);
                //SET_OBJECT_DYNAMIC(ropeObjHandle, true);

                // Increase the draw distance of the rope so it can bee seen from further away
                IVObject ropeObj = IVObject.FromUIntPtr(IVPools.GetObjectPool().GetAt((uint)ropeObjHandle));
                ropeObj.DrawDistance = 150f;

                // Calculate the position of the rope object
                Vector3 expectedPos = startPos;

                if (ropeTargetPoints.Count != 0)
                    // Get position of previous point and expand to ground
                    expectedPos = ropeTargetPoints[ropeTargetPoints.Count - 1] - new Vector3(0f, 0f, rope.RopeModelSize.Z);

                // Add the rope object
                rope.ropeObjects.Add(ropeObj);
                ropeTargetPoints.Add(expectedPos);

                // Check if point is below ground
                Vector3 lastRopePoint = ropeTargetPoints[ropeTargetPoints.Count - 1];

                if (lastRopePoint.Z <= groundPos)
                    break;
            }

            // Set state
            rope.CurrentDeploymentState = startingState;

            // Some cleanup
            MARK_MODEL_AS_NO_LONGER_NEEDED((int)rope.RopeModel);
            ropeTargetPoints.Clear();
            ropeTargetPoints = null;

            // Create readonly collection
            rope.CreateReadonlyCollection();

            return rope;
        }

    }
}
