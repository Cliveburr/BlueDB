using BlueDB.Communication.Messages;
using BlueDB.Communication.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.DriverNET.Messages
{
    public class RequestBuilder
    {
        private ClientConnection _connection;
        private List<ICommand> _commands;

        public RequestBuilder(ClientConnection connection)
        {
            _connection = connection;
            _commands = new List<ICommand>();
        }

        public RequestBuilder WithDatabase(string databaseName)
        {
            return this;
        }

        public MessageReponse Process()
        {
            var request = new MessageRequest
            {
                Commands = _commands.ToArray()
            };

            return _connection.SendMessage(request).Result;
        }
    }
}