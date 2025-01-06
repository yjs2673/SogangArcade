using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_2 : MonoBehaviour
{
    [Header("Boss Settings")]
    public int MaxHP = 15;
    public int CurrHP;

    [Header("Movement Settings")]
    public float moveDistance = 8f; // 이동할 거리
    public float moveSpeed = 0.5f; // 이동 속도
    public GameObject streamleftPrefab; // 물줄기 프리팹
    public GameObject streamrightPrefab;
    public GameObject streammidPrefab;
    public float streamSpawnInterval = 1f; // 물줄기 생성 간격 (세로)
    public float streamSpacing = 1f; // 물줄기 간격 (가로)
    public Slider HP_bar;

    private bool isKilled = false;

    private Vector3 startPosition; // 초기 위치

    private float moveTotal = 0f; // 총 이동 거리 누적
    private float lastY = 0f; // 마지막으로 물줄기를 생성한 y 위치

    void Start() {
        CurrHP = MaxHP;
    }

    void Update() {
        if(isKilled) { // 플레이어랑 똑같이 사라짐
            if(transform.position.y <= -10f) {
                Destroy(gameObject);
            }
        }

        MovePattern();
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

    private void MovePattern() {
        if(moveTotal >= moveDistance) return; // 이동 거리 다다르면 정지

        // 이동
        float moveDelta = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(0, -moveDelta, 0);
        moveTotal += moveDelta;

        // 물줄기 생성
        if(Mathf.Abs(transform.position.y - lastY) >= streamSpawnInterval) {
            SpawnStreams();
            lastY = transform.position.y;
        }
    }

    private void SpawnStreams() { // 물줄기 생성
        float startX = transform.position.x - 3 * streamSpacing / 2;
        Vector3 streamPosition = new Vector3(startX, transform.position.y, 0);
        Instantiate(streamleftPrefab, streamPosition, Quaternion.identity);
        streamPosition = new Vector3(startX + streamSpacing, transform.position.y, 0);
        Instantiate(streammidPrefab, streamPosition, Quaternion.identity);
        streamPosition = new Vector3(startX + 2 * streamSpacing, transform.position.y, 0);
        Instantiate(streammidPrefab, streamPosition, Quaternion.identity);
        streamPosition = new Vector3(startX + 3 * streamSpacing, transform.position.y, 0);
        Instantiate(streamrightPrefab, streamPosition, Quaternion.identity);
    }
}