using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class DeerBasicAttack : Node
{
    private Transform _playerTF; //Player transform
    private Transform _enemyTF;  //Enemy transform
    private BoxCollider _hitBox; //Hitbox of attack
    private float _attackLength; //How long the hitbox stays active
    private float _attackTimer;  //Used internally to time the attack
    private float _windupLength; //How long the windup for the attack is
    private float _windupTimer;  //Used internally to time the windup

    //Playtesting vars
    private GameObject _attackVisual;
    private Material _windupMaterial;
    private Material _attackMaterial;
    private Material _deerMaterial;
    private MeshRenderer _enemyMaterial;
    
    //The deer's basic attack, has a short windup and then sticks out a hitbox for a provided length - RYAN
    public DeerBasicAttack(BoxCollider hitBox, float attackLength, float basicWindup, Transform playerTF, Transform enemyTF, GameObject attackVisual,
        MeshRenderer enemyMaterial, Material windupMaterial, Material attackMaterial, Material deerMaterial)
    {
        _hitBox       = hitBox;
        _attackLength = attackLength;
        _attackTimer  = 0;
        _windupLength = basicWindup;
        _windupTimer  = 0;
        _playerTF     = playerTF;
        _enemyTF      = enemyTF;
        _attackVisual = attackVisual;
        _enemyMaterial = enemyMaterial;
        _windupMaterial = windupMaterial;
        _attackMaterial = attackMaterial;
        _deerMaterial = deerMaterial;
    }

    public override NodeState Evaluate()
    {
        //Windup of the attack, turns deer to look at player
        if (_windupTimer < _windupLength)
        { 
            Vector3 toPlayer = (_playerTF.position - _enemyTF.position).normalized;
            _enemyTF.forward = Vector3.Lerp(_enemyTF.forward, toPlayer, (_windupTimer / _windupLength) * 0.95f);
            _windupTimer    += Time.deltaTime;
            _enemyMaterial.material = _windupMaterial;
            SetData("basic", true);
            SetData("attacking", true);
            state = NodeState.RUNNING;
            return state;
        } 
        else
        {
            _enemyMaterial.material = _attackMaterial;
            //Checks if attack is over, resets all the vars
            if (_attackTimer >= _attackLength)
            {
                _attackTimer    = 0;
                _windupTimer    = 0;
                _hitBox.enabled = false;
                _attackVisual.SetActive(false);
                _enemyMaterial.material = _deerMaterial;
                ClearData("basic");
                ClearData("attacking");
                state = NodeState.SUCCESS;
                return state;
            }
            //Keeps all the hitboxes on and increments timer while attacking
            _hitBox.enabled = true;
            _attackVisual.SetActive(true);
            _attackTimer += Time.deltaTime;
            state = NodeState.RUNNING;
            return NodeState.RUNNING;
        }
    }
}
