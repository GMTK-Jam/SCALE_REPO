using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    public List<Limb> limbs = new List<Limb>();
    static Player _instance;
    public int healthInt = 100;
    public int healthMaxInt = 100;
    public int recoveryPerSecond = 1;

    private int scaleInt = 0;
    private int xpInt = 0;
    public Slider healthSlider;
    public Slider scaleSlider;
    private int currentLevel = 1;
    public float pickupDistance = 10f;
    public float pickupSpeed = 20f;
    [SerializeField]
    private float _damageCooldown = 2.0f;
    public TextMeshProUGUI weightText;

    private Dictionary<GameObject, float> _lastDamageTimeByEnemy = new Dictionary<GameObject, float>();

    public TextMeshProUGUI uiText;
    public float[] upgradeScales;
    public GameObject upgradeScreen;
    public TextMeshProUGUI levelText;
    public CinemachineVirtualCamera cineCamera;
    public Heart heart;
    public Eye eye;
    public Leg leg;
    public Arm arm;
    public Mouth mouth;
    public Lung lung;

    public Image heartImage;
    public Image eyeImage;
    public Image legImage;
    public Image armImage;
    public Image mouthImage;
    public Image lungImage;

    public GameObject uiOverlay;

    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Player>();
            }
            return _instance;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        uiOverlay.SetActive(false);
        foreach (Transform child in transform)
        {
            Limb limb;
            if (child.TryGetComponent<Limb>(out limb))
            {
                limbs.Add(limb);
            }
        }

        // Start health regeneration coroutine
        StartCoroutine(HealthRegeneration());
    }

    public void Move(Vector2 velocity, float rotation)
    {
        rb.velocity = velocity;
        rb.rotation = rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            uiOverlay.SetActive(true);
            Time.timeScale = 0.05f;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            uiOverlay.SetActive(false);
            Time.timeScale = 1f;
        }

        heartImage.fillAmount = heart.FillPercentage();
        heartImage.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "LV" + (heart.stage + 1).ToString();
        legImage.fillAmount = leg.FillPercentage();
        legImage.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "LV" + (leg.stage + 1).ToString();
        armImage.fillAmount = arm.FillPercentage();
        armImage.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "LV" + (arm.stage + 1).ToString();

        eyeImage.fillAmount = eye.FillPercentage();
        eyeImage.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "LV" + (eye.stage + 1).ToString();

        mouthImage.fillAmount = mouth.FillPercentage();
        mouthImage.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "LV" + (mouth.stage + 1).ToString();
        lungImage.fillAmount = lung.FillPercentage();
        lungImage.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "LV" + (lung.stage + 1).ToString();
    }

    public void Collide(GameObject collisionObject)
    {
        if (collisionObject.GetComponent<Enemy>())
        {
            float currentTime = Time.time;

            if (!_lastDamageTimeByEnemy.TryGetValue(collisionObject, out float lastDamageTime))
            {
                lastDamageTime = -_damageCooldown;
            }

            if (currentTime - lastDamageTime >= _damageCooldown)
            {
                collisionObject.GetComponent<Enemy>().TakeDamage(10);
                TakeDamage(10);
                Debug.Log("Damage taken from " + collisionObject.name);

                _lastDamageTimeByEnemy[collisionObject] = currentTime;
            }
        }
    }

    public void TakeDamage(int damageTaken)
    {
        healthInt -= damageTaken;
        healthInt = Mathf.Clamp(healthInt, 0, healthMaxInt); // Ensure health doesn't go below 0
        healthSlider.value = (float)healthInt / healthMaxInt;
    }

    public void AddXP(string xpType, int xpCount)
    {
        if (xpType == "heart")
        {
            heart.AddXP(xpCount);
        }
        if (xpType == "legs")
        {
            leg.AddXP(xpCount);
        }
        if (xpType == "arms")
        {
            arm.AddXP(xpCount);
        }
        if (xpType == "eye")
        {
            eye.AddXP(xpCount);
        }
        if (xpType == "mouth")
        {
            mouth.AddXP(xpCount);
        }
        if (xpType == "lung")
        {
            lung.AddXP(xpCount);
        }
    }

    public IEnumerator UpdateHealth(int newHealthInt, int newIncreaseRate)
    {
        healthMaxInt = newHealthInt;
        recoveryPerSecond = newIncreaseRate;
        TakeDamage(0);
        Vector3 initialScale = healthSlider.transform.localScale;
        float targetScaleX = 0.38f * (newHealthInt / 100f);
        Vector3 targetScale = new Vector3(targetScaleX, initialScale.y, initialScale.z);
        float duration = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            healthSlider.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        healthSlider.transform.localScale = targetScale;
    }

    private IEnumerator HealthRegeneration()
    {
        while (true)
        {
            if (healthInt < healthMaxInt)
            {
                healthInt += recoveryPerSecond;
                healthInt = Mathf.Clamp(healthInt, 0, healthMaxInt); // Ensure health doesn't exceed max
                healthSlider.value = (float)healthInt / healthMaxInt;
            }
            yield return new WaitForSeconds(1f); // Wait 1 second between each increment
        }
    }

    public int CalculateWeight()
    {
        int totalWeight = heart.stagesWeight[heart.stage] + lung.stagesWeight[lung.stage] + arm.stagesWeight[arm.stage] + leg.stagesWeight[leg.stage] + eye.stagesWeight[eye.stage] + mouth.stagesWeight[mouth.stage];
        weightText.text = ("Weight: " + totalWeight.ToString());
        return totalWeight;
    }
}
