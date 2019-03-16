﻿using System;
using System.Collections.Generic;
using Shop.Shared.Entities;
using System.Data.SqlClient;
using Typography = Shop.Shared.Constants.TypographyConstants;
using SqlConst = Shop.Data.Constants.SqlQueryConstants;
using Shop.Data.DataContext.Interfaces;
using Shop.Shared.Helpers;
using Shop.Shared.Entities.Enums;

namespace Shop.Data.DataContext.Realization.MsSql
{
    public class UserContext : IUserContext
    {
        public User GetUser(SqlDataReader reader)
        {
            return new User
            {
                Login = (string)reader["Login"],
                Email = (string)reader["Email"],
                PhoneNumber = (string)reader["PhoneNumber"],
                Role = RoleHelper.ConvertToRoleType((int)reader["RoleId"])
            };
        }

        public User GetAuthorizedUser(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            using (var connection = new SqlConnection(SqlConst.ConnectionToConsoleShopString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT TOP 1 * FROM [User] WHERE [Login] = @login AND [Password] = @password", connection); //соединить с roles
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return GetUser(reader);
                }
                else
                {
                    throw new Exception("Неверный логин или пароль");
                }
            }
        }

        public void Save(User user)
        {
            using (var connection = new SqlConnection(SqlConst.ConnectionToConsoleShopString))
            {
                connection.Open();
                    var command = new SqlCommand($"INSERT INTO [User] (RoleId, Login, Password, Email, PhoneNumber) VALUES (3, @login, @password, @email, @phonenumber)", connection);
                    command.Parameters.AddWithValue("@login", user.Login);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@phonenumber", user.PhoneNumber);
                    command.ExecuteNonQuery();
            }
        }

        public IReadOnlyCollection<User> GetAll()
        {
            List<User> returnList = new List<User>();
            using (var connection = new SqlConnection(SqlConst.ConnectionToConsoleShopString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM [User]", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        returnList.Add(GetUser(reader));
                    }
                    catch(SqlException) { }
                }
                return returnList;
            }
        }

        public User GetById(int id)
        {
            using (var connection = new SqlConnection(SqlConst.ConnectionToConsoleShopString))
            {
                connection.Open();
                List<User> products = new List<User>();
                string query = SqlConst.SelectAllProductInDbString + Typography.NewLine + $"WHERE [Id] = {id}";
                var command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                return GetUser(reader);
            }
        }

        public void DeleteById(int id)
        {
            using (var connection = new SqlConnection(SqlConst.ConnectionToConsoleShopString))
            {
                connection.Open();
                var command = new SqlCommand($"DELETE [User] WHERE [Id] = {id}", connection);
                command.ExecuteNonQuery();
            }
        }

        public int GetIdByUser(User user)
        {
            using (var connection = new SqlConnection(SqlConst.ConnectionToConsoleShopString))
            {
                connection.Open();
                string query = SqlConst.SelectAllProductInDbString + Typography.NewLine + $"WHERE [Login] = {user.Login}";
                var command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                return (int)reader["Id"];
            }
        }
    }
}
