using UnityEngine;


[CreateAssetMenu(menuName = "DayNight/DayNightState")]
public class DayNightState : ScriptableObject
{
    public Color32 Color;
    public float intensity;
    public float timeBegin;
    public float timeEnd;
    public float speedChangeColor;
    public float speedChangeIntensity;
}
