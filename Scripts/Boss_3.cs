using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_3 : MonoBehaviour
{
    [Header("Boss Settings")]
    public int MaxHP = 20;
    public int CurrHP;
    public Slider HP_bar;

    public GameObject poisonPrefab;
    public float poisonSpeed = 5f;
    public float poisonTime = 3f;

    private bool isKilled = false;

    private Vector3 startPosition; // 초기 위치

    void Start() {
        CurrHP = MaxHP;
        StartCoroutine(ShootPattern());
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

    private IEnumerator ShootPattern() {
        while(!isKilled) {
            yield return new WaitForSeconds(5f); // 5초마다 실행
            Shoot();
        }
    }

    private void Shoot() {
        float angleStep = 30f;
        float startAngle = -150f;

        for(int i = 0; i < 5; i++) {
            float angle = startAngle + i * angleStep;
            float radian = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

            GameObject poison = Instantiate(poisonPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = poison.GetComponent<Rigidbody2D>();

            if(rb != null) rb.velocity = direction * poisonSpeed;

            StartCoroutine(poisonDestroy(poison)); // 독 삭제 코루틴
        }
    }

    private IEnumerator poisonDestroy(GameObject poison) {
        yield return new WaitForSeconds(poisonTime);
        Destroy(poison);
    }
}