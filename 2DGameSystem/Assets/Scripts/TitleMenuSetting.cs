using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenuSetting : MonoBehaviour
{
    public GameObject loadPanel;
    public GameObject settingPanel;
    public GameObject selectPanel;
    public void OnClickNewGameButton()
    {

    }
    public void OnClickSettingButton()
    {
        selectPanel.SetActive(false);
        settingPanel.SetActive(true);
    }
    public void OnClickLoadButton()
    {
        selectPanel.SetActive(false);
        loadPanel.SetActive(true);
    }
    public void OnClickQuitButton()
    {
        Application.Quit();
    }
    public void OnClickCencelButton()
    {
        settingPanel.SetActive(false);
        loadPanel.SetActive(false);
        selectPanel.SetActive(true);
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
    }
}
