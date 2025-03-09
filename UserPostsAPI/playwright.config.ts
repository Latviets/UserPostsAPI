import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
    testDir: './tests', // Directory where your test files are located
    timeout: 70000,
    fullyParallel: true, // Run tests in parallel
    retries: 1, // Retry failed tests once
    reporter: 'html', // Generate an HTML report
    use: {
        baseURL: 'https://localhost:7053', // Set your API base URL
        headless: true, // Run tests in headless mode (no browser UI)
        ignoreHTTPSErrors: true, // Ignore SSL certificate issues for localhost
        trace: 'on-first-retry', // Collect trace for debugging failed tests
    },
    projects: [
        {
            name: 'chromium', // Test in Google Chrome (via Chromium)
            use: { ...devices['Desktop Chrome'] },
        },
        {
            name: 'firefox', // Test in Mozilla Firefox
            use: { ...devices['Desktop Firefox'] },
        },
        {
            name: 'webkit', // Test in WebKit (Safari)
            use: { ...devices['Desktop Safari'] },
        },
    ],
});