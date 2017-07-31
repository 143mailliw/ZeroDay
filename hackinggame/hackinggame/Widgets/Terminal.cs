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
    public class Terminal : Widget
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;
        SpriteFont Font;
        Game Context;
        string CurrentIn = "";
        MonoGame.Extended.Input.InputListeners.KeyboardListenerSettings KeySettings = new MonoGame.Extended.Input.InputListeners.KeyboardListenerSettings();
        MonoGame.Extended.Input.InputListeners.MouseListenerSettings MouseSettings = new MonoGame.Extended.Input.InputListeners.MouseListenerSettings();
        MonoGame.Extended.Input.InputListeners.KeyboardListener KeyListen;
        MonoGame.Extended.Input.InputListeners.MouseListener MouseListen;
        int CurrentY = 32;
        int CurrentX = 10;
        int ValuesChecked = 0;
        string Prefix = "Memes~$ ";
        List<string> Strings = new List<string>();
        Shell ShellToUse = new DefaultShell();
        int MaxLineWidth = 0;
        public void Init(GraphicsDeviceManager GD, SpriteBatch SB, Game GameContext)
        {
            Context = GameContext;
            SpriteBatch = SB;
            Graphics = GD;
            KeySettings.RepeatDelayMilliseconds = 15;
            Context.IsMouseVisible = true;
            KeyListen = new MonoGame.Extended.Input.InputListeners.KeyboardListener(KeySettings);
            MouseListen = new MonoGame.Extended.Input.InputListeners.MouseListener(MouseSettings);
            Context.Components.Add(new InputListenerComponent(Context, MouseListen, KeyListen));
            Font = Context.Content.Load<SpriteFont>("font2");
            Context.Window.AllowUserResizing = true;
            Strings.Add(Prefix);
            KeyListen.KeyTyped += (sender, args) =>
            {
                if (args.Key == Keys.Back && CurrentIn.Length > 0)
                {
                    if (Strings[Strings.Count - 1] != ShellToUse.GetPrompt())
                        Strings[Strings.Count - 1] = Strings[Strings.Count - 1].Substring(0, Strings[Strings.Count - 1].Length - 1);
                }
                else if (args.Key == Keys.Enter)
                {
                    string ToSend = Strings[Strings.Count - 1].Substring(ShellToUse.GetPrompt().Length + 1, Strings[Strings.Count -1].Length - ShellToUse.GetPrompt().Length - 1);
                    ShellToUse.ParseIn(ToSend, this);
                }
                else
                    Strings[Strings.Count - 1] += args.Character?.ToString() ?? "";
            };

        }

        public void SendOut(string ToAdd)
        {
            Strings.Add(ToAdd);
        }

        public void Update(GameTime GameTick)
        {
            MaxLineWidth = Context.Window.ClientBounds.Width - 20;
        }

        private string WrapText(string Text) //Credit to Sankra
        {
            if (Font.MeasureString(Text).X < MaxLineWidth)
                return Text;

            string[] Words = Text.Split(' ');
            StringBuilder WrappedText = new StringBuilder();
            float LineWidth = 0f;
            float SpaceWidth = Font.MeasureString(" ").X;
            for (int i = 0; i < Words.Length; ++i)
            {
                Vector2 Size = Font.MeasureString(Words[i]);
                if (LineWidth + Size.X < MaxLineWidth)
                {
                    LineWidth += Size.X + SpaceWidth;
                }
                else
                {
                    WrappedText.Append("\n");
                    LineWidth = Size.X + SpaceWidth;
                }
                WrappedText.Append(Words[i]);
                WrappedText.Append(" ");
            }

            return WrappedText.ToString();
        }

        public void Draw(GameTime GameTick)
        {
            try
            {
                CurrentIn = "";
                foreach (string ToAdd in Strings)
                    CurrentIn += WrapText(ToAdd) + Environment.NewLine;
                Vector2 Measure = Font.MeasureString(CurrentIn);
                if (Measure.Y > Context.Window.ClientBounds.Height - 32 && ValuesChecked != Strings.Count)
                {
                    float FCA = Context.Window.ClientBounds.Height - Measure.Y;
                    int CheckAgainst = (int)FCA + 10;
                    CurrentY = CheckAgainst;
                    ValuesChecked = Strings.Count;
                }
                Vector2 Position = new Vector2(CurrentX, CurrentY);
                SpriteBatch.DrawString(Font, CurrentIn, Position, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            }
            catch { CurrentIn = ""; }
        }
    }
}