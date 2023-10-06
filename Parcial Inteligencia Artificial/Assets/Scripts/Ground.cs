using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public static Ground Instance;

    private void Awake()
    {
        Instance = this;
    }
}
