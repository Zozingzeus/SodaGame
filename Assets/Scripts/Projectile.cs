using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    Vector2 moveDir;
    float speed;
    float startTime;
    float lifeTime;
    int damage;

	// Use this for initialization
	void Start () {
        speed = 20f;
        startTime = Time.time;
        lifeTime = 5f;
        damage = 10;
        moveDir = Vector2.down;
	}

    // Update is called once per frame
    void Update () {
        if (Time.time >= startTime + lifeTime) {
            Destroy(gameObject);
        }
    }

	void FixedUpdate () {
        transform.Translate(moveDir * Time.deltaTime * speed);
	}

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy") {
            col.gameObject.SendMessage("TakeDamage", damage);
        }
        Destroy(gameObject);
    }

    public void setMoveDir(Vector2 moveDir) {
        this.moveDir = moveDir;
    }

    public void setDamage(int damage) {
        this.damage = damage;
    }
}
