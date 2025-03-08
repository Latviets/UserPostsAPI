// Base API URL
const apiUrl = "https://localhost:7053/api/users";

// Fetch a user by ID
export async function getUserById(userId) {
    if (userId < 0) {
        throw new Error("Invalid User ID. User ID cannot be a negative number.");
    }

    const response = await fetch(`${apiUrl}/${userId}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
    });

    if (!response.ok) {
        let errorMessage = `Error: ${response.status}`;
        if (response.status === 404) {
            errorMessage = "User not found. Please check the ID and try again."; // Custom error message for 404
        } else {
            try {
                const errorResponse = await response.text();
                if (errorResponse) {
                    errorMessage += ` - ${errorResponse}`;
                }
            } catch (error) {
                console.error("Failed to parse error response:", error);
            }
        }
        throw new Error(errorMessage);
    }

    return response.json(); // Return user data if successful
}



export async function getUserPosts(userId) {
    // Validation to block negative IDs
    if (userId < 0) {
        console.log("Validation triggered: Negative User ID is not allowed.");
        throw new Error("Invalid User ID. User ID cannot be a negative number.");
    }

    // Fetch user posts
    const response = await fetch(`${apiUrl}/${userId}/posts`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
    });

    if (!response.ok) {
        const errorText = response.status === 404 ? "Posts not found." : `Error: ${response.status}`;
        throw new Error(errorText);
    }

    return response.json();
}
