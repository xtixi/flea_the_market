using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
// These videos take long to make so I hope this helps you out and if you want to help me out you can by leaving a like and subscribe, thanks!
 
public class Movement : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] public float mouseSensitivity = 3.5f;
    [SerializeField] float Speed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
 
 
    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;
    
    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector3 velocity;
 
    void Start()
    {
        controller = GetComponent<CharacterController>();
 
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }
 
    void Update()
    {
        UpdateMouse();
        UpdateMove();
    }
 
    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
 
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
 
        cameraCap -= currentMouseDelta.y * mouseSensitivity;
 
        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);
 
        playerCamera.localEulerAngles = Vector3.right * cameraCap;
 
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }
 
    void UpdateMove()
    {
 
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
 
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
 
 
        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * Speed + Vector3.up * -5;
 
        controller.Move(velocity * Time.deltaTime);
 

 

    }
}