using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    Transform m_Transform;
    Transform m_gem;
    // Start is called before the first frame update
    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_gem = m_Transform.Find("gem 3");
    }

    // Update is called once per frame
    void Update()
    {
        m_gem.Rotate(Vector3.up);
    }

    
}
