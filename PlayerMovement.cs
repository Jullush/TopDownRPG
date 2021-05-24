using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState     
{
    walk,
    interact,
    attack,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _myRigid;
    private Vector3 _changeOfVectorMovement;
    private Animator _animator;
    private SpriteRenderer _playersSprite;
    
    [SerializeField] private GameObject itemLightening;
    [SerializeField] private SpriteRenderer playersSprite;
    
    public PlayerState currentState;
    public float speed;
    public FloatValue currentHealth;
    public SignalSender playerHealthSignal;
    public VectorValue changeVector;
    public Inventory playersInventory;
    public SpriteRenderer currentItem;
   
    private void Start()
    {
        _playersSprite = GetComponent<SpriteRenderer>();
        currentState = PlayerState.walk;
        _myRigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.SetFloat("MoveX", 0);
        _animator.SetFloat("MoveY", -1);
        _myRigid.transform.position = changeVector.initialVector;
    }

    private void Update()
    {
       
        if (currentState == PlayerState.interact)
        {
            return;
        }
        
        _changeOfVectorMovement = Vector3.zero;
        _changeOfVectorMovement.x = Input.GetAxisRaw("Horizontal");
        _changeOfVectorMovement.y = Input.GetAxisRaw("Vertical");


        if(Input.GetButtonDown("attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());

        }else if(currentState == PlayerState.walk)
        {
            UpdatePlayerAndMoveAnimations();        
        }
        
    }

    private IEnumerator AttackCo()
    {
        _animator.SetBool("Attacking", true);
        currentState = PlayerState.attack;
        yield return null;  //czeka 1 frame 
        _animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    public void OnItemRaise()
    {
        currentState = PlayerState.interact;
        _animator.SetBool("ReceivedItem", true);
        currentItem.sprite = playersInventory.item.itemSprite;
        itemLightening.SetActive(true);
        StartCoroutine(ReceivedItemCo());

        
    }
    
    void UpdatePlayerAndMoveAnimations()
    {
        if (_changeOfVectorMovement != Vector3.zero)
        {
            MovePlayer();
            _animator.SetFloat("MoveX", _changeOfVectorMovement.x);
            _animator.SetFloat("MoveY", _changeOfVectorMovement.y);
            _animator.SetBool("Moving", true);
        }
        else
        {
            _animator.SetBool("Moving", false);
        }
    }
    

     void MovePlayer()
    {
        _myRigid.MovePosition(transform.position + _changeOfVectorMovement.normalized * speed * Time.fixedDeltaTime);
    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.runtimeValue -= damage;
        if (currentHealth.runtimeValue > 0)
        {
            playerHealthSignal.Raise();
            StartCoroutine(FlashCo());
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
        }

    }


    private IEnumerator KnockCo(float knockTime)
    {
        if (_myRigid != null && currentState == PlayerState.stagger)
        {
            yield return new WaitForSeconds(knockTime);
            _myRigid.velocity = Vector2.zero;
            currentState = PlayerState.walk;
        }
    }

    private IEnumerator ReceivedItemCo()
    {
        yield return new WaitForSeconds(3f);
        _animator.SetBool("ReceivedItem", false);
        currentState = PlayerState.walk;
        itemLightening.SetActive(false);
        currentItem.sprite = null;
    }

    private IEnumerator FlashCo()
    {
        playersSprite.color = new Color(_playersSprite.color.r, _playersSprite.color.g, _playersSprite.color.b, 0f);
        yield return new WaitForSeconds(0.05f);
        playersSprite.color = new Color(_playersSprite.color.r, _playersSprite.color.g, _playersSprite.color.b, 1f);
    }
    
}
