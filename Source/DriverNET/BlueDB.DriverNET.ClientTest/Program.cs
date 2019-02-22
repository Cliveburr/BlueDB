using BlueDB.Communication;
using BlueDB.Communication.Entity;
using BlueDB.Communication.Messages;
using BlueDB.Communication.Socket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BlueDB.DriverNET.ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new ClientConnection("127.0.0.1", 8011))
            {
                TestOne(connection);

                TestRefrence(connection);
            }

            Console.ReadKey();
        }

        static void TestOne(ClientConnection connection)
        {
            var accessDatabase = connection.CreateRequest()
                .WithDatabase("Test0")
                .Process();

            var inserOneToTableUserTest = connection.CreateRequest()
                .WithDatabase("Test0")
                .WithTable("UserTest")
                .Set(
                    new Property(PropertyType.Int32, "_id", 0),
                    new Property(PropertyType.String, "name", "teste numero um"),
                    new Property(PropertyType.Int32, "valor", 100)
                )
                .Set(
                    new Property(PropertyType.String, "name", "teste numero dois"),
                    new Property(PropertyType.Int32, "valor", 110)
                )
                .Set(
                    new Property(PropertyType.Int32, "_id", 0),
                    new Property(PropertyType.String, "name", "teste numero tres"),
                    new Property(PropertyType.Int32, "valor", 200)
                )
                .Process();

            //var selectInTableUserTest = connection.CreateRequest()
            //    .WithDatabase("Test0")
            //    .WithTable("UserTest")
            //    .Where("valor", 100)
            //    .Or().Where("valor", 110)
            //    .Select()
            //    .Process();

            //var removeFromTableUserTest = connection.CreateRequest()
            //    .WithDatabase("Test0")
            //    .WithTable("UserTest")
            //    .Where("valor", 100)
            //    .Remove()
            //    .Process();
        }

        static void TestRefrence(ClientConnection connection)
        {
            //// filtra um item da tabela UserTest
            //// cria um registro na tabela UserExtraData com um vinculo para o item filtrado
            //// e faz um update do item filtrado adicionado o link para o registro criado
            //var insertIntoUserExtraData = connection.CreateRequest()
            //    .WithDatabase("Test0")
            //    .WithTable("UserTest", true)    // coloca a tabela UserTest como selecionada e todos seus registros como selecionados (true força selecionar todos registros)
            //    .First("_id", 2)                // filtra a tabela UserTest e coloca o resultado como selecionado
            //    .WithTable("UserExtraData")     // coloca a tabela UserExtraData como selecionada
            //    .Set(new BlueProperty[]         // adiciona um registro na tabela UserExtraData (selecionada) com um link para o campo user (selecionado) e coloca esse registro como selecionado
            //    {
            //        new BlueProperty(PropertyType.Int32, "_id", 0),     // _id = 0 indica que é um registro novo, create
            //        new BlueProperty(PropertyType.String, "address", "rua qualquer lugar"),
            //        new BlueProperty(PropertyType.String, "zip", "14060-140"),
            //        new BlueProperty(PropertyType.Link, "user", "UserTest")
            //    })
            //    .WithTable("UserTest")         // coloca a tabela UserTest como selecionada
            //    .Set(new BlueProperty[]        // faz create/update das propriedades de todos registros selecionados da tabela UserTest
            //    {
            //        new BlueProperty(PropertyType.Link, "data", "UserExtraData")
            //    })
            //    .Process();

            //var updateOneToTableUserTest = connection.CreateRequest()
            //    .WithDatabase("Test0")
            //    .WithTable("Build")                             // coloca a tabela Build como selecionada e todos seus registros selecionados
            //    .Where("_id", Expression.Equal(10))             // filtra a tabela selecionada e coloca o resultado como selecionados
            //    .First()                                        // seleciona o primeiro item dos selecionados, emite erro se não tiver
            //    .WithTable("BuildModule")                       // coloca a tabela BuildModule como selecionada e todos seus registros selecionados
            //    //.WhereTableEqual("idBuild", "Build", "_id")     // filtra a tabela selecionada com a tabela "Build" e coloca o resultado selecionado
            //    .Select("Build")                                // retorna os registros selecionados da tabela Build
            //    .Select("BuildModule")                          // retorna os registros selecionados da tabela BuildModule
            //    .Where("idBuild", Expression.Equal(Expression.Table("Build", "_id")))
            //    .Process();

            //var selectLinkToTableUserTest = connection.CreateRequest()
            //    .WithDatabase("Test0")
            //    .WithTable("Build")
            //    .Where("_id", Expression.Equal(10))
            //    .First()
            //    .WithLink("modules")
            //    .Select("BuildModule")
            //    .Process();
        }
    }
}