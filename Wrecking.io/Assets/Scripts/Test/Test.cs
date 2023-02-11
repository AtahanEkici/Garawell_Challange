using UnityEngine;
public class Test : MonoBehaviour
{
    [Header("Toggle")]
    [SerializeField] bool drawRays;

    [Header("Vectors")]
    [SerializeField] public static Vector3 forward;
    [SerializeField] public static Vector3 downward;
    [SerializeField] public static Vector3 left;
    [SerializeField] public static Vector3 right;

    [Header("Object Position")]
    [SerializeField] Vector3 pos;

    [Header("Vector Lenght")]
    [SerializeField] float length = 20f;

    [Header("Move Controlls")]
    [SerializeField] private float MoveSpeed = 5f;

    void Update()
    {
        //DrawRays();
        //Move();
    }

    private void DrawRays()
    {
        forward = transform.forward + transform.right;
        downward = forward * -1f;

        left = (transform.forward + (transform.forward - forward));
        right = left * -1f;

        pos = transform.position;

        Debug.DrawRay(pos, forward * length, Color.red);
        Debug.DrawRay(pos, downward * length, Color.green);

        Debug.DrawRay(pos, right * length, Color.magenta);
        Debug.DrawRay(pos, left * length, Color.cyan);
    }
    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 horizontal = y * forward;
        Vector3 vertical = x * right;

        Vector3 MoveVector = horizontal + vertical;

        transform.position = Vector3.Lerp(transform.position, MoveVector,Time.smoothDeltaTime * MoveSpeed);
    }
}
