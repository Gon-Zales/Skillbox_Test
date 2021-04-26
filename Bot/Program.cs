using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;
using TG_File = Telegram.Bot.Types.File;

namespace SkillBot
{
    class Program
    {
        #region Commands list
        const string start = "/start";
        const string help = "/help";
        const string get = "/get";
        const string save = "/save";
        const string list = "/list";
        #endregion

        static ITelegramBotClient botClient;
        static readonly ICollection<Message> files = new List<Message>();
        const string filesDir = "files/";
        const string tokenPath = "token.txt";

        static void Main()
        {
            var token = File.ReadAllText(tokenPath);
            botClient = new TelegramBotClient(token);

            botClient.GetMeAsync().Wait();

            Directory.CreateDirectory(filesDir);

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            botClient.StopReceiving();

        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            FileBase file = null;
            string reply;
            if (e.Message.Type is MessageType.Text)
                reply = HandleText(e.Message);
            else
                reply = HandleMedia(e.Message, ref file);

            await botClient.SendTextMessageAsync(e.Message.Chat, reply, replyToMessageId:e.Message.MessageId);

            if (file != null)
                await CacheFile(e, file);
        }

        private static async Task CacheFile(MessageEventArgs e, FileBase file)
        {
            Console.WriteLine("CacheFile");
            TG_File tg_file = await botClient.GetFileAsync(file.FileId);
            files.Add(new Message
            {
                Caption = e.Message.Caption,
                FileId = tg_file.FileId,
                Type = e.Message.Type,
                FilePath = tg_file.FilePath.Split('/').LastOrDefault()
            });
        }

        private static string HandleMedia(Telegram.Bot.Types.Message message, ref FileBase file)
        {
            Console.WriteLine("HandleMedia");
            switch (message.Type)
            {
                case MessageType.Video:
                    file = message.Video;
                    return "Thx for the vids";
                case MessageType.VideoNote:
                    file = message.VideoNote;
                    return "Thx for the vid";
                case MessageType.Photo:
                    file = message.Photo.LastOrDefault();
                    return "Thx for the pics";
                case MessageType.Audio:
                    file = message.Audio;
                    return "Thx for the audio";
                case MessageType.Voice:
                    file = message.Voice;
                    return "Thx for the voice";
                case MessageType.Document:
                    file = message.Document;
                    return "Thx for the docs";
                case MessageType.Poll:
                    return "I'm not yet given the right to vote. Your society is cruel and xenophobic.";
                case MessageType.Dice:
                    return "You won!";
                default:
                    return "whuzzat?";
            }
        }

        private static string HandleText(Telegram.Bot.Types.Message message)
        {
            var command = message.Text.Split(' ').First();
            switch (command)
            {
                case start:
                    return "So, the tale begins";
                case help:
                    return $"{list} просмотреть список загруженных файлов./n{get} Позволяет скачать файл по номеру.";
                case list:
                    var reply = "Your files are:\n";
                    foreach (var f in files)
                        reply += (f.Caption ?? f.FilePath) + "\n";
                    return reply;
                case save:
                case get:
                    var id = message.Text.Substring(command.Length + 1).Trim();
                    Message selectedFile;
                    try
                    {
                        selectedFile = files.Single(f => f.Caption == id || f.FilePath == id);
                    }
                    catch (InvalidOperationException)
                    {
                        return "No matching id is found";
                    }
                    if (command == save)
                    {
                        SaveFileAsync(selectedFile);
                        return "I'll save it, don't you worry.";
                    }
                    else
                    {
                        SendFileAsync(message.Chat, selectedFile);
                        return "Here is your order, have a nice day!";
                    }
                default:
                    return $"What did you mean by \"{message.Text}\"";
            }
        }

        private static async void SaveFileAsync(Message selectedFile)
        {
            var file = await botClient.GetFileAsync(selectedFile.FileId);

            var filename = filesDir + selectedFile.FilePath;
            using (var fileStream = File.Open(filename, FileMode.Create))
                await botClient.DownloadFileAsync(file.FilePath, fileStream);
        }

        static async void SendFileAsync(Chat chat, Message message)
        {
            switch (message.Type)
            {
                case MessageType.Video:
                    await botClient.SendVideoAsync(chat, new InputOnlineFile(message.FileId));
                    break;
                case MessageType.VideoNote:
                    await botClient.SendVideoNoteAsync(chat, new InputOnlineFile(message.FileId));
                    break;
                case MessageType.Photo:
                    await botClient.SendPhotoAsync(chat, new InputOnlineFile(message.FileId));
                    break;
                case MessageType.Audio:
                    await botClient.SendAudioAsync(chat, new InputOnlineFile(message.FileId));
                    break;
                case MessageType.Voice:
                    await botClient.SendVoiceAsync(chat, new InputOnlineFile(message.FileId));
                    break;
                case MessageType.Document:
                    await botClient.SendDocumentAsync(chat, new InputOnlineFile(message.FileId));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public struct Message
        {
            public string FileId;
            public string FilePath;
            public string Caption;
            public MessageType Type;
        }
    }
}
