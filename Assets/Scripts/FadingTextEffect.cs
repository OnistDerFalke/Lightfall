using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingTextEffect : MonoBehaviour
{
    public Vector2 fadeScope;
    public float fadeSpeed;
    
    private float currentScale;
    private bool isFadingUp;
    
    void Start()
    {
        currentScale = 1f;
        isFadingUp = true;
    }
    
    void Update()
    {
        ControlFadingDirection();
        Fade();
    }

    private void Fade()
    {
        gameObject.transform.localScale = Vector3.one * currentScale;
        if (isFadingUp) currentScale += fadeSpeed * Time.deltaTime;
        else currentScale -= fadeSpeed * Time.deltaTime;
    }

    private void ControlFadingDirection()
    {
        if (currentScale > fadeScope.y && isFadingUp)
            isFadingUp = false;
        else if (currentScale < fadeScope.x && !isFadingUp)
            isFadingUp = true;
    }
}
