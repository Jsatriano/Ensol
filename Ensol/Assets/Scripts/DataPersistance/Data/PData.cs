using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PData
{
    public bool worked;


    public bool startedGame;
    public bool hasBroom;
    public bool hasSolarUpgrade;
    public bool hasThrowUpgrade;
    public bool diedToCrackDeer;
    public bool hasShield;

    //Player Death Stuff
    public bool currentlyHasBroom;
    public bool currentlyHasSolar;
    public float currHP;

    //Player Progress
    public int currentNode;

    //Player Tutorial Text
    public bool shownWalkText;
    public bool shownBroomText;
    public bool shownSolarText;
    public bool shownThrowText;

    //Player Stats (used for tutorial text)
    public float lightAttacks;
    public float dashes;
    public float heavyAttacks;
    public float throwAttacks;
    public float deaths;
    public float distanceMoved;

    //Interactable Item States
    public bool doorInteracted;
    public bool podInteracted;
    public bool conveyerInteracted;
    public bool windowInteracted;
    public bool plushInteracted;
    public bool footstepsInteracted;
    
    // Enemy's Killed
    public int bearsKilled;
    public int bunniesKilled;
    public int deerKilled;

    //River control progress
    public bool controlsHit;

    //Security Tower Node Progression
    public bool birdTriggered;
    public bool disableBird;
    public bool hasTransponder;

    public int NGworked;
    


    // Node 1 variables
    public bool weaponPickedUp;

    // completed nodes variables
    public int prevNode;
    // public int lNode;
    public bool cabinNode;
    public bool deerNode;
    public bool riverNode; 
    public bool gateNode; 
    public bool riverControlNode; 
    public bool bearNode; 
    public bool brokenMachineNode; 
    public bool securityTowerNode; 
    public bool birdNode; 
    public bool powerGridNode; 
    public bool metalFieldNode; 
    public bool computerNode;

    public bool[] nodes;

    public bool[] firstLoad;

    public bool[] completedNodes;

    public bool[] firstTransition;

    public bool[] checkpoints;

    public float[] timeSinceAtNode;

    public int[] enemiesAliveInNode;

    public PData()
    {
        worked = false;
        startedGame = false;
        hasBroom = false;
        hasSolarUpgrade = false;
        hasThrowUpgrade = false;
        diedToCrackDeer = false;
        hasShield = false;

        currentlyHasBroom = false;
        currentlyHasSolar = false;
        currHP = -1;

        //Player Progress
        currentNode = 1;

        //Player Tutorial Text
        shownWalkText  = false;
        shownBroomText = false;
        shownSolarText = false;
        shownThrowText = false;

        //Player Stats (used for tutorial text)
        lightAttacks = 0;
        dashes = 0;
        heavyAttacks = 0;
        throwAttacks = 0;
        deaths = 0;
        distanceMoved = 0;

        //Interactable Item States
        doorInteracted = false;
        podInteracted = false;
        conveyerInteracted = false;
        windowInteracted = false;
        plushInteracted = false;
        footstepsInteracted = false;
        
        // Enemy's Killed
        bearsKilled = 0;
        bunniesKilled = 0;
        deerKilled = 0;

        //River control progress
        controlsHit = false;

        //Security Tower Node Progression
        birdTriggered = false;
        disableBird = false;
        NGworked = 0;
        hasTransponder = false;

        // Node 1 variables
        weaponPickedUp = false;

        // Completed Nodes
        prevNode = 999;
        // lNode = 0;

        cabinNode  = true;
        deerNode  = true;
        riverNode  = false; 
        gateNode  = false; 
        riverControlNode  = false; 
        bearNode  = false; 
        brokenMachineNode  = false; 
        securityTowerNode  = false; 
        birdNode  = false; 
        powerGridNode  = false; 
        metalFieldNode  = false; 
        computerNode = false;

        // nodes;

        firstLoad = new bool[] {
            false, false, true, true, true, true,
            true, true, true, true, true, false, false
        };

        completedNodes = new bool[]
        {
            true, false, false, false, false, false,
            false, false, false, false, false, true, true
        };

       firstTransition = new bool[] {
            false, true, true, true, true,
            true, true, true, true, true, true, false, false
        };

        checkpoints = new bool[] {
            false, false, false, false
        };

        timeSinceAtNode = new float[] {
            -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1
        };

        enemiesAliveInNode = new int[] {
            999, 999, 999, 999, 999, 999,
            999, 999, 999, 999, 999, 999
        };
    }
}
