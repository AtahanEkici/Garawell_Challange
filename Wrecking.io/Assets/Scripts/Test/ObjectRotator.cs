using UnityEngine;
public class ObjectRotator : MonoBehaviour
{
    public float rollSpeed = 360f;
    private Rigidbody rb;
    public float angle = 0.1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // FixedUpdate is called once per fixed time step
    private void FixedUpdate()
    {
        float katsayi = rollSpeed * Time.fixedDeltaTime;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0f, katsayi, 0f);
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, katsayi));
    }
}
