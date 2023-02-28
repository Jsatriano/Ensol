using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkBallManager : MonoBehaviour
{
    public BearBT bearBT;
    public Rigidbody ballPrefab;
    public Transform junkOrigin;
    [SerializeField] private float maxPrediction;
    private Rigidbody junkBall;
    [HideInInspector] public float maxThrowStrength;
    [HideInInspector] public float minThrowStrength;
    private Transform playerTF;
    private Rigidbody playerRB;
    private Transform bearTF;
    private float rotation;
    private bool throwingJunk;

    private void Start()
    {
        throwingJunk = false;
        maxThrowStrength = bearBT.bearStats.junkMaxSpeed;
        minThrowStrength = bearBT.bearStats.junkMinSpeed;
        bearTF = bearBT.bearStats.enemyTF;
        rotation = bearBT.bearStats.rotationSpeed / 40;
    }

    private void FixedUpdate()
    {
        //Don't do anything when bear is dead
        if (bearBT.isAlive == false)
        {
            return;
        }
        //Sets playerTF using bearStats
        if (playerTF == null)
        {
            playerTF = bearBT.bearStats.playerTF;
            playerRB = bearBT.bearStats.playerRB;
            return;
        }

        //Throws junk when signaled by animation event
        if (!throwingJunk && bearBT.root.GetData("throwJunk") != null)
        {
            ThrowJunk();
        }

        if (!throwingJunk && bearBT.root.GetData("throwingAnim") != null)
        {
            Vector3 toPlayer = new Vector3(playerTF.position.x - bearTF.position.x, 0, playerTF.position.z - bearTF.position.z).normalized;
            bearTF.forward = Vector3.Lerp(bearTF.forward, toPlayer, rotation);
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
        ThrowData data = CalculateThrowData(playerTF.position + Vector3.up, junkBall.position);
        data = PredictThrow(data, playerTF.position, junkBall.position);


        //Assign which bear is throwing the ball, and all the stats about the junk ball, then throws it
        JunkBall junkBallScript = junkBall.GetComponent<JunkBall>();
        junkBallScript.bearTF          = transform;
        junkBallScript.junkDamage      = bearBT.bearStats.junkDamage;
        junkBallScript.explosionDamage = bearBT.bearStats.explosionDamage;
        junkBallScript.explosionLength = bearBT.bearStats.explosionLength;
        Vector3 finalScale = new Vector3(1, 1, 1) * bearBT.bearStats.explosionSize;
        junkBallScript.explosionFinalSize = finalScale;
        junkBall.velocity = data.initialVelocity;
    }

    private ThrowData CalculateThrowData(Vector3 targetPosition, Vector3 startPosition)
    {
        ThrowData data = new ThrowData();
        //Calcuating displacement on XZ and Y axis
        Vector3 displacement = new Vector3(targetPosition.x - startPosition.x, 0, targetPosition.z - startPosition.z);
        float deltaY = targetPosition.y - startPosition.y;
        float deltaXZ = displacement.magnitude;
        data.deltaY = deltaY;
        data.deltaXZ = deltaXZ;

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
        data.angle = angle;

        //Finding initial velocity (how hard it needs to be launched in the XZ and Y direction seperately)
        Vector3 initialVelocity = Mathf.Cos(angle) * v * displacement.normalized
                                  + Mathf.Sin(angle) * v * Vector3.up;
        data.initialVelocity = initialVelocity;
        return data;
    }

    private ThrowData PredictThrow(ThrowData data, Vector3 targetPos, Vector3 startPosition)
    {
        //Find how long it will take for the ball to get to the ball
        Vector3 velocity = data.initialVelocity;
        velocity.y = 0;
        float time = data.deltaXZ / velocity.magnitude;

        //Predict how much the player will move in that time
        Vector3 playerVelocity = playerRB.velocity;
        //Clamps how much the bear will predict
        if (playerVelocity.magnitude > maxPrediction)
        {
            playerVelocity = Vector3.ClampMagnitude(playerVelocity, maxPrediction);
        }
        Vector3 playerMovement = playerRB.velocity * time;

        //Calculate new throw data
        Vector3 newTarget = new Vector3(targetPos.x + playerMovement.x, targetPos.y, targetPos.z + playerMovement.z);
        ThrowData predictData = CalculateThrowData(newTarget + Vector3.up, startPosition);
        return predictData;
    }

    private class ThrowData
    {
        public float deltaY;
        public float deltaXZ;
        public float angle;
        public Vector3 initialVelocity;
    }

}
