# MVC .Net Core App with Microsoft's Identity

## Description

A simple mvc web client that authenticate users using Microsoft Identity and uses an email service to send confirmation emails and password resets, once the user is authenticated it will give the user access to see a table with a paginated list of employees. When creating a new user, an upload of a profile image is done in order to see the employee's picture in the table, these images are saved as a static content in an images folder. The table Employee saves the file's name in order to get the correct image pertaining to the record.

## Email Service Use

In order to use the email service for user confirmation, you must allow the application access to an email provider, by using your email's credentials, you can perform an email confirmation as long as the email provider allows access to the application.

## Migration of Database

The application contains the ability to migrate seed data, to allow an easier migration, the commands are provided and a generated sql script is also available.

## Google's External Login

A google external login button was provided for experimental purposes, the callback login however doesn't execute as expected, this is due to google needing time to approve the use of an oauth2 client with a callback url to the applications's localhost of the IIS express server.
