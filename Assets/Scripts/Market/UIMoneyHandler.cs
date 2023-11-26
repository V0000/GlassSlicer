using TMPro;
using UnityEngine;

namespace Market
{
    public class UIMoneyHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetMoneyText(int moneyValue)
        {
            moneyText.text = $"▲{moneyValue.ToString()}";
        }
    
        public void PlayShakeAnimation() //проигрывается при нехватке валюты
        {
            _animator.Play("ShakeMoneyCube");

        }

    }
}
