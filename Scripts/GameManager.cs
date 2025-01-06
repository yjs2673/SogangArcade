using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    // 텍스트 패널 업데이트를 위한 변수, TMPro 헤더라인에 쓰기
    [SerializeField] private TextMeshProUGUI Count_text;
    [SerializeField] private TextMeshProUGUI Score_text;
    [SerializeField] private TextMeshProUGUI Escape_text;
    [SerializeField] private TextMeshProUGUI Monster_text;

    [SerializeField] private GameObject EscapePanel;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject StageClearPanel;
    [SerializeField] private GameObject LevelClearPanel;
    [SerializeField] private GameObject StartPanel; // 시작 화면의 버튼들 다루기

    [SerializeField] private Sprite[] rankSprites;
    [SerializeField] private Image rankImage_over;
    [SerializeField] private Image rankImage_clear;
 
    [SerializeField] public int count_bomb; // 사용 가능한 물풍선 개수
    [SerializeField] public static int score = 0; // 코인으로 얻은 점수
    [SerializeField] public int count_monster; // 맵에 주어진 몬스터 수
    [SerializeField] public int stage; // 현재 스테이지
    private int count_down = 10; // 물풍선에 갇혔을 때 남은 연타 수

    void Awake() { // start보다 빨리 호출되는 매소드
        if(instance == null) instance = this;
    }
    
    void Start() {
        if(stage == 2 || stage == 3)Score_text.SetText(score.ToString());
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
	        Application.Quit(); //게임 앱 종료
        }
    }

    public void IncreaseCount(int n) {
        count_bomb += n;
        Count_text.SetText(count_bomb.ToString());
    }

    public void DecreaseCount(int n) {
        count_bomb -= n;
        Count_text.SetText(count_bomb.ToString());
    }

    public void IncreaseScore() {
        score += 10;
        Score_text.SetText(score.ToString());
    }

    public void DecreaseMonster() { // 몬스터 처치시
        count_monster--;
        Monster_text.SetText(count_monster.ToString());
        if(count_monster == 0) {
            Player player = FindObjectOfType<Player>(); // Player 객체 찾기
            player.movable = false; // 플레이어 이동 불가
            player.clear = true;

            if(stage == 3) ShowLevelClearPanel();
            else ShowStageClearPanel();
        }
    }

    public void ShowEscapePanel() { 
        EscapePanel.SetActive(true); 
    }

    public void CountDown() {
        count_down--;
        Escape_text.SetText(count_down.ToString());
    }
    
    public void CloseEscapePanel() { 
        count_down = 10;
        Escape_text.SetText(count_down.ToString());
        EscapePanel.SetActive(false); 
    }

    public void ShowGmaeOverPanel() {
        int rankIdx;
        if(180 <= score) rankIdx = 0; // 180 이상 A
        else if(150 <= score) rankIdx = 1; // 170 160 150 B
        else if(110 <= score) rankIdx = 2; // 140 130 120 110 C
        else if(70 <= score) rankIdx = 3; // 100 90 80 70 D
        else if(30 <= score) rankIdx = 4; // 60 50 40 30 E
        else rankIdx = 5; // 나머지 다 F

        score = 0;
        GameOverPanel.SetActive(true);
        rankImage_over.sprite = rankSprites[rankIdx];
    }

    public void ShowStageClearPanel() {
        StageClearPanel.SetActive(true);
    }

    public void ShowLevelClearPanel() {
        int rankIdx;
        score += 50;
        if(230 <= score) rankIdx = 6;      // 보스 단계까지 모두 클리어 해야 S
        else if(180 <= score) rankIdx = 0; // 220 210 200 190 180 A
        else if(150 <= score) rankIdx = 1; // 170 160 150 B
        else if(120 <= score) rankIdx = 2; // 140 130 120 C
        else if(80 <= score) rankIdx = 3;  // 100 90 80 D
        else if(40 <= score) rankIdx = 4;  // 70 60 50 40 30 E
        else rankIdx = 5;                  // 나머지 다 F

        score = 0;
        LevelClearPanel.SetActive(true);
        rankImage_clear.sprite = rankSprites[rankIdx];
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    // 버튼 함수

    public void gotoTitle() { // 시작 화면
        GameOverPanel.SetActive(false);
        SceneManager.LoadScene("Start_Scene");
    }

    public void goto1_1() {
        SceneManager.LoadScene("1_1");
    }

    public void goto1_2() {
        SceneManager.LoadScene("1_2");
    }

    public void goto1_3() {
        SceneManager.LoadScene("1_3");
    }

    public void goto2_1() {
        SceneManager.LoadScene("2_1");
    }

    public void goto2_2() {
        SceneManager.LoadScene("2_2");
    }

    public void goto2_3() {
        SceneManager.LoadScene("2_3");
    }

    public void goto3_1() {
        SceneManager.LoadScene("3_1");
    }

    public void goto3_2() {
        SceneManager.LoadScene("3_2");
    }

    public void goto3_3() {
        SceneManager.LoadScene("3_3");
    }

    public void gotoExit() { // 게임 종료
        Application.Quit();
    }
}