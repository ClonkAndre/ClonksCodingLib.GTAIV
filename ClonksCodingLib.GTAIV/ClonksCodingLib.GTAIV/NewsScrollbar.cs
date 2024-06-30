using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Stuff for the news scrollbar in The Triangle, south of Star Junction.
    /// </summary>
    public class NewsScrollbar
    {

        /// <summary>
        /// This method adds a string to the news scrollbar as seen in The Triangle, south of Star Junction.<br/>
        /// It's recommended to call <see cref="ClearNewsScrollbar()"/> before calling this function. If you don't, the string will be added next to the previous one that was created.
        /// </summary>
        /// <param name="str">The string to add to the news scrollbar.</param>
        public static void AddStringToNewsScrollbar(string str)
        {
            ADD_STRING_TO_NEWS_SCROLLBAR(str);
        }

        /// <summary>
        /// This method removes all text from the news scrollbar as seen in The Triangle, south of Star Junction.
        /// </summary>
        public static void ClearNewsScrollbar()
        {
            CLEAR_NEWS_SCROLLBAR();
        }

    }
}
