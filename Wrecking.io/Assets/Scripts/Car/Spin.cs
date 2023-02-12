using UnityEngine;
using UnityEngine.UI;
public class Spin : MonoBehaviour
{
    private static readonly string SpinButtonTag = "SpinButton";
    private static readonly string PlayerTag = "Player";

    [SerializeField] private WheelCollider[] wheels;

    [Header("Foreign Script References")]
    [SerializeField] private BallController ballControl;

    [Header("Local References")]
    [SerializeField] private Rigidbody rb;

    [Header("Flip Controlls")]
    [SerializeField] public bool isFlipped = false;
    [SerializeField] private float FlipForceMultiplier = 1f;
    [SerializeField] private ForceMode FlipForceMode = ForceMode.Impulse;
    [SerializeField] private float StartingAngle = 45;
    [SerializeField] private Quaternion initialRotation_Flip = Quaternion.identity;
    [SerializeField] private float FlippedTimer = 1f;
    [SerializeField] private float counter = 1f;

    [Header("Spin Controlls")]
    [SerializeField] private bool SpinRequested = false;
    [SerializeField] private Quaternion targetRotation = Quaternion.identity;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float SpinTimer = 1f;
    [SerializeField] private float counter_spin = 0;
    [SerializeField] private Button Spin_Button;
    [SerializeField] private float InitialFollowSpeed = 5f;
    [SerializeField] private float BallFollowSpeedMultiplier = 4f;

    [Header("Disqualification Parameters")]
    [SerializeField] private Vector3 ComparisonVector = Vector3.zero;
    [SerializeField] private float MaxDistance = 500f;
    [SerializeField] private float CurrentDistance;

    [Header("Wheel Hit")]
    [SerializeField] private WheelHit hit;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        wheels = transform.parent.GetComponentsInChildren<WheelCollider>();
        initialRotation_Flip = transform.rotation;
        counter = FlippedTimer;
        Spin_Button = GameObject.FindGameObjectWithTag(SpinButtonTag).GetComponent<Button> (); // if appears to be red text underlay uninstall and reinstall the Unity UI package from Package Manager //
        ballControl = transform.parent.GetComponent<BallController>();
        InitialFollowSpeed = ballControl.FollowSpeed;
    }
    private void Start()
    {
        if(CompareTag(PlayerTag)) // only player //
        {
            Spin_Button.onClick.AddListener(SpinIfPlayer);
        }  
    }
    private void FixedUpdate()
    {
        FlipCar();
        SpinPlatform();
    }
    private void Update()
    {
        DetectFlipped();
        SpinCounter(); 
        CheckDisqualification();
    }
    private void CheckDisqualification()
    {
        if(Vector3.Distance(transform.position, ComparisonVector) > MaxDistance)
        {
            GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            gm.GameOver();
        }
    }
    public void SpinIfPlayer()
    {
        SpinRequest();
    }
    public void SpinAI()
    {
        SpinRequest();
    }
    private void SpinRequest()
    {
        InitialFollowSpeed = ballControl.FollowSpeed;
        ballControl.FollowSpeed = InitialFollowSpeed * BallFollowSpeedMultiplier;
        counter_spin = 0f;
        SpinRequested = true;
    }
    private void SpinCounter()
    {
        if (isFlipped) { return; }

        if (SpinRequested) 
        { 
            counter_spin += Time.deltaTime; 

            if(counter_spin >= SpinTimer)
            {
                SpinRequested = false;
                ballControl.FollowSpeed = InitialFollowSpeed;
                counter_spin = 0f;
            }
        }   
    }
    private void SpinPlatform()
    {
        if (!SpinRequested) { return; }

        float katsayi = rotationSpeed * Time.fixedDeltaTime;
        Quaternion NewRotation = Quaternion.Euler(0f, katsayi, 0f);
        targetRotation = transform.rotation * NewRotation;
        Quaternion LocalRotation = Quaternion.Lerp(transform.rotation, targetRotation, katsayi);
        rb.MoveRotation(LocalRotation);
    }
    private void FlipCar()
    {
        if (!isFlipped) { return; }
        if (SpinRequested) { return; }

        Quaternion rotation = Quaternion.Lerp(transform.rotation, initialRotation_Flip, Time.fixedDeltaTime * rotationSpeed);
        rb.MoveRotation(rotation);
    }
    private void DetectFlipped()
    {
        if (isFlipped) { counter = FlippedTimer; return; }

        int total_hits = 0;

        for (int i = 0; i < wheels.Length; i++)
        {
            if (!wheels[i].GetGroundHit(out hit)) // Check if the wheelcollider is not colliding with something. If half of  them is not hitting anything will flip the car //
            {
                total_hits++;
            }
        }

        if (total_hits >= wheels.Length && Mathf.Abs(transform.rotation.eulerAngles.x) < StartingAngle)
        {
            counter -= Time.deltaTime;

            if (counter <= 0)
            {
                rb.AddForce(Vector3.up * FlipForceMultiplier, FlipForceMode); // Levitate the car a bit //
                isFlipped = true;
                counter = FlippedTimer;
            }
        }

    }
}
