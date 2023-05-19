using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShotManager : MonoBehaviour
{
    //Attack Vars
    private float minSpeed;
    private float maxSpeed;
    private float maxPrediction;
    private float debuffLength;
    private float speedDebuff;

    //References
    private SpiderStats spiderStats;
    private SpiderBT spiderBT;
    private Transform enemyTF;
    private Transform webShotSpawnPoint;
    private Rigidbody webShotPrefab;
    private Transform playerTF;
    private Rigidbody playerRB;

    private void Start()
    {
        spiderStats = GetComponent<SpiderStats>();
        spiderBT = GetComponent<SpiderBT>();
        enemyTF = spiderStats.enemyTF;
        webShotSpawnPoint = spiderStats.webShotSpawnPoint;
        webShotPrefab = spiderStats.webShotPrefab;
        minSpeed = spiderStats.webMinSpeed;
        maxSpeed = spiderStats.webMaxSpeed;
        maxPrediction = spiderStats.webMaxPrediction;
        debuffLength = spiderStats.webShotDebuffLength;
        speedDebuff = spiderStats.webShotDebuff;
    }

    public void StartWebShotAttack()
    {
        if (playerTF == null)
        {
            playerTF = spiderStats.playerTF;
            playerRB = spiderStats.playerRB;
        }
        StartCoroutine(WebShotAttack());
    }

    private IEnumerator WebShotAttack()
    {
        while (spiderBT.root.GetData("shootWeb") == null)
        {
            if (!spiderBT.isAlive)
            {
                yield break;
            }
            yield return null;
        }
        ShootWeb();
        AudioManager.instance.PlayOneShot(FMODEvents.instance.spiderWebShoot, this.transform.position);
        spiderBT.root.ClearData("shootWeb");
    }

    private void ShootWeb()
    {
        Rigidbody webShot = Instantiate(webShotPrefab, webShotSpawnPoint.position, transform.rotation);
        WebData data = CalculateThrowData(playerTF.position + Vector3.up, webShot.position);
        data = PredictThrow(data, playerTF.position, webShot.position);

        WebShot webShotScript = webShot.GetComponent<WebShot>();
        webShotScript.spiderTF = enemyTF;
        webShotScript.debuffLength = debuffLength;
        webShotScript.speedDebuff = speedDebuff;
        webShot.velocity = data.initialVelocity;
    }

    private WebData CalculateThrowData(Vector3 targetPosition, Vector3 startPosition)
    {
        WebData data = new WebData();
        //Calcuating displacement on XZ and Y axis
        Vector3 displacement = new Vector3(targetPosition.x - startPosition.x, 0, targetPosition.z - startPosition.z);
        float deltaY = targetPosition.y - startPosition.y;
        float deltaXZ = displacement.magnitude;
        data.deltaY = deltaY;
        data.deltaXZ = deltaXZ;

        //Physics equation to figure out optimal initial velocity
        float gravity = Mathf.Abs(Physics.gravity.y);
        float v = Mathf.Clamp(Mathf.Sqrt(gravity * (deltaY + Mathf.Sqrt((deltaY * deltaY) + (deltaXZ * deltaXZ)))), minSpeed, maxSpeed);

        //Finding angle it needs to be thrown at with PHYSICS
        float angle = Mathf.Atan((Mathf.Pow(v, 2) - Mathf.Sqrt(Mathf.Pow(v, 4) - gravity * (gravity * Mathf.Pow(deltaXZ, 2) + 2 * deltaY * Mathf.Pow(v, 2)))) / (gravity * deltaXZ));
        //Make sure the web doesn't get thrown at too sharp an angle
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

    private WebData PredictThrow(WebData data, Vector3 targetPos, Vector3 startPosition)
    {
        //Find how long it will take for the web to get to the player
        Vector3 velocity = data.initialVelocity;
        velocity.y = 0;
        float time = data.deltaXZ / velocity.magnitude;

        //Predict how much the player will move in that time
        Vector3 playerVelocity = playerRB.velocity;
        //Clamps the change in trajectory
        if (playerVelocity.magnitude > maxPrediction)
        {
            playerVelocity = Vector3.ClampMagnitude(playerVelocity, maxPrediction);
        }
        Vector3 playerMovement = playerVelocity * time;

        //Calculate new throw data
        Vector3 newTarget = new Vector3(targetPos.x + playerMovement.x, targetPos.y, targetPos.z + playerMovement.z);
        WebData predictData = CalculateThrowData(newTarget + Vector3.up, startPosition);
        return predictData;
    }

    private class WebData
    {
        public float deltaY;
        public float deltaXZ;
        public float angle;
        public Vector3 initialVelocity;
    }
}
