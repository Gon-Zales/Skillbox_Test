using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using TG_File = Telegram.Bot.Types.File;

namespace SkillBot
{
    class Program
    {
        static readonly ITelegramBotClient botClient = new TelegramBotClient("1621303696:AAFZjrHdPUvnGvyX799F5l322azUxPdDFxA");
        static readonly ICollection<Message> files = new List<Message>();
        static void Main()
        {
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            botClient.StopReceiving();
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");
            FileBase file = null;
            string reply;
            switch (e.Message.Type)
            {
                case MessageType.Video:
                    file = e.Message.Video;
                    reply = "Thx for the vids";
                    break;
                case MessageType.VideoNote:
                    file = e.Message.VideoNote;
                    reply = "Thx for the vid";
                    break;
                case MessageType.Photo:
                    file = e.Message.Photo.LastOrDefault();
                    reply = "Thx for the pics";
                    break;
                case MessageType.Audio:
                    file = e.Message.Audio;
                    reply = "Thx for the audio";
                    break;
                case MessageType.Voice:
                    file = e.Message.Voice;
                    reply = "Thx for the voice";
                    break;
                case MessageType.Document:
                    file = e.Message.Document;
                    reply = "Thx for the docs";
                    break;
                case MessageType.Poll:
                    reply = "I'm not yet given the right to vote. Your society is cruel and xenophobic.";
                    break;
                case MessageType.Dice:
                    reply = "You won!";
                    break;
                case MessageType.Text:
                    ReplyToText(e.Message, out reply);
                    break;
                default:
                    reply = "whuzzat?";
                    break;
            }

            await botClient.SendTextMessageAsync(
              chatId: e.Message.Chat,
              text: reply
            );
            if (file == null)
                return;

            file = await botClient.GetFileAsync(file.FileId);
            var n_file = (TG_File)file;
            var docname = n_file.FilePath;
            if (!string.IsNullOrWhiteSpace(e.Message.Caption))
                docname = e.Message.Caption + "." + n_file.FilePath.Split('.').Last();
            files.Add(new Message { caption = docname, File_id = file.FileId, Type = e.Message.Type });
        }

        private static void ReplyToText(Telegram.Bot.Types.Message message, out string reply)
        {
            var command = message.Text.Split(' ').First();
            const string get_id = "/get_id";
            switch (command)
            {
                case "/start":
                    reply = "So, the tale begins";
                    return;
                case "/help":
                    reply = @"/list просмотреть список загруженных файлов./n/get_id Позволяет скачать файл по номеру.";
                    break;
                case "/list":
                    reply = "Your files are:\n";
                    foreach (var f in files)
                        reply += f.caption + "\n";
                    break;
                case get_id:
                    reply = "Here is your order, have a nice day!";
                    var id = message.Text.Substring(get_id.Length + 1);
                    Message selected_file;
                    try
                    {
                        selected_file = files.Single(f => f.caption == id);
                    }
                    catch (InvalidOperationException)
                    {
                        reply = "No matching id is found";
                        break;
                    }
                    SendFileAsync(message.Chat, selected_file);
                    break;
                default:
                    reply = $"What did you mean by \"{message.Text}\"";
                    break;
            }
        }
        static async void SendFileAsync(Chat chat, Message message)
        {
            switch (message.Type)
            {
                case MessageType.Video:
                    await botClient.SendVideoAsync(chat, new InputOnlineFile(message.File_id));
                    break;
                case MessageType.VideoNote:
                    await botClient.SendVideoNoteAsync(chat, new InputOnlineFile(message.File_id));
                    break;
                case MessageType.Photo:
                    await botClient.SendPhotoAsync(chat, new InputOnlineFile(message.File_id));
                    break;
                case MessageType.Audio:
                    await botClient.SendAudioAsync(chat, new InputOnlineFile(message.File_id));
                    break;
                case MessageType.Voice:
                    await botClient.SendVoiceAsync(chat, new InputOnlineFile(message.File_id));
                    break;
                case MessageType.Document:
                    await botClient.SendDocumentAsync(chat, new InputOnlineFile(message.File_id));
                    break;
                default:
                    throw new NotImplementedException();
            }

        }
        public struct Message
        {
            public string File_id;
            public string caption;
            public MessageType Type;
        }
    }
}
