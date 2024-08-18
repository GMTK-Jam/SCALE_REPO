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
    private int healthInt = 100;
    private int scaleInt = 0;
    private int xpInt = 0;
    public Slider healthSlider;
    public Slider scaleSlider;
    private int currentLevel = 1;
    public float pickupDistance = 10f;

    [SerializeField]
    private float _damageCooldown = 2.0f; // Cooldown time in seconds

    // Dictionary to track the last damage time for each enemy
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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        foreach (Transform child in transform)
        {
            Limb limb;
            if (child.TryGetComponent<Limb>(out limb))
            {
                limbs.Add(limb);
            }
        }
    }

    public void Move(Vector2 velocity, float rotation)
    {
        rb.velocity = velocity;
        rb.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collide(GameObject collisionObject)
    {
        if (collisionObject.GetComponent<Enemy>())
        {
            // Get the current time
            float currentTime = Time.time;

            // Check if we have a record of this enemy's last damage time
            if (!_lastDamageTimeByEnemy.TryGetValue(collisionObject, out float lastDamageTime))
            {
                // If no record, set the last damage time to a very old time
                lastDamageTime = -_damageCooldown;
            }

            // Check if the cooldown period has passed
            if (currentTime - lastDamageTime >= _damageCooldown)
            {
                collisionObject.GetComponent<Enemy>().TakeDamage(10);
                TakeDamage(10);
                Debug.Log("Damage taken from " + collisionObject.name);

                // Update the last damage time for this enemy
                _lastDamageTimeByEnemy[collisionObject] = currentTime;
            }
        }
    }

    public void TakeDamage(int damageTaken)
    {
        healthInt -= damageTaken;
        healthSlider.value = healthInt / 100f;
        uiText.text = $"Health: {healthInt} XP: {xpInt}";
    }

  /*  public void AddScale(int scaleGained)
    {
        scaleInt += scaleGained;

        if (currentLevel >= 2)
        {
            scaleSlider.value = scaleInt - upgradeScales[currentLevel - 2];
            scaleSlider.maxValue = upgradeScales[currentLevel - 1] - upgradeScales[currentLevel - 2];
        }
        else
        {
            scaleSlider.value = scaleInt;
            scaleSlider.maxValue = upgradeScales[0];
        }

        if (scaleInt > upgradeScales[currentLevel - 1])
        {
            UpLevel();
        }
    }
  */

    public void AddXP(string xpType, int xpCount)
    {
        if(xpType == "heart")
        {
            heart.AddXP(xpCount);
        }
        if (xpType == "leg")
        {
            leg.AddXP(xpCount);
        }
        if (xpType == "arm")
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
    }


    public void UpLevel()
    {
        currentLevel += 1;
        upgradeScreen.SetActive(true);
        Time.timeScale = 0.05f;
        levelText.text = "LV" + currentLevel.ToString();
    }

    public void Button()
    {
        upgradeScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void VisionButton()
    {
        upgradeScreen.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(ChangeFOV(cineCamera, 40f, 0.5f));
    }

    IEnumerator ChangeFOV(CinemachineVirtualCamera cam, float endFOV, float duration)
    {
        float startFOV = cam.m_Lens.OrthographicSize;
        float time = 0;
        while (time < duration)
        {
            cam.m_Lens.OrthographicSize = Mathf.Lerp(startFOV, endFOV, time / duration);
            yield return null;
            time += Time.deltaTime;
        }
    }
}
