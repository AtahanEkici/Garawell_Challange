using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{
    [Header("Local Components")]
    [SerializeField] private NavMeshAgent agent;

    [Header("Instance ID")]
    [SerializeField] private int Instance_ID;

    [Header("Target Info")]
    [SerializeField] private Transform Target;
    [SerializeField] private float TargetDistance;
    [SerializeField] private float AttackDistance = 2f;

    private void Awake()
    {
        AI_Settings();
        Instance_ID = gameObject.GetInstanceID();
        LevelManager.AddList(gameObject);
    }
    private void Update()
    {
        FollowTarget();
        AttackTarget();
    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
    }
    private void AI_Settings()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }
    private void FollowTarget()
    {
        GetNearTarget();
    }
    private void GetNearTarget()
    {
        if(Target == null)
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

        if(TargetDistance < AttackDistance)
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
        
        for (int i=0;i< cars.Count;i++)
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
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.CompareTag("Ground"))
        {
            Destroy(collision.gameObject);
        }
    }
}
