using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover> {

    // used for selected tower sprite when placing towers
    private SpriteRenderer spriteRenderer;

    // holds tower range sprite when placing tower
    private SpriteRenderer rangedSpriteRenderer;

    private Transform rangeTransform;

    // Use this for initialization
    void Start ()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.rangedSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        this.rangeTransform = transform.GetChild(0).GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        FollowMouse();
	}

    /// <summary>
    /// Renders sprite when placing towers
    /// </summary>
    private void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    /// <summary>
    /// Activates range sprite when placing towers
    /// </summary>
    /// <param name="sprite"></param>
    public void Activate(Sprite sprite, float scale)
    {
        spriteRenderer.enabled = true;
        rangedSpriteRenderer.enabled = true;
        this.spriteRenderer.sprite = sprite;
        this.rangeTransform.localScale = new Vector3(scale, scale, 0);

    }

    /// <summary>
    /// Deactivates range sprite when done placing tower
    /// </summary>
    public void Deactivate()
    {
        spriteRenderer.enabled = false;
        rangedSpriteRenderer.enabled = false;
        GameManager.Instance.ClickedBtn = null;
    }
}
