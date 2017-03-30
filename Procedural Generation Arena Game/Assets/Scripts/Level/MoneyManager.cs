using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoneyManager : MonoBehaviour {

    // Don't bother looking here :'D het was 3 uur 's nachts en iets werktte niet :<

    public float currentMoney;

    public float betAI1;
    public float betAI2;

    private float estValueAI1;
    private float estValueAI2;

    private float betValueAI1;
    private float betValueAI2;

    private float amountBetOnAI1;
    private float amountBetOnAI2;

    private Text betValueAI1Text;
    private Text betValueAI2Text;
    private Text estValueAI1Text;
    private Text estValueAI2Text;
    private Text betAI1Text;
    private Text betAI2Text;
    private Text currentMoneyText;
    private Text AI1name;
    private Text AI2name;

    private Button buttonAI1;
    private Button buttonAI2;

    private WeaponGenerationForGameplay ai01stats;
    private WeaponGenerationForGameplay ai02stats;

    private static MoneyManager instance = null;

    public static MoneyManager Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        if (instance != null) {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {
        if (SceneManager.GetActiveScene().name == "WorldGen") {

            GetComponents();

            ai01stats = GameObject.Find("Weapon_01").GetComponent<WeaponGenerationForGameplay>();
            ai02stats = GameObject.Find("Weapon_02").GetComponent<WeaponGenerationForGameplay>();

            estValueAI1 = ai01stats.itemValue + Random.Range(-ai01stats.itemValue / 3f, ai01stats.itemValue / 3f);
            estValueAI2 = ai02stats.itemValue + Random.Range(-ai02stats.itemValue / 3f, ai02stats.itemValue / 3f);
        }

    }

    private void GetComponents() {

        betValueAI1Text = GameObject.Find("Bet Value AI1").GetComponent<Text>();
        betValueAI2Text = GameObject.Find("Bet Value AI2").GetComponent<Text>();

        estValueAI1Text = GameObject.Find("Est Value AI1").GetComponent<Text>();
        estValueAI2Text = GameObject.Find("Est Value AI2").GetComponent<Text>();

        AI1name = GameObject.Find("Name AI1").GetComponent<Text>();
        AI2name = GameObject.Find("Name AI2").GetComponent<Text>();

        betAI1Text = GameObject.Find("Bet AI1").GetComponent<Text>();
        betAI2Text = GameObject.Find("Bet AI2").GetComponent<Text>();

        currentMoneyText = GameObject.Find("CurrentMoney").GetComponent<Text>();

        buttonAI1 = GameObject.Find("Button AI1").GetComponent<Button>();
        buttonAI2 = GameObject.Find("Button AI2").GetComponent<Button>();

        estValueAI1 = ai01stats.itemValue + Random.Range(-ai01stats.itemValue / 3f, ai01stats.itemValue / 3f);
        estValueAI2 = ai02stats.itemValue + Random.Range(-ai02stats.itemValue / 3f, ai02stats.itemValue / 3f);

        AI1name.text = NameDatabase.Instance.GenerateName();
        AI2name.text = NameDatabase.Instance.GenerateName();

    }

    private void Update() {

        if (SceneManager.GetActiveScene().name == "WorldGen") {
            if (GameObject.Find("Weapon_01") != null) {
                if (GameObject.Find("Weapon_01").activeInHierarchy) {
                    ai01stats = GameObject.Find("Weapon_01").GetComponent<WeaponGenerationForGameplay>();
                    ai02stats = GameObject.Find("Weapon_02").GetComponent<WeaponGenerationForGameplay>();
                }
            }

            if (instance != null && instance != this) {
                Destroy(this.gameObject);
            }

            // check if it doesn't have the components (? for some reason not saving on scene reload.. durr)
            if (betValueAI1Text == null) {
                GetComponents();
                buttonAI1.GetComponent<SetOnBtnClickValues>().AddFunctionality();
                buttonAI2.GetComponent<SetOnBtnClickValues>().AddFunctionality();

            }

            betValueAI1 = estValueAI2 / 100f;
            betValueAI2 = estValueAI1 / 100f;

            if (currentMoney < 100) {
                buttonAI1.interactable = false;
                buttonAI2.interactable = false;
            }
            else {
                buttonAI1.interactable = true;
                buttonAI2.interactable = true;
            }

            betValueAI1Text.text = "100:" + betValueAI1.ToString("F0");
            betValueAI2Text.text = "100:" + betValueAI2.ToString("F0");

            estValueAI1Text.text = "Est. Value: " + estValueAI1.ToString("F0");
            estValueAI2Text.text = "Est. Value: " + estValueAI2.ToString("F0");

            betAI1Text.text = "Current Bet: $" + betAI1.ToString("F0");
            betAI2Text.text = "Current Bet: $" + betAI2.ToString("F0");

            currentMoneyText.text = "$ " + currentMoney.ToString("F0");

        }
    }

    public void AdjustMoney(float value) {
        currentMoney += value;
    }

    public void AdjustBetAI01() {
        betAI1 += betValueAI1;
        amountBetOnAI1 += 100f;
    }

    public void AdjustBetAI02() {
        betAI2 += betValueAI2;
        amountBetOnAI2 += 100f;
    }

    public void AIWon(int nr) {
        if (nr == 1) {
            currentMoney += betAI1;
            currentMoney += amountBetOnAI1;

        } else {
            currentMoney += betAI2;
            currentMoney += amountBetOnAI1;
        }

        amountBetOnAI1 = 0;
        amountBetOnAI2 = 0;
        betAI1 = 0;
        betAI2 = 0;
    }
}
