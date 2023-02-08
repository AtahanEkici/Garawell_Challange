using UnityEngine;

public class CarController : MonoBehaviour
{
    private static readonly string GroundTag = "Ground";

    [Header("Camera Reference")]
    [SerializeField] Camera main_camera;

    [Header("Mouse/Touch Position")]
    [SerializeField] private Vector3 InitialMousePosition = Vector3.zero;

    [Header("Fall Settings")]
    [SerializeField] private float FallThreshold = -10f;

    [Header("Local Componenet References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float mass;

    [Header("Movement Settings")]
    [SerializeField] private float MoveSpeed = 10f;
    [SerializeField] private float MaxSpeed = 20f;
    [SerializeField] private float MinSpeed = 0f;

    [Header("ForceMode")]
    [SerializeField]private ForceMode forcemode = ForceMode.Force;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;
    }
    private void Start()
    {
        main_camera = Camera.main;
    }
    private void FixedUpdate()
    {
        //AlternateMovement();
    }
    private void Update()
    {
        CheckFall();
        TestMovement();
    }
    private void LateUpdate()
    {
        GetInitialPosition();
        SpeedCheck();
    }
    private void Movement()
    {
        if (InitialMousePosition == Vector3.zero) { return; }

        Vector2 CurrentMousePos = Input.mousePosition;
        Vector2 InitialMousePos = InitialMousePosition;

        Vector2 WorldCurrent = main_camera.WorldToScreenPoint(CurrentMousePos);
        Vector2 WorldInitial = main_camera.WorldToScreenPoint(InitialMousePos);

        //Debug.DrawRay(transform.position,WorldCurrent * 200f, Color.green,1f);
        //Debug.DrawRay(transform.position, WorldInitial * 200f, Color.magenta,1f);
        //Debug.DrawLine(WorldInitial, WorldCurrent * Mathf.Infinity, Color.red,1f);

        float distance = Vector2.Distance(WorldInitial, WorldCurrent);

        //rb.AddForce();

        Debug.Log("Distance: "+distance+"");
    }
    private void AlternateMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        // if (x == 0 && y == 0) { return; }

        Vector3 forwardMove = Vector3.right * x;

        Vector3 moveBy = forwardMove;
        rb.AddForce(mass * MoveSpeed * Time.fixedDeltaTime * moveBy, forcemode);
    }
    private void TestMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (y == 0) { return; }

        Vector3 forward =-x * transform.forward;
        Vector3 side = y * transform.right;
        Vector3 moveBy = side + forward;
        transform.Translate(MoveSpeed * Time.deltaTime * moveBy.normalized);
    }
    private void GetInitialPosition()
    {
        if(Input.GetMouseButtonDown(0))
        {
            InitialMousePosition = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            InitialMousePosition = Vector3.zero;
        }
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
    private void OnDestroy()
    {
        Debug.Log(gameObject.name+" destroyed");
        main_camera.transform.parent = null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(GroundTag))
        {
            Debug.Log("Touched Ground");
        }
    }
}
