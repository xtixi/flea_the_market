using System;
using UnityEngine;

namespace _Game.Scripts.Game
{
    public class Item : ScriptableObject
    {
        [SerializeField] private GameObject model;
        
    }

    [Serializable]
    public struct ItemInfo
    {
        [SerializeField] public string rarity;
        [SerializeField] public string condition;
        [SerializeField] public string category;
        [SerializeField] public int realPrice;
    }
}