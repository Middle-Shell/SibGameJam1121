﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class AutoGenerator : MonoBehaviour
{
    public string TypeOfGenerator;
    public int NumOfGenerators; //цена
    public float CostOfOne; //цена
    public float DefaultElectricityPerMoment = 1; //значение увелечения
    /*[HideInInspector]*/public float CurrentElectricityPerMoment = 1;
    public bool IsWorking = true;
    [Space, Header("Visuals")]
    public UnityArmatureComponent VisualGenerator;
    public Placement myPlacement;
    [Header("Audio")]
    public AudioSource GenerorTurnOffSound;
    public AudioSource GenerorTurnOnSound;

    private bool IsBuffed = false;
    private bool IsDebuffed = false;
    private bool lastIsWorking;

    private void Start()
    {
        CurrentElectricityPerMoment = DefaultElectricityPerMoment;
    }

    private void Update()
    {
        if (NumOfGenerators == 0)
        {
            VisualGenerator.gameObject.SetActive(false);
        }
        else
        {
            VisualGenerator.gameObject.SetActive(true);
        }

        if (IsWorking)
        {
            if (VisualGenerator.animation.lastAnimationName != "0")
            {
                VisualGenerator.animation.Play("0");
            }
        }
        else
        {
            if (VisualGenerator.animation.lastAnimationName != "1")
            {
                VisualGenerator.animation.Play("1");
            }
        }

        if (NumOfGenerators == 0 || !IsWorking)
        {
            myPlacement.TurnOffElectricity();
        }
        else if (NumOfGenerators > 0 && IsWorking)
        {
            myPlacement.TurnOnElectricity();
        }

        if (lastIsWorking != IsWorking)
        {
            SFX();
        }
        lastIsWorking = IsWorking;
    }

    private void SFX()
    {
        if (NumOfGenerators > 0)
        {
            if (!IsWorking)
            {
                GenerorTurnOffSound.Play();
            }
            else
            {
                GenerorTurnOnSound.Play();
            }
        }
    }

    #region Buff:

    public void Buff()
    {
        if (!IsBuffed)
        {
            StartCoroutine(RemoveBuff());
        }
    }

    IEnumerator RemoveBuff()
    {
        IsBuffed = true;
        CurrentElectricityPerMoment *= 5;
        VisualGenerator.animation.timeScale = 2;

        yield return new WaitForSeconds(.5f);

        IsBuffed = false;
        VisualGenerator.animation.timeScale = 1;
        
        CurrentElectricityPerMoment = DefaultElectricityPerMoment;
    }

    #endregion

    #region Debuff:

    public void Debuff(float debuffTime)
    {
        if (!IsDebuffed)
        {
            StartCoroutine(RemoveDebuff(debuffTime));
        }
    }

    IEnumerator RemoveDebuff(float debuffTime)
    {
        IsDebuffed = true;
        CurrentElectricityPerMoment /= 5;

        yield return new WaitForSeconds(debuffTime);

        IsDebuffed = false;
        CurrentElectricityPerMoment = DefaultElectricityPerMoment;
    }

    #endregion

    #region TurnOff:

    public void TurnOff(float debuffTime)
    {
        if (!IsDebuffed)
        {
            StartCoroutine(TurnOn(debuffTime));
        }
    }

    IEnumerator TurnOn(float debuffTime)
    {
        IsWorking = false;

        yield return new WaitForSeconds(debuffTime);

        IsWorking = true;
    }

    #endregion
}