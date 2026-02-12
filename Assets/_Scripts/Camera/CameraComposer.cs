using UnityEngine;

public class CameraComposer : MonoBehaviour
{
    public static CameraComposer instance;

    [SerializeField] private Camera cam;
    public Transform target;

    [Space]
    [SerializeField] private Vector2 deadZone = Vector2.zero;
    private Vector2 _deadZone;
    [SerializeField] private Vector2 smoothness = Vector3.one;

    private Vector2 targetToViewport;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (target == null)
            return;

        _deadZone = deadZone * 0.5f;

        targetToViewport = cam.WorldToViewportPoint(target.position);
        targetToViewport.x -= 0.5f;
        targetToViewport.y -= 0.5f;

        if (targetToViewport.y > _deadZone.y) // Mover para arriba
        {
            transform.position = Vector3.Lerp(
                transform.position, new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z + ((targetToViewport.y - _deadZone.y) * 10)),
                Time.deltaTime * smoothness.y);
        }
        else if (targetToViewport.y < -_deadZone.y) // Mover para abajo
        {
            transform.position = Vector3.Lerp(
                transform.position, new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z - ((-_deadZone.y - targetToViewport.y) * 10)),
                Time.deltaTime * smoothness.y);
        }

        if (targetToViewport.x > _deadZone.x) // Mover para derecha
        {
            transform.position = Vector3.Lerp(
                transform.position, new Vector3(
                    transform.position.x + ((targetToViewport.x - _deadZone.x) * 10),
                    transform.position.y,
                    transform.position.z),
                Time.deltaTime * smoothness.x);
        }
        else if (targetToViewport.x < -_deadZone.x) // Mover para izquierda
        {
            transform.position = Vector3.Lerp(
                transform.position, new Vector3(
                    transform.position.x - ((-_deadZone.x - targetToViewport.x) * 10),
                    transform.position.y,
                    transform.position.z),
                Time.deltaTime * smoothness.x);
        }
    }
}