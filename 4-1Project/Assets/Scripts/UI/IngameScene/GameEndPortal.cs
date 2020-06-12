using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndPortal : MonoBehaviour
{
    public GameObject portal;

    private void OnDisable()
    {
        if(portal != null)
            portal.SetActive(true);
    }
}
