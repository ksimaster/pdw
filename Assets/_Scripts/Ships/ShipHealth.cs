using UnityEngine;
using UnityEngine.UI;

public class ShipHealth : MonoBehaviour
{
    [SerializeField] GameObject shield;
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider shieldSlider;

    [SerializeField] GameObject hitShield_VFX;
    [SerializeField] GameObject hitHull_VFX;
    [SerializeField] GameObject shieldDestroy_VFX;
    [SerializeField] GameObject shipDestroy_VFX;
    [SerializeField] GameObject impactScrap_VFX;
    [SerializeField] GameObject impactMissileExplosion_VFX;

    [SerializeField] AudioClip shipShieldBroken_SFX;
    [SerializeField] AudioClip explosionShip_SFX;

    [SerializeField] float hullHP;
    [SerializeField] float shieldHP;

    Transform VFXsPool;
    SFXManager sm;

    bool isShieldDisabled;

    float currentHullHP;
    float currentShieldHP;
    public float CurrentShieldHP { get => currentShieldHP; }

    void Awake()
    {
        VFXsPool = GameObject.FindGameObjectWithTag("VFXs").transform;
        sm = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
    }

    void Start()
    {
        currentHullHP = hullHP;
        currentShieldHP = shieldHP;

        isShieldDisabled = currentShieldHP <= 0;

        DisplaySlidersValue();
    }

    void Update()
    {
        DisplaySlidersValue();
    }

    public void DamageShield(int amount)
    {
        currentShieldHP -= amount;

        if (currentShieldHP <= 0 && !isShieldDisabled)
        {
            sm.PlayOneShot(shipShieldBroken_SFX);

            isShieldDisabled = true;

            sm.PlayOneShot(shipShieldBroken_SFX);

            DisplayShieldDestroyVFX();
            Destroy(shield.gameObject);
        }
    }

    void DamageHull(int amount)
    {
        currentHullHP -= amount;

        if (currentHullHP <= 0)
        {
            sm.PlayOneShot(explosionShip_SFX);
            DisplayShipDestroyVFX();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile"))
        {
            if (!isShieldDisabled)
            {
                DamageShield(collision.GetComponent<Damage>().vsShield);
                DisplayImpactShieldVFX(collision);
            }
            else
            {
                DamageHull(collision.GetComponent<Damage>().vsComponent);
                DisplayImpactHullVFX(collision);
            }

            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("PlayerScrapProjectile"))
        {
            if (!isShieldDisabled)
            {
                DisplayImpactShieldVFX(collision);
            }
            else
            {
                DamageHull(collision.GetComponent<Damage>().vsComponent);
                DisplayImpactHullVFX(collision);
            }

            Instantiate(impactScrap_VFX, collision.transform.position, Quaternion.identity, VFXsPool);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("PlayerMissileProjectile"))
        {
            if (!isShieldDisabled)
            {
                DamageShield(collision.GetComponent<Damage>().vsShield);
                DisplayImpactShieldVFX(collision);
            }
            else
            {
                DamageHull(collision.GetComponent<Damage>().vsComponent);
                DisplayImpactHullVFX(collision);
            }

            Instantiate(impactMissileExplosion_VFX, collision.transform.position, Quaternion.identity, VFXsPool);
            Destroy(collision.gameObject);
        }
    }

    void DisplaySlidersValue()
    {
        healthSlider.value = currentHullHP / hullHP;
        shieldSlider.value = currentShieldHP / shieldHP;

        if (shieldSlider.value <= 0)
        {
            shieldSlider.transform.Find("Fill Area").gameObject.SetActive(false);
        }

    }

    void DisplayImpactShieldVFX(Collider2D collision)
    {
        Instantiate(hitShield_VFX, collision.transform.position,
                Quaternion.LookRotation(collision.transform.position - transform.position),
                VFXsPool);
    } 
    
    void DisplayImpactHullVFX(Collider2D collision)
    {
        Instantiate(hitHull_VFX, collision.transform.position,
                Quaternion.LookRotation(collision.transform.position - transform.position),
                VFXsPool);
    }

    void DisplayShieldDestroyVFX()
    {
        sm.PlayOneShot(shipShieldBroken_SFX);
        Instantiate(shieldDestroy_VFX, transform.position, Quaternion.identity, VFXsPool);
    }

    void DisplayShipDestroyVFX()
    {
        Instantiate(shipDestroy_VFX, transform.position, Quaternion.identity, VFXsPool);
    }
}
