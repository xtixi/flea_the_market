using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts
{
    public class test : MonoBehaviour
    {
        void Start()
        {
            transform.DOMove(Vector3.one* 10, 1f);
        }
    }
}
