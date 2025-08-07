using System.Collections;
using TMPro;
using UnityEngine;

namespace SoulsLike
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("You Died Pop Up")] [SerializeField]
        private GameObject _youDiedPopUpGameObject;

        [SerializeField] private TextMeshProUGUI _youDiedPopUpBackgroundText;
        [SerializeField] private TextMeshProUGUI _youDiedPopUpText;
        [SerializeField] private CanvasGroup _youDiedPopUpCanvasGroup; // ALLOWS US TO SET THE ALPHA TO FADE OVER TIME 

        public void SendYouDiedPopUp() {
            _youDiedPopUpGameObject.SetActive(true);
            _youDiedPopUpBackgroundText.characterSpacing = 0f;
            StartCoroutine(StretchPopUpTextOverTime(_youDiedPopUpBackgroundText, 8f, 19f));
            StartCoroutine(FadeInPopUpOverTime(_youDiedPopUpCanvasGroup, 5f));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(_youDiedPopUpCanvasGroup, 2f, 5f));
        }

        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount) {
            if (duration > 0f) {
                text.characterSpacing = 0f; // RESETS OUT CHARACTER SPACING
                var timer = 0f;

                yield return null;

                while (timer < duration) {
                    timer += Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20f));
                    yield return null;
                }
            }
        }

        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvasGroup, float duration) {
            if (duration > 0f) {
                canvasGroup.alpha = 0f;
                var timer = 0f;

                yield return null;

                while (timer < duration) {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvasGroup.alpha = 1f;
            yield return null;
        }

        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvasGroup, float duration, float delay) {
            if (duration > 0f) {
                while (delay > 0f) {
                    delay -= Time.deltaTime;
                    yield return null;
                }

                canvasGroup.alpha = 1f;
                var timer = 0f;

                yield return null;

                while (timer < duration) {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvasGroup.alpha = 0f;
            yield return null;
        }
    }
}