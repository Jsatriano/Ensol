using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAnimation : MonoBehaviour
{

    public enum State
    {
        IDLE,
        WALKING,
        SWIPING,
        THROWING,
        DYING
    }

    [SerializeField] private Animator animController;
    [SerializeField] private Transform headTF;
    [SerializeField] private float lookingSpeed;
    private Transform playerTF;
    public BearBT bearBT;

    private Vector3 previousDirection;
    private Vector3 _dirToPlayer;
    State state;

    void Start()
    {
        state = State.IDLE;
    }

    private void FixedUpdate()
    {
        //Stops all animation once bear is dead
        if (state == State.DYING)
        {
            return;
        }
        if (!bearBT.isAlive)
        {
            state = State.DYING;
            //animController.SetTrigger("dying");
            return;
        }

        if (playerTF == null && bearBT.root.GetData("player") != null)
        {
            playerTF = (Transform)bearBT.root.GetData("player");
        }
    }
}
