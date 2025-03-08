// Update the browser URL with query parameters
export function updateUrlWithQueryParam(key, value) {
    if (history.pushState) {
        const url = new URL(window.location);
        if (value) {
            url.searchParams.set(key, value);
        } else {
            url.searchParams.delete(key);
        }
        history.pushState({ path: url.href }, '', url.href);
    } else {
        console.warn("History API is not supported in this browser.");
    }
}

// Debounce function to prevent rapid repeated calls
export function debounce(func, delay) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => func(...args), delay);
    };
}
