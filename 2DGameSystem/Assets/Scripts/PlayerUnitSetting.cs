using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUnitSetting : MonoBehaviour
{
    public static PlayerUnitSetting instance;
    public GameObject canvas;
    public GameObject loadMenu;
    public Image pauseBoard;
    public GameObject saveBoard;
    public  int sceneIndex = 0;//场景指针，每涉及切换场景的函数都需要修改该值
    public GameObject player;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(gameObject);
        
    }
    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    private void Update()
    {

    }
    public void GameImport(SaveData data)//Level,Collection and Buff...
    {
        PlayerControl playerControl = player.GetComponent<PlayerControl>();
        /**/
        player.SetActive(true);
        canvas.SetActive(true);
        player.GetComponent<PlayerControl>().isInvincible = false;
        player.GetComponent<PlayerControl>().isHitKeep = false;
        sceneIndex = data.sceneIndex;
        /**/
        playerControl.maxHP = data.maxHP;
        playerControl.maxMP = data.maxMP;
        playerControl.HP = data.HP;
        playerControl.MP = data.MP;
        playerControl.ATK = data.ATK;
        playerControl.gameProgress = data.gameProgress;
        SceneManager.LoadScene(sceneIndex);
        TransformChanged(new Vector2(data.positionX, data.positionY));
        Time.timeScale = 1;
    }
    public SaveData GameExport()
    {
        SaveData data = new SaveData();
        PlayerControl playerControl = player.GetComponent<PlayerControl>();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        data.sceneIndex = sceneIndex;
        data.maxHP = playerControl.maxHP;
        data.maxMP = playerControl.maxMP;
        data.HP = playerControl.HP;
        data.MP = playerControl.MP;
        data.ATK = playerControl.ATK;
        data.gameProgress = playerControl.gameProgress;
        data.positionX = player.transform.position.x;
        data.positionY = player.transform.position.y;
        return data;
    }
    public void TransformChanged(Vector2 position)
    {
        foreach (Transform child in instance.gameObject.transform)
        {
            if (child.name == "Q" | child.name == "Main Camera")
                child.gameObject.transform.position = position;
        }
    }
}
