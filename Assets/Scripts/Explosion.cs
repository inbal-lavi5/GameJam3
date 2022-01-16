using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Particle
{
    [SerializeField] public GameManager gameManager;

    public float power = 10;
    public float radius = 5;
    public float upforce = 1;

    public override void Detonate()
    {
        GameObject particles = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(particles.gameObject, 2);

        Vector3 explosionPosition = transform.position;
        Collider[] coliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach (Collider hit in coliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb == null && !hit.CompareTag("Plane") && !hit.CompareTag("Player"))
            {
                hit.gameObject.AddComponent<Rigidbody>()
                    .AddExplosionForce(power, explosionPosition, radius, upforce, ForceMode.Impulse);
                if (hit.CompareTag("Collapse"))
                {
                    gameManager.AddRigidChildren(hit.transform.parent.parent);
                }
            }

            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upforce, ForceMode.Impulse);
            }
        }

        Destroy(transform.gameObject);
    }
}