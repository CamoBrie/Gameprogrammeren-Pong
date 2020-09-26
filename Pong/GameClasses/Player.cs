using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.GameClasses
{
    class Player
    {
        //private variables
        private int lives;
        private Color color;
        private readonly String side;
        private readonly Texture2D image;
        private Vector2 position;

        //public variables
        public int Width;
        public int Height;
        public Player(int lives, Color? color, Texture2D image, Vector2 startingPosition ,string side = "East")
        {
            //set default color
            if (!color.HasValue)
            {
                this.color = Color.White;
            } else
            {
                this.color = (Color) color;
            }

            //assign object variables
            this.lives = lives;
            this.position = startingPosition; 
            this.side = side;
            this.image = image;
            this.Width = this.image.Width;
            this.Height = this.image.Height;
        }

        //SETTERS
        public void SetLives(int currentLives)
        {
            this.lives = currentLives;
        }

        public void SetPosition(Vector2 newPosition)
        {
            this.position = newPosition;
        }

        //GETTERS
        public int GetLives()
        {
            return this.lives;
        }

        public Color GetColor()
        {
            return this.color;
        }

        public String GetSide()
        {
            return this.side;
        }

        public Vector2 GetPosition()
        {
            return this.position;
        }

        public Texture2D GetImage()
        {
            return this.image;
        }
        
    }
}
