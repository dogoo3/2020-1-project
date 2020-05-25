using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMagicianTut : MonoBehaviour
{
    private void Awake()
    {
        if (GameManager.instance.type == 0)
            gameObject.SetActive(false);

    }
}
