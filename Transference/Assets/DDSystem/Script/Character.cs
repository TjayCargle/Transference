using UnityEngine;
using UnityEngine.UI;

namespace Doublsb.Dialog
{
    [RequireComponent(typeof(Image))]
    public class Character : MonoBehaviour
    {
        public Emotion Emotion;
        public AudioClip[] ChatSE;
        public AudioClip[] CallSE;
        public Image myImage;
        public bool isFirst = true;
        private void OnEnable()
        {
            myImage = GetComponent<Image>();
        }
    }
}