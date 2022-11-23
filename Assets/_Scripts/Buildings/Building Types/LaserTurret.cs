using System.Collections;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    [SerializeField] GameObject laserHit_VFX;
    [SerializeField] float shootRange = 16f;

    LineRenderer lineR;
    Building buildingScript;
    Transform target;
    ShipHealth shipHealth;
    Transform enemyShipsPool;

    bool canShoot;
    int laserShieldDamage = 1;

    bool shouldCauseDamage = true;
    float damageCooldown = 0.1f;

    bool isVFXDisplaying = false;

    void Awake()
    {
        lineR = GetComponent<LineRenderer>();
        buildingScript = GetComponent<Building>();
        enemyShipsPool = GameObject.FindGameObjectWithTag("EnemyShipsPool").transform;
    }

    void Start()
    {
        SetInitialLineProperties();
        canShoot = true;
    }

    void Update()
    {
        if (!buildingScript.IsBuildingActive || enemyShipsPool.childCount <= 0) return;
        FindClosestTarget();

        if (target == null) return;
        TurretAI();
    }

    void SetInitialLineProperties()
    {
        lineR.useWorldSpace = true;
        lineR.endWidth = 0.03f;
        lineR.startWidth = 0.09f;
    }

    void FindClosestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("EnemyShips");

        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

            if ((distanceToTarget < maxDistance) && target.GetComponent<ShipHealth>().CurrentShieldHP > 0)
            {
                closestTarget = target.transform;
                maxDistance = distanceToTarget;
            }
            else
            {
                lineR.enabled = false;
            }
        }

        if (closestTarget == null) return;

        target = closestTarget;
        shipHealth = target.GetComponent<ShipHealth>();
    }

    void TurretAI()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget < shootRange && canShoot)
        {
            Shooting();
        }
    }

    void Shooting()
    {
        if (shipHealth == null) return;

        if (shipHealth.CurrentShieldHP <= 0) 
        {
            lineR.enabled = false;
            isVFXDisplaying = false;
        }
        else
        {
            lineR.enabled = true;
            lineR.SetPosition(0, transform.position);
            lineR.SetPosition(1, target.position);

            if (!isVFXDisplaying)
            {
                DisplayShieldDestroyVFX();
            }

            if (shouldCauseDamage)
            {
                StartCoroutine(DamageShield());
            }
        }  
    }

    IEnumerator DamageShield()
    {
        shouldCauseDamage = false;

        yield return new WaitForSeconds(damageCooldown);
        shipHealth.DamageShield(laserShieldDamage);

        shouldCauseDamage = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }

    void DisplayShieldDestroyVFX()
    {
        isVFXDisplaying = true;
        Transform targetShield = target.Find("Components").Find("Shield").transform;

        Instantiate(laserHit_VFX, target.position, Quaternion.identity, targetShield);
    }
}
