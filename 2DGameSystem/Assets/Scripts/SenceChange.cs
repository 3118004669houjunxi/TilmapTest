using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SenceChange : MonoBehaviour
{
    public int toScence = 0;
    public Vector2 position;
    bool isPlayerHere;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) & isPlayerHere)
        {
            SceneManager.LoadScene(toScence);
            PlayerUnitSetting.instance.sceneIndex = toScence;            
            PlayerUnitSetting.instance.TransformChanged(position);
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
