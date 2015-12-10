using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

        public GameObject projectile;    
    public float health;
    public static float shotsPerSeconds;
    public float projectileSpeed;
    public int points;
    public AudioClip arriveSound;
    public AudioClip destroySound;
    public AudioClip fireSound;

    private string hostileColliderTag = "PlayerLaser";

    void Start() {
        AudioSource.PlayClipAtPoint(arriveSound, transform.position);
    }

    void Update() {
        if (ShouldFire()) Fire();
    }

    bool ShouldFire() {
        float probability = Time.deltaTime * shotsPerSeconds;
        return Random.value < probability;
    }


    void GetHit(float damage) {
        health -= damage;
        if (health <= 0)
        {
            ScoreController sc = GameObject.Find("Score").GetComponent<ScoreController>();
            sc.AddScore(points);
            Die();
        }
    }

    void Die() {        
        AudioSource.PlayClipAtPoint(destroySound, transform.position);
        Destroy(gameObject);
    }

    private void Fire() {
        Vector3 startPosition = transform.position + Vector3.down;
        GameObject beam = Instantiate(projectile, startPosition, Quaternion.identity) as GameObject;        
        beam.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(hostileColliderTag)) {
            LaserController laserController = collider.gameObject.GetComponent<LaserController>();
            GetHit(laserController.GetDamage());
            laserController.Hit();
        }
    }
}
