using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using DG.Tweening;
using HighlightPlus;
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

    public class Npc : MonoBehaviour, IInteractable
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
                currentItem = Instantiate(GameController.instance.itemPrefab,_npcModel.itemSlot).GetComponent<Item>();
                currentItem.transform.localPosition = Vector3.zero;
                currentItem.transform.localRotation = Quaternion.identity;
                currentItem.transform.localScale = Vector3.one;
                
            }
        }


        public void MoveItemToCheckout()
        {
            currentItem.interactable = true;
            MoveItem(GameController.instance.checkOutSlot);
        }
        public void MoveItemBack()
        {
            currentItem.interactable = true;
            MoveItem(_npcModel.itemSlot);
        }

        private void MoveItem(Transform movePos)
        {
            currentItem.transform.SetParent(movePos);
            currentItem.transform.DOLocalJump(Vector3.zero, 1f,1,.5f) ;
            currentItem.transform.DOLocalRotate(Vector3.zero, .5f);
            currentItem.transform.DOScale(Vector3.one,.5f);
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

            if (currentRoadSlot.isCheckoutSlot)
            {
                canInteractable = true;
                // MoveItem(GameController.instance.checkOutSlot);
                // _npcModel.animatorController.SetHolding(false);

            }
        }

        [Button]
        public void Resume()
        {
            if (currentRoadSlot)
                currentRoadSlot.EmptySlot();
            canInteractable = false;
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


        [SerializeField] private HighlightEffect h;
        [SerializeField] private bool canInteractable;
        
        public void OnMouseDown()
        {
            if (!canInteractable)
            {
                return;
            }
            GameUIController.instance.OpenTopLeftPanel();
        }

        public void OnInteraction()
        {
            if (!canInteractable)
            {
                return;
            }
            if (currentRoadSlot.isCheckoutSlot)
            {
                h.enabled = true;
            }
        }

        public void UnInteraction()
        {
            h.enabled = false;
        }
    }
}