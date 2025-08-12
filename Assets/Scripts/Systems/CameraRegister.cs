using Unity.Cinemachine;
using UnityEngine;


public class CameraRegister : MonoBehaviour
{
    void Awake()
    {
        GameManager.Instance.RegisterPlayerCamera(GetComponent<CinemachineCamera>());
    }
}
