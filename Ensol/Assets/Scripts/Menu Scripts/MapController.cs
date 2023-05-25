using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public enum MapState
    {
        UNOPENED,
        OPENED,
        NODETRANSFER
    }

    [SerializeField] private GameObject mapUI;
    [SerializeField] private CompletedNodes completedNodes;
    [HideInInspector] public MapState mapState;
    [SerializeField] private PauseMenu pauseMenu;

    private void Start()
    {
        mapState = MapState.UNOPENED;
    }

    private void Update()
    {
        switch (mapState)
        {
            case MapState.UNOPENED:
                if (Input.GetButtonDown("Map") && pauseMenu.menuState == PauseMenu.MenuState.UNPAUSED)
                {
                    OpenMap();
                }
                break;

            case MapState.OPENED:
                if ((Input.GetButtonDown("Map") || Input.GetButtonDown("Cancel")) && pauseMenu.menuState == PauseMenu.MenuState.UNPAUSED)
                {
                    CloseMap();
                }
                break;

            case MapState.NODETRANSFER:
                break;
        }
    }

    private void OpenMap()
    {
        mapState = MapState.OPENED;
        mapUI.SetActive(true);
        completedNodes.LookAtMap();      
    }

    private void CloseMap()
    {
        mapState = MapState.UNOPENED;
        completedNodes.StopAllCoroutines();
        mapUI.SetActive(false);
    }

    private void OpenMapForNodeTransfer()
    {
        mapState = MapState.NODETRANSFER;
        completedNodes.NodeTransferMap();
    }

}
