using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WorldCamera : MonoBehaviour {
    
    public Transform target;
    public float height = 1.5f;
    public float aimHeight = 1.3f;
    public float aimRight = 1.3f;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public static float shoulderDistance;

    float desiredDistance = 5.0f;
    
    float x = 0.0f;
    float y = 0.0f;

    private bool shoulderZoom;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        Cursor.lockState = CursorLockMode.Locked;

        Object.DontDestroyOnLoad(this);
    }

    void LateUpdate()
    {
        if (target && !GameManager.instance.paused)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.05f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            desiredDistance = Mathf.Clamp(desiredDistance, distanceMin, distanceMax);

            distance = desiredDistance;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            Vector3 position = new Vector3(0, 0, 0);
            Vector3 cameraTargetPosition;

            if (shoulderZoom)
            {
                GameManager.instance.shoulderView = true;
                target.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                distance = Mathf.Clamp(desiredDistance, distanceMin, 1.5f);

                position = target.position - (rotation * Vector3.forward * distance) + transform.right * aimRight + Vector3.up * aimHeight;
                cameraTargetPosition = new Vector3(target.position.x, target.position.y + height + 3, target.position.z) + ((transform.right * 2));

            }
            else
            {
                // Collision Detection
                GameManager.instance.shoulderView = false;
                position = target.position - (rotation * Vector3.forward * distance) + Vector3.up * height;
                cameraTargetPosition = new Vector3(target.position.x, target.position.y + height, target.position.z);
            }

            RaycastHit collisionHit;

            if (Physics.Linecast(cameraTargetPosition, position, out collisionHit))
            {
                position = collisionHit.point;
                distance = Vector3.Distance(target.position, position);
//Debug.Log(distance);
            }
            transform.rotation = rotation;
            transform.position = position;

            shoulderDistance = 50;
            RaycastHit rayHit;
            Debug.DrawRay(transform.position, transform.forward * 50, Color.yellow);
            if (Physics.Raycast(transform.position, transform.forward, out rayHit, 50))
            {
                shoulderDistance = Vector3.Distance(transform.position, rayHit.point);
            }

            if (Input.GetButtonDown("AimMode") && !GameManager.instance.talking)
            {
                shoulderZoom = !shoulderZoom;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.UnloadScene("AITest");
            }
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

}
