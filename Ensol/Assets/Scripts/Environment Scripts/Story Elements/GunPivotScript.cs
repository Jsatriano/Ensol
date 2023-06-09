using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPivotScript : MonoBehaviour
{
    [HideInInspector] public bool foundPlayer = false;
    private GameObject player;
    public LaserScript laserScript;
    public GameObject laserBeamWindup;
    public float speed;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(foundPlayer)
        {
            Vector3 direction = player.transform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);

            //transform.LookAt(player.transform);
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        laserBeamWindup.SetActive(true);

        yield return new WaitForSeconds(3f);

        laserScript.shootPlayer = true;
    }
}
