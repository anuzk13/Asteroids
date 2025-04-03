using UnityEngine;

public class Spaceship : MonoBehaviour
{   
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    private float maxSpeed = 2f;
    private float trhustForce = 0.00015f;
    private float turnSpeed = 180;
    //Ship starts up
    private Vector3 shipDirection = new Vector3(0, 1, 0);

    private Rigidbody2D rb;

    float maxX = 9.2f;
    float maxY = 5.2f;
    private float bulletSpeed = 20f;

    private GameController gameController;

    private AudioSource audioSource;
    public AudioClip shootingSoundFX;
    public AudioClip thursterSoundFX;

    // Awake is called when the script instance is first instanced
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        // set random position
        transform.position = new Vector3(Random.Range(-maxX + 2f, maxX - 2f), Random.Range(-maxY + 2f, maxY - 2f), 0);
        gameObject.name = "Spaceship";
        gameObject.tag = "Spaceship";
    }

    public void setGameController(GameController _gameController)
    {
        this.gameController = _gameController;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float turnAngle;
        if (Input.GetKey(KeyCode.J))
        {
            // turn left
            turnAngle = turnSpeed * Time.deltaTime;
            transform.Rotate(0,0, turnAngle );
            shipDirection = Quaternion.Euler(0, 0, turnAngle) * shipDirection;
        }
        if (Input.GetKey(KeyCode.L))
        {
            // turn right
            turnAngle = - turnSpeed * Time.deltaTime;
            transform.Rotate(0,0, turnAngle );
            shipDirection = Quaternion.Euler(0, 0, turnAngle) * shipDirection;
        }
        if (Input.GetKey(KeyCode.K))
        {
            // move forward
            rb.AddForce(shipDirection * trhustForce);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            audioSource.Stop();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            audioSource.clip = thursterSoundFX;
            audioSource.Play();
        }

        // ship firing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.PlayOneShot(shootingSoundFX);
            // create a bullet
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 90);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = shipDirection * bulletSpeed;
        }

        //throttle max speed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
        
        // if ship goes out of bounds, wrap around
        if (transform.position.x < -maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, 0);
        } 
        else if (transform.position.x > maxX)
        {
            transform.position = new Vector3(-maxX, transform.position.y, 0);
        }

        if (transform.position.y < -maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, 0);
        } 
        else if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, -maxY, 0);
        }

        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            gameController.timeDied = Time.time;
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.transform.position = transform.position;
            // Destroy the spaceship
            Destroy(gameObject);
        }
    }
}
