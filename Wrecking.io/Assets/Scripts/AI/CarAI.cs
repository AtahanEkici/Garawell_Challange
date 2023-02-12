using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour
{
    [Header("Instance ID")]
    [SerializeField] private int Instance_ID;

    [Header("Motor Attributes")]
    [SerializeField] private float motorForce = 5000f;
    [SerializeField] private float steerAngle = 30f;

    [Header("Target Info")]
    [SerializeField] private Transform Target;
    [SerializeField] private float TargetDistance;
    [SerializeField] private float AttackDistance = 2f;

    [Header("Wheel Components")]
    [SerializeField] private WheelCollider frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;

    [Header("NavMesh Agent")]
    [SerializeField] private NavMeshAgent agent;

    private void Awake()
    {
        Instance_ID = gameObject.GetInstanceID();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        FollowTarget();
        AttackTarget();
    }
    private void FollowTarget()
    {
        GetNearTarget();
    }
    private void AttackTarget()
    {
        if (Target == null) { return; }

        if (TargetDistance < AttackDistance)
        {
            Debug.Log("Attack!!!");
            // Attack Target //
        }
    }
    private void FindNewTarget()
    {
        List<GameObject> cars = LevelManager.Cars;

        if (cars.Count <= 0) { return; }

        float MinDistance = float.MaxValue;
        float CurrentDistance = 0f;
        int index = 0;

        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].GetInstanceID() == Instance_ID) { continue; }

            CurrentDistance = Vector3.Distance(transform.position, cars[i].transform.position);

            if (CurrentDistance < MinDistance)
            {
                MinDistance = CurrentDistance;
                index = i;
            }
        }
        Target = cars[index].transform;
        CalculateTargetDistance();
    }
    private void GetNearTarget()
    {
        if (Target == null)
        {
            FindNewTarget();
        }
        else
        {
            SetDestination(Target.position);
            CalculateTargetDistance();
        }
    }
    private void SetDestination(Vector3 destination)
    {
        if (agent == null) { return; }
        agent.destination = destination;
        MovePlatform();
    }
    private void CalculateTargetDistance()
    {
        TargetDistance = Vector3.Distance(transform.position, Target.position);
    }
    private void MovePlatform()
    {
        float horizontal = agent.desiredVelocity.x;
        float vertical = agent.desiredVelocity.z;

        frontLeftWheel.steerAngle = steerAngle * horizontal;
        frontRightWheel.steerAngle = steerAngle * horizontal;

        rearLeftWheel.motorTorque = motorForce * vertical;
        rearRightWheel.motorTorque = motorForce * vertical;
    }
}