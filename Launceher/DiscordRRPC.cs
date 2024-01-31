using DiscordRPC;
using System;
using System.IO;

namespace Launceher
{

    public class DiscordRRPC
    {
        public DiscordRpcClient Client { get; private set; }
        public void Setup()
        {
            Client = new DiscordRpcClient("1200859758702375073");  //Creates the client
            Client.Initialize();                            //Connects the client
                                                            //Set Presence
            
            Client.SetPresence(new RichPresence()
            {
                Details = "Играет на Project: Seasons",
                State = $"Ник: {File.ReadAllText(@".\nick.pseasons")}",
                Timestamps = new Timestamps
                {
                    Start = DateTime.UtcNow
                },
                
                Buttons = new Button[]
                {
                    new Button()
                    {
                        Label = "Присоедениться",                        
                        Url = "https://discord.gg/d9xhGknRkW"
                    }
                },
                Assets = new Assets()
                {
                    LargeImageKey = "kandinsky-download-1693485811437",
                    LargeImageText = "Сервер с модом Create",
                    SmallImageKey = "https://projectseasons.ru/gear.png"
                }
            }) ;
        }

    }
}
