using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera c_VCam;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.instance.isConversaiton)
        {
            c_VCam.m_Lens.OrthographicSize = 8;
        }
        else
        {
            c_VCam.m_Lens.OrthographicSize = 10;
        }
    }
}
