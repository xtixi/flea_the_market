using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    public int gameLevel;
    public int minBasePrice = 50;
    public int maxBasePrice = 150;
    
    
}
