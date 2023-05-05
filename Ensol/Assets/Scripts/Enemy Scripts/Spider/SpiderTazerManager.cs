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
    private Transform _playerTF;
    private Transform _enemyTF;
    private Transform _boltSpawnPoint;
    private float _tazerBurstNum;
    private float _tazerCounter;
    private float _tazerBurstSpeed;
    private float _tazerBurstTimer;
    private float _tazerPower;
    private float _tazerRotation;
    private Rigidbody _boltPrefab;
    private Vector3 _target;
    private Vector3 _toPlayer;
    private LayerMask _obstacleMask;
    private State state = State.IDLE;

    private void Start()
    {
        spiderStats = GetComponent<SpiderStats>();
        spiderBT = GetComponent<SpiderBT>();
        _playerTF = spiderStats.playerTF;
        _enemyTF = spiderStats.enemyTF;
        _boltSpawnPoint = spiderStats.tazerSpawnPoint;
        _tazerBurstNum = spiderStats.tazerBurstNum;
        _tazerBurstSpeed = spiderStats.tazerBurstSpeed;
        _tazerPower = spiderStats.tazerPower;
        _tazerRotation = spiderStats.tazerRotation;
        _boltPrefab = spiderStats.boltPrefab;
        _obstacleMask = spiderStats.obstacleMask;

    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.IDLE:
                if (spiderBT.root.GetData("startTazer") != null)
                {
                    spiderBT.root.ClearData("startTazer");
                    ResetVars();
                    state = State.SHOOTING;
                }
                break;

            case State.SHOOTING:
                RotateSpider();
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

    private void RotateSpider()
    {
        //Picking the spider's target (either the player or one of their breadcrumbs)
        _target = _playerTF.position;
        List<Vector3> breadcrumbs = (List<Vector3>)spiderBT.root.GetData("breadcrumbs");
        //Uses player's current position as the target if they are currently in FOV
        if (breadcrumbs == null || !Physics.Linecast(_playerTF.position, _playerTF.position, _obstacleMask))
        {
            _target = _playerTF.position;
        }
        //Else use the breadcrumbs
        else
        {
            bool foundTarget = false;
            //Check if any of the breadcrumbs are in FOV, starting at the most recent one
            for (int i = breadcrumbs.Count - 1; i >= 0; i--)
            {
                if (!Physics.Linecast(_playerTF.position, breadcrumbs[i], _obstacleMask))
                {
                    _target = breadcrumbs[i];
                    foundTarget = true;
                    break;
                }
            }
            //Use oldest breadcrumb as target if none are within FOV
            if (!foundTarget)
            {
                if (breadcrumbs.Count <= 0)
                {
                    _target = _playerTF.position;
                }
                else
                {
                    _target = breadcrumbs[0];
                }
            }
        }
        _toPlayer = new Vector3(_target.x - _playerTF.position.x, 0, _target.z - _playerTF.position.z).normalized;
        _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, _toPlayer, _tazerRotation);
    }

    private void ShootBolt()
    {
        Rigidbody boltRB = Instantiate(_boltPrefab, _boltSpawnPoint.position, _boltSpawnPoint.rotation);
        boltRB.AddForce(_boltSpawnPoint.forward * _tazerPower, ForceMode.Impulse);
    }

    private void ResetVars()
    {
        _tazerBurstTimer = _tazerBurstSpeed;
        _tazerCounter = 0;
    }
}
