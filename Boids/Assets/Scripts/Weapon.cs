using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour {

    public int kills = 0;
    public float cooldown;
    public float zDistance = 10.0f;

    public GameObject laser;
    public GameObject boid;

    public TextMeshPro killsText;
    public TextMeshPro cooldownText;

    private float cooldownTimer;
    private bool shoot;
    private bool cd;

    private bool controlModeOn;

    public GameObject weapon;
    public GameObject hud;

    private void FixedUpdate() {

        if (cooldownTimer > 0) {
            cooldownTimer -= Time.deltaTime;
            cd = true;
        } else {
            cd = false;
            cooldownTimer = 0;
        }

        var mousePos = Input.mousePosition;
        transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, zDistance)));

        if (Input.GetMouseButtonDown(0)) {
            if (!cd) {
                shoot = true;
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            Instantiate(boid, laser.transform.position + transform.forward*5f, transform.rotation);
        }

        if (shoot) {
            StartCoroutine("Charge");
        }

        killsText.text = kills.ToString();
        cooldownText.text = cooldownTimer.ToString("F1");

        if (Input.GetKeyDown(KeyCode.C)) {
            ControlMode();
        }
    }

    private IEnumerator Charge() {
        laser.transform.localScale += Vector3.forward * 5f;
        yield return new WaitForSeconds(2);
        shoot = false;
        laser.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        StopCoroutine("Charge");
        cooldownTimer = cooldown;
    }

    private void ControlMode() {
        if (!controlModeOn) {
            controlModeOn = true;
            weapon.SetActive(false);
            hud.SetActive(true);
        } else {
            controlModeOn = false;
            weapon.SetActive(true);
            hud.SetActive(false);
        }
    }
}

