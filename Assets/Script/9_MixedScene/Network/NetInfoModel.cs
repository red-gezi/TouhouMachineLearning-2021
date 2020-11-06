using Model;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
namespace Network
{
    [Serializable]
    public class NetInfoModel
    {
        public class Location
        {
            public int x;
            public int y;
            public Location(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        public class PlayerInfo
        {
            public string _id;
            public string name;
            public string password;
            public int level;
            public int rank;
            public Dictionary<string, int> resource { get; set; }
            [ShowInInspector]
            public Dictionary<string, int> cardLibrary { get; set; }
            public int useDeckNum;
            public List<CardDeck> decks;
            public CardDeck UseDeck => decks[useDeckNum];

            public PlayerInfo(string Name, string Password, List<CardDeck> Deck)
            {
                this.name = Name;
                this.decks = Deck;
                this.password = Password;
                level = 0;
                rank = 0;
                useDeckNum = 0;
                resource = new Dictionary<string, int>();
                resource.Add("faith", 0);
                resource.Add("recharge", 0);
            }

        }
        [Serializable]
        public class GeneralCommand
        {
            public object[] Datas;
            public GeneralCommand()
            {
            }
            public GeneralCommand(params object[] Datas)
            {
                this.Datas = Datas;
            }
        }
        [Serializable]
        public class GeneralCommand<T>
        {
            public T[] Datas;
            public GeneralCommand()
            {
            }
            public GeneralCommand(params T[] Datas)
            {
                this.Datas = Datas;
            }
        }
    }
}

