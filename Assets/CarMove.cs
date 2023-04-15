using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    
    private const int Acceleration = 20;
    private Transform _target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        var speed = Acceleration * Time.deltaTime;
        Vector3 direction;
        
        //pressing the 'up' or 'down' arrows will move the car backwards or forwards
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = Vector3.forward;
            transform.Translate(direction * speed);  
        }  
         
        if (Input.GetKey(KeyCode.DownArrow))  
        {  
            direction = Vector3.back;
            transform.Translate(direction * speed);  
        }  
         
        //the left and right arrows can be ued to control the direction in conjunction with the backwards/forwards movement
        if (Input.GetKey(KeyCode.LeftArrow))  
        {  
            transform.Rotate(Vector3.up, -10);  
        }  
        
        if (Input.GetKey(KeyCode.RightArrow))  
        {  
            transform.Rotate(Vector3.up, 10);  
        }  
    }
}
