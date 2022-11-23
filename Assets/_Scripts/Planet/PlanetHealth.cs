using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetHealth : MonoBehaviour
{
    [SerializeField] Image healthIndicator;
    [SerializeField] GameObject planetUI;
    [SerializeField] TextMeshProUGUI planetHealthText;

    [SerializeField] GameObject explosionShip_VFX;
    [SerializeField] GameObject explosionLaser_VFX;
    [SerializeField] GameObject explosionMissile_VFX;
    [SerializeField] GameObject explosionNuke_VFX;
    [SerializeField] GameObject planetDestroy_VFX1;
    [SerializeField] GameObject planetDestroy_VFX2;
    [SerializeField] GameObject planetDestroy_VFX3;

    [SerializeField] AudioClip explosionShip_SFX;
    [SerializeField] AudioClip explosionLaser_SFX;
    [SerializeField] AudioClip explosionMissile_SFX;
    [SerializeField] AudioClip explosionNuke_SFX;
    [SerializeField] AudioClip planetDestroy_SFX;

    Transform VFXsPool;
    SFXManager sm;

    float totalHealth = 1000f;
    float currentHealth;

    bool isPlanetDestroy = false;
    public bool IsPlanetDestroy { get => isPlanetDestroy; }

    Color initialHealthColor;

    void Awake()
    {
        VFXsPool = GameObject.FindGameObjectWithTag("VFXs").transform;
        sm = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
    }

    void Start()
    {
        currentHealth = totalHealth;
        initialHealthColor = healthIndicator.color;
    }

    void Update()
    {
        healthIndicator.fillAmount = currentHealth / totalHealth;

        DisplayHealthText();
    }

    void DisplayHealthText()
    {
        planetHealthText.text = $"{currentHealth} / {totalHealth}";
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlickerEffect());

        if (currentHealth <= 0)
        {
            sm.PlayOneShot(planetDestroy_SFX);
            DisplayPlanetDestructionVFXs();

            DestroyPlanet();
        }
    }

    IEnumerator FlickerEffect()
    {
        var tempColor = healthIndicator.color;
        tempColor.a = 0.5f;
        healthIndicator.color = tempColor;

        yield return new WaitForSeconds(0.1f);
        healthIndicator.color = initialHealthColor;
    }

    void DisplayImpactVFX(Collider2D collision)
    {
        Vector3 impactDirection = (collision.transform.position - transform.position).normalized;

        if (collision.gameObject.CompareTag("Harpoon"))
        {
            sm.PlayOneShot(explosionShip_SFX);
            DisplayImpactVFX(explosionShip_VFX, collision, impactDirection);
        }
        else if (collision.CompareTag("EnemyProjectile"))
        {
            sm.PlayOneShot(explosionLaser_SFX);
            DisplayImpactVFX(explosionLaser_VFX, collision, impactDirection);
        }
        else if (collision.CompareTag("EnemyMissile"))
        {
            sm.PlayOneShot(explosionMissile_SFX);
            DisplayImpactVFX(explosionMissile_VFX, collision, impactDirection);
        } 
        else if (collision.CompareTag("EnemyNuke"))
        {
            sm.PlayOneShot(explosionNuke_SFX);
            DisplayImpactVFX(explosionNuke_VFX, collision, impactDirection);
        } 
    }

    void DisplayImpactVFX(GameObject prefab, Collider2D collision, Vector3 direction)
    {
        Instantiate(prefab,
            collision.transform.position,
            Quaternion.AngleAxis(GetAngleFromVectorFloat(direction) - 90f, Vector3.forward),
            VFXsPool);
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    void DisplayPlanetDestructionVFXs()
    {
        Instantiate(planetDestroy_VFX1, transform.position, Quaternion.identity, VFXsPool);
        Instantiate(planetDestroy_VFX2, transform.position, Quaternion.identity, VFXsPool);
        Instantiate(planetDestroy_VFX3, transform.position, Quaternion.identity, VFXsPool);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collider IMPACT - " + collision.gameObject.name);

        Destroy(collision.gameObject);
    }

    void DestroyPlanet()
    {
        isPlanetDestroy = true;

        planetUI.SetActive(false);
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);

        Invoke("ProcessToEndGame", 2f);
    }

    void ProcessToEndGame()
    {
        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        gm.EndDialogState(true);
        gm.SetEndDialogWaveNumber();
        gm.SettingsButtonState(false);
        gm.IsGameOver = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile") 
            || collision.CompareTag("EnemyMissile")
            || collision.CompareTag("EnemyNuke"))
        {
            TakeDamage(collision.GetComponent<Damage>().vsComponent);

            DisplayImpactVFX(collision);

            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Harpoon"))
        {
            TakeDamage(collision.GetComponentInParent<Damage>().vsComponent);

            DisplayImpactVFX(collision);

            Destroy(collision.transform.parent.transform.parent.gameObject);
        }
    }
}
