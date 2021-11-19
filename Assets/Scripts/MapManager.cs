using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private GameObject m_prefab_tile;
    private GameObject m_prefab_wall;
    public List<GameObject[]> mapList = new List<GameObject[]>();
    private Transform m_Transform;
    private Color colorOne = new Color(124 / 255f, 155 / 255f, 230 / 255f);
    private Color colorTwo = new Color(125 / 255f, 169 / 255f, 233 / 255f);
    private Color colorWall = new Color(87 / 255f, 93 / 255f, 169 / 255f);

    public float halfFloor = Mathf.Sqrt(2) * 0.254f;
    // Start is called before the first frame update
    void Start()
    {
        m_prefab_tile = Resources.Load("tile_white") as GameObject;
        m_prefab_wall = Resources.Load("wall") as GameObject;
        m_Transform = gameObject.GetComponent<Transform>();
        CreateMapItem(0);

    }

    public void CreateMapItem(float offsetZ)
    {

        for (int i = 0; i < 10; i++)
        {
            GameObject[] item = new GameObject[6];
            for (int j = 0; j < 6; j++)
            {
                Vector3 pos = new Vector3(j * halfFloor, 0, offsetZ + i * halfFloor);
                Vector3 rot = new Vector3(-90, 45, 0);
                GameObject obj = null;
                if (j == 0 || j == 5)
                {
                    obj = GameObject.Instantiate(m_prefab_wall, pos, Quaternion.Euler(rot));
                    obj.GetComponent<MeshRenderer>().material.color = colorWall;
                }
                else
                {
                    obj = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot));
                    obj.GetComponent<Transform>().Find("normal_a2").GetComponent<MeshRenderer>().material.color = colorOne;

                    obj.GetComponent<MeshRenderer>().material.color = colorOne;
                }
                obj.GetComponent<Transform>().SetParent(m_Transform);
                item[j] = obj;
            }
            mapList.Add(item);

            GameObject[] item2 = new GameObject[5];
            for (int j = 0; j < 5; j++)
            {
                Vector3 pos = new Vector3(j * halfFloor + halfFloor / 2, 0, offsetZ + i * halfFloor + halfFloor / 2);
                Vector3 rot = new Vector3(-90, 45, 0);
                GameObject tile = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot));
                tile.GetComponent<Transform>().SetParent(m_Transform);
                tile.GetComponent<Transform>().Find("normal_a2").GetComponent<MeshRenderer>().material.color = colorTwo;
                tile.GetComponent<MeshRenderer>().material.color = colorTwo;
                item2[j] = tile;
            }
            mapList.Add(item2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     for (int i = 0; i < mapList.Count; i++)
        //     {
        //         for (int j = 0; j < mapList[i].Length; j++)
        //         {
        //             mapList[i][j].name = i + "-" + j;
        //         }

        //     }
        // }
    }

    public void StartTileDown()
    {
        print("StartTileDown");
        StartCoroutine("TileDown");
    }

    public void StopTileDown()
    {
        StopCoroutine("TileDown");
    }

    int index = 0;
    private IEnumerable TileDown()
    {
        while (true)
        {
            print("TileDown");
            for (int i = 0; i < mapList[index].Length; i++)
            {
                GameObject obj = mapList[index][i];
                Rigidbody rb = obj.AddComponent<Rigidbody>();
                rb.angularVelocity = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) * Random.Range(1, 10);
                GameObject.Destroy(obj, 1.0f);
            }
            index++;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
