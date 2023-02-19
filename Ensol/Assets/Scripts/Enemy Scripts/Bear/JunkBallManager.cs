using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkBallManager : MonoBehaviour
{
    public BearBT bearBT;
    public GameObject ballPrefab;
    public Transform junkOrigin;
    [HideInInspector] public Transform playerTF;
    [HideInInspector] public bool throwingJunk;

    private void Start()
    {
        throwingJunk = false;
    }

    private void Update()
    {
        if (playerTF == null)
        {
            playerTF = bearBT.bearStats.playerTF;
            return;
        }
        if (bearBT.root.GetData("throwJunk") != null)
        {
            ThrowJunk();
        }
    }

    private void ThrowJunk()
    {
        throwingJunk = true;
        //GameObject junkBall = Instantiate(ballPrefab, junkOrigin.position, junkOrigin.rotation);

    }
}
