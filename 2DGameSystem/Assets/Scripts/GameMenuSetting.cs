using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuSetting : MonoBehaviour
{
    [Header("UI控件")]
    public GameObject loadMenu;
    public Image pauseBoard;
    public GameObject saveBoard;
    [Header("私有参数")]
    bool isPause = false;
    float x = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BoardMove();
        SaveBoardCheck();
    }
    public void OnClickQuitBotton()
    {
        Application.Quit();
    }
    public void OnClickPauseBotton()
    {
        if (!GameSave.isSaving)
        {
            x = 0;
            if (isPause)
            {
                Time.timeScale = 1;
                loadMenu.SetActive(false);
                isPause = false;
            }
            else
            {
                Time.timeScale = 0;
                isPause = true;
            }
        }
    }
    public void OnClickSButton()
    {
        int num = 0;
        SaveAndLoad S = new SaveAndLoad();
        GameObject Button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        switch (Button.transform.name)
        {
            case "S0Button":
                num = 0;
                break;
            case "S1Button":
                num = 1;
                break;
            case "S2Button":
                num = 2;
                break;
            case "S3Button":
                num = 3;
                break;
        }
        S.Save(num);
    }
    public void OnClickLButton()
    {
        int num = 0;
        GameObject Button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        switch (Button.transform.name)
        {
            case "L0Button":
                num = 0;
                break;
            case "L1Button":
                num = 1;
                break;
            case "L2Button":
                num = 2;
                break;
            case "L3Button":
                num = 3;
                break;
        }
        SaveAndLoad S = new SaveAndLoad();
        S.Load(num);
        isPause = false;
        loadMenu.SetActive(false);
    }
    public void OnClickLoadMenuBotton()
    {
        if (!loadMenu.active)
        {
            loadMenu.SetActive(true);
        }
        else
        {
            loadMenu.SetActive(false);
        }
    }
    void SaveBoardCheck()
    {
        if (Input.GetKeyDown(KeyCode.X) & GameSave.isSaving)
        {
            Time.timeScale = 1;
            saveBoard.SetActive(false);
            GameSave.isSaving = false;
        }
    }
    void BoardMove()
    {
        if (isPause)
        {
            pauseBoard.rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(pauseBoard.rectTransform.anchoredPosition.x, 161.98f, x * 0.05f), pauseBoard.rectTransform.anchoredPosition.y);
        }
        else
        {
            pauseBoard.rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(pauseBoard.rectTransform.anchoredPosition.x, 563f, x * 0.05f), pauseBoard.rectTransform.anchoredPosition.y);
        }
        x++;
    }
}