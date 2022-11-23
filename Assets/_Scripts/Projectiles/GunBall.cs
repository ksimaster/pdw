using UnityEngine;

public class GunBall : MonoBehaviour
{
    Vector3 shootDirection;

    float speed = 10f;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectiles"), LayerMask.NameToLayer("Projectiles"));
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += shootDirection * speed * Time.deltaTime;
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
