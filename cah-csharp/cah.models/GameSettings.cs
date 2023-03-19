using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cah.models
{
    public class GameSettings
    {
        public int CardsPerPerson = 8;

        public bool GameStarted {  get; set; }   

        public string GameSet { get; set; }  
        
        public List<GameUser> GameUsers { get; set; } = new List<GameUser>();

		public string SelectedSet { get; set; }

		public GameState GameState { get; set; } 
        
    }
}
