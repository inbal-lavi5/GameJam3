using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] protected GameObject explosion;

    public virtual void Detonate()
    {
        GameObject particles = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(particles.gameObject, 2);
        Destroy(transform.gameObject);
    }
}