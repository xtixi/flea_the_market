using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using DG.Tweening;
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
        var npcModel = modelSelector.SelectNpc();
        _npcAnimatorController = npcModel.animatorController;
    }

    [Button]
    public void Move(NpcRoadSlot npcRoadSlot)
    {
        if (currentSlot)
        {
            currentSlot.EmptySlot();
        }
        currentSlot = npcRoadSlot;
        Move(npcRoadSlot.transform);
        npcRoadSlot.FillSlot();
    }

    public void Move(Transform npcRoadSlot)
    {
        _npcAnimatorController.SetMoving(true);
        navMeshAgent.SetDestination(npcRoadSlot.position);
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
        transform.DORotateQuaternion(currentSlot.transform.rotation,.5f);
        //todo
    }
    
    [Button]
    public void Resume()
    {
        if(currentSlot)
            currentSlot.EmptySlot();
        NpcController.instance.RemoveNpc(this);
        NpcController.instance.ReOrderAllNpc();
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
