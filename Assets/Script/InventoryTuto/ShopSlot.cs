using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] Image image;//ΩΩ∑‘¿Ã ∂ÁøÔ ¿ÃπÃ¡ˆ
    
    Sprite _sprite;
    public Sprite sprite
    {
        get { return _sprite; }
        set
        {
            _sprite = value;
            if (_sprite != null)
            {
                image.sprite = _sprite;
                image.color = new Color(1, 1, 1, 1);
                
            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }

}
