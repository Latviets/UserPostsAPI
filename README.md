# UserPostsAPI

A lightweight **ASP.NET Web API** project that provides endpoints to manage user data and their associated posts. This repository includes an **integrated JavaScript front-end**, offering a seamless interface for users to interact with the API. It is designed for simplicity and serves as a foundational project for learning and implementing RESTful APIs with C# and JavaScript.

![ASP NET WEB API with JS integration](https://github.com/user-attachments/assets/c64d0701-fc6c-4fd5-b296-3ad3cc92ce28)

---

## Features

- **Get User by ID**: Retrieve detailed information about a user by their unique ID.
- **Get User Posts**: Fetch all posts associated with a specific user ID.
- **Reset Displayed Data**: Clear or reset the currently displayed data for testing or refreshing purposes.
- **JavaScript Front-End Integration**: A lightweight front-end to interact with the API endpoints.

---

## Endpoints

### 1. **Get User by ID**
- **URL**: `/api/users/{id}`
- **Method**: `GET`
- **Description**: Fetches user details based on the unique user ID.

### 2. **Get User Posts**
- **URL**: `/api/users/{id}/posts`
- **Method**: `GET`
- **Description**: Retrieves all posts created by a specific user ID.

---

## Technologies Used

- **Framework**: ASP.NET Core
- **Database**: Local SQL Server database (for production data storage)
- **Front-End**: JavaScript
- **Language**: C#
---

