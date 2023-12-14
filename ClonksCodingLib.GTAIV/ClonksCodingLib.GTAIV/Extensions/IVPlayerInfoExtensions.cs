using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Contains extensions for the <see cref="IVPlayerInfo"/> class.
    /// </summary>
    public static class IVPlayerInfoExtensions
    {
        #region Methods
        /// <summary>
        /// Sets the money of the player to the given amount.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="amount">The new amount.</param>
        public static void SetMoney(this IVPlayerInfo info, int amount)
        {
            if (info == null)
                return;
            if (amount < 0)
                return;

            STORE_SCORE(info.PlayerId, out uint oldMoney);
            ADD_SCORE(info.PlayerId, (int)(amount - oldMoney));
        }
        /// <summary>
        /// Adds money to the current player money.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="amount">The amount to add.</param>
        public static void AddMoney(this IVPlayerInfo info, int amount)
        {
            if (info == null)
                return;
            if (amount <= 0)
                return;

            ADD_SCORE(info.PlayerId, amount);
        }

        /// <summary>
        /// Removes the money of the player by the given amount.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="amount">The amount to remove.</param>
        public static void RemoveMoney(this IVPlayerInfo info, int amount)
        {
            if (info == null)
                return;
            if (amount < 0)
                return;

            ADD_SCORE(info.PlayerId, -1 * amount);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Gets the money amount of the player.
        /// </summary>
        /// <param name="info"></param>
        /// <returns>The amount of money the player has.</returns>
        public static uint GetMoney(this IVPlayerInfo info)
        {
            if (info == null)
                return 0;

            STORE_SCORE(info.PlayerId, out uint money);
            return money;
        }
        #endregion
    }
}
