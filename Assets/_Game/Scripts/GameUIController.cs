using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Game.Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameUIController : MonoBehaviour
{
    public static GameUIController instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private GameObject topLeftPanel;
    [SerializeField] private float topLeftPanelXPosStart;
    [SerializeField] private float topLeftPanelXPosEnd;

    [SerializeField] private GameObject topRightPanel;
    [SerializeField] private float topRightPanelXPosStart;
    [SerializeField] private float topRightPanelXPosEnd;

    [SerializeField] private TMP_Text money;

    [SerializeField] private TMP_Text dateM;
    [SerializeField] private TMP_Text dateW;
    [SerializeField] private TMP_Text dateD;

    [SerializeField,ReadOnly] private int currentMoneyOffer;
    [SerializeField] private TMP_Text currentMoneyOfferText;
    [SerializeField,ReadOnly] internal Item currentItem;
    [SerializeField,ReadOnly] internal Npc currentNpc;
    [SerializeField] private int triedBidCount;
    

    public void TryBid()
    {
        if (!currentItem)
        {
            return;
        }

        triedBidCount++;
        if (currentNpc.npcType is NpcTypes.Seller)
        {
            if (currentMoneyOffer >= currentItem.estimatedPrice )
            {
                AcceptOrder();
            }
            else if (currentMoneyOffer < currentItem.estimatedPrice - currentItem.estimatedPrice / 10)
            {
                var a = Random.Range(0, 2);
                if(a == 1)
                    AcceptOrder();
                else
                    RejectOrder();
            }
            else if (currentMoneyOffer < currentItem.estimatedPrice - currentItem.estimatedPrice / 5)
            {
                var a = Random.Range(0, 5);
                if(a == 1)
                    AcceptOrder();
                else
                    RejectOrder();
            }
            else
            {
                RejectOrder();
            }
        }
    }

    private async void AcceptOrder()
    {
        if (!currentItem)
        {
            return;
        }
        triedBidCount = 0;

        var slot = GameController.instance.storageSlots.GetAvailableSlot();
        slot.FillSlot();
        currentNpc.MoveItem(slot.transform);
        await Task.Delay(100);
        currentNpc.Resume();
        GameController.instance.inventory.money -= currentMoneyOffer;
        InitCurrentMoney();
    }

    private void RejectOrder()
    {
        if (triedBidCount > 2)
        {
            LoseCustomer();
        }
    }

    private void LoseCustomer()
    {
        
    }

    internal void InitCurrentMoney()
    {
        if (currentItem)
        {
            currentMoneyOffer = currentItem.estimatedPrice;
            currentMoneyOfferText.text = $"${currentMoneyOffer.ToString()}";
        }
        else
        {
            currentMoneyOfferText.text = "Waiting For Customer...";
        }
    }

    public void IncreaseValue()
    {
        if (!currentItem)
        {
            return;
        }



        var value = currentMoneyOffer / 100;

        if (currentMoneyOffer + value > GameController.instance.inventory.money)
        {
            return;
        }
        
        currentMoneyOffer += value == 0 ? 1 : value;
        currentMoneyOfferText.text = $"${currentMoneyOffer.ToString()}";
    }

    public void DecreaseValue()
    {
        if (!currentItem)
        {
            return;
        }
        var value = currentMoneyOffer / 100;

        if (currentMoneyOffer - value <= 0)
        {
            return;
        }
        
        currentMoneyOffer -= value == 0 ? 1 : value;
        currentMoneyOfferText.text = $"${currentMoneyOffer.ToString()}";
    }


    public void ChangeMoneyText(int money)
    {
        this.money.text = $"{money.ToString()}";
    }

    public void ChangeDateText()
    {
    }

    public void OpenTopLeftPanel()
    {
        var position = topLeftPanel.transform.position;
        position = new Vector3(topLeftPanelXPosStart, position.y, position.z);
        topLeftPanel.transform.position = position;
        topLeftPanel.SetActive(true);
        topLeftPanel.transform.DOMoveX(topLeftPanelXPosEnd, .5f);
    }

    public void OpenTopRightPanel()
    {
        var position = topRightPanel.transform.position;
        position = new Vector3(topRightPanelXPosStart, position.y, position.z);
        topRightPanel.transform.position = position;
        topRightPanel.SetActive(true);
        topRightPanel.transform.DOMoveX(topRightPanelXPosEnd, .5f);
    }

    public void CloseTopLeftPanel()
    {
        topLeftPanel.transform.DOMoveX(topLeftPanelXPosStart, .5f).OnComplete(() => { topLeftPanel.SetActive(false); });
    }

    public void CloseTopRightPanel()
    {
        topRightPanel.transform.DOMoveX(topRightPanelXPosStart, .5f).OnComplete(() => { topRightPanel.SetActive(false); });
    }

    public void LetsSeeButtonTapped()
    {
        currentItem = NpcController.instance.npcCharactersOnRoad.First().MoveItemToCheckout();
        InitCurrentMoney();
    }

    public async void NoThanksButtonTapped()
    {
        currentItem = null;
        InitCurrentMoney();
        NpcController.instance.npcCharactersOnRoad.First().MoveItemBack();
        await Task.Delay(500);
        CloseTopLeftPanel();
        NpcController.instance.npcCharactersOnRoad.First().Resume();
    }
}