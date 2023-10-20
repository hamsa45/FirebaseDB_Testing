using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MobileBackButtonPress : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    private string OriginalText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            escapeClicked();
		}

    }

	private void escapeClicked()
	{
        if (OriginalText == null)
		{
            OriginalText = buttonText.text;
		}
        buttonText.text = "escapeClicked";
        StartCoroutine(resetText());
	}

    private IEnumerator  resetText()
	{
		yield return new WaitForSeconds(1f);
        buttonText.text = OriginalText;
	}

}
