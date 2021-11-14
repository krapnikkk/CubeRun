using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private GameObject m_prefab_tile;
    private Transform m_Transform;
    // Start is called before the first frame update
    void Start()
    {
        m_prefab_tile = Resources.Load("tile_white") as GameObject;
        m_Transform = gameObject.GetComponent<Transform>();
        createMapItem();

    }

    private void createMapItem()
    {
        float span = Mathf.Sqrt(2)*0.254f;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Vector3 pos = new Vector3(j * span, 0, i * span);
                Vector3 rot = new Vector3(-90, 45, 0);
                GameObject tile = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot));
                tile.GetComponent<Transform>().SetParent(m_Transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
