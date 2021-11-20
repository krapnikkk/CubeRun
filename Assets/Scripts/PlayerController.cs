using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform m_Transform;
    private MapManager m_MapManager;

    private CameraFollow m_CameraFollow;
    private Color colorOne = new Color(122 / 255f, 85 / 255f, 179 / 255f);
    private Color colorTwo = new Color(126 / 255f, 93 / 255f, 183 / 255f);
    private Color playerColor2;

    private int gemCount = 0;

    private void AddGem()
    {
        gemCount++;
    }
    public int z = 3;
    public int x = 2;

    private bool live = true;
    // Start is called before the first frame update
    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_MapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        m_CameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        // SetPlayerPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetPlayerPos(z, x);
            m_CameraFollow.startFollow = true;
            m_MapManager.StartTileDown();
        }
        if (live)
        {
            PlayerControl();
        }
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
        MeshRenderer obj = null;

        m_Transform.position = playerPos.position + new Vector3(0, 0.254f / 2, 0);
        m_Transform.rotation = playerPos.rotation;

        if (playerPos.tag == "tile")
        {
            obj = playerPos.Find("normal_a2").GetComponent<MeshRenderer>();
        }
        else if (playerPos.tag == "sky_trap")
        {
            obj = playerPos.Find("smashing_spikes_a2").GetComponent<MeshRenderer>();
        }
        else if (playerPos.tag == "floor_trap")
        {
            obj = playerPos.Find("moving_spikes_a2").GetComponent<MeshRenderer>();
        }


        if (obj)
        {
            if (z % 2 == 0)
            {
                obj.material.color = colorOne;
            }
            else
            {
                obj.material.color = colorTwo;
            }
        }
        else
        {
            FallDown();
        }
    }

    private void CalcPosition()
    {
        int mapListCount = m_MapManager.mapList.Count;
        if (mapListCount - z <= 12)
        {
            m_MapManager.AddPR();
            float offsetZ = m_MapManager.mapList[mapListCount - 1][0].GetComponent<Transform>().position.z + m_MapManager.halfFloor / 2;
            m_MapManager.CreateMapItem(offsetZ);
        }
    }

    public void FallDown()
    {
        gameObject.AddComponent<Rigidbody>();
        StartCoroutine("GameOver", true);
    }

    private void OnTriggerEnter(Collider coll)
    {
        print(coll);
        if (coll.tag == "spikes")
        {
            StartCoroutine("GameOver", false);
        }
        if (coll.tag == "gem")
        {
            GameObject.Destroy(coll.gameObject.GetComponent<Transform>().parent.gameObject);
            AddGem();
        }
    }

    public IEnumerator GameOver(bool now)
    {
        if (now)
        {
            yield return new WaitForSeconds(0.5f);
        }
        if (live)
        {
            live = false;
            m_CameraFollow.startFollow = false;
        }
    }
}
