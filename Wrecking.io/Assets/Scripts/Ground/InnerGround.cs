using UnityEngine;

public class InnerGround : MonoBehaviour
{
    private static InnerGround _instance; // Singleton Pattern //

    [Header("Shrinking options")]
    [SerializeField] public static bool IsShrinking = false;
    [SerializeField] private Vector3 InitialScale = Vector3.zero;
    [SerializeField] private Vector3 WantedScale = Vector3.zero;
    [SerializeField] private float ShrinkSpeed = 10f;
    [SerializeField] private float ShrinkAmount = 10f;
    [SerializeField] private float DistanceOffset = 1f;

    [Header("Inner Shell Components")]
    [SerializeField] private Material material;
    [SerializeField] private Renderer render;
    [SerializeField] private Texture2D texture;
    [SerializeField] private Color color = Color.cyan;
    private void Awake()
    {
        CheckInstance();
        render = GetComponent<Renderer>();
        material = render.material;
        InitialScale = transform.localScale;
    }
    private void Start()
    {
        material.SetTexture(OuterGround.TextureString,OuterGround.AssignNewTexture(texture,color,false));
    }
    private void Update()
    {
        ShrinkGround();
    }
    private void ShrinkGround()
    {
        if (!IsShrinking) { return; } // if not set to shrink do not execute this function/method //
        Vector3 size = InitialScale;
        float x = Mathf.Abs(size.x); float y = Mathf.Abs(size.y); float z = Mathf.Abs(size.z); // Math Absolute Value Function is required for non negative scale value which unity 
        WantedScale = new(x - ShrinkAmount, y, z - ShrinkAmount);
        transform.localScale = Vector3.Lerp(transform.localScale, WantedScale, Time.deltaTime * ShrinkSpeed);
        float distance = Vector3.Distance(transform.localScale, WantedScale);
        //Debug.Log(distance);

        if (distance <= DistanceOffset) // Lerp Stopper //
        {
            IsShrinking = false;
            InitialScale = WantedScale;
        }
    }
    private void CheckInstance()
    {
        if(this != _instance && _instance != null)
        {
            Destroy(this);
        }
        else if(_instance == null)
        {
            _instance = this;
        }
    }
}
