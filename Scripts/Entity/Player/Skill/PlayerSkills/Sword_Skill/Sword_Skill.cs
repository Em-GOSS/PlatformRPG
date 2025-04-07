using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sword_Type
{
    Regular,
    Bounce,
    Pierce,
    Spin

}

public class Sword_Skill : Skill
{       
    [Header("Sword Info")]
    [SerializeField] private Sword_Type sword_Type;
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 sword_LaunchForce;  
    [SerializeField] private float normal_Gravity;

    [Header("Pierce Sword Info")]
    [SerializeField] private float pierce_Gravity;

    private Vector2 finalSpeed;

    [Header("Aim Dots")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private int numberOfDot;
    [SerializeField] private float spaceBetweenDot;
    [SerializeField] private float shrinkOfDots;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;


    //Properties
    private float sword_Gravity
    {
        get{
            if(sword_Type == Sword_Type.Pierce)
                return pierce_Gravity;
            else
                return normal_Gravity;
        }
    }

    protected override void Start()
    {
        GenerateDots();
        base.Start();
    }


    protected override void Update()
    {   
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalSpeed = GetAimDirection().normalized * sword_LaunchForce;
            SetDots(false);
        }

        if(Input.GetKey(KeyCode.Mouse1))
        {
            SortDots();
        }
    }

    public void CreateSword()
    {      
        GameObject instantiate_Sword = Instantiate(swordPrefab,player.transform.position,swordPrefab.transform.rotation);
        Sword_Skill_Controller sword_Skill_Controller = instantiate_Sword.GetComponent<Sword_Skill_Controller>();
        if(sword_Skill_Controller != null)
        {
            player.AssignNewSword(instantiate_Sword);
            sword_Skill_Controller.SetUpSword(sword_Type, finalSpeed, sword_Gravity, player);
        }
    }
    

    private Vector2 GetAimDirection()   
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return mousePosition-playerPosition;
    }

    public void SetDots(bool isVisable)
    {
        foreach(GameObject dot in dots)
        {
            dot.SetActive(isVisable);
        }
    }

    private void GenerateDots() 
    {
        dots = new GameObject[numberOfDot];
        for(int i = 0; i < numberOfDot; i++)
        {
            dots[i] = Instantiate(dotPrefab,this.transform.position,Quaternion.identity,dotsParent);
            dots[i].transform.localScale = dotPrefab.transform.localScale - new Vector3(shrinkOfDots,shrinkOfDots,shrinkOfDots) * i;
            dots[i].SetActive(false);
        }
    }

    private Vector2 GetDotPos(float time)
    {
        Vector2  presetLaunchSpeed = GetAimDirection().normalized * sword_LaunchForce;

        return presetLaunchSpeed * time + (0.5f * time * time) * Physics2D.gravity * sword_Gravity;
    }

    private void SortDots()
    {
        for(int i = 0; i < numberOfDot; i++)
        {
            dots[i].transform.position =  GetDotPos(i * spaceBetweenDot) + (Vector2)(player.transform.position);
        }
    }
}
