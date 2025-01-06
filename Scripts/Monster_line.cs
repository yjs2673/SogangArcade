using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_line : MonoBehaviour
{
    [SerializeField] public Sprite upSprite; // 위쪽 스프라이트
    [SerializeField] public Sprite downSprite; // 아래쪽 스프라이트
    [SerializeField] public Sprite leftSprite; // 왼쪽 스프라이트
    [SerializeField] public Sprite rightSprite; // 오른쪽 스프라이트
    private SpriteRenderer spriteRenderer;

    private bool isKilled = false;
    private bool movable = true;

    [Header("Movement Settings")]
    [SerializeField] public bool moveHorizontally = true; // true면 좌우 이동, false면 상하 이동
    [SerializeField] public bool movingPositive = true; // 현재 이동 방향 (양수 방향으로 이동 중)
    [SerializeField] public float moveDistance = 4f; // 이동할 거리
    [SerializeField] public float moveSpeed = 1f; // 이동 속도

    private Vector3 startPosition; // 초기 위치

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position; // 초기 위치 저장
    }

    void Update() {
        if(isKilled) { // 플레이어랑 똑같이 사라짐
            if(transform.position.y <= -10f) {
                Destroy(gameObject);
            }
        }

        if(movable) MovePattern();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(isKilled) return;
        string tag = collision.gameObject.tag;
        if(tag == "Bomb") Kill();
        if(tag == "Stream") Kill();
    }

    private void Kill() {
        movable = false;
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

    private void MovePattern() {
        float displacement = Mathf.PingPong(Time.time * moveSpeed, moveDistance); // 0에서 moveDistance까지 반복
        if (moveHorizontally) { // 좌우 이동
            float currx = transform.position.x;
            transform.position = startPosition + new Vector3(movingPositive ? displacement : -displacement, 0, 0);
            if(currx < transform.position.x) spriteRenderer.sprite = rightSprite;
            else spriteRenderer.sprite = leftSprite;
        }
        else { // 상하 이동
            float curry = transform.position.y;
            transform.position = startPosition + new Vector3(0, movingPositive ? displacement : -displacement, 0);
            if(curry < transform.position.y) spriteRenderer.sprite = upSprite;
            else spriteRenderer.sprite = downSprite;
        }

        // z값 y값과 동일하게 
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
}