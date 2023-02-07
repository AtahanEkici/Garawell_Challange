using UnityEngine;

public class OuterGround : MonoBehaviour
{
    private static OuterGround _instance; // Singleton Pattern //

    public static readonly string TextureString = "_MainTex"; // String needed to access the main texture component of the default shader //

    [Header("Shrinking options")]
    [SerializeField] public static bool IsShrinking = false;
    [SerializeField] private Vector3 InitialScale = Vector3.zero;
    [SerializeField] private Vector3 WantedScale = Vector3.zero;
    [SerializeField] private float ShrinkSpeed = 10f;
    [SerializeField] private float ShrinkAmount = 10f;
    [SerializeField] private float DistanceOffset = 1f;

    [Header("Timer Setttings")]
    [SerializeField] private bool isTimerActive = true;
    [SerializeField] private float InvokeTime = 10f;
    [SerializeField] private float Timer = 10f;

    [Header("Outher Shell Components")]
    [SerializeField] private Material material;
    [SerializeField] private Renderer render;
    [SerializeField] private Texture2D texture;
    [SerializeField] private Color color = Color.white; // Default will be white //

    private void Awake()
    {
        CheckInstance();
        render = GetComponent<Renderer>();
        material = render.material;
        InitialScale = transform.localScale;
        Timer = InvokeTime;
    }
    private void Start()
    {
        material.SetTexture(TextureString, AssignNewTexture(texture,color,true));
        ShrinkGround();
    }
    private void Update()
    {
        ShrinkGround();
        TimerUpdater();
    }
    private void LateUpdate()
    {
        ScaleCheck();
    }
    private void TimerUpdater() // Updates timer //  - Could have used Invoke Repeating -
    {
        if (!isTimerActive) { return; } // if timer is not active stop executing this function/method //
        
        Timer -= Time.deltaTime;

        if(Timer <= 0)
        {
            IsShrinking = true;
            Timer = InvokeTime;
        }
    }
    private void ShrinkGround()
    {
        if (!IsShrinking) { return; } // if not set to shrink do not execute this function/method //
        Vector3 size = InitialScale;
        float x = Mathf.Abs(size.x); float y = Mathf.Abs(size.y); float z = Mathf.Abs(size.z);
        WantedScale = new(x-ShrinkAmount,y,z-ShrinkAmount);
        transform.localScale = Vector3.Lerp(transform.localScale,WantedScale,Time.deltaTime * ShrinkSpeed);
        float distance = Vector3.Distance(transform.localScale, WantedScale);

        if(distance <= DistanceOffset) // Lerp Stopper //
        {
            IsShrinking = false;
            InitialScale = WantedScale;
            InnerGround.IsShrinking = true;
        }
    }
    public static Texture AssignNewTexture(Texture2D tex, Color col, bool Randomize) // Creates a new texture and returns it //
    {
        tex = new Texture2D(1,1,TextureFormat.ARGB32,false); // Create new Texture //
        if (Randomize) { col = Random.ColorHSV(); } // Get Random Color //
        tex = ChangeColorOfTexture(tex, col, 0, 0); // Change the color of the given texture //
        return tex;
    }
    public static Texture2D ChangeColorOfTexture(Texture2D texture, Color color, int Start, int Finish) // change color of the given texture //
    {
        for (int i = Start; i < texture.width; i++) // traverse through all the pixels and set their color one by one //
        {
            for (int j = Finish; j < texture.height; j++)
            {
                texture.SetPixel(i, j, color);
            }
        }
        texture.Apply();
        return texture;
    }
    private void CheckInstance() // Check instance to see if any other than this script is running  if so destroy it for sigleton purposes ofc//
    {
        if (this != _instance && _instance != null)
        {
            Destroy(this.gameObject);
        }
        else if (_instance == null)
        {
            _instance = this;
        }
    }
    private void ScaleCheck()
    {
        if(transform.localScale.x < 0.1)
        {
            isTimerActive = false;
        }
    }
}