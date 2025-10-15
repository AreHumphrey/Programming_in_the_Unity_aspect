using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WCTilePair
{
    public float prob;
    public GameObject tile;
}

[CreateAssetMenu(fileName = "WCTile", menuName = "Scriptable Objects/WCTile")]
public class WCTile : ScriptableObject
{
    [SerializeField] GameObject tile;
    [SerializeField] List<WCTilePair> probabilitys;
    public GameObject Tile
    {
        get => tile;
    }
    public List<WCTilePair> Prob
    {
        get => probabilitys;
    }

    public String Name
    {
        get => name;
    }

}
