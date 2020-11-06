using System.Collections.Generic;

public class BattleSummary
{
    public List<TurnOperation> turnOperations = new List<TurnOperation>();
    public class TurnOperation
    {
        public int roundRank;
        public int turnRank;
        public bool isPlayer1;
        public List<List<Card>> allCardList = new List<List<Card>>();
        public List<PlayOperation> playOperations = new List<PlayOperation>();

        public TurnOperation(int roundRank, int turnRank, bool isPlayer1, List<List<Card>> allCardList, List<PlayOperation> playOperations)
        {
            this.roundRank = roundRank;
            this.turnRank = turnRank;
            this.isPlayer1 = isPlayer1;
            this.allCardList = allCardList;
            this.playOperations = playOperations;
        }

        public enum Operation
        {
            PlayCard,
            DisCard,
            SelectUnite,
            SelectRegion,
            SelectLocation,
            Pass
        }
        public class PlayOperation
        {
            public Operation operation;
            public object[] param;

            public PlayOperation(Operation operation, params object[] param)
            {
                this.operation = operation;
                this.param = param;
            }
        }

        public class Card
        {
            public int CardID = 100;
        }
    }
    public void Summary(int roundRank, int turnRank, bool isPlayer1, List<List<TurnOperation.Card>> allCardList, List<TurnOperation.PlayOperation> playOperations)
    {
        turnOperations.Add(new TurnOperation(roundRank: roundRank, turnRank: turnRank, isPlayer1: isPlayer1, allCardList: allCardList, playOperations));
    }
}
//BattleSummary summary = new BattleSummary();
//List<List<TurnOperation.Card>> allCardList = new List<List<TurnOperation.Card>>()
//            {
//                {
//                    new List<TurnOperation.Card>()
//                    {
//                        new TurnOperation.Card(),
//                        new TurnOperation.Card()
//                    }
//                }
//            };
//List<TurnOperation.PlayOperation> playOperations = new List<TurnOperation.PlayOperation>();
//playOperations = new List<TurnOperation.PlayOperation>() { new TurnOperation.PlayOperation(TurnOperation.Operation.PlayCard, 1) };
//            summary.Summary(1, 1, true, allCardList, playOperations);
//            playOperations = new List<TurnOperation.PlayOperation>() { new TurnOperation.PlayOperation(TurnOperation.Operation.DisCard, 2) };
//            summary.Summary(1, 1, false, allCardList, playOperations);
//            playOperations = new List<TurnOperation.PlayOperation>()
//            {
//                new TurnOperation.PlayOperation(TurnOperation.Operation.PlayCard, 2) ,
//                new TurnOperation.PlayOperation(TurnOperation.Operation.SelectLocation, 2,5)
//            };
//            summary.Summary(1, 2, true, allCardList, playOperations);
//            playOperations = new List<TurnOperation.PlayOperation>()
//            {
//                new TurnOperation.PlayOperation(TurnOperation.Operation.PlayCard, 2) ,
//                new TurnOperation.PlayOperation(TurnOperation.Operation.SelectRegion, 1)
//            };
//            summary.Summary(1, 2, false, allCardList, playOperations);
//            Console.WriteLine(summary.ToJson(Newtonsoft.Json.Formatting.Indented));