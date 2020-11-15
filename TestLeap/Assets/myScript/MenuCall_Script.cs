﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuCall_Script : MonoBehaviour
{
    [SerializeField] GameObject menuCanvas;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "hand")
        {
            Debug.Log("Menu Call");
            GetComponent<MeshExploder>().Explode();
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);

            menuCanvas.SetActive(true);
        }
    }
}
