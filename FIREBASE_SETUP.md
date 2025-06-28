# Firebase Project Setup Guide

This guide provides detailed steps to set up Firebase for your MyNorthwind project, including both Firebase Hosting (for static site) and Firebase Cloud Messaging (for push notifications).

## 🎯 Firebase Services Used

1. **Firebase Hosting** - For static website deployment
2. **Firebase Cloud Messaging (FCM)** - For push notifications (already integrated in your app)

## 📋 Prerequisites

- Google account
- Node.js installed (for Firebase CLI)
- GitHub repository access

## 🚀 Step-by-Step Setup

### Step 1: Create Firebase Project

1. **Go to Firebase Console**
   - Visit [https://console.firebase.google.com/](https://console.firebase.google.com/)
   - Sign in with your Google account

2. **Create New Project**
   - Click "Create a project" or "Add project"
   - Enter project name: `mynorthwind-api` (or your preferred name)
   - Click "Continue"

3. **Configure Google Analytics (Optional)**
   - Choose whether to enable Google Analytics
   - If enabled, select an Analytics account or create new one
   - Click "Create project"

4. **Wait for Project Creation**
   - Firebase will set up your project
   - Click "Continue" when ready

### Step 2: Enable Firebase Hosting

1. **Navigate to Hosting**
   - In your Firebase project console, click "Hosting" in the left sidebar
   - Click "Get started"

2. **Install Firebase CLI** (if not already installed)
   ```bash
   npm install -g firebase-tools
   ```

3. **Login to Firebase**
   ```bash
   firebase login
   ```
   - This will open a browser window for authentication
   - Sign in with the same Google account

4. **Initialize Firebase in Your Project**
   ```bash
   cd /path/to/your/MyNorthwind/project
   firebase init hosting
   ```

5. **Configure Hosting**
   - Select your project: `mynorthwind-api`
   - Public directory: `firebase-public` (this will be created by the GitHub Action)
   - Configure as single-page app: `No`
   - Set up automatic builds: `No` (we'll use GitHub Actions)

### Step 3: Get Firebase CI Token

1. **Generate CI Token**
   ```bash
   firebase login:ci
   ```
   - This will open a browser for authentication
   - Copy the token that appears in the terminal

2. **Save the Token**
   - Keep this token secure - you'll need it for GitHub Actions
   - The token looks like: `1//0e...` (long string)

### Step 4: Get Project ID

1. **Find Your Project ID**
   - In Firebase Console, click the gear icon ⚙️ next to "Project Overview"
   - Select "Project settings"
   - Copy the "Project ID" (e.g., `mynorthwind-api-12345`)

### Step 5: Configure GitHub Secrets

1. **Go to Your GitHub Repository**
   - Navigate to your MyNorthwind repository on GitHub
   - Click "Settings" tab

2. **Add Repository Secrets**
   - Click "Secrets and variables" → "Actions"
   - Click "New repository secret"

3. **Add Required Secrets**
   
   **Secret 1: FIREBASE_TOKEN**
   - Name: `FIREBASE_TOKEN`
   - Value: The CI token from Step 3
   
   **Secret 2: FIREBASE_PROJECT_ID**
   - Name: `FIREBASE_PROJECT_ID`
   - Value: Your project ID from Step 4

### Step 6: Verify Firebase Configuration

1. **Check firebase.json**
   Your `firebase.json` should look like this:
   ```json
   {
     "hosting": {
       "public": "firebase-public",
       "ignore": [
         "firebase.json",
         "**/.*",
         "**/node_modules/**"
       ],
       "rewrites": [
         {
           "source": "**",
           "destination": "/index.html"
         }
       ],
       "headers": [
         {
           "source": "**/*.@(js|css)",
           "headers": [
             {
               "key": "Cache-Control",
               "value": "max-age=31536000"
             }
           ]
         }
       ]
     }
   }
   ```

2. **Test Local Deployment** (Optional)
   ```bash
   # Create test directory
   mkdir firebase-public
   echo "<h1>Test</h1>" > firebase-public/index.html
   
   # Deploy locally
   firebase deploy --only hosting
   ```

## 🔧 Firebase Cloud Messaging Setup (Already Integrated)

Your application already uses Firebase Cloud Messaging for push notifications. To complete the setup:

### Step 1: Get Service Account Key

1. **Go to Project Settings**
   - In Firebase Console, click gear icon → "Project settings"
   - Go to "Service accounts" tab

2. **Generate New Private Key**
   - Click "Generate new private key"
   - Click "Generate key"
   - Download the JSON file

3. **Add to GitHub Secrets**
   - Name: `SERVICE_ACCOUNT_PATH`
   - Value: The path to your service account JSON file (for Cloud Run deployment)

### Step 2: Configure Environment Variables

For local development, add to your environment:
```bash
export SERVICE_ACCOUNT_PATH="/path/to/your/serviceAccountKey.json"
```

## 🚀 Deployment

### Automatic Deployment
Once configured, your GitHub Actions workflow will automatically:
1. Build your .NET application
2. Create a static HTML page with API documentation
3. Deploy to Firebase Hosting

### Manual Deployment
```bash
# Build and deploy manually
firebase deploy --only hosting
```

## 📊 Monitoring

### View Deployments
1. Go to Firebase Console → Hosting
2. View deployment history and status
3. Check analytics and performance

### View Logs
```bash
# View hosting logs
firebase hosting:log
```

## 🔍 Troubleshooting

### Common Issues

1. **"Project not found"**
   - Verify your `FIREBASE_PROJECT_ID` secret
   - Check that you're using the correct project ID

2. **"Authentication failed"**
   - Regenerate your Firebase CI token
   - Update the `FIREBASE_TOKEN` secret

3. **"Permission denied"**
   - Ensure you're logged in with the correct Google account
   - Check that you have access to the Firebase project

4. **"Build failed"**
   - Check that the `firebase-public` directory is being created
   - Verify the `firebase.json` configuration

### Debug Commands
```bash
# Check Firebase CLI version
firebase --version

# List your projects
firebase projects:list

# Check current project
firebase use

# Test deployment locally
firebase serve --only hosting
```

## 📱 Testing Push Notifications

1. **Register Device Token**
   ```bash
   curl -X POST https://your-api-url/api/device/register \
     -H "Content-Type: application/json" \
     -d '{"token": "your-fcm-device-token"}'
   ```

2. **Create Order to Trigger Notification**
   ```bash
   curl -X POST https://your-api-url/api/orders \
     -H "Content-Type: application/json" \
     -d '{"customerId": "CUST01", "orderDate": "2024-01-15T10:00:00Z"}'
   ```

## 🔐 Security Best Practices

1. **Keep Tokens Secure**
   - Never commit tokens to version control
   - Use GitHub Secrets for all sensitive data
   - Rotate tokens regularly

2. **Limit Access**
   - Use service accounts with minimal required permissions
   - Regularly review access permissions

3. **Monitor Usage**
   - Set up billing alerts
   - Monitor Firebase usage in console

## 📚 Additional Resources

- [Firebase Hosting Documentation](https://firebase.google.com/docs/hosting)
- [Firebase CLI Reference](https://firebase.google.com/docs/cli)
- [GitHub Actions with Firebase](https://github.com/w9jds/firebase-action)
- [Firebase Security Rules](https://firebase.google.com/docs/rules) 