using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class GetInputOfficer : SerializedMonoBehaviour
{
    public bool touchable = false;
    
    private bool previousMouseState = false;
    [SerializeField] private bool isFixedUpdate = false;
    
    public delegate void InputTypeProcess(bool touchStart, bool touchMoved, bool touchEnded, Vector2 touchPos);

    //public event InputTypeProcess InputDrag;
    //public event InputTypeProcess InputHold;
    //public event InputTypeProcess InputSwipe;
    public event InputTypeProcess InputSlide;
    public event InputTypeProcess InputTouch;
    //public event InputTypeProcess InputJoystick;

    [SerializeField] int UILayer;
    
    private void Update()
    {
        if (!isFixedUpdate && touchable)
        {
            InputControl(); 
        }
    }

    void FixedUpdate()
    {
        if (isFixedUpdate && touchable)
        {
            InputControl();
        }
    }
    void InputControl()
    {
        if (Application.isMobilePlatform)
        {
            TouchInput();
        }
        else 
        {
            MouseInput();
        }
    }

    void MouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (previousMouseState == false)
            {
                previousMouseState = true;
                InputManagerInjector(true, false, false, Input.mousePosition);
            }
            else
            {
                InputManagerInjector(false, true, false, Input.mousePosition);
            }
        }
        else
        {
            if (previousMouseState == true)
            {
                previousMouseState = false;
                InputManagerInjector(false, false, true, Vector2.zero);
            }
        }
    }
    void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                InputManagerInjector(true, false, false, touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                InputManagerInjector(false, true, false, touch.position);
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                InputManagerInjector(false, true, false, touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                InputManagerInjector(false, false, true, touch.position);
            }
        }
    }

    void InputManagerInjector(bool touchStart, bool touchMoved, bool touchEnded, Vector2 touchPos)
    {
        InputSlide(touchStart, touchMoved, touchEnded, touchPos);
    }

}
