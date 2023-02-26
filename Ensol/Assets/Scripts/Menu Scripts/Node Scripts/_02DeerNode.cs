using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _02DeerNode : MonoBehaviour
{
    public ElectricGateController electricGateController = null;
    
    public GameObject weaponPickup;
    public GameObject dropDeer;
    public GameObject pickupParticles;

    private bool dropped = false;

    public void Update()
    {
        print(weaponPickup.activeInHierarchy);
        // spawns weapon pickup item after short delay if dropDeer is dead
        if(dropDeer.GetComponent<DeerBT>().isAlive == false && dropped == false)
        {
            StartCoroutine(DropAfterDelay(weaponPickup, dropDeer, pickupParticles));
            dropped = true;
        }

        // despawns particles once weapon pickup is looted
        //if(weaponPickup.activeInHierarchy == false)
        //{
        //    pickupParticles.SetActive(false);
        //}

        // unlocks next node if door is opened
        if(electricGateController.opening)
        {
            print("unlocked river node");
            CompletedNodes.riverNode = true;
        }
    }

    public IEnumerator DropAfterDelay(GameObject item, GameObject enemy, GameObject particles)
    {
        yield return new WaitForSeconds(1f);
        GameObject inSceneItem;
        inSceneItem = Instantiate(item, enemy.transform.position, enemy.transform.rotation);
        Instantiate(particles, inSceneItem.transform.position, inSceneItem.transform.rotation, inSceneItem.transform);
    }
}
