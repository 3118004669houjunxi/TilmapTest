using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSave : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isSaving = false;
    bool isPlayerHere;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) & isPlayerHere)
        {
            if (!isSaving)
            {
                Time.timeScale = 0;
                PlayerUnitSetting.instance.saveBoard.SetActive(true);
                isSaving = true;
            }
        }  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerHere = true;
            collision.GetComponent<PlayerControl>().canAttack = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerHere = false;
            collision.GetComponent<PlayerControl>().canAttack = true;
        }
    }
}
