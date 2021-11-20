using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform m_Transiton;

    private Transform m_Player;

    public bool startFollow = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Transiton = gameObject.GetComponent<Transform>();
        m_Player = GameObject.Find("cube_books").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        if (startFollow)
        {
            Vector3 nextPos = new Vector3(m_Transiton.position.x, m_Player.position.y + 1.5f, m_Player.position.z);
            m_Transiton.position = Vector3.Lerp(m_Transiton.position, nextPos, Time.deltaTime);
        }
    }

}
