using System.Collections.Generic;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    [Header("Foreign References")]

    [Header("Car Objects Container")]
    [SerializeField]public static List<GameObject> Cars = new();

    [Header("Winner")]
    [SerializeField] private static GameObject Winner = null;

    private void Awake()
    {
        CheckLocalInstance();
    }
    private void Update()
    {
        CheckWinner();
    }
    public static void AddList(GameObject addition)
    {
        Cars.Add(addition);
        //Debug.Log("Added " + addition + "");
    }
    public static void RemoveFromList(int Instance_ID)
    {
        for(int i=0;i<Cars.Count;i++)
        {
            if (Cars[i].GetInstanceID().Equals(Instance_ID))
            {
                Cars.RemoveAt(i);

                CheckWinner();
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
        if(Cars.Count == 1)
        {
            Winner = Cars[0];
        }

        Debug.Log("The Winner is : " + Winner + "");
        // GameOver ?
    }
}