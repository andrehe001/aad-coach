using System;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

using AdventureDay.DataModel;
using AdventureDay.Runner.Model;

namespace AdventureDay.Runner.Players.RealPlayers
{
    public class RandomPlayer : RealPlayerBase
    {
        private readonly Random _random;

        public override string Name => "Lachlan";

        private Move GetRandomMove()
        {
            int length = Enum.GetValues(typeof(Move)).Length;
            byte[] _uint32Buffer = new byte[4];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {                
                rng.GetBytes(_uint32Buffer);
                UInt32 rand = BitConverter.ToUInt32(_uint32Buffer, 0);

                Int64 max = (1 + (Int64)UInt32.MaxValue);
                Int64 remainder = max % length;
                if (rand < max - remainder)
                {
                    return (Move)(rand % length);
                }
                else
                {
                    return (Move)new Random().Next(length);
                }
            }            
        }

        protected override Move GetFirstMove()
        {
            return GetRandomMove();
        }

        protected override Move GetNextMove(MatchResponse lastMatchResponse)
        {
            return GetRandomMove();
        }
    }
}