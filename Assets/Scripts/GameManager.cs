using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void MoneyChanged();

public class GameManager : Singleton<GameManager>
{
    // currently selected button
    [SerializeField]
    public TowerBtn ClickedBtn { get; set; }

    // holds user money total
    private int money;

    // text displaying user money
    [SerializeField]
    private Text moneyTxt;

    // selected tower 
    [SerializeField]
    private Tower selectedTower;

    private const int startHp = 5;
    private int hp;

    [SerializeField]
    private Text hpText;

    private int lives;

    [SerializeField]
    private Text livesText;

    private bool gameOver;
    public bool impactCheck;

    [SerializeField]
    public ObjectPool Pool { get; set; }

    // User money
    public int Money
    {
        get
        {
            return money;
        }

        set
        {
            money = value;
            moneyTxt.text = value.ToString();
            SceneController.Instance.money = money;

            OnMoneyChanged();
        }
    }

    // user hp
    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
            SceneController.Instance.hitpoints = hp;
            hpText.text = value.ToString();
            if (hp <= 0)
            {
                Lives--;
                if (Lives > 0)
                {
                    hp = startHp;
                    SceneController.Instance.hitpoints = hp;
                    hpText.text = startHp.ToString();
                }
                else
                {
                    hp = 0;
                    SceneController.Instance.hitpoints = hp;
                    hpText.text = 0.ToString();
                }
            }
        }
    }

    public int Lives
    {
        get
        {
            return lives;
        }

        set
        {
            lives = value;
            SceneController.Instance.lives = lives;
            livesText.text = value.ToString();
            if (lives <= 0)
            {
                GameOver = true;
				SceneController.Instance.gameFinished = true;
                SceneController.Instance.loadScene5 = true;
            }

        }
    }

    public bool GameOver
    {
        get
        {
            return gameOver;
        }

        set
        {
            gameOver = value;
        }
    }

    

    // handles user money changes
    public event MoneyChanged Changed;

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }
    // Use this for initialization
    void Start ()
    {
        //Lives = SceneController.Instance.lives;
		Lives = SceneController.Instance.lives;
        //Hp = SceneController.Instance.hitpoints;
		Hp = SceneController.Instance.hitpoints;
        //Money = SceneController.Instance.money;
		Money = SceneController.Instance.money;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    /// <summary>
    /// Handles picking tower from tower button
    /// </summary>
    /// <param name="towerBtn"></param>
    public void PickTower(TowerBtn towerBtn)
    {
        if (Money >= towerBtn.Price)
        {
            this.ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite, towerBtn.TowerRange);
        }
    }

    /// <summary>
    /// Sets selected tower for GameManager
    /// </summary>
    /// <param name="tower"></param>
    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.SelectTower();
        }

        selectedTower = tower;
        selectedTower.SelectTower();
    }

    /// <summary>
    /// Deselects GameManager tower
    /// </summary>
    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.SelectTower();
        }

        selectedTower = null;
    }

    /// <summary>
    /// Event used to track changes to user money
    /// </summary>
    public void OnMoneyChanged()
    {
        if (Changed != null)
        {
            Changed();
        }
    }
}
