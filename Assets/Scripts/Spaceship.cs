using UnityEngine;

public class Spaceship : MonoBehaviour
{
    private float trhustForce = 0.00015f;
    private float turnSpeed = 180;
    //Ship starts up
    private Vector3 shipDirection = new Vector3(0, 1, 0);

    private Rigidbody2D rb;

    float maxX = 9.2f;
    float maxY = 5.2f;

    // Awake is called when the script instance is first instanced
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
}
