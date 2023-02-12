using UnityEngine;
public class Water : MonoBehaviour
{
    [Header("RigidBody")]
    [SerializeField] private Rigidbody rb;

    [Header("Particle Dump")]
    [SerializeField] private ParticleSystem ps;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        Transform temp = collision.gameObject.transform;
        Vector3 globalPositionOfContact = collision.contacts[0].point;

        ps = ParticleController.InstantiateOnLocation(ParticleController.Block, globalPositionOfContact, temp.rotation);
        ps.transform.SetParent(temp);

        ps = ParticleController.InstantiateOnLocation(ParticleController.Flame, globalPositionOfContact, temp.rotation);
        ps.transform.SetParent(temp);
    }
}
