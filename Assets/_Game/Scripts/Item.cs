using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
    public class Item : ScriptableObject
    {
        [SerializeField] private ModelSelector modelSelector;
        [SerializeField] private ItemModel itemModel;
        [SerializeField] public int paidPrice;
        [SerializeField] public Rarities rarity;
        [SerializeField] public Conditions condition;
        [SerializeField] public Categories category;
        [SerializeField] public int estimatedPrice;
        private void Awake()
        {
            itemModel = modelSelector.InitModel().GetComponent<ItemModel>();
            InitItemVariables();
        }

        private void InitItemVariables()
        {
            var values = Enum.GetValues(typeof(Rarities));
            rarity = (Rarities)values.GetValue(UnityEngine.Random.Range(0, values.Length));
            
            var values2 = Enum.GetValues(typeof(Conditions));
            condition = (Conditions)values2.GetValue(UnityEngine.Random.Range(0, values2.Length));
            
            var values3 = Enum.GetValues(typeof(Categories));
            category = (Categories)values3.GetValue(UnityEngine.Random.Range(0, values3.Length));

            estimatedPrice = GameManager.instance.gameLevel *
                             Random.Range(GameManager.instance.minBasePrice, GameManager.instance.maxBasePrice);
        }



    }


    public enum Rarities
    {
        Common,
        Uncommon,
        Rare,
        Mythical,
        Legendary,
        Immortal
    }

    public enum Conditions
    {
        VeryBad,
        Bad,
        Normal,
        Good,
        VeryGood,
        Perfect
    }

    public enum Categories
    {
        Wearable,
        Culture,
        Leisure,
        Weapon
    }
}