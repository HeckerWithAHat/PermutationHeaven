using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public ParticleSystem trail;
    public ParticleSystem explosion;


    private void Awake()
    {
        trail.Play();
        explosion.Stop();
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            return;
        }
        Explode();
    }

    public void Explode()
    {
        explosion.Play();
        trail.Stop();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 5f);
    }
}
