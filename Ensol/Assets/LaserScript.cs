using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [HideInInspector] public bool shootPlayer;

    public Transform startPoint;
    [HideInInspector] public Transform endPoint;
    LineRenderer laserLine;
    private bool fired = false;
    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.SetWidth(.4f, .4f);
        endPoint = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // casts line out from barrel to player and "shoots" them
        if(shootPlayer)
        {
            if(PlayerData.currHP <= 0)
            {
                laserLine.enabled = false;
            }
            if(PlayerData.hasShield) {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.playerShieldBreak, damageVFXLocation.transform.position);
                PlayerData.hasShield = false;
            }
            if (!fired){
                fired = true;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.turretFire, this.transform.position);
            }
            laserLine.SetPosition(0, startPoint.position);
            laserLine.SetPosition(1, new Vector3(endPoint.position.x, endPoint.position.y + 1f, endPoint.position.z));
            PlayerData.currHP = 0;
        }
    }
}
