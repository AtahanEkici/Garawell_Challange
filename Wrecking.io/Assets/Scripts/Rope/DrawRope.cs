using UnityEngine;

public class DrawRope : MonoBehaviour
{
    [Header("Line Renderer Component")]
    [SerializeField] private LineRenderer lr;

    [Header("Rope Specification")]
    [SerializeField] private Transform StartPos;
    [SerializeField] private Transform endPos;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        StartPos = transform.GetChild(0).GetChild(0).GetComponent<Transform>();
        endPos = transform.GetChild(1).GetChild(3).GetComponent<Transform>();
    }
    private void Start()
    {
        lr.positionCount = 2;
    }
    private void Update()
    {
        CheckIfNull();
        UpdatePositions();
    }
    private void CheckIfNull()
    {
        if (StartPos == null || endPos == null)
        {
            Destroy(transform.gameObject);
        }
    }
    private void UpdatePositions()
    {
        if(StartPos == null || endPos == null) { return; }
        lr.SetPosition(0, StartPos.position);
        lr.SetPosition(1, endPos.position);
    }
}
