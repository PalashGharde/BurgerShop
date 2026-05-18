using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform plateVisual;
    [SerializeField] private Transform counterTopPoint;

    private int plateCount;


    private List<GameObject> platesVisualList;

    private void Awake()
    {
        platesVisualList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateAdded += PlatesCounter_OnPlateAdded;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
    {
        if (plateCount > 0)
        {
            GameObject plateOnTop = platesVisualList[platesVisualList.Count-1];
            platesVisualList.Remove(plateOnTop);
            Destroy(plateOnTop);
        }
        
    }

    private void PlatesCounter_OnPlateAdded(object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisual,counterTopPoint);

        float platePositionOffsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, platePositionOffsetY * platesVisualList.Count, 0);

        platesVisualList.Add(plateVisualTransform.gameObject);
        plateCount++;
    }
}
