using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderTazerManager : MonoBehaviour
{
    //Attack Vars   
    private float _tazerBurstNum;
    private float _tazerCounter;
    private float _tazerBurstSpeed;
    private float _tazerBurstTimer;
    private float _tazerPower;
    private float _rotation;
   
    //References
    private SpiderStats spiderStats;
    private SpiderBT spiderBT;
    private Transform _enemyTF;
    private Transform _playerTF;
    private Transform _boltSpawnPoint;
    private Rigidbody _boltPrefab;

    private void Start()
    {
        spiderStats = GetComponent<SpiderStats>();
        spiderBT = GetComponent<SpiderBT>();
        _enemyTF = spiderStats.enemyTF;
        _playerTF = spiderStats.playerTF;
        _boltSpawnPoint = spiderStats.tazerSpawnPoint;
        _tazerBurstNum = spiderStats.tazerBurstNum;
        _tazerBurstSpeed = spiderStats.tazerBurstSpeed;
        _tazerPower = spiderStats.tazerPower;
        _boltPrefab = spiderStats.boltPrefab;
        _rotation = spiderStats.tazerRotation;
    }

    public void StartTazerAttack()
    {
        ResetVars();
        StartCoroutine(TazerAttack());
    }

    private IEnumerator TazerAttack()
    {
        //Add edge case of dying
        while (_tazerCounter < _tazerBurstNum)
        {
            if (!spiderBT.isAlive)
            {
                yield break;
            }
            //Shoots a bolt at a given timer interval as many times as specified
            if (_tazerCounter < _tazerBurstNum && _tazerBurstTimer >= _tazerBurstSpeed)
            {
                _tazerCounter++;
                _tazerBurstTimer = 0;
                ShootBolt();
                AudioManager.instance.PlayOneShot(FMODEvents.instance.spiderTazer, this.transform.position);
            }
            //RotateTowardsPlayer();
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

    private void RotateTowardsPlayer()
    {
        Vector3 toPlayer = new Vector3(_playerTF.position.x - _enemyTF.position.x, 0, _playerTF.position.z - _enemyTF.position.z).normalized;
        float angle = Vector3.Angle(_enemyTF.forward, toPlayer);
        if (angle < _rotation)
        {
            _enemyTF.forward = toPlayer;
        }
        else
        {
            if (Vector3.Dot(_enemyTF.right, toPlayer) > Vector3.Dot(-_enemyTF.right, toPlayer))
            {
                _enemyTF.rotation = Quaternion.AngleAxis(_rotation, _enemyTF.up) * _enemyTF.rotation;
            }
            else
            {
                _enemyTF.rotation = Quaternion.AngleAxis(_rotation, -_enemyTF.up) * _enemyTF.rotation;
            }
        }
    }

    private void ResetVars()
    {
        _tazerBurstTimer = _tazerBurstSpeed;
        _tazerCounter = 0;
    }
}
