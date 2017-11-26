using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingTextBox : MonoBehaviour {

    [SerializeField]
    private float fadeInterval = .01f;
    [SerializeField]
    private Image box;
    [SerializeField]
    private Text text;
    [SerializeField]
    private float defaultBoxColorAlpha, defaultTextColorAlpha;


    // Use this for initialization
    void Awake () {
        defaultBoxColorAlpha = box.color.a;
        defaultTextColorAlpha = text.color.a;
    }

    // Update is called once per frame
    void Update () {
		
	}

    //Box GameObject automatically starts fading whenever enabled, then disables itself
    private void OnEnable()
    {
        //This shuts down the text box immediately if its parent was closed before it could finish fading before.
        if (box.color.a < defaultBoxColorAlpha)
        {
            StopCoroutine(FadeOut(0));
            box.gameObject.SetActive(false);
        }

        //Reset colors to default
        box.color = new Vector4 (box.color.r, box.color.g, box.color.b, defaultBoxColorAlpha);
        text.color = new Vector4(text.color.r, text.color.g, text.color.b, defaultTextColorAlpha);
        //box can apparently stay disabled, even though this OnEnable is for enabling it.....
        if (box.gameObject.activeSelf)
            StartCoroutine(FadeOut(fadeInterval));
    }

    IEnumerator FadeOut (float fadeInterval)
    {
        //Amount to subtract Box and Text color alphas
        float fadeAmt = .01f;
        //Hold textbox from fading for a bit, so the player can read it
        yield return new WaitForSeconds(fadeInterval * 20);

        while (box.color.a > 0)
        {

            box.color = new Vector4(box.color.r, box.color.g, box.color.b, box.color.a - fadeAmt);
            text.color = new Vector4(text.color.r, text.color.g, text.color.b, text.color.a - fadeAmt);
            yield return new WaitForSeconds(fadeInterval);
        }
        //Only disables box because it's the parent
        box.gameObject.SetActive(false);
        yield return new WaitForSeconds(0);
    }
}
