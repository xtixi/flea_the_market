using System;
using System.Collections.Generic;
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
            
            
            
        }

        public async void EndDay()
        {
            
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


