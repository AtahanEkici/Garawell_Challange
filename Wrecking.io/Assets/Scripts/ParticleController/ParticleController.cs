using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public class ParticleController : MonoBehaviour
{
    [Header("Static variables")]
    private static ParticleController _instance;
    private static readonly string ParticlesPath = "Assets/Prefabs/Particles";
    private static readonly string ParticleFileExtension = "*.prefab";

    [Header("Particles' List")]
    [SerializeField] private static List<ParticleSystem> Particles = new();

    [Header("Particles' Names")]
    [SerializeField] public static string Flame = "Flame";
    [SerializeField] public static string Block = "Block";
    private void Awake()
    {
        CheckInstance();
    }
    private void Start()
    {
        GetParticleAssets();
    }
    private void CheckInstance()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public static ParticleSystem GetParticleFromName(string given_name)
    {
        for (int i = 0; i < Particles.Count; i++)
        {
            if (Particles[i].name == given_name)
            {
                return Particles[i];
            }
        }
        return Particles[0]; // Defaultly return the first element of the list //
    }
    public static ParticleSystem InstantiateOnLocation(string ParticleName, Vector3 Position, Quaternion Rotation)
    {
        ParticleSystem temp = Instantiate(GetParticleFromName(ParticleName), Position, Rotation);

        if(ParticleName == Flame)
        {
            Destroy(temp, 0.5f);
        }
        
        return temp;
    }
    public static void InstantiateOnLocation(string ParticleName, Vector3 Position, Quaternion Rotation, Color GivenColor)
    {
        ParticleSystem temp = GetParticleFromName(ParticleName);
        temp.GetComponent<Renderer>().material.color = GivenColor;

        if (ParticleName == Flame)
        {
            Destroy(temp, 0.5f);
        }

        Instantiate(temp, Position, Rotation);
    }
    private static void GetParticleAssets()
    {
        if (Particles.Count > 0) { return; }

        string[] Files = Directory.GetFiles(ParticlesPath, ParticleFileExtension);

        for (int i = 0; i < Files.Length; i++)
        {
            ParticleSystem temp = (ParticleSystem)AssetDatabase.LoadAssetAtPath(Files[i], typeof(ParticleSystem));
            Particles.Add(temp);
        }
    }
}