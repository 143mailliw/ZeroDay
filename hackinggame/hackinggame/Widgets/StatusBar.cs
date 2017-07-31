/*
 * MIT License
 * 
 * Copyright (c) 2017 william341
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Input.InputListeners;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace hackinggame
{
    class StatusBar : Widget
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;
        SpriteFont Font;
        Game Context;
        Texture2D Texture;
        Texture2D Floppy;
        Texture2D Ping;
        Vector2 Position;

        public void Init(GraphicsDeviceManager GD, SpriteBatch SB, Game GameContext)
        {
            Context = GameContext;
            SpriteBatch = SB;
            Graphics = GD;
            Position = new Vector2(0, 0);
            Font = Context.Content.Load<SpriteFont>("font");
            Texture = Context.Content.Load<Texture2D>("findthepixel");
            Floppy = Context.Content.Load<Texture2D>("flp");
            Ping = Context.Content.Load<Texture2D>("Ping");
        }

        public void Update(GameTime GameTick)
        {

        }

        public void Draw(GameTime GameTick)
        {
            SpriteBatch.Draw(Texture, new Rectangle(0, 0, Context.Window.ClientBounds.Width, 22), new Color (0,0,50));
            SpriteBatch.Draw(Texture, new Rectangle(0, 22, Context.Window.ClientBounds.Width, 2), new Color(0, 0, 75));
            Vector2 Measure = Font.MeasureString(DateTime.Now.ToString("h:mm"));
            Vector2 Position = new Vector2(Context.Window.ClientBounds.Width - 10 - Measure.X, 3);
            SpriteBatch.DrawString(Font, DateTime.Now.ToString("h:mm"), Position, new Color(230, 230, 255), 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            SpriteBatch.Draw(Floppy, new Rectangle(10, 2, 20, 20), new Color(230, 230, 255));
        }
    }
}
