# UserPostsAPI

A lightweight **ASP.NET Web API** project that provides endpoints to manage user data and their associated posts. This repository includes an **integrated JavaScript front-end**, offering a seamless interface for users to interact with the API. It is designed for simplicity and serves as a foundational project for learning and implementing RESTful APIs with C# and JavaScript.

![ASP NET WEB API with JS integration](https://github.com/user-attachments/assets/0c059a25-a03c-4f4e-a0ef-2bb21518e366)


## Features

- **Get User by ID**: Retrieve detailed information about a user by their unique ID.
- **Get User Posts**: Fetch all posts associated with a specific user ID.
- **Reset Displayed Data**: Clear or reset the currently displayed data for testing or refreshing purposes.
- **Integrated JavaScript Front-End**: Offers an intuitive, responsive front-end interface for interacting with API endpoints.
- **Error Handling and Validation**: Provides robust feedback to ensure consistent and meaningful interactions, including validation for invalid or missing user IDs.

---

### Key Highlights:
- Implements real-time DOM updates to reflect API interactions.
- Provides a clean UI for testing and exploring API functionality.
- Enhanced responsiveness and error management for a user-friendly experience.
### Technologies Used
   - Framework: ASP.NET Core for developing the RESTful API.
   - Database: Local SQL Server for storing production data.
   - Front-End: JavaScript for dynamic user interaction.
   - Language: C# for back-end functionality.

## **API Endpoints**

### **1. Get User by ID**
- **URL**: `/api/users/{id}`
- **Method**: `GET`
- **Description**: Fetches user details based on the unique user ID.
- **Response Example**:
- ```json
  {
      "id": 1,
      "name": "Edvins",
      "email": "edvins@example.com",
      "address": "Riga, Latvia"
  }

### 2. **Get User Posts**
- **URL**: `/api/users/{id}/posts`
- **Method**: `GET`
- **Description**: Retrieves all posts created by a specific user ID.
- **Response Example**:
- ```json
  {
    {
        "id": 1,
        "userId": 1,
        "postContent": "My first post"
    },
    {
        "id": 2,
        "userId": 1,
        "postContent": "Another post"
    }
  }

---
## Unit Tests
- Focus on testing individual components of the application, such as controllers and services.
- Validates functionality like data fetching, input validation, and response formatting.
- Example: Ensures /api/users/{id} correctly handles valid, invalid, and non-existent user IDs.

## Integration Tests
- Verify that different components work together seamlessly, including database connectivity and API endpoint functionality.
- Example: Ensures /api/users/{id}/posts retrieves posts from the database for a valid user ID and gracefully handles users with no posts.

## End-to-End Tests with Playwright
- Comprehensive UI testing to validate the functionality of the JavaScript front-end interacting with the API, ensuring consistent behavior across Chromium, Firefox, and WebKit.
### Example Scenarios:
- Happy Path: Successfully fetch user data and posts for valid IDs.
- Error Scenarios: Handle invalid or non-existent IDs by clearing fields and displaying meaningful error messages.
- Edge Cases: Validate empty input, non-numeric IDs, and error states.

## Integrated JavaScript Front-End
- The project features an easy-to-use JavaScript front-end designed to interact with the API seamlessly. It provides:
- Dynamic rendering of user data and posts.
- Error messages displayed for invalid user IDs or failed API requests.
- Smooth handling of data resets and updates.
