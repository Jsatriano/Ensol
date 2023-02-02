using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class DeerBasicAttack : Node
{
    private BoxCollider _hitBox;
    private float _attackLength;
    private float _attackTimer;
    private float _windupLength;
    private float _windupTimer;
    private Transform _playerTF;
    private Transform _enemyTF;
    private GameObject _attackVisual;
    private Material _windupMaterial;
    private Material _attackMaterial;
    private Material _deerMaterial;
    private MeshRenderer _enemyMaterial;
    

    public DeerBasicAttack(BoxCollider hitBox, float attackLength, float basicWindup, Transform playerTF, Transform enemyTF, GameObject attackVisual,
        MeshRenderer enemyMaterial, Material windupMaterial, Material attackMaterial, Material deerMaterial)
    {
        _hitBox = hitBox;
        _attackLength = attackLength;
        _attackTimer = 0;
        _windupLength = basicWindup;
        _windupTimer = 0;
        _playerTF = playerTF;
        _enemyTF = enemyTF;
        _attackVisual = attackVisual;
        _enemyMaterial = enemyMaterial;
        _windupMaterial = windupMaterial;
        _attackMaterial = attackMaterial;
        _deerMaterial = deerMaterial;
    }

    public override NodeState Evaluate()
    {
        if (_windupTimer < _windupLength)
        {
            SetData("basic", true);
            SetData("attacking", true);
            Vector3 toPlayer = (_playerTF.position - _enemyTF.position).normalized;
            _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, toPlayer, (_windupTimer / _windupLength) * 0.95f);
            _windupTimer += Time.deltaTime;
            _enemyMaterial.material = _windupMaterial;
            state = NodeState.RUNNING;
            return state;
        } 
        else
        {
            _enemyMaterial.material = _attackMaterial;
            if (_attackTimer >= _attackLength)
            {
                _attackTimer = 0;
                _windupTimer = 0;
                ClearData("basic");
                ClearData("attacking");
                _hitBox.enabled = false;
                _attackVisual.SetActive(false);
                _enemyMaterial.material = _deerMaterial;
                state = NodeState.SUCCESS;
                return state;
            }
            _hitBox.enabled = true;
            _attackVisual.SetActive(true);
            _attackTimer += Time.deltaTime;
            state = NodeState.RUNNING;
            return NodeState.RUNNING;
        }
    }
}
