using System.Collections;
using UnityEngine;

public class MissileTurret : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] AudioClip missile_SFX;
    [SerializeField] float shootRange = 18f;
    [SerializeField] float fireRate = 10f;

    Transform target;
    Transform playerProjectilesPool;
    Building buildingScript;
    Transform enemyShipsPool;
    SFXManager sm;

    bool canShoot;

    void Awake()
    {
        playerProjectilesPool = GameObject.FindGameObjectWithTag("PlayerProjectilesPool").transform;
        buildingScript = GetComponent<Building>();
        enemyShipsPool = GameObject.FindGameObjectWithTag("EnemyShipsPool").transform;
        sm = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
    }

    void Start()
    {
        canShoot = true;
    }

    void Update()
    {
        if (!buildingScript.IsBuildingActive || enemyShipsPool.childCount <= 0) return;

        FindClosestTarget();

        if (target == null) return;
        TurretAI();
    }

    void TurretAI()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget < shootRange && canShoot)
        {
            StartCoroutine(Shooting());
        }
    }

    IEnumerator Shooting()
    {
        canShoot = false;

        sm.PlayOneShot(missile_SFX);
        InstantiateProjectile();

        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    void InstantiateProjectile()
    {
        GameObject projectileTransform = Instantiate(projectile, transform.position, Quaternion.identity, playerProjectilesPool);
        Vector3 shootDirection = (target.position - transform.position).normalized;
        projectileTransform.GetComponent<Missile>().ShootSetup(target);
    }

    void FindClosestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("EnemyShips");
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (GameObject targetPos in targets)
        {
            float distanceToTarget = Vector2.Distance(transform.position, targetPos.transform.position);

            if (distanceToTarget < maxDistance)
            {
                closestTarget = targetPos.transform;
                maxDistance = distanceToTarget;
            }
        }

        target = closestTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 1);
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}