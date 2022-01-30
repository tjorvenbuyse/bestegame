using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;
    public float blastRadius = 6f;
    public float explosionForce = 700f;
    public float explosionDamage = 700f;

    float distance;

    public GameObject explosionEffect;

    float countdown;
    bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;   
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        // Show effect
        GameObject grenade = Instantiate(explosionEffect, transform.position, transform.rotation);

        // Get nearby objects
        Collider[] collidersToDamage = Physics.OverlapSphere(transform.position, blastRadius);
        foreach(Collider nearbyObject in collidersToDamage)
        {
            Target dest = nearbyObject.GetComponent<Target>();
            if(dest != null)
            {
                distance = Vector3.Distance(dest.transform.position, gameObject.transform.position);
                dest.TakeDamage(explosionDamage/(distance*2));
            }
        }

        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, blastRadius);
        foreach(Collider nearbyObject in collidersToMove)
        {
            // Add force
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, blastRadius);
            }
        }

        // Remove grenade
        Destroy(gameObject);
        Destroy(grenade,2f);
    }
}
