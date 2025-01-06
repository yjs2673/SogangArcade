using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_corner_dr : MonoBehaviour
{
    [SerializeField] public Sprite upSprite; // 위쪽 스프라이트
    [SerializeField] public Sprite downSprite; // 아래쪽 스프라이트
    [SerializeField] public Sprite leftSprite; // 왼쪽 스프라이트
    [SerializeField] public Sprite rightSprite; // 오른쪽 스프라이트
    private SpriteRenderer spriteRenderer;

    private bool isKilled = false;
    private bool movable = true;

    [Header("Movement Settings")]
    [SerializeField] public float sideLength = 7f; // 정사각형 한 변의 길이
    [SerializeField] public float moveSpeed = 2f; // 이동 속도

    private Vector2[] waypoints; // 정사각형의 꼭짓점 좌표
    private int currentWaypointIndex = 0; // 현재 목표 지점 인덱스
    private Vector2 startPosition; // 시작 위치

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position; // 초기 위치 저장

        waypoints = new Vector2[] // 정사각형의 꼭짓점 좌표 설정
        {
            startPosition, // 첫 번째 꼭짓점
            startPosition - Vector2.up * sideLength, // 아래
            startPosition + (Vector2.right - Vector2.up) * sideLength, // 오른쪽 아래
            startPosition - Vector2.up * sideLength, // 아래
        };

        StartCoroutine(MovePattern()); // 코루틴 시작
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

    private IEnumerator MovePattern() {
        while(movable) {
            Vector2 target = waypoints[currentWaypointIndex];
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            // 목표 지점까지 이동
            while ((Vector2)transform.position != target && movable) {
                float currx = transform.position.x;
                float curry = transform.position.y;
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

                if(currx != transform.position.x) {
                    if(currx < transform.position.x) spriteRenderer.sprite = rightSprite;
                    else spriteRenderer.sprite = leftSprite;
                }
                else {
                    if(curry < transform.position.y) spriteRenderer.sprite = upSprite;
                    else spriteRenderer.sprite = downSprite;
                }

                // z값 y값과 동일하게 
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
 
                yield return null; // 다음 프레임까지 대기
            } 

            // 다음 지점으로 업데이트
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
