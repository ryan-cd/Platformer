using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{
    public class HUD
    {
        public SpriteFont font { get; set; }
        private Vector2 scorePos = new Vector2(20, 10);
        private Vector2 livesPos = new Vector2(20, 30);
        private Vector2 infoPos = new Vector2(400, 20);
        public int score { get; set; }
        public int lives { get; set; }
        public String info { get; set; }
        public HUD() { }

        public void DrawScore(SpriteBatch spriteBatch)
        {
            //Draw score in top left of screen
            spriteBatch.DrawString(
                font,                           //SpriteFont
                "Score: " + score.ToString(),   //Text
                scorePos,                       //Position
                Color.White);                   //Colour
        }
        public void DrawLives(SpriteBatch spriteBatch)
        {
            //Draw score in top left of screen
            spriteBatch.DrawString(
                font,                           //SpriteFont
                "Lives: " + lives.ToString(),   //Text
                livesPos,                       //Position
                Color.White);                   //Colour
        }
        public void DrawInfo(SpriteBatch spriteBatch2)
        {
            //Draw score in top left of screen
            spriteBatch2.DrawString(
                font,                           //SpriteFont
                "Info: " + info.ToString(),   //Text
                infoPos,                       //Position
                Color.White);                   //Colour
        }
    }
}
