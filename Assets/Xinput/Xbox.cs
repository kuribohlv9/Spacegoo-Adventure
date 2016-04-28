using UnityEngine;
using System.Collections;
using XInputDotNetPure;


public enum Player
{
    All = -1,
    One = 0,
    Two = 1,
    Three = 2,
    Four = 3
}
public enum XboxTrigger
{
    Both,
    Left,
    Right
}
public enum XboxButton
{
    A,
    B,
    Back,
    Guide,
    LeftShoulder,
    LeftStick,
    RightShoulder,
    RightStick,
    Start,
    X,
    Y,
    DPad_Up,
    DPad_Down,
    DPad_Left,
    DPad_Right
}
public enum XboxStick
{
    Both,
    Left,
    Right
}
public class Timer
{
    public bool Tick(float increment)
    {
        time += increment;

        if (time >= tickDelay)
        {
            time -= tickDelay;
            return true;
        }
        else
        {
            return false;
        }
    }

    private float time;
    private float tickDelay;

    public Timer(float tickDelay)
    {
        this.time = 0.0f;
        this.tickDelay = tickDelay;
    }
    public void Reset()
    {
        time = 0;
    }
}
public class XboxController
{
    public Player player;
    private GamePadState state;
    private GamePadState previousState;
    private Timer timer;
    private bool vibrating;

    
    public XboxController(Player player)
    {
        this.player = player;
    }

    public void Update(GamePadState newState)
    {
        SetState(newState);

        if (timer != null)
        {
            if (vibrating)
            {
                if (timer.Tick(Time.deltaTime))
                {
                    ResetVibration();
                }
            }
            else
            {
                timer.Reset();
            }
        }
        
    }
    

    //Controller methods
    public void SetState(GamePadState state)
    {
        this.previousState = this.state;
        this.state = state;
    }

    //Button state methods
    public bool GetButtonPress(XboxButton button)
    {
        switch(button)
        {
            case XboxButton.A:
                return previousState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed;
            case XboxButton.B:
                return previousState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed;
            case XboxButton.Back:
                return previousState.Buttons.Back == ButtonState.Released && state.Buttons.Back == ButtonState.Pressed;
            case XboxButton.Guide:
                return previousState.Buttons.Guide == ButtonState.Released && state.Buttons.Guide == ButtonState.Pressed;
            case XboxButton.LeftShoulder:
                return previousState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed;
            case XboxButton.LeftStick:
                return previousState.Buttons.LeftStick == ButtonState.Released && state.Buttons.LeftStick == ButtonState.Pressed;
            case XboxButton.RightShoulder:
                return previousState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed;
            case XboxButton.RightStick:
                return previousState.Buttons.RightStick == ButtonState.Released && state.Buttons.RightStick == ButtonState.Pressed;
            case XboxButton.Start:  
                return previousState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed;
            case XboxButton.X:
                return previousState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed;
            case XboxButton.Y:
                return previousState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed;
            case XboxButton.DPad_Up:
                return previousState.DPad.Up == ButtonState.Released && state.DPad.Up == ButtonState.Pressed;
            case XboxButton.DPad_Down:
                return previousState.DPad.Down == ButtonState.Released && state.DPad.Down == ButtonState.Pressed;
            case XboxButton.DPad_Left:
                return previousState.DPad.Left == ButtonState.Released && state.DPad.Left == ButtonState.Pressed;
            case XboxButton.DPad_Right:
                return previousState.DPad.Right == ButtonState.Released && state.DPad.Right == ButtonState.Pressed;
            default:
                return false;
        }
    }
    public bool GetButtonRelease(XboxButton button)
    {
        switch (button)
        {
            case XboxButton.A:
                return previousState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released;
            case XboxButton.B:
                return previousState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Released;
            case XboxButton.Back:
                return previousState.Buttons.Back == ButtonState.Pressed && state.Buttons.Back == ButtonState.Released;
            case XboxButton.Guide:
                return previousState.Buttons.Guide == ButtonState.Pressed && state.Buttons.Guide == ButtonState.Released;
            case XboxButton.LeftShoulder:
                return previousState.Buttons.LeftShoulder == ButtonState.Pressed && state.Buttons.LeftShoulder == ButtonState.Released;
            case XboxButton.LeftStick:
                return previousState.Buttons.LeftStick == ButtonState.Pressed && state.Buttons.LeftStick == ButtonState.Released;
            case XboxButton.RightShoulder:
                return previousState.Buttons.RightShoulder == ButtonState.Pressed && state.Buttons.RightShoulder == ButtonState.Released;
            case XboxButton.RightStick:
                return previousState.Buttons.RightStick == ButtonState.Pressed && state.Buttons.RightStick == ButtonState.Released;
            case XboxButton.Start:
                return previousState.Buttons.Start == ButtonState.Pressed && state.Buttons.Start == ButtonState.Released;
            case XboxButton.X:
                return previousState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Released;
            case XboxButton.Y:
                return previousState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Released;
            case XboxButton.DPad_Up:
                return previousState.DPad.Up == ButtonState.Pressed && state.DPad.Up == ButtonState.Released;
            case XboxButton.DPad_Down:
                return previousState.DPad.Down == ButtonState.Pressed && state.DPad.Down == ButtonState.Released;
            case XboxButton.DPad_Left:
                return previousState.DPad.Left == ButtonState.Pressed && state.DPad.Left == ButtonState.Released;
            case XboxButton.DPad_Right:
                return previousState.DPad.Right == ButtonState.Pressed && state.DPad.Right == ButtonState.Released;
            default:
                return false;
        }
    }
    public bool GetButtonDown(XboxButton button)
    {
        switch (button)
        {
            case XboxButton.A:
                return state.Buttons.A == ButtonState.Pressed;
            case XboxButton.B:
                return state.Buttons.B == ButtonState.Pressed;
            case XboxButton.Back:
                return state.Buttons.Back == ButtonState.Pressed;
            case XboxButton.Guide:
                return state.Buttons.Guide == ButtonState.Pressed;
            case XboxButton.LeftShoulder:
                return state.Buttons.LeftShoulder == ButtonState.Pressed;
            case XboxButton.LeftStick:
                return state.Buttons.LeftStick == ButtonState.Pressed;
            case XboxButton.RightShoulder:
                return state.Buttons.RightShoulder == ButtonState.Pressed;
            case XboxButton.RightStick:
                return state.Buttons.RightStick == ButtonState.Pressed;
            case XboxButton.Start:
                return state.Buttons.Start == ButtonState.Pressed;
            case XboxButton.X:
                return state.Buttons.X == ButtonState.Pressed;
            case XboxButton.Y:
                return state.Buttons.Y == ButtonState.Pressed;
            case XboxButton.DPad_Up:
                return state.DPad.Up == ButtonState.Pressed;
            case XboxButton.DPad_Down:
                return state.DPad.Down == ButtonState.Pressed;
            case XboxButton.DPad_Left:
                return state.DPad.Left == ButtonState.Pressed;
            case XboxButton.DPad_Right:
                return state.DPad.Right == ButtonState.Pressed;
            default:
                return false;
        }
    }

    //Axis state methods
    public Vector2 GetStick(XboxStick stick)
    {
        if (stick == XboxStick.Left)
            return new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
        else if (stick == XboxStick.Right)
            return new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
        else
        {
            Vector2 axisTotal;
            axisTotal.x = state.ThumbSticks.Left.X + state.ThumbSticks.Right.X;
            axisTotal.y = state.ThumbSticks.Left.Y + state.ThumbSticks.Right.Y;
            return axisTotal;
        }
    }
    public Vector2 GetDPad()
    {
        int up = (state.DPad.Up == ButtonState.Pressed) ? 1 : 0;
        int down = (state.DPad.Down == ButtonState.Pressed) ? 1 : 0;
        int left = (state.DPad.Left == ButtonState.Pressed) ? 1 : 0;
        int right = (state.DPad.Right == ButtonState.Pressed) ? 1 : 0;

        return new Vector2(-left + right, up - down);
    }
    public float GetTrigger(XboxTrigger trigger)
    {
        if (trigger == XboxTrigger.Left)
            return state.Triggers.Left;
        else if (trigger == XboxTrigger.Right)
            return state.Triggers.Right;
        else
            return (state.Triggers.Left + state.Triggers.Right);
    }

    //Vibration
    public void Vibrate(float leftMotor, float rightMotor, float time, bool overideExistingVibration)
    {
        if (vibrating && !overideExistingVibration)
            return;
        vibrating = true;
        timer = new Timer(time);

        PlayerIndex index = (PlayerIndex)((int)player);
        GamePad.SetVibration(index, leftMotor, rightMotor);
    }
    public void ResetVibration()
    {
        PlayerIndex index = (PlayerIndex)((int)player);
        GamePad.SetVibration(index, 0f, 0f);
        vibrating = false;
    }
}

public class Xbox : MonoBehaviour 
{
    private static XboxController[] controllers = new XboxController[4];

    void Update()
    {
        //Search for new controllers
        for (int i = 0; i < controllers.Length; i++)
        {
            //Aquire controller
            PlayerIndex index = (PlayerIndex)i;
            GamePadState controller = GamePad.GetState(index);

            if (controllers[i] == null)
            {
                //Register new controller
                if (controller.IsConnected)
                {
                    Debug.Log("Registered controller #" + (i + 1).ToString());
                    controllers[i] = new XboxController((Player)i);
                    controllers[i].SetState(controller);
                }
            }
        }

        //Update controller states
        for (int i = 0; i < controllers.Length; i++)
        {
            XboxController controller = controllers[i];
            if (controller != null)
                controller.Update(GamePad.GetState((PlayerIndex)controller.player));
        }
    }

    //Button state methods
    public static bool GetButtonPress(XboxButton button, Player player = Player.All)
    {
        if (player == Player.All)
        {
            for (int i = 0; i < controllers.Length; i++)
                if (controllers[i] != null)
                    if (controllers[i].GetButtonPress(button))
                        return true;
        }
        else
        {
            if (controllers[(int)player] != null)
                return controllers[(int)player].GetButtonPress(button);
        }
        return false;
    }
    public static bool GetButtonRelease(XboxButton button, Player player = Player.All)
    {
        if (player == Player.All)
        {
            for (int i = 0; i < controllers.Length; i++)
                if (controllers[i] != null)
                    if (controllers[i].GetButtonRelease(button))
                        return true;
        }
        else
        {
            if (controllers[(int)player] != null)
                return controllers[(int)player].GetButtonRelease(button);
        }
        return false;
    }
    public static bool GetButtonDown(XboxButton button, Player player = Player.All)
    {
        if (player == Player.All)
        {
            for (int i = 0; i < controllers.Length; i++)
                if (controllers[i] != null)
                    if (controllers[i].GetButtonDown(button))
                        return true;
        }
        else
        {
            if (controllers[(int)player] != null)
                return controllers[(int)player].GetButtonDown(button);
        }
        return false;
    }

    //Axis state methods
    public static Vector2 GetStick(XboxStick stick, Player player = Player.All)
    {
        if (player == Player.All)
        {
            Vector2 axisTotal = new Vector2();
            for (int i = 0; i < controllers.Length; i++)
                if (controllers[i] != null)
                    axisTotal += controllers[i].GetStick(stick);
            return axisTotal;
        }
        else
        {
            XboxController controller = controllers[(int)player];
            if (controller != null)
                return controller.GetStick(stick);
        }
        return Vector2.zero;
    }
    public static Vector2 GetDPad(Player player = Player.All)
    {
        if (player == Player.All)
        {
            Vector2 axisTotal = new Vector2();
            for (int i = 0; i < controllers.Length; i++)
                if (controllers[i] != null)
                    axisTotal += controllers[i].GetDPad();
            return axisTotal;
        }
        else
        {
            XboxController controller = controllers[(int)player];
            if (controller != null)
                return controller.GetDPad();
        }
        return Vector2.zero;
    }
    public static float GetTrigger(XboxTrigger trigger, Player player = Player.All)
    {
        if (player == Player.All)
        {
            float triggerTotal = 0.0f;
            for (int i = 0; i < controllers.Length; i++)
                if (controllers[i] != null)
                    triggerTotal += controllers[i].GetTrigger(trigger);
            return triggerTotal;
        }
        else
        {
            XboxController controller = controllers[(int)player];
            if (controller != null)
                return controller.GetTrigger(trigger);
        }
        return 0.0f;
    }

    //Vibration
    public void Vibrate(float leftMotor, float rightMotor, float time, Player player = Player.All, bool overideExistingVibration = false)
    {
        if (player == Player.All)
        {
            for (int i = 0; i < controllers.Length; i++)
                if (controllers[i] != null)
                    controllers[i].Vibrate(leftMotor, rightMotor, time, overideExistingVibration);
        }
        else
        {
            XboxController controller = controllers[(int)player];
            if (controller != null)
                controller.Vibrate(leftMotor, rightMotor, time, overideExistingVibration);
        }
    }
    public void ResetVibrationManually(Player player = Player.All)
    {
        if (player == Player.All)
        {
            for (int i = 0; i < controllers.Length; i++)
                if (controllers[i] != null)
                    controllers[i].ResetVibration();
        }
        else
        {
            XboxController controller = controllers[(int)player];
            if (controller != null)
                controller.ResetVibration();
        }
    }
}
