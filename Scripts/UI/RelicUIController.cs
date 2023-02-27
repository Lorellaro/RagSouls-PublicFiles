using MainGame.ColoursPalette;
using MainGame.Relics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame.UI
{
    public class RelicUIController : MonoBehaviour
    {
        [SerializeField] public RelicData relicData;
        [SerializeField] List<Image> borderRarityLight;
        [SerializeField] List<Image> borderRarityDark;
        [SerializeField] TextMeshProUGUI relicNameText;
        [SerializeField] TextMeshProUGUI rarityText;
        [SerializeField] TextMeshProUGUI relicDescriptionLong;
        [SerializeField] TextMeshProUGUI relicDescriptionEffect;
        [SerializeField] float fadeSpeed;

        CanvasGroup canvasGroup;
        Coroutine fadeInCoroutine;
        Coroutine fadeOutCoroutine;

        //Called from Relic Base Class
        public void Begin()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            SetupColours();
            SetupDescriptions();
        }

        //Sets up the text displayed
        private void SetupDescriptions()
        {
            //Setup Name
            relicNameText.SetText(relicData.relicName);

            //Long Description
            relicDescriptionLong.SetText(relicData.relicDescription);

            //If relic gives beneficial stats then make it green
            if (relicData.isEffectGood)
            {
                relicDescriptionEffect.color = new Color(ColourPallete.Emerald.r / 255, ColourPallete.Emerald.g / 255, ColourPallete.Emerald.b / 255, 100);
            }
            else
            {
                relicDescriptionEffect.color = new Color(ColourPallete.Red.r / 255, ColourPallete.Red.g / 255, ColourPallete.Red.b / 255, 100);
            }

            relicDescriptionEffect.SetText(relicData.relicDescriptionShort);
        }

        //Change UI Colours depending on rarity
        private void SetupColours()
        {
            if (relicData.rarity == Rarity.Common)
            {
                //make Green
                changeBorderColour(ColourPallete.Turquoise, ColourPallete.DarkTurquoise);
                rarityText.SetText("Common");
            }

            else if(relicData.rarity == Rarity.Rare)
            {
                //make blue
                changeBorderColour(ColourPallete.Blue, ColourPallete.DarkBlue);
                rarityText.SetText("Rare");
            }

            else if(relicData.rarity == Rarity.Legendary)
            {
                //make gold
                changeBorderColour(ColourPallete.LightOrange, ColourPallete.Orange);
                rarityText.SetText("Legendary");
            }
        }

        //Loops through colour lists to make them match the relic's rarity
        private void changeBorderColour(Color LightColor, Color DarkColour)
        {
            LightColor = new Color(LightColor.r / 255, LightColor.g / 255, LightColor.b / 255, 100);
            DarkColour = new Color(DarkColour.r / 255, DarkColour.g / 255, DarkColour.b / 255, 100);

            for (int i = 0; i < borderRarityLight.Count; i++)
            {
                borderRarityLight[i].color = LightColor;
            }

            for (int i = 0; i < borderRarityDark.Count; i++)
            {
                borderRarityDark[i].color = DarkColour;
            }

            rarityText.color = LightColor;
            relicNameText.color = LightColor;
        }



        // ---Fade Handler ---
        public void FadeIn()
        {
            if(fadeInCoroutine != null) { return; }
            if(fadeOutCoroutine != null) { StopCoroutine(fadeOutCoroutine); fadeOutCoroutine = null; }

            fadeInCoroutine = StartCoroutine(FadeInEnum());
        }

        private IEnumerator FadeInEnum()
        {
            while(canvasGroup.alpha < 1)
            {
                yield return new WaitForEndOfFrame();
                canvasGroup.alpha += fadeSpeed * Time.deltaTime;
            }
        }

        public void FadeOut()
        {
            if (fadeOutCoroutine != null) { return; }
            if (fadeInCoroutine != null) { StopCoroutine(fadeInCoroutine); fadeInCoroutine = null; }

            fadeOutCoroutine = StartCoroutine(FadeOutEnum());
        }

        private IEnumerator FadeOutEnum()
        {
            while (canvasGroup.alpha > 0)
            {
                yield return new WaitForEndOfFrame();
                canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            }
        }
    }
}