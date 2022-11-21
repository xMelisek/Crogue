namespace Crogue.Core
{
    public class ItemID
    {
        #region IDs

        public const int ionBooster = 0;

        #endregion


        #region Item stats

        private static readonly Item[] items = {
            new Item("Ion Booster", "Damage up", damage: 1)
        }; 

        #endregion


        #region Functions

        /// <summary>
        /// Get stats of specified item
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <returns>Item stats</returns>
        public static Item GetItem(int id)
        {
            return items[id];
        }

        #endregion
    }
}