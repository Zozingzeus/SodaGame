using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    float speed;
    float lastShot;
    float shootCd;
    GameObject arm;
    GameObject gun;
    public GameObject projectile;
    public int hp;
    public bool useCd;

	// Use this for initialization
	void Start () {
        speed = 5f;
        shootCd = .2f;
        hp = 100;
        useCd = false;

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
        
	}
    
    // Update is called once per frame
    void Update () {

        // Arm rotation
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        arm.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90);

        // Arm localscale
        Vector3 angle = arm.transform.eulerAngles;
        Vector3 armScale = arm.transform.localScale;
        if (angle.z >= 0f && angle.z <=180f)
            armScale.x = 1;
        else
            armScale.x = -1;
        arm.transform.localScale = armScale;

        // Shoot
        if (Input.GetButtonDown("Fire1"))
            Shoot();

        // Check if dead
        if (hp <= 0)
            Destroy(gameObject);
    }


    // Movement
    void FixedUpdate () {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.Translate(move * Time.deltaTime * speed);

        // Walking animation
        if (move.magnitude >= .1) {
            GetComponent<Animator>().SetBool("walking", true);
        } else {
            GetComponent<Animator>().SetBool("walking", false);
        }
	}

    void Shoot() {
        if(Time.time >= lastShot + shootCd || !useCd) {
            GameObject projectileClone = Instantiate(projectile, gun.transform.position, arm.transform.rotation) as GameObject;
            Physics2D.IgnoreCollision(projectileClone.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
            lastShot = Time.time;
        }
    }

    public void TakeDamage(int damage) {
        hp -= damage;
    }

}
