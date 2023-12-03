using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalItemLists
{
    // For now I am going to hard code this, but we might want to change it sometime in the future.
    // The setup for this list is (Item, int) => (Item, Position in Selection List)

    public static Dictionary<string, (Item, int)> ItemList = new Dictionary<string, (Item, int)>
        {
            {"Health Potion", (new HPPotion(), 11)},
            {"MP Potion", (new MPPotion(), 21)},
            {"Mystery Potion", (new MysteryPotion(), 31)},
            {"Back", (null, 41)}
        };
}
