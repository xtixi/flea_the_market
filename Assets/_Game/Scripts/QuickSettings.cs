using System;
using System.Collections;
using _Game.Scripts;
using TMPro;
using UnityEngine;

public class QuickSettings : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text sensitivity;
    private bool isSettingsOpened;
    [SerializeField] private bool removeCursor ;
    
    private  IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        if (!GameController.instance)
        {
            yield break;
        }
        sensitivity.text = GameController.instance.movementController.mouseSensitivity.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isSettingsOpened)
            {
               OpenPanel();
            }
            else
            {
                ClosePanel();
            }
        }
    }

    public void OpenPanel()
    {
        isSettingsOpened = true;
        if (removeCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        panel.SetActive(true);
        Time.timeScale = 0f;
    }
    
    public void ClosePanel()
    {
        isSettingsOpened = false;
        Time.timeScale = 1f;
        panel.SetActive(false);
        if (removeCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

    }

    public void IncreaseSensitivity()
    {
        if (!GameController.instance)
        {
            return;
        }
        GameController.instance.movementController.mouseSensitivity += .25f;
        sensitivity.text = GameController.instance.movementController.mouseSensitivity.ToString();
    }

    public void DecreaseSensitivity()
    {
        if (!GameController.instance)
        {
            return;
        }
        GameController.instance.movementController.mouseSensitivity -= .25f;
        sensitivity.text = GameController.instance.movementController.mouseSensitivity.ToString();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}