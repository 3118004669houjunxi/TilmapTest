using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSave : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isSaving = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Z) & collision.tag == "Player")
        {
            if (!isSaving)
            {
                Time.timeScale = 0;
                PlayerUnitSetting.instance.saveBoard.SetActive(true);
                isSaving = true;
            }
        }
    }
}
