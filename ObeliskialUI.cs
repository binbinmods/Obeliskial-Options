﻿using Steamworks.ServerList;
using System;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace Obeliskial_Options
{

    public class ObeliskialUI : MonoBehaviour
    {
        public static UIBase uiBase { get; private set; }
        public static RectTransform NavBarRect;
        public static GameObject medsNav;
        public static ButtonRef settingsBtn;
        public static ButtonRef userToolsBtn;
        public static ButtonRef devToolsBtn;
        public static ButtonRef hideBtn;
        public static GameObject lockAtOGO;
        public static Toggle lockAtOToggle;
        public static bool ShowUI
        {
            get => uiBase != null && uiBase.Enabled;
            set
            {
                if (uiBase == null || !uiBase.RootObject || uiBase.Enabled == value)
                    return;

                UniversalUI.SetUIActive(Plugin.ModGUID, value);
            }
        }
        internal static void InitUI()
        {

            uiBase = UniversalUI.RegisterUI(Plugin.ModGUID, UpdateUI);
            //MedsUI MedsPanel = new MedsUI(uiBase);
            medsNav = UIFactory.CreateUIObject("medsNavbar", uiBase.RootObject);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(medsNav, false, false, true, true, 5, 4, 4, 4, 4, TextAnchor.UpperCenter);
            medsNav.AddComponent<Image>().color = new Color(0.03f, 0.008f, 0.05f, 0.9f);
            NavBarRect = medsNav.GetComponent<RectTransform>();
            NavBarRect.pivot = new Vector2(1f, 0.5f);

            NavBarRect.anchorMin = new Vector2(1f, 0.5f);
            NavBarRect.anchorMax = new Vector2(1f, 0.5f);
            NavBarRect.anchoredPosition = new Vector2(NavBarRect.anchoredPosition.x, NavBarRect.anchoredPosition.y);
            NavBarRect.sizeDelta = new(100f, 225f);
            Text title = UIFactory.CreateLabel(medsNav, "Title", "Obeliskial\nOptions\nv" + Plugin.ModVersion, TextAnchor.UpperCenter);
            UIFactory.SetLayoutElement(title.gameObject, minWidth: 100);

            // ButtonRef tempBtn = UIFactory.CreateButton(medsNav, "tempButton", "TEST");
            // UIFactory.SetLayoutElement(tempBtn.Component.gameObject, minWidth: 80, minHeight: 30, flexibleWidth: 0);
            // RuntimeHelper.SetColorBlock(tempBtn.Component, new Color(0.22f, 0.54f, 0.22f), new Color(0.15f, 0.71f, 0.1f), new Color(0.08f, 0.5f, 0.06f));

            // tempBtn.OnClick += CaptureEvent;



            settingsBtn = UIFactory.CreateButton(medsNav, "settingsButton", "Settings");
            UIFactory.SetLayoutElement(settingsBtn.Component.gameObject, minWidth: 85, minHeight: 30, flexibleWidth: 0);


            userToolsBtn = UIFactory.CreateButton(medsNav, "userToolsBtn", "User Tools");
            UIFactory.SetLayoutElement(userToolsBtn.Component.gameObject, minWidth: 85, minHeight: 30, flexibleWidth: 0);

            devToolsBtn = UIFactory.CreateButton(medsNav, "devToolsBtn", "Dev Tools");
            UIFactory.SetLayoutElement(devToolsBtn.Component.gameObject, minWidth: 85, minHeight: 30, flexibleWidth: 0);


            hideBtn = UIFactory.CreateButton(medsNav, "hideBtn", "Hide (F1)");
            UIFactory.SetLayoutElement(hideBtn.Component.gameObject, minWidth: 85, minHeight: 30, flexibleWidth: 0);

            lockAtOGO = UIFactory.CreateToggle(medsNav, "disableButtonsToggle", out lockAtOToggle, out Text lockAtOText);
            lockAtOText.text = "Lock AtO";
            lockAtOToggle.isOn = false;
            UIFactory.SetLayoutElement(lockAtOGO, minWidth: 85, minHeight: 20);
            //medsNav.

            Canvas.ForceUpdateCanvases();
            ShowUI = false;
            UniversalUI.SetUIActive(Plugin.ModGUID, false);
            Plugin.Log.LogInfo($"UI... created?!");
        }
        internal static void UpdateUI()
        {

        }

        void OnGUI()
        {
            Plugin.Log.LogInfo(Event.current.ToString());
        }

        static void CaptureEvent()
        {
            Plugin.Log.LogInfo(Event.current.ToString());
            Event.current.Use();
            Plugin.Log.LogInfo(Event.current.ToString());
        }

    }

}