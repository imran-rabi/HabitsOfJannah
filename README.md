Project Structure
Frontend (React with Vite):

Pages:
Login/Register
Dashboard (habit tracker)
Analytics/Progress
Components:
HabitCard (displays habit details, streak, and completion status)
AddHabitModal (to add/edit habits)
ReminderSetup (for setting habit reminders)
Backend (.NET Web API):

Features:
User Authentication (JWT)
Habit CRUD (Create, Read, Update, Delete)
Streak Calculation
Reminder Notifications (optional using a third-party service)
Database (SQL Server):

Tables:
Users: User information and credentials.
Habits: Habit details (name, frequency, reminders).
HabitProgress: Tracks daily progress for each habit.
Achievements: Badges and milestones.
Core Features
User Authentication:

Secure JWT-based login and registration.
Option to reset passwords using email verification.
Habit Tracking:

Add/Edit/Delete habits with details like:
Habit name (e.g., "Read Quran")
Frequency (daily, weekly, monthly)
Reminder time (optional)
Mark habits as complete for the day.
Streak System:

Track consecutive days a habit is completed.
Display streaks visually (e.g., “10-day streak!”).
Analytics Dashboard:

Visual representation of habit progress (graphs, charts).
Stats like “most completed habit” or “average weekly progress.”
Motivational Content:

Daily motivational Hadith, Quran verses, or quotes.
Display as a banner or notification on the dashboard.
Reminders:

Allow users to set reminders for specific habits.
Option to send reminders via email or push notifications.
Gamification:

Award badges for milestones (e.g., 30 days of praying Fajr on time).
Leaderboard for shared family or friends' progress (optional).
Tech Stack
Frontend:
React (for UI)
Vite (for fast development)
CSS/HTML (for styling)
Backend:
.NET Web API (for business logic and API endpoints)
Entity Framework Core (for database interaction)
Database:
SQL Server
Others:
JWT for authentication
Chart.js (or similar) for analytics
Email or push notification services for reminders
Example API Endpoints
User Authentication:

POST /api/auth/register: Register a new user.
POST /api/auth/login: Login and get JWT.
POST /api/auth/reset-password: Reset password.
Habits:

GET /api/habits: Get all habits for the logged-in user.
POST /api/habits: Create a new habit.
PUT /api/habits/{id}: Update a habit.
DELETE /api/habits/{id}: Delete a habit.
Habit Progress:

POST /api/habits/{id}/progress: Mark habit as completed for the day.
GET /api/habits/{id}/progress: Get progress history for a habit.


|-- IslamicHabitTracker/
    |-- Controllers/
    |-- Models/
    |-- Data/
    |-- Repositories/
    |-- Services/
    |-- DTOs/
    |-- Helpers/
    |-- Middleware/
    |-- Program.cs
    |-- appsettings.json



Folder Details
1. Controllers
Purpose: Handle HTTP requests and responses.
Files:
UsersController.cs
HabitsController.cs
HabitProgressController.cs
AchievementsController.cs

2. Models
Purpose: Define the entities that map to the database tables.
Files:
User.cs
Habit.cs
HabitProgress.cs
Achievement.cs

3. Data
Purpose: Manage the database context and migrations.
Files:
AppDbContext.cs (inherits from DbContext).
Migration files (generated using Add-Migration).

4. Repositories
Purpose: Encapsulate data access logic and interact with the database through AppDbContext.
Files:
IUserRepository.cs and UserRepository.cs
IHabitRepository.cs and HabitRepository.cs
IHabitProgressRepository.cs and HabitProgressRepository.cs

5. Services
Purpose: Contain the business logic of the application.
Files:
UserService.cs
HabitService.cs
HabitProgressService.cs
AchievementService.cs

6. DTOs (Data Transfer Objects)
Purpose: Define data contracts for client-server communication, avoiding exposing the database schema.
Files:
UserDTO.cs
HabitDTO.cs
CreateHabitDTO.cs
UpdateHabitDTO.cs

7. Helpers
Purpose: Contain utility classes for reusable logic like JWT generation, date utilities, or validation.
Files:
JwtHelper.cs
DateHelper.cs

8. Middleware
Purpose: Add custom middleware for tasks like error handling or logging.
Files:
ErrorHandlerMiddleware.cs

