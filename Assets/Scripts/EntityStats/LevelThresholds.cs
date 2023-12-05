using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelThresholds : MonoBehaviour
{
    private static List<int> thresholds = new List<int>
    {
        50,58,65,72,80,88,97,106,115,125,135,146,157,168,180,192,205,218,231,245,259,274,289,304,320,336,353,370,387,405,423,442,461,480,500,520,541,562,583,605,627,650,673,696,720,744,769,794,819,845,871,898,925,952,980,1008,1037,1066,1095,1125,1155,1186,1217,1248,1280,1312,1345,1378,1411,1445,1479,1514,1549,1584,1620,1656,1693,1730,1767,1805,1843,1882,1921,1960,2000,2040,2081,2122,2163,2205,2247,2290,2333,2376,2420
    };

    public static int determineLevel(int xp)
    {
        int lvl = 0;
        for (int i = 1; xp > thresholds[i - 1] && i - 1 < thresholds.Count; i++)
        {
            xp -= thresholds[i];
            lvl = i;
        }
        return lvl + 5;
    }
}
