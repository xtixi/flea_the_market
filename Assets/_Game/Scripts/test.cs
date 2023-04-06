using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts
{
    public class test : MonoBehaviour
    {
        [SerializeField] private List<GameObject> gameObjects;
        [SerializeField] private int index;

        public void ChangeModel()
        {
            gameObjects[gameObjects.Count% index].SetActive(false);
            index++;
            gameObjects[gameObjects.Count% index].SetActive(true);
        }
    }
}
