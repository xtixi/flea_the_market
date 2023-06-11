using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class KeyToButtonConverter : MonoBehaviour
{
    [SerializeField] private KeyCode keyCode;
    [SerializeField] private Button threeDButton;

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            Convert();
        }
    }

    public void Convert()
    {
        threeDButton.onClick.Invoke();

    }
}
