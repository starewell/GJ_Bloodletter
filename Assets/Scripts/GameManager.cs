using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake() {
        if (GameManager.instance && GameManager.instance != this) {
            Destroy(this.gameObject);
            return;
        }
        GameManager.instance = this;
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    Bloodletter bloodletter;
    PauseManager pauseManager;
    public Door exit;
    public List<WellSite> wells;
    public int poisonedWellCount;
    public float exitProgress { get { return poisonedWellCount / wells.Count; }}


    public enum GameState { Menu, Running, Paused, Gameover };
    public GameState gameState;

    [SerializeField] bool cursorLock;
    [SerializeField] string sceneName;


    void Start() {
        sceneName = SceneManager.GetActiveScene().name;
    }

    public void ChangeState(GameState state) {
        gameState = state;
        if (state == GameState.Running) {
            LockCursor(true);

        } else {
            LockCursor(false);
        }
    }

    void Update() {
    	if (gameState == GameState.Running) {
// lock when mouse is clicked
           if(!cursorLock)
                LockCursor(!cursorLock);        
        } else {
            if(cursorLock)
                LockCursor(!cursorLock);        
        }
    }
    
    public void LockCursor(bool state) {
    	if (state) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        cursorLock = state;
    }

    public void KillPlayer() {
        DebugUI.instance.EnableGameover();
        gameState = GameState.Gameover;
        LockCursor(false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        bloodletter = Bloodletter.instance;
        if (bloodletter) {
            var doors = FindObjectsByType(typeof(Door), FindObjectsSortMode.None);
            foreach(Door door in doors) {
                if (door.doorType == Door.DoorType.Well) exit = door;
            }
            var _wells = FindObjectsByType(typeof(WellSite), FindObjectsSortMode.None);
            foreach(WellSite well in _wells) {
                wells.Add(well);
                well.ExhaustSiteCallback += PoisonWell;
            }
        }
        pauseManager = PauseManager.instance;
    }

    public void PoisonWell() {
        Debug.Log("Well poisoned");
        poisonedWellCount++;
        if (poisonedWellCount >= wells.Count) {
            exit.ExhaustSite();
        }
    }

    public void RestartScene() {
        SceneManager.LoadScene(sceneName);
    }

    public void Pause(bool state) {
        pauseManager.Pause(state);
        gameState = state ? GameState.Paused : GameState.Running;
    }

}
