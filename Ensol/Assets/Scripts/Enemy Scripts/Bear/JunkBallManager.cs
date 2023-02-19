using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkBallManager : MonoBehaviour
{
    public BearBT bearBT;
    public Rigidbody ballPrefab;
    public Transform junkOrigin;
    private Rigidbody junkBall;
    [HideInInspector] public float maxThrowStrength;
    [HideInInspector] public float minThrowStrength;
    [HideInInspector] public Transform playerTF;
    private bool throwingJunk;

    private void Start()
    {
        throwingJunk = false;
        maxThrowStrength = bearBT.bearStats.junkMaxSpeed;
        minThrowStrength = bearBT.bearStats.junkMinSpeed;
    }

    private void FixedUpdate()
    {
        //Sets playerTF using bearStats
        if (playerTF == null)
        {
            playerTF = bearBT.bearStats.playerTF;
            return;
        }
        //Throws junk when signaled by animation event
        if (!throwingJunk && bearBT.root.GetData("throwJunk") != null)
        {
            ThrowJunk();
        }
        //No longer throwing junk when junk is destroyed
        if (bearBT.root.GetData("throwJunk") == null)
        {
            throwingJunk = false;
        }
    }

    private void ThrowJunk()
    {
        throwingJunk = true;

        //Spawn junk ball
        junkBall = Instantiate(ballPrefab, junkOrigin.position, junkOrigin.rotation);
        junkBall.transform.position = junkOrigin.position;
        junkBall.transform.rotation = junkOrigin.rotation;
        junkBall.velocity = Vector3.zero;

        //Get velocity for ball
        Vector3 throwVelocity = CalculateThrowData(playerTF.position, junkBall.position);


        //Assign which bear is throwing the ball, the damage of the ball, and then throw it
        junkBall.GetComponent<JunkBall>().bearTF     = transform;
        junkBall.GetComponent<JunkBall>().junkDamage = bearBT.bearStats.junkDamage;
        junkBall.velocity = throwVelocity;
    }

    private Vector3 CalculateThrowData(Vector3 targetPosition, Vector3 startPosition)
    {
        //Calcuating displacement on XZ and Y axis
        Vector3 displacement = new Vector3(targetPosition.x - startPosition.x, 0, targetPosition.z - startPosition.z);
        float deltaY = targetPosition.y - startPosition.y;
        float deltaXZ = displacement.magnitude;

        //Physics equation to figure out optimal initial velocity
        float gravity = Mathf.Abs(Physics.gravity.y);
        float v = Mathf.Clamp(Mathf.Sqrt(gravity * (deltaY + Mathf.Sqrt((deltaY * deltaY) + (deltaXZ * deltaXZ)))), minThrowStrength, maxThrowStrength);

        //Finding angle it needs to be thrown at with PHYSICS
        //float angle = Mathf.PI / 2f - (0.5f * (Mathf.PI / 2 - (deltaY / deltaXZ)));
        float angle = Mathf.Atan((Mathf.Pow(v, 2) - Mathf.Sqrt(Mathf.Pow(v, 4) - gravity * (gravity * Mathf.Pow(deltaXZ, 2) + 2 * deltaY * Mathf.Pow(v, 2)))) / (gravity * deltaXZ));
        //Make sure the bear doesn't throw at too sharp an angle
        angle = Mathf.Clamp(angle, -.30f, .70f);
        //Prevents cases where angle ends up being NaN by entering a default throw angle
        if (float.IsNaN(angle))
        {
            angle = 0;
        }

        //Finding initial velocity (how hard it needs to be launched in the XZ and Y direction seperately)
        Vector3 initialVelocity = Mathf.Cos(angle) * v * displacement.normalized
                                  + Mathf.Sin(angle) * v * Vector3.up;
        return initialVelocity;
    }
}
