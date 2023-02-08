using UnityEngine;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [Header("Do Not Destroy")]
    [SerializeField] private bool Dont_Destroy_On_Load = true;

    [Header("Outside References")]
    [SerializeField] private LevelManager levelman;
    [SerializeField] private GameObject GameOverCanvas;
    private void Awake()
    {
        CheckInstance();
        CheckDestruction(); 
    }
    private void Start()
    {
        
    }
    private void CheckDestruction()
    {
        if (Dont_Destroy_On_Load)
        {
            DontDestroyOnLoad(this);
        }
    }
    private void CheckInstance()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }
    public void Start_Game()
    {

    }
    public static void Quit_Game()
    {
        Application.Quit();
    }
    public void GameOver()
    {

    }
    public static void Restart()
    {

    }
    public static void V_Sync()
    {
        if (QualitySettings.vSyncCount == 1)
        {
            QualitySettings.vSyncCount = 0;
        }
        else
        {
            QualitySettings.vSyncCount = 1;
        }
    }
    public void Force_Frame_Rate(int given_frame_rate)
    {
        Application.targetFrameRate = given_frame_rate;
    }

}