using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingSapling : Sign
{
    private bool _thereIsASaplingInIt = false;
    private bool _deleteSaplingFromInventory = false;
    [SerializeField] private Animator saplingAnim;
    [SerializeField] private CircleCollider2D saplingCollider;
    public Inventory playerInventory;
    public GameObject areaOfTriggering;
    
    
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            if (playerInventory.numberOfSaplings == 0 && !_thereIsASaplingInIt)
            {
                dialogBox.SetActive(true);
                dialogText.text = dialog;
                
            }else if (playerInventory.numberOfSaplings >= 0 && !_thereIsASaplingInIt)
            {
                saplingAnim.SetBool("isReborn", true);
                _thereIsASaplingInIt = true;
                areaOfTriggering.SetActive(false);
                saplingCollider.enabled = true;
                isNecessary = false;
                
                
                if (!_deleteSaplingFromInventory)
                {
                    playerInventory.numberOfSaplings--;
                    _deleteSaplingFromInventory = true;
                }
                
            }
        }
      
    }
}
