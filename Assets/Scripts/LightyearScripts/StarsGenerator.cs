using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StarsGenerator : MonoBehaviour
{
    public static StarsGenerator Instance;

    [SerializeField] private Transform camera;
    [SerializeField] private GameObject starPrefab;
    
    [Header("Generation parameters (cube dimensions)")]
    [Tooltip("Amount of stars to generate in each star cube")]
    public int qstarAmount;
    [Tooltip("Maximal length of a generated star cube")]
    public float generationLength;
    [Tooltip("Minimal spawnable position of stars, to avoid colliding with the camera")]
    public float minGenerationWidth;
    [Tooltip("Maximal width of a generated star cube")]
    public float maxGenerationWidth;

    private List<StarCube> _starCubes = new List<StarCube>();

    private int _generationStep = -1;
    private int _cameraStep = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Invoke(nameof(SubStart), 1f);
    }

    private void SubStart()
    {
        for (int i = 0; i < 3; i++)
        {
            GenerateStarCube();
        }
    }

    private void Update()
    {
        if (IsBeyondCurrentStarCubeLimit())
        {
            _cameraStep++;
            GenerateStarCube();
            DestroyLastStarCube();
        }
    }

    private bool IsBeyondCurrentStarCubeLimit()
    {
        return camera.position.z > _cameraStep * generationLength;
    }

    private void GenerateStarCube()
    {
        StarCube starCube = new StarCube(_generationStep * generationLength, starAmount);
        _starCubes.Add(starCube);
        _generationStep++;
    }

    private void DestroyLastStarCube()
    {
        _starCubes[0].DestroyStars();
        _starCubes.RemoveAt(0);
    }
    
    private class StarCube
    {
        GameObject starContainer;
        GameObject[] stars;

        public StarCube(float zPos, int starAmount)
        {
            stars = new GameObject[starAmount];
            
            starContainer = new GameObject("StarCube");
            starContainer.transform.position = new Vector3(0, 0, zPos);
            
            for (int i = 0; i < starAmount; i++)
            {
                GenerateStar(i);
            }
        }

        private void GenerateStar(int index)
        {
            GameObject star = Instantiate(StarsGenerator.Instance.starPrefab, starContainer.transform, false);
            
            star.transform.localPosition = GenerateStarPosition(
                StarsGenerator.Instance.minGenerationWidth,
                StarsGenerator.Instance.maxGenerationWidth,
                StarsGenerator.Instance.generationLength);
            
            stars[index] = star;
        }

        private Vector3 GenerateStarPosition(float minWidth, float maxWidth, float maxRange)
        {
            float xRandomPos = Random.Range(minWidth, maxWidth) * RandomSign();
            float yRandomPos = Random.Range(minWidth, maxWidth) * RandomSign();
            float zRandomPos = Random.Range(0, maxRange);

            return new Vector3(xRandomPos, yRandomPos, zRandomPos);
        }
        
        private static int RandomSign()
        {
            int[] signs = { -1, 1 }; return signs[Random.Range(0, signs.Length)];
        }

        public void DestroyStars()
        {
            foreach (var t in stars)
            {
                Destroy(t);
            }
            
            Destroy(starContainer);
        }
    }
}
