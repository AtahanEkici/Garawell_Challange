using System.Collections.Generic;
using UnityEngine;

public class CarAIController : MonoBehaviour
{
    [Header("Instance ID")]
    [SerializeField] private int Instance_ID;

    [Header("Controls")]
    [SerializeField] private float targetSpeed = 1000f;
    [SerializeField] private float maxSteerAngle = 45f;

    [Header("Target Info")]
    [SerializeField] private Transform Target = null;
    [SerializeField] private Vector3 direction = Vector3.zero;
    [SerializeField] private float MaxDistance = 20f;
    [SerializeField] private float TargetDistance = 20f;

    [Header("Attack Info")]
    [SerializeField] private bool CanAttack = true;
    [SerializeField] private float AttackDistance = 5f;
    [SerializeField] private float AttackCoolDown = 2f;
    [SerializeField] private float AttackTimer = 0f;

    [Header("Wheels")]
    [SerializeField] private WheelCollider frontLeftWheel;
    [SerializeField] private WheelCollider frontRightWheel;
    [SerializeField] private WheelCollider rearLeftWheel;
    [SerializeField] private WheelCollider rearRightWheel;

    [Header("Foreign Components")]
    [SerializeField] private Spin LocalSpin;

    private void Awake()
    {
        Instance_ID = gameObject.GetInstanceID();
        LevelManager.AddList(gameObject);
        LocalSpin = GetComponent<Spin>();    
    }
    private void Start()
    {
        FindNewTarget();
    }
    void FixedUpdate()
    {
        Movement();
    }
    private void Update()
    {
        FollowTarget();
        AttackTarget();
        Debug.DrawRay(transform.position, direction, Color.cyan);
    }
    private void RamTarget() // If can not attack ram the target //
    {

    }
    private void AttackTarget()
    {
        if (Target == null) { return; }
        if(CanAttack == false) { AttackTimer += Time.deltaTime;  return; }

        if(AttackTimer >= AttackCoolDown)
        {
            CanAttack = true;
            AttackTimer = 0f;
        }

        if (TargetDistance < AttackDistance)
        {
            LocalSpin.SpinAI();
            CanAttack = false;
        }
    }
    private void Movement()
    {
        direction = Target.position - transform.position;

        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        float steer = Mathf.Clamp(angle / maxSteerAngle, -1f, 1f);

        frontLeftWheel.steerAngle = steer * maxSteerAngle;
        frontRightWheel.steerAngle = steer * maxSteerAngle;
        rearLeftWheel.motorTorque = targetSpeed - steer;
        rearRightWheel.motorTorque = targetSpeed + steer;
    }
    private void CalculateDistance()
    {
        TargetDistance = Vector3.Distance(transform.position, Target.position);

        if(TargetDistance > MaxDistance)
        {
            FindNewTarget();
        }
    }
    private void GetNearTarget()
    {
        if (Target == null)
        {
            FindNewTarget();
        }
        else
        {
            CalculateDistance();
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
    }
    private void FollowTarget()
    {
        GetNearTarget();
    }
    private void OnDestroy()
    {
        LevelManager.RemoveFromList(Instance_ID);
    }
}
