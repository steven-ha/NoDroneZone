using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerSpotManager : MonoBehaviour
{

    [SerializeField]
    private GameObject towerSpot;

    // holds current scene name
    private string sceneName;

    // holds size of towerspot
    private float spotSize;

    public float SpotSize
    {
        get
        {
            return towerSpot.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }

        set
        {
            spotSize = value;
        }
    }



    // Use this for initialization
    void Start()
    {
        CreateTowerSpots();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Loads tower spots from file 
    /// </summary>
    private void CreateTowerSpots()
    {
        string[] spotData = ReadSpotText();

        int spotX = spotData[0].ToCharArray().Length;
        int spotY = spotData.Length;

        Vector3 worldStart = new Vector3(-4.6f, 2.0f);

        for (int y = 0; y < spotY; y++)
        {
            char[] newSpots = spotData[y].ToCharArray();

            for (int x = 0; x < spotX; x++)
            {
                PlaceSpot(newSpots[x].ToString(), x, y, worldStart);
            }
        }
    }

    /// <summary>
    /// Places spot on map
    /// </summary>
    /// <param name="isSpot"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="worldStart"></param>
    private void PlaceSpot(string isSpot,  int x, int y, Vector3 worldStart)
    {
        if (isSpot == "1")
        {
            GameObject newSpot = Instantiate(towerSpot);
            newSpot.transform.position = new Vector3(worldStart.x + (SpotSize * x), worldStart.y - (SpotSize * y), 0);
        }
    }

    /// <summary>
    /// Reads in map file and splits data
    /// </summary>
    /// <returns></returns>
    private string[] ReadSpotText()
    {
        sceneName = SceneManager.GetActiveScene().name;
        TextAsset bindData = Resources.Load(sceneName) as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }
}
