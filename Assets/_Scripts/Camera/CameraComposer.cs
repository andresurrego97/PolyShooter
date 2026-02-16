using UnityEngine;

public class CameraComposer : MonoBehaviour
{
    public static CameraComposer instance;

    [SerializeField] private Camera cam;

    [Space]
    [SerializeField] private Vector2 deadZone = Vector2.zero;
    private Vector2 _deadZone;
    [SerializeField] private Vector2 smoothness = Vector3.one;

    private Transform target;
    private Teams team;
    private Vector2 targetToViewport;

    private void Awake()
    {
        instance = this;
    }

    public void Init(Transform _target, Teams _team)
    {
        target = _target;
        team = _team;

        switch (team)
        {
            case Teams.TeamA:
                transform.localPosition = new Vector3(1, 35, -33);
                transform.localEulerAngles = new Vector3(45, 0, 0);
                break;

            case Teams.TeamB:
                transform.localPosition = new Vector3(1, 35, 66);
                transform.localEulerAngles = new Vector3(45, 180, 0);
                break;
        }
    }

    private void Update()
    {
        if (target == null)
            return;

        _deadZone = deadZone * 0.5f;

        targetToViewport = cam.WorldToViewportPoint(target.position);
        targetToViewport.x -= 0.5f;
        targetToViewport.y -= 0.5f;

        switch (team)
        {
            case Teams.TeamA:
                DirectionA();
                break;

            case Teams.TeamB:
                DirectionB();
                break;
        }
    }

    private void DirectionA()
    {
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

    private void DirectionB()
    {
        if (targetToViewport.y > _deadZone.y) // Mover para arriba
        {
            transform.position = Vector3.Lerp(
                transform.position, new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z - ((targetToViewport.y - _deadZone.y) * 10)),
                Time.deltaTime * smoothness.y);
        }
        else if (targetToViewport.y < -_deadZone.y) // Mover para abajo
        {
            transform.position = Vector3.Lerp(
                transform.position, new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z + ((-_deadZone.y - targetToViewport.y) * 10)),
                Time.deltaTime * smoothness.y);
        }

        if (targetToViewport.x > _deadZone.x) // Mover para derecha
        {
            transform.position = Vector3.Lerp(
                transform.position, new Vector3(
                    transform.position.x - ((targetToViewport.x - _deadZone.x) * 10),
                    transform.position.y,
                    transform.position.z),
                Time.deltaTime * smoothness.x);
        }
        else if (targetToViewport.x < -_deadZone.x) // Mover para izquierda
        {
            transform.position = Vector3.Lerp(
                transform.position, new Vector3(
                    transform.position.x + ((-_deadZone.x - targetToViewport.x) * 10),
                    transform.position.y,
                    transform.position.z),
                Time.deltaTime * smoothness.x);
        }
    }
}