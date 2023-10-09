using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    public Transform cameraTarget;
    public float cameraSmoothnessFactor = 0.125f;

    private void FixedUpdate(){
        Vector3 desiredPosition = cameraTarget.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, cameraSmoothnessFactor);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
