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

    [SerializeField,ReadOnly] private int currentMoney;
    [SerializeField] private TMP_Text currentMoneyOfferText;
    [SerializeField,ReadOnly] private Item currentItem;


    private void InitCurrentMoney()
    {
        if (currentItem)
        {
            currentMoney = currentItem.estimatedPrice;
            currentMoneyOfferText.text = $"${currentMoney.ToString()}";
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

        var value = currentMoney / 100;
        currentMoney += value == 0 ? 1 : value;
        currentMoneyOfferText.text = $"${currentMoney.ToString()}";
    }

    public void DecreaseValue()
    {
        if (!currentItem)
        {
            return;
        }
        var value = currentMoney / 100;
        currentMoney -= value == 0 ? 1 : value;
        currentMoneyOfferText.text = $"${currentMoney.ToString()}";
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