# Deployment Guide

This guide covers deploying your .NET 9 MyNorthwind API to different platforms using GitHub Actions.

## 🚀 Deployment Options

### Option 1: Firebase Hosting (Static Site)
- **Best for**: Frontend applications, static documentation
- **Limitation**: Cannot run .NET Web APIs directly
- **Solution**: Serves a static HTML page with API documentation

### Option 2: Google Cloud Run (Recommended for APIs)
- **Best for**: .NET Web APIs, containerized applications
- **Features**: Auto-scaling, pay-per-use, serverless
- **Perfect for**: Your MyNorthwind API

## 📋 Prerequisites

### For Firebase Hosting
1. Firebase project
2. Firebase CLI token
3. GitHub repository secrets

### For Google Cloud Run
1. Google Cloud project
2. Service account with necessary permissions
3. GitHub repository secrets

## 🔧 Setup Instructions

### Firebase Hosting Setup

#### 1. Create Firebase Project
1. Go to [Firebase Console](https://console.firebase.google.com/)
2. Create a new project or select existing one
3. Enable Hosting in the project

#### 2. Get Firebase Token
```bash
# Install Firebase CLI
npm install -g firebase-tools

# Login to Firebase
firebase login

# Generate CI token
firebase login:ci
```

#### 3. Configure GitHub Secrets
Go to your GitHub repository → Settings → Secrets and variables → Actions, and add:

- `FIREBASE_TOKEN`: Your Firebase CI token
- `FIREBASE_PROJECT_ID`: Your Firebase project ID

#### 4. Deploy
The workflow will automatically deploy when you push to main/master branch.

### Google Cloud Run Setup

#### 1. Create Google Cloud Project
1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select existing one
3. Enable required APIs:
   - Cloud Run API
   - Container Registry API
   - Cloud Build API

#### 2. Create Service Account
```bash
# Create service account
gcloud iam service-accounts create github-actions \
    --display-name="GitHub Actions Service Account"

# Grant necessary roles
gcloud projects add-iam-policy-binding YOUR_PROJECT_ID \
    --member="serviceAccount:github-actions@YOUR_PROJECT_ID.iam.gserviceaccount.com" \
    --role="roles/run.admin"

gcloud projects add-iam-policy-binding YOUR_PROJECT_ID \
    --member="serviceAccount:github-actions@YOUR_PROJECT_ID.iam.gserviceaccount.com" \
    --role="roles/storage.admin"

gcloud projects add-iam-policy-binding YOUR_PROJECT_ID \
    --member="serviceAccount:github-actions@YOUR_PROJECT_ID.iam.gserviceaccount.com" \
    --role="roles/iam.serviceAccountUser"

# Create and download key
gcloud iam service-accounts keys create key.json \
    --iam-account=github-actions@YOUR_PROJECT_ID.iam.gserviceaccount.com
```

#### 3. Configure GitHub Secrets
Add these secrets to your GitHub repository:

- `GCP_PROJECT_ID`: Your Google Cloud project ID
- `GCP_SA_KEY`: The entire content of the key.json file
- `CONNECTION_SQL`: Your SQL Server connection string
- `SERVICE_ACCOUNT_PATH`: Path to your Firebase service account JSON

#### 4. Deploy
The workflow will automatically deploy when you push to main/master branch.

## 🔄 Workflow Files

### Firebase Hosting Workflow
File: `.github/workflows/deploy-to-firebase.yml`

This workflow:
1. Builds your .NET application
2. Creates a static HTML page with API documentation
3. Deploys to Firebase Hosting

### Google Cloud Run Workflow
File: `.github/workflows/deploy-to-cloud-run.yml`

This workflow:
1. Builds your .NET application
2. Creates a Docker container
3. Pushes to Google Container Registry
4. Deploys to Cloud Run

## 🐳 Docker Configuration

### Dockerfile
The `Dockerfile` is optimized for Cloud Run:
- Multi-stage build for smaller images
- Non-root user for security
- Proper port configuration (8080)
- Production environment settings

### .dockerignore
Excludes unnecessary files to speed up builds:
- Build artifacts
- Git files
- IDE files
- Dependencies

## 🔐 Environment Variables

### Required for Both Deployments
- `CONNECTION_SQL`: SQL Server connection string
- `SERVICE_ACCOUNT_PATH`: Firebase service account path

### Firebase Hosting Specific
- `FIREBASE_TOKEN`: Firebase CI token
- `FIREBASE_PROJECT_ID`: Firebase project ID

### Google Cloud Run Specific
- `GCP_PROJECT_ID`: Google Cloud project ID
- `GCP_SA_KEY`: Service account key

## 📊 Monitoring and Logs

### Firebase Hosting
- View deployment status in Firebase Console
- Check hosting analytics and performance

### Google Cloud Run
- View logs: `gcloud logs read --service=mynorthwind-api`
- Monitor metrics in Cloud Console
- Set up alerts for errors and performance

## 🔧 Customization

### Modify Deployment Region
Edit the `REGION` variable in the Cloud Run workflow:
```yaml
env:
  REGION: us-central1  # Change to your preferred region
```

### Adjust Resource Limits
Modify Cloud Run deployment parameters:
```yaml
--memory 512Mi  # Adjust memory
--cpu 1         # Adjust CPU
--max-instances 10  # Adjust max instances
```

### Custom Domain
For Cloud Run:
```bash
gcloud run domain-mappings create \
  --service mynorthwind-api \
  --domain your-domain.com \
  --region us-central1
```

## 🚨 Troubleshooting

### Common Issues

#### Firebase Hosting
- **Build fails**: Check Firebase token and project ID
- **Deployment fails**: Verify firebase.json configuration

#### Google Cloud Run
- **Authentication fails**: Verify service account key
- **Build fails**: Check Dockerfile and .dockerignore
- **Runtime errors**: Check environment variables and logs

### Debug Commands
```bash
# Check Cloud Run logs
gcloud logs read --service=mynorthwind-api --limit=50

# Check service status
gcloud run services describe mynorthwind-api --region=us-central1

# Test locally
docker build -t mynorthwind .
docker run -p 8080:8080 mynorthwind
```

## 📈 Cost Optimization

### Firebase Hosting
- Free tier: 10GB storage, 360MB/day transfer
- Paid: $0.026/GB storage, $0.15/GB transfer

### Google Cloud Run
- Free tier: 2 million requests/month, 360,000 vCPU-seconds
- Paid: $0.00002400/vCPU-second, $0.00000250/GiB-second

## 🔄 CI/CD Best Practices

1. **Branch Protection**: Protect main/master branch
2. **Required Reviews**: Require PR reviews before merge
3. **Environment Secrets**: Use different secrets for staging/production
4. **Rollback Strategy**: Keep previous versions for quick rollback
5. **Monitoring**: Set up alerts for deployment failures

## 📚 Additional Resources

- [Firebase Hosting Documentation](https://firebase.google.com/docs/hosting)
- [Google Cloud Run Documentation](https://cloud.google.com/run/docs)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [.NET Docker Images](https://hub.docker.com/_/microsoft-dotnet) 