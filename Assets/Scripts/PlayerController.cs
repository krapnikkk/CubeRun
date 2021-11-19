using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform m_Transform;
    private MapManager m_MapManager;
    private Color colorOne = new Color(122 / 255f, 85 / 255f, 179 / 255f);
    private Color colorTwo = new Color(126 / 255f, 93 / 255f, 183 / 255f);
    private Color playerColor2;
    private int z = 3;
    private int x = 2;
    // Start is called before the first frame update
    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_MapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        // SetPlayerPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SetPlayerPos(z, x);
        }
        PlayerControl();
    }

    private void PlayerControl()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (x != 0)
            {
                z++;
            }
            if (z % 2 == 1 && x != 0)
            {
                x--;
            }
            Debug.Log("z:" + z + "x:" + x);
            SetPlayerPos(z, x);
            CalcPosition();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (x != 4 || z % 2 != 1)
            {
                z++;
            }
            if (z % 2 == 0 && x != 4)
            {
                x++;
            }
            Debug.Log("z:" + z + "x:" + x);
            CalcPosition();
            SetPlayerPos(z, x);
        }
    }

    void SetPlayerPos(int z, int x)
    {
        Transform playerPos = m_MapManager.mapList[z][x].GetComponent<Transform>();
        MeshRenderer floor = playerPos.Find("normal_a2").GetComponent<MeshRenderer>();

        if (z % 2 == 0)
        {
            floor.material.color = colorOne;
        }
        else
        {
            floor.material.color = colorTwo;
        }
        m_Transform.position = playerPos.position + new Vector3(0, 0.254f / 2, 0);
        m_Transform.rotation = playerPos.rotation;
    }

    private void CalcPosition(){
        int mapListCount = m_MapManager.mapList.Count;
        if(mapListCount - z <= 12){
            float offsetZ = m_MapManager.mapList[mapListCount - 1][0].GetComponent<Transform>().position.z + m_MapManager.halfFloor / 2;
            m_MapManager.CreateMapItem(offsetZ);
        }
    }
}
