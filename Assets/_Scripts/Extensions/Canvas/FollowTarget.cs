using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 offset;

    private Camera cam;
    private Vector3 position;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        position = cam.WorldToScreenPoint(target.position);
        position.x += offset.x;
        position.y += offset.y;
        position.z = 0;

        transform.position = position;
    }
}