using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMover : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            SceneManager.LoadScene(1);
        }
    }
}
