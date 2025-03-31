using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float maxX = 9.2f;
    [SerializeField] private float maxY = 5.2f;
    [SerializeField] private float maxSpeed = 0.5f;
    [SerializeField] private float minSpeed = 0.1f;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // set random position
        transform.position = new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), 0);
        rb.linearVelocity = new Vector2(Random.Range(minSpeed, maxSpeed), Random.Range(minSpeed, maxSpeed));
        // rb.linearVelocity = Quaternion.Euler(0, 0, Random.Range(0, 360)) * new Vector2(Random.Range(0.5f, maxSpeed), 0.0f);

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
}
