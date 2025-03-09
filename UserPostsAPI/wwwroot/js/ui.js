// Exported functions for UI updates
export function showUserDetails(user, userDetailsContainer) {
    userDetailsContainer.innerHTML = `
        <p><strong>ID:</strong> ${user.id}</p>
        <p><strong>Name:</strong> ${user.name}</p>
        <p><strong>Email:</strong> ${user.email}</p>
        <p><strong>Address:</strong> ${user.address}</p>
    `;
}

export function showUserPosts(posts, userPostsContainer) {
    userPostsContainer.innerHTML = posts.map(post => `
        <tr>
            <td>${post.id}</td>
            <td>${post.postContent}</td>
        </tr>
    `).join("");
}

export function showError(message, errorContainer) {
    errorContainer.innerHTML = `
        <div class="error">
            <p>${message}</p>
        </div>
    `;
    errorContainer.style.display = "block";
}

export function resetUI(userDetailsContainer, userPostsContainer, errorContainer) {
    userDetailsContainer.innerHTML = "No user data available";
    userPostsContainer.innerHTML = `
        <tr>
            <td colspan="2">No posts available</td>
        </tr>
    `;
    errorContainer.style.display = "none";
    errorContainer.innerHTML = "";
}