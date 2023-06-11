using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


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
        
        
        
        private void Awake()
        {
            instance = this;
        }
    
    }

    [Serializable]
    public class Inventory
    {
        public int size = 10;
        [ReadOnly] public readonly List<Item> items = new ();
    }

}


