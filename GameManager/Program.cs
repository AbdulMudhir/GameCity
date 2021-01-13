using System;
using System.Threading.Tasks;
using GameManager.Steam;
using Persistence.DBFactories;

namespace GameManager
{
    class Program
    {


        public static void Main(string[] args)
        {



            var steamManager = new SteamManager();


            steamManager.Start();


            while (true)
            {

     

                


            }
        }
    }
}
