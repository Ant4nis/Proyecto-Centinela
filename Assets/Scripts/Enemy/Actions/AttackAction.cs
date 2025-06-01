using System;
using Enemy.FSM;
using UnityEngine;

namespace Enemy.Actions
{
    public class AttackAction : ActionFSM
    {

        private EnemyWeapon _enemyWeapon;
        private EnemyFSM _enemyFsm;
        private float _timer;

        private void Awake()
        {
            _enemyWeapon = GetComponent<EnemyWeapon>();
            _enemyFsm = GetComponent<EnemyFSM>();
        }

        private void Start()
        {
            if (_enemyWeapon.CurrentWeapon != null)
            {
                _timer = _enemyWeapon.CurrentWeapon.ItemWeapon.TimeBetweenAttacks;
            }
        }

        public override void ExecuteAction()
        {
            Attack();
        }

        private void Attack()
        {
            if (_enemyFsm.Player == null || _enemyWeapon.CurrentWeapon == null) return;

            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _timer = _enemyWeapon.CurrentWeapon.ItemWeapon.TimeBetweenAttacks;
                _enemyWeapon.TryShoot();
            }
            
        }
    }
}