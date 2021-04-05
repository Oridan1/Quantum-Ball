using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    bool reverse;
    bool goingDown;    
    bool vanishing;    
    bool isVanishing;
    MeshRenderer render;
    Color startColor;

    [SerializeField]
    float speed;
    [SerializeField]
    float maxY;
    [SerializeField]
    float minY;
    [SerializeField]
    float vanishSpeed;
    [SerializeField]
    bool rotator;
	
    void Awake()
    {
        render = GetComponent<MeshRenderer>();
        startColor = render.material.color;
    }

	// Update is called once per frame
	void Update () {
        //if (Input.GetMouseButtonDown(0) && PlayerController.vivo)
        //{
        //    freeze = !freeze;
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    freeze = false;
        //}        
        if (!reverse)
        {
            if (!GameManger.freeze)
            {
                Move();
            }
        }
        else
        {
            if (GameManger.freeze)
            {
                Move();
            }
        }
        if (isVanishing && PlayerController.vivo)
        {
            Vanish();
        }
	}

    void Move()
    {
        float currentSpeed = speed * Time.deltaTime;
        if (!rotator)
        {            
            if (goingDown)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - currentSpeed, transform.position.z);
                if (transform.position.y <= minY)
                {
                    goingDown = false;
                }
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + currentSpeed, transform.position.z);
                if (transform.position.y >= maxY)
                {
                    goingDown = true;
                }
            }
        }
        else
        {
            transform.Rotate(Vector3.right * currentSpeed);
        }
    }

    void Vanish()
    {
        Color aux = render.material.color;
        aux.a -= (vanishSpeed * Time.deltaTime);
        render.material.color = aux;
    }
   
    void OnBecameVisible()
    {
        if (vanishing)
        {
            isVanishing = true;
        }
    }

    public void Unvanish()
    {
        if (render == null)
        {
            render = GetComponent<MeshRenderer>();
            startColor = render.material.color;
        }
        render.material.color = startColor;
        isVanishing = false;
    }

    public void SetSpeed(float num)
    {
        speed = num;
    }

    public void SetReverse(bool nuevo)
    {
        reverse = nuevo;
        if (render == null)
        {
            render = GetComponent<MeshRenderer>();
            startColor = render.material.color;
        }
        render.material.color = Color.red;
    }

    public void SetVanishing(bool nuevo)
    {
        vanishing = nuevo;        
    }

    public void SetVanishSpeed(float nuevo)
    {
        vanishSpeed = nuevo;
    }
}

//public class TimeNode
//{
//    public Vector3 position;
//    public Quaternion rotation;
    
//    public TimeNode(Vector3 _position, Quaternion _rotation)
//    {
//        position = _position;
//        rotation = _rotation;
//    }
//}
