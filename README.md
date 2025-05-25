# LogUp

A simple ASP.NET Core API for uploading and retrieving log files using S3-compatible storage (MinIO).

> **⚠️ Note**: This is a small side project and proof-of-concept. It is **not intended for production use** and lacks many features required for a production environment (authentication, authorization, input validation, error handling, etc.).

---

## Overview

LogUp provides a minimal REST API with two endpoints:
- **Upload**: Store log files in S3-compatible storage
- **Retrieve**: Fetch uploaded log files by UUID

Files are stored with a unique UUID for retrieval.

---

## Features

- File upload to S3-compatible storage (MinIO)
- UUID-based file retrieval
- Automatic bucket creation
- File expiry metadata (12 months)
- Docker containerization

---

## Getting Started

### Prerequisites

- Docker and Docker Compose
- .NET 9.0 SDK (for local development)

### Running with Docker Compose

1. Clone the repository
2. Run the application:

```shell
docker-compose up -d
```

This will start:
- LogUp API server on http://localhost:8080
- MinIO storage server on http://localhost:9000 
- Web interface: http://localhost:9001

---

### Example Usage
Upload a file:

```shell
curl -X POST -F "_=@logfile.txt" http://localhost:8080/upload
```
*Where logfile.txt is the file you are uploading.*

Retrieve a file:

```shell
curl http://localhost:8080/{uuid}
```

*Where **{uuid}** is the uuid you got after uploading the file*

---

### Configuration
The application uses **appsettings.json** for configuration:

- **AWS.ServiceURL**: MinIO endpoint URL
- **AWS.AccessKey/SecretKey**: MinIO credentials
- **AWS.ForcePathStyle**: Required for MinIO compatibility

---

### Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Storage**: MinIO
- **Containerization**: Docker
- **Language**: C#

---

### License
This project is licensed under the MIT License - see the LICENSE file for details.

---

### Contributing
This is a personal side project, but feel free to fork and experiment! Pull requests welcome for educational purposes.

---
*This project was created as a learning exercise and demonstration of basic ASP.NET Core API development with an S3-compatible storage.*