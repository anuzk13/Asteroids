using System.Collections;
using UnityEngine;

public class ShipExplosion : MonoBehaviour
{
    private void Awake()
    {
        gameObject.name = "ShipExplosion";
        gameObject.tag = "ShipExplosion";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
