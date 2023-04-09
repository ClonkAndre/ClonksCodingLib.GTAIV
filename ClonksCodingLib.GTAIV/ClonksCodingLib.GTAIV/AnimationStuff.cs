namespace CCL.GTAIV
{

    /// <summary>
    /// Represents a GTA IV animation set with all the animation names inside it.
    /// </summary>
    public class IVAnimSet
    {

        #region Variables
        /// <summary>
        /// Gets the name of this animation set.
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Gets the collection of animation names inside the animation set.
        /// </summary>
        public readonly string[] AnimNames;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new <see cref="IVAnimSet"/>.
        /// </summary>
        /// <param name="animSetName">The name of the animation set.</param>
        /// <param name="animNames">The collection of animation names inside the animation set.</param>
        public IVAnimSet(string animSetName, string[] animNames)
        {
            Name = animSetName;
            AnimNames = animNames;
        }
        #endregion

    }

}
