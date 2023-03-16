using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    //Weapon Upgrades
    public static bool hasBroom = true;
    public static bool hasSolarUpgrade = true;
    public static bool hasThrowUpgrade = true;
    public static bool diedToCrackDeer = true;

    //Player Progress
    public static int currentNode = 3;

    //Player Tutorial Text
    public static bool shownBroomText = false;
    public static bool shownSolarText = false;
    public static bool shownThrowText = false;

    //Player Stats (used for tutorial text)
    public static float lightAttacks = 0;
    public static float dashes = 0;
    public static float heavyAttacks = 0;
    public static float throwAttacks = 0;
}
