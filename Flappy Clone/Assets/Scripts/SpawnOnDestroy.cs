using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Spawns a new given Gameobject when this Gameobject is destroyed
/// </summary>
public class SpawnOnDestroy : MonoBehaviour
{
    [SerializeField]
    private GameObject ToSpawn;

    private void OnDestroy()
    {
        if(ToSpawn != null) Instantiate(ToSpawn);
    }
}
