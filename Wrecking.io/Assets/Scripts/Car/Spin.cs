using UnityEngine;
using UnityEngine.UI;
public class Spin : MonoBehaviour
{
    private static readonly string SpinButtonTag = "SpinButton";
    private static readonly string PlayerTag = "Player";

    [Header("Foreign Script References")]
    [SerializeField] private CarRider car_rider;

    [Header("Parent object")]
    [SerializeField] private Transform BallTransform;
    [SerializeField] private Rigidbody ballRigidbody;

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
    [SerializeField] private UnityEngine.UI.Button Spin_Button;

    [Header("Wheel Hit")]
    [SerializeField] private WheelHit hit;
    private void Awake()
    {
        car_rider = GetComponent<CarRider>();
        initialRotation_Flip = transform.rotation;
        counter = FlippedTimer;
        BallTransform = transform.parent.GetChild(0);
        ballRigidbody = BallTransform.gameObject.GetComponent<Rigidbody>();
        Spin_Button = GameObject.FindGameObjectWithTag(SpinButtonTag).GetComponent<UnityEngine.UI.Button> (); // if appears to be red text underlay uninstall and reinstall the Unity UI package from Package Manager //
    }
    private void Start()
    {
        CheckIfIsPlayer();
        Spin_Button.onClick.AddListener(delegate { Debug.Log("Button Pressed"); });
        Debug.Log("Geçti");
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
    }
    private bool CheckIfIsPlayer()
    {
        if(gameObject.CompareTag(PlayerTag))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SpinIfPlayer()
    {
        Debug.Log("Geçti spin");
        SpinRequest();
    }
    public void SpinAI()
    {
        SpinRequest();
    }
    private void SpinRequest()
    {
        Debug.Log("Spin requested");
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
            }

            return;
        }   
    }
    private void SpinPlatform()
    {
        if (!SpinRequested) { return; }

        float katsayi = rotationSpeed * Time.fixedDeltaTime;
        Quaternion NewRotation = Quaternion.Euler(0f, katsayi, 0f);

        targetRotation = transform.rotation * NewRotation;
        Quaternion BallTargetRotation = BallTransform.rotation * NewRotation;

        Quaternion LocalRotation = Quaternion.Lerp(transform.rotation, targetRotation, katsayi);
        Quaternion BallRotation = Quaternion.Lerp(BallTransform.rotation, BallTargetRotation, katsayi);

        car_rider.rb.MoveRotation(LocalRotation);
        ballRigidbody.MoveRotation(BallRotation);
    }
    private void FlipCar()
    {
        if (!isFlipped) { return; }
        if (SpinRequested) { return; }

        Quaternion rotation = Quaternion.Lerp(transform.rotation, initialRotation_Flip, Time.fixedDeltaTime * rotationSpeed);
        car_rider.rb.MoveRotation(rotation);
    }
    private void DetectFlipped()
    {
        if (isFlipped) { counter = FlippedTimer; return; }

        int total_hits = 0;

        for (int i = 0; i < car_rider.RightWheels.Length; i++)
        {
            if (!car_rider.LeftWheels[i].GetGroundHit(out hit)) // Check if the wheelcollider is not colliding with something. If half of  them is not hitting anything will flip the car //
            {
                total_hits++;
            }
            if (!car_rider.LeftWheels[i].GetGroundHit(out hit))
            {
                total_hits++;
            }
        }

        if (total_hits >= car_rider.RightWheels.Length && Mathf.Abs(transform.rotation.eulerAngles.x) < StartingAngle)
        {
            counter -= Time.deltaTime;

            if (counter <= 0)
            {
                car_rider.rb.AddForce(Vector3.up * FlipForceMultiplier, FlipForceMode); // Levitate the car a bit //
                isFlipped = true;
                counter = FlippedTimer;
            }
        }

    }
}
