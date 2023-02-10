using UnityEngine;
public class BallController : MonoBehaviour
{
    [Header("Ball Info")]
    [SerializeField] private Transform Ball;
    [SerializeField] private Rigidbody BallBody;

    [Header("Follow Speed")]
    [SerializeField] private float speed = 10f;

    [Header("Car Follow Location")]
    [SerializeField] private Transform DesiredLocation;
    private void Awake()
    {
        Ball = transform.GetChild(0).GetComponent<Transform>();
        BallBody = Ball.gameObject.GetComponent<Rigidbody>();
        DesiredLocation = transform.GetChild(1).GetChild(2).GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        Vector3 move = Vector3.Lerp(Ball.position, DesiredLocation.position, Time.smoothDeltaTime * speed);
        BallBody.MovePosition(move);
    }

    /*
    public Transform Ball;
    public Transform objectToFollow;
    public Vector3 offset;
    public Quaternion rotationoffset;
    public float desiredDistance = 4.5f;
    public float speed = 5f;

    [Header("Ball Rigidbody")]
    [SerializeField] private Transform BallTransform;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float DesiredDistance = 5f;
    
    private void FixedUpdate()
    {
        Vector3 lerpMovement = Vector3.Lerp(Ball.position, (objectToFollow.position + offset), Time.fixedDeltaTime * speed);
        Quaternion lerpRotation = Quaternion.Lerp(Ball.rotation, objectToFollow.rotation, Time.fixedDeltaTime * speed);

        rb.MovePosition(lerpMovement);
        rb.MoveRotation(lerpRotation);

        rb.velocity = Vector3.zero; 
        rb.angularVelocity = Vector3.zero;   
    }
    
    [Header("Target Specifications")]
    [SerializeField] private Transform Target;
    [SerializeField] private float TargetDistance;

    [Header("Ball Rigidbody")]
    [SerializeField] private Transform BallTransform;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float DesiredDistance = 5f;

    [Header("Speed Constraints")]
    [SerializeField] private float speed;

    private void Awake()
    {
        rb = transform.GetChild(0).GetComponent<Rigidbody>();
        Target = transform.GetChild(1).GetComponent<Transform>();
        BallTransform = rb.gameObject.GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        MoveToDesiredCoordinates();
    }
    private void Update()
    {
        TargetDistance = Vector3.Distance(BallTransform.position, Target.position);
    }
    private void MoveToDesiredCoordinates()
    {
        Vector3 combine = Target.position;
        //Debug.DrawRay(BallTransform.position, combine * 20f, Color.red);
        combine = speed * Time.fixedDeltaTime * combine;
        Debug.DrawRay(BallTransform.position, combine, Color.blue);

        if (TargetDistance >= DesiredDistance)
        {
            Debug.Log("Moving");
            rb.MovePosition(combine);
        }  
    }
    */
}
