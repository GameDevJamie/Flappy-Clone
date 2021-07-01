using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    public GameObject GroundPrefab;
    public float SpawnYPos;     //Where on Y-Position to spawn grounds
    public float DestroyXPos;   //When to move the ground back to the start (furthest right position of a ground)
    public float MoveSpeed;     //How fast to move the ground

    private GameObject[] GroundList;
    private float GroundSizeWidth;

    private bool m_Active;

    private void Awake()
    {
        //Instantiate all ground objects needed
        var sr = GroundPrefab.GetComponent<SpriteRenderer>();
        GroundSizeWidth = sr.size.x - 0.5f;

        GroundList = new GameObject[3];
        GroundList[0] = Instantiate(GroundPrefab, new Vector3(0.0f, SpawnYPos, 0.0f), Quaternion.identity);
        GroundList[1] = Instantiate(GroundPrefab, new Vector3(GroundSizeWidth, SpawnYPos, 0.0f), Quaternion.identity);
        GroundList[2] = Instantiate(GroundPrefab, new Vector3(GroundSizeWidth * 2.0f, SpawnYPos, 0.0f), Quaternion.identity);

        GroundList[0].transform.SetParent(this.transform);
        GroundList[1].transform.SetParent(this.transform);
        GroundList[2].transform.SetParent(this.transform);

        m_Active = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!m_Active) return;
        Move();
    }

    private void Move()
    {
        foreach(GameObject ground in GroundList)
        {
            ground.transform.position += Vector3.left * MoveSpeed * Time.deltaTime;

            if(ground.transform.position.x < DestroyXPos)
            {
                //Find the rightmost ground object
                float rightMostXPos = -100f;
                for(int i = 0; i < GroundList.Length; ++i)
                {
                    if(GroundList[i].transform.position.x > rightMostXPos) rightMostXPos = GroundList[i].transform.position.x;
                }

                //Move to the end of the ground train
                ground.transform.position = new Vector3(rightMostXPos + GroundSizeWidth, ground.transform.position.y);
            }
        }
    }

    #region Public Methods
    public void Enable(bool enable = true)
    {
        m_Active = enable;
    }
    #endregion
}
