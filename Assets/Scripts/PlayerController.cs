using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [Header("Reference Values")]
    public Camera MainCamera;
    [SerializeField] private Transform recoilObj;

    [Header("Audio Values")]
    [SerializeField] private AudioClip walkTile;
    [SerializeField] private AudioSource stepsAudio;

    [Header("Player Values")]
    [SerializeField] private float mouseSensitivity;

    [SerializeField] private float movementSpeed;


    [Header("Headbob Values")]
    [SerializeField] private float walkingBobbingSpeed;

    [SerializeField] private float bobbingAmount;

    private float _defaultPosY;

    private float _timer;
    private CharacterController _characterController;

    private float _mouseX;

    private float _mouseY;

    private float _duckMoveSpeed = 6f;

    private float _playerViewYOffset = 0.6f;
    private float _recoilReturnSpeed = 8f;

    private float _horizontal;

    private float _vertical;

    private Vector3 _moveDirection = Vector3.zero;

    private Vector3 __xRotateVector;

    private Vector3 _eulerRotation = Vector3.zero;

    private Vector3 _recoil = Vector3.zero;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Application.targetFrameRate = 300;
        QualitySettings.vSyncCount = 0;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        MainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + _playerViewYOffset, transform.position.z);
        _defaultPosY = recoilObj.localPosition.y;
    }

    private void Update()
    {

        HandleHeadBob();
        HandleMovement();
        HandleCameraRotation();
        HandleAudioSteps();
    }
    private float ConvertAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f)
        {
            angle -= 360f;
        }
        return angle;
    }
    private void HandleCameraRotation(float yClampMin = 85f, float yClampMax = 85f, float xClampMin = 181f, float xClampMax = 181f)
    {
        _mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        _mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        float num = ConvertAngle(MainCamera.transform.localEulerAngles.x);
        if (num > 85f)
        {
            num = 85f;
            if (_mouseY < 0f)
            {
                _mouseY = 0f;
            }
        }
        else if (num < -85f)
        {
            num = -85f;
            if (_mouseY > 0f)
            {
                _mouseY = 0f;
            }
        }
        ClampXAxisRotationToValue(num);
        _recoil.z = _horizontal * -2f;
        recoilObj.transform.localRotation = Quaternion.RotateTowards(recoilObj.transform.localRotation, Quaternion.Euler(_recoil), _recoilReturnSpeed * Time.deltaTime);
        MainCamera.transform.Rotate(Vector3.left * _mouseY);
        transform.Rotate(Vector3.up * _mouseX);
        __xRotateVector = transform.localRotation.ToEulerAngles() * 57.29578f;
        __xRotateVector.y = Mathf.Clamp(__xRotateVector.y, 0f - xClampMin, xClampMax);
        transform.localEulerAngles = __xRotateVector;
    }

    private void ClampXAxisRotationToValue(float value)
    {
        _eulerRotation = MainCamera.transform.localEulerAngles;
        _eulerRotation.x = value;
        _eulerRotation.z = 0f;
        _eulerRotation.y = 0f;
        MainCamera.transform.localEulerAngles = _eulerRotation;
    }
    private void HandleHeadBob()
    {
        if (Mathf.Abs(_characterController.velocity.x) > 0.1f || Mathf.Abs(_characterController.velocity.z) > 0.1f)
        {
            _timer += Time.deltaTime * walkingBobbingSpeed;
            recoilObj.localPosition = new Vector3(recoilObj.localPosition.x, _defaultPosY + Mathf.Sin(_timer) * bobbingAmount, recoilObj.localPosition.z);
        }
        else
        {
            _timer = 0f;
            recoilObj.localPosition = new Vector3(recoilObj.localPosition.x, Mathf.Lerp(recoilObj.localPosition.y, _defaultPosY, Time.deltaTime * walkingBobbingSpeed), recoilObj.localPosition.z);
        }
    }

    private void HandleAudioSteps()
    {
        stepsAudio.volume = Mathf.Clamp(_characterController.velocity.magnitude / 3.2f, 0f, 0.3f);
        if (stepsAudio.clip != walkTile)
        {
            stepsAudio.clip = walkTile;
            stepsAudio.Play();
        }
    }
    private void HandleMovement()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, new Vector3(transform.position.x, transform.position.y + _playerViewYOffset, transform.position.z), _duckMoveSpeed * Time.deltaTime);
        Vector3 direction = new Vector3(_horizontal, 0f, _vertical);
        Vector3 vector = transform.TransformDirection(direction) * movementSpeed;
        vector = Vector3.ClampMagnitude(vector, movementSpeed);
        _moveDirection = new Vector3(vector.x, _moveDirection.y, vector.z);
        _characterController.Move(_moveDirection * Time.deltaTime);
    }



}
