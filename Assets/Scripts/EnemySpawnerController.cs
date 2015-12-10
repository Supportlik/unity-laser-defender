using UnityEngine;
using System.Collections;

public class EnemySpawnerController : MonoBehaviour {

    public GameObject enemyPrefab;
    public float width = 23f;
    public float height = 7f;
    public float spawnDelay = 0.1f;

    private float speed = 5f;
    private float padding = 12f;
    private float minX;
    private float maxX;

    bool turningRight = true;

	// Use this for initialization
	void Start () {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        minX = leftmost.x + padding;
        maxX = rightmost.x - padding;
        EnemyController.shotsPerSeconds = 0.1f;
        
        SpawnUntilFull();
	}
	
	// Update is called once per frame
	void Update () {
        FormationMovement();
        if (AllMembersDead()) SpawnUntilFull();
	}

    void SpawnUntilFull() {
        Transform freePosition = NextFreePosition();
        if (freePosition) {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
            Invoke("SpawnUntilFull", spawnDelay);
        }

    }

    Transform NextFreePosition() {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0) return childPositionGameObject;
        }
        return null;
    }

    bool AllMembersDead() {
        foreach (Transform childPositionGameObject in transform) {
            if (childPositionGameObject.childCount > 0) return false;
        }
        
        EnemyController.shotsPerSeconds += 0.05f;
        ScoreController sc = GameObject.Find("Score").GetComponent<ScoreController>();
        sc.AddScore(100);
        return true;
    }

    void FormationMovement() {
        if (turningRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, transform.position.z);
        if (transform.position.x == minX)
        {
            turningRight = true;
        }

        if (transform.position.x == maxX)
        {
            turningRight = false;
        }
    }

    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }
}
