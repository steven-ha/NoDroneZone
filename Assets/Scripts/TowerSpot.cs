using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour {

    [SerializeField]
    private GameObject tower;

    [SerializeField]
    private Sprite fullSpot;
    [SerializeField]
    private Sprite emptySpot;
    [SerializeField]
    private Sprite towerSpot;

    private Tower spotTower; 

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleSpace();
    }



    // check if spot is already occupied by tower and user has enough money
    private bool canPlaceTower()
    {
        if (GameManager.Instance.Money >= GameManager.Instance.ClickedBtn.Price && tower == null)
        {
            return true;
        }
        return false;
    }

    // On mouse click, if no tower present, create and place tower
    /// <summary>
    /// Handles when player clicks on tower spot.
    /// Handles tower placement and selection/deselection
    /// </summary>
    private void OnMouseOver()
    {
        // check if player has selected a tower to place
        if (GameManager.Instance.ClickedBtn != null)
        {
            // check if spot already has tower and uses appropriate sprite
            if (!canPlaceTower())
            {
                spriteRenderer.enabled = true;
                spriteRenderer.sprite = fullSpot;
            }
            else
            {
                spriteRenderer.enabled = true;
                spriteRenderer.sprite = emptySpot;
            }

            if (Input.GetMouseButtonDown(0))
            {
            
                if (canPlaceTower())
                {
                    GameManager.Instance.Money -= GameManager.Instance.ClickedBtn.Price;
                
                    // Checks the selected tower type and places it
                    tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
                    GameManager.Instance.ClickedBtn = null;
                    tower.transform.SetParent(transform);
                    this.spotTower = tower.transform.GetChild(0).GetComponent<Tower>();
                    // play tower placement sound
                    AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                    audioSource.PlayOneShot(audioSource.clip);
                    FindObjectOfType<Hover>().Deactivate();
                    spriteRenderer.sprite = towerSpot;
                }            
            }
        }
        // Handles tower selection/deselection
        else
        {          
            if (Input.GetMouseButtonDown(0))
            {
                if (tower != null && !spotTower.isSelected)
                {
                    GameManager.Instance.DeselectTower();
                    GameManager.Instance.SelectTower(spotTower);
                
                }
                else
                {
                    GameManager.Instance.DeselectTower();
                }
                    
                    
            }
        }
            
        
    }
    
    /// <summary>
    /// Returns sprite to normal when player removes mouse
    /// </summary>
    private void OnMouseExit()
    {
        spriteRenderer.enabled = false;
        //spriteRenderer.sprite = towerSpot;
    }

    /// <summary>
    /// Handles cancelling tower placement and tower selection
    /// </summary>
    private void HandleSpace()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hover.Instance.Deactivate();
            if (spotTower != null)
            {
                if (spotTower.isSelected)
                    GameManager.Instance.DeselectTower();
            }
        }
    }
}
