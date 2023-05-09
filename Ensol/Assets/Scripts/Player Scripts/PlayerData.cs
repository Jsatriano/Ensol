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
    public static bool hasShield = false;

    //Player Death Stuff
    public static bool currentlyHasBroom = false;
    public static bool currentlyHasSolar = false;
    public static float currHP = -1;

    //Player Progress
    public static int currentNode = 1;

    //Player Tutorial Text
    public static bool shownWalkText  = false;
    public static bool shownBroomText = false;
    public static bool shownSolarText = false;
    public static bool shownThrowText = false;

    //Player Stats (used for tutorial text)
    public static float lightAttacks = 0;
    public static float dashes = 0;
    public static float heavyAttacks = 0;
    public static float throwAttacks = 0;
    public static float deaths = 0;
    public static float distanceMoved = 0;

    //Interactable Item States
    public static bool doorInteracted = false;
    public static bool podInteracted = false;
    public static bool conveyerInteracted = false;
    public static bool windowInteracted = false;
    public static bool plushInteracted = false;
    public static bool footstepsInteracted = false;
    
    // Enemy's Killed
    public static int bearsKilled = 0;
    public static int bunniesKilled = 0;
    public static int deerKilled = 0;

    //River control progress
    public static bool controlsHit = false;

    //Security Tower Node Progression
    public static bool birdTriggered = false;
    public static bool disableBird = false;
}
