using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BlobGame;

public class KeyManager
{
    public bool DLeft {get; set;} = false;
    public bool DRight {get; set;} = false;
    public bool PJump {get; set;} = false;
    public bool PDash {get; set;} = false;
    public bool PFireball {get; set;} = false;
    public bool PConfirm {get; set;} = false;
    public bool PUp {get; set;} = false;
    public bool PDown {get; set;} = false;
    KeyboardState prevkstate;

    public KeyManager()
    {
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState kstate = Keyboard.GetState();

        DLeft = false;
        DRight = false;
        PJump = false;
        PDash = false;
        PFireball = false;
        PConfirm = false;
        PUp = false;
        PDown = false;


        if(kstate.IsKeyDown(Keys.A) || kstate.IsKeyDown(Keys.Left))
        {
            DLeft = true;
        }

        if(kstate.IsKeyDown(Keys.D) || kstate.IsKeyDown(Keys.Right))
        {
            DRight = true;
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

        prevkstate = kstate;
    }

    public static bool IsKeyPressed(KeyboardState kstate, KeyboardState prevkstate, Keys key)
        {
            return kstate.IsKeyDown(key) && !prevkstate.IsKeyDown(key);
        }
}