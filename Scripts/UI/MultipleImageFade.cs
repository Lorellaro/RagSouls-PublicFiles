using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MultipleImageFade : MonoBehaviour
{
	[SerializeField] float fadeTime;
	[SerializeField, Range(0, 1)] List<float> imageColourMaxAlpha = new List<float>();

	// the image you want to fade, assign in inspector
	public List<Image> imgs = new List<Image>();

	List<Color> imgColors =  new List<Color>();
	List<float> currentAlpha = new List<float>();
	[SerializeField] bool fadeAway;

	private void Awake()
	{
		for(int i = 0; i < imgs.Count; i++)
		{
			imgColors.Add(imgs[i].color);
			currentAlpha.Add(0f);
		}
	}

	private void Update()
	{
		if (fadeAway)
		{
			for(int i = 0; i < imgs.Count; i++)
			{
				if (imgs[i].color.a < imageColourMaxAlpha[i])//Fade in if not fully faded in
				{
					currentAlpha[i] += Time.deltaTime * fadeTime;
					imgs[i].color = new Color(imgColors[i].r, imgColors[i].g, imgColors[i].b, currentAlpha[i]);
				}
			}
		}
		else
		{
			for(int i = 0; i < imgs.Count; i++)
			{
				if (imgs[i].color.a > 0)//fade out if not fully faded out
				{
					currentAlpha[i] -= Time.deltaTime * fadeTime;
					imgs[i].color = new Color(imgColors[i].r, imgColors[i].g, imgColors[i].b, currentAlpha[i]);
				}
			}
		}
	}

	public void fadeIn()
	{
		fadeAway = true;
	}

	public void fadeOut()
	{
		fadeAway = false;
	}

}
