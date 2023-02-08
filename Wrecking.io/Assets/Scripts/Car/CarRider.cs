using UnityEngine;
using System.Collections.Generic;

public class CarRider : MonoBehaviour
{
    [Header("Axle Container")]
    [SerializeField] public List<AxleInfo> axleInfos;

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
    //[SerializeField] private float FlippingActivationAngle = 165f;
    [SerializeField] private Quaternion initialRotation = Quaternion.identity;
    [SerializeField] private float FlippedTimer = 1f;
    [SerializeField] private float counter = 1f;

    [Header("Wheel Hit")]
    [SerializeField] private WheelHit hit;
    private void Awake()
    {
        initialRotation = transform.rotation;
        LeftWheels = new WheelCollider[axleInfos.Count];
        RightWheels = new WheelCollider[axleInfos.Count];
        GetWheelColliders();
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

        //Debug.Log("Flipping Back");

        transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation,Time.deltaTime * FlipSpeed);

        if(Quaternion.Angle(transform.rotation, initialRotation) < StoppingAngle)
        {
            isFlipped = false;
            counter = FlippedTimer;
        }
    }
    private void DetectFlipped()
    {
        if (isFlipped) { return; }

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

        if( total_hits > RightWheels.Length + 1)
        {
            counter -= Time.deltaTime;

            Vector3 pos = transform.position;
            transform.position = new(pos.x, pos.y + (Time.deltaTime) , pos.z);

            if (counter <= 0)
            {
                isFlipped = true;
                counter = FlippedTimer;
            }
        }

        /*
         *  Retracted because of steering issues will try to use listening wheel colliders 
        if (Mathf.Abs(transform.eulerAngles.x) >= FlippingActivationAngle)
        {
            counter -= Time.deltaTime;
            
            if(counter <= 0)
            {
                isFlipped = true;
                counter = FlippedTimer;
            }
        }
        */
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