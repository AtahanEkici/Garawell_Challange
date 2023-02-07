using UnityEngine;
public class CameraController : MonoBehaviour
{
    private static CameraController _instance;

    private static readonly string PlayerTag = "Player";

    [Header("Camera Settings")]
    [SerializeField] private Transform camTransform;
    [SerializeField] private float SmoothTime = 0.150f;
    [SerializeField] private Vector3 Offset = new(0,0,0);

    [Header("Camera Target")]
    [SerializeField] private Transform Target;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    private void Awake()
    {
        CheckInstance();
        Target = GameObject.FindGameObjectWithTag(PlayerTag).transform;
        camTransform = transform;
    }
    private void LateUpdate()
    {
        targetPosition = Target.position + Offset;
        camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        transform.LookAt(camTransform);
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
}