using UnityEngine;
using System.Collections;

public class WorldCamera : MonoBehaviour {
    
    public Transform target;
    public Transform maxTarget;
    public float height = 5.0f;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    float desiredDistance = 5.0f;

   // private Rigidbody rigidbody;
    private Vector3 camMax;

    float x = 0.0f;
    float y = 0.0f;

    private float hitDistance;

    private bool shoulderZoom;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        //rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        /*if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }*/

        hitDistance = 0;
        camMax = transform.position;
    }

    void LateUpdate()
    {
        if (target)
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
                distance = Mathf.Clamp(desiredDistance, distanceMin, 2);
                position = target.position - (rotation * Vector3.forward * distance) + new Vector3(0.7f, 1, 0);
                cameraTargetPosition = new Vector3(target.position.x+0.7f, target.position.y + height+1, target.position.z);

            }
            else
            {
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
