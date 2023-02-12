using UnityEngine;

[CreateAssetMenu(fileName = "New Country", menuName = "Level/New Country", order = 51)]
public class Country : ScriptableObject
{
    public string Name;
    public Town[] Towns;
    public Sprite Background;
}
