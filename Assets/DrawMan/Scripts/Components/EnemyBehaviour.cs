using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using DrawMan.Core;

namespace DrawMan.Components
{
    [System.Serializable]
    public struct EnemyStats
    {
        [SerializeField] [Min(0)] private float InteractionRange;
        [SerializeField] [Min(0)] private float MoveSpeed;
        [SerializeField] [Min(0)] private float DamagePerHit;
        [SerializeField] [Min(0)] private float AttackSpeed;
    }

    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private EnemyStats Stats;
        [SerializeField] State[] InStates;

        FiniteStateMachine machine;

        public void Awake()
        {
            machine = new FiniteStateMachine(gameObject, InStates, (int)EnemyBStates.Idle);
        }

        /*public void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                machine.ChangeState((int)EnemyBStates.Idle);
            if (Input.GetKeyDown(KeyCode.B))
                machine.ChangeState((int)EnemyBStates.Battle);

            if (Input.GetKeyDown(KeyCode.Space))
                machine.CurrentState.Execute();
        }*/
    }
}
