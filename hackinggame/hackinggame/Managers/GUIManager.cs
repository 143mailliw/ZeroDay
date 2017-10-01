using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace hackinggame
{
	public static class GUIManager
	{
		static List<IWidget> Widgets = new List<IWidget>();
		static Game GameContext;

		public static void SetupGUISys(Game Context)
		{
			GameContext = Context;
			Widgets.Add(new Terminal());
			Widgets.Add(new StatusBar());
			Widgets[0].IsBound = true;
			Widgets[1].IsBound = true;
		}

		public static void Init()
		{
			foreach (IWidget WidgetToInit in Widgets)
				WidgetToInit.Init(GameContext.Graphics, GameContext.SpriteBatch, GameContext);
			Widgets[0].CurrentY = Widgets[1].Height;
		}

		public static void Update(GameTime gameTime)
		{
			foreach (IWidget WidgetToUpdate in Widgets)
				WidgetToUpdate.Update(gameTime);
		}

		public static void Draw(GameTime gameTime)
		{
			foreach (IWidget WidgetToDraw in Widgets)
				WidgetToDraw.Draw(gameTime);
		}
	}
}
