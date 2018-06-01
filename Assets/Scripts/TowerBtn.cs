using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour {

    /// <summary>
    /// Tower prefab assigned to the button
    /// </summary>
    [SerializeField]
    private GameObject towerPrefab;

    /// <summary>
    /// Tower button sprite
    /// </summary>
    [SerializeField]
    private Sprite sprite;

    // price of tower
    [SerializeField]
    private int price;

    // price text displayed
    [SerializeField]
    private Text priceTxt;

    [SerializeField]
    private float towerRange;

    /// <summary>
    /// Getter for tower prefab
    /// </summary>
    public GameObject TowerPrefab
    {
        get
        {
            return this.towerPrefab;
        }
    }

    /// <summary>
    /// Getter for button sprite
    /// </summary>
    public Sprite Sprite
    {
        get
        {
            return sprite;
        }        
    }

    /// <summary>
    /// Getter for Price
    /// </summary>
    public int Price
    {
        get
        {
            return price;
        }
    }

    /// <summary>
    /// Getter/Setter for TowerRange
    /// </summary>
    public float TowerRange
    {
        get
        {
            return towerRange;
        }

        set
        {
            towerRange = value;
        }
    }

    /// <summary>
    /// Sets price text on game start and adds event listener
    /// </summary>
    private void Start()
    {
        priceTxt.text = "$" + Price;

        GameManager.Instance.Changed += new MoneyChanged(checkMoney);
    }

    /// <summary>
    /// Checks if user has enough money to purchase each tower and 
    /// Changes sprite and text accordingly
    /// </summary>
    private void checkMoney()
    {
        if (price <= GameManager.Instance.Money)
        {
            GetComponent<Image>().color = Color.white;
            priceTxt.color = new Color32(0xCE,0xAF,0x00, 0xFF);
        }
        else
        {
            GetComponent<Image>().color = Color.grey;
            priceTxt.color = Color.grey;
        }
    }
}
