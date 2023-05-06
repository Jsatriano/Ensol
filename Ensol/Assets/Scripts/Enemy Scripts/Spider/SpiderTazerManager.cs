using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderTazerManager : MonoBehaviour
{
    private enum State
    {
        IDLE,
        SHOOTING
    }

    private SpiderStats spiderStats;
    private SpiderBT spiderBT;
    private Transform _enemyTF;
    private Transform _boltSpawnPoint;
    private float _tazerBurstNum;
    private float _tazerCounter;
    private float _tazerBurstSpeed;
    private float _tazerBurstTimer;
    private float _tazerPower;
    private float _tazerDamage;
    private Rigidbody _boltPrefab;
    private State state = State.IDLE;

    private void Start()
    {
        spiderStats = GetComponent<SpiderStats>();
        spiderBT = GetComponent<SpiderBT>();
        _enemyTF = spiderStats.enemyTF;
        _boltSpawnPoint = spiderStats.tazerSpawnPoint;
        _tazerBurstNum = spiderStats.tazerBurstNum;
        _tazerBurstSpeed = spiderStats.tazerBurstSpeed;
        _tazerPower = spiderStats.tazerPower;
        _boltPrefab = spiderStats.boltPrefab;
        _tazerDamage = spiderStats.tazerDamage;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            //In this state when not doing the attack
            case State.IDLE:
                //Attack is starting, triggered by animation event
                if (spiderBT.root.GetData("startTazer") != null)
                {
                    spiderBT.root.ClearData("startTazer");
                    ResetVars();
                    state = State.SHOOTING;
                }
                break;

            case State.SHOOTING:
                //Shoots a bolt at a given timer interval as many times as specified
                if (_tazerCounter < _tazerBurstNum && _tazerBurstTimer >= _tazerBurstSpeed)
                {
                    _tazerCounter++;
                    _tazerBurstTimer = 0;
                    ShootBolt();
                }
                else
                {
                    if (_tazerCounter >= _tazerBurstNum)
                    {
                        spiderBT.root.SetData("tazerEnded", true);
                        state = State.IDLE;
                    }
                    else
                    {
                        _tazerBurstTimer += Time.deltaTime;
                    }
                }
                break;
        }
    }

    private void ShootBolt()
    {
        Rigidbody boltRB = Instantiate(_boltPrefab, _boltSpawnPoint.position, _boltSpawnPoint.rotation);
        TazerShot tazerScript = boltRB.GetComponent<TazerShot>();
        tazerScript.spiderTF = spiderStats.enemyTF;
        tazerScript.tazerDamage = spiderStats.tazerDamage;
        boltRB.AddForce(_enemyTF.forward * _tazerPower, ForceMode.Impulse);
    }

    private void ResetVars()
    {
        _tazerBurstTimer = _tazerBurstSpeed;
        _tazerCounter = 0;
    }
    /*
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_enemyTF, )
        }
    }
    */
}
