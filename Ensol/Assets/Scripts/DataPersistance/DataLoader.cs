using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour, IDataPersistance
{
    public TextAsset glbs;

    public void LoadData(PData data)
    {
        // print(DialogueVariables.saveFile);
        // PlayerData.NGworked = data.NGworked;




        PlayerData.startedGame = data.startedGame;
        PlayerData.clickedStartDialogue = data.clickedStartDialogue;
        PlayerData.hasBroom = data.hasBroom;
        PlayerData.hasSolarUpgrade = data.hasSolarUpgrade;
        PlayerData.hasThrowUpgrade = data.hasThrowUpgrade;
        PlayerData.diedToCrackDeer = data.diedToCrackDeer;
        PlayerData.hasShield = data.hasShield;

        PlayerData.currentlyHasBroom = data.currentlyHasBroom;
        PlayerData.currentlyHasSolar = data.currentlyHasSolar;
        PlayerData.currHP = data.currHP;

        //Player Progress
        PlayerData.currentNode = data.currentNode;

        //Player Tutorial Text
        PlayerData.shownWalkText  = data.shownWalkText;
        PlayerData.shownBroomText = data.shownBroomText;
        PlayerData.shownSolarText = data.shownSolarText;
        PlayerData.shownThrowText = data.shownThrowText;
        PlayerData.shownMapText   = data.shownMapText;

        //Player Stats (used for tutorial text)
        PlayerData.lightAttacks = data.lightAttacks;
        PlayerData.dashes = data.dashes;
        PlayerData.heavyAttacks = data.heavyAttacks;
        PlayerData.throwAttacks = data.throwAttacks;
        PlayerData.deaths = data.deaths;
        PlayerData.distanceMoved = data.distanceMoved;

        //Interactable Item States
        PlayerData.doorInteracted = data.doorInteracted;
        PlayerData.podInteracted = data.podInteracted;
        PlayerData.conveyerInteracted = data.conveyerInteracted;
        PlayerData.windowInteracted = data.windowInteracted;
        PlayerData.plushInteracted = data.plushInteracted;
        PlayerData.footstepsInteracted = data.footstepsInteracted;
        
        // Enemy's Killed
        PlayerData.bearsKilled = data.bearsKilled;
        PlayerData.bunniesKilled = data.bunniesKilled;
        PlayerData.deerKilled = data.deerKilled;

        //River control progress
        PlayerData.controlsHit = data.controlsHit;

        //Security Tower Node Progression
        PlayerData.birdTriggered = data.birdTriggered;
        PlayerData.disableBird = data.disableBird;
        PlayerData.hasTransponder = data.hasTransponder;

        //Power Grid Node Progression
        PlayerData.firstGenHit = data.firstGenHit;
        PlayerData.secondGenHit = data.secondGenHit;
        PlayerData.thirdGenHit = data.thirdGenHit;


        // Node 1 variables
        _01DeerNode.weaponPickedUp = data.weaponPickedUp;

        // completed nodes variables
        CompletedNodes.prevNode = data.prevNode;
        // CompletedNodes.lNode = data.lNode;
        CompletedNodes.cabinNode = data.cabinNode;
        CompletedNodes.deerNode = data.deerNode;
        CompletedNodes.riverNode = data.riverNode;
        CompletedNodes.gateNode = data.gateNode;
        CompletedNodes.riverControlNode = data.riverControlNode;
        CompletedNodes.bearNode = data.bearNode;
        CompletedNodes.brokenMachineNode = data.brokenMachineNode; 
        CompletedNodes.securityTowerNode = data.securityTowerNode;
        CompletedNodes.birdNode = data.birdNode;
        CompletedNodes.powerGridNode = data.powerGridNode;
        CompletedNodes.metalFieldNode = data.metalFieldNode;
        CompletedNodes.computerNode = data.computerNode;

        CompletedNodes.nodes = data.nodes;

        CompletedNodes.firstLoad = data.firstLoad;

        CompletedNodes.completedNodes = data.completedNodes;

        CompletedNodes.firstTransition = data.firstTransition;
        
        CompletedNodes.checkpoints = data.checkpoints;

        PlayerData.timeSinceAtNode = data.timeSinceAtNode;
        PlayerData.enemiesAliveInNode = data.enemiesAliveInNode;
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
        data.hasTransponder = PlayerData.hasTransponder;



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

        data.timeSinceAtNode = PlayerData.timeSinceAtNode;
        data.enemiesAliveInNode = PlayerData.enemiesAliveInNode;
    }

    public void SaveStory(ref TextAsset globals)
    {
        globals = glbs;
    }
}
