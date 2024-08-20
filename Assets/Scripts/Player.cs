using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    static Player _instance;
    public CinemachineVirtualCamera cineCamera;
    private Rigidbody2D rb;
    private DamageEventListener _damageEventListener;

    #region PlayerStats
    public int healthInt = 100;
    public int healthMaxInt = 100;
    public int recoveryPerSecond = 1;
    private int scaleInt = 0;
    private int xpInt = 0;
    public float pickupDistance = 10f;
    public float pickupSpeed = 20f;
    [SerializeField]
    private float _damageCooldown = 2.0f;
    #endregion

    #region Limbs
    /*    public List<Limb> limbs = new List<Limb>();
    */
    public Heart heart;
    public Eye eye;
    public Leg leg;
    public Arm arm;
    public Mouth mouth;
    public Lung lung;
    #endregion

    #region UI
    public TextMeshProUGUI uiText;
    public float[] upgradeScales;
    public GameObject upgradeScreen;
    public TextMeshProUGUI levelText;
    public Image heartImage;
    public Image eyeImage;
    public Image legImage;
    public Image armImage;
    public Image mouthImage;
    public Image lungImage;
    public GameObject uiOverlay;
    public TextMeshProUGUI heartText;
    public Slider healthSlider;
    public Slider scaleSlider;
    public TextMeshProUGUI weightText;
    #endregion

    #region Sprinting
    public float sprintFactor = 1.5f;
    public float sprintLength = 5f;
    public float recoveryLength = 10f;
    private float sprintTimer;
    private bool isSprinting = false;
    private bool isRecovering = false;
    public Slider sprintSlider;
    #endregion

    /*    private Dictionary<GameObject, float> _lastDamageTimeByEnemy = new Dictionary<GameObject, float>();
*/

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
        sprintTimer = sprintLength;
        sprintSlider.value = 1f;

/*        foreach (Transform child in transform)
        {
            Limb limb;
            if (child.TryGetComponent<Limb>(out limb))
            {
                limbs.Add(limb);
            }
        }*/

    }

    void Update()
    {
        heartText.text = healthInt.ToString() + "/" + healthMaxInt.ToString();

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

        // Check for sprint input
        if (Input.GetKey(KeyCode.LeftShift) && sprintTimer > 0)
        {
            isSprinting = true;
            sprintTimer -= Time.deltaTime;
            sprintTimer = Mathf.Clamp(sprintTimer, 0f, sprintLength);
            if (sprintTimer <= 0)
            {
                isRecovering = true;
                isSprinting = false;
            }
        }
        else
        {
            isSprinting = false;

            // Recovery logic handled here
            if (isRecovering || sprintTimer < sprintLength)
            {
                sprintTimer += (sprintLength / recoveryLength) * Time.deltaTime;
                sprintTimer = Mathf.Clamp(sprintTimer, 0f, sprintLength);

                // Only stop recovery when fully recovered
                if (sprintTimer >= sprintLength)
                {
                    isRecovering = false;
                }
            }
        }

        sprintSlider.value = sprintTimer / sprintLength;

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

    public void Move(Vector2 velocity, float rotation)
    {
        if (isSprinting && sprintTimer > 0)
        {
            velocity *= sprintFactor;
        }

        rb.velocity = velocity;

        float targetAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        float smoothedAngle = Mathf.LerpAngle(rb.rotation, targetAngle, Time.deltaTime * 12f);
        rb.rotation = smoothedAngle;
    }

    // Deprecated
    public void Collide(GameObject collisionObject)
    {
        /*if (collisionObject.GetComponent<Enemy>())
        {
            float currentTime = Time.time;

            if (!_lastDamageTimeByEnemy.TryGetValue(collisionObject, out float lastDamageTime))
            {
                lastDamageTime = -_damageCooldown;
            }

            if (currentTime - lastDamageTime >= _damageCooldown)
            {
                collisionObject.GetComponent<Enemy>().TakeDamage(10);
                SubtractHealth(10);
                Debug.Log("Damage taken from " + collisionObject.name);

                _lastDamageTimeByEnemy[collisionObject] = currentTime;
            }
        }*/
    }


    // ##################### Damage Handling ##################### //

    public void SubtractHealth(int damageTaken)
    {
        healthInt -= damageTaken;
        healthInt = Mathf.Clamp(healthInt, 0, healthMaxInt);
        healthSlider.value = (float)healthInt / healthMaxInt;
    }

    public void EvaluateDamageQueue()
    {
        DamageInfo damageInfo = _damageEventListener.PopDamage();
        if (damageInfo == null) return;
        float rawDamage = damageInfo.damage;
        float finalDamage = rawDamage; // Enter damage scaling code if needed

        SubtractHealth((int)finalDamage);
    }

    // ##################### Upgrade Handing ##################### //

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

    public IEnumerator UpgradeMaxHealth(int newHealthInt, int newIncreaseRate)
    {
        healthMaxInt = newHealthInt;
        recoveryPerSecond = newIncreaseRate;
        SubtractHealth(0);
        Vector3 initialScale = healthSlider.transform.localScale;
        float targetScaleX = 0.38f * (newHealthInt / 70f);
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

    public IEnumerator UpgradeMaxSprint(float newSprintLentth, float newRecoveryTime)
    {
        sprintLength = newSprintLentth;
        recoveryLength = newRecoveryTime;
        Vector3 initialScale = sprintSlider.transform.localScale;
        float targetScaleX = 0.2f * (newSprintLentth / lung.sprintTimeStages[0]);
        Vector3 targetScale = new Vector3(targetScaleX, initialScale.y, initialScale.z);
        float duration = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            sprintSlider.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        sprintSlider.transform.localScale = targetScale;
    }

    public int CalculateWeight()
    {
        int totalWeight = heart.stagesWeight[heart.stage] + lung.stagesWeight[lung.stage] + arm.stagesWeight[arm.stage] + leg.stagesWeight[leg.stage] + eye.stagesWeight[eye.stage] + mouth.stagesWeight[mouth.stage];
        weightText.text = ("Weight: " + totalWeight.ToString());
        return totalWeight;
    }
}
