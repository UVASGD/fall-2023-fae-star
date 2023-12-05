using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMoveDictionary : MonoBehaviour
{
    [SerializeField] List<GameObject> movePrefabs;

    void Awake()
    {
        foreach(GameObject g in movePrefabs)
        {
            MoveInvokes[g.name] = g;
        }
    }

    public static void invoke(string moveName)
    {
        Instantiate(MoveInvokes[moveName]);
    }

    public static Dictionary<string, Move.ActionTypes> MoveActionTypes = new Dictionary<string, Move.ActionTypes>
    {
        //Mage moves
        { "Club", Move.ActionTypes.SE },
        { "Lesser Heal", Move.ActionTypes.SA },
        { "Heal", Move.ActionTypes.SA },
        { "Lesser Flame", Move.ActionTypes.SE },
        { "Blazing", Move.ActionTypes.PB },

        // Assassin Moves
        { "Knife", Move.ActionTypes.SE },
        { "Backstab", Move.ActionTypes.SE },

        // Hacker Moves
        { "Poison Touch", Move.ActionTypes.SE },
        { "Malware", Move.ActionTypes.SE },

        // Knight Moves
        { "Slash", Move.ActionTypes.SE },
        { "Raise Shield", Move.ActionTypes.PB },

        // Enemy Moves
        { "Wolf Bite", Move.ActionTypes.SE },
    };

    public static Dictionary<string, int> MoveManaCosts = new Dictionary<string, int>
    {
        //Mage moves
        { "Club", 0 },
        { "Lesser Heal", 3 },
        { "Heal", 5 },
        { "Lesser Flame", 4 },
        { "Blazing", 6 },

        // Assassin Moves
        { "Knife", 0 },
        { "Backstab", 3 },

        // Hacker Moves
        { "Poison Touch", 0 },
        { "Malware", 3 },

        // Knight Moves
        { "Slash", 0 },
        { "Raise Shield", 3 },
    };

    public static Dictionary<string, GameObject> MoveInvokes = new Dictionary<string, GameObject>();
}
