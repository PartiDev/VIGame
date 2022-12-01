using UnityEngine;
using System.Collections;

public class Metronome : MonoBehaviour
{
    [SerializeField] private double bpm = 140.0f;
    [SerializeField] private AudioClip metronomeTick;

    public double nextTick = 0.0F;
    public bool ticked = false;

    private int metronomePitchMaxCount = 4;
    private int currentMetronomePitchCount;
    private float currentMetronomePitch = 0.9f;

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
        if (currentMetronomePitchCount < metronomePitchMaxCount)
        {
            currentMetronomePitch += 0.2f;
            currentMetronomePitchCount++;
        } else
        {
            currentMetronomePitch = 1f;
            currentMetronomePitchCount = 1;
        }

        AudioManager.Instance.PlaySFX(metronomeTick, 0.2f, currentMetronomePitch);
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