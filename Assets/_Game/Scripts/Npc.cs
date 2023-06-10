using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class Npc : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private ModelSelector modelSelector;
    [SerializeField] public UnityEvent onResume;
    [SerializeField,ReadOnly] private NpcRoadSlot currentSlot;
    private NpcAnimatorController _npcAnimatorController;


    public void PickRandomModel()
    {
        var modelAndAnimator = modelSelector.InitModelAndAnimator();
        _npcAnimatorController = modelAndAnimator.Item2;
    }

    [Button]
    public void Move(NpcRoadSlot npcRoadSlot)
    {
        if (currentSlot)
        {
            currentSlot.EmptySlot();
        }
        _npcAnimatorController.SetMoving(true);
        currentSlot = npcRoadSlot;
        npcRoadSlot.FillSlot();
        navMeshAgent.SetDestination(npcRoadSlot.transform.position);
        StartCoroutine(StartMoving());
    }


    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
    private IEnumerator StartMoving()
    {
        while (!IsDestinationReached())
        {
            yield return _waitForEndOfFrame;
        }
        _npcAnimatorController.SetMoving(false);
        //todo
    }
    
    [Button]
    public void Resume()
    {
        if(currentSlot)
            currentSlot.EmptySlot();
        onResume?.Invoke();
    }

    private bool IsDestinationReached()
    {
        if (!navMeshAgent.enabled)
        {
            return false;
        }

        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnDisable()
    {
        onResume.RemoveAllListeners();
    }
}
