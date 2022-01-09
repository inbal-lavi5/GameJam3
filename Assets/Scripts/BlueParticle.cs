using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueParticle : MonoBehaviour
{
    [SerializeField] private GameObject explosion;

    public void Detonate()
    {
        GameObject particles = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(particles.gameObject, 2);
        Destroy(transform.gameObject);
    }
}