using UnityEngine;

[CreateAssetMenu(fileName = "New Town", menuName = "Level/New Town", order = 51)]
public class Town : ScriptableObject
{
    public string Name;
    public Level[] Levels;
}
