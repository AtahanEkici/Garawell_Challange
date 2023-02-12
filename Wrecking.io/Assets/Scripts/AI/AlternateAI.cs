using System.Collections.Generic;
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

    [Header("Instance ID")]
    [SerializeField] private int Instance_ID;

    [Header("Target Info")]
    [SerializeField] private Transform Target;
    [SerializeField] private float TargetDistance;
    [SerializeField] private float AttackDistance = 2f;

    [Header("Foreign Components")]
    [SerializeField] private Spin LocalSpin;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        wheelColliders = GetComponentsInChildren<WheelCollider>();
        Instance_ID = gameObject.GetInstanceID();
        LevelManager.AddList(gameObject);
        WheelSettings();
    }
    private void Update()
    {
        Movement();
        FollowTarget();
        AttackTarget();
    }
    private void Movement()
    {
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
    private void WheelSettings()
    {
        for(int i=0;i<wheelColliders.Length;i++)
        {
            wheelMeshes[i] = wheelColliders[i].gameObject.transform;
        }
    }
    private void FollowTarget()
    {
        GetNearTarget();
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
    }
    private void AttackTarget()
    {
        if (Target == null) { return; }

        if (TargetDistance < AttackDistance)
        {
            Debug.Log("Attack!!!");
            LocalSpin.SpinAI();
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
    private void CalculateTargetDistance()
    {
        TargetDistance = Vector3.Distance(transform.position, Target.position);
    }
    private void OnDestroy()
    {
        LevelManager.RemoveFromList(Instance_ID);
    }
}
