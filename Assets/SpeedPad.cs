using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpeedPad : MonoBehaviour {

    public CinemachineVirtualCamera followCamera;

    [Range(0, 5)]
    public float duration = 1f;
    [Range(0,50)]
    public float boostAmount;
    public float zoomAmount = 40;

    private float originalFOV;

    private bool isBoostFOV;

    void Awake()
    {
        originalFOV = followCamera.m_Lens.FieldOfView;
    }
    void OnTriggerEnter(Collider other)
    {
        var rb = other.attachedRigidbody;
        if (rb == null) return;
        var kart = rb.GetComponent<VehicleMovement>();
        kart.StartCoroutine(KartModifier(kart, duration));
    }

    IEnumerator KartModifier(VehicleMovement kart, float lifetime)
    {
        kart.driveForce += boostAmount;
        isBoostFOV = true;
        yield return new WaitForSeconds(lifetime);
        kart.driveForce -= boostAmount;
        isBoostFOV = false;
    }

    void Update()
    {
        if (followCamera.m_Lens.FieldOfView != originalFOV)
        {
            if (isBoostFOV)
            {
                followCamera.m_Lens.FieldOfView = Mathf.Lerp(followCamera.m_Lens.FieldOfView, originalFOV + zoomAmount, Time.deltaTime / duration);
            }
            else
            {
                followCamera.m_Lens.FieldOfView = Mathf.Lerp(followCamera.m_Lens.FieldOfView, originalFOV, Time.deltaTime / duration);
            }
        }
    }
}
