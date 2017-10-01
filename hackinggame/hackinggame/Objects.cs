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
using Newtonsoft.Json;

namespace hackinggame
{
    public interface IWidget
    {
		bool IsBound { get; set; } //if not bound then CurrentY and CurrentX are meaningless
		int CurrentY { get; set; }
		int CurrentX { get; set; }
		int Height { get; set; }
		void Init(GraphicsDeviceManager GD, SpriteBatch SB, Game GameContext);
        void Draw(GameTime GameTick);
        void Update(GameTime GameTick);
    }

    public interface IShell
    {
        int Index { get; set; }
        string GetFromIndex(Terminal Context);
        void ParseIn(string Command, Terminal Context);
        string GetPrompt(Terminal Context);
        string[] GetTabCompletes(string ToCheck);
    }

    public class Port
    {
		public string PortName { get; set; }
		public int Value { get; set; }
    }

    public class Exploit
    {
		public string ExploitName { get; set; }
		public string PortEffective { get; set; }
		public string FileName { get; set; }
    }

    public class Payload
    {
		public string PayloadName { get; set; }
		public string PortEffective { get; set; }
		public string FileName { get; set; }
    }

    public class Loot
    {
		public string LootName { get; set; }
		public string Type { get; set; }
		public string Data { get; set; }
    }

    public class SystemGame
    {
        public string SystemHostName { get; set; }
		public string SystemRootName { get; set; }
		public Port[] PortsAvailible { get; set; }
		public Port[] PortsExploited { get; set; }
		public Payload[] PayloadsInjected { get; set; }
		public Loot[] Loot { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class Command : Attribute
    {
        public string name;
        public string description;
		public string package;
		public string context;
        public Command(string name, string description, string package, string context)
        {
            this.name = name;
            this.description = description;
			this.package = package;
			this.context = context;
        }
    }

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class CommandClass : Attribute
	{
		public CommandClass() { }
	}
}
