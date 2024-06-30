using System;
using System.Drawing;
using System.Numerics;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CCL.GTAIV
{
    /// <summary>
    /// Gives you easy access to native functions that involve lights.
    /// </summary>
    public class NativeLight
    {

        #region Variables and Properties
        // Variables
        private Script parent;
        private bool _visible;
        private Vector3 _position;
        private float _radius, _intensity;
        private Color _color;

        // Properties
        /// <summary>
        /// Gets or sets if this light should be visible.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        /// <summary>
        /// Gets or sets the position of this light.
        /// </summary>
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        /// <summary>
        /// Gets or sets the radius of this light.
        /// </summary>
        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }
        /// <summary>
        /// Gets or sets the intensity of this light.
        /// </summary>
        public float Intensity
        {
            get { return _intensity; }
            set { _intensity = value; }
        }
        /// <summary>
        /// Gets or sets the color of this light.
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        #endregion

        #region Constructor / Destructor
        /// <summary>
        /// Creates a new light.
        /// </summary>
        /// <param name="createFor">The script needed for drawing this light.</param>
        /// <param name="pos">The position of this light.</param>
        /// <param name="radius">The radius of this light.</param>
        /// <param name="intensity">The intensity of this light.</param>
        /// <param name="color">The color of this light.</param>
        /// <exception cref="ArgumentNullException">Thrown when the createFor parameter is null.</exception>
        public NativeLight(Script createFor, Vector3 pos, float radius, float intensity, Color color)
        {
            if (createFor == null)
                throw new ArgumentNullException("createFor");

            parent = createFor;
            Visible = false;
            Position = pos;
            Radius = radius;
            Intensity = intensity;
            Color = color;

            parent.Tick += Parent_Tick;
        }
        /// <summary>
        /// Creates a new light.
        /// </summary>
        /// <param name="createFor">The script needed for drawing this light.</param>
        /// <exception cref="ArgumentNullException">Thrown when the createFor parameter is null.</exception>
        public NativeLight(Script createFor)
        {
            if (createFor == null)
                throw new ArgumentNullException("createFor");

            parent = createFor;
            Visible = false;
            Position = Vector3.Zero;
            Radius = 3f;
            Intensity = 50f;
            Color = Color.White;

            parent.Tick += Parent_Tick;
        }

        ~NativeLight()
        {
            if (parent != null)
                parent.Tick -= Parent_Tick;
        }
        #endregion

        private void Parent_Tick(object sender, EventArgs e)
        {
            if (Visible)
                DRAW_LIGHT_WITH_RANGE(Position.X, Position.Y, Position.Z, Color.R, Color.G, Color.B, Radius, Intensity);
        }

    }
}