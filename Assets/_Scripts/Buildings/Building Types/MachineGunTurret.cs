using System.Collections;
using UnityEngine;

public class MachineGunTurret : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] AudioClip shooting_SFX;
    [SerializeField] float shootRange = 14f;
    [SerializeField] float fireRate = 0.1f;

    Transform target;
    Transform playerProjectilesPool;
    Building buildingScript;
    Transform enemyShipsPool;
    MachineGunSFX machineGunSFXScript;

    float minDeviation = -0.15f;
    float maxDeviation = 0.15f;
    bool canShoot;

    int turret_index;

    void Awake()
    {
        playerProjectilesPool = GameObject.FindGameObjectWithTag("PlayerProjectilesPool").transform;
        buildingScript = GetComponent<Building>();
        enemyShipsPool = GameObject.FindGameObjectWithTag("EnemyShipsPool").transform;
        machineGunSFXScript = GameObject.FindGameObjectWithTag("MachineGunSFX").GetComponent<MachineGunSFX>();
    }

    void Start()
    {
        canShoot = true;
        AddNewMachineGunToIndex();
    }

    void Update()
    {
        if (!buildingScript.IsBuildingActive || enemyShipsPool.childCount <= 0)
        {
            machineGunSFXScript.Turrets[turret_index] = false;
            return;
        }

        FindClosestTarget();

        if (target == null)
        {
            machineGunSFXScript.Turrets[turret_index] = false;
            return;
        }
  
        TurretAI();
    }

    void AddNewMachineGunToIndex()
    {
        machineGunSFXScript.MG_index++;
        turret_index = machineGunSFXScript.MG_index;
        machineGunSFXScript.Turrets.Add(turret_index, false);
    }

    public void RevomeTurretFromList()
    {
        machineGunSFXScript.Turrets.Remove(turret_index);
    }

    void TurretAI()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget < shootRange && canShoot)
        {
            machineGunSFXScript.Turrets[turret_index] = true;
            StartCoroutine(Shooting());  
        }
        else if (distanceToTarget > shootRange)
        {
            machineGunSFXScript.Turrets[turret_index] = false;
        }
    }

    IEnumerator Shooting()
    {
        canShoot = false;

        InstantiateProjectile();

        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    void InstantiateProjectile()
    {
        GameObject projectileTransform = Instantiate(projectile, transform.position, Quaternion.identity, playerProjectilesPool);
        
        Vector3 shootDirection = (target.position - transform.position).normalized;
        Vector3 finalDirection = (shootDirection + new Vector3(Random.Range(minDeviation, maxDeviation), Random.Range(minDeviation, maxDeviation), 0f));

        projectileTransform.GetComponent<GunBall>().ShootSetup(finalDirection);
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
        Gizmos.color = new Color(1.0f, 0.64f, 0.0f);
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
