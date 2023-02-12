using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    private static readonly string AssetLocation = "Assets/Prefabs/AI";

    [Header("Spawn Options")]
    [SerializeField] private GameObject AI_Platforms;
    [SerializeField] Vector3 spawnVector = Vector3.zero;
    [SerializeField] Quaternion spawnRotation = Quaternion.identity;
    [SerializeField] private int SpawnAmount = 5;
    [SerializeField] private float SpawnHeight = 1f;

    [Header("Outer Shell Scale")]
    [SerializeField] private float FieldScale = 5f;

    [Header("Car Objects Container")]
    [SerializeField] public static List<GameObject> Cars = new();

    [Header("Winner")]
    [SerializeField] private static GameObject Winner = null;

    private void Awake()
    {
        CheckLocalInstance();
        GetAsset();
        FieldScale = GameObject.FindGameObjectWithTag(OuterGround.GroundTag).transform.localScale.x;
        SpawnObjects(); 
    }
    private void SpawnObjects()
    {
        for(int i=0;i<SpawnAmount;i++)
        {
            spawnVector = (Random.insideUnitCircle * FieldScale) / 2;
            Debug.Log(spawnVector);
            spawnVector.z = spawnVector.y;
            spawnVector.y = SpawnHeight;
            Debug.Log(spawnVector);
            Instantiate(AI_Platforms, spawnVector, spawnRotation);
        }
    }
    public static void AddList(GameObject addition)
    {
        Cars.Add(addition);
    }
    public static void RemoveFromList(int Instance_ID)
    {
        for(int i=0;i<Cars.Count;i++)
        {
            if (Cars[i].GetInstanceID().Equals(Instance_ID))
            {
                CheckWinner();
                Cars.RemoveAt(i); 
            }
        }
    }
    private void CheckLocalInstance()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }
    private static void CheckWinner()
    {
        if(Cars.Count <= 1)
        {
            Winner = Cars[0];
            Debug.Log("The Winner is : " + Winner + "");
            // GameOver ?
        }
    }
    private void GetAsset()
    {
        string[] Files = Directory.GetFiles(AssetLocation,ShowTouch.AssetFileExtension);
        Debug.Log(Files[0]);
        AI_Platforms = (GameObject)AssetDatabase.LoadAssetAtPath(Files[0], typeof(GameObject));
    }
}