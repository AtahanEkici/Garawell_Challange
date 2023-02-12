using UnityEngine;
using System.Collections.Generic;

public class CarRider : MonoBehaviour
{
    [Header("Instance ID Given by LevelManager")]
    [SerializeField] private int Instance_ID;

    [Header("Axle Container")]
    [SerializeField] public List<AxleInfo> axleInfos;

    [Header("Local Components")]
    [SerializeField] public Rigidbody rb;

    [Header("Wheel Colliders")]
    [SerializeField] public WheelCollider[] LeftWheels = new WheelCollider[2];
    [SerializeField] public WheelCollider[] RightWheels = new WheelCollider[2];

    [Header("Controlls for the car")]
    [SerializeField] public float maxMotorTorque;
    [SerializeField] public float maxSteeringAngle;
    [SerializeField] public float maxBrakeTorque;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        LeftWheels = new WheelCollider[axleInfos.Count];
        RightWheels = new WheelCollider[axleInfos.Count];
        GetWheelColliders();
        Instance_ID = gameObject.GetInstanceID();
        LevelManager.AddList(gameObject);
    }
    private void FixedUpdate()
    {
        MouseMovement();
    }
    private void MouseMovement()
    {
        Vector2 MousePos = ShowTouch.MouseControllerAxis;

        float x = MousePos.x ;
        float y = MousePos.y;

        //Debug.Log("X: " + x + " Y: " + y + "");

        float motor = maxMotorTorque * x;
        float steering = maxSteeringAngle * y;

        //Debug.Log("(Mouse)Motor: " + motor + "");
        //Debug.Log("(Mouse)Steering: " + steering + "");

        for (int i = 0; i < axleInfos.Count; i++)
        {
            if (x == 0 && y == 0) // if the mouse is not held down activate the brakes of the car //
            {
                axleInfos[i].leftWheel.brakeTorque = maxBrakeTorque;
                axleInfos[i].rightWheel.brakeTorque = maxBrakeTorque;
                continue;
            }
            else
            {
                axleInfos[i].leftWheel.brakeTorque = 0;
                axleInfos[i].rightWheel.brakeTorque = 0;
            }
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