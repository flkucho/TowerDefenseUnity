using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInteractionManager : MonoBehaviour
{
    public float radius;
    //Mask 
    public LayerMask mask;
    //Current TowerType to be placed
    public Tower_SO.TowerType CurrentType;
    // Assign the object prefabs to this variable
    // these prefabs will be instanced by index of the enum type
    public List<Tower> objectToInstantiate;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!GameManager.Instance.CanBuyTower())
                return;

            // Cast a ray from the main camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Interactable")
                {
                    // Check for overlapping colliders within the specified radius
                    Collider[] colliders = Physics.OverlapSphere(hit.point, radius, mask);
                    if (colliders.Length > 0) return;

                    try
                    {
                        // Get a pooled instance of the selected TowerType and set its position to the hit point
                        objectToInstantiate[(int)CurrentType].GetPooledInstance<Tower>().SetPosition(hit.point);
                        GameManager.Instance.BuyTower();
                    }
                    catch
                    {
                        Debug.Log("Failed to create instance of selected TowerType");
                    }

                }
            }
        }
    }
    public int GetSelectedTowerCost()
    {
        return objectToInstantiate[(int)CurrentType].Cost;
    }
}
