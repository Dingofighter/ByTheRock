using UnityEngine;
using System.Collections;

public class WorldCamera : MonoBehaviour {

















    /*

    [Header("Camera Properties")]
    public float DistanceAway;                     //how far the camera is from the player.
    public float DistanceUp;                    //how high the camera is above the player
    public float smooth = 4.0f;                    //how smooth the camera moves into place
    public float rotateAround = 70f;            //the angle at which you will rotate the camera (on an axis)
    [Header("Player to follow")]
    public Transform target;                    //the target the camera follows
    [Header("Layer(s) to include")]
    public LayerMask CamOcclusion;                //the layers that will be affected by collision
    [Header("Map coordinate script")]
    //public worldVectorMap wvm;
    RaycastHit hit;
    float cameraHeight = 55f;
    float cameraPan = 0f;
    float camRotateSpeed = 180f;
    Vector3 camPosition;
    Vector3 camMask;
    Vector3 followMask;
    // Use this for initialization
    void Start()
    {
        //the statement below automatically positions the camera behind the target.
        rotateAround = target.eulerAngles.y - 45f;
    }
    void Update()
    {

    }
    // Update is called once per frame

    void LateUpdate()
    {
        //Offset of the targets transform (Since the pivot point is usually at the feet).
        Vector3 targetOffset = new Vector3(target.position.x, (target.position.y + 2f), target.position.z);
        Quaternion rotation = Quaternion.Euler(cameraHeight, rotateAround, cameraPan);
        Vector3 vectorMask = Vector3.one;
        Vector3 rotateVector = rotation * vectorMask;
        //this determines where both the camera and it's mask will be.
        //the camMask is for forcing the camera to push away from walls.
        camPosition = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;
        camMask = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;

        occludeRay(ref targetOffset);
        smoothCamMethod();

        transform.LookAt(target);

        #region wrap the cam orbit rotation
        if (rotateAround > 360)
        {
            rotateAround = 0f;
        }
        else if (rotateAround < 0f)
        {
            rotateAround = (rotateAround + 360f);
        }
        #endregion

        rotateAround += Input.GetAxis("Mouse X") * camRotateSpeed * 0.07f; //* Time.deltaTime;
        DistanceUp = Mathf.Clamp(DistanceUp += Input.GetAxis("Mouse Y"), -0.79f, 2.3f);
    }
    void smoothCamMethod()
    {
        smooth = 4f;
        transform.position = Vector3.Lerp(transform.position, camPosition, Time.deltaTime * smooth);
    }
    void occludeRay(ref Vector3 targetFollow)
    {
        #region prevent wall clipping
        //declare a new raycast hit.
        RaycastHit wallHit = new RaycastHit();
        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
        if (Physics.Linecast(targetFollow, camMask, out wallHit, CamOcclusion))
        {
            //the smooth is increased so you detect geometry collisions faster.
            smooth = 10f;
            //the x and z coordinates are pushed away from the wall by hit.normal.
            //the y coordinate stays the same.
            camPosition = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, camPosition.y, wallHit.point.z + wallHit.normal.z * 0.5f);
        }
        #endregion
    }

    */

    


    
    
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


            Vector3 position = target.position - (rotation * Vector3.forward * distance);            // Collision Detection
            RaycastHit collisionHit;
            Vector3 cameraTargetPosition = new Vector3(target.position.x, target.position.y + height, target.position.z);
            if (Physics.Linecast(cameraTargetPosition, position, out collisionHit))
            {
                position = collisionHit.point;
                distance = Vector3.Distance(target.position, position);
            }
            transform.rotation = rotation;
            transform.position = position;





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

























    /*
    public Transform target;
    public float angularSpeed;

    [SerializeField]
    [HideInInspector]
    private Vector3 initialOffset;

    private Vector3 currentOffset;

    [ContextMenu("Set Current Offset")]
    private void SetCurrentOffset()
    {
        if (target == null)
        {
            return;
        }

        initialOffset = transform.position - target.position;
    }

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Assign a target for the camera in Unity's inspector");
        }

        currentOffset = initialOffset;
    }

    private void LateUpdate()
    {
        transform.position = target.position + currentOffset;

        float movementY = Input.GetAxis("Mouse Y") * angularSpeed * Time.deltaTime;
        float movementX = Input.GetAxis("Mouse X") * angularSpeed * Time.deltaTime;

        if (!Mathf.Approximately(movementX, 0f) && !Mathf.Approximately(movementY, 0f))
        {
            transform.RotateAround(target.position, new Vector3(0, 1, 0), movementX);
            
            //transform.Rotate(new Vector3(1, 0, 0), -movementY);
            
            transform.RotateAround(target.position, new Vector3(transform.eulerAngles.y / 360, 0, 1-transform.eulerAngles.y / 360), movementY);
            
            
            currentOffset = transform.position - target.position;
        }
    }

    */















    /*
    public GameObject player;
    private Vector3 offsetX;
    private Vector3 offsetY;
    public float rotationSpeed = 5;
    public float turnSpeed = 4.0f;

    private float rotationY = 0f;

    // Use this for initialization
    void Start () {

        player = GameObject.Find("Player");

        //offset = transform.position - player.transform.position;
        offsetX = new Vector3(player.transform.position.x, player.transform.position.y + 8.0f, player.transform.position.z + 7.0f);
        offsetY = new Vector3(player.transform.position.x, player.transform.position.y + 8.0f, player.transform.position.z + 7.0f);
    }
	
	// Update is called once per frame
	void Update () {

        rotationY = Input.GetAxis("Mouse Y") * 1;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        //transform.position = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
        
        offsetX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offsetX;
        offsetY = Quaternion.AngleAxis(rotationY * turnSpeed, Vector3.right) * offsetY;

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), rotationSpeed * Time.deltaTime);
        transform.position = player.transform.position + offsetX + offsetY;
        transform.LookAt(player.transform.position);

    }

    */
}
