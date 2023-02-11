using UnityEngine;
public class CameraController : MonoBehaviour
{
    private static CameraController _instance;

    private static readonly string PlayerTag = "Player";

    [Header("Camera Settings")]
    [SerializeField] private Transform camTransform;
    [SerializeField] private float SmoothTime = 0.150f;
    [SerializeField] private Vector3 LocationOffset = Vector3.zero;
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
        //Offset = transform.position;
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
        targetPosition = Target.position + LocationOffset;
        //camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        //camTransform.rotation = Quaternion.Lerp(camTransform.rotation, InitialRotation, SmoothTime);
        camTransform.SetPositionAndRotation(Vector3.SmoothDamp(transform.position,targetPosition,ref velocity,SmoothTime), Quaternion.Lerp(camTransform.rotation,InitialRotation,Time.smoothDeltaTime));
        transform.LookAt(camTransform);
    }
    public void RotateDeathCam()
    {
        camTransform.rotation = Quaternion.RotateTowards(camTransform.rotation, Quaternion.Euler(camTransform.up),SmoothTime);
    }
}