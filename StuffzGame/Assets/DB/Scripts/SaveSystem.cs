using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem
{
    private volatile static SaveSystem uniqueInstance;  //volatile so you instantiate and synchronize lazily
    private static readonly object padlock = new object();


    private SaveSystem()
    {
    }

    private static SaveSystem GetInstance()
    {
        if (uniqueInstance == null)
        {
            lock (padlock)
            {
                if (uniqueInstance == null) // check again to be thread-safe
                {
                    uniqueInstance = new SaveSystem();
                }
            }
        }
        return uniqueInstance;
    }

    public void Save(System.Object obj, Boolean saveAsJson)
    {
        if (saveAsJson)
        {
            SaveAsJson(obj);
        }
        else
        {
            SaveAsBinary(obj);
        }
    }

    private void SaveAsJson(System.Object obj)
    {
        string fileExtension = ".json";
    }

    private void SaveAsBinary(System.Object obj)
    {
        string fileExtension = ".sav";
    }
}