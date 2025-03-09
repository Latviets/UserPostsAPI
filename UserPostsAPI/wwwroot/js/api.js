// Base API URL
const apiUrl = "https://localhost:7053/api/users";

// Fetch a user by ID
export async function getUserById(userId) {
    if (userId < 0) {
        throw new Error("Invalid User ID. User ID cannot be a negative number.");
    }

    try {
        const response = await fetch(`${apiUrl}/${userId}`, {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' },
        });

        await handleResponseErrors(response, "User not found. Please check the ID and try again.");

        const data = await response.json();
        if (!data || Object.keys(data).length === 0) {
            throw new Error("No user data available.");
        }

        return data;
    } catch (error) {
        console.error("An error occurred while fetching user data:", error.message);
        throw error;
    }
}

// Fetch user posts by ID
export async function getUserPosts(userId) {
    if (userId < 0) {
        throw new Error("Invalid User ID. User ID cannot be a negative number.");
    }

    try {
        const response = await fetch(`${apiUrl}/${userId}/posts`, {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' },
        });

        await handleResponseErrors(response, "Posts not found.");

        const posts = await response.json();
        if (!posts || posts.length === 0) {
            throw new Error("No posts found for the given user ID.");
        }

        return posts;
    } catch (error) {
        console.error("An error occurred while fetching user posts:", error.message);
        throw error;
    }
}

async function handleResponseErrors(response, notFoundMessage) {
    if (!response.ok) {
        let errorMessage = `Error: ${response.status}`;
        if (response.status === 404) {
            errorMessage = notFoundMessage;
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
}