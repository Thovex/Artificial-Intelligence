using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetOnBtnClickValues : MonoBehaviour {

    public int btnNr;

    private Button button;
    private bool hasFunctionality = false;

    private void Start() {
        button = GetComponent<Button>();

        AddFunctionality();
    }

    public void AddFunctionality() {
        if (!hasFunctionality) {
            if (btnNr == 1) {
                button.onClick.AddListener(delegate () {
                    MoneyManager.Instance.AdjustMoney(-100);
                    MoneyManager.Instance.AdjustBetAI01();
                });
            }
            else {
                button.onClick.AddListener(delegate () {
                    MoneyManager.Instance.AdjustMoney(-100);
                    MoneyManager.Instance.AdjustBetAI02();
                });
            }
            hasFunctionality = true;
        }
    }
}
