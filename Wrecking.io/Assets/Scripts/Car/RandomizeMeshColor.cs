using UnityEngine;

public class RandomizeMeshColor : MonoBehaviour
{
    [SerializeField] private Texture2D texture;
    [SerializeField] private Color color;
    private void Awake()
    {
        GetComponent<Renderer>().material.SetTexture(OuterGround.TextureString, OuterGround.AssignNewTexture(texture, color, true));
    }
    void Start()
    {
        Debug.Log("RAndom Colorizer Destroyed on object: "+gameObject.name+"");
        Destroy(this);
    }
}
