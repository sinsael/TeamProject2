using UnityEngine;
using UnityEngine.UI;

public class BookUIManager : MonoBehaviour
{
    [SerializeField] Image page;
    [SerializeField] Sprite[] pageSprites;
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;

    private int _currentIndex = 0;
    private void Start()
    {
        nextButton.onClick?.AddListener(OnNextBtnClick);
        prevButton.onClick?.AddListener(OnPrevBtnClick);

        UpdatePage();
    }

    [ContextMenu("test next")]
    private void OnNextBtnClick()
    {
        if(_currentIndex < pageSprites.Length -1)
        {
            _currentIndex++;
            UpdatePage();
        }
    }

    [ContextMenu("test prev")]
    private void OnPrevBtnClick()
    {
        if(_currentIndex > 0)
        {
            _currentIndex--;
            UpdatePage();
        }
    }

    private void UpdatePage()
    {
        page.sprite = pageSprites[_currentIndex];
        prevButton.gameObject?.SetActive(_currentIndex > 0);
        nextButton.gameObject?.SetActive(_currentIndex < pageSprites.Length - 1);
    }
}
