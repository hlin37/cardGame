using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    [SerializeField]
    private LayerMask clickableLayer;

    void Update()
    {
        //If the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            Debug.DrawRay(worldPoint, worldPoint, new Color32(255,0,0,0), 10);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);
            }
        }
    }

    // // Update is called once per frame
    // void Update () 
    // {
    //     if (Input.GetMouseButtonDown (0)) {
    //         Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
    //         RaycastHit2D hit = Physics2D.GetRayIntersection (ray, Mathf.Infinity);
    //         if (hit.collider != null && hit.collider.name == name) {
    //             Debug.Log("hello");
    //         }
    //     }
    // }
    // void Update () {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Vector2 origin = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    //         RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f);
    //         Debug.Log(hit);
    //         if (!hit) {
    //             Debug.Log("hello");
    //             print(hit.transform.gameObject.name);
    //         }
    //     }
    // }

    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0)) {
    //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //         RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, clickableLayer);
    //         Debug.Log("SOME");
    //         Debug.Log(hit);

    //         if (hit) {
    //             hit.collider.GetComponent<ClickOn>().ClickMe();
    //         }
    //     }
    // }
}
