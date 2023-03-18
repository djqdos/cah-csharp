using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cah.models
{
    public class GameUser
    {
        public string Username { get; set; }

        public string ConnectionId { get; set; }    

        public BlackCard BlackCard { get; set; }

        public List<WhiteCard> WhiteCards { get; set; } 

        public bool IsHost { get; set; }    

    }
}
