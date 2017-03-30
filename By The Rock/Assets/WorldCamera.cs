using UnityEngine;
using System.Collections;

public class WorldCamera : MonoBehaviour {




    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    private Rigidbody rigidbody;
    private Vector3 camMax;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }

        camMax = transform.position;
    }

    void LateUpdate()
    {
        if (target)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            //if (distance >= 5) camMax = transform.position;

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
            

            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                distance -= hit.distance;
            }
            if (distance < 5)
            {
                if (Physics.Linecast(target.position, camMax, out hit))
                {
                    distance += 6-hit.distance;
                }
                else
                {
                    //distance = 5;
                }
            }
            
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            camMax = rotation * new Vector3(0.0f, 0.0f, 5) + target.position;
            
            transform.rotation = rotation;
            transform.position = position;
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
