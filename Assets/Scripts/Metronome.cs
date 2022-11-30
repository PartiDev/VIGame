using UnityEngine;
using System.Collections;

public class Metronome : MonoBehaviour
{
    [SerializeField] private double bpm = 140.0f;
    [SerializeField] private AudioClip metronomeTick;

    public double nextTick = 0.0F;
    public bool ticked = false;

    public bool turn = false;
    
    public delegate void Ticked();
    public event Ticked OnMetronomeTick;

    private void Start()
    {
        double startTick = AudioSettings.dspTime;

        nextTick = startTick + (60.0 / bpm);
    }

    private void LateUpdate()
    {
        if (!ticked && nextTick >= AudioSettings.dspTime)
        {
            ticked = true;
            OnTick();
        }
    }

    private void OnTick()
    {
        float pitch = 1;

        if (turn)
        {
            pitch = 1.5f;
            turn = false;
        }
        else turn = true;

        AudioManager.Instance.PlaySFXDirectional(metronomeTick, turn, 0.2f, pitch);
        OnMetronomeTick();
    }

    private void FixedUpdate()
    {
        double timePerTick = 60.0f / bpm;
        double dspTime = AudioSettings.dspTime;

        while (dspTime >= nextTick)
        {
            ticked = false;
            nextTick += timePerTick;
        }
    }
}