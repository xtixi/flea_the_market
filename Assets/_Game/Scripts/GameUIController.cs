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
using UnityEngine.UI;
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
    [SerializeField] private List<GameObject> topLeftPanelButtons;
    private bool panelLock;


    // [SerializeField] private GameObject topRightPanel;
    // [SerializeField] private float topRightPanelXPosStart;
    // [SerializeField] private float topRightPanelXPosEnd;

    [SerializeField] private TMP_Text money;

    [SerializeField] private TMP_Text dateM;
    [SerializeField] private TMP_Text dateW;
    [SerializeField] private TMP_Text dateD;

    [SerializeField, ReadOnly] private int currentMoneyOffer;
    [SerializeField] private TMP_Text currentMoneyOfferText;
    [SerializeField, ReadOnly] internal Item currentItem;
    [SerializeField, ReadOnly] internal Npc currentNpc;
    [SerializeField] private int triedBidCount;


    [SerializeField] private TMP_Text rarityText;
    [SerializeField] private TMP_Text conditionText;
    [SerializeField] private TMP_Text categoryText;
    [SerializeField] private TMP_Text estimateText;

    [SerializeField] private TMP_Text paidText;
    // [SerializeField] private TMP_Text nowText;

    [SerializeField] private ParticleSystem moneyParticle;


    private void Start()
    {
        InitItemValues(null);
        UpdateEarnedMoneyText();
        UpdateDate();
    }

    private void UpdateEarnedMoneyText()
    {
        money.text = $"${GameController.instance.inventory.money}";
    }

    private void UpdateDate()
    {
        dateD.text = GameController.instance.inventory.day.ToString();
        dateW.text = GameController.instance.inventory.week.ToString();
        dateM.text = GameController.instance.inventory.month.ToString();
    }

    public void InitItemValues(Item item)
    {
        if (!currentNpc || !item)
        {
            rarityText.text = $"Rarity: ???";
            conditionText.text = $"Condition: ???";
            categoryText.text = $"Category: ???";
            estimateText.text = $"Estimate Price: ???";
            paidText.text = $"Paid: ???";
            return;
        }

        paidText.gameObject.SetActive(currentNpc.npcType is NpcTypes.Buyer);

        rarityText.text = $"Rarity: {item.rarity}";
        conditionText.text = $"Condition: {item.condition}";
        categoryText.text = $"Category: {item.category}";
        estimateText.text = $"Estimate Price: ${item.estimatedPrice}";
        paidText.text = $"Paid: {item.paidPrice}";
    }

    public void TryBid()
    {
        if (!currentItem)
        {
            return;
        }

        triedBidCount++;
        if (currentNpc.npcType is NpcTypes.Seller)
        {
            if (currentMoneyOffer >= currentItem.estimatedPrice)
            {
                AcceptOrder();
            }
            else if (currentMoneyOffer > currentItem.estimatedPrice - currentItem.estimatedPrice / 10)
            {
                var a = Random.Range(0, 3);
                if (a == 1)
                    AcceptOrder();
                else
                    RejectOrder();
            }
            else if (currentMoneyOffer > currentItem.estimatedPrice - currentItem.estimatedPrice / 5)
            {
                var a = Random.Range(0, 10);
                if (a == 1)
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

        CloseTopLeftPanel();
        seeLock = false;
        currentNpc.happyParticle.Play();

        GameController.instance.inventory.items.Add(currentItem);
        moneyParticle.Play();
        currentItem.paidPrice = currentMoneyOffer;
        GameController.instance.inventory.money -= currentMoneyOffer;
        UpdateEarnedMoneyText();
        currentMoneyOffer = 0;
        triedBidCount = 0;

        var slot = GameController.instance.storageSlots.GetAvailableSlot();
        slot.FillSlot();
        currentNpc.MoveItem(slot.transform, 1f);
        await Task.Delay(1000);
        currentNpc.Resume();
        GameController.instance.inventory.money -= currentMoneyOffer;
        InitCurrentMoney();
    }

    private void RejectOrder()
    {
        currentNpc.angryParticle.Play();
        if (triedBidCount > 2)
        {
            triedBidCount = 0;
            LoseCustomer();
        }
    }

    private void LoseCustomer()
    {
        NoThanksButtonTapped();
    }

    internal void InitCurrentMoney()
    {
        if (currentItem)
        {
            currentMoneyOffer = GameController.instance.inventory.money <= currentItem.estimatedPrice
                ? GameController.instance.inventory.money
                : currentItem.estimatedPrice;
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

    [SerializeField] private Image leftPanelColorImage;
    [SerializeField] private TMP_Text leftPanelDescriptionText;
    [SerializeField] private TMP_Text leftPanelLabelText;

    private void InitLeftPanelText(NpcTypes npcType)
    {
        leftPanelColorImage.color = npcType is NpcTypes.Buyer ? Color.green : Color.red;
        leftPanelDescriptionText.text = npcType is NpcTypes.Buyer ? "Hi, I want to buy this item!" : "Hey, I want to sell this item!";
        leftPanelLabelText.text = npcType is NpcTypes.Buyer ? "Buyer" : "Seller";
    }

    public void OpenTopLeftPanel()
    {
        if (panelLock)
        {
            return;
        }

        InitLeftPanelText(currentNpc.npcType);
        
        
        panelLock = true;
        topLeftPanelButtons.ForEach(x => x.transform.localScale = Vector3.one);
        var position = topLeftPanel.transform.position;
        position = new Vector3(topLeftPanelXPosStart, position.y, position.z);
        topLeftPanel.transform.position = position;
        topLeftPanel.SetActive(true);
        topLeftPanel.transform.DOMoveX(topLeftPanelXPosEnd, .5f);
    }

    public void CloseTopLeftPanel()
    {
        panelLock = false;
        topLeftPanel.transform.DOMoveX(topLeftPanelXPosStart, .5f).OnComplete(() => { topLeftPanel.SetActive(false); });
    }

    private bool seeLock;

    public void LetsSeeButtonTapped()
    {
        if (seeLock)
        {
            return;
        }

        topLeftPanelButtons.First().transform.DOScale(1.2f, .2f).SetLoops(2, LoopType.Yoyo).From(1f).OnComplete(() =>
        {
            topLeftPanelButtons.ForEach(x => x.transform.DOScale(0, .2f).From(1f));
        });
        seeLock = true;
        currentItem = NpcController.instance.npcCharactersOnRoad.First().MoveItemToCheckout();
        InitCurrentMoney();
    }

    public async void NoThanksButtonTapped()
    {
        seeLock = false;
        if (currentItem)
        {
            NpcController.instance.npcCharactersOnRoad.First().MoveItemBack();
        }

        topLeftPanelButtons.Last().transform.DOScale(1.2f, .2f).SetLoops(2, LoopType.Yoyo).From(1f).OnComplete(() =>
        {
            topLeftPanelButtons.ForEach(x => x.transform.DOScale(0, .2f).From(1f));
        });

        currentItem = null;
        InitCurrentMoney();
        await Task.Delay(500);
        CloseTopLeftPanel();
        NpcController.instance.npcCharactersOnRoad.First().Resume();
    }
}