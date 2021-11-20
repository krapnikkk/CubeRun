using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private Transform m_Transform;
    private Transform child_Transform;

    private Vector3 normalPos;
    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        child_Transform = m_Transform.Find("moving_spikes_b").GetComponent<Transform>();
        normalPos = child_Transform.position;
        targetPos = child_Transform.position + new Vector3(0, 0.15f, 0);
        StartCoroutine("UpAndDown");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator UpAndDown()
    {
        while (true)
        {
            StopCoroutine("Down");
            StartCoroutine("Up");
            yield return new WaitForSeconds(2.0f);
            StopCoroutine("Up");
            StartCoroutine("Down");
            yield return new WaitForSeconds(2.0f);
        }
    }

    private IEnumerator Up()
    {
        while (true)
        {
            child_Transform.position = Vector3.Lerp(child_Transform.position, targetPos, Time.deltaTime * 40);
            yield return null;
        }
    }

    private IEnumerator Down()
    {
        while (true)
        {
            child_Transform.position = Vector3.Lerp(child_Transform.position, normalPos, Time.deltaTime * 40);
            yield return null;
        }
    }
}
