using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class RabbitAgroMode : Node
{
    private Transform _playerTF;  //Player transform
    private Transform _enemyTF;   //Enemy transform
    private Rigidbody _enemyRB;   //Enemy rigidbody
    private float _acceleration;  //How fast the rabbit gets to max speed
    private float _maxSpeed;      //Speed of the enemy
    private Vector3 _dirToPlayer; //The direction from the enemy to the player
    private Vector3 movingDir;    //The enemy's direction of movement
    private float _rotationSpeed; //How quickly the enemy turns (how well they can track the player)
    private LayerMask _envLayerMask; //Used for linecasting to player breadcrumbs
    private float _landingDrag;
    private float _normalDrag;
    private float _aggroLeaps;
    private float _leapCounter;

    public RabbitAgroMode(SphereCollider hitbox, float acceleration, float maxSpeed, Transform playerTF, Transform enemyTF, Rigidbody enemyRB, float rotationSpeed, 
                          LayerMask envLayerMask, float landingDrag, float normalDrag, float aggroLeaps)
    {
        _playerTF = playerTF;
        _enemyTF = enemyTF;
        _enemyRB = enemyRB;
        _acceleration = acceleration;
        _maxSpeed = maxSpeed / 10;
        _rotationSpeed = rotationSpeed;
        movingDir = Vector3.zero;
        _envLayerMask = envLayerMask;
        _aggroLeaps = aggroLeaps;
        _leapCounter = 0;
        _landingDrag = landingDrag;
        _normalDrag = normalDrag;
    }

    public override NodeState Evaluate()
    {
        SetData("attacking", true);

        SetData("AGGRO", true);

        SetData("aggro", true);
        
        //Exits aggro mode when the rabbit hits the player
        if (GetData("attackHit") != null)
        {
            _leapCounter = 0;
            ClearData("attackHit");
            ClearData("aggro");
            ClearData("attacking");
            state = NodeState.SUCCESS;
            return state;
        }

        //Exits aggro mode when the rabbit has leaped a set amount of times
        if (_leapCounter >= _aggroLeaps)
        {
            _leapCounter = 0;
            ClearData("aggro");
            ClearData("attacking");
            state = NodeState.SUCCESS;
            return state;
        }

        ChooseDirection();

        //Increments the leap counter when signaled by an animation event
        if (GetData("incrementLeaps") != null)
        {
            _leapCounter++;
            ClearData("incrementLeaps");
        }

        float speedDot;
        //Applies landing drag when signaled by an animation event
        if (GetData("applyLandingDrag") != null)
        {
            _enemyRB.drag = _landingDrag;

            speedDot = Vector3.Dot(_enemyTF.forward, movingDir);
            speedDot = (speedDot / 2) + 0.5f;
            speedDot = Mathf.Clamp(speedDot, 0.3f, 1);

            _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, movingDir, _rotationSpeed / 100);
        }

        //Makes sure the var for feet being on ground is set
        if (GetData("feetOnGround") == null)
        {
            SetData("feetOnGround", false);
        }

        //Doesn't accelerate or rotate rabbit while it is in the air
        if (!(bool)GetData("feetOnGround"))
        {
            state = NodeState.SUCCESS;
            return state;
        }

        _enemyRB.drag = _normalDrag;
        //Finds how closely the rabbit's transform.forward is to the direction it wants to move and then limits that to a number between 0 and 1
        speedDot = Vector3.Dot(_enemyTF.forward, movingDir);
        speedDot = (speedDot / 2) + 0.5f;
        speedDot = Mathf.Clamp(speedDot, 0.3f, 1);

        _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, movingDir, _rotationSpeed / 100);

        //Moves rabbit in the desired direction (with a speed cap)
        if (_enemyRB.velocity.magnitude > _maxSpeed)
        {
            //Keep moving rabbit at max speed
            _enemyRB.velocity = Vector3.ClampMagnitude(_enemyRB.velocity, _maxSpeed);
        }
        else
        {
            //Accelerate rabbit when not at max speed
            _enemyRB.AddForce(_enemyTF.forward * _acceleration * speedDot, ForceMode.Acceleration);
        }
        state = NodeState.SUCCESS;
        return state;
    }

    private void ChooseDirection()
    {
        //Gets the weights for all 8 directions and adds them together to get the most optimal direction
        float[] weights = CalculateWeights();
        movingDir = Vector3.zero;
        for (int i = 0; i < weights.Length; i++)
        {
            movingDir += Directions.eightDirections[i] * weights[i];
        }
        movingDir = movingDir.normalized;
        SetData("moveDir", movingDir);
    }

    private float[] CalculateWeights()
    {
        float[] playerWeights = CalculatePlayerWeights();
        float[] obstacleWeights = (float[])GetData("obstacles");
        float[] finalWeights = new float[8];
        //Substracts the interest weights by the danger weights to get the final weights
        for (int i = 0; i < playerWeights.Length; i++)
        {
            finalWeights[i] = Mathf.Clamp01(playerWeights[i] - obstacleWeights[i]);
        }
        SetData("final", finalWeights);
        SetData("playerWeights", playerWeights);
        return finalWeights;
    }

    private float[] CalculatePlayerWeights()
    {
        //Sets up array and calcuates the distance and direction to the player
        float[] playerWeights = new float[8];
        Vector3 target = _playerTF.position;

        List<Vector3> breadcrumbs = (List<Vector3>)GetData("breadcrumbs");
        //Uses player's current position as the target if they are currently in FOV
        if (breadcrumbs == null || !Physics.Linecast(_enemyTF.position, _playerTF.position, _envLayerMask))
        {
            target = _playerTF.position;
        }
        //Else use the breadcrumbs
        else
        {
            bool foundTarget = false;
            //Check if any of the breadcrumbs are in FOV, starting at the most recent one
            for (int i = breadcrumbs.Count - 1; i >= 0; i--)
            {
                if (!Physics.Linecast(_enemyTF.position, breadcrumbs[i], _envLayerMask))
                {
                    target = breadcrumbs[i];
                    foundTarget = true;
                    break;
                }
            }
            //Use oldest breadcrumb as target if none are within FOV
            if (!foundTarget)
            {
                if (breadcrumbs.Count <= 0)
                {
                    target = _playerTF.position;
                }
                else
                {
                    target = breadcrumbs[0];
                }
            }
        }

        _dirToPlayer = new Vector3(target.x - _enemyTF.position.x, 0, target.z - _enemyTF.position.z);
        float distanceToPlayer = _dirToPlayer.magnitude;
        _dirToPlayer = _dirToPlayer.normalized;

        //Calculates a weight for all 8 directions based on how close it is to the direction the the player
        for (int i = 0; i < playerWeights.Length; i++)
        {
            float dot = Vector3.Dot(_dirToPlayer.normalized, Directions.eightDirections[i]);
            //Favors directions the rabbit is already facing
            float dot2 = Vector3.Dot(_enemyTF.forward, Directions.eightDirections[i]);
            dot += Mathf.Clamp(dot2, 0, 0.2f);
            float weightToAdd = Mathf.Clamp01(dot);
            if (weightToAdd > playerWeights[i])
            {
                playerWeights[i] = weightToAdd;
            }
        }
        SetData("playerWeights", playerWeights);

        return playerWeights;
    }
}
