using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace _Game.Scripts
{
    public class GameController : MonoBehaviour
    {
        // [SerializeField] private List<Item> Items;
        public static GameController instance;


        public Inventory inventory;
        [SerializeField] public GameObject itemPrefab;

        [SerializeField] public Transform checkOutSlot;
        
        [SerializeField] public Movement movementController;

        [SerializeField] public StorageSlots storageSlots;
        
        
        
        private void Awake()
        {
            instance = this;
        }




        [SerializeField] private Image fadeImage;
        [SerializeField] private TMP_Text dailyProfitText;
        
        
        public async void StartDay()
        {

            inventory.day++;
            if (inventory.day > 7)
            {
                inventory.day = 0;
                inventory.week++;
            }

            if (inventory.week > 4)
            {
                inventory.week = 0;
                inventory.month++;
            }

            GameUIController.instance.UpdateDate();
            
            SetLightsOn();
            isDayEnded = false;

            await Task.Delay(5000);
            NpcController.instance.Start();

        }

        [SerializeField] private List<Light> lights;
        
        [Button]
        public void EndDay()
        {
            DOVirtual.Color(RenderSettings.ambientSkyColor ,Color.black, 4F,SetColor).OnComplete(SetLightsOff);
        }

        private void SetColor(Color value)
        {
            RenderSettings.ambientSkyColor = value;
        }

        private async void SetLightsOff()
        {
            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].enabled = false;
                await Task.Delay(500);
            }

            isDayEnded = true;
            GameUIController.instance.currentMoneyOfferText.color = Color.green;
            GameUIController.instance.currentMoneyOfferText.text = "Press F2 To Jump Next Day";
        }
        
        private async void SetLightsOn()
        {
            GameUIController.instance.currentMoneyOfferText.color = Color.white;
            GameUIController.instance.currentMoneyOfferText.text = "Waiting For Customer...";
            for (int i = lights.Count - 1; i >= 0; i--)
            {
                lights[i].enabled = true;
                await Task.Delay(500);
            }
            DOVirtual.Color(RenderSettings.ambientSkyColor ,Color.white, 4F,SetColor);
        }

        [SerializeField] private bool isDayEnded;
        
        private void Update()
        {
            if (isDayEnded)
            {
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    StartDay();
                }
            }
        }
    }

    [Serializable]
    public class Inventory
    {
        public int month;
        public int week;
        public int day;
        public int money = 2500;
        public int size = 10;
        [ReadOnly] public readonly List<Item> items = new ();
    }

}


