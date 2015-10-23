using UnityEngine;
using System.Collections;
using UnityEditor;

public class Enemy : MonoBehaviour {

    float shootRadius;
    float walkRadius;
    bool inLineOfSight;
    float lastShot;
    float shootCd;
    GameObject arm;
    GameObject gun;

    public Transform target;
    float speed;

    public int hp;
    public GameObject projectile;
    public LayerMask whatToHit;

	// Used by Spawner.cs
	private GameObject objSpawn;
	private int SpawnerID;
    

    // Use this for initialization
    void Start () {

        hp = 30;
        target = GameObject.Find("Player").transform;
        shootRadius = 3;
        walkRadius = 10;
        shootCd = 1f;
        speed = 3f;

        // Set arm and gun gameObjects
        foreach (Transform child in transform) {
            if (child.gameObject.name == "Arm") {
                arm = child.gameObject;
            }
        }
        foreach (Transform child in arm.transform) {
            if (child.gameObject.name == "Gun") {
                gun = child.gameObject;
            }
        }

		// Used by Spawner.cs
		objSpawn = (GameObject) GameObject.FindWithTag ("Spawner");
    }
	
	// Update is called once per frame
	void Update () {
        // Check if dead
        if (hp <= 0) {
			// Used by Spawner.cs
			objSpawn.BroadcastMessage("killEnemy", SpawnerID);
			Destroy (gameObject);
		}
	}

    void FixedUpdate() {

        // Movement(with animations and arm rotation)
        Vector2 move = target.position - transform.position;
        float rotZ = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
        if (move.magnitude >= shootRadius && move.magnitude <= walkRadius) {
			move.Normalize();
			transform.Translate(move * speed * Time.deltaTime);
            GetComponent<Animator>().SetBool("walking", true);
        } else if (move.magnitude <= shootRadius && inLineOfSight) {
            Shoot();
            GetComponent<Animator>().SetBool("walking", false);
        } else {
            GetComponent<Animator>().SetBool("walking", false);
        }
        arm.transform.rotation = Quaternion.Euler(0, 0, rotZ + 90);

        // Arm localscale
        Vector3 angle = arm.transform.eulerAngles;
        Vector3 armScale = arm.transform.localScale;
        if (angle.z >= 0f && angle.z <= 180f)
            armScale.x = 1;
        else
            armScale.x = -1;
        arm.transform.localScale = armScale;

        // Line of sight check
        RaycastHit2D hit = Physics2D.Raycast(transform.position, move, walkRadius, whatToHit);
        if(hit.collider != null) {
            if (hit.collider.gameObject.name == "Player") {
                inLineOfSight = true;
            } else {
                inLineOfSight = false;
            }
        }
        
    }
    
    void Shoot () {
        if (Time.time >= lastShot + shootCd) {
            GameObject projectileClone = Instantiate(projectile, gun.transform.position, arm.transform.rotation) as GameObject;
            Physics2D.IgnoreCollision(projectileClone.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            lastShot = Time.time;
        }
    }

    public void TakeDamage(int damage) {
        hp -= damage;
    }

    void OnDrawGizmos () {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.forward, shootRadius);
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.forward, walkRadius);
        if(inLineOfSight) Debug.DrawLine(transform.position, target.position, Color.green);
        else Debug.DrawLine(transform.position, target.position, Color.red);
    }

	// Used by Spawner.cs
	void setName(int sName) {
		SpawnerID = sName;
	}
}
