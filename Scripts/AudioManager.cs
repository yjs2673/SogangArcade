using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [System.Serializable]
    public class SceneGroupMusic {
        public string groupName;        // {그룹 이름, 씬}
        public List<string> sceneNames; // 그룹에 속한 씬
        public AudioClip audioClip;     // 그룹에 사용할 브금
    }

    [SerializeField] private List<SceneGroupMusic> sceneGroupMusicList; // 그룹 정보 리스트
    private Dictionary<string, AudioClip> sceneMusicMap; // 씬 이름과 브금 맵핑

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        InitializeSceneMusicMap();
    }

    private void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 변경 시 이벤트 활성화
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트 비활성화
    }

    private void InitializeSceneMusicMap() {
        // 씬 이름과 브금을 맵핑
        sceneMusicMap = new Dictionary<string, AudioClip>();
        foreach(var group in sceneGroupMusicList) {
            foreach(var sceneName in group.sceneNames) {
                if(!sceneMusicMap.ContainsKey(sceneName)) sceneMusicMap.Add(sceneName, group.audioClip);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        string sceneName = scene.name;

        if(sceneMusicMap.TryGetValue(sceneName, out AudioClip clip)) {
            if(audioSource.clip != clip) { // 다른 음악이면 변경
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
        else audioSource.Stop(); // 지정되지 않은 씬은 음악 정지
    }
}
