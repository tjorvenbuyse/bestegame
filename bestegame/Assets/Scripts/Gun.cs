using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{

    public float damage = 10f;
    float headShotMultiplier = 2.5f;
    float armShotMultiplier = 0.8f;
    float legShotMultiplier = 0.6f;
    float feetShotMultiplier = 0.5f;

    public float range = 100f;
    public float fireRate = 5f;
    public float impactForce = 30f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    public Animator animator;

    private void Start()
    {
        currentAmmo = maxAmmo;   
    }
    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
    // Update is called once per frame
    void Update()
    {
        if (isReloading) return;
        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
        if(Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        currentAmmo--;

        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            // for Target
            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                // Damage
                // head shot
                if (hit.transform.tag == "Head")
                {
                    target.TakeDamage(damage * headShotMultiplier);
                } // arm shot
                else if(hit.transform.tag == "Arm")
                {
                    target.TakeDamage(damage * armShotMultiplier);
                } // leg shot
                else if (hit.transform.tag == "Leg")
                {
                    target.TakeDamage(damage * legShotMultiplier);
                } // feet shot
                else if (hit.transform.tag == "Feet")
                {
                    target.TakeDamage(damage * feetShotMultiplier);
                }
                else // body shot
                {
                    target.TakeDamage(damage);
                }
            }

            // for DPSTarget
            DPSTarget dpsTarget = hit.transform.GetComponent<DPSTarget>();
            if (dpsTarget != null)
            {
                // Damage
                // head shot
                if (hit.transform.tag == "Head")
                {
                    dpsTarget.dpsScore(damage * headShotMultiplier);
                } // arm shot
                else if (hit.transform.tag == "Arm")
                {
                    dpsTarget.dpsScore(damage * armShotMultiplier);
                } // leg shot
                else if (hit.transform.tag == "Leg")
                {
                    dpsTarget.dpsScore(damage * legShotMultiplier);
                } // feet shot
                else if (hit.transform.tag == "Feet")
                {
                    dpsTarget.dpsScore(damage * feetShotMultiplier);
                }
                else // body shot
                {
                    dpsTarget.dpsScore(damage);
                }
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);
        }
    }

    //Coroutine (used for delaying a event while everything else is still working)
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - 0.25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
