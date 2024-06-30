using System;
using System.Drawing;
using System.Numerics;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CCL.GTAIV
{
    /// <summary>
    /// Gives you easy access to native functions that involve checkpoints.
    /// </summary>
    public class NativeCheckpoint
    {

        #region Variables and Properties
        // Variables
        private Script parent;
        private bool _visible;
        private Vector3 _position;
        private float _radius;
        private Color _color;

        // Properties
        /// <summary>
        /// Gets or sets if this checkpoint should be visible.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        /// <summary>
        /// Gets or sets the position of this checkpoint.
        /// </summary>
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        /// <summary>
        /// Gets or sets the radius of this checkpoint.
        /// </summary>
        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }
        /// <summary>
        /// Gets or sets the color of this checkpoint.
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        #endregion

        #region Constructor / Destructor
        /// <summary>
        /// Creates a new checkpoint.
        /// </summary>
        /// <param name="createFor">The script needed for drawing this checkpoint.</param>
        /// <param name="pos">The position of this checkpoint.</param>
        /// <param name="radius">The radius of this checkpoint.</param>
        /// <param name="color">The color of this checkpoint.</param>
        /// <exception cref="ArgumentNullException">Thrown when the createFor parameter is null.</exception>
        public NativeCheckpoint(Script createFor, Vector3 pos, float radius, Color color)
        {
            if (createFor == null)
                throw new ArgumentNullException("createFor");

            parent = createFor;
            Visible = false;
            Position = pos;
            Radius = radius;
            Color = color;

            parent.Tick += Parent_Tick;
        }
        /// <summary>
        /// Creates a new checkpoint.
        /// </summary>
        /// <param name="createFor">The script needed for drawing this checkpoint.</param>
        /// <exception cref="ArgumentNullException">Thrown when the createFor parameter is null.</exception>
        public NativeCheckpoint(Script createFor)
        {
            if (createFor == null)
                throw new ArgumentNullException("createFor");

            parent = createFor;
            Visible = false;
            Position = Vector3.Zero;
            Radius = 1f;
            Color = Color.White;

            parent.Tick += Parent_Tick;
        }

        ~NativeCheckpoint()
        {
            if (parent != null)
                parent.Tick -= Parent_Tick;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Places this checkpoint on the ground properly.
        /// <para>Does only work properly if the area is loaded.</para>
        /// </summary>
        public void PlaceOnGroundProperly()
        {
            GET_GROUND_Z_FOR_3D_COORD(Position, out float z);
            Position = new Vector3(Position.X, Position.Y, z);
        }

        // Statics
        /// <summary>
        /// Draws a checkpoint at the given coords, the given radius and color in the world.
        /// </summary>
        /// <param name="pos">The position where to draw the checkpoint.</param>
        /// <param name="radius">The radius of the checkpoint.</param>
        /// <param name="color">The color of the checkpoint.</param>
        public static void DrawCheckpoint(Vector3 pos, float radius, Color color)
        {
            DRAW_CHECKPOINT_WITH_ALPHA(pos, radius, color);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Checks if the given <see cref="IVVehicle"/> is inside this checkpoint.
        /// </summary>
        /// <param name="veh">The given <see cref="IVVehicle"/> to check for.</param>
        /// <param name="ignoreZCoordinate">Sets if the Z coordinate of the given <see cref="IVVehicle"/> should be ignored. If set to <see langword="true"/>, this function will return <see langword="true"/> when the <see cref="IVVehicle"/> is inside the checkpoint no matter the current height.</param>
        /// <returns>True if the given <see cref="IVVehicle"/> is inside this checkpoint. Otherwise, false.</returns>
        public bool IsInside(IVVehicle veh, bool ignoreZCoordinate = false)
        {
            if (!Visible)
                return false;
            if (veh == null)
                return false;

            Vector3 pos = veh.Matrix.Pos;

            if ((pos.Z > (Position.Z + 5f)) && !ignoreZCoordinate)
                return false;

            return Vector2.Distance(new Vector2(pos.X, pos.Y), new Vector2(Position.X, Position.Y)) < Radius / 2f;
        }
        /// <summary>
        /// Checks if the given <see cref="IVPed"/> is inside this checkpoint.
        /// </summary>
        /// <param name="ped">The given <see cref="IVPed"/> to check for.</param>
        /// <param name="ignoreZCoordinate">Sets if the Z coordinate of the given <see cref="IVPed"/> should be ignored. If set to <see langword="true"/>, this function will return <see langword="true"/> when the <see cref="IVPed"/> is inside the checkpoint no matter the current height.</param>
        /// <returns>True if the given <see cref="IVPed"/> is inside this checkpoint. Otherwise, false.</returns>
        public bool IsInside(IVPed ped, bool ignoreZCoordinate = false)
        {
            if (!Visible)
                return false;
            if (ped == null)
                return false;

            Vector3 pos = ped.Matrix.Pos;

            if ((pos.Z > (Position.Z + 5f)) && !ignoreZCoordinate)
                return false;

            return Vector2.Distance(new Vector2(pos.X, pos.Y), new Vector2(Position.X, Position.Y)) < Radius / 2f;
        }
        /// <summary>
        /// Checks if the given position is inside this checkpoint.
        /// </summary>
        /// <param name="pos">The given position to check for.</param>
        /// <param name="ignoreZCoordinate">Sets if the Z coordinate of the given <see cref="Vector3"/> should be ignored. If set to <see langword="true"/>, this function will return <see langword="true"/> when the <see cref="Vector3"/> is inside the checkpoint no matter the current height.</param>
        /// <returns>True if the given position is inside this checkpoint. Otherwise, false.</returns>
        public bool IsInside(Vector3 pos, bool ignoreZCoordinate = false)
        {
            if (!Visible)
                return false;
            if ((pos.Z > (Position.Z + 5f)) && !ignoreZCoordinate)
                return false;

            return Vector2.Distance(new Vector2(pos.X, pos.Y), new Vector2(Position.X, Position.Y)) < Radius / 2f;
        }
        #endregion

        private void Parent_Tick(object sender, EventArgs e)
        {
            if (Visible)
                DRAW_CHECKPOINT_WITH_ALPHA(Position, Radius, Color);
        }

    }
}
