using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SenceChange : MonoBehaviour
{
    public int toScence = 0;
    public Vector2 position;
    void Start()
    {
        
    }
    void Update()
    {
       
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Z) & collision.tag == "Player")
        {
            SceneManager.LoadScene(toScence);
            PlayerUnitSetting.instance.sceneIndex = toScence;            
            PlayerUnitSetting.instance.TransformChanged(position);
        }     
    }
}
