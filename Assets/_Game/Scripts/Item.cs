using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ModelSelector modelSelector;
        [SerializeField,ReadOnly] private ItemModel itemModel;
        [SerializeField,ReadOnly] public Rarities rarity;
        [SerializeField,ReadOnly] public Conditions condition;
        [SerializeField,ReadOnly] public Categories category;
        [SerializeField,ReadOnly] public int estimatedPrice;
        [SerializeField,ReadOnly] public int paidPrice;

        private void Start()
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