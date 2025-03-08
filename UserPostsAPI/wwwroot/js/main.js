import { getUserById, getUserPosts } from './api.js';
import { showUserDetails, showUserPosts, showError, resetUI } from './ui.js';
import { updateUrlWithQueryParam } from './utils.js';

// Cached DOM elements
const userDetailsContainer = document.getElementById("userDetails");
const userPostsContainer = document.getElementById("userPosts");
const errorContainer = document.getElementById("errorMessage");
const userIdInput = document.getElementById("userIdInput");
const fetchUserButton = document.getElementById("fetchUserButton");
const fetchUserPostsButton = document.getElementById("fetchUserPostsButton");
const resetButton = document.getElementById("resetButton");
let currentUserId = null;

// Event listeners
fetchUserButton.addEventListener("click", async () => {
    const userId = userIdInput.value;

    if (!userId) {
        alert("Please enter a valid user ID.");
        return;
    }

    if (userId !== currentUserId) {
        resetUserPosts(); // Clear posts only if the user ID is different
    }

    try {
        const user = await getUserById(userId);
        showUserDetails(user, userDetailsContainer);
        currentUserId = userId; // Update the currently displayed user ID
        updateUrlWithQueryParam("userId", userId);
    } catch (error) {
        console.error("Error caught in main.js:", error.message);
        showError(error.message, errorContainer);
    }
});

fetchUserPostsButton.addEventListener("click", async () => {
    const userId = userIdInput.value;

    if (!userId) {
        alert("Please enter a valid user ID.");
        return;
    }

    if (userId !== currentUserId) {
        resetUserDetails(); // Clear user details only if the user ID is different
    }

    try {
        const posts = await getUserPosts(userId);
        showUserPosts(posts, userPostsContainer);
        currentUserId = userId; // Update the currently displayed user ID
        updateUrlWithQueryParam("userId", userId);
        updateUrlWithQueryParam("view", "posts");
    } catch (error) {
        console.error("Error caught in main.js:", error.message);
        showError(error.message, errorContainer);
    }
});

resetButton.addEventListener("click", () => {
    resetUI(userDetailsContainer, userPostsContainer, errorContainer);
    currentUserId = null;
    updateUrlWithQueryParam("userId", null);
    updateUrlWithQueryParam("view", null);
});


// Automatically fetch data based on URL parameters
window.addEventListener("DOMContentLoaded", async () => {
    const params = new URLSearchParams(window.location.search);
    const userIdFromUrl = params.get("userId");
    const viewFromUrl = params.get("view");

    if (userIdFromUrl) {
        userIdInput.value = userIdFromUrl;

        try {
            const user = await getUserById(userIdFromUrl);
            showUserDetails(user, userDetailsContainer);

            if (viewFromUrl === "posts") {
                const posts = await getUserPosts(userIdFromUrl);
                showUserPosts(posts, userPostsContainer);
            }
        } catch (error) {
            showError(error.message, errorContainer);
        }
    }
});

function resetUserDetails() {
    userDetailsContainer.innerHTML = "No user data available";
}

function resetUserPosts() {
    userPostsContainer.innerHTML = `
        <tr>
            <td colspan="2">No posts available</td>
        </tr>
    `;
}
function resetForNewRequest() {
    resetUserDetails();
    resetUserPosts();
}
