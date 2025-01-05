using System.Diagnostics;
using System.Numerics;
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
    public bool PBack {get; set;} = false;
    public bool PUp {get; set;} = false;
    public bool PDown {get; set;} = false;
    public static bool isCapsLockOn = false;
    public List<int> gamepads = new();
    public Vector2 mouse = GetMousePosition();
    public Vector2 vMouse = Vector2.Zero;
    float leftStickX = 0.0f;
    float leftStickY = 0.0f;

    public enum PressedDirection 
    {Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight, NA}
    public PressedDirection pressedDirection {get; set;} = PressedDirection.NA;

    public InputManager()
    {
        
    }

    public void Update()
    {
        for(int i = 0; i < 16; i++)
        {
            if(IsGamepadAvailable(i))
            {
                gamepads.Add(i);
            } else {
                gamepads.Remove(i);
            }
        }

        DLeft = false;
        DRight = false;
        DUp = false;
        DDown = false;
        PJump = false;
        PDash = false;
        PFireball = false;
        PConfirm = false;
        PBack = false;
        PUp = false;
        PDown = false;

        mouse =  GetMousePosition();
        float scale = Math.Min((float)GetScreenWidth()/Settings.SimulationSize.X, (float)GetScreenHeight()/Settings.SimulationSize.Y);
        vMouse.X = (mouse.X - (GetScreenWidth() - (Settings.SimulationSize.X*scale))*0.5f)/scale;
        vMouse.Y = (mouse.Y - (GetScreenHeight() - (Settings.SimulationSize.Y*scale))*0.5f)/scale;
        vMouse = Vector2.Clamp(vMouse, Vector2.Zero, Settings.SimulationSize.ToVector2());

        if(!Game.game.TypingMode)
        {

        foreach(int gamepad in gamepads)
        {

        if(IsGamepadAvailable(gamepad))
        {
            leftStickX = GetGamepadAxisMovement(gamepad, GamepadAxis.LeftX);
            leftStickY = GetGamepadAxisMovement(gamepad, GamepadAxis.LeftY);
            //float rightStickX = GetGamepadAxisMovement(gamepad, GamepadAxis.RightX);
            //float rightStickY = GetGamepadAxisMovement(gamepad, GamepadAxis.RightY);
        
        if(leftStickX < -0.5 || IsGamepadButtonDown(gamepad, GamepadButton.LeftFaceLeft))
        {
            DLeft = true;
        }

        if(leftStickX > 0.5 || IsGamepadButtonDown(gamepad, GamepadButton.LeftFaceRight))
        {
            DRight = true;
        }

        if(leftStickY < -0.5 || IsGamepadButtonDown(gamepad, GamepadButton.LeftFaceUp))
        {
            DUp = true;
        }

        if(leftStickY > 0.5 || IsGamepadButtonDown(gamepad, GamepadButton.LeftFaceDown))
        {
            DDown = true;
        }

        if(IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceDown))
        {
            PJump = true;
        }

        if(IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceLeft))
        {
            PDash = true;
        }

        if(IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceUp))
        {
            PFireball = true;
        }

        if(IsGamepadButtonPressed(gamepad, GamepadButton.RightFaceDown))
        {
            PConfirm = true;
        }

        if(IsGamepadButtonPressed(gamepad, GamepadButton.MiddleRight))
        {
            PBack = true;
        }

        if(IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceUp))
        {
            PUp = true;
        }

        if(IsGamepadButtonPressed(gamepad, GamepadButton.LeftFaceDown))
        {
            PDown = true;
        }
        }
        }

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

        if(IsKeyPressed(KeyboardKey.Escape))
        {
            PBack = true;
        }

        if(IsKeyPressed(KeyboardKey.W) || IsKeyPressed(KeyboardKey.Up))
        {
            PUp = true;
        }

        if(IsKeyPressed(KeyboardKey.S) || IsKeyPressed(KeyboardKey.Down))
        {
            PDown = true;
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
            } else if(!isCapsLockOn) {
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
            bool isUpperCase = shift || isCapsLockOn; // XOR to toggle case
            return isUpperCase ? char.ToUpper((char)key) : char.ToLower((char)key);
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
            //KeyboardKey.Grave => shift ? '~' : '`', // Tilde or Backtick
            _ => null // Unhandled key
        };
    }

    public void DrawController()
    {
        //DrawText(leftStickY + ", " + previousLeftStickY, 100, 100, 32, Color.White);

        /*if(IsGamepadAvailable(gamepad))
        {
            DrawTextEx(Game.rijusans, "Controller Found: " + GetGamepadName_(gamepad), new Vector2(20, 70), Game.indexSize, 0, Color.White);
        } else {
            DrawTextEx(Game.rijusans, "Controller NOT Found", new Vector2(20, 70), Game.indexSize, 0, Color.White);
        }
        
        for (int i = 0; i < 15; i++) // Assuming there are 15 buttons on the gamepad
        {
            if (IsGamepadButtonDown(gamepad, (GamepadButton)i))
            {
                DrawTextEx(Game.rijusans, "Holding Button: " + i, new Vector2(20, 120), Game.indexSize, 0, Color.White);
            }
        }*/

        for (int gamepad = 0; gamepad < 4; gamepad++) // Check up to 4 controllers
        {
            if (IsGamepadAvailable(gamepad))
            {
                DrawText("Gamepad Detected: " + gamepad + ", " + GetGamepadName_(gamepad), 100, 100 * (gamepad + 1), 32, Color.White);

                foreach (GamepadButton button in Enum.GetValues(typeof(GamepadButton)))
                {
                    if (IsGamepadButtonDown(gamepad, button))
                    {
                        DrawText("Gamepad Button Down: " + button + " from gamepad: " + gamepad, 100, 100 * (gamepad + 1) + 400, 32, Color.White);
                    }
                }
            }
        }
    }
}