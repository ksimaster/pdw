using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] GameObject[] thrusters_VFX;
    [SerializeField] float stopRange = 20f;
    [SerializeField] float speed = 0.05f;

    Transform components;
    Rigidbody2D rb;
    Transform target;
    ShipShooting shipShootingScript;

    bool shouldThrust = false;

    float speedReductionFactor = 0.97f;

    void Awake()
    {
        components = transform.Find("Components").transform;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("PlanetBase").transform;
        shipShootingScript = GetComponent<ShipShooting>();
    }

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ships"), LayerMask.NameToLayer("Ships"));
    }

    void Update()
    {
        if (target == null || target.gameObject.GetComponent<PlanetHealth>().IsPlanetDestroy) return;

        ShipIA();

        Vector2 direction = (Vector2)target.transform.position - rb.position;
        direction.Normalize();
        components.transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(direction));
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        rb.MoveRotation(angle);

        if (shouldThrust)
        {
            rb.velocity = targetDirection * speed;
        }
        else
        {
            SlowThrusting();
        }
    }
    void SlowThrusting()
    {
        rb.velocity = rb.velocity * speedReductionFactor;
    }

    void ShipIA()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget > stopRange)
        {
            shouldThrust = true;
            ChangeThrustersState(true);
        }
        else
        {
            shouldThrust = false;
            ChangeThrustersState(false);

            if (!shipShootingScript.IsShooting)
            {
                shipShootingScript.StartAttack();
            }
        }
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    void ChangeThrustersState(bool value)
    {
        foreach (GameObject item in thrusters_VFX)
        {
            item.SetActive(value);
        }
    }
}
