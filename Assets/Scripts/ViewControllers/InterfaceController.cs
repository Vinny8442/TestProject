using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Button _button;
    
    private IInterfaceControllerHandler _handler;

    // Start is called before the first frame update
    public void SetText(string value)
    {
        _text.text = value;
    }

    public void SetButtonText(string value)
    {
        _button.GetComponentInChildren<Text>().text = value;
    }

    public void SetHandler(IInterfaceControllerHandler handler)
    {
        if (_handler != null)
        {
            _button.onClick.RemoveListener(_handler.HandleButtonClick);
        }
        
        _handler = handler;
        
        if (_handler != null)
        {
            _button.onClick.AddListener(_handler.HandleButtonClick);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public interface IInterfaceControllerHandler
    {
        void HandleButtonClick();
    } 
}
