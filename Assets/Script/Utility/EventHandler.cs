using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public static EventHandler instance;

    private List<IRecieveEvents> recievers = new List<IRecieveEvents>();
    public enum EventType
    {
        zoomIn, zoomOut, sceneSwitch
    }
    private void Awake()
    {
        instance = this;
    }

    public void AddReciever(IRecieveEvents reciever)
    {
        recievers.Add(reciever);
    }
    public void RemoveReciever(IRecieveEvents reciever)
    {
        if(recievers.Contains(reciever))
            recievers.Remove(reciever);
    }

    public void Call(EventType type)
    {
        foreach (IRecieveEvents reciever in recievers)
        {
            if(reciever!=null)
                reciever.EventCall(type);
        }
    }
}
public interface IRecieveEvents
{
    void EventSubscribe();

    void EventCall(EventHandler.EventType type);
}
