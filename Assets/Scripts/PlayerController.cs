using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static bool vivo;
    public static float speed;

    Camera camara;
    Vector3 posInicial;
    
    [SerializeField]
    Transform goal;   
    [SerializeField]
    GameObject replayButton;

    void Awake()
    {
        camara = Camera.main;
        posInicial = transform.position;
        GameManger.playerTransform = transform;
        GameManger.posInicial = transform.position.x;
    }

    void Update()
    {
        if (vivo)
        {
            transform.position = Vector3.MoveTowards(transform.position, goal.position, speed*Time.deltaTime);           
        }
        camara.transform.position = new Vector3(transform.position.x, camara.transform.position.y, camara.transform.position.z);
    }
	
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            EndGame(true);
        }
        else if (col.gameObject.CompareTag("Finish"))
        {
            EndGame(false);            
        }
    }
    
    void EndGame(bool death)
    {
        if (vivo)
        {
            vivo = false;
            GameManger.manager.CheckScore(death);
            if (death)
            {
                GameManger.manager.MostrarDatos();
                replayButton.SetActive(true);
                GameManger.noob = true;
            }
            else
            {
                Restart();               
                GameManger.manager.LevelUp();
                GameManger.ready = true;
                GameManger.manager.LoadLevel(Saves.datos.maxLevel);                
            }
        }
    }

    public void Restart()
    {
        transform.position = posInicial;        
        replayButton.SetActive(false);
    }    
}
