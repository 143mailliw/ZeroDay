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
        string CurrentIn = "";
        MonoGame.Extended.Input.InputListeners.KeyboardListenerSettings KeySettings = new MonoGame.Extended.Input.InputListeners.KeyboardListenerSettings();
        MonoGame.Extended.Input.InputListeners.KeyboardListener KeyListen;
        MonoGame.Extended.Input.InputListeners.MouseListener MouseListen;
        int CurrentY = 32;
        int CurrentX = 10;
        int ValuesChecked = 0;
        string Prefix = "Memes~$ ";
        List<string> Strings = new List<string>();
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
            Strings.Add(Prefix);
            KeyListen.KeyTyped += (sender, args) =>
            {
                if (args.Key == Keys.Back && CurrentIn.Length > 0)
                {
                    if (Strings[Strings.Count - 1] != Prefix)
                        Strings[Strings.Count - 1] = Strings[Strings.Count - 1].Substring(0, Strings[Strings.Count - 1].Length - 1);
                }
                else if (args.Key == Keys.Enter)
                    Strings.Add(Prefix);
                else
                    Strings[Strings.Count - 1] += args.Character?.ToString() ?? "";
            };

        }

        public void Update(GameTime GameTick)
		{

        }

		public void Draw(GameTime GameTick)
		{
			// Finds the center of the string in coordinates inside the text rectangle
            try
            {
                CurrentIn = "";
                foreach(string ToAdd in Strings)
                    CurrentIn += ToAdd + Environment.NewLine;
                Vector2 Measure = Font.MeasureString(Prefix + CurrentIn);
                if(Measure.Y > Context.Window.ClientBounds.Height - 32 && ValuesChecked != Strings.Count)
                {
                    float FCA = Context.Window.ClientBounds.Height - Measure.Y;
                    int CheckAgainst = (int)FCA + 10;
                    CurrentY = CheckAgainst;
                    ValuesChecked = Strings.Count;
                }
                // Places text in center of the screen
                Vector2 Position = new Vector2(CurrentX, CurrentY);
                SpriteBatch.DrawString(Font, CurrentIn, Position, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            }
            catch { CurrentIn = ""; }
		}
	}
}
