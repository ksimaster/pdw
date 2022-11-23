using System.Collections;
using UnityEngine;

public class ScrapTurret : MonoBehaviour
{
    [SerializeField] GameObject[] projectiles;
    [SerializeField] AudioClip shooting_SFX;
    [SerializeField] float shootRange = 10f;
    [SerializeField] float fireRate = 4f;

    Transform target;
    Transform playerProjectilesPool;
    Building buildingScript;
    Transform enemyShipsPool;
    SFXManager sm;

    float minDeviation = -0.22f;
    float maxDeviation = 0.22f;
    bool canShoot;

    int numOfProjectiles = 5;

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

        for (int i = 0; i < numOfProjectiles; i++)
        {
            sm.PlayOneShot(shooting_SFX);
            InstantiateProjectiles();
        }

        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    void InstantiateProjectiles()
    {
        GameObject projectileTransform = Instantiate(projectiles[Random.Range(0, projectiles.Length)], transform.position, Quaternion.identity, playerProjectilesPool);

        Vector3 shootDirection = (target.position - transform.position).normalized;
        Vector3 finalDirection = (shootDirection + new Vector3(Random.Range(minDeviation, maxDeviation), Random.Range(minDeviation, maxDeviation), 0f));

        projectileTransform.GetComponent<Scrap>().ShootSetup(finalDirection);
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
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
