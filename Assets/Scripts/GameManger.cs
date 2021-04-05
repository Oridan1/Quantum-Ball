using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour {

    public static GameManger manager;

    List<GameObject> lista1 = new List<GameObject>();
    List<GameObject> lista2 = new List<GameObject>();
    Transform goal;
    AudioSource[] audios;    
    int anterior;
    int repetidas;
    int cantidadEnemigos;
    int maxRnd;
    int musicMultiplier;
    float currentScore;
    float scoreMultiplier;
    float diferencia;    
    float goalPosInicial;
    float distanciaCubierta;
    float currentIntervalo;
    float deadSpace;

    [SerializeField]
    GameObject pool1;
    [SerializeField]
    GameObject pool2;
    [SerializeField]
    int maxRepetidas;
    [SerializeField]
    float intervaloTexto;
    [SerializeField]
    float defaultDeadSpace;
    [SerializeField]
    GameObject plano1;
    [SerializeField]
    GameObject plano2;
    [SerializeField]
    GameObject plano3;
    [SerializeField]
    GameObject plano4;
    [SerializeField]
    Animator textoAnim;
    [SerializeField]
    Sprite musicOn;
    [SerializeField]
    Sprite musicOff;

    [SerializeField]
    Text textoPuntos;
    [SerializeField]
    Text textoBestScore;
    [SerializeField]
    Text textoLvl;
    [SerializeField]
    Text textoNextLvl;
    [SerializeField]
    Image progressBar;
    [SerializeField]
    Image botonMusica;
    //[SerializeField]
    //Slider volumeSlider;

    public static bool freeze;
    public static bool ready;
    public static bool noob;
    public static Transform playerTransform;
    public static float posInicial;

    void Awake()
    {
        manager = this;
        CrearLista(pool1, lista1);
        CrearLista(pool2, lista2);
        currentIntervalo = intervaloTexto;
        audios = GetComponents<AudioSource>();        
        deadSpace = defaultDeadSpace;
        //foreach (GameObject n in lista1)
        //{
        //    Debug.Log(n.name);
        //}         
    }  

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Tap();           
        }
        if (PlayerController.vivo && !freeze)
        {
            currentScore += scoreMultiplier * Time.deltaTime;
            AnimarTexto();
        }        
        distanciaCubierta = Mathf.Abs(playerTransform.position.x - posInicial);
        progressBar.fillAmount = distanciaCubierta / diferencia;
        //AudioListener.volume = volumeSlider.value;
    }

    void Tap()
    {
        if (Input.mousePosition.y >= Screen.height * deadSpace)
        {
            if (PlayerController.vivo)
            {
                freeze = !freeze;
                textoPuntos.text = currentScore.ToString("F0");
                AudioSwitch();
            }
            else if (ready)
            {
                PlayerController.vivo = true;
                ready = false;
            }
            deadSpace = 0;
        }        
    }

    void AudioSwitch()
    {
        if (freeze)
        {
            audios[2].Play();
            audios[0].volume = 0;
            audios[1].volume = 1*musicMultiplier;
        }
        else
        {
            audios[3].Play();
            audios[0].volume = 1*musicMultiplier;
            audios[1].volume = 0;
        }
        //audios[2].Play();
    }

    void AnimarTexto()
    {
        currentIntervalo -= Time.deltaTime;
        if (currentIntervalo <= 0)
        {
            textoAnim.SetTrigger("Trig");
            textoPuntos.text = currentScore.ToString("F0");
            currentIntervalo = intervaloTexto;            
        }
    }

    void CrearLista(GameObject pool, List<GameObject> lista)
    {
        Transform[] aux = pool.GetComponentsInChildren<Transform>();
        for (int i = 0; i < aux.Length; i++)
        {
            if (aux[i].gameObject.name != pool.name)
            {
                lista.Add(aux[i].gameObject);
            }            
        }
    }

    void Randomizar(GameObject obj, float posX)
    {
        //float rnd = Random.Range(25f, 66f) / 100f;
        obj.SetActive(true);           
        EnemyController enemyScript = obj.GetComponent<EnemyController>();
        enemyScript.SetReverse(false);
        enemyScript.SetVanishing(false);
        enemyScript.Unvanish();
        enemyScript.SetVanishSpeed(PlayerController.speed / 4f);
        int rnd = Random.Range(0, maxRnd);
        float aux = 0f;
        if (repetidas >= maxRepetidas)
        {
            while (rnd == anterior)
            {
                rnd = Random.Range(0, maxRnd);
            }
            repetidas = 0;
        }
        int rndAux = Random.Range(0, 2);
        switch (rnd)
        {
            case 0:
                aux = 0.26f;
                break;
            case 1:
                aux = 0.65f;
                break;
            case 2:                
                if (rndAux == 0)
                {
                    aux = 0.26f;
                }
                else
                {
                    aux = 0.65f;
                }
                enemyScript.SetReverse(true);
                break;
            case 3:
                enemyScript.SetVanishing(true);
                if (rndAux == 0)
                {
                    aux = 0.26f;
                }
                else
                {
                    aux = 0.65f;
                }
                break;
        }
        if (rnd == anterior)
        {
            repetidas++;
        }
        anterior = rnd;
        obj.transform.position = new Vector3(posX, aux, obj.transform.position.z);
    }

    void MostrarPlano(GameObject plano)
    {
        plano1.SetActive(false);
        plano2.SetActive(false);
        plano3.SetActive(false);
        plano4.SetActive(false);
        plano.SetActive(true);
        goal = plano.GetComponentsInChildren<Transform>()[1];
    }

    void SetDifficulty(int level)
    {
        if (level < 6)
        {
            Dificultad(1.15f, 50, 4, 2);
        }
        else if (level < 9)
        {            
            Dificultad(1.25f, 75, 6, 2);
        }
        else if (level < 11)
        {           
            Dificultad(1.25f, 75, 6, 3);
        }
        else if (level < 16)
        {
            Dificultad(1.5f, 100, 9, 3);
        }
        else if (level < 21)
        {           
            Dificultad(1.75f, 100, 9, 4);
        }
        else if (level < 26)
        {           
            Dificultad(2f, 125, 14, 4);
        }
        else if (level < 31)
        {
            Dificultad(2.5f, 150, 14, 4);
        }
        else if (level > 30)
        {
            Dificultad(3, 200, 14, 4);
        }
    }

    void Dificultad(float sp, float mult, int enemigos, int rng)
    {
        if (noob)
        {
            sp *= 0.75f;
        }
        PlayerController.speed = sp;
        scoreMultiplier = (mult + Saves.datos.maxLevel*3)*sp/3;        
        cantidadEnemigos = enemigos;
        maxRnd = rng;
        switch (enemigos)
        {
            case 4:
                MostrarPlano(plano1);
                break;
            case 6:
                MostrarPlano(plano2);
                break;
            case 9:
                MostrarPlano(plano3);
                break;
            case 14:
                MostrarPlano(plano4);
                break;
        }
    }

    public void LoadLevel(int level)
    {
        SetDifficulty(level);       
        goalPosInicial = goal.position.x;
        diferencia = Mathf.Abs(posInicial - goalPosInicial);
        for (int i = 0; i < lista1.Count; i++)
        {
            lista1[i].SetActive(false);
        }
        float aux = 1.5f;
        for (int i = 0; i < cantidadEnemigos; i++)
        {
            Randomizar(lista1[i], aux);
            float rnd = Random.Range(15f, 17.5f) / 10;
            aux -= rnd;
        }
        if (freeze)
        {
            AudioSwitch();
        }
        freeze = false;
        ready = true;
        textoLvl.text = level.ToString();
        textoNextLvl.text = (level+1).ToString();
        textoPuntos.text = currentScore.ToString("F0");
        deadSpace = defaultDeadSpace;
        audios[0].volume = 1 * musicMultiplier;
        audios[1].volume = 0;
    }

    public void CheckScore(bool death)
    {
        if (currentScore > Saves.datos.bestScore)
        {
            Saves.datos.bestScore = currentScore;            
        }
        if (death)
        {
            textoPuntos.text = currentScore.ToString("F0");
            currentScore = 0;
        }
        Saves.datos.currentScore = currentScore;
        Saves.instance.Save();
    }

    public void SetCurrentScore(float nuevo)
    {
        currentScore = nuevo;
    }

    public void MostrarDatos()
    {
        textoBestScore.text = "Best: " + Saves.datos.bestScore.ToString("F0");        
    }

    //public void ToggleSlider()
    //{
    //    if (volumeSlider.IsActive())
    //    {
    //        volumeSlider.gameObject.SetActive(false);
    //        Saves.datos.volumen = volumeSlider.value;
    //        Saves.instance.Save();
    //    }
    //    else
    //    {
    //        volumeSlider.gameObject.SetActive(true);
    //    }
    //}

    public void ToggleMusic()
    {
        if (musicMultiplier == 0)
        {
            botonMusica.sprite = musicOn;
            musicMultiplier = 1;
        }
        else
        {
            botonMusica.sprite = musicOff;
            musicMultiplier = 0;
        }
        if (!freeze)
        {
            audios[0].volume = 1 * musicMultiplier;
        }
        else
        {
            audios[1].volume = 1 * musicMultiplier;
        }        
        Saves.datos.volumen = musicMultiplier;
        Saves.instance.Save();
    }
    
    public void SetVolume(int nuevo)
    {
        //volumeSlider.value = nuevo;
        musicMultiplier = nuevo;
        if (nuevo == 0)
        {
            botonMusica.sprite = musicOff;
        }
        else
        {
            botonMusica.sprite = musicOn;
            audios[0].volume = 1;
        }
    }

    public void Replay()
    {
        //SceneManager.LoadScene(0);
        LoadLevel(Saves.datos.maxLevel);
        ready = true;
    }

    public void LevelUp()
    {
        Saves.datos.maxLevel++;
        Saves.instance.Save();
        noob = false;
    }
}