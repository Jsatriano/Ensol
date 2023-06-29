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
        // data.NGworked = PlayerData.NGworked;



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
        data.bloodLocations = PlayerData.bloodLocations;

        //Player Tutorial Text
        data.shownWalkText = PlayerData.shownWalkText;
        data.shownBroomText = PlayerData.shownBroomText;
        data.shownSolarText = PlayerData.shownSolarText;
        data.shownThrowText = PlayerData.shownThrowText;
        data.shownMapText   = PlayerData.shownMapText;

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
        data.hasTransponder = PlayerData.hasTransponder;

        //Power Grid Node Progression
        data.firstGenHit = PlayerData.firstGenHit;
        data.secondGenHit =  PlayerData.secondGenHit;
        data.thirdGenHit =  PlayerData.thirdGenHit;

        //data.NGworked = PlayerData.NGworked;


        // Node 1 variables
        data.weaponPickedUp = _01DeerNode.weaponPickedUp;

        // completed nodes variables
        data.prevNode = CompletedNodes.prevNode;
        // data.lNode = CompletedNodes.lNode;
        data.cabinNode = CompletedNodes.cabinNode;
        data.deerNode = CompletedNodes.deerNode;
        data.riverNode = CompletedNodes.riverNode;
        data.gateNode = CompletedNodes.gateNode;
        data.riverControlNode = CompletedNodes.riverControlNode;
        data.bearNode = CompletedNodes.bearNode;
        data.brokenMachineNode = CompletedNodes.brokenMachineNode; 
        data.securityTowerNode = CompletedNodes.securityTowerNode;
        data.birdNode = CompletedNodes.birdNode;
        data.powerGridNode = CompletedNodes.powerGridNode;
        data.metalFieldNode = CompletedNodes.metalFieldNode;
        data.computerNode = CompletedNodes.computerNode;

        data.nodes = CompletedNodes.nodes;

        data.firstLoad = CompletedNodes.firstLoad;
        
        data.completedNodes = CompletedNodes.completedNodes;

        data.firstTransition = CompletedNodes.firstTransition;
        
        data.checkpoints = CompletedNodes.checkpoints;

        data.timeSinceAtNode = PlayerData.timeSinceAtNode;
        data.cabinEnemiesAlive = PlayerData.enemiesAliveInNode[0];
        data.deerEnemiesAlive = PlayerData.enemiesAliveInNode[1];
        data.riverEnemiesAlive = PlayerData.enemiesAliveInNode[2];
        data.gateEnemiesAlive = PlayerData.enemiesAliveInNode[3];
        data.riverControlEnemiesAlive = PlayerData.enemiesAliveInNode[4];
        data.bearEnemiesAlive = PlayerData.enemiesAliveInNode[5];
        data.brokenMachineEnemiesAlive = PlayerData.enemiesAliveInNode[6];
        data.securityTowerEnemiesAlive = PlayerData.enemiesAliveInNode[7];
        data.birdEnemiesAlive = PlayerData.enemiesAliveInNode[8];
        data.powerGridEnemiesAlive = PlayerData.enemiesAliveInNode[9];
        data.metalFieldEnemiesAlive = PlayerData.enemiesAliveInNode[10];
        data.computerEnemiesAlive = PlayerData.enemiesAliveInNode[11];
        data.computerInteriorEnemiesAlive = PlayerData.enemiesAliveInNode[12];

        //data that is not overwritten in a new game
        data.musicValue = OptionsMenu.musicValue;
        data.sfxValue = OptionsMenu.sfxValue;
        data.screenShakeValue = OptionsMenu.screenShakeValue;
        data.catModeActivated = OptionsMenu.catModeActivated;
        data.grassActivated = OptionsMenu.grassActivated;
        data.beatenGame = PlayerData.beatenGame;
    }

    // public void LoadStory(TextAsset globals)
    // {

    // }

    public void SaveStory(ref TextAsset globals)
    {
        globals = glbs;
    }
}
