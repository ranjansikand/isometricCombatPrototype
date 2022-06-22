
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private static CinemachineImpulseSource _impulseSource;

    void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public static void GenerateImpulse() {
        _impulseSource.GenerateImpulse();
    }
}
