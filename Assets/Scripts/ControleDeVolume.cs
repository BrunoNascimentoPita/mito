using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControleDeVolume : MonoBehaviour
{
    public AudioSource fundoMusical;
    public TMP_Text textVolume;

    public void Volume(float value)
    {
        fundoMusical.volume = value;
    }

    public void FixedUpdate()
    {
       textVolume.text = (fundoMusical.volume * 100).ToString("F0");
 }
}




