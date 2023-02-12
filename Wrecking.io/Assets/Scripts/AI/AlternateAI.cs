using UnityEngine;
using UnityEngine.AI;

public class AlternateAI : MonoBehaviour
{
    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public Transform[] wheelMeshes = new Transform[4];
    public float maxSteerAngle = 30f;
    public float motorForce = 50f;
    public NavMeshAgent agent;
    public Quaternion wheelMeshRotation;
    public Vector3 wheelMeshPosition;

    private void Awake()
    {
        agent.updateRotation = false;
        agent = GetComponent<NavMeshAgent>();
#pragma warning disable UNT0014 // Invalid type for call to GetComponent
        wheelColliders = GetComponentInChildren<WheelCollider[]>();
#pragma warning restore UNT0014 // Invalid type for call to GetComponent
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
           
            wheelColliders[i].GetWorldPose(out wheelMeshPosition, out wheelMeshRotation);
            wheelMeshes[i].transform.position = wheelMeshPosition;
            wheelMeshes[i].transform.rotation = wheelMeshRotation;
        }

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            float speedFactor = agent.desiredVelocity.magnitude / agent.speed;
            float steerAngle = maxSteerAngle * agent.steeringTarget.x;
            float motorInput = agent.desiredVelocity.magnitude;

            wheelColliders[0].steerAngle = steerAngle;
            wheelColliders[1].steerAngle = steerAngle;

            wheelColliders[2].motorTorque = motorInput * motorForce * speedFactor;
            wheelColliders[3].motorTorque = motorInput * motorForce * speedFactor;
        }
        else
        {
            wheelColliders[2].motorTorque = 0f;
            wheelColliders[3].motorTorque = 0f;
        }
    }
}
