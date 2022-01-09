using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float power = 10;
    public float radius = 5;
    public float upforce = 1;

    [SerializeField] private GameObject explosion;

    public void Detonate()
    {
        print("DETTTTT");
        
        GameObject particles = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(particles.gameObject, 2);
        
        Vector3 explosionPosition = transform.position;
        Collider[] coliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach (Collider hit in coliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upforce, ForceMode.Impulse);
            }
        }

        Destroy(transform.parent.gameObject);
    }

    public void Dest()
    {
        Destroy(transform.parent.gameObject);
    }
}