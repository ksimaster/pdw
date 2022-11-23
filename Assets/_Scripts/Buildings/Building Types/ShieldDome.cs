using UnityEngine;
using UnityEngine.UI;

public class ShieldDome : MonoBehaviour
{
    [SerializeField] GameObject hitShieldDome_VFX;
    [SerializeField] GameObject destroyShieldDome_VFX;
    [SerializeField] GameObject impactMissileExplosion_VFX;
    [SerializeField] GameObject impactNukeExplosion_VFX;
    [SerializeField] GameObject impactShipExplosion_VFX;

    [SerializeField] AudioClip shieldDomeBroken_SFX;
    [SerializeField] AudioClip shieldDomeActive_SFX;
    [SerializeField] AudioClip explosionProjectile_SFX;
    [SerializeField] AudioClip explosionMissile_SFX;
    [SerializeField] AudioClip explosionNuke_SFX;
    [SerializeField] AudioClip explosionShip_SFX;

    Building buildingScript;
    Collider2D circleCollider;
    SpriteRenderer shieldSprite;

    GameObject shieldDomeUIElement;
    Slider shieldSlider;
    Image fillArea;
    Transform VFXsPool;
    SFXManager sm;

    bool isShieldInitiate = false;
    bool isShieldDischarge = false;

    float shieldHP = 30f;
    float currentHP;

    float cooldownTime = 30f;
    float sliderTime = 0f;
    Color initColor;
    Color cooldowmColor = Color.yellow;

    void Awake()
    {
        circleCollider = GetComponent<Collider2D>();
        shieldSprite = GetComponent<SpriteRenderer>();
        shieldDomeUIElement = transform.parent.Find("Shield Dome UI").gameObject;
        buildingScript = GetComponent<Building>();
        shieldSlider = shieldDomeUIElement.GetComponentInChildren<Slider>();
        fillArea = shieldSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        VFXsPool = GameObject.FindGameObjectWithTag("VFXs").transform;
        sm = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
    }

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Planet"), LayerMask.NameToLayer("Shield"));

        shieldDomeUIElement.SetActive(false);
        DisableShieldDome();
        initColor = fillArea.color;
        currentHP = shieldHP; 
    }

    void Update()
    {
        ShieldDomeState();

        if (isShieldInitiate && !isShieldDischarge)
        {
            DisplaySliderValue();
        }
    }

    void ShieldDomeState()
    {
        if (!isShieldInitiate && buildingScript.IsBuildingActive)
        {
            shieldDomeUIElement.SetActive(true);
            ActivateShieldDome();
            isShieldInitiate = true;
        }
        else if (isShieldDischarge)
        {
            DisableShieldDome();
            RechargingShield();
        }
    }

    void ActivateShieldDome()
    {
        circleCollider.enabled = true;
        shieldSprite.enabled = true;

        sm.PlayOneShot(shieldDomeActive_SFX);
    }

    void DisableShieldDome()
    {
        circleCollider.enabled = false;
        shieldSprite.enabled = false;
    }

    public void DestroyShieldDomeObjectAndElement()
    {
        shieldDomeUIElement.SetActive(false);
        Destroy(gameObject);
    }

    void RechargingShield()
    {
        fillArea.color = cooldowmColor;

        if (sliderTime < cooldownTime)
        {
            sliderTime += Time.deltaTime;

            shieldSlider.value = sliderTime / cooldownTime;
        }
        else
        {
            sliderTime = 0f;
            fillArea.color = initColor;
            currentHP = shieldHP;
            isShieldDischarge = false;

            ActivateShieldDome();
        }
    }

    void ReduceShieldPoints(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
        {
            DisplayDestroyShieldVFX();
            DisableShieldDome();

            isShieldDischarge = true;
        }
    }

    void DisplaySliderValue()
    {
        shieldSlider.value = 1 * (currentHP / shieldHP);
    }

    void DisplayImpactShieldVFX(Collider2D collision)
    {
        Instantiate(hitShieldDome_VFX, collision.transform.position, Quaternion.identity, VFXsPool);
    }

    void DisplayDestroyShieldVFX()
    {
        var shieldDestroyVFX = Instantiate(destroyShieldDome_VFX, transform.position, Quaternion.identity, VFXsPool);
        shieldDestroyVFX.transform.localScale = new Vector3(2.2f, 2.2f, 1f);

        sm.PlayOneShot(shieldDomeBroken_SFX);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile"))
        {
            sm.PlayOneShot(explosionProjectile_SFX);

            DisplayImpactShieldVFX(collision);
            ReduceShieldPoints(collision.GetComponent<Damage>().vsShield);

            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("EnemyMissile"))
        {
            sm.PlayOneShot(explosionMissile_SFX);

            Instantiate(impactMissileExplosion_VFX, collision.transform.position, Quaternion.identity, VFXsPool);
            DisplayImpactShieldVFX(collision);

            ReduceShieldPoints(collision.GetComponent<Damage>().vsShield);

            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("EnemyNuke"))
        {
            sm.PlayOneShot(explosionNuke_SFX);

            Instantiate(impactNukeExplosion_VFX,
             collision.transform.position,
             Quaternion.AngleAxis(GetAngleFromVectorFloat(collision.transform.position - transform.position)
             - 90f, Vector3.forward),
             VFXsPool);

            DisplayImpactShieldVFX(collision);

            ReduceShieldPoints(collision.GetComponent<Damage>().vsShield);

            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Harpoon"))
        {
            sm.PlayOneShot(explosionShip_SFX);

            Instantiate(impactShipExplosion_VFX,
             collision.transform.position,
             Quaternion.AngleAxis(GetAngleFromVectorFloat(collision.transform.position - transform.position)
             - 90f, Vector3.forward),
             VFXsPool);

            DisplayImpactShieldVFX(collision);

            ReduceShieldPoints(collision.GetComponentInParent<Damage>().vsShield);

            Destroy(collision.transform.parent.transform.parent.gameObject);
        }
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
