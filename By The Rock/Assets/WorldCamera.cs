using UnityEngine;
using System.Collections;

public class WorldCamera : MonoBehaviour {
    
    public Transform target;
    public float height = 5.0f;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    float desiredDistance = 5.0f;
    
    float x = 0.0f;
    float y = 0.0f;

    private bool shoulderZoom;

    public UIManager UI;
    public GameObject canvas;

    public static bool shoulderView;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (target && !canvas.activeInHierarchy)
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
                shoulderView = true;
                target.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                distance = Mathf.Clamp(desiredDistance, distanceMin, 2);

                position = target.position - (rotation * Vector3.forward * distance) + transform.right * 1.3f + transform.up * 1.3f;
                cameraTargetPosition = new Vector3(target.position.x, target.position.y + height+3, target.position.z) + ((transform.right * 2));

            }
            else
            {
                shoulderView = false;
                // Collision Detection
                position = target.position - (rotation * Vector3.forward * distance);
                cameraTargetPosition = new Vector3(target.position.x, target.position.y + height, target.position.z);
            }
            
            RaycastHit collisionHit;
            
            if (Physics.Linecast(cameraTargetPosition, position, out collisionHit))
            {
                position = collisionHit.point;
                distance = Vector3.Distance(target.position, position);
            }
            transform.rotation = rotation;
            transform.position = position;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                shoulderZoom = !shoulderZoom;
            }



            /*

            //if (distance >= 5) camMax = transform.position;

            //distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
            desiredDistance = Mathf.Clamp(desiredDistance, distanceMin, distanceMax);
            distance = desiredDistance;
            
            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                distance = hit.distance;
                hitDistance = hit.distance;
            }
            //else distance = 5;
            else if (distance < 5)
            {
                if (Physics.Linecast(transform.position, camMax, out hit))
                {
                    //distance += hit.distance-hitDistance;
                    //distance += (5-distance)-hit.distance;
                    distance = hit.distance;

                }
                else
                {
                    distance = 5;
                }
            }
            else
            {
                //distance = desiredDistance;
            }

            Mathf.Clamp(distance, distanceMin, distanceMax);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + new Vector3(target.position.x, target.position.y + height, target.position.z);

            camMax = rotation * new Vector3(0.0f, 0.0f, -5.0f) + target.position;
            maxTarget.position = camMax;
            
            transform.rotation = rotation;
            transform.position = position;*/
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
