using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;



public class Npc : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private List<GameObject> models;


    private void Start()
    {
        PickRandomModel();
    }

    private void PickRandomModel()
    {
        
    }

    [Button]
    public void Move()
    {
        navMeshAgent.SetDestination(target.position);
    }

}
