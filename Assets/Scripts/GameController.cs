using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public GameObject spaceshipPrefab;
    private GameObject spaceship;
    private int numAsteroids = 5;
    private float minColissionDistance = 2.0f;

    private void Awake()
    {
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        // spawn the asteroids
        for (int i = 0; i < numAsteroids; i++)
        {
            SpawnAsteroid();
        }

        SpawnASpaceship();
    }

    private void SpawnAsteroid()
    {
        // instantiate the asteroid
        GameObject asteroid;
        bool valid;
        do {
            asteroid = Instantiate(asteroidPrefab);
            asteroid.gameObject.tag = "Asteroid";
            valid = CheckTooCloseToAsteroids(asteroid);
        } while (valid == false);
        
        return;
    }

    private void SpawnASpaceship()
    {
        // instantiate the asteroid
        bool valid;
        do {
            spaceship = Instantiate(spaceshipPrefab);
            spaceship.gameObject.tag = "Spaceship";
            valid = CheckTooCloseToAsteroids(spaceship);
        } while (valid == false);
        
    }

    private bool CheckTooCloseToAsteroids(GameObject testObject)
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (GameObject asteroid in asteroids)
        {
            if (asteroid != testObject)
            {
                float distance = Vector3.Distance(asteroid.transform.position, testObject.transform.position);
                if (distance < minColissionDistance)
                {
                    Destroy(testObject);
                    return false;
                }
            }
            
        }

        return true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
