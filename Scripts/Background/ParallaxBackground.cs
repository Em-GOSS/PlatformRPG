using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{    
    [SerializeField] private float parallaxEffect;
    private GameObject mainCameraObj;
    private float xPosition;
    private float length;

    private void Start() 
    {
        mainCameraObj=Camera.main.gameObject;

        length=GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition=transform.position.x;
    }

    private void Update() 
    {
        float distanceMoved=mainCameraObj.transform.position.x*(1-parallaxEffect);
        float disatanceToMove=mainCameraObj.transform.position.x*parallaxEffect;

        transform.position=new Vector3(xPosition+disatanceToMove,transform.position.y);
        
        if(distanceMoved>xPosition+length)
        {
            xPosition=xPosition+length;
        }
        else if( distanceMoved<xPosition-length)
        {
            xPosition=xPosition-length;
        }
    }

}
