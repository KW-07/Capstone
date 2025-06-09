using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity Settings")]
    public float mouseSensitivity = 100f;

    [Header("References")]
    public Transform playerBody;
    public Transform cameraRoot;
    public CinemachineVirtualCamera virtualCamera;

    [Header("UI Control")]
    public bool isUIActive = false;

    private float xRotation = 0f;

    private void Start()
    {
        UpdateCursorState();
    }

    private void Update()
    {
        UpdateCursorState();

        if(isUIActive)
        {
            return;
        }

        float mouseX = Input.GetAxis("MouseX") * mouseSensitivity * Time.deltaTime;


        float mouseY = Input.GetAxis("MouseY") * mouseSensitivity * Time.deltaTime;

        xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -15f, 15f);
        cameraRoot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);


        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void UpdateCursorState()
    {
        if(isUIActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState= CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SetUIActive(bool active)
    {
        isUIActive = active;
    }
}
