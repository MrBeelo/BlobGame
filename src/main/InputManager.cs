using Raylib_cs;
using static Raylib_cs.Raylib;

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
    public static bool isCapsLockOn = false;

    public enum PressedDirection 
    {Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight, NA}
    public PressedDirection pressedDirection {get; set;} = PressedDirection.NA;

    public InputManager()
    {
    }

    public void Update()
    {
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

        if(!Game.game.TypingMode)
        {

        int gamepad = 0;

        if(IsGamepadAvailable(gamepad))
        {

            float leftStickX = GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX);
            float leftStickY = GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY);
            //float rightStickX = GetGamepadAxisMovement(gamepad, GamepadAxis.RightX);
            //float rightStickY = GetGamepadAxisMovement(gamepad, GamepadAxis.RightY);
        
        if(IsKeyDown(KeyboardKey.A) || IsKeyDown(KeyboardKey.Left) || leftStickX < -16384 || IsGamepadButtonDown(gamepad, GamepadButton.LeftFaceLeft))
        {
            DLeft = true;
        }

        if(IsKeyDown(KeyboardKey.D) || IsKeyDown(KeyboardKey.Right) || leftStickX > 16384 || IsGamepadButtonDown(gamepad, GamepadButton.LeftFaceRight))
        {
            DRight = true;
        }

        if(IsKeyDown(KeyboardKey.W) || IsKeyDown(KeyboardKey.Up) || leftStickY < -16384 || IsGamepadButtonDown(gamepad, GamepadButton.LeftFaceUp))
        {
            DUp = true;
        }

        if(IsKeyDown(KeyboardKey.S) || IsKeyDown(KeyboardKey.Down) || leftStickY > 16384 || IsGamepadButtonDown(gamepad, GamepadButton.LeftFaceDown))
        {
            DDown = true;
        }

        if(IsKeyPressed(KeyboardKey.Space) || IsKeyPressed(KeyboardKey.Up) || IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceDown))
        {
            PJump = true;
        }

        if(IsKeyPressed(KeyboardKey.J) || IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
        {
            PDash = true;
        }

        if(IsKeyPressed(KeyboardKey.F) || IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
        {
            PFireball = true;
        }

        if(IsKeyPressed(KeyboardKey.Enter) || IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceDown))
        {
            PConfirm = true;
        }

        if(IsKeyPressed(KeyboardKey.W) || IsKeyPressed(KeyboardKey.Up) || leftStickY < -16384 || IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceUp))
        {
            PUp = true;
        }

        if(IsKeyPressed(KeyboardKey.S) || IsKeyPressed(KeyboardKey.Down) || leftStickY > 16384 || IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceDown))
        {
            PDown = true;
        }
        } 
        else if(!IsGamepadAvailable(gamepad))
        {
        if(IsKeyDown(KeyboardKey.A) || IsKeyDown(KeyboardKey.Left))
        {
            DLeft = true;
        }

        if(IsKeyDown(KeyboardKey.D) || IsKeyDown(KeyboardKey.Right))
        {
            DRight = true;
        }

        if(IsKeyDown(KeyboardKey.W) || IsKeyDown(KeyboardKey.Up))
        {
            DUp = true;
        }

        if(IsKeyDown(KeyboardKey.S) || IsKeyDown(KeyboardKey.Down))
        {
            DDown = true;
        }

        if(IsKeyPressed(KeyboardKey.Space) || IsKeyPressed(KeyboardKey.Up))
        {
            PJump = true;
        }

        if(IsKeyPressed(KeyboardKey.J))
        {
            PDash = true;
        }

        if(IsKeyPressed(KeyboardKey.F))
        {
            PFireball = true;
        }

        if(IsKeyPressed(KeyboardKey.Enter))
        {
            PConfirm = true;
        }

        if(IsKeyPressed(KeyboardKey.W) || IsKeyPressed(KeyboardKey.Up))
        {
            PUp = true;
        }

        if(IsKeyPressed(KeyboardKey.S) || IsKeyPressed(KeyboardKey.Down))
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

        if(IsKeyPressed(KeyboardKey.CapsLock))
        {
            if(isCapsLockOn)
            {
                isCapsLockOn = false;
            } else {
                isCapsLockOn = true;
            }
        }
        }
    }

    public static char? KeyToChar(KeyboardKey key, bool shift)
    {

        // Handle A-Z
        if (key >= KeyboardKey.A && key <= KeyboardKey.Z)
        {
            // Uppercase if either Shift or Caps Lock is active, but not both
            bool isUpperCase = shift ^ isCapsLockOn; // XOR to toggle case
            return isUpperCase ? (char)key : char.ToLower((char)key);
        }

        return key switch
        {
            KeyboardKey.One => shift ? '!' : '1',
            KeyboardKey.Two => shift ? '@' : '2',
            KeyboardKey.Three => shift ? '#' : '3',
            KeyboardKey.Four => shift ? '$' : '4',
            KeyboardKey.Five => shift ? '%' : '5',
            KeyboardKey.Six => shift ? '^' : '6',
            KeyboardKey.Seven => shift ? '&' : '7',
            KeyboardKey.Eight => shift ? '*' : '8',
            KeyboardKey.Nine => shift ? '(' : '9',
            KeyboardKey.Zero => shift ? ')' : '0',
            KeyboardKey.Space => ' ',
            KeyboardKey.Comma => shift ? '<' : ',', // Comma or Less-than
            KeyboardKey.Period => shift ? '>' : '.', // Period or Greater-than
            KeyboardKey.Slash => shift ? '?' : '/', // Slash or Question Mark
            KeyboardKey.Semicolon => shift ? ':' : ';', // Semicolon or Colon
            KeyboardKey.Apostrophe => shift ? '"' : '\'', // Quote or Double Quote
            KeyboardKey.LeftBracket => shift ? '{' : '[', // Open Bracket or Curly Brace
            KeyboardKey.RightBracket => shift ? '}' : ']', // Close Bracket or Curly Brace
            KeyboardKey.Backslash => shift ? '|' : '\\', // Pipe or Backslash
            KeyboardKey.Equal => shift ? '+' : '=', // Plus or Equal
            KeyboardKey.Minus => shift ? '_' : '-', // Underscore or Hyphen
            //KeyboardKey.OemTilde => shift ? '~' : '`', // Tilde or Backtick
            _ => null // Unhandled key
        };
    }
}