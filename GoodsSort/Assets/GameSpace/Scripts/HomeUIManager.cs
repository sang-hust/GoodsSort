using System.Collections;
using TMPro;
using UnityEngine;

public class HomeUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text clientHeart, clientLevel;
    private WrapperData _wrapperData;
    private void Start()
    {
        GameDataCenter.Instance.GetOrCreate(out _wrapperData);
        
        SetUIHome();
        StartCoroutine(IEStart());
    }

    private void SetUIHome()
    {
        clientHeart.text = _wrapperData.Client.heart.ToString();
        clientLevel.text = "Play level: " + (_wrapperData.Client.level + 1);
    }
    
    private void OnDisable()
    {
        
    }

    private IEnumerator IEStart()
    {
        yield return new WaitForSeconds(3f);
        OnClickPlay();
    }

    public void OnClickPlay()
    {
        if (_wrapperData.Client.heart < 1)
        {
            NotifyManager.Instance.ShowWarning("Not enough heart");

            return;
        }
        
        _wrapperData.ModifyHeart(-1);
        LoadSceneManager.Instance.LoadScene("InGameScene");
    }
}
