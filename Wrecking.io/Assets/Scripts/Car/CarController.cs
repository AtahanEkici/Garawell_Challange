using UnityEngine;

public class CarController : MonoBehaviour
{
    private static readonly string GroundTag = "Ground";

    [Header("Instance ID Given by LevelManager")]
    [SerializeField] private int Instance_ID;

    [Header("Camera Reference")]
    [SerializeField] Camera main_camera;

    [Header("Fall Settings")]
    [SerializeField] private float FallThreshold = -10f;

    [Header("Local Componenet References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float mass;

    [Header("Movement Settings")]
    [SerializeField] private float MoveSpeed = 10f;
    [SerializeField] private float MaxSpeed = 20f;
    [SerializeField] private float MinSpeed = 0f;

    //[Header("ForceMode")]
    //[SerializeField]private ForceMode forcemode = ForceMode.Force;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;
        Instance_ID = gameObject.GetInstanceID();
        LevelManager.AddList(gameObject);
    }
    private void Start()
    {
        main_camera = Camera.main;
    }
    private void Update()
    {
        CheckFall();
        Movement();
    }
    private void LateUpdate()
    {
        SpeedCheck();
    }
    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (y == 0) { return; }

        Vector3 forward =-x * transform.forward;
        Vector3 side = y * transform.right;
        Vector3 moveBy = side + forward;
        transform.Translate(MoveSpeed * Time.deltaTime * moveBy.normalized);
    }
    private void CheckFall()
    {
        if(transform.position.y <= FallThreshold)
        {
            Debug.Log(gameObject.name + " named vehicle fell");
            Destroy(gameObject);
        }
    }
    private void SpeedCheck()
    {
        rb.velocity = ClampMagnitude(rb.velocity,MaxSpeed, MinSpeed);
    }
    public static Vector3 ClampMagnitude(Vector3 v, float max, float min)
    {
        double sm = v.sqrMagnitude;
        if (sm > (double)max * (double)max) return v.normalized * max;
        else if (sm < (double)min * (double)min) return v.normalized * min;
        return v;
    }
    private void OnDestroy() // OnDestroy Remove Object From List //
    {
        LevelManager.RemoveFromList(Instance_ID);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(GroundTag))
        {
            Debug.Log("Touched Ground");
        }
    }
}
