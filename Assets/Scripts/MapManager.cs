using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private GameObject m_prefab_tile;
    private GameObject m_prefab_wall;
    private GameObject m_prefab_spikes;
    private GameObject m_prefab_sky_spikes;
    private GameObject m_prefab_gem;
    public List<GameObject[]> mapList = new List<GameObject[]>();
    private Transform m_Transform;
    private int pr_hole = 0;
    private int pr_spikes = 0;
    private int pr_sky_spikes = 0;
    private int pr_gem = 2;
    private PlayerController m_PlayerController;
    private Color colorOne = new Color(124 / 255f, 155 / 255f, 230 / 255f);
    private Color colorTwo = new Color(125 / 255f, 169 / 255f, 233 / 255f);
    private Color colorWall = new Color(87 / 255f, 93 / 255f, 169 / 255f);

    public float halfFloor = Mathf.Sqrt(2) * 0.254f;
    // Start is called before the first frame update
    void Start()
    {
        m_prefab_tile = Resources.Load("tile_white") as GameObject;
        m_prefab_wall = Resources.Load("wall") as GameObject;
        m_prefab_spikes = Resources.Load("moving_spikes") as GameObject;
        m_prefab_sky_spikes = Resources.Load("smashing_spikes") as GameObject;
        m_prefab_gem = Resources.Load("gem 2") as GameObject;

        m_Transform = gameObject.GetComponent<Transform>();
        m_PlayerController = GameObject.Find("cube_books").GetComponent<PlayerController>();
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
                    int pr = CalcPR();
                    if (pr == 0)
                    {
                        obj = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot));
                        obj.GetComponent<Transform>().Find("normal_a2").GetComponent<MeshRenderer>().material.color = colorOne;
                        obj.GetComponent<MeshRenderer>().material.color = colorOne;

                        CreateGem(obj);
                    }
                    else if (pr == 1)
                    {
                        obj = new GameObject();
                        obj.GetComponent<Transform>().position = pos;
                        obj.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                    }
                    else if (pr == 2)
                    {
                        obj = GameObject.Instantiate(m_prefab_spikes, pos, Quaternion.Euler(rot));
                    }
                    else if (pr == 3)
                    {
                        obj = GameObject.Instantiate(m_prefab_sky_spikes, pos, Quaternion.Euler(rot));
                    }
                }
                obj.GetComponent<Transform>().SetParent(m_Transform);
                item[j] = obj;
            }
            mapList.Add(item);

            GameObject[] item2 = new GameObject[5];
            for (int j = 0; j < 5; j++)
            {
                GameObject obj = null;

                Vector3 pos = new Vector3(j * halfFloor + halfFloor / 2, 0, offsetZ + i * halfFloor + halfFloor / 2);
                Vector3 rot = new Vector3(-90, 45, 0);
                int pr = CalcPR();
                if (pr == 0)
                {
                    obj = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot));
                    obj.GetComponent<Transform>().Find("normal_a2").GetComponent<MeshRenderer>().material.color = colorTwo;

                    obj.GetComponent<MeshRenderer>().material.color = colorTwo;
                    CreateGem(obj);
                }
                else if (pr == 1)
                {
                    obj = new GameObject();
                    obj.GetComponent<Transform>().position = pos;
                    obj.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                }
                else if (pr == 2)
                {
                    obj = GameObject.Instantiate(m_prefab_spikes, pos, Quaternion.Euler(rot));
                }
                else if (pr == 3)
                {
                    obj = GameObject.Instantiate(m_prefab_sky_spikes, pos, Quaternion.Euler(rot));
                }

                obj.GetComponent<Transform>().SetParent(m_Transform);
                item2[j] = obj;
            }
            mapList.Add(item2);
        }
    }

    private void CreateGem(GameObject obj)
    {
        int gemPr = CalcGemPR();
        if (gemPr == 1)
        {
            Transform transform = obj.GetComponent<Transform>();
            GameObject gem = GameObject.Instantiate(m_prefab_gem, transform.position + new Vector3(0, 0.06f, 0), Quaternion.identity);
            gem.GetComponent<Transform>().SetParent(transform);
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
        StartCoroutine("TileDown");
    }

    public void StopTileDown()
    {
        StopCoroutine("TileDown");
    }

    int index = 0;
    private IEnumerator TileDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < mapList[index].Length; i++)
            {
                GameObject obj = mapList[index][i];
                Rigidbody rb = obj.AddComponent<Rigidbody>();
                rb.angularVelocity = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) * Random.Range(1, 10);
                GameObject.Destroy(obj, 1.0f);
            }
            if (m_PlayerController.z == index)
            {
                StopTileDown();
                m_PlayerController.FallDown();
            }
            index++;

        }
    }

    // 0:floor
    // 1:hole
    // 2:trap_floor
    // 3:trap_sky
    private int CalcPR()
    {
        int pr = Random.Range(1, 100);
        if (pr < pr_hole)
        {
            return 1;
        }
        else if (31 < pr && pr < pr_spikes + 30)
        {
            return 2;
        }
        else if (61 < pr && pr < pr_sky_spikes + 60)
        {
            return 3;
        }
        return 0;
    }

    private int CalcGemPR()
    {
        int pr = Random.Range(1, 100);
        if (pr <= pr_gem)
        {
            return 1;
        }
        return 0;
    }

    public void AddPR()
    {
        pr_hole += 2;
        pr_spikes += 2;
        pr_sky_spikes += 2;
    }

    public void ResetMap()
    {
        Transform[] child = m_Transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < child.Length; i++)
        {
            GameObject.Destroy(child[i].gameObject);
        }

        pr_gem = 2;
        pr_hole = 0;
        pr_spikes = 0;
        pr_sky_spikes = 0;

        index = 0;
        mapList.Clear();

        CreateMapItem(0);
    }
}
