﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Floor
{
    public GameObject reference;
    public Vector3 floorStart;

    public Floor(GameObject floorReference, Vector3 startPosition)
    {
        reference = floorReference;
        floorStart = startPosition;
    }
}

[ExecuteInEditMode]
//This Script Will Load In A New Floor
public class FloorManager : MonoBehaviour
{
    [SerializeField]
    public GameObject[] floors;                             //[] is the gameObject floor holder
    public Dictionary<state.floor, Floor> floorDic;

    //The Currently Active Floor by enumerator, can be searced in floorDic for GameObject Reference
    private state.floor activeFloor;


    void Awake() {
        if(floors.Length < 7)
        {
            return;
        }
        //Initialize the Dictionary
        floorDic = new Dictionary<state.floor, Floor>();

        //Iterate through the state.floor enum to assign a Dict with every floor from the array
        string[] floorNames = System.Enum.GetNames(typeof(state.floor));
        for (int i = 0; i < floorNames.Length; i++) {
            floorDic.Add((state.floor)i, new Floor(floors[i], floors[i].transform.position));
        }
        activeFloor = state.floor.Empty;
    }

    //Called from elevatorMovement when a new floor is reached
    //inEditor is true if this function is called inEditor, false otherwise
    public void loadNewFloor(state.floor targetFloor, bool inEditor)
    {
        if (activeFloor != targetFloor)
        {
            //Turn off the previously active floor
            foreach (Transform child in transform)
            {
                //child is your child transform
                child.gameObject.SetActive(false);
            }

            //Turn on the next floor
            floorDic[targetFloor].reference.SetActive(true);

            //The New Active floor is the floor we just activated
            activeFloor = targetFloor;
        }
        if (!inEditor)
        {
            //Load in Extras, play sound effects etc.
            GetComponent<ExtraManager>().clearExtras();
            GetComponent<ExtraManager>().loadNewExtras(targetFloor);
        }
    }

    public GameObject getReference(state.floor param)
    {
        return floorDic[param].reference;
    }
}
