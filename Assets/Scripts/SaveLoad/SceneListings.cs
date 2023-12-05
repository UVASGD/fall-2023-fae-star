using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneListings
{
    public static Dictionary<string, (string, string)> nextScene = new Dictionary<string, (string, string)>
    {
        { "1.1", ("1.2", "0-1") },
        { "1.2", ("2.1", "0-2") },
        { "2.1", ("2.2", "0-2") },
        { "2.2", ("2.3", "0-2") },
        { "2.3", ("3.1", "0-3") },
        { "3.1", ("3.2", "0-3") },
        { "3.2", ("3.3", "0-3") },
        { "3.3", ("1.1", "0-1") },
    };
}
