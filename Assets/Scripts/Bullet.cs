using UnityEngine;

public class Bullet : MonoBehaviour
{
    float maxX = 12f;
    float maxY = 7f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -maxX ||
            transform.position.x > maxX  ||
            transform.position.y < -maxY ||
            transform.position.y > maxY)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            other.gameObject.GetComponent<Asteroid>().TakeDamage();
            Destroy(gameObject);
        }
    }
}
