using System;
using System.IO;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
// 1621303696:AAFZjrHdPUvnGvyX799F5l322azUxPdDFxA
namespace SkillBot
{
    class Program
    {
        static ITelegramBotClient botClient;

        static void Main()
        {
            botClient = new TelegramBotClient("1621303696:AAFZjrHdPUvnGvyX799F5l322azUxPdDFxA");

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            botClient.StopReceiving();
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");
            var reply = "";
            switch (e.Message.Type)
            {
                case MessageType.Video:
                    break;
                case MessageType.Photo:
                    var file = await botClient.GetFileAsync(e.Message.Photo.LastOrDefault()?.FileId);

                    var filename = (e.Message.Caption ?? file.FileId) + "." + file.FilePath.Split('.').Last();

                    using (var saveImageStream = File.Open(filename, FileMode.Create))
                        await botClient.DownloadFileAsync(file.FilePath, saveImageStream);

                    reply = "Thx for the Pics";
                    break;
                case MessageType.Audio:
                    break;
                case MessageType.Voice:
                    break;
                case MessageType.Document:
                    break;
                case MessageType.Sticker:
                    break;
                case MessageType.Contact:
                    break;
                case MessageType.Poll:
                    reply = "I'm not yet given the right to vote. Your society is cruel and xenophobic.";
                    break;
                case MessageType.Dice:
                    break;
                case MessageType.Text:
                    switch (e.Message.Text)
                    {
                        case "/start": return;
                        case "/help":
                            reply = "/start for ";
                            break;
                        case "/some":
                            reply = "";
                            break;
                        default:
                            reply = $"What did you mean by \"{e.Message.Text}\"";
                            break;
                    }
                    break;
                default:
                    reply = "whuzzat?";
                    break;
            }

            await botClient.SendTextMessageAsync(
              chatId: e.Message.Chat,
              text: reply
            );
        }
    }
}
