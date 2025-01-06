using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_stop : MonoBehaviour
{
    private bool isKilled = false;

    void Start() {
        
    }

    void Update() {
        if(isKilled) { // 플레이어랑 똑같이 사라짐
            if(transform.position.y <= -10f) {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(isKilled) return;
        string tag = collision.gameObject.tag;
        if(tag == "Bomb") Kill();
        if(tag == "Stream") Kill();
    }

    private void Kill() {
        GameManager.instance.DecreaseMonster();

        isKilled = true; 
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.gray;

        Collider2D collider = GetComponent<Collider2D>();
        if(collider != null) collider.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1f;
    }
}