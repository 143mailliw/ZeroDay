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
    public class Terminal : IWidget
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;
        SpriteFont Font;
        public Game Context;
        string CurrentIn = "";
        KeyboardListenerSettings KeySettings = new KeyboardListenerSettings();
        MouseListenerSettings MouseSettings = new MouseListenerSettings();
        KeyboardListener KeyListen;
        MouseListener MouseListen;
        Texture2D caret;
        Rectangle caretPos;
        public int CurrentY = 32;
        int CurrentX = 10;
        public int ValuesChecked = 0;
        public List<string> Strings = new List<string>();
		public IShell ShellToUse = new DefaultShell();
		public string Username = "user";
        int MaxLineWidth = 0;
        int ScrollUp = 0;
        int LastScrollValue = 0;
        int CharScroll = 0;
        public void Init(GraphicsDeviceManager GD, SpriteBatch SB, Game GameContext)
        {
            Context = GameContext;
            SpriteBatch = SB;
            Graphics = GD;
            KeySettings.RepeatDelayMilliseconds = 15;
            Context.IsMouseVisible = true;
            KeyListen = new KeyboardListener(KeySettings);
            MouseListen = new MouseListener(MouseSettings);
            Context.Components.Add(new InputListenerComponent(Context, MouseListen, KeyListen));
            Font = Context.Content.Load<SpriteFont>("font2");
            Context.Window.AllowUserResizing = true;
            Strings.Add(ShellToUse.GetPrompt(this));

            caret = Context.Content.Load<Texture2D>("findthepixel");
            caretPos = new Rectangle(0, 0, 8, 2 );

            MouseListen.MouseDown += (sender, args) =>
            {
                if (args.Button == MouseButton.Right)
                    Strings[Strings.Count - 1] += System.Windows.Forms.Clipboard.GetText();
            };
            KeyListen.KeyPressed += (sender, args) =>
            {
                if (args.Key == Keys.Up)
                {
                    ShellToUse.Index += 1;
                    Strings[Strings.Count - 1] = ShellToUse.GetFromIndex(this);
                }
                else if (args.Key == Keys.Down)
                {
                    ShellToUse.Index -= 1;
                    Strings[Strings.Count - 1] = ShellToUse.GetFromIndex(this);
                }
                else if (args.Key == Keys.Left)
                {
                    CharScroll += 1;
                }
                else if (args.Key == Keys.Right)
                {
                    CharScroll -= 1;
                }
                else if (args.Key == Keys.Tab)
                {
                    string ToSend = Strings[Strings.Count - 1].Substring(ShellToUse.GetPrompt(this).Length, Strings[Strings.Count - 1].Length - ShellToUse.GetPrompt(this).Length);
                    string[] TabCompletes = ShellToUse.GetTabCompletes(ToSend);
                    if (TabCompletes.Length == 1)
                        Strings[Strings.Count - 1] = (ShellToUse.GetPrompt(this) + TabCompletes[0]);
                    else if (TabCompletes.Length > 0)
                    {
                        string ToOut = "";
                        foreach (string Complete in TabCompletes)
                        {
                            ToOut += Complete + " ";
                        }
                        SendOut(ToOut);
                        SendOut(ShellToUse.GetPrompt(this));
                    }
                }
            };
            KeyListen.KeyTyped += (sender, args) =>
            {
                if (args.Key == Keys.Back && CurrentIn.Length > 0)
                {
                    if (Strings[Strings.Count - 1].Substring(0, Strings[Strings.Count - 1].Length - CharScroll) != ShellToUse.GetPrompt(this))
                        Strings[Strings.Count - 1] = Strings[Strings.Count - 1].Remove(Strings[Strings.Count - 1].Length - CharScroll - 1, 1);
                }
                else if (args.Key == Keys.Enter)
                {
                    string ToSend = Strings[Strings.Count - 1].Substring(ShellToUse.GetPrompt(this).Length, Strings[Strings.Count - 1].Length - ShellToUse.GetPrompt(this).Length);
                    ShellToUse.ParseIn(ToSend, this);
                    SendOut(ShellToUse.GetPrompt(this));
                }
                else if (args.Key == Keys.Tab)
                {
                    //do nothing because this breaks everything lol 
                }
                else
                {
                    Strings[Strings.Count - 1] = Strings[Strings.Count - 1].Insert(Strings[Strings.Count - 1].Length - CharScroll, args.Character?.ToString() ?? "");
                    ShellToUse.Index = 0;
                    ScrollUp = 0;
                }
            };

        }

        public void SendOut(string ToAdd)
        {
            Strings.Add(ToAdd);
            ScrollUp = 0;
        }

        public void Update(GameTime GameTick)
        {
            MaxLineWidth = Context.Window.ClientBounds.Width - 20;
            if (Mouse.GetState().ScrollWheelValue > LastScrollValue)
                ScrollUp += 10;
            if (Mouse.GetState().ScrollWheelValue < LastScrollValue)
                ScrollUp -= 10;
            LastScrollValue = Mouse.GetState().ScrollWheelValue;
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
                    LineWidth += Size.X + SpaceWidth;
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
                if (ScrollUp > (int)(Measure.Y - (Context.Window.ClientBounds.Height - 32)))
                    ScrollUp = (int)(Measure.Y - (Context.Window.ClientBounds.Height - 32));
                if (0 > ScrollUp)
                    ScrollUp = 0;
                if (0 > CharScroll)
                {
                    CharScroll += 1;
                }
                if (Strings[Strings.Count - 1].Substring(0, Strings[Strings.Count - 1].Length - CharScroll).Length < ShellToUse.GetPrompt(this).Length )
                {
                    CharScroll -= 1;
                }
                caretPos.Y = (int)Measure.Y + 11;
                caretPos.X = (int)Font.MeasureString(Strings[Strings.Count - 1].Substring(0, Strings[Strings.Count - 1].Length - CharScroll)).X + 10;
                float FCA = Context.Window.ClientBounds.Height - Measure.Y;
                if (Measure.Y > Context.Window.ClientBounds.Height - 32)
                {
                    FCA = Context.Window.ClientBounds.Height - Measure.Y + 1;
                    int CheckAgainst = (int)FCA + 10;
                    CurrentY = CheckAgainst + ScrollUp;
                    ValuesChecked = Strings.Count;
                }
                Vector2 Position = new Vector2(CurrentX, CurrentY);
                SpriteBatch.DrawString(Font, CurrentIn, Position, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                SpriteBatch.Draw(caret, caretPos, Color.White);
            }
            catch { CurrentIn = ""; }
        }
    }
}
