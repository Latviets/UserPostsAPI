import { test, expect, request } from '@playwright/test';

test('Verify API returns user details for user id 1', async () => {
    const apiContext = await request.newContext();

    const response = await apiContext.get('/api/users/1');
    expect(response.ok()).toBeTruthy();

    const user = await response.json();
    expect(user.name).toBe('Edvins');
    expect(user.email).toBe('edvins@example.com');
});

test('Verify API returns user details for user id 2', async () => {
    const apiContext = await request.newContext();

    const response = await apiContext.get('/api/users/2');
    expect(response.ok()).toBeTruthy();

    const user = await response.json();
    expect(user.name).toBe('Liga');
    expect(user.address).toBe('Ventspils, Liela Street 45-26');
});

test('Verify API response structure for user details', async () => {
    const apiContext = await request.newContext();

    const response = await apiContext.get('/api/users/1');
    expect(response.ok()).toBeTruthy();

    const user = await response.json();
    expect(user).toHaveProperty('id');
    expect(user).toHaveProperty('name');
    expect(user).toHaveProperty('email');
    expect(user).toHaveProperty('address');
});

test('Verify API handles non-existent user ID', async () => {
    const apiContext = await request.newContext();

    const response = await apiContext.get('/api/users/9999');
    expect(response.status()).toBe(404);

    const error = await response.json();
    expect(error.title).toBe('Not Found');
});

test('Verify API handles invalid user ID (negative number)', async () => {
    const apiContext = await request.newContext();

    const response = await apiContext.get('/api/users/-1');
    expect(response.status()).toBe(400);

    const error = await response.text();
    expect(error).toBe('Invalid user ID.');
});

test('Verify API handles invalid user ID (non-numeric)', async () => {
    const apiContext = await request.newContext();

    const response = await apiContext.get('/api/users/abc');
    expect(response.status()).toBe(400);

    const error = await response.json();

    expect(error.title).toBe('One or more validation errors occurred.');
    expect(error.errors.id).toContain("The value 'abc' is not valid.");
});

test('Verify API handles missing user ID', async () => {
    const apiContext = await request.newContext();

    const response = await apiContext.get('/api/users/');
    expect(response.status()).toBe(404);
});

test('Verify API returns posts for valid user ID', async () => {
    const apiContext = await request.newContext();

    const response = await apiContext.get('/api/users/1/posts');
    expect(response.ok()).toBeTruthy();

    const posts = await response.json();
    expect(Array.isArray(posts)).toBe(true);
    expect(posts.length).toBeGreaterThan(0);

    expect(posts[0].id).toBeDefined();
    expect(posts[0].postContent).toBeDefined();
});

test('Verify API handles posts request for non-existent user ID', async () => {
    const apiContext = await request.newContext();

    const response = await apiContext.get('/api/users/9999/posts');
    expect(response.status()).toBe(404);

    const error = await response.json();
    expect(error.title).toBe('Not Found');
});

test('Verify API handles invalid user ID for posts (negative number)', async () => {
    const apiContext = await request.newContext();

    const response = await apiContext.get('/api/users/-1/posts');
    expect(response.status()).toBe(400);

    const error = await response.text();
    expect(error).toBe('Invalid user ID.');
});

test('Verify API handles invalid user ID for posts (non-numeric)', async () => {
    const apiContext = await request.newContext();

    const response = await apiContext.get('/api/users/abc/posts');
    expect(response.status()).toBe(400);

    const error = await response.json();

    expect(error.title).toBe('One or more validation errors occurred.');
    expect(error.errors.id).toContain("The value 'abc' is not valid.");
});