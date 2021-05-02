using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehavior : MonoBehaviour
{
    [SerializeField]
    Transform m_matchingEffect;
    [SerializeField]
    Transform m_destroyEffect;

    public void PlayBlockEffect(bool isPlay)
    {
        m_matchingEffect.gameObject.SetActive(isPlay);
    }

    public void PlayDestroyBlock()
    {
        m_destroyEffect.transform.parent = null;
        m_destroyEffect.gameObject.SetActive(true);
        GameObject.Destroy(gameObject);
    }
}
