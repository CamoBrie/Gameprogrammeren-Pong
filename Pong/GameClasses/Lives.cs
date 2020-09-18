using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.GameClasses
{
    class Lives
    {
        private int lives;

        public Lives(int lives = 3)
        {
            this.lives = lives;
        }

        public int getLives()
        {
            return this.lives;
        }

        public void setLives(int lives)
        {
            this.lives = lives;
        }
    }
}
