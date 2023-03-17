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

    //Player Death Stuff
    public static bool currentlyHasBroom = false;
    public static bool currentlyHasSolar = false;

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
    public static float deaths = 0;

    //Interactable Item States
    public static bool doorInteracted = false;
    public static bool podInteracted = false;
    public static bool conveyerInteracted = false;
    public static bool windowInteracted = false;
    public static bool plushInteracted = false;
    public static bool footstepsInteracted = false;
}
