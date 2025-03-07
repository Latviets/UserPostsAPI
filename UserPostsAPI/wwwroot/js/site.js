const apiUrl = "https://localhost:7053/api/users"; // Base API URL

// Fetch a user by ID
async function getUserById(userId) {
    const errorContainer = document.getElementById("errorMessage");
    try {
        console.log(`Fetching user with ID: ${userId} from ${apiUrl}/${userId}`);
        const response = await fetch(`${apiUrl}/${userId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        console.log("Response:", response);

        if (!response.ok) {
            if (response.status === 404) {
                throw new Error("User not found. Please check the ID.");
            } else {
                throw new Error(`Error: ${response.status} - ${response.statusText}`);
            }
        }

        const user = await response.json();
        console.log("User data:", user);

        // Replace placeholder with user details and hide error message
        document.getElementById("userDetails").innerHTML = `
            <p><strong>ID:</strong> ${user.id}</p>
            <p><strong>Name:</strong> ${user.name}</p>
            <p><strong>Email:</strong> ${user.email}</p>
            <p><strong>Address:</strong> ${user.address}</p>
        `;
        errorContainer.style.display = "none"; // Hide error message
    } catch (error) {
        console.error("Failed to fetch user:", error);

        // Show error message
        errorContainer.textContent = error.message;
        errorContainer.style.display = "block";

        // Clear user details
        document.getElementById("userDetails").innerHTML = "No user data available";
    }
}


// Event listener for "Get User" button
document.getElementById("fetchUserButton").addEventListener("click", async () => {
    const userId = document.getElementById("userIdInput").value;

    if (!userId) {
        alert("Please enter a valid user ID");
        return;
    }

    await getUserById(userId);

    updateUrlWithQueryParam("userId", userId);
});

// Fetch all posts by a specific user ID
async function getUserPosts(userId) {
    const errorContainer = document.getElementById("errorMessage");
    try {
        const response = await fetch(`${apiUrl}/${userId}/posts`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            if (response.status === 404) {
                throw new Error("Posts not found for the given User ID.");
            } else {
                throw new Error(`Error: ${response.status}`);
            }
        }

        const posts = await response.json();

        // Clear placeholder and populate table with posts
        const userPostsContainer = document.getElementById("userPosts");
        userPostsContainer.innerHTML = ""; // Clear placeholder content
        posts.forEach(post => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${post.id}</td>
                <td>${post.postContent}</td>                
            `;
            userPostsContainer.appendChild(row);
        });

        errorContainer.style.display = "none"; // Hide error message
    } catch (error) {
        console.error("Failed to fetch user posts:", error);

        // Show error message
        errorContainer.textContent = error.message;
        errorContainer.style.display = "block";

        // Clear user posts
        document.getElementById("userPosts").innerHTML = `
            <tr>
                <td colspan="2">No posts available</td>
            </tr>
        `;
    }
}

// Event listener for "Get User Posts" button
document.getElementById("fetchUserPostsButton").addEventListener("click", async () => {
    const userId = document.getElementById("userIdInput").value;

    if (!userId) {
        alert("Please enter a valid user ID");
        return;
    }

    await getUserPosts(userId);

    updateUrlWithQueryParam("userId", userId);
    updateUrlWithQueryParam("view", "posts");
});

// Reset displayed data
function resetData() {
    // Restore placeholders
    document.getElementById("userDetails").innerHTML = "No user data available";
    document.getElementById("userPosts").innerHTML = `
        <tr>
            <td colspan="2">No posts available</td>
        </tr>
    `;
    document.getElementById("userIdInput").value = ""; // Clear input field

    // Hide error message
    const errorContainer = document.getElementById("errorMessage");
    errorContainer.style.display = "none";
    errorContainer.textContent = ""; // Clear any existing message
}


// Event listener for the "Reset" button
document.getElementById("resetButton").addEventListener("click", () => {
    resetData();
    updateUrlWithQueryParam("userId", null);
    updateUrlWithQueryParam("view", null);
});

// Update the URL with query parameters or clear them
function updateUrlWithQueryParam(key, value) {
    if (history.pushState) {
        const url = new URL(window.location);
        if (value) {
            url.searchParams.set(key, value);
        } else {
            url.searchParams.delete(key);
        }
        history.pushState({ path: url.href }, '', url.href); // Push the new URL
    } else {
        console.warn("History API is not supported in this browser.");
    }
}

// Automatically fetch user data if "userId" exists in the URL
window.addEventListener("DOMContentLoaded", async () => {
    const params = new URLSearchParams(window.location.search);
    const userIdFromUrl = params.get("userId");
    const viewFromUrl = params.get("view");

    if (userIdFromUrl) {
        document.getElementById("userIdInput").value = userIdFromUrl; // Pre-fill the input field
        await getUserById(userIdFromUrl);

        if (viewFromUrl === "posts") {
            await getUserPosts(userIdFromUrl); // Fetch user posts if "view=posts" is present
        }
    }
});
