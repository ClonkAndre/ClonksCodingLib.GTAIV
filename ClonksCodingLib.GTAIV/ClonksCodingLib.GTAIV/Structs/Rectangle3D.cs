using System;
using System.Drawing;
using System.Numerics;

using IVSDKDotNet.Native;

namespace CCL.GTAIV
{
    public struct Rectangle3D
    {
        #region Variables and Properties
        // Variables
        private Vector3 m_Center;
        private Vector3 m_Size;

        // Properties
        public Vector3 Center
        {
            get { return m_Center; }
            set { m_Center = value; }
        }
        public Vector3 Size
        {
            get { return m_Size; }
            set { m_Size = value; }
        }

        public Vector3 Min
        {
            get { return Center - Size; }
        }
        public Vector3 Max
        {
            get { return Center + Size; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new <see cref="Rectangle3D"/>.
        /// </summary>
        /// <param name="center">The center/initial position.</param>
        /// <param name="size">The size of this <see cref="Rectangle3D"/>.</param>
        public Rectangle3D(Vector3 center, Vector3 size)
        {
            m_Center = center;
            m_Size = size * 0.5f;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Visualizes the bounds of this <see cref="Rectangle3D"/>. This needs to be called in a loop, like from your <see cref="IVSDKDotNet.Script.Tick"/> event.
        /// </summary>
        /// <param name="centerSize">The size of the corona that represents the <see cref="Center"/> of this <see cref="Rectangle3D"/>.</param>
        /// <param name="cornerSize">The size of the corona that represents the corners of this <see cref="Rectangle3D"/>.</param>
        public void Visualize(float centerSize = 50f, float cornerSize = 100f)
        {
            // Center
            Natives.DRAW_CORONA(Center, centerSize, 0, 0f, Color.Orange);

            // Bottom
            Natives.DRAW_CORONA(Min, cornerSize, 0, 0f, Color.Magenta);
            Natives.DRAW_CORONA(new Vector3(Max.X, Min.Y, Min.Z), cornerSize, 0, 0f, Color.Magenta);
            Natives.DRAW_CORONA(new Vector3(Min.X, Max.Y, Min.Z), cornerSize, 0, 0f, Color.Magenta);
            Natives.DRAW_CORONA(new Vector3(Max.X, Max.Y, Min.Z), cornerSize, 0, 0f, Color.Magenta);

            // Top
            Natives.DRAW_CORONA(Max, cornerSize, 0, 0f, Color.Magenta);
            Natives.DRAW_CORONA(new Vector3(Max.X, Min.Y, Max.Z), cornerSize, 0, 0f, Color.Magenta);
            Natives.DRAW_CORONA(new Vector3(Min.X, Max.Y, Max.Z), cornerSize, 0, 0f, Color.Magenta);
            Natives.DRAW_CORONA(new Vector3(Min.X, Min.Y, Max.Z), cornerSize, 0, 0f, Color.Magenta);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Checks if the given <see cref="Rectangle3D"/> is intersects with this <see cref="Rectangle3D"/>.
        /// </summary>
        /// <param name="bounds">The <see cref="Rectangle3D"/> to check.</param>
        /// <returns>True if they intersect. Otherwise, false.</returns>
        public bool Intersects(Rectangle3D bounds)
        {
            return Min.X <= bounds.Max.X && Max.X >= bounds.Min.X && Min.Y <= bounds.Max.Y && Max.Y >= bounds.Min.Y && Min.Z <= bounds.Max.Z && Max.Z >= bounds.Min.Z;
        }

        // Statics
        /// <summary>
        /// Returns an empty <see cref="Rectangle3D"/>.
        /// </summary>
        /// <returns>An empty <see cref="Rectangle3D"/>.</returns>
        public static Rectangle3D Empty()
        {
            return new Rectangle3D();
        }
        #endregion
    }
}
