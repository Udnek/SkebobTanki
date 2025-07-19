using System.Collections;
using Tank;
using UnityEngine;

public class HitscanWeapon : MonoBehaviour
{
    public float impactForce = 10f;
    public float fireRate = 5f;
    public float fireRange = 50f;
    public bool spreadEnabled = true;
    public float bulletSpreadFactor = 0.1f;
    
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private ParticleSystem impactEffect;
    [SerializeField]
    private TrailRenderer bulletTrail;
    [SerializeField]
    public Transform firePoint;
    [SerializeField]
    private LayerMask layerMask;
    
    private float timeReloaded;
    
    void Update()
    {
        if (!Input.GetButton("Fire1") || !(Time.time > timeReloaded)) return;
        
        timeReloaded = Time.time + 1/fireRate;
        TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
        Vector3 direction = GetDirection();
        
        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, fireRange, layerMask))
        {
            Vector3 relativeHitPoint = hit.point - hit.transform.position;
            Quaternion toHitRotation = Quaternion.FromToRotation(hit.transform.forward, hit.normal);
            StartCoroutine(SpawnTrail(trail, hit, relativeHitPoint, toHitRotation));
            hit.collider.GetComponent<HitReceiver>()?.Hit(new HitEvent(hit, impactForce, direction));
        }
        else
        {
            StartCoroutine(SpawnTrail(trail, firePoint.position + direction * fireRange));
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

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 trailEndPoint)
    {
        Vector3 trailStartPoint = trail.transform.position;
        float distance = Vector3.Distance(trailStartPoint, trailEndPoint);
        float remainingDistance = distance;
        
        while (remainingDistance > 0)   
        {
            trail.transform.position = Vector3.Lerp(trailStartPoint, trailEndPoint, 1 - remainingDistance / distance);
            remainingDistance -= 1 / trail.time * Time.deltaTime;

            yield return null;
        }
        trail.transform.position = trailEndPoint;
        Destroy(trail.gameObject, trail.time);
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit, Vector3 relativeHitPoint, Quaternion toHitRotation)
    {
        Vector3 trailStartPoint = trail.transform.position;
        float distance = Vector3.Distance(trailStartPoint, hit.point);
        float remainingDistance = distance;
        while (remainingDistance > 0)   
        {
            trail.transform.position = Vector3.Lerp(trailStartPoint, hit.point, 1 - remainingDistance / distance);
            remainingDistance -= 1 / trail.time * Time.deltaTime;

            yield return null;
        }

        trail.transform.position = hit.point;
        ParticleSystem impactEffectInstance = Instantiate(impactEffect, 
            hit.transform.position + relativeHitPoint,
            Quaternion.LookRotation(toHitRotation * hit.transform.forward)
            );
        impactEffectInstance.transform.SetParent(hit.transform);
        
        Destroy(trail.gameObject, trail.time);
    }
}
