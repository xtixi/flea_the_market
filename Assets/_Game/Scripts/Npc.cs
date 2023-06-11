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
using Random = UnityEngine.Random;


namespace _Game.Scripts
{

    public enum NpcTypes
    {
        Seller,
        Buyer
    }

    public class Npc : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private ModelSelector modelSelector;
        [SerializeField] public UnityEvent onResume;
        [SerializeField, ReadOnly] private NpcRoadSlot currentRoadSlot;
        [SerializeField] private NpcTypes npcType;
        [SerializeField,ReadOnly] private Item currentItem;
        private NpcModel _npcModel;

        private void Start()
        {
            InitCharacteristics();
            TryCreateItem();
        }

        private void TryCreateItem()
        {
            if (npcType is NpcTypes.Seller)
            {
                currentItem = Instantiate(GameController.instance.ItemPrefab,_npcModel.itemSlot).GetComponent<Item>();
                currentItem.transform.localPosition = Vector3.zero;
                currentItem.transform.localRotation = Quaternion.identity;
                currentItem.transform.localScale = Vector3.one;
                
            }
        }

        private void InitCharacteristics()
        {
            //todo kasaya gelene kadar envanter dolarsa veya bosalirsa nolcak ona bi bak

            if (GameController.instance.inventory.items.Count == 0 )
            {
                npcType = NpcTypes.Seller;
            }
            else if (GameController.instance.inventory.items.Count == GameController.instance.inventory.size)
            {
                npcType = NpcTypes.Buyer;
            }
            else
            {
                npcType = Random.Range(0, 2) == 0 ? NpcTypes.Buyer : NpcTypes.Seller;
            }
            _npcModel.animatorController.SetHolding(npcType is NpcTypes.Seller);
        }

        
        
        public void PickRandomModel()
        {
            _npcModel = modelSelector.SelectNpc();
        }


        [Button]
        public void Move(NpcRoadSlot npcRoadSlot)
        {
            if (currentRoadSlot)
            {
                currentRoadSlot.EmptySlot();
            }

            currentRoadSlot = npcRoadSlot;
            Move(npcRoadSlot.transform);
            npcRoadSlot.FillSlot();
        }

        public void Move(Transform npcRoadSlot)
        {
            _npcModel.animatorController.SetMoving(true);
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

            _npcModel.animatorController.SetMoving(false);
            transform.DORotateQuaternion(currentRoadSlot.transform.rotation, .5f);
            //todo
        }

        [Button]
        public void Resume()
        {
            if (currentRoadSlot)
                currentRoadSlot.EmptySlot();
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
}