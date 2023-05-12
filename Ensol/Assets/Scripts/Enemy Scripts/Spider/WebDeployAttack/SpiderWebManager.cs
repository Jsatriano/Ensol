using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebManager : MonoBehaviour
{
    private SpiderStats spiderStats;
    private SpiderBT spiderBT;
    private Transform _enemyTF;
    private GameObject webPrefab;
    private Transform webSpawnPoint;
    private float speedDebuff;
    private float debuffLength;
    private float webDuration;

    private void Start()
    {
        spiderStats = GetComponent<SpiderStats>();
        spiderBT = GetComponent<SpiderBT>();
        _enemyTF = spiderStats.enemyTF;
        webPrefab = spiderStats.webPrefab;
        webSpawnPoint = spiderStats.webSpawnPoint;
        speedDebuff = spiderStats.webDeployDebuff;
        debuffLength = spiderStats.webDeployDebuffLength;
        webDuration = spiderStats.webDeployDuration;
    }

    public void StartWebDeployAttack()
    {
        StartCoroutine(WebDeployAttack());
    }

    private IEnumerator WebDeployAttack()
    {
        //Add edge case of dying
        while (spiderBT.root.GetData("dropWeb") == null && spiderBT.isAlive)
        {
            yield return null;
        }
        GameObject web = Instantiate(webPrefab, webSpawnPoint.position, transform.rotation);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.spiderWeb, this.transform.position);
        GroundWeb webScript = web.GetComponent<GroundWeb>();
        webScript.spiderTF = _enemyTF;
        webScript.speedDebuff = speedDebuff;
        webScript.debuffLength = debuffLength;
        webScript.webDuration = webDuration;
        spiderBT.root.ClearData("dropWeb");
    }
}
