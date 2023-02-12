using UnityEngine;
public class BallBehaviour : MonoBehaviour // A simple script to ignore sibling gameobject of their collisions //
{
    [Header("All colliders inside the car object")]
    public Collider[] colliders;

    [Header("Local Collider")]
    [SerializeField] private Collider LocalCollider;

    [Header("Ignore")]
    [SerializeField] private bool IgnoreColliders = true;

    private void Awake()
    {
        colliders = transform.parent.GetChild(1).GetComponentsInChildren<Collider>();
        LocalCollider = gameObject.GetComponent<Collider>();
    }
    private void Start()
    {
        DisableCollisions();
    }

    private void DisableCollisions()
    {
        for(int i=0;i<colliders.Length;i++)
        {
            Physics.IgnoreCollision(colliders[i], LocalCollider, IgnoreColliders);
        }
    }
}
