using System.Collections;
using UnityEngine;

public class ShipShooting : MonoBehaviour
{
    enum ProjectileTypes { blaster, missile, nuke}

    [SerializeField] ProjectileTypes projectileType;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] AudioClip missile_SFX;
    [SerializeField] AudioClip blaster_SFX;

    [SerializeField] float shotDelay = 4f;

    Transform target;
    Transform enemyProjectilesPool;
    SFXManager sm;

    bool isShooting = false;
    public bool IsShooting { get => isShooting; set => isShooting = value; }

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("PlanetBase").GetComponent<Transform>();
        enemyProjectilesPool = GameObject.FindGameObjectWithTag("EnemyProjectilesPool").transform;
        sm = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
    }

    public void StartAttack()
    {
        StartCoroutine(ShootTarget());
    }

    IEnumerator ShootTarget()
    {
        isShooting = true;

        yield return new WaitForSeconds(shotDelay);
        InstantiateProjectile();

        isShooting = false;
    }

    void InstantiateProjectile()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, enemyProjectilesPool);

        if (target == null) return;

        if (projectileType == ProjectileTypes.blaster)
        {
            sm.PlayOneShot(blaster_SFX);
            Vector3 shootDirection = (target.position - transform.position).normalized;
            newProjectile.GetComponent<Blaster>().ShootSetup(shootDirection);
        }
        else if (projectileType == ProjectileTypes.missile || projectileType == ProjectileTypes.nuke)
        {
            sm.PlayOneShot(missile_SFX);
            newProjectile.GetComponent<Missile>().ShootSetup(target);
        }
    }
}
