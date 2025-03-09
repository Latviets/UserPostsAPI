import { test, expect } from '@playwright/test';

test('Fetch and display user details from API', async ({ page }) => {
    await page.goto('/');

    await page.fill('#userIdInput', '1');

    await Promise.all([
        page.click('#fetchUserButton'),
        page.waitForResponse('**/api/users/1'),
    ]);

    await page.waitForFunction(() =>
        document.querySelector('#userDetails')?.textContent?.includes('Edvins')
    );

    const userDetails = await page.textContent('#userDetails');
    expect(userDetails).toContain('Edvins');
    expect(userDetails).toContain('edvins@example.com');
});

test('Fetch and display user posts from API', async ({ page }) => {
    await page.goto('/');

    await page.fill('#userIdInput', '1');

    await Promise.all([
        page.click('#fetchUserPostsButton'),
        page.waitForResponse('**/api/users/1/posts'),
    ]);

    await page.waitForFunction(() =>
        document.querySelector('#userPosts')?.textContent?.includes('first post')
    );

    const userPosts = await page.textContent('#userPosts');
    expect(userPosts).toContain('first post');
});

test('Display error for negative user ID when fetching user data', async ({ page }) => {
    await page.goto('/');

    await page.fill('#userIdInput', '-1');
    await page.click('#fetchUserButton');

    const errorMessage = await page.textContent('#errorMessage');
    expect(errorMessage).toContain('Invalid User ID. User ID cannot be a negative number.');
});

test('Display error for negative user ID when fetching user posts', async ({ page }) => {
    await page.goto('/');

    await page.fill('#userIdInput', '-1');
    await page.click('#fetchUserPostsButton');

    const errorMessage = await page.textContent('#errorMessage');
    expect(errorMessage).toContain('Invalid User ID. User ID cannot be a negative number.');
});

test('Display an alert for non-numeric user ID input - get user button', async ({ page }) => {
    page.on('dialog', async (dialog) => {
        expect(dialog.message()).toBe('Please enter a valid user ID.');
        await dialog.dismiss();
    });

    await page.goto('/');

    await page.fill('#userIdInput', '');
    await page.type('#userIdInput', 'abc');
    await page.click('#fetchUserButton');
});

test('Display an alert for non-numeric user ID input - get user posts button', async ({ page }) => {
    page.on('dialog', async (dialog) => {
        expect(dialog.message()).toBe('Please enter a valid user ID.');
        await dialog.dismiss();
    });

    await page.goto('/');

    await page.fill('#userIdInput', '');
    await page.type('#userIdInput', 'abc');
    await page.click('#fetchUserPostsButton');
});

test('Display error for non-existing user ID - get user button', async ({ page }) => {
    await page.route('/api/users/*', route => {
        route.fulfill({
            status: 404,
            contentType: 'application/json',
            body: JSON.stringify({ message: 'User not found' }),
        });
    });

    await page.goto('/');

    await page.fill('#userIdInput', '999');
    await Promise.all([
        page.click('#fetchUserButton'),
        page.waitForResponse('**/api/users/999'),
    ]);

    await page.waitForSelector('#errorMessage');
    const errorMessage = await page.textContent('#errorMessage');
    expect(errorMessage).toContain('try again');
});

test('Display error for non-existing user ID - get user posts button', async ({ page }) => {
    await page.route('**/api/users/999/posts', route => {
        route.fulfill({
            status: 404,
            contentType: 'application/json',
            body: JSON.stringify({ message: 'No posts found' }),
        });
    });

    await page.goto('/');

    await page.fill('#userIdInput', '999');
    await Promise.all([
        page.click('#fetchUserPostsButton'),
        page.waitForResponse('**/api/users/999/posts'),
    ]);

    await page.waitForSelector('#errorMessage');
    const errorMessage = await page.textContent('#errorMessage');
    expect(errorMessage).toContain('not found');
});

test('Display error for generic server error (500)', async ({ page }) => {
    await page.route('**/api/users/*', async (route) => {
        route.fulfill({
            status: 500,
            contentType: 'application/json',
            body: 'Internal Server Error',
        });
    });

    await page.goto('/');
    await page.fill('#userIdInput', '1');
    await page.click('#fetchUserButton');

    await page.waitForSelector('#errorMessage:visible');

    const errorMessage = await page.textContent('#errorMessage');
    expect(errorMessage).toContain('Error: 500 - Internal Server Error');
});

test('Handle slow API response gracefully', async ({ page }) => {
    await page.route('**/api/users/1', route => {
        setTimeout(() => {
            route.fulfill({
                status: 200,
                contentType: 'application/json',
                body: JSON.stringify({ id: 1, name: 'Edvins', email: 'edvins@example.com' }),
            });
        }, 3000);
    });

    await page.goto('/');

    const placeholderText = await page.textContent('#userDetails');
    expect(placeholderText).toBe('No user data available');

    await page.fill('#userIdInput', '1');
    await page.click('#fetchUserButton');

    await page.waitForSelector('#userDetails:has-text("Edvins")');

    const userDetails = await page.textContent('#userDetails');
    expect(userDetails).toContain('Edvins');
    expect(userDetails).toContain('edvins@example.com');
});

test('Clear error message on new request', async ({ page }) => {
    await page.goto('/');

    await page.fill('#userIdInput', '9999'); // Invalid user ID
    await page.click('#fetchUserButton');
    await page.waitForSelector('#errorMessage');

    const errorMessageBefore = await page.textContent('#errorMessage');
    expect(errorMessageBefore).toContain('User not found');

    await page.fill('#userIdInput', '1'); // Valid user ID
    await page.click('#fetchUserButton');

    const errorMessageAfter = await page.textContent('#errorMessage');
    expect(errorMessageAfter).toBe('');
});

test('Update data fields when a different user ID is requested', async ({ page }) => {
    await page.goto('/');

    await page.fill('#userIdInput', '1');
    await Promise.all([
        page.click('#fetchUserButton'),
        page.waitForResponse('**/api/users/1'),
    ]);

    await page.waitForFunction(() =>
        document.querySelector('#userDetails')?.textContent?.includes('Edvins')
    );

    const userDetailsBefore = await page.textContent('#userDetails');
    expect(userDetailsBefore).toContain('Edvins');
    expect(userDetailsBefore).toContain('edvins@example.com');

    await page.fill('#userIdInput', '2');
    await Promise.all([
        page.click('#fetchUserButton'),
        page.waitForResponse('**/api/users/2'),
    ]);

    await page.waitForFunction(() =>
        document.querySelector('#userDetails')?.textContent?.includes('Liga')
    );

    const userDetailsAfter = await page.textContent('#userDetails');
    expect(userDetailsAfter).toContain('Liga');
    expect(userDetailsAfter).toContain('liga@example.com');
});

test('Retain data fields when the same user ID is requested', async ({ page }) => {
    await page.goto('/');

    await page.fill('#userIdInput', '1');
    await page.click('#fetchUserButton');

    await page.waitForFunction(() =>
        document.querySelector('#userDetails')?.textContent?.includes('Edvins')
    );

    const userDetailsBefore = await page.textContent('#userDetails');
    expect(userDetailsBefore).toContain('Edvins');

    await page.click('#fetchUserPostsButton');

    await page.waitForFunction(() =>
        document.querySelector('#userPosts')?.textContent?.includes('My first post')
    );

    const userDetailsAfter = await page.textContent('#userDetails');
    expect(userDetailsAfter).toContain('Edvins');

    const userPosts = await page.textContent('#userPosts');
    expect(userPosts).toContain('My first post');
});