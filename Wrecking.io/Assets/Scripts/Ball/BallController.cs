using UnityEngine;
public class BallController : MonoBehaviour
{
    [Header("Local Rigidbody")]
    [SerializeField] private Rigidbody rb;

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
        rb = GetComponent<Rigidbody>();
        rb.detectCollisions = false;
    }
    private void FixedUpdate()
    {
        Vector3 move = Vector3.Lerp(Ball.position, DesiredLocation.position, Time.fixedDeltaTime * speed);
        BallBody.MovePosition(move);
    }
}
