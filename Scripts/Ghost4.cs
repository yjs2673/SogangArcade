using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost4 : MonoBehaviour
{
    [SerializeField] public Sprite leftSprite;  // 왼쪽 스프라이트
    [SerializeField] public Sprite rightSprite; // 오른쪽 스프라이트
    private SpriteRenderer spriteRenderer;

    [Header("Movement Settings")]
    [SerializeField] public float widthLength = 3f;  // 직사각형 가로 변의 길이
    [SerializeField] public float heightLength = 2f; // 직사각형 세로 변의 길이
    [SerializeField] public float moveSpeed = 2f;    // 이동 속도

    private Vector2[] waypoints; // 직사각형의 꼭짓점 좌표
    private int currentWaypointIndex = 0; // 현재 목표 지점 인덱스
    private Vector2 startPosition; // 시작 위치

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position; // 초기 위치 저장

        waypoints = new Vector2[] // 정사각형의 꼭짓점 좌표 설정
        {
            startPosition, // 첫 번째 꼭짓점
            startPosition - Vector2.right * widthLength, // 왼쪽
            startPosition - Vector2.right * widthLength - Vector2.up * heightLength, // 왼쪽 아래
            startPosition - Vector2.up * heightLength, // 아래
        };

        StartCoroutine(MovePattern()); // 코루틴 시작
    }

    void Update() {

    }

    private IEnumerator MovePattern() {
        while(true) {
            Vector2 target = waypoints[currentWaypointIndex];
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            // 목표 지점까지 이동
            while ((Vector2)transform.position != target) {
                float currx = transform.position.x;
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                if(currx < transform.position.x) spriteRenderer.sprite = rightSprite;
                else spriteRenderer.sprite = leftSprite;

                // z값 y값과 동일하게 
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
 
                yield return null; // 다음 프레임까지 대기
            }  

            // 다음 지점으로 업데이트
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
