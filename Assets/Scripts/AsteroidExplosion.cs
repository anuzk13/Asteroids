using System.Collections;
using UnityEngine;

public class AsteroidExplosion : MonoBehaviour
{
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameObject.name = "AsteroidExplosion";
        gameObject.tag = "AsteroidExplosion";
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
    
    public void SetAudio(float volumeLevel, float pitchLevel)
    {
        audioSource.pitch = pitchLevel;
        audioSource.volume = volumeLevel;
    }
}
