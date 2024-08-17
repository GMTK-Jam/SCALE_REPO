<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    public List<Limb> limbs;
    static Player _instance;
    private int healthInt = 100;
    private int xpInt = 0;

    [SerializeField]
    private float _damageCooldown = 2.0f; // Cooldown time in seconds

    // Dictionary to track the last damage time for each enemy
    private Dictionary<GameObject, float> _lastDamageTimeByEnemy = new Dictionary<GameObject, float>();

    public TextMeshProUGUI uiText;

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
        uiText.text = ("Health:" + healthInt.ToString() + " XP:" + xpInt.ToString());

        foreach (Transform child in transform)
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    public List<Limb> limbs;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

/*        foreach (Transform child in transform)
>>>>>>> 83461356243b0118054d4dcb09d6cc3517bbc657
        {
            Limb limb;
            if (TryGetComponent<Limb>(out limb))
            {
                limbs.Add(limb);
            }
<<<<<<< HEAD
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        rb.velocity = moveInput * moveSpeed;
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
        uiText.text = ("Health:" + healthInt.ToString() + " XP:" + xpInt.ToString());
    }

    public void AddXp(int xpGained)
    {
        xpInt += xpGained;
        uiText.text = ("Health:" + healthInt.ToString() + " XP:" + xpInt.ToString());
    }
}
=======
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        rb.velocity = moveInput* moveSpeed;
    }
}
>>>>>>> 83461356243b0118054d4dcb09d6cc3517bbc657
