# 🎭 Mefisto Theatre - Community Blog Platform

![ASP.NET MVC](https://img.shields.io/badge/ASP.NET_MVC-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity_Framework-68217A?style=for-the-badge&logo=.net&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

## 📖 Overview
**Mefisto Theatre Blog** is a content management system (CMS) designed to help a local theatre reconnect with its community. The platform serves as a digital hub where theatre staff can share updates, behind-the-scenes stories, and announcements, while fostering audience engagement through a moderated comment system.

This project was developed as an advanced college assignment to demonstrate **Role-Based Access Control (RBAC)**, **Complex State Management**, and **Secure Data Administration**.

## ✨ Key Features

### 🏛️ For the Community (Public/Users)
* **Blog Feed:** Browse articles and announcements from the theatre.
* **Engagement:** Registered users can leave comments and discuss plays.
* **Interactive Contact:** Integrated **Google Maps API** on the contact page for easy location finding.
* **Intuitive Design:** Custom UI/UX matching the official Mefisto Theatre color palette.

### 🎭 For Theatre Staff
* **Content Creation:** dedicated tools to write, edit, and categorize blog posts.
* **Category Management:** Organize posts into relevant theatre topics (e.g., "Premieres", "Cast Interviews").

### 🛡️ For Administrators (The "Control Center")
* **User Governance:**
    * View all registered users and staff.
    * **Promote** users to Staff or Staff to Admin roles.
    * **Suspend** abusive users for 30 days (automated lockout logic).
* **Content Moderation:**
    * Delete inappropriate comments or posts instantly.
    * Create, Edit, and Delete blog categories.

## 🛠️ System Architecture
* **Frontend:** HTML5, CSS3 (Custom Mefisto Theme), Razor View Engine, Bootstarp.
* **Backend:** C# / ASP.NET MVC.
* **Data Access:** Entity Framework Core (Code-First).
* **Database:** SQL Server.

## 🚀 How to Run Locally

### Prerequisites
* Visual Studio 2022
* SQL Server Express (LocalDB)

### Installation Steps
1.  **Clone the Repository**
    ```bash
    git clone [https://github.com/yuliaVy/BlogWebsiteMefisto.git](https://github.com/yuliaVy/BlogWebsiteMefisto.git)
    ```
2.  **Open the Solution**
    * Navigate to the folder and open `Initilal_YV_Assesment2.sln`.
3.  **Database Setup**
    * Open **Package Manager Console** (`Tools` > `NuGet Package Manager` > `Package Manager Console`).
    * Run the command:
        ```powershell
        Update-Database
        ```
    * *Note: This will generate the local database and seed the default Admin/Staff roles.*
4.  **Run**
    * Press `F5` to launch.

## 🔐 Default Login (for Testing)
* **Admin Email:** `theBestAdminEver@mefistotheatre.com`
* **Password:** `admin123`
*(Check `DatabaseInitializer.cs` for specific initial credentials)*

## 🎓 Learning Outcomes
* Implementing **Identity** for authentication and complex authorization policies.
* Managing **Relational Data integrity** (e.g., ensuring comments are deleted if a post is deleted).
* Integrating third-party services (Google Maps).
* designing a **custom Admin Dashboard** for non-technical users.

## 📄 License
This project is for educational purposes.