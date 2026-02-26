using UnityEngine;

public sealed class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offsetY = 2.5f;

    private float _maxY;

    private void LateUpdate()
    {
        if (target == null) return;

        float desiredY = target.position.y + offsetY;
        if (desiredY > _maxY)
        {
            _maxY = desiredY;
            var p = transform.position;
            p.y = _maxY;
            transform.position = p;
        }
    }

    public void ResetFollow(float startY)
    {
        _maxY = startY;
        var p = transform.position;
        p.y = startY;
        transform.position = p;
    }
}