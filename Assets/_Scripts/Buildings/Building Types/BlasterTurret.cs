using System.Collections;
using UnityEngine;

public class BlasterTurret : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] AudioClip blaster_SFX;
    [SerializeField] float shootRange = 15f;
    [SerializeField] float fireRate = 0.5f;

    Transform target;
    Transform playerProjectilesPool;
    Building buildingScript;
    Transform enemyShipsPool;
    SFXManager sm;

    float minDeviation = -0.05f;
    float maxDeviation = 0.05f;
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

        InstantiateProjectile();
        sm.PlayOneShot(blaster_SFX);

        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    void InstantiateProjectile()
    {
        GameObject projectileTransform = Instantiate(projectile, transform.position, Quaternion.identity, playerProjectilesPool);
        
        Vector3 shootDirection = (target.position - transform.position).normalized;
        Vector3 finalDirection = (shootDirection + new Vector3(Random.Range(minDeviation, maxDeviation), Random.Range(minDeviation, maxDeviation), 0f));
        
        projectileTransform.GetComponent<Blaster>().ShootSetup(finalDirection);
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
