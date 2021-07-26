using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Localhost Data", menuName = "localhost Data")]
public class LocalhostData : ScriptableObject
{
    public string IP;
    public Int32 Port;
}
