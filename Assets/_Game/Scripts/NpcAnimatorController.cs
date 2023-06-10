using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAnimatorController : MonoBehaviour
{
    private Animator _animator;
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int IsHolding = Animator.StringToHash("IsHolding");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetMoving(bool state)
    {
        _animator.SetBool(IsMoving,state);
    }
    public void SetHolding(bool state)
    {
        _animator.SetBool(IsHolding,state);
    }
    
    
}
