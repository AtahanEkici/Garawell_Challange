using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public class ParticleController : MonoBehaviour
{
    [Header("Static variables")]
    private static ParticleController _instance;

    [Header("Particles' List")]
    [SerializeField] private List<ParticleSystem> Particles = new();
    [SerializeField] private static List<ParticleSystem> ParticlesStatic = new();

    [Header("Particles' Names")]
    [SerializeField] public static string Flame = "Flame";
    [SerializeField] public static string Block = "Block";
    private void Awake()
    {
        CheckInstance();
    }
    private void Start()
    {
        Particles = ParticlesStatic;
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
        for (int i = 0; i < ParticlesStatic.Count; i++)
        {
            if (ParticlesStatic[i].name == given_name)
            {
                return ParticlesStatic[i];
            }
        }
        return ParticlesStatic[0]; // Defaultly return the first element of the list //
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
}