using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainApp : Singleton<MainApp>
{
    protected MainApp() { } // guarantee this will be always a singleton only - can't use the constructor!

    private Subject subject;

    private Observer[] observers = new Observer[0];

    private void Start()
    {
        subject = new Subject();

        RegisterAllController(subject);

        Init();
    }

    public void Notify(string entity, params object[] objects)
    {
        subject.notify(entity, objects);
    }

    public void Init() {
        subject.notify(GeneralFlag.ObeserverEvent.AppStart);
    }

    private void RegisterAllController(Subject p_subject)
    {
        Transform ctrlHolder = transform.Find("Controller");

        if (ctrlHolder == null) return;

        observers = transform.GetComponentsInChildren<Observer>();

        foreach (Observer observer in observers)
        {
            subject.addObserver(observer);
        }
    }


    public T GetObserver<T>() where T : Observer
    {
        foreach (Observer observer in observers)
        {
            if (observer.GetType() == typeof(T)) return (T)observer;
        }

        return default(T);
    }
}
