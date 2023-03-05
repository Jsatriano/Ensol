using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _02DeerNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;
    
    [Header("Weapon Pickup Variables")]
    public GameObject weaponPickup;
    public GameObject dropDeer;
    public static bool weaponPickedUp = false;
    

    private bool dropped = false;
    private GameObject inSceneItem = null;

    // timer to stop weaponPickup from insta-dropping
    private float timerLength = 0.25f;
    private float timer = 0f;

    public void Update()
    {
        timer += Time.deltaTime;
        
        // spawns weapon pickup item after short delay if dropDeer is dead
        if(timer >= timerLength)
        {
            if(dropDeer.GetComponent<DeerBT>().isAlive == false && dropped == false)
            {
                StartCoroutine(DropAfterDelay(weaponPickup, dropDeer));
                dropped = true;
            }
        }

        // checks if weapon has been spawned, and picked up
        if(inSceneItem != null && inSceneItem.activeInHierarchy == false)
        {
            weaponPickedUp = true;
        }

        // unlocks next node if door is opened
        if(electricGateController.opening)
        {
            print("unlocked river node");
            CompletedNodes.riverNode = true;
        }
    }

    public IEnumerator DropAfterDelay(GameObject item, GameObject enemy)
    {
        yield return new WaitForSeconds(1f);

        // spawn item, spawn particles and parent item
        inSceneItem = Instantiate(item, enemy.transform.position, enemy.transform.rotation);
    }
}
