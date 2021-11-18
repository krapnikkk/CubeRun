using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform m_Transform;
    private MapManager m_MapManager;
    private int z = 3;
    private int x = 2;
    // Start is called before the first frame update
    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_MapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        // CreatePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S)){
            CreatePlayer(z,x);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            z++;
            if(z%2==1){
                x--;
            }
            CreatePlayer(z,x);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            z++;
            if(z%2==1){
                x++;
            }
            CreatePlayer(z,x);
        }
    }

    void CreatePlayer(int z,int x)
    {
        Transform playerPos = m_MapManager.mapList[z][x].GetComponent<Transform>();
        m_Transform.position = playerPos.position + new Vector3(0,0.254f/2,0);
        m_Transform.rotation = playerPos.rotation;
    }
}
