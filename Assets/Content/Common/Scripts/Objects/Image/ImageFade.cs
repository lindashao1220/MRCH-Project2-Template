using System;
using System.Collections;
using System.Collections.Generic;
using Immersal;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


/// <summary>
/// This class provides functionality to fade in `RawImage` or `Image` components over a specified duration.
/// </summary>

public abstract class ImageFade : MonoBehaviour
{
    [ReadOnly,SerializeField] protected RawImage rawImage;
    [ReadOnly,SerializeField] protected Image image;
    [ReadOnly,SerializeField] protected SpriteRenderer spriteRenderer;

    protected bool _rawImageExists;
    protected bool _imageExists;
    protected bool _spriteRendererExists;
    [SerializeField] protected float secondsToFade = 0.5f;
    [Space(10), SerializeField] protected bool fadeInOnAwake = true;

    protected virtual void Awake()
    {
        var activeCount = 0;
        rawImage = GetComponent<RawImage>();
        image = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(rawImage)
        {
            _rawImageExists = true;
            activeCount++;
        }
        
        if(image)
        {
            _imageExists = true;
            activeCount++;
        }
        if(spriteRenderer)
        {
            _spriteRendererExists = true;
            activeCount++;
        }

        if (activeCount == 0)
        {
            Debug.LogWarning("No Image component found in " + gameObject.name);
        }
        else if (activeCount > 1)
        {
            Debug.LogWarning("Multiple Image components found in " + gameObject.name);
        }
    }

    protected virtual void OnEnable()
    {
        if(fadeInOnAwake)
        {
           FadeIn();
        }
    }

    public virtual void FadeIn()
    {
        if(_rawImageExists)
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0f);
        if(_imageExists)
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        if(_spriteRendererExists)
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        StartCoroutine(Fade(true));
    }

    public virtual void Fadeout()
    {
        if(_rawImageExists)
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 1f);
        if(_imageExists)
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        if(_spriteRendererExists)
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        StartCoroutine(Fade(false));    
    }
    
    protected virtual  IEnumerator Fade(bool target)
    {  
        var t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / secondsToFade;
            if(_rawImageExists)
            {
                var color = rawImage.color;
                color.a = Mathf.Lerp(target ? 0f : 1f, target ? 1f : 0f, t);
                rawImage.color = color;
            }
            else if(_imageExists)
            {
                var color = image.color;
                color.a = Mathf.Lerp(target ? 0f : 1f, target ? 1f : 0f, t);
                image.color = color;
            }
            else if(_spriteRendererExists)
            {
                var color = spriteRenderer.color;
                color.a = Mathf.Lerp(target ? 0f : 1f, target ? 1f : 0f, t);
                spriteRenderer.color = color;
            }
            yield return null;
        }
    }
}
