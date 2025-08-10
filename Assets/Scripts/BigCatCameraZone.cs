using UnityEngine;
using Unity.Cinemachine;

public class CameraTriggerZone : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] private CinemachineCamera catCam;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            catCam.Priority = 20; // higher than your default camera
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            catCam.Priority = 0; // lower than default camera
        }
    }
}
