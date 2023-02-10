using UnityEngine;
using System.Collections.Generic;
public class CarRider : MonoBehaviour
{
    //private static readonly string GroundTag = "Ground";

    [Header("Instance ID Given by LevelManager")]
    [SerializeField] private int Instance_ID;

    [Header("Axle Container")]
    [SerializeField] public List<AxleInfo> axleInfos;

    [Header("Local Components")]
    [SerializeField] private Rigidbody rb;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider[] LeftWheels = new WheelCollider[2];
    [SerializeField] private WheelCollider[] RightWheels = new WheelCollider[2];

    [Header("Controlls for the car")]
    [SerializeField] public float maxMotorTorque;
    [SerializeField] public float maxSteeringAngle;

    [Header("Flip Controlls")]
    [SerializeField] private bool isFlipped = false;
    [SerializeField] private float FlipForceMultiplier = 1f;
    [SerializeField] private ForceMode FlipForceMode = ForceMode.Impulse;
    [SerializeField] private float FlipSpeed = 10f;
    [SerializeField] private float StoppingAngle = 1f;
    [SerializeField] private float StartingAngle = 45;
    [SerializeField] private Quaternion initialRotation = Quaternion.identity;
    [SerializeField] private float FlippedTimer = 1f;
    [SerializeField] private float counter = 1f;

    [Header("Wheel Hit")]
    [SerializeField] private WheelHit hit;

    [Header("Fall Threshold")]
    [SerializeField] private float FallThreshold = -15f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = transform.rotation;
        LeftWheels = new WheelCollider[axleInfos.Count];
        RightWheels = new WheelCollider[axleInfos.Count];
        GetWheelColliders();
        Instance_ID = gameObject.GetInstanceID();
        LevelManager.AddList(gameObject);
    }
    private void Start()
    {
        counter = FlippedTimer;
    }
    private void FixedUpdate()
    {
        MouseMovement();
        //KeyboardMovement();
    }
    private void Update()
    {
        DetectFlipped();
        FlipCar();
        DetectFall();
    }
    private void MouseMovement()
    {
        Vector2 MousePos = ShowTouch.MouseControllerAxis;
        float x = MousePos.x;
        float y = MousePos.y;

        //Debug.Log("X: " + x + " Y: " + y + "");

        float motor = maxMotorTorque * x;
        float steering = maxSteeringAngle * y;

        //Debug.Log("(Mouse)Motor: " + motor + "");
        //Debug.Log("(Mouse)Steering: " + steering + "");

        for (int i = 0; i < axleInfos.Count; i++)
        {
            if (axleInfos[i].steering)
            {
                axleInfos[i].leftWheel.steerAngle = steering;
                axleInfos[i].rightWheel.steerAngle = steering;
            }
            if (axleInfos[i].motor)
            {
                axleInfos[i].leftWheel.motorTorque = motor;
                axleInfos[i].rightWheel.motorTorque = motor;
            }
        }

    }
    private void KeyboardMovement() // Basic Car Movement Script //
    {
        float x = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Horizontal");

        Debug.Log("X: " + x + " Y: " + y + "");

        float motor = maxMotorTorque * x;
        float steering = maxSteeringAngle * y;

        Debug.Log("(KeyBoard)Motor: " + motor + "");
        Debug.Log("(KeyBoard)Steering: " + steering + "");

        for (int i = 0; i < axleInfos.Count; i++)
        {
            if (axleInfos[i].steering)
            {
                axleInfos[i].leftWheel.steerAngle = steering;
                axleInfos[i].rightWheel.steerAngle = steering;
            }
            if (axleInfos[i].motor)
            {
                axleInfos[i].leftWheel.motorTorque = motor;
                axleInfos[i].rightWheel.motorTorque = motor;
            }
        }
    }
    private void GetWheelColliders()
    {
        for(int i=0;i<axleInfos.Count;i++)
        {
            LeftWheels[i] = axleInfos[i].leftWheel;
            RightWheels[i] = axleInfos[i].rightWheel;
        }
    }
    private void FlipCar()
    {
        if (!isFlipped) { return; }

        Quaternion rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * FlipSpeed);

        rb.MoveRotation(rotation);

        if(Quaternion.Angle(transform.rotation, initialRotation) < StoppingAngle)
        {
            isFlipped = false;
            counter = FlippedTimer;
        }
    }
    private void DetectFlipped()
    {
        if (isFlipped) { counter = FlippedTimer; return; }

        int total_hits = 0;

        for(int i=0;i<RightWheels.Length;i++)
        {
            if (!LeftWheels[i].GetGroundHit(out hit)) // Check if the wheelcollider is not colliding with something. If half of  them is not hitting anything will flip the car //
            {
                total_hits++;
            }
            if(!LeftWheels[i].GetGroundHit(out hit))
            {
                total_hits++;
            }
        }

        if(total_hits >= RightWheels.Length && Mathf.Abs(transform.rotation.eulerAngles.x) < StartingAngle)
        {
            counter -= Time.deltaTime;

            if (counter <= 0)
            {
                rb.AddRelativeForce(Vector3.up * FlipForceMultiplier, FlipForceMode); // Levitate the car a bit //
                isFlipped = true;
                counter = FlippedTimer;
            }
        }
        
    }
    private void DetectFall()
    {
        if (transform.position.y <= FallThreshold)
        {
            Destroy(this);
            // Game Over Screen //
        }
    }
    private void OnDestroy()
    {
        LevelManager.RemoveFromList(Instance_ID);
    }
}
[System.Serializable]
public class AxleInfo
{
    [Header("Wheel Docker")]
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;

    [Header("Wheel Controlls")]
    public bool motor;
    public bool steering;
}