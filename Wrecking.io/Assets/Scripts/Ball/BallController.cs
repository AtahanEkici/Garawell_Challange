using UnityEngine;

public class BallController : MonoBehaviour
{
    public Transform Space;
     public Rigidbody TowedObject;

    public ForceMode ForceMode = ForceMode.Force;

    public float MaxForce = 15;
    public float MinForce = -5;
    public float MaxRange = 10;
    public float MinRange = 5;

    private void FixedUpdate()
    {
        var direction = Space.transform.position - TowedObject.position;
        var distance = direction.magnitude;
        var range = Mathf.Lerp(MinRange, MaxRange, distance) / MaxRange;
        var force = Mathf.Lerp(MinForce, MaxForce, range);

        TowedObject.AddForce(direction.normalized * force, ForceMode);
    }
}
