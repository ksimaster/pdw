using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;
    [SerializeField] GameObject explosion_VFX;

    Rigidbody2D rb;
    Transform target;
    Vector2 direction;
    
    float rotateSpeed = 250f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectiles"), LayerMask.NameToLayer("Projectiles"));
        Invoke("DriftingDestroy", 10f);
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(direction));
    }

    void FixedUpdate()
    {
        MoveToTarget();
    }

    public void ShootSetup(Transform target)
    {
        this.target = target;
    }

    void MoveToTarget()
    {
        if (target == null) return;

        direction = (Vector2)target.transform.position - rb.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed; 
        rb.AddForce(direction * speed, ForceMode2D.Force);
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    void DriftingDestroy()
    {
        Instantiate(explosion_VFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
