using System;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public GameObject spaceshipPrefab;
    public GameObject [] lifeIconPrefab;
    private GameObject spaceship;
    private GameObject gameOverSign;
    private GameObject levelCleredSign;

    // should have a get and set for this
    [NonSerialized]
    public float timeDied;
    private float respawnTime = 3.0f;

    private int myScore;
    private Score scoreText;

    private int numMaxAsteroids = 10;
    // should have a get and set for this
    [NonSerialized]
    public int numAsteroids;
    private int numLivesLeft;
    private int maxLives = 4;
    private float minColissionDistance = 2.0f;
    private bool gameFinished =  false;
    private float finisTime;
    private bool gameWon = false;

    private void Awake()
    {
        myScore = 0;
        numLivesLeft = maxLives;
        gameOverSign = GameObject.Find("GameOver");
        levelCleredSign = GameObject.Find("LevelCleared");
        scoreText = FindAnyObjectByType<Score>();
        scoreText.UpdateScore(myScore);
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        numAsteroids = numMaxAsteroids;
        // spawn the asteroids
        for (int i = 0; i < numAsteroids; i++)
        {
            Debug.Log("Spawning Asteroid");
            SpawnAsteroid();
        }

        SpawnSpaceship();

        Assert.IsNotNull(gameOverSign);
        gameOverSign.SetActive(false);

        Assert.IsNotNull(levelCleredSign);
        levelCleredSign.SetActive(false);

        gameFinished = false;
        gameWon = false;
    }

    private void SpawnAsteroid()
    {
        // instantiate the asteroid
        GameObject asteroid;
        bool valid;
        do {
            asteroid = Instantiate(asteroidPrefab);
            valid = CheckTooCloseToAsteroids(asteroid);
        } while (valid == false);
        asteroid.GetComponent<Asteroid>().setGameController(this);
        return;
    }

    private void SpawnSpaceship()
    {
        // This fails on death because the spaceship is destroyed but not really null
        // Assert.IsNull(spaceship);
        // instantiate the asteroid
        bool valid;
        do {
            spaceship = Instantiate(spaceshipPrefab);
            spaceship.gameObject.tag = "Spaceship";
            valid = CheckTooCloseToAsteroids(spaceship);
        } while (valid == false);

        numLivesLeft -= 1;
        spaceship.GetComponent<Spaceship>().setGameController(this);
        return;
    }

    public void IncreaseScore()
    {
        myScore += 10;
        scoreText.UpdateScore(myScore);
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
        // check if the spaceship is destroyed
        if (spaceship == null)
        {
            if (Time.time - timeDied < respawnTime)
            {
                return;
            }
            if (numLivesLeft > 0)
            {
                SpawnSpaceship();
                // update life icon
                Destroy(lifeIconPrefab[numLivesLeft]);
            } 
            else 
            {
                gameOverSign.SetActive(true);
            }
        }
        // check if all asteroids are destroyed
        if ((numAsteroids == 0) && !gameWon)
        {
            if (gameFinished)
            {
                if (Time.time - finisTime < respawnTime)
                {
                    levelCleredSign.SetActive(true);
                    gameFinished = false;
                    gameWon = true;
                    StartCoroutine(Pause());
                }
            }
            else
            {
                gameFinished = true;
                finisTime = Time.time;
            }
        }
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(3f);
        numMaxAsteroids = numMaxAsteroids * 2;
        if (numMaxAsteroids > 16)
        {
            numMaxAsteroids = 16;
        }

        Destroy(spaceship);
        spaceship = null;
        // Intialize Level ->  Spawn Spaceship reduces the number of lives by one
        numLivesLeft++;
        InitializeLevel();
    }
}
