using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugPanel : MonoBehaviour
{
    public DebugPanel Instance;

    [SerializeField] private GameObject debugPanel;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private TextMeshProUGUI movementSpeedText;
    [SerializeField] private TextMeshProUGUI jumpForceText;
    [SerializeField] private TextMeshProUGUI jumpCutoffTimeText;
    [SerializeField] private TextMeshProUGUI velocityText;
    [SerializeField] private TextMeshProUGUI gravityScaleText;

    [SerializeField] private Slider movementSpeedSlider;
    [SerializeField] private Slider jumpForceSlider;
    [SerializeField] private Slider jumpCutoffTimeSlider;
    [SerializeField] private Slider gravityScaleSlider;

    [SerializeField] private int fps = 60;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
		{
            Instance = this;
		}
		else
		{
            Debug.Log("There may only be one " + typeof(DebugPanel));
            Destroy(this);
		}

        movementSpeedSlider.value = playerController.movementSpeed;
        jumpForceSlider.value = playerController.jumpForce;
        jumpCutoffTimeSlider.value = playerController.jumpCutoffTime;
        gravityScaleSlider.value = playerController.rb2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.F3))
		{
            debugPanel.SetActive(!debugPanel.activeInHierarchy);
		}

		if (debugPanel.activeInHierarchy)
		{
            movementSpeedText.text = "Movement Speed: " + playerController.movementSpeed;
            jumpForceText.text = "Jump Force: " + playerController.jumpForce;
            jumpCutoffTimeText.text = "Jump Cutoff Time: " + playerController.jumpCutoffTime;
            gravityScaleText.text = "Gravity Scale: " + playerController.rb2D.gravityScale;
            velocityText.text = "Velocity: " + playerController.rb2D.velocity;
		}

        Application.targetFrameRate = fps;
    }

    public void changeMovementSpeed(float value)
	{
        playerController.movementSpeed = value;
	}
    public void changeJumpForce(float value)
    {
        playerController.jumpForce = value;
    }
    public void changeJumpCutoffTime(float value)
    {
        playerController.jumpCutoffTime = value;
    }
    public void changeGravityScale(float value)
	{
        playerController.rb2D.gravityScale = value;
	}
}
