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
        private Lives lives;
        private Color color;
        private String side;
        private Texture2D image;
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

            this.lives = new Lives(lives);
            this.position = startingPosition; 
            this.side = side;
            this.image = image;
            this.Width = this.image.Width;
            this.Height = this.image.Height;
        }

        //SETTERS
        public void setLives(int currentLives)
        {
            lives.setLives(currentLives);
        }

        public void setPosition(Vector2 newPosition)
        {
            this.position = newPosition;
        }

        //GETTERS
        public int getLives()
        {
            return lives.getLives();
        }

        public Color getColor()
        {
            return this.color;
        }

        public String getSide()
        {
            return this.side;
        }

        public Vector2 getPosition()
        {
            return this.position;
        }

        public Texture2D getImage()
        {
            return this.image;
        }
        
    }
}
