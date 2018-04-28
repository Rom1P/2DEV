using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable]
public class GameDataEditor
{
    public string Name;
    public int Number;
    public List<string> ListBridges = new List<string>();
    public int SpawnX;
    public int SpawnZ;
    public float timer;
}