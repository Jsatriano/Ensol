using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimation : MonoBehaviour
{
    public enum State
    {
        IDLE,
        MOVING_FORWARD,
        MOVING_LEFT,
        MOVING_RIGHT,
        WEB_SHOOT,
        TAZER_SHOOT,
        DYING
    }

    [Header("References")]
    [SerializeField] private Animator animController;
    [SerializeField] private Rigidbody spiderRB;
    [SerializeField] SpiderBT spiderBT;
    private Transform playerTF;

    [Header("Misc")]
    private State state;
    private float maxWalkSpeed;

    void Start()
    {
        state = State.IDLE;
    }

    private void FixedUpdate()
    {
        
    }

}
