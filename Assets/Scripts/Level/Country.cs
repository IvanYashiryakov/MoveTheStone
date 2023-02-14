using UnityEngine;

[CreateAssetMenu(fileName = "New Country", menuName = "Level/New Country", order = 51)]
public class Country : ScriptableObject
{
    public string EnName;
    public string RuName;
    public string TrName;
    public Town[] Towns;
    public Sprite Background;
}
