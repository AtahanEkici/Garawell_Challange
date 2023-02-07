using UnityEngine;
public class CarController : MonoBehaviour
{
    private static readonly string GroundTag = "Ground";

    [Header("Fall Settings")]
    [SerializeField] private float FallThreshold = -10f;

    [Header("Local Componenet References")]
    [SerializeField] private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        
    }
    private void FixedUpdate()
    {
        
    }
    private void Update()
    {
        CheckFall();
    }
    private void LateUpdate()
    {
        
    }
    private void CheckFall()
    {
        if(transform.position.y <= FallThreshold)
        {
            Debug.Log(gameObject.name + " named vehicle fell");
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        Debug.Log(gameObject.name+" destroyed");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(GroundTag))
        {
            Debug.Log("Touched Ground");
        }
    }
}
