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
    private Transform _enemyTF;
    private Transform _boltSpawnPoint;
    private float _tazerBurstNum;
    private float _tazerCounter;
    private float _tazerBurstSpeed;
    private float _tazerBurstTimer;
    private float _tazerPower;
    private Rigidbody _boltPrefab;
    private State state = State.IDLE;

    private void Start()
    {
        spiderStats = GetComponent<SpiderStats>();
        _enemyTF = spiderStats.enemyTF;
        _boltSpawnPoint = spiderStats.tazerSpawnPoint;
        _tazerBurstNum = spiderStats.tazerBurstNum;
        _tazerBurstSpeed = spiderStats.tazerBurstSpeed;
        _tazerPower = spiderStats.tazerPower;
        _boltPrefab = spiderStats.boltPrefab;
    }

    public void StartTazerAttack()
    {
        ResetVars();
        StartCoroutine(TazerAttack());
    }

    private IEnumerator TazerAttack()
    {
        while (_tazerCounter < _tazerBurstNum)
        {
            //Shoots a bolt at a given timer interval as many times as specified
            if (_tazerCounter < _tazerBurstNum && _tazerBurstTimer >= _tazerBurstSpeed)
            {
                _tazerCounter++;
                _tazerBurstTimer = 0;
                ShootBolt();
            }
            _tazerBurstTimer += Time.deltaTime;
            yield return null;
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
}
