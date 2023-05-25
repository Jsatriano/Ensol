using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver : MonoBehaviour, IDataPersistance
{
    public TextAsset glbs;
    public void LoadData(PData data)
    {
        // PlayerData.NGworked = data.NGworked;


        // PlayerData.worked = data.worked;


        // PlayerData.startedGame = data.startedGame;
        // PlayerData.hasBroom = data.hasBroom;
        // PlayerData.hasSolarUpgrade = data.hasSolarUpgrade;
        // PlayerData.hasThrowUpgrade = data.hasThrowUpgrade;
        // PlayerData.diedToCrackDeer = data.diedToCrackDeer;
        // PlayerData.hasShield = data.hasShield;

        // PlayerData.currentlyHasBroom = data.currentlyHasBroom;
        // PlayerData.currentlyHasSolar = data.currentlyHasSolar;
        // PlayerData.currHP = data.currHP;

        // //Player Progress
        // PlayerData.currentNode = data.currentNode;

        // //Player Tutorial Text
        // PlayerData.shownWalkText  = data.shownWalkText;
        // PlayerData.shownBroomText = data.shownBroomText;
        // PlayerData.shownSolarText = data.shownSolarText;
        // PlayerData.shownThrowText = data.shownThrowText;

        // //Player Stats (used for tutorial text)
        // PlayerData.lightAttacks = data.lightAttacks;
        // PlayerData.dashes = data.dashes;
        // PlayerData.heavyAttacks = data.heavyAttacks;
        // PlayerData.throwAttacks = data.throwAttacks;
        // PlayerData.deaths = data.deaths;
        // PlayerData.distanceMoved = data.distanceMoved;

        // //Interactable Item States
        // PlayerData.doorInteracted = data.doorInteracted;
        // PlayerData.podInteracted = data.podInteracted;
        // PlayerData.conveyerInteracted = data.conveyerInteracted;
        // PlayerData.windowInteracted = data.windowInteracted;
        // PlayerData.plushInteracted = data.plushInteracted;
        // PlayerData.footstepsInteracted = data.footstepsInteracted;
        
        // // Enemy's Killed
        // PlayerData.bearsKilled = data.bearsKilled;
        // PlayerData.bunniesKilled = data.bunniesKilled;
        // PlayerData.deerKilled = data.deerKilled;

        // //River control progress
        // PlayerData.controlsHit = data.controlsHit;

        // //Security Tower Node Progression
        // PlayerData.birdTriggered = data.birdTriggered;
        // PlayerData.disableBird = data.disableBird;
        // //PlayerData.NGworked = data.NGworked;
    }

    public void SaveData(ref PData data)
    {
        data.NGworked = PlayerData.NGworked;



        data.startedGame = PlayerData.startedGame;
        data.hasBroom = PlayerData.hasBroom;
        data.hasSolarUpgrade = PlayerData.hasSolarUpgrade;
        data.hasThrowUpgrade =  PlayerData.hasThrowUpgrade;
        data.diedToCrackDeer = PlayerData.diedToCrackDeer;
        data.hasShield = PlayerData.hasShield;

        data.currentlyHasBroom = PlayerData.currentlyHasBroom;
        data.currentlyHasSolar = PlayerData.currentlyHasSolar;
        data.currHP = PlayerData.currHP;

        //Player Progress
        data.currentNode = PlayerData.currentNode;

        //Player Tutorial Text
        data.shownWalkText = PlayerData.shownWalkText;
        data.shownBroomText = PlayerData.shownBroomText;
        data.shownSolarText = PlayerData.shownSolarText;
        data.shownThrowText = PlayerData.shownThrowText;

        //Player Stats (used for tutorial text)
        data.lightAttacks = PlayerData.lightAttacks;
        data.dashes = PlayerData.dashes;
        data.heavyAttacks = PlayerData.heavyAttacks;
        data.throwAttacks = PlayerData.throwAttacks;
        data.deaths = PlayerData.deaths;
        data.distanceMoved = PlayerData.distanceMoved;

        //Interactable Item States
        data.doorInteracted = PlayerData.doorInteracted;
        data.podInteracted = PlayerData.podInteracted;
        data.conveyerInteracted = PlayerData.conveyerInteracted;
        data.windowInteracted = PlayerData.windowInteracted;
        data.plushInteracted = PlayerData.plushInteracted;
        data.footstepsInteracted = PlayerData.footstepsInteracted;
        
        // Enemy's Killed
        data.bearsKilled = PlayerData.bearsKilled;
        data.bunniesKilled = PlayerData.bunniesKilled;
        data.deerKilled = PlayerData.deerKilled;

        //River control progress
        data.controlsHit = PlayerData.controlsHit;

        //Security Tower Node Progression
        data.birdTriggered = PlayerData.birdTriggered;
        data.disableBird = PlayerData.disableBird;
        //data.NGworked = PlayerData.NGworked;
    }

    // public void LoadStory(TextAsset globals)
    // {

    // }

    public void SaveStory(ref TextAsset globals)
    {
        globals = glbs;
    }
}
