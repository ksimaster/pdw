using UnityEngine;

public class Scrap : MonoBehaviour
{
    Vector3 shootDirection;

    float speed;
    float rotationSpeed;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectiles"), LayerMask.NameToLayer("Projectiles"));
        speed = Random.Range(1.5f, 3f);
        rotationSpeed = Random.Range(120f, 300f);
    }

    void Update()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        transform.position += shootDirection * speed * Time.deltaTime;
    }

    void Rotate()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    public void ShootSetup(Vector3 shootDirection)
    {
        this.shootDirection = shootDirection;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDirection));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
