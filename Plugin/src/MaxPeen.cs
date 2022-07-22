using BepInEx;
using UnityEngine;
using Valve.VR;
using System.Collections;
using FistVR;

namespace H3VRMod
{
	[BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
	[BepInProcess("h3vr.exe")]
	public class MaxPeen : BaseUnityPlugin
	{
        private const float SlowdownFactor = .1f;
        private const float SlowdownLength = 6f;
        private string SlomoStatus = "Off";
        private const float MaxSlomo = .1f;
        private const float SlomoWaitTime = 2f;
        public MaxPeen()
        {
            Logger.LogInfo("Loading Max Peen");
        }

        private void Awake()
        {
            Logger.LogInfo("Successfully loaded Max Peen!");
        }

        private void Update()
        {
            //if (Time.timeScale != 1)
            //{
            // return time to normal at a gradient
            // Time.timeScale += (1f / SlowdownLength) * Time.unscaledDeltaTime;
            // Time.fixedDeltaTime = Time.timeScale / SteamVR.instance.hmd_DisplayFrequency;
            // Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            // }

            if (GM.CurrentMovementManager.Hands[1].Input.AXButtonDown)
            {
                Logger.LogInfo("Detected Right A Press!");
                SlomoStatus = "Slowing";
            }

            if (SlomoStatus == "Slowing")
            {
                //Logger.LogInfo("Slowing!");
                SlomoScaleDown();
            }

            if (SlomoStatus == "Wait")
            {
                //Logger.LogInfo("Waiting!");
                SlomoStatus = "Paused";
                StartCoroutine(SlomoWait());
            }

            if (SlomoStatus == "Return")
            {
                //Logger.LogInfo("Returning!");
                SlomoReturn();
            }

            if (Time.timeScale == 1)
            {
                SlomoStatus = ("Off");
            }

        }

        private void DoSlomotion()
        {
            Time.timeScale = SlowdownFactor;
            Time.fixedDeltaTime = Time.timeScale / SteamVR.instance.hmd_DisplayFrequency;
            SlomoStatus = "Wait";
        }

        private void SlomoScaleDown()
        {
            if (Time.timeScale > MaxSlomo)
            {
                Time.timeScale -= (1) * Time.unscaledDeltaTime;
                Time.fixedDeltaTime = Time.timeScale / SteamVR.instance.hmd_DisplayFrequency;
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            }

            if (Time.timeScale <= MaxSlomo)
            {
                SlomoStatus = ("Wait");
            }
        }

        private void SlomoReturn()
        {
            if (Time.timeScale != 1)
            {
                Time.timeScale += (1f / 3f) * Time.unscaledDeltaTime;
                Time.fixedDeltaTime = Time.timeScale / SteamVR.instance.hmd_DisplayFrequency;
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            }
        }

        IEnumerator SlomoWait()
        {
            yield return new WaitForSecondsRealtime(SlomoWaitTime);
            SlomoStatus = "Return";
        }

    }
}