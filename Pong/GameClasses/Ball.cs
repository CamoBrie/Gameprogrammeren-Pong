using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.GameClasses
{
    class Ball
    {
        //private variables
        private Vector2 position;
        private Vector2 speed;
        private readonly Texture2D image;
        private Color color;

        //public variables
        public int Width;
        public int Height;
        public Ball(Vector2 startingPosition, Vector2 startingSpeed, Texture2D image, Color? color)
        {
            //set default color if not given
            if (!color.HasValue)
            {
                this.color = Color.White;
            }
            else
            {
                this.color = (Color)color;
            }

            //assign object variables
            this.position = startingPosition;
            this.speed = startingSpeed;
            this.image = image;

            this.Width = this.image.Width;
            this.Height = this.image.Height;
        }

        //public functions
        public void InvertY()
        {
            this.speed.Y = -this.speed.Y;
        }

        //SETTERS
        public void SetPosition(Vector2 newPosition)
        {
            this.position = newPosition;
        }
        public void SetSpeed(Vector2 newSpeed)
        {
            this.speed = newSpeed;
        }


        //GETTERS
        public Vector2 GetPosition()
        {
            return this.position;
        }

        public Vector2 GetSpeed() {
            this.speed.Normalize();
            return this.speed;
        }
        public Texture2D GetImage()
        {
            return this.image;
        }

        public Color GetColor()
        {
            return this.color;
        }
    }
}
