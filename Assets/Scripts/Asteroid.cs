using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public GameObject explosionPrefab;
    private Rigidbody2D rb;

    private GameController gameController;
    private float maxX = 10.5f;
    private float maxY = 6.2f;
    private float maxSpeed = 2.5f;
    private float minSpeed = 0.1f;
    private int health = 5;
    // size of the asteroid
    public int scale;
    private int maxScale = 3;
    public GameObject asteroidPrefab;
    public float childAsteroidOffset = 1f;

    void Awake()
    {

        scale = maxScale;
        rb = GetComponent<Rigidbody2D>();
        gameObject.name = "Asteroid";
        gameObject.tag = "Asteroid";
        // set random position
        transform.position = new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), 0);
        rb.linearVelocity = new Vector2(Random.Range(minSpeed, maxSpeed), Random.Range(minSpeed, maxSpeed));
        // rb.linearVelocity = new Vector2(0,0);
    }

    public void setGameController(GameController _gameController)
    {
        this.gameController = _gameController;
    }

    // Update is called once per frame
    void Update()
    {
        // throttle velocity to max speed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // if ateroid goes out of bounds, wrap around
        if (transform.position.x < -maxX)
        {
            transform.position = new Vector2(maxX, transform.position.y);
        } 
        else if (transform.position.x > maxX)
        {
            transform.position = new Vector2(-maxX, transform.position.y);
        }
        if (transform.position.y < -maxY)
        {
            transform.position = new Vector2(transform.position.x, maxY);
        } 
        else if (transform.position.y > maxY)
        {
            transform.position = new Vector2(transform.position.x, -maxY);
        }
        
    }

    public void TakeDamage()
    {
        health -= 1;
        if (health == 0)
        {
            gameController.IncreaseScore();
            Die();
        }
    }

    private void Die()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        int scaleFactor = maxScale - scale;
        explosion.GetComponent<AsteroidExplosion>().SetAudio(0.8f - scaleFactor * 0.25f, 1 + scaleFactor * 0.5f);
        explosion.transform.position = transform.position;
        ParticleSystem partSys = explosion.GetComponent<ParticleSystem>();
        partSys.Stop();
        var main = partSys.main;
        if (scale < 3 && scale > 0)
        {
            main.startSize = scale;
        }
        else if (scale == 0)
        {
            main.startSize = 0.5f;
        }
        main.simulationSpeed = 1 * (maxScale - scale + 1);
        partSys.Play();
        
        if (scale > 0)
        {
            SpawnChildAsteroid();
            // update the number of asteroids (+4)
            gameController.numAsteroids += 4;
        }

        // update the number of asteroids (-1)
        gameController.numAsteroids -= 1;
        Destroy(gameObject);
    }

    private void SpawnChildAsteroid()
    {
        Vector2[] newDirections = new Vector2[4];
        newDirections[0] = new Vector2(1, 0);
        newDirections[1] = new Vector2(0, 1);
        newDirections[2] = new Vector2(-1, 0);
        newDirections[3] = new Vector2(0, -1);

        float randAngle = Random.Range(0, 360);
        for (int i = 0; i < newDirections.Length; i++)
        {
            // rotate new direction by rand angle and pertubs it a bit
            newDirections[i] = Quaternion.Euler(0, 0, randAngle + Random.Range(-30,30)) * newDirections[i];
            GameObject childAsteroid = Instantiate(asteroidPrefab);
            Asteroid asteroidHandle = childAsteroid.GetComponent<Asteroid>();
            childAsteroid.transform.position = transform.position + (Vector3)newDirections[i] * childAsteroidOffset;
            childAsteroid.transform.localScale = transform.localScale / 2;
            asteroidHandle.scale = scale - 1;
            asteroidHandle.childAsteroidOffset = childAsteroidOffset / 2;
            asteroidHandle.setGameController(gameController);
            
            Rigidbody2D childRb = childAsteroid.GetComponent<Rigidbody2D>();
            // Mass goes down by facctor of egiht because it's cubic
            childRb.mass = rb.mass / 8;
            childRb.AddForce(newDirections[i] * childAsteroidOffset * childAsteroidOffset * childAsteroidOffset * 5);
        }
    }   
}
