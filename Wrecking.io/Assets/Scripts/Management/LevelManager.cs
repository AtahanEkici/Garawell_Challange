using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    [Header("Spawn Positions")]
    [SerializeField] private Vector3[] SpawnLocations;

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
        FieldScale = GameObject.FindGameObjectWithTag(OuterGround.GroundTag).transform.localScale.x;
        SpawnObjects(); 
    }
    private void SpawnObjects()
    {
        SpawnLocations = new Vector3[SpawnAmount];

        for (int i=0;i<SpawnAmount;i++)
        {
            spawnVector = (Random.insideUnitCircle * FieldScale) / 3;
            spawnVector.z = spawnVector.y;
            spawnVector.y = SpawnHeight;
            SpawnLocations[i] = spawnVector;
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

            if(Winner.CompareTag("Player"))
            {
                GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
                gm.GameOver();
            }
        }
    }
}