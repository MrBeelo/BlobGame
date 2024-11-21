using System;
using System.Diagnostics.Tracing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BlobGame;

public class InputManager
{
    public bool DLeft {get; set;} = false;
    public bool DRight {get; set;} = false;
    public bool DUp {get; set;} = false;
    public bool DDown {get; set;} = false;
    public bool PJump {get; set;} = false;
    public bool PDash {get; set;} = false;
    public bool PFireball {get; set;} = false;
    public bool PConfirm {get; set;} = false;
    public bool PUp {get; set;} = false;
    public bool PDown {get; set;} = false;
    KeyboardState prevkstate;
    GamePadState prevgstate;

    public enum PressedDirection 
    {Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight, NA}
    public PressedDirection pressedDirection {get; set;} = PressedDirection.NA;

    public InputManager()
    {
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState kstate = Keyboard.GetState();
        JoystickState jstate = Joystick.GetState((int)PlayerIndex.One);
        GamePadState gstate = GamePad.GetState((int)PlayerIndex.One);

        DLeft = false;
        DRight = false;
        DUp = false;
        DDown = false;
        PJump = false;
        PDash = false;
        PFireball = false;
        PConfirm = false;
        PUp = false;
        PDown = false;

        if(!Main.main.TypingMode)
        {

        if(Joystick.LastConnectedIndex == 0)
        {
        
        if(kstate.IsKeyDown(Keys.A) || kstate.IsKeyDown(Keys.Left) || jstate.Axes[0] < -16384 || gstate.IsButtonDown(Buttons.DPadLeft))
        {
            DLeft = true;
        }

        if(kstate.IsKeyDown(Keys.D) || kstate.IsKeyDown(Keys.Right) || jstate.Axes[0] > 16384 || gstate.IsButtonDown(Buttons.DPadRight))
        {
            DRight = true;
        }

        if(kstate.IsKeyDown(Keys.W) || kstate.IsKeyDown(Keys.Up) || jstate.Axes[1] < -16384 || gstate.IsButtonDown(Buttons.DPadUp))
        {
            DUp = true;
        }

        if(kstate.IsKeyDown(Keys.S) || kstate.IsKeyDown(Keys.Down) || jstate.Axes[1] > 16384 || gstate.IsButtonDown(Buttons.DPadDown))
        {
            DDown = true;
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.Space) || IsKeyPressed(kstate, prevkstate, Keys.Up) || IsButtonPressed(gstate, prevgstate, Buttons.A))
        {
            PJump = true;
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.J) || IsButtonPressed(gstate, prevgstate, Buttons.X))
        {
            PDash = true;
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.F) || IsButtonPressed(gstate, prevgstate, Buttons.Y))
        {
            PFireball = true;
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.Enter) || IsButtonPressed(gstate, prevgstate, Buttons.A))
        {
            PConfirm = true;
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.W) || IsKeyPressed(kstate, prevkstate, Keys.Up) || jstate.Axes[1] < -16384 || IsButtonPressed(gstate, prevgstate, Buttons.DPadUp))
        {
            PUp = true;
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.S) || IsKeyPressed(kstate, prevkstate, Keys.Down) || jstate.Axes[1] > 16384 || IsButtonPressed(gstate, prevgstate, Buttons.DPadDown))
        {
            PDown = true;
        }
        } 
        else if(Joystick.LastConnectedIndex == -1)
        {
        if(kstate.IsKeyDown(Keys.A) || kstate.IsKeyDown(Keys.Left))
        {
            DLeft = true;
        }

        if(kstate.IsKeyDown(Keys.D) || kstate.IsKeyDown(Keys.Right))
        {
            DRight = true;
        }

        if(kstate.IsKeyDown(Keys.W) || kstate.IsKeyDown(Keys.Up))
        {
            DUp = true;
        }

        if(kstate.IsKeyDown(Keys.S) || kstate.IsKeyDown(Keys.Down))
        {
            DDown = true;
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.Space) || IsKeyPressed(kstate, prevkstate, Keys.Up))
        {
            PJump = true;
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.J))
        {
            PDash = true;
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.F))
        {
            PFireball = true;
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.Enter))
        {
            PConfirm = true;
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.W) || IsKeyPressed(kstate, prevkstate, Keys.Up))
        {
            PUp = true;
        }

        if(IsKeyPressed(kstate, prevkstate, Keys.S) || IsKeyPressed(kstate, prevkstate, Keys.Down))
        {
            PDown = true;
        }
        }

        switch(DLeft, DRight, DUp, DDown)
        {
            case (false, true, false, true):
                pressedDirection = PressedDirection.DownRight;
                break;

            case (true, false, false, true):
                pressedDirection = PressedDirection.DownLeft;
                break;

            case (false, true, true, false):
                pressedDirection = PressedDirection.UpRight;
                break;

            case (true, false, true, false):
                pressedDirection = PressedDirection.UpLeft;
                break;

            case (false, true, false, false):
                pressedDirection = PressedDirection.Right;
                break;

            case (true, false, false, false):
                pressedDirection = PressedDirection.Left;
                break;

            case (false, false, false, true):
                pressedDirection = PressedDirection.Down;
                break;

            case (false, false, true, false):
                pressedDirection = PressedDirection.Up;
                break;

            case (false, false, false, false):
                pressedDirection = PressedDirection.NA;
                break;
        }

        }

        prevkstate = kstate;
        prevgstate = gstate;
    }

    public static bool IsKeyPressed(KeyboardState kstate, KeyboardState prevkstate, Keys key)
    {
        return kstate.IsKeyDown(key) && !prevkstate.IsKeyDown(key);
    }
    public static bool IsButtonPressed(GamePadState gstate, GamePadState prevgstate, Buttons button)
    {
        return gstate.IsButtonDown(button) && !prevgstate.IsButtonDown(button);
    }

    public static char? KeyToChar(Keys key, bool shift)
    {
        bool isCapsLock = Keyboard.GetState().CapsLock;

        // Handle A-Z
        if (key >= Keys.A && key <= Keys.Z)
        {
            // Uppercase if either Shift or Caps Lock is active, but not both
            bool isUpperCase = shift ^ isCapsLock; // XOR to toggle case
            return isUpperCase ? (char)key : char.ToLower((char)key);
        }

        // Handle 0-9 (D0-D9)
        if (key >= Keys.D0 && key <= Keys.D9)
        {
            return shift ? key switch
            {
                Keys.D1 => '!',
                Keys.D2 => '@',
                Keys.D3 => '#',
                Keys.D4 => '$',
                Keys.D5 => '%',
                Keys.D6 => '^',
                Keys.D7 => '&',
                Keys.D8 => '*',
                Keys.D9 => '(',
                Keys.D0 => ')',
                _ => null
            } : (char)('0' + (key - Keys.D0));
        }

        // Handle common punctuation and symbols
        return key switch
        {
            Keys.Space => ' ',
            Keys.OemComma => shift ? '<' : ',', // Comma or Less-than
            Keys.OemPeriod => shift ? '>' : '.', // Period or Greater-than
            Keys.OemQuestion => shift ? '?' : '/', // Slash or Question Mark
            Keys.OemSemicolon => shift ? ':' : ';', // Semicolon or Colon
            Keys.OemQuotes => shift ? '"' : '\'', // Quote or Double Quote
            Keys.OemOpenBrackets => shift ? '{' : '[', // Open Bracket or Curly Brace
            Keys.OemCloseBrackets => shift ? '}' : ']', // Close Bracket or Curly Brace
            Keys.OemPipe => shift ? '|' : '\\', // Pipe or Backslash
            Keys.OemPlus => shift ? '+' : '=', // Plus or Equal
            Keys.OemMinus => shift ? '_' : '-', // Underscore or Hyphen
            //Keys.OemTilde => shift ? '~' : '`', // Tilde or Backtick
            _ => null // Unhandled key
        };
    }
}