# Blood-Bank Management System - Database based Application
## Introduction
Every year our nation requires about 4 Cr. units of blood, out of which only 5 Lakh units of blood are available. It is not that, people do not want to donate blood. Often they are unaware of the need and also they do not have a proper facility to enquire about it. As a result, needy people end up going through a lot of pain. To solve this problem we choose this project.

## Problem Statement
The ‘BLOOD BANK MANAGEMENT SYSTEM’ project is to interconnect all the blood banks, hospitals, donors into a single network, validation, store various data and information of blood of each individual. This system is used to store data over a centralized server which consists of database where the individual’s information cannot be accessed by a third party.

## ER-Diagram
![Image of ER-Diagram](https://github.com/Revanthpn/BloodBank/blob/master/BloodBank/BloodBank/Resources/ER-diagram.PNG)

## ER_Mapping
![Image of ER-Diagram](https://github.com/Revanthpn/BloodBank/blob/master/BloodBank/BloodBank/Resources/ER-Mapping.PNG)

## Connecting to a SQLITE database
Install-Package System.Data.SQLite -Version 3 
https://www.nuget.org/packages/System.Data.SQLite
```
using(SQLiteConnection con= new SQLiteConnection(@"Data Source=D:\test.db;"))
    {
        conn.Open();
        SQLiteCommand command = new SQLiteCommand("Select * from yourTable", conn);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        Console.WriteLine(reader["YourColumn"]);
        reader.Close();
    }
```
