using UnityEngine;
using System.Collections.Generic;
public class CarRider : MonoBehaviour
{
    private static readonly string GroundTag = "Ground";

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
    [SerializeField] private float FlipSpeed = 10f;
    [SerializeField] private float StoppingAngle = 5f;
    [SerializeField] private Quaternion initialRotation = Quaternion.identity;
    [SerializeField] private float FlippedTimer = 1f;
    [SerializeField] private float counter = 1f;

    [Header("Wheel Hit")]
    [SerializeField] private WheelHit hit;

    [Header("Fall Threshold")]
    [SerializeField] private float FallThreshold = -15f;

    [Header("Ray Attributes")]
    [SerializeField] private Vector3 RayDirection = Vector3.down;
    [SerializeField] private float VectorLenght = 2f;
    [SerializeField] private RaycastHit RayCastHit;
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
        Movement();
    }
    private void Update()
    {
        DetectFlipped();
        FlipCar();
        DetectFall();
        Debug.DrawRay(transform.position,(Vector3.down * VectorLenght), Color.blue);
    }
    private void Movement() // Basic Car Movement Script //
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        for (int i=0;i< axleInfos.Count;i++)
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

        transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation,Time.deltaTime * FlipSpeed);

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

        if(total_hits >= RightWheels.Length)
        {
            Physics.Raycast(transform.position, RayDirection, out RayCastHit, VectorLenght); // Cast a raycast to see if the car is levitating

            if (RayCastHit.collider.gameObject.CompareTag(GroundTag)) { return; }

            counter -= Time.deltaTime;

            if (counter <= 0)
            {
                isFlipped = true;
                rb.AddRelativeForce(Vector3.up,ForceMode.Impulse); // Levitate the car a bit //
                counter = FlippedTimer;
            }
        }
    }
    private void DetectFall()
    {
        if (transform.position.y <= FallThreshold)
        {
            Destroy(this);
            // Game Over //
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