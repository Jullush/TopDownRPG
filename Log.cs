using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{
    
    public Rigidbody2D rb;
    public Transform targetTransform;
    public Animator logAnim;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    
    


    // Start is called before the first frame update
    private void Start()
    {
        currentState = EnemyState.idle; 
        logAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        logAnim.SetBool("Woke", false);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        CheckDistance();
    }
    public virtual void CheckDistance()
    {
        if (Vector3.Distance(targetTransform.position, transform.position) <= chaseRadius
            && Vector3.Distance(targetTransform.position, transform.position) > attackRadius)
        {
            logAnim.SetBool("Woke", true);

            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                logAnim.SetBool("isWalking", true);
               Vector3 tempTarget = Vector3.MoveTowards(transform.position,
                    targetTransform.position, moveSpeed * Time.fixedDeltaTime);
                
                ChangeAnim(tempTarget - transform.position);
                
                rb.MovePosition(tempTarget); //ruszam go nei transform.position, bo to jest ingerencja w fizykę ! 
                ChangeState(EnemyState.walk);
                
            }                                   // używam Rigidbody do poruszenia LOga 
        }
        else if (Vector3.Distance(targetTransform.position, transform.position) >= chaseRadius)
        {
            Vector3 tempHouse = Vector3.MoveTowards(transform.position,
                homePosition.position, moveSpeed*Time.deltaTime);
            
            ChangeAnim(tempHouse - transform.position);
            rb.MovePosition(tempHouse);
            
            if (transform.position == homePosition.position)
            {
                ChangeState(EnemyState.idle) ;
                logAnim.SetBool("isWalking", false);
                logAnim.SetBool("Woke", false);
                
            }
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if(currentState != newState)
        {
            currentState = newState;
        }
    }



    private void SetAnim(Vector2 setVector)  //muszę sobie coś takie ustawić, bo tutaj chodzi o VECTOR !!! 
    {
        logAnim.SetFloat("MoveX",setVector.x);
        logAnim.SetFloat("MoveY", setVector.y);
    }
    public void ChangeAnim(Vector2 dir)
    {
        if (Math.Abs(dir.x) > Math.Abs(dir.y))
        {
            if (dir.x > 0)
            {
                SetAnim(Vector2.right);
                
            } else if (dir.x < 0)
            {
                SetAnim(Vector2.left);
            }
            
        }else if (Math.Abs(dir.x) < Math.Abs(dir.y))
        {
            if (dir.y > 0)
            {
                SetAnim(Vector2.up);
                
            }else if (dir.y < 0)
            {
                SetAnim(Vector2.down);
            }
        }
        
        
        
    }
    
}
