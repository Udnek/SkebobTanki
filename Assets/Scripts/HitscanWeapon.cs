using System.Collections;
using UnityEngine;

public class HitscanWeapon : MonoBehaviour
{
    public float impactForce = 10f;
    public float fireRate = 5f;
    public float fireRange = 50f;
    public bool spreadEnabled = true;
    public float bulletSpreadFactor = 0.1f;

    
    private float timeReloaded;
    
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private ParticleSystem impactEffect;
    [SerializeField]
    private TrailRenderer bulletTrail;
    [SerializeField]
    public Transform firePoint;
    
    void Update()
    {
        if (!Input.GetButton("Fire1") || !(Time.time > timeReloaded)) return;
        timeReloaded = Time.time + 1/fireRate;
        if (Physics.Raycast(firePoint.position, GetDirection(), out RaycastHit hit, fireRange))
        {
            TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));
            hit.rigidbody?.AddForce(-hit.normal * impactForce);
        }
        muzzleFlash.Play();
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        if (spreadEnabled)
        {
            direction += new Vector3(
                Random.Range(-bulletSpreadFactor, bulletSpreadFactor),
                Random.Range(-bulletSpreadFactor, bulletSpreadFactor),
                Random.Range(-bulletSpreadFactor, bulletSpreadFactor)
            );
            direction.Normalize();
        }
        return direction;

    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;
        Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        
        Destroy(trail.gameObject, trail.time);
    }
}
