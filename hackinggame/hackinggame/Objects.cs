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
    public interface Widget
    {
        void Init(GraphicsDeviceManager GD, SpriteBatch SB, Game GameContext);
        void Draw(GameTime GameTick);
        void Update(GameTime GameTick);
    }
    public interface Shell
    {
        int Index { get; set; }
        string GetFromIndex(Terminal Context);
        void ParseIn(string Command, Terminal Context);
        string GetPrompt();
        string[] GetTabCompletes(string ToCheck);
    }

    public class Port
    {
        string PortName { get; set; }
        int Value { get; set; }
    }

    public class Exploit
    {
        string ExploitName { get; set; }
        string PortEffective { get; set; }
        string FileName { get; set; }
    }

    public class Payload
    {
        string PayloadName { get; set; }
        string PortEffective { get; set; }
        string FileName { get; set; }
    }

    public class Loot
    {
        string LootName { get; set; }
        string Type { get; set; }
        string Payload { get; set; }
    }

    public class System
    {
        string SystemHostName { get; set; }
        string SystemRootName { get; set; }
        Port[] PortsAvailible { get; set; }
        Port[] PortsExploited { get; set; }
        Payload[] PayloadsInjected { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DefaultCommand : Attribute
    {
        public string name;
        public string description;

        public DefaultCommand(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }
}
