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
	class Terminal
	{
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;
        SpriteFont Font;
        Game Context;
        String CurrentIn = "";
        MonoGame.Extended.Input.InputListeners.KeyboardListenerSettings KeySettings = new MonoGame.Extended.Input.InputListeners.KeyboardListenerSettings();
        MonoGame.Extended.Input.InputListeners.KeyboardListener KeyListen;
        MonoGame.Extended.Input.InputListeners.MouseListener MouseListen;
        int CurrentX;
        int CurrentY;
        public void Init(GraphicsDeviceManager GD, SpriteBatch SB, Game GameContext)
		{
            Context = GameContext;
            SpriteBatch = SB;
            Graphics = GD;
            KeyListen = new MonoGame.Extended.Input.InputListeners.KeyboardListener(KeySettings);
            MouseListen = new MonoGame.Extended.Input.InputListeners.MouseListener(new MouseListenerSettings());
            Context.Components.Add(new InputListenerComponent(Context, MouseListen, KeyListen));
            Font = Context.Content.Load<SpriteFont>("font2");
            Context.Window.AllowUserResizing = true;
            KeyListen.KeyTyped += (sender, args) =>
            {
                if (args.Key == Keys.Back && CurrentIn.Length > 0)
                {
                    CurrentIn = CurrentIn.Substring(0, CurrentIn.Length - 1);
                }
                else if (args.Key == Keys.Enter)
                {
                    CurrentIn += Environment.NewLine;
                }
                else
                {
                    CurrentIn += args.Character?.ToString() ?? "";
                }
            };

        }

        public void Update(GameTime GameTick)
		{
            if (CurrentIn.Length < 0)
            {
                CurrentIn = "";
            }
        }

		private void KD(object sender, MonoGame.Extended.Input.InputListeners.KeyboardEventArgs EventArgs)
		{
            Console.Beep();
            Console.WriteLine(EventArgs.Character);
            CurrentIn = CurrentIn + EventArgs.Character.Value;
		}

		public void Draw(GameTime GameTick)
		{
			// Finds the center of the string in coordinates inside the text rectangle
            try
            {
                Vector2 TextMiddlePoint = Font.MeasureString("Memes~$ " + CurrentIn) / 2;
                // Places text in center of the screen
                Vector2 Position = new Vector2(Context.Window.ClientBounds.Width / 2, Context.Window.ClientBounds.Height / 2);
                SpriteBatch.DrawString(Font, "Memes~$ " + CurrentIn, Position, Color.White, 0, TextMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            }
            catch
            {
                CurrentIn = "";
            }
		}
	}
}
