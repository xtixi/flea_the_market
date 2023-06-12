using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Clicker : MonoBehaviour
{
    [SerializeField] private AudioSource audioListener;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioListener.pitch = Random.Range(.8f, 1.2f);
            audioListener.Play();
        }
    }
}
