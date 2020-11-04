# MVC .Net Core App with Microsoft's Identity

## Email Service Use

In order to use the email service for user confirmation, you must allow the application access to an email provider, by using your email's credentials, you can perform an email confirmation as long as the email provider allows access to the application.

## Migration of Database

The application contains the ability to migrate seed data, to allow an easier migration, the commands are provided and a generated sql script is also available.

## Description

The application is really basic, it allows user registration and login using microsoft's identity and also has a page to see employees, search for their names and basic crud functionalities.

## Google's External Login

A google external login button was provided for experimental purposes, the callback login however doesn't execute as expected, this is due to google needing time to approve the use of an oauth2 client with a callback url to the applications's localhost of the IIS express server.
