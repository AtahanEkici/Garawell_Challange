using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.ParticleSystem;

public class ShowTouch : MonoBehaviour
{
    private static readonly string AssetLocation = "Assets/Prefabs/UI";
    private static readonly string AssetFileExtension = "*.prefab".Trim();

    [Header("Components for Touch Virtualization")]
    [SerializeField] private GameObject[] GameObjects;

    [Header("Mouse Position")]
    [SerializeField] private Vector2 MousePosition; 

    [Header("Circle")]
    [SerializeField] private GameObject Circle;
    [SerializeField] private static readonly string CircleName = "Circle";
    [SerializeField] private Vector2 InitialMousePosition;

    [Header("Touch pointer")]
    [SerializeField] private GameObject Touch;
    [SerializeField] private static readonly string TouchName = "Touch";
    [SerializeField] private float MaxDistance;
    [SerializeField] Vector2 allowedPos;




    private void Awake()
    {
        GetTouchVirtualizationAssets();
        InstantiateTouchObjects();
    }
    private void Update()
    {
        TrackMouse();
        GetInitialPosition(); 
    }
    private void LateUpdate()
    {
        MoveTouchObjects();
    }
    private void GetTouchVirtualizationAssets()
    {
        string[] Files = Directory.GetFiles(AssetLocation,AssetFileExtension);
        GameObjects = new GameObject[Files.Length];

        for (int i = 0;i<Files.Length;i++)
        {
            GameObject temp = (GameObject)AssetDatabase.LoadAssetAtPath(Files[i],typeof(GameObject));
            GameObjects[i] = temp;

            if (GameObjects[i].name == CircleName)
            {
                Circle = GameObjects[i];
            }
            else if (GameObjects[i].name == TouchName)
            {
                Touch = GameObjects[i];
            }
        }
    }
    private void InstantiateTouchObjects()
    {
        Circle = Instantiate(Circle, Vector3.zero,Quaternion.identity);
        Touch = Instantiate(Touch, Vector3.zero, Quaternion.identity);

        Circle.transform.SetParent(gameObject.transform);
        Touch.transform.SetParent(gameObject.transform);

        Circle.SetActive(false);
        Touch.SetActive(false);
    }
    private void GetInitialPosition()
    {
        if(Input.GetMouseButtonDown(0))
        {
            InitialMousePosition = MousePosition;

            Circle.transform.position = InitialMousePosition;
            Touch.transform.position = InitialMousePosition;

            Circle.SetActive(true);
            Touch.SetActive(true);
        }
    }
    private void MoveTouchObjects()
    {
        if (Input.GetMouseButton(0)) // Mouse Input Visualizer's Joystick Emulator //
        {
            allowedPos = MousePosition - InitialMousePosition;
            allowedPos = Vector2.ClampMagnitude(allowedPos, MaxDistance);
            Touch.transform.position = InitialMousePosition + allowedPos;
        }
    
        else 
        {
            Circle.SetActive(false);
            Touch.SetActive(false);
        }
    }
    private void TrackMouse()
    {
        MousePosition = Input.mousePosition;
    }
}
