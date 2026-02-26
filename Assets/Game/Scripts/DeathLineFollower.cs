using UnityEngine;

public sealed class DeathLineFollower : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float offsetBelowCamera = 7f;

    private void LateUpdate()
    {
        if (cameraTransform == null) return;

        var p = transform.position;
        p.y = cameraTransform.position.y - offsetBelowCamera;
        transform.position = p;
    }
}