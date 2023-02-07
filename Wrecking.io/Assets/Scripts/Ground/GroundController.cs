using UnityEngine;
public class GroundController : MonoBehaviour
{
    private static readonly string TextureString = "_MainTex"; // String needed to access the main texture component of the default shader //

    [Header("Transform Variables")]
    [SerializeField]private GameObject trans_outer;
    [SerializeField]private GameObject trans_inner;

    [Header("Outher Shell Components")]
    [SerializeField]private Material mat_outer;
    [SerializeField]private Renderer render_outer;
    [SerializeField]private Texture2D tex_outer;
    [SerializeField]private Color color_outer = Color.white; // Default will be white //

    [Header("Inner Shell Components")]
    [SerializeField] private Material mat_inner;
    [SerializeField] private Renderer render_inner;
    [SerializeField] private Texture2D tex_inner;
    [SerializeField] private Color color_inner = Color.cyan; // Default will be white //
    private void Awake()
    {
        trans_outer = this.transform.GetChild(0).gameObject;
        trans_inner = this.transform.GetChild(1).gameObject;

        render_outer = transform.GetChild(0).GetComponent<Renderer>();
        mat_outer = render_outer.material;

        render_inner = transform.GetChild(1).GetComponent<Renderer>();
        mat_inner = render_inner.material;
    }
    private void Start()
    {
        mat_outer.SetTexture(TextureString, AssignNewOuterTexture()); 
        mat_inner.SetTexture(TextureString, AssignNewInnerTexture());
    }
    private void Update()
    {
        
    }
    private Texture AssignNewOuterTexture() // Creates a new texture and returns it //
    {
        tex_outer = new Texture2D(1,1,TextureFormat.ARGB32,false); // Create new Texture //
        color_outer = Random.ColorHSV(); // Get Random Color //
        tex_outer = ChangeColorOfTexture(tex_outer, color_outer, 0,0); // Change the color of the given texture //
        return tex_outer;
    }
    private Texture AssignNewInnerTexture() // Creates a new texture and returns it //
    {
        tex_inner = new Texture2D(1, 1, TextureFormat.ARGB32, false); // Create new Texture //
        tex_inner = ChangeColorOfTexture(tex_inner, color_inner, 0, 0); // Change the color of the given texture //
        return tex_inner;
    }
    private Texture2D ChangeColorOfTexture(Texture2D texture, Color color, int Start, int Finish) // change color of the given texture //
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
}
