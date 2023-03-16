using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    //Weapon Upgrades
    public static bool hasBroom = false;
    public static bool hasSolarUpgrade = false;
    public static bool hasThrowUpgrade = false;
    public static bool diedToCrackDeer = false;

    //Player Progress
    public static int currentNode = 1;

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
