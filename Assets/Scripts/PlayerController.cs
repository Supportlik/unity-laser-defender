using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject projectile;
    public float health;
    public float shipSpeed;
    public float projectileSpeed;
    public float fireRate;
    public Image[] lives = new Image[5];
    public Button menuButton;
    public Text menuText;
    


    private float minX;
    private float maxX;
    private float padding = 1.75f;       
    private KeyCode moveLeftKey = KeyCode.LeftArrow;
    private KeyCode moveRightKey = KeyCode.RightArrow;
    private KeyCode fireKey = KeyCode.Space;
    private KeyCode menuKey = KeyCode.Escape;
    private string hostileColliderTag = "EnemyLaser";


    // Use this for initialization
    void Start () {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        minX = leftmost.x + padding;
        maxX = rightmost.x - padding;
        lives[0] = GameObject.Find("Live1").GetComponent<Image>();
        lives[1] = GameObject.Find("Live2").GetComponent<Image>();
        lives[2] = GameObject.Find("Live3").GetComponent<Image>();
        lives[3] = GameObject.Find("Live4").GetComponent<Image>();
        lives[4] = GameObject.Find("Live5").GetComponent<Image>();
        menuButton.enabled = false;
        menuText.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        HandleControll();
        HandleLaser();
        if (Input.GetKeyDown(menuKey)){

            GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadLevel("Start Menu");
        }
	}

    private void Fire() {
        Vector3 startPosition = transform.position + Vector3.up;
        GameObject beam = Instantiate(projectile, startPosition, Quaternion.identity) as GameObject;
        //beam.transform.parent = transform;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
        GetComponent<AudioSource>().Play();
    }

    private void HandleLaser() {
        if (Input.GetKeyDown(fireKey)) {
            InvokeRepeating("Fire", 0.000001f, fireRate);
        }
        if (Input.GetKeyUp(fireKey)) {
            CancelInvoke("Fire");
        }
    }

    private void HandleControll() {
        if (Input.GetKey(moveLeftKey) && transform.position.x > minX) {
            transform.position += Vector3.left * shipSpeed * Time.deltaTime;
        } else if (Input.GetKey(moveRightKey) && transform.position.x < maxX) {
            transform.position += Vector3.right * shipSpeed * Time.deltaTime;
        }
        float newX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(newX, transform.position.y, 0);
    }

    public float GetHealth() {
        return health;
    }

    void GetHit(float damage) {
        Debug.Log("GOT HIT");
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        int live_remaining = (int)health / 10;
        Debug.Log("HEALTH: " + live_remaining);
        Debug.Log("HEALTH: " + health);
        for (int i = 5; i > (live_remaining); i--) {
            lives[i - 1].enabled = false;
        }
        if (live_remaining == 0) {
            menuButton.enabled = true;
            menuText.enabled = true;   
        }

    }

    void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("TRIGGER: LASER HIT");
        if (collider.gameObject.CompareTag(hostileColliderTag))
        {
            LaserController laserController = collider.gameObject.GetComponent<LaserController>();
            GetHit(laserController.GetDamage());
            laserController.Hit();
        }
    }
}
