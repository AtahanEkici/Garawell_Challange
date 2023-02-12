using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private static readonly string GameOverTag = "GameOverCanvas";

    [Header("Outside References")]
    [SerializeField] private LevelManager levelman;
    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private GameObject TouchCanvas;
    [SerializeField] private Button RestartButton;
    private void Awake()
    {
        CheckInstance();
        GameOverCanvas = GameObject.FindGameObjectWithTag(GameOverTag);
        GameOverCanvas.SetActive(false);
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
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        //Application.LoadLevel(Application.loadedLevel);
    }
    public static void Quit_Game()
    {
        Application.Quit();
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        TouchCanvas.SetActive(false);
        GameOverCanvas.SetActive(true);
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
    public static void Force_Frame_Rate(int given_frame_rate)
    {
        Application.targetFrameRate = given_frame_rate;
    }

}