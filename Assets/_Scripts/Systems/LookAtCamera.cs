using UnityEngine;

[ExecuteInEditMode]
public class LookAtCamera : MonoBehaviour
{
    private Transform cam;

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.position, cam.up);
    }
}