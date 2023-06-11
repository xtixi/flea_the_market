using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using DG.Tweening;
using HighlightPlus;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class ThreeDButton : MonoBehaviour, IInteractable
{
    [SerializeField] private HighlightEffect highlightEffect;
    [SerializeField] private float clickedYPos;
    private bool _animLock;
    public UnityEvent onButtonClicked;

    public void OnMouseDown()
    {
        if (_animLock)
        {
            return;
        }
        onButtonClicked?.Invoke();
        _animLock = true;
        transform.DOLocalMoveY(clickedYPos,.05f).SetLoops(2, LoopType.Yoyo).OnComplete(() => { _animLock = false;});
    }

    public void OnInteraction()
    {
        highlightEffect.enabled = true;
    }

    public void UnInteraction()
    {
        highlightEffect.enabled = false;
    }
}
