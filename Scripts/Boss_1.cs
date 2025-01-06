using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_1 : MonoBehaviour
{
    [Header("Boss Settings")]
    public int MaxHP = 10;
    public int CurrHP;
    public Slider HP_bar;

    private bool isKilled = false;

    void Start() {
        CurrHP = MaxHP;
    }

    void Update() {
        if(isKilled) { // 플레이어랑 똑같이 사라짐
            if(transform.position.y <= -10f) {
                Destroy(gameObject);
            }
        }
    }

    /* private void OnCollisionEnter2D(Collision2D collision) {
        if(isKilled) return;
        string tag = collision.gameObject.tag;
        if(tag == "Bomb") Damage();
    } */

    private void OnTriggerEnter2D(Collider2D collision) {
        if(isKilled) return;
        string tag = collision.gameObject.tag;
        if(tag == "Bomb") Damage();
    }

    public void CheckHP() {
        if(HP_bar != null) HP_bar.value = CurrHP;
    }

    public void Damage() {
        CurrHP--;
        CheckHP();
        if(CurrHP == 0) Kill();
    }

    private void Kill() {
        GameManager.instance.DecreaseMonster();

        isKilled = true; 
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.gray;

        Collider2D collider = GetComponent<Collider2D>();
        if(collider != null) collider.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1f;
    }
}