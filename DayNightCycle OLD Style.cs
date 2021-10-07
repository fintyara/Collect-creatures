using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using SuperMaxim.Messaging;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    public List<DayNightState> States;
    public DayNightState CurState;
   // public int indexState;
    [Space(10)]
    public DayNightState StartState;
    [Space(10)]
    public Light MainLight;
    public SpriteRenderer Sky;

    public Type MyType;
    [Space(10)]   
    public float curHour;
    public float durationHour = 30;
    [ShowOnly]
    public float timerTime;

    public TextMeshProUGUI CountHourGUI;


    public void NextState()
    {
        int newIndex = States.IndexOf(CurState) + 1;

        if (newIndex == States.Count)
            CurState = States[0];
        else
            CurState = States[newIndex];
    
        Messenger.Default.Publish(new TimeOfDayPayload { NewState = CurState });
    }
    public void SetState(DayNightState _NewState)
    {
        if (!States.Contains(_NewState)) { Debug.Log("ERROR"); return; }

        CurState = _NewState;

        UpdateTime();
        SetColor();
        SetIntensity();

        if (CountHourGUI != null)
            CountHourGUI.text = curHour.ToString();

        Messenger.Default.Publish(new TimeOfDayPayload { NewState = CurState });
    }
    public void NextStateOld()
    {
     
      ///  indexState++;

      //  if (indexState == States.Count)
      //      indexState = 0;

     //   CurState = States[indexState];

      //  Messenger.Default.Publish(new TimeOfDayPayload { NewState = CurState });
    }
    public void SetStateOld(int _index)
    {
      //  if (_index >= States.Count) { Debug.Log("ERROR"); return; }

      //  indexState = _index;

      //  CurState = States[indexState];

      //  UpdateTime();
      //  SetColor();
     //   SetIntensity();

      //  Messenger.Default.Publish(new TimeOfDayPayload { NewState = CurState });
    }
    public void UpdateTime()
    {
        curHour = CurState.timeBegin;
        timerTime = 0;   
    }
    void SetColor()
    {
        if (MyType == Type.Light)
            MainLight.color = CurState.Color;
        else
            Sky.color = CurState.Color;
    }
    void SetIntensity()
    {
        if (MyType == Type.Light)
            MainLight.intensity = CurState.intensity;
    }
    void ControlColor()
    {
        if (MyType == Type.Light) 
            MainLight.color = Color.Lerp(MainLight.color, CurState.Color, CurState.speedChangeColor / durationHour * Time.deltaTime);
        else
            Sky.color = Color.Lerp(Sky.color, CurState.Color, CurState.speedChangeColor / durationHour * Time.deltaTime);
    }
    void ControlIntensity()
    {
        if (MyType == Type.Light)
            MainLight.intensity = Mathf.Lerp(MainLight.intensity, CurState.intensity, CurState.speedChangeIntensity / durationHour * Time.deltaTime);
    }
    void ControlTime()
    {
        timerTime += Time.deltaTime;

        if (timerTime >= durationHour)
        {         
            curHour += 1;
            timerTime = 0;
          
            if (curHour > CurState.timeEnd)         
                NextState();

            if (curHour == 24)
                curHour = 0;

            if (CountHourGUI != null)
                CountHourGUI.text = curHour.ToString();

            Messenger.Default.Publish(new TimePayload { time = (int)curHour });
        }
    }
    
   
    void Start()
    {
        SetState(StartState);
    }
    void Update()
    {
        ControlColor();
        ControlIntensity();
        ControlTime();
    }

    public enum Type
    {
        Light, Sprite
    }
}
