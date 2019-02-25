using BlueDB.Communication.Entity;
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
            _commands.Add(new WithDatabaseCommand
            {
                DatabaseName = databaseName
            });

            return this;
        }

        public RequestBuilder WithTable(string tableName, bool clearSelection = false)
        {
            _commands.Add(new WithTableCommand
            {
                TableName = tableName,
                ClearSelection = clearSelection
            });

            return this;
        }

        public RequestBuilder Set(params Property[] properties)
        {
            _commands.Add(new SetCommand
            {
                Properties = properties
            });

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

        public RequestBuilder Select()
        {
            _commands.Add(new SelectCommand
            {
            });

            return this;
        }
    }
}