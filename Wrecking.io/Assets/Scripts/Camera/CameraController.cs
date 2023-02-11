using UnityEngine;
public class CameraController : MonoBehaviour
{
    private static CameraController _instance;

    private static readonly string PlayerTag = "Player";

    [Header("Camera Settings")]
    [SerializeField] private Transform camTransform;
    [SerializeField] private float SmoothTime = 0.150f;
    [SerializeField] private Vector3 Offset = new(0,0,0);
    [SerializeField] private Quaternion InitialRotation = Quaternion.identity;

    [Header("Camera Target")]
    [SerializeField] private Transform Target;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    private void Awake()
    {
        CheckInstance();
        Target = GameObject.FindGameObjectWithTag(PlayerTag).transform;
        camTransform = transform;
        InitialRotation = transform.rotation;
    }
    private void Start()
    {
        //transform.parent = Target;
    }
    private void Update()
    {
        TailTarget();
    }
    private void CheckInstance()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }
    private void TailTarget()
    {
        if (Target == null) { RotateDeathCam(); return; } // if the target is destroyed stop execution //
        targetPosition = Target.position + Offset;
        camTransform.position =  Vector3.SmoothDamp(transform.position,targetPosition,ref velocity,SmoothTime);
        transform.LookAt(Target);
    }
    public void RotateDeathCam()
    {
        camTransform.rotation = Quaternion.RotateTowards(camTransform.rotation, Quaternion.Euler(camTransform.up),Time.smoothDeltaTime);
    }
}